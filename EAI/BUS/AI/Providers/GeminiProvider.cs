using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Configuration;
using BUS.AI.Models;

namespace BUS.AI.Providers
{
    public class GeminiProvider : IAIProvider
    {
        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(25) };

        public string ProviderName => "Google Gemini";
        public AIProviderType ProviderType => AIProviderType.Gemini;

        public bool IsConfigured => !string.IsNullOrWhiteSpace(AIConfigHelper.GetGeminiKey());

        public IReadOnlyList<string> SupportedModels => new[]
        {
            "gemini-3.1-flash-lite",
            "gemini-3.8-flash",
            "gemini-3.5-flash",
            "gemini-3.7-flash",
            "gemini-flash-latest"
        };

        public string DefaultModel => "gemini-3.1-flash-lite";

        public async Task<AIResponse> GenerateContentAsync(string prompt, string? model = null, CancellationToken ct = default)
        {
            string apiKey = AIConfigHelper.GetGeminiKey();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return AIResponse.Fail(ProviderName, "Chưa cấu hình Gemini API Key (Vui lòng thiết lập biến GEMINI_API_KEY hoặc trong secrets.json)", 401);
            }

            var modelsToTry = new List<string>();
            if (!string.IsNullOrWhiteSpace(model))
            {
                modelsToTry.Add(model);
            }
            foreach (var m in SupportedModels)
            {
                if (!modelsToTry.Contains(m))
                    modelsToTry.Add(m);
            }

