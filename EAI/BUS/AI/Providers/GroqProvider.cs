using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Configuration;
using BUS.AI.Models;

namespace BUS.AI.Providers
{
    public class GroqProvider : IAIProvider
    {
        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(25) };
        private const string GroqBaseUrl = "https://api.groq.com/openai/v1";

        public string ProviderName => "Groq Cloud";
        public AIProviderType ProviderType => AIProviderType.Groq;

        public bool IsConfigured => !string.IsNullOrWhiteSpace(AIConfigHelper.GetGroqKey());

        public IReadOnlyList<string> SupportedModels => new[]
        {
            "qwen/qwen3.8-27b",
            "openai/gpt-oss-120b",
            "openai/gpt-oss-20b",
            "allam-2-7b",
            "llama-3.3-70b-versatile"
        };

        public string DefaultModel => "qwen/qwen3.8-27b";

        public async Task<AIResponse> GenerateContentAsync(string prompt, string? model = null, CancellationToken ct = default)
        {
            string apiKey = AIConfigHelper.GetGroqKey();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return AIResponse.Fail(ProviderName, "Chưa cấu hình Groq API Key (Vui lòng thiết lập biến GROQ_API_KEY hoặc trong secrets.json)", 401);
            }

            string targetModel = string.IsNullOrWhiteSpace(model) ? DefaultModel : model;
            string endpoint = $"{GroqBaseUrl}/chat/completions";

            object requestObj;
            if (targetModel.Contains("qwen", StringComparison.OrdinalIgnoreCase))
            {
                requestObj = new
                {
                    model = targetModel,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.4,
                    max_tokens = 3500
                };
            }
            else
            {
                requestObj = new
                {
                    model = targetModel,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.4,
                    max_tokens = 3500
                };
            }

