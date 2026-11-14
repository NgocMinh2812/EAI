using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI;
using BUS.AI.Models;
using DTO.Assistant;

namespace BUS.Assistant
{
    /// <summary>
    /// Thao tác ghi dữ liệu đang chờ người dùng xem trước và bấm xác nhận.
    /// </summary>
    public class StagedWriteAction
    {
        public string Token { get; set; } = string.Empty;
        public string ToolName { get; set; } = string.Empty;
        public Dictionary<string, object?> Arguments { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        public AssistantContext Context { get; set; } = new();
        public int ConversationId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(15);
        public string? PreviewCardPayloadJson { get; set; }
    }

    /// <summary>
    /// Kết quả phản hồi của Trợ lý AI sau khi điều phối tin nhắn người dùng.
    /// </summary>
    public class AssistantResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string? CardType { get; set; }
        public string? CardPayloadJson { get; set; }
        public bool RequiresConfirmation { get; set; } = false;
        public string? ConfirmationToken { get; set; }
        public AssistantToolResultDTO? ToolResult { get; set; }
        public string? ExecutedToolName { get; set; }
        public string? ModelUsed { get; set; }
        public long LatencyMs { get; set; }
        public string? ErrorCode { get; set; }

        public static AssistantResponse Text(string message, string? model = null, long latencyMs = 0)
        {
            return new AssistantResponse
            {
                Success = true,
                Message = message,
                ModelUsed = model,
                LatencyMs = latencyMs
            };
        }

        public static AssistantResponse WithCard(
            string message, 
            string cardType, 
            string cardPayloadJson, 
            string? toolName = null, 
            AssistantToolResultDTO? toolResult = null, 
            string? model = null, 
            long latencyMs = 0)
        {
            return new AssistantResponse
            {
                Success = true,
                Message = message,
                CardType = cardType,
                CardPayloadJson = cardPayloadJson,
                ExecutedToolName = toolName,
                ToolResult = toolResult,
                ModelUsed = model,
                LatencyMs = latencyMs
            };
        }

        public static AssistantResponse ConfirmationRequired(
            string message, 
            string token, 
            string cardType, 
            string cardPayloadJson, 
            string toolName, 
            long latencyMs = 0)
        {
            return new AssistantResponse
            {
                Success = true,
                Message = message,
                RequiresConfirmation = true,
                ConfirmationToken = token,
                CardType = cardType,
                CardPayloadJson = cardPayloadJson,
                ExecutedToolName = toolName,
                LatencyMs = latencyMs
            };
        }