            var requestObj = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.4,
                    maxOutputTokens = 8192
                }
            };

            string requestJson = JsonSerializer.Serialize(requestObj);
            var sw = Stopwatch.StartNew();
            string lastError = string.Empty;
            int lastCode = 500;

            foreach (var currentModel in modelsToTry)
            {
                try
                {
                    string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{currentModel}:generateContent?key={apiKey}";
                    using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);

                    var response = await _httpClient.PostAsync(endpoint, content, linkedCts.Token);
                    string responseBody = await response.Content.ReadAsStringAsync(ct);
                    lastCode = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        sw.Stop();
                        using var doc = JsonDocument.Parse(responseBody);
                        var candidates = doc.RootElement.GetProperty("candidates");
                        if (candidates.GetArrayLength() > 0)
                        {
                            var parts = candidates[0].GetProperty("content").GetProperty("parts");
                            var sb = new StringBuilder();
                            foreach (var part in parts.EnumerateArray())
                            {
                                if (part.TryGetProperty("text", out var textProp))
                                {
                                    sb.Append(textProp.GetString());
                                }
                            }

                            string resultText = sb.ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(resultText))
                            {
                                return AIResponse.Ok(resultText, ProviderName, currentModel, sw.ElapsedMilliseconds, lastCode);
                            }
                        }
                    }

                    // Phân loại lỗi
                    lastError = lastCode switch
                    {
                        400 => $"Gemini: Yêu cầu không hợp lệ hoặc cấu hình sai (HTTP 400)",
                        401 or 403 => "Gemini: API Key không hợp lệ hoặc không có quyền truy cập (HTTP 401/403)",
                        404 => $"Gemini: Model '{currentModel}' không tìm thấy trên API v1beta (HTTP 404)",
                        429 => "Gemini: Đã đạt hạn mức Quota hoặc bị Rate limit (HTTP 429)",
                        >= 500 => $"Gemini: Lỗi máy chủ Google (HTTP {lastCode})",
                        _ => $"Gemini ({currentModel}) trả về HTTP {lastCode}: {responseBody}"
                    };

                    // Nếu lỗi 404 (model không hỗ trợ) hoặc 429, thử tiếp model tiếp theo trong chuỗi fallback
                    if (lastCode == 404 || lastCode == 429 || lastCode >= 500)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    lastError = $"Timeout khi gọi mô hình {currentModel}";
                    lastCode = 408;
                }
                catch (Exception ex)
                {
                    lastError = $"Lỗi khi gọi {currentModel}: {ex.Message}";
                    lastCode = 500;
                }
            }

            sw.Stop();
            return AIResponse.Fail(ProviderName, lastError, lastCode, sw.ElapsedMilliseconds);
        }

        public async Task<AIHealthStatus> CheckHealthAsync(CancellationToken ct = default)
        {
            string apiKey = AIConfigHelper.GetGeminiKey();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new AIHealthStatus
                {
                    ProviderName = ProviderName,
                    ProviderType = ProviderType,
                    IsConfigured = false,
                    Health = "Unavailable",
                    Message = "Chưa cấu hình API Key (GEMINI_API_KEY)",
                    ActiveModel = DefaultModel,
                    LastChecked = DateTime.Now
                };
            }

            var sw = Stopwatch.StartNew();
            try
            {
                // Gọi API chính thức v1beta/models để kiểm tra kết nối và tính hợp lệ của key
                string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(6));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

                var res = await _httpClient.GetAsync(endpoint, linkedCts.Token);
                sw.Stop();

                int code = (int)res.StatusCode;
                if (res.IsSuccessStatusCode)
                {
                    // Chỉ đánh dấu Degraded nếu độ trễ quá cao (> 7000ms)
                    string health = sw.ElapsedMilliseconds < 7000 ? "Healthy" : "Degraded";
                    return new AIHealthStatus
                    {
                        ProviderName = ProviderName,
                        ProviderType = ProviderType,
                        IsConfigured = true,
                        Health = health,
                        LatencyMs = sw.ElapsedMilliseconds,
                        Message = $"Kết nối ổn định ({sw.ElapsedMilliseconds}ms)",
                        ActiveModel = DefaultModel,
                        LastChecked = DateTime.Now
                    };
                }

                // Phân biệt trạng thái lỗi chi tiết
                var (healthLevel, msg) = code switch
                {
                    400 or 401 or 403 => ("Unavailable", "Gemini API Key không hợp lệ hoặc hết hạn (HTTP 401/403)"),
                    404 => ("Unavailable", "Endpoint Gemini API không tồn tại (HTTP 404)"),
                    429 => ("Degraded", "Vượt quá hạn mức Google Gemini Quota (HTTP 429)"),
                    >= 500 => ("Unavailable", $"Máy chủ Google Gemini gặp lỗi (HTTP {code})"),
                    _ => ("Unavailable", $"HTTP {code} ({res.ReasonPhrase})")
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
                    Message = $"Lỗi kết nối Gemini: {ex.Message}",
                    ActiveModel = DefaultModel,
                    LastChecked = DateTime.Now
                };
            }
        }

        public bool IsCatalogDynamic => false;
        public string CatalogStatusMessage { get; private set; } = "Danh mục chuẩn Google Gemini";

        public Task<IReadOnlyList<AIModelInfo>> GetModelsAsync(bool forceRefresh = false, CancellationToken ct = default)
        {
            var list = new List<AIModelInfo>
            {
                new AIModelInfo { Id = "gemini-3.1-flash-lite", DisplayName = "Gemini 3.1 Flash-Lite (Khuyên dùng - Siêu tốc)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "google", AccessTier = "standard", Modality = "chat", ContextLength = 1048576, DefaultModel = true },
                new AIModelInfo { Id = "gemini-3.8-flash", DisplayName = "Gemini 3.8 Flash (Thế hệ mới nhất)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "google", AccessTier = "standard", Modality = "chat", ContextLength = 1048576 },
                new AIModelInfo { Id = "gemini-3.5-flash", DisplayName = "Gemini 3.5 Flash", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "google", AccessTier = "standard", Modality = "chat", ContextLength = 1048576 },
                new AIModelInfo { Id = "gemini-3.7-flash", DisplayName = "Gemini 3.7 Flash", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "google", AccessTier = "standard", Modality = "chat", ContextLength = 1048576 },
                new AIModelInfo { Id = "gemini-flash-latest", DisplayName = "Gemini Flash Latest", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "google", AccessTier = "standard", Modality = "chat", ContextLength = 1048576 }
            };
            CatalogStatusMessage = $"Đang sử dụng danh mục chuẩn Google Gemini ({list.Count} mô hình)";
            return Task.FromResult<IReadOnlyList<AIModelInfo>>(list);
        }
    }
}