            string requestJson = JsonSerializer.Serialize(requestObj);
            var sw = Stopwatch.StartNew();

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(45));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);

                var response = await _httpClient.SendAsync(request, linkedCts.Token);
                sw.Stop();

                string responseBody = await response.Content.ReadAsStringAsync(ct);
                int statusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(responseBody);
                    var choices = doc.RootElement.GetProperty("choices");
                    if (choices.GetArrayLength() > 0)
                    {
                        string text = choices[0].GetProperty("message").GetProperty("content").GetString() ?? "";
                        return AIResponse.Ok(text, ProviderName, targetModel, sw.ElapsedMilliseconds, statusCode);
                    }
                }

                // Phân loại mã lỗi rõ ràng theo tiêu chuẩn API
                string errorDetail = statusCode switch
                {
                    401 => "API Key Groq không hợp lệ hoặc đã hết hạn (HTTP 401)",
                    403 => "Không có quyền truy cập tài nguyên Groq (HTTP 403)",
                    404 => $"Mô hình Groq '{targetModel}' không tồn tại hoặc sai endpoint (HTTP 404)",
                    429 => "Đã vượt quá hạn mức yêu cầu Groq (Rate limit/Quota exceeded - HTTP 429)",
                    >= 500 => $"Máy chủ Groq đang gặp sự cố nội bộ (HTTP {statusCode})",
                    _ => $"Groq trả về HTTP {statusCode}: {responseBody}"
                };

                return AIResponse.Fail(ProviderName, errorDetail, statusCode, sw.ElapsedMilliseconds);
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                return AIResponse.Fail(ProviderName, "Yêu cầu tới Groq API bị timeout (quá 15s).", 408, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                return AIResponse.Fail(ProviderName, $"Lỗi kết nối Groq: {ex.Message}", 500, sw.ElapsedMilliseconds);
            }
        }

        public async Task<AIHealthStatus> CheckHealthAsync(CancellationToken ct = default)
        {
            string apiKey = AIConfigHelper.GetGroqKey();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new AIHealthStatus
                {
                    ProviderName = ProviderName,
                    ProviderType = ProviderType,
                    IsConfigured = false,
                    Health = "Unavailable",
                    Message = "Chưa cấu hình API Key (GROQ_API_KEY)",
                    ActiveModel = DefaultModel,
                    LastChecked = DateTime.Now
                };
            }

            var sw = Stopwatch.StartNew();
            try
            {
                // Gọi endpoint GET /models chính thức của Groq (chuẩn OpenAI)
                string endpoint = $"{GroqBaseUrl}/models";
                using var req = new HttpRequestMessage(HttpMethod.Get, endpoint);
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(6));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

                var res = await _httpClient.SendAsync(req, linkedCts.Token);
                sw.Stop();

                int statusCode = (int)res.StatusCode;
                if (res.IsSuccessStatusCode)
                {
                    return new AIHealthStatus
                    {
                        ProviderName = ProviderName,
                        ProviderType = ProviderType,
                        IsConfigured = true,
                        Health = "Healthy",
                        LatencyMs = sw.ElapsedMilliseconds,
                        Message = $"Kết nối ổn định ({sw.ElapsedMilliseconds}ms)",
                        ActiveModel = DefaultModel,
                        LastChecked = DateTime.Now
                    };
                }

                // Phân biệt trạng thái lỗi chi tiết
                var (healthLevel, msg) = statusCode switch
                {
                    401 => ("Unavailable", "API Key Groq không hợp lệ (HTTP 401 Unauthorized)"),
                    403 => ("Unavailable", "Không có quyền truy cập Groq (HTTP 403 Forbidden)"),
                    404 => ("Unavailable", "Endpoint Groq không tìm thấy (HTTP 404 Not Found)"),
                    429 => ("Degraded", "Vượt quá giới hạn hạn mức Groq Quota (HTTP 429)"),
                    >= 500 => ("Unavailable", $"Máy chủ Groq gặp lỗi nội bộ (HTTP {statusCode})"),
                    _ => ("Unavailable", $"Lỗi HTTP {statusCode} ({res.ReasonPhrase})")
                };

                return new AIHealthStatus
                {
                    ProviderName = ProviderName,
                    ProviderType = ProviderType,
                    IsConfigured = true,
                    Health = healthLevel,
                    LatencyMs = sw.ElapsedMilliseconds,
                    Message = msg,
                    ActiveModel = DefaultModel,
                    LastChecked = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                sw.Stop();
                return new AIHealthStatus
                {
                    ProviderName = ProviderName,
                    ProviderType = ProviderType,
                    IsConfigured = true,
                    Health = "Unavailable",
                    LatencyMs = sw.ElapsedMilliseconds,
                    Message = $"Lỗi kết nối: {ex.Message}",
                    ActiveModel = DefaultModel,
                    LastChecked = DateTime.Now
                };
            }
        }

        private static List<AIModelInfo>? _cachedModels;
        private static DateTime _lastCacheTime = DateTime.MinValue;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public bool IsCatalogDynamic { get; private set; } = false;
        public string CatalogStatusMessage { get; private set; } = "Chưa tải danh mục";

        public async Task<IReadOnlyList<AIModelInfo>> GetModelsAsync(bool forceRefresh = false, CancellationToken ct = default)
        {
            if (!forceRefresh && _cachedModels != null && (DateTime.Now - _lastCacheTime) < CacheDuration)
            {
                return _cachedModels;
            }

            string apiKey = AIConfigHelper.GetGroqKey();
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                try
                {
                    string endpoint = $"{GroqBaseUrl}/models";
                    using var req = new HttpRequestMessage(HttpMethod.Get, endpoint);
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(6));
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

                    var res = await _httpClient.SendAsync(req, linkedCts.Token);
                    if (res.IsSuccessStatusCode)
                    {
                        string body = await res.Content.ReadAsStringAsync(ct);
                        using var doc = JsonDocument.Parse(body);
                        if (doc.RootElement.TryGetProperty("data", out var dataArr) && dataArr.ValueKind == JsonValueKind.Array)
                        {
                            var dynamicList = new List<AIModelInfo>();
                            foreach (var item in dataArr.EnumerateArray())
                            {
                                string id = item.TryGetProperty("id", out var idProp) ? (idProp.GetString() ?? "") : "";
                                if (string.IsNullOrWhiteSpace(id)) continue;

                                string idLower = id.ToLowerInvariant();
                                // Lọc bỏ các mô hình không phục vụ chat/text generation (whisper, audio, guard, moderation, orpheus, canopylabs, tts, voice)
                                if (idLower.Contains("whisper") || idLower.Contains("audio") || idLower.Contains("guard") || 
                                    idLower.Contains("moderation") || idLower.Contains("orpheus") || idLower.Contains("canopylabs") ||
                                    idLower.Contains("tts") || idLower.Contains("voice"))
                                    continue;

                                string ownedBy = item.TryGetProperty("owned_by", out var obProp) ? (obProp.GetString() ?? "") : "groq";
                                int? ctx = item.TryGetProperty("context_window", out var ctxProp) ? ctxProp.GetInt32() : (int?)null;
                                bool isDefault = (id == DefaultModel) || (id == "qwen/qwen3.8-27b");

                                dynamicList.Add(new AIModelInfo
                                {
                                    Id = id,
                                    DisplayName = FormatGroqDisplayName(id),
                                    ProviderName = ProviderName,
                                    ProviderType = ProviderType,
                                    OwnedBy = ownedBy,
                                    AccessTier = "standard",
                                    Modality = "chat",
                                    ContextLength = ctx,
                                    IsDynamic = true,
                                    DefaultModel = isDefault
                                });
                            }

                            if (dynamicList.Count > 0)
                            {
                                // Sắp xếp mô hình mặc định (Khuyên dùng) lên đầu tiên
                                _cachedModels = dynamicList
                                    .OrderByDescending(x => x.DefaultModel)
                                    .ThenBy(x => x.DisplayName)
                                    .ToList();
                                _lastCacheTime = DateTime.Now;
                                IsCatalogDynamic = true;
                                CatalogStatusMessage = $"Đồng bộ trực tiếp từ Groq API ({_cachedModels.Count} mô hình chat)";
                                return _cachedModels;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GroqProvider.GetModelsAsync] Không thể lấy catalog động: {ex.Message}");
                }
            }

            // Fallback danh sách cấu hình sẵn đã xác minh chính thức từ Groq Console Docs
            var fallback = GetFallbackModels();
            _cachedModels = fallback;
            _lastCacheTime = DateTime.Now;
            IsCatalogDynamic = false;
            CatalogStatusMessage = string.IsNullOrWhiteSpace(apiKey)
                ? "Chưa có API Key — Dùng danh mục cấu hình sẵn đã xác minh từ Groq Docs"
                : "Không thể kết nối API catalog — Dùng danh mục cấu hình sẵn đã xác minh từ Groq Docs";
            return fallback;
        }

        private static string FormatGroqDisplayName(string modelId)
        {
            return modelId switch
            {
                "qwen/qwen3.8-27b" => "Qwen 3.8 27B (Khuyên dùng - Siêu tốc)",
                "openai/gpt-oss-120b" => "OpenAI GPT-OSS 120B (Lập luận sâu)",
                "openai/gpt-oss-20b" => "OpenAI GPT-OSS 20B (Nhanh & Ổn định)",
                "allam-2-7b" => "Allam 2 7B (Đa ngôn ngữ)",
                "llama-3.3-70b-versatile" => "Meta Llama 3.3 70B Versatile",
                "llama-3.1-8b-instant" => "Meta Llama 3.1 8B Instant",
                "gemma2-9b-it" => "Google Gemma 2 9B IT",
                "deepseek-r1-distill-llama-70b" => "DeepSeek R1 Distill Llama 70B",
                _ => modelId
            };
        }

        private List<AIModelInfo> GetFallbackModels()
        {
            return new List<AIModelInfo>
            {
                new AIModelInfo { Id = "qwen/qwen3.8-27b", DisplayName = "Qwen 3.8 27B (Khuyên dùng - Siêu tốc)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "qwen", AccessTier = "standard", Modality = "chat", ContextLength = 32768, DefaultModel = true, IsDynamic = false },
                new AIModelInfo { Id = "openai/gpt-oss-120b", DisplayName = "OpenAI GPT-OSS 120B (Lập luận sâu)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "openai", AccessTier = "standard", Modality = "chat", ContextLength = 128000, IsDynamic = false },
                new AIModelInfo { Id = "openai/gpt-oss-20b", DisplayName = "OpenAI GPT-OSS 20B (Nhanh & Ổn định)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "openai", AccessTier = "standard", Modality = "chat", ContextLength = 128000, IsDynamic = false },
                new AIModelInfo { Id = "allam-2-7b", DisplayName = "Allam 2 7B (Đa ngôn ngữ)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "sdaia", AccessTier = "standard", Modality = "chat", ContextLength = 8192, IsDynamic = false }
            };
        }
    }
}