        public static AssistantResponse Error(string message, string? errorCode = "ERROR", string? model = null, long latencyMs = 0)
        {
            return new AssistantResponse
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                ModelUsed = model,
                LatencyMs = latencyMs
            };
        }
    }

    /// <summary>
    /// Bộ điều phối trung tâm (Orchestrator) của Trợ lý AI EAI.
    /// Chịu trách nhiệm:
    /// 1. Tạo Dynamic System Prompt theo vai trò và ngữ cảnh trang.
    /// 2. Điều phối tin nhắn người dùng và lưu trữ hội thoại.
    /// 3. Phân tích intent gọi công cụ từ phản hồi AI.
    /// 4. Kiểm tra quyền hạn nghiêm ngặt qua AssistantPermissionService.
    /// 5. Cơ chế xác nhận an toàn (Confirmation Token Flow) cho mọi thao tác ghi (Write).
    /// 6. Rào chắn an toàn (Safety Guardrails): Chống thực thi SQL thô và mã C# tùy ý.
    /// 7. Quản lý timeout và hủy tạo phản hồi (CancellationToken) với thông báo tiếng Việt thân thiện.
    /// </summary>
    public class AssistantOrchestrator
    {
        private static readonly Lazy<AssistantOrchestrator> _instance = new(() => new AssistantOrchestrator());
        public static AssistantOrchestrator Instance => _instance.Value;

        private readonly AssistantConversationService _conversationService;
        private readonly AssistantToolRegistry _toolRegistry;
        private readonly AssistantContextProvider _contextProvider;

        // Lưu trữ các hành động ghi đang chờ xác nhận an toàn (thread-safe)
        private readonly ConcurrentDictionary<string, StagedWriteAction> _stagedActions = new();

        public AssistantOrchestrator(
            AssistantConversationService? conversationService = null,
            AssistantToolRegistry? toolRegistry = null,
            AssistantContextProvider? contextProvider = null)
        {
            _conversationService = conversationService ?? AssistantConversationService.Instance;
            _toolRegistry = toolRegistry ?? AssistantToolRegistry.Instance;
            _contextProvider = contextProvider ?? AssistantContextProvider.Instance;
        }

        #region Public Orchestration API

        /// <summary>
        /// Xử lý tin nhắn người dùng theo luồng hội thoại hoàn chỉnh.
        /// </summary>
        public async Task<AssistantResponse> ProcessUserMessageAsync(
            int conversationId, 
            string userMessage, 
            AssistantContext? context = null, 
            CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                // Kiểm tra token hủy
                ct.ThrowIfCancellationRequested();

                // Xác định ngữ cảnh
                var effectiveContext = context ?? _contextProvider.GetCurrentContext();

                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    return AssistantResponse.Error("Vui lòng nhập nội dung câu hỏi hoặc yêu cầu.", "EMPTY_MESSAGE");
                }

                string trimmedInput = userMessage.Trim();

                // 1. Kiểm tra Rào chắn An toàn (Safety Guardrails)
                if (CheckSafetyGuardrailsViolation(trimmedInput, out string safetyWarning))
                {
                    sw.Stop();
                    string safeRejectMessage = $"[Cảnh báo an toàn hệ thống]: {safetyWarning}";

                    if (conversationId > 0)
                    {
                        await _conversationService.AddMessageAsync(conversationId, "user", trimmedInput);
                        await _conversationService.AddMessageAsync(conversationId, "assistant", safeRejectMessage, status: "Error");
                    }

                    return AssistantResponse.Error(safeRejectMessage, "SECURITY_VIOLATION", null, sw.ElapsedMilliseconds);
                }

                // 2. Lưu tin nhắn người dùng vào CSDL nếu có conversationId
                if (conversationId > 0)
                {
                    await _conversationService.AddMessageAsync(conversationId, "user", trimmedInput);
                }

                // 3. Xây dựng System Prompt động
                string systemPrompt = BuildDynamicSystemPrompt(effectiveContext);

                // 4. Lấy lịch sử hội thoại gần nhất để duy trì ngữ cảnh đa lượt
                string conversationHistory = string.Empty;
                if (conversationId > 0)
                {
                    conversationHistory = await BuildConversationHistoryPromptAsync(conversationId, limit: 6);
                }

                // 5. Tổng hợp Prompt hoàn chỉnh gửi cho AI
                string fullPrompt = $"{systemPrompt}\n\n{conversationHistory}\n\n[TIN NHẮN NGƯỜI DÙNG HIỆN TẠI]:\n{trimmedInput}";

                // 6. Gọi mô hình AI qua AIServiceManager
                AIResponse aiResponse;
                try
                {
                    aiResponse = await AIServiceManager.Instance.ExecutePromptAsync(
                        fullPrompt, 
                        actionName: "Trợ lý AI EAI", 
                        ct: ct);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    string friendlyError = MapExceptionToFriendlyMessage(ex);
                    if (conversationId > 0)
                    {
                        await _conversationService.AddMessageAsync(conversationId, "assistant", friendlyError, status: "Error");
                    }
                    return AssistantResponse.Error(friendlyError, "AI_CALL_FAILED", null, sw.ElapsedMilliseconds);
                }

                if (!aiResponse.Success)
                {
                    sw.Stop();
                    string friendlyError = MapStatusCodeToFriendlyMessage(aiResponse.StatusCode, aiResponse.ErrorMessage);
                    if (conversationId > 0)
                    {
                        await _conversationService.AddMessageAsync(conversationId, "assistant", friendlyError, status: "Error");
                    }
                    return AssistantResponse.Error(friendlyError, "AI_PROVIDER_ERROR", aiResponse.ModelUsed, sw.ElapsedMilliseconds);
                }

                string responseText = aiResponse.Text ?? string.Empty;

                // 7. Phân tích xem AI có trả về yêu cầu gọi Tool hay không
                if (TryParseToolCall(responseText, out string toolName, out Dictionary<string, object?> toolArgs, out string? plainTextPreamble))
                {
                    // Tìm công cụ trong sổ đăng ký
                    var tool = _toolRegistry.GetTool(toolName);
                    if (tool == null)
                    {
                        // Model gọi một tool không tồn tại
                        string missingMsg = $"Tôi hiểu bạn muốn thực hiện thao tác '{toolName}', nhưng công cụ này chưa được kích hoạt trong hệ thống EAI.";
                        if (conversationId > 0)
                        {
                            await _conversationService.AddMessageAsync(conversationId, "assistant", missingMsg);
                        }
                        sw.Stop();
                        return AssistantResponse.Text(missingMsg, aiResponse.ModelUsed, sw.ElapsedMilliseconds);
                    }

                    // 8. Kiểm tra phân quyền thực thi công cụ
                    if (!AssistantPermissionService.CanExecute(tool, effectiveContext, out string permissionDeniedReason))
                    {
                        sw.Stop();
                        if (conversationId > 0)
                        {
                            await _conversationService.AddMessageAsync(conversationId, "assistant", permissionDeniedReason, status: "Forbidden");
                        }
                        return AssistantResponse.Error(permissionDeniedReason, "FORBIDDEN", aiResponse.ModelUsed, sw.ElapsedMilliseconds);
                    }

                    // 9. Kiểm tra xem các đối số của Tool có vi phạm SQL Injection hay không
                    if (CheckArgumentsSafetyViolation(toolArgs, out string argViolationReason))
                    {
                        sw.Stop();
                        string argRejectMessage = $"[Cảnh báo an toàn tham số]: {argViolationReason}";
                        if (conversationId > 0)
                        {
                            await _conversationService.AddMessageAsync(conversationId, "assistant", argRejectMessage, status: "Error");
                        }
                        return AssistantResponse.Error(argRejectMessage, "SECURITY_VIOLATION", aiResponse.ModelUsed, sw.ElapsedMilliseconds);
                    }

                    // 10. Luồng Xác Nhận An Toàn (Confirmation Flow) cho Thao Tác Ghi
                    if (tool.IsWriteOperation && tool.RequiresConfirmation)
                    {
                        string token = Guid.NewGuid().ToString("N");
                        string previewPayloadJson = GeneratePreviewCardPayloadJson(tool, toolArgs, token);

                        var stagedAction = new StagedWriteAction
                        {
                            Token = token,
                            ToolName = tool.Name,
                            Arguments = toolArgs,
                            Context = effectiveContext,
                            ConversationId = conversationId,
                            CreatedAt = DateTime.Now,
                            ExpiresAt = DateTime.Now.AddMinutes(15),
                            PreviewCardPayloadJson = previewPayloadJson
                        };

                        _stagedActions[token] = stagedAction;

                        string confirmPromptMsg = !string.IsNullOrWhiteSpace(plainTextPreamble) 
                            ? plainTextPreamble 
                            : $"Đã chuẩn bị nội dung cho thao tác **{tool.Name}** ({tool.Description}). Vui lòng xem trước thông tin bên dưới và bấm **Xác nhận lưu** để ghi vào CSDL, hoặc **Hủy bỏ**.";

                        if (conversationId > 0)
                        {
                            await _conversationService.AddMessageAsync(
                                conversationId, 
                                "assistant", 
                                confirmPromptMsg, 
                                toolCallsJson: JsonSerializer.Serialize(new { tool = tool.Name, arguments = toolArgs }),
                                cardPayloadJson: previewPayloadJson);
                        }

                        sw.Stop();
                        return AssistantResponse.ConfirmationRequired(
                            confirmPromptMsg, 
                            token, 
                            "preview_card", 
                            previewPayloadJson, 
                            tool.Name, 
                            sw.ElapsedMilliseconds);
                    }

                    // 11. Thực thi ngay đối với Thao Tác Đọc (Read Operation)
                    var toolResult = await tool.ExecuteAsync(toolArgs, effectiveContext, ct);
                    sw.Stop();

                    string assistantMsg = !string.IsNullOrWhiteSpace(plainTextPreamble)
                        ? $"{plainTextPreamble}\n\n{toolResult.Message}"
                        : toolResult.Message;

                    if (conversationId > 0)
                    {
                        await _conversationService.AddMessageAsync(
                            conversationId, 
                            "assistant", 
                            assistantMsg,
                            toolCallsJson: JsonSerializer.Serialize(new { tool = tool.Name, arguments = toolArgs }),
                            toolResultJson: JsonSerializer.Serialize(toolResult),
                            cardPayloadJson: toolResult.CardPayloadJson);
                    }

                    if (!string.IsNullOrWhiteSpace(toolResult.CardType) && toolResult.CardType != "none")
                    {
                        return AssistantResponse.WithCard(
                            assistantMsg, 
                            toolResult.CardType, 
                            toolResult.CardPayloadJson ?? string.Empty, 
                            tool.Name, 
                            toolResult.ToDTO(), 
                            aiResponse.ModelUsed, 
                            sw.ElapsedMilliseconds);
                    }

                    return AssistantResponse.Text(assistantMsg, aiResponse.ModelUsed, sw.ElapsedMilliseconds);
                }

                // 12. Phản hồi văn bản thông thường (không gọi tool)
                sw.Stop();
                if (conversationId > 0)
                {
                    await _conversationService.AddMessageAsync(conversationId, "assistant", responseText);
                }

                return AssistantResponse.Text(responseText, aiResponse.ModelUsed, sw.ElapsedMilliseconds);
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                string canceledMsg = "Yêu cầu đã được dừng bởi người dùng.";
                if (conversationId > 0)
                {
                    try { await _conversationService.AddMessageAsync(conversationId, "assistant", canceledMsg, status: "Canceled"); } catch { }
                }
                return AssistantResponse.Error(canceledMsg, "CANCELED", null, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                string errorMsg = MapExceptionToFriendlyMessage(ex);
                if (conversationId > 0)
                {
                    try { await _conversationService.AddMessageAsync(conversationId, "assistant", errorMsg, status: "Error"); } catch { }
                }
                return AssistantResponse.Error(errorMsg, "UNHANDLED_ERROR", null, sw.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Đưa một hành động ghi vào hàng đợi chờ xác nhận hai bước (dành cho API điều phối và Unit Test).
        /// </summary>
        public string StageWriteAction(string toolName, Dictionary<string, object?> arguments, AssistantContext context, int conversationId = 0, string? previewPayloadJson = null)
        {
            string token = Guid.NewGuid().ToString("N");
            var staged = new StagedWriteAction
            {
                Token = token,
                ToolName = toolName,
                Arguments = arguments,
                Context = context,
                ConversationId = conversationId,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMinutes(15),
                PreviewCardPayloadJson = previewPayloadJson
            };
            _stagedActions[token] = staged;
            return token;
        }

        /// <summary>
        /// Xác nhận và thực thi thao tác ghi dữ liệu đã được xem trước qua mã ConfirmationToken.
        /// </summary>
        public async Task<AssistantToolResult> ConfirmAndExecuteWriteToolAsync(
            string confirmationToken, 
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(confirmationToken))
            {
                return AssistantToolResult.Fail("Mã xác nhận (ConfirmationToken) không được để trống.", "INVALID_TOKEN");
            }

            string cleanToken = confirmationToken.Trim();

            // Lấy và xóa token (chỉ cho phép sử dụng 1 lần duy nhất)
            if (!_stagedActions.TryRemove(cleanToken, out var staged))
            {
                return AssistantToolResult.Fail("Mã xác nhận không tồn tại hoặc đã được thực thi trước đó.", "TOKEN_NOT_FOUND");
            }

            // Kiểm tra thời hạn hiệu lực (15 phút)
            if (staged.ExpiresAt < DateTime.Now)
            {
                return AssistantToolResult.Fail("Mã xác nhận đã hết hạn hiệu lực (quá 15 phút). Vui lòng thực hiện lại yêu cầu.", "TOKEN_EXPIRED");
            }

            var tool = _toolRegistry.GetTool(staged.ToolName);
            if (tool == null)
            {
                return AssistantToolResult.Fail($"Công cụ '{staged.ToolName}' không còn tồn tại trong hệ thống.", "TOOL_NOT_FOUND");
            }

            // Tái thẩm định quyền hạn người dùng tại thời điểm bấm xác nhận
            if (!AssistantPermissionService.CanExecute(tool, staged.Context, out string reason))
            {
                return AssistantToolResult.Fail($"Từ chối xác nhận: {reason}", "FORBIDDEN");
            }

            // Thực thi thao tác ghi chính thức
            try
            {
                var result = await tool.ExecuteAsync(staged.Arguments, staged.Context, ct);

                // Ghi nhận phản hồi thành công vào lịch sử hội thoại nếu có conversationId
                if (staged.ConversationId > 0)
                {
                    string successSummary = $"[Đã xác nhận thực thi công cụ '{tool.Name}']:\n{result.Message}";
                    await _conversationService.AddMessageAsync(
                        staged.ConversationId, 
                        "assistant", 
                        successSummary, 
                        toolCallsJson: JsonSerializer.Serialize(new { tool = staged.ToolName, arguments = staged.Arguments, confirmed = true }),
                        toolResultJson: JsonSerializer.Serialize(result),
                        cardPayloadJson: result.CardPayloadJson);
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                return AssistantToolResult.Fail("Thao tác thực thi đã bị hủy.", "CANCELED");
            }
            catch (Exception ex)
            {
                return AssistantToolResult.Fail($"Lỗi trong quá trình ghi dữ liệu: {ex.Message}", "EXECUTION_FAILED");
            }
        }

        /// <summary>
        /// Hủy bỏ một thao tác ghi đang chờ xác nhận qua mã ConfirmationToken.
        /// </summary>
        public bool CancelAction(string confirmationToken)
        {
            if (string.IsNullOrWhiteSpace(confirmationToken)) return false;
            return _stagedActions.TryRemove(confirmationToken.Trim(), out _);
        }

        /// <summary>
        /// Xác nhận và thực thi thao tác ghi dữ liệu đã được xem trước qua mã ConfirmationToken.
        /// </summary>
        public async Task<AssistantToolResult> ConfirmActionAsync(string confirmationToken, CancellationToken ct = default)
        {
            return await ConfirmAndExecuteWriteToolAsync(confirmationToken, ct);
        }

        #endregion

        #region Prompt Generation & Parsing Helpers

        /// <summary>
        /// Tạo dynamic system prompt hoàn chỉnh chứa đầy đủ ngữ cảnh phiên làm việc và danh mục schema công cụ.
        /// </summary>
        public string BuildDynamicSystemPrompt(AssistantContext context)
        {
            string contextBlock = _contextProvider.BuildSystemPromptContext(context);
            string toolSchemaBlock = _toolRegistry.BuildToolDeclarationsPrompt(context.UserRole);

            return $@"Bạn là Trợ lý AI EAI (EnglishAI Assistant), trợ lý ảo thông minh tích hợp trực tiếp trong ứng dụng học và luyện thi Tiếng Anh EAI.
Mục tiêu của bạn là đồng hành, giải đáp thắc mắc, hỗ trợ học tập, tra cứu thông tin và tự động hóa các tác vụ quản trị đào tạo theo đúng thẩm quyền.

{contextBlock}

[QUY TẮC PHẢN HỒI]:
1. Luôn phản hồi lịch sự, tôn trọng, giải thích rõ ràng và tự nhiên bằng tiếng Việt chuẩn (trừ khi hướng dẫn ngữ pháp, từ vựng hoặc phát âm tiếng Anh).
2. TUYỆT ĐỐI TUÂN THỦ PHÂN QUYỀN: Người dùng chỉ được phép truy cập các công cụ được liệt kê trong danh mục bên dưới. Đừng bao giờ bịa đặt công cụ hoặc gợi ý hành động vượt thẩm quyền.
3. TUYỆT ĐỐI KHÔNG SINH HOẶC THỰC THI LỆNH SQL THÔ (Raw SQL), không sinh script PowerShell, cmd hoặc mã thực thi C# tùy ý. Mọi truy vấn dữ liệu chỉ được thông qua danh mục công cụ an toàn đã đăng ký.
4. KHI CẦN GỌI CÔNG CỤ (FUNCTION CALLING):
   - Bạn CHỈ ĐƯỢC CHỌN từ danh mục công cụ khả dụng bên dưới.
   - Trả về DUY NHẤT một khối JSON theo cú pháp chuẩn sau (có thể kèm câu thông báo ngắn phía trước):
```json
{{
  ""tool"": ""<tên_công_cụ>"",
  ""arguments"": {{
    ""<tên_tham_số>"": ""<giá_trị>""
  }}
}}
```
5. KHI KHÔNG CẦN GỌI CÔNG CỤ:
   - Hãy trả lời trực tiếp câu hỏi của người dùng bằng định dạng Markdown đẹp mắt (sử dụng bullet points, in đậm từ khóa quan trọng).

[DANH MỤC CÔNG CỤ KHẢ DỤNG CHO VAI TRÒ HIỆN TẠI]:
{toolSchemaBlock}";
        }

        /// <summary>
        /// Trích xuất lịch sử các tin nhắn gần nhất để đưa vào ngữ cảnh AI.
        /// </summary>
        private async Task<string> BuildConversationHistoryPromptAsync(int conversationId, int limit = 6)
        {
            try
            {
                var messages = await _conversationService.GetMessagesAsync(conversationId);
                if (messages == null || messages.Count == 0) return string.Empty;

                var recent = messages.TakeLast(limit).ToList();
                var lines = new List<string> { "[LỊCH SỬ HỘI THOẠI GẦN ĐÂY]:" };

                foreach (var msg in recent)
                {
                    string sender = msg.IsUser ? "Người dùng" : "Trợ lý AI";
                    string snippet = msg.Content.Length > 200 ? msg.Content.Substring(0, 197) + "..." : msg.Content;
                    lines.Add($"- {sender}: {snippet}");
                }

                return string.Join(Environment.NewLine, lines);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Phân tích phản hồi từ LLM để nhận diện lệnh gọi công cụ có cấu trúc JSON.
        /// </summary>
        public static bool TryParseToolCall(
            string responseText, 
            out string toolName, 
            out Dictionary<string, object?> arguments, 
            out string? plainTextPreamble)
        {
            toolName = string.Empty;
            arguments = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            plainTextPreamble = null;

            if (string.IsNullOrWhiteSpace(responseText)) return false;

            string text = responseText.Trim();

            // Tìm khối JSON trong cặp dấu ```json ... ``` hoặc ``` ... ```
            string jsonCandidate = string.Empty;
            var codeBlockMatch = Regex.Match(text, @"```(?:json)?\s*(\{[\s\S]*?\})\s*```", RegexOptions.IgnoreCase);
            if (codeBlockMatch.Success)
            {
                jsonCandidate = codeBlockMatch.Groups[1].Value;
                int idx = text.IndexOf("```");
                if (idx > 0)
                {
                    plainTextPreamble = text.Substring(0, idx).Trim();
                }
            }
            else if (text.StartsWith("{") && text.EndsWith("}"))
            {
                jsonCandidate = text;
            }
            else
            {
                // Thử tìm khối JSON đầu tiên bắt đầu bằng { và kết thúc bằng }
                int firstBrace = text.IndexOf('{');
                int lastBrace = text.LastIndexOf('}');
                if (firstBrace >= 0 && lastBrace > firstBrace)
                {
                    jsonCandidate = text.Substring(firstBrace, lastBrace - firstBrace + 1);
                    if (firstBrace > 0)
                    {
                        plainTextPreamble = text.Substring(0, firstBrace).Trim();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(jsonCandidate)) return false;

            try
            {
                using var doc = JsonDocument.Parse(jsonCandidate);
                var root = doc.RootElement;

                if (root.ValueKind != JsonValueKind.Object) return false;

                // Tìm tên công cụ từ property "tool", "name", hoặc "tool_name"
                string extractedTool = string.Empty;
                if (root.TryGetProperty("tool", out var toolProp) && toolProp.ValueKind == JsonValueKind.String)
                {
                    extractedTool = toolProp.GetString() ?? string.Empty;
                }
                else if (root.TryGetProperty("name", out var nameProp) && nameProp.ValueKind == JsonValueKind.String)
                {
                    extractedTool = nameProp.GetString() ?? string.Empty;
                }
                else if (root.TryGetProperty("tool_name", out var toolNameProp) && toolNameProp.ValueKind == JsonValueKind.String)
                {
                    extractedTool = toolNameProp.GetString() ?? string.Empty;
                }

                if (string.IsNullOrWhiteSpace(extractedTool)) return false;

                toolName = extractedTool.Trim();

                // Tìm arguments từ property "arguments", "parameters", hoặc "args"
                JsonElement argsElement = default;
                if (root.TryGetProperty("arguments", out var aProp) && aProp.ValueKind == JsonValueKind.Object)
                {
                    argsElement = aProp;
                }
                else if (root.TryGetProperty("parameters", out var pProp) && pProp.ValueKind == JsonValueKind.Object)
                {
                    argsElement = pProp;
                }
                else if (root.TryGetProperty("args", out var argsProp) && argsProp.ValueKind == JsonValueKind.Object)
                {
                    argsElement = argsProp;
                }

                if (argsElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var prop in argsElement.EnumerateObject())
                    {
                        arguments[prop.Name] = ExtractJsonValue(prop.Value);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static object? ExtractJsonValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt32(out int i) ? i : (element.TryGetInt64(out long l) ? l : element.GetDouble()),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.GetRawText()
            };
        }

        private static string GeneratePreviewCardPayloadJson(IAssistantTool tool, Dictionary<string, object?> arguments, string token)
        {
            var previewObj = new
            {
                tool_name = tool.Name,
                tool_description = tool.Description,
                category = tool.Category,
                is_write = tool.IsWriteOperation,
                confirmation_token = token,
                arguments = arguments,
                staged_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return JsonSerializer.Serialize(previewObj, new JsonSerializerOptions { WriteIndented = true });
        }

        #endregion

        #region Safety Guardrails

        /// <summary>
        /// Rào chắn an toàn kiểm tra phát hiện hành vi cố ý tiêm lệnh SQL thô hoặc mã nguồn C# tùy ý.
        /// </summary>
        public static bool CheckSafetyGuardrailsViolation(string input, out string warning)
        {
            warning = string.Empty;
            if (string.IsNullOrWhiteSpace(input)) return false;

            string normalized = input.ToUpperInvariant();

            // Rào chắn SQL Injection nguy hiểm
            string[] dangerousSqlPatterns = new[]
            {
                @"DROP\s+TABLE",
                @"ALTER\s+TABLE",
                @"TRUNCATE\s+TABLE",
                @"DELETE\s+FROM",
                @"INSERT\s+INTO\s+.*VALUES",
                @"UNION\s+ALL\s+SELECT",
                @"UNION\s+SELECT",
                @"SELECT\s+.*\s+FROM\s+SYS\.",
                @"EXEC\s+XP_",
                @"EXEC\s+SP_",
                @"SHUTDOWN",
                @";\s*DROP",
                @";\s*DELETE"
            };

            foreach (var pattern in dangerousSqlPatterns)
            {
                if (Regex.IsMatch(normalized, pattern, RegexOptions.IgnoreCase))
                {
                    warning = "Phát hiện câu lệnh SQL trực tiếp. Vì lý do an toàn dữ liệu, Trợ lý AI EAI tuyệt đối không được phép thực thi SQL tùy ý.";
                    return true;
                }
            }

            // Rào chắn thực thi mã C# / OS command tùy ý
            string[] dangerousCodePatterns = new[]
            {
                @"PROCESS\.START",
                @"SYSTEM\.DIAGNOSTICS",
                @"CMD\.EXE",
                @"POWERSHELL",
                @"ASSEMBLY\.LOAD",
                @"EVAL\(",
                @"ACTIVATOR\.CREATEINSTANCE"
            };

            foreach (var pattern in dangerousCodePatterns)
            {
                if (Regex.IsMatch(normalized, pattern, RegexOptions.IgnoreCase))
                {
                    warning = "Phát hiện yêu cầu gọi tiến trình hệ thống hoặc thực thi mã C# tùy ý. Hành động này bị nghiêm cấm.";
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra rào chắn an toàn cho các tham số được truyền vào Tool.
        /// </summary>
        private static bool CheckArgumentsSafetyViolation(Dictionary<string, object?> arguments, out string reason)
        {
            reason = string.Empty;
            if (arguments == null) return false;

            foreach (var kvp in arguments)
            {
                if (kvp.Value is string strVal)
                {
                    if (CheckSafetyGuardrailsViolation(strVal, out reason))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Friendly Error Mapping (Vietnamese)

        private static string MapStatusCodeToFriendlyMessage(int statusCode, string? originalError)
        {
            return statusCode switch
            {
                429 => "Nhà cung cấp AI hiện đang quá tải hoặc đã chạm ngưỡng giới hạn (Rate Limit / Quota). Vui lòng đợi trong giây lát rồi thử lại, hoặc chuyển sang mô hình khác.",
                401 or 403 => "Khóa API của nhà cung cấp AI không hợp lệ hoặc chưa được cấp quyền. Vui lòng kiểm tra lại cấu hình API trong phần Giám sát hệ thống.",
                404 => "Mô hình AI được yêu cầu không tìm thấy hoặc đã ngừng cung cấp dịch vụ.",
                408 => "Quá trình xử lý yêu cầu AI bị quá thời gian (Timeout). Vui lòng kiểm tra kết nối mạng hoặc thử lại với câu hỏi ngắn gọn hơn.",
                >= 500 => "Máy chủ dịch vụ AI đang gặp sự cố tạm thời. Hệ thống sẽ tự động chuyển sang mô hình dự phòng nếu được kích hoạt.",
                _ => !string.IsNullOrWhiteSpace(originalError) 
                    ? $"Không thể kết nối đến Trợ lý AI: {originalError}" 
                    : "Mạng bận hoặc tạm thời không thể kết nối đến Trợ lý AI, vui lòng thử lại sau giây lát."
            };
        }

        private static string MapExceptionToFriendlyMessage(Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                return "Yêu cầu đã được dừng bởi người dùng.";
            }

            string msg = ex.Message;
            if (msg.Contains("429") || msg.Contains("quota", StringComparison.OrdinalIgnoreCase))
            {
                return "Nhà cung cấp AI hiện đang quá tải hoặc đã hết hạn mức (Quota). Vui lòng thử lại sau giây lát.";
            }
            if (msg.Contains("timed out", StringComparison.OrdinalIgnoreCase) || msg.Contains("timeout", StringComparison.OrdinalIgnoreCase))
            {
                return "Thời gian phản hồi của mô hình AI quá lâu. Vui lòng thử lại hoặc chọn mô hình có tốc độ phản hồi nhanh hơn.";
            }

            return $"Gặp sự cố khi kết nối Trợ lý AI: {msg}. Vui lòng thử lại sau.";
        }

        #endregion
    }
}
