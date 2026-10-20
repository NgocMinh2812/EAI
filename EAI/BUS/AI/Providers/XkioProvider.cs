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
    /// <summary>
    /// Adapter tích hợp dành cho nhà cung cấp xKiro AI Gateway (api.xkiro.com).
    /// Hỗ trợ chuẩn OpenAI, quản lý routing nhiều mô hình LLM thông qua một API Key thống nhất.
    /// </summary>
    public class XkioProvider : IAIProvider
    {
        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(50) };

        public string ProviderName => "xKiro AI Gateway";
        public AIProviderType ProviderType => AIProviderType.Xkio;

        public bool IsConfigured
        {
            get
            {
                var (apiKey, _) = AIConfigHelper.GetXkioConfig();
                return !string.IsNullOrWhiteSpace(apiKey);
            }
        }

        public IReadOnlyList<string> SupportedModels => new[]
        {
            "qwen/qwen3.5-flash:free",
            "qwen/qwen3.8-max:free",
            "qwen/qwen3.7-plus:free",
            "qwen/qwen3.7-flash:free",
            "cohere/command-r7b-12-2024",
            "nvidia/nemotron-3-ultra",
            "stealth/big-pickle",
            "mistralai/ministral-8b",
            "z-ai/glm-5.3"
        };

        public string DefaultModel => "qwen/qwen3.5-flash:free";

        public async Task<AIResponse> GenerateContentAsync(string prompt, string? model = null, CancellationToken ct = default)
        {
            var (apiKey, baseUrl) = AIConfigHelper.GetXkioConfig();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return AIResponse.Fail(
                    ProviderName, 
                    "Chưa cấu hình xKiro API Key. Vui lòng thiết lập biến môi trường XKIO_API_KEY hoặc cấu hình trong secrets.json.", 
                    401);
            }

            string targetModel = string.IsNullOrWhiteSpace(model) ? DefaultModel : model;
            string endpoint = AIConfigHelper.BuildEndpoint(baseUrl, "chat/completions");

            var requestObj = new
            {
                model = targetModel,
                messages = new[] { new { role = "user", content = prompt } },
                temperature = 0.5
            };

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
                    if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                    {
                        var choice0 = choices[0];
                        if (choice0.TryGetProperty("message", out var msg) && msg.TryGetProperty("content", out var c))
                        {
                            return AIResponse.Ok(c.GetString() ?? "", ProviderName, targetModel, sw.ElapsedMilliseconds, statusCode);
                        }
                    }
                    if (doc.RootElement.TryGetProperty("text", out var directText))
                    {
                        return AIResponse.Ok(directText.GetString() ?? "", ProviderName, targetModel, sw.ElapsedMilliseconds, statusCode);
                    }
                }

                // Phân loại mã lỗi chi tiết
                string errorDetail = statusCode switch
                {
                    401 => "API Key xKiro không hợp lệ hoặc đã hết hạn (HTTP 401)",
                    403 => "Không có quyền sử dụng tài nguyên xKiro (HTTP 403)",
                    404 => $"Mô hình xKiro '{targetModel}' không tồn tại hoặc sai đường dẫn endpoint (HTTP 404)",
                    429 => "Đã vượt quá hạn mức token/lượt gọi xKiro hôm nay (HTTP 429)",
                    >= 500 => $"Máy chủ cổng xKiro đang gặp sự cố (HTTP {statusCode})",
                    _ => $"Cổng xKiro trả về HTTP {statusCode}: {responseBody}"
                };

                return AIResponse.Fail(ProviderName, errorDetail, statusCode, sw.ElapsedMilliseconds);
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                return AIResponse.Fail(ProviderName, "Yêu cầu tới xKiro API bị timeout (quá 20s).", 408, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                return AIResponse.Fail(ProviderName, $"Lỗi kết nối cổng xKiro: {ex.Message}", 500, sw.ElapsedMilliseconds);
            }
        }

        public async Task<AIHealthStatus> CheckHealthAsync(CancellationToken ct = default)
        {
            var (apiKey, baseUrl) = AIConfigHelper.GetXkioConfig();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new AIHealthStatus
                {
                    ProviderName = ProviderName,
                    ProviderType = ProviderType,
                    IsConfigured = false,
                    Health = "Unavailable",
                    Message = "Chưa cấu hình API Key (XKIO_API_KEY)",
                    ActiveModel = DefaultModel,
                    LastChecked = DateTime.Now
                };
            }

            var sw = Stopwatch.StartNew();
            try
            {
                // Gọi endpoint GET /models chuẩn hóa của xKiro AI Gateway
                string endpoint = AIConfigHelper.BuildEndpoint(baseUrl, "models");
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
                        Message = $"Cổng xKiro kết nối tốt ({sw.ElapsedMilliseconds}ms)",
                        ActiveModel = DefaultModel,
                        LastChecked = DateTime.Now
                    };
                }

                var (healthLevel, msg) = statusCode switch
                {
                    401 => ("Unavailable", "API Key xKiro không hợp lệ (HTTP 401 Unauthorized)"),
                    403 => ("Unavailable", "Không có quyền truy cập xKiro (HTTP 403 Forbidden)"),
                    404 => ("Unavailable", "Sai đường dẫn Endpoint Gateway xKiro (HTTP 404)"),
                    429 => ("Degraded", "Vượt quá hạn mức Token/Quota xKiro hôm nay (HTTP 429)"),
                    >= 500 => ("Unavailable", $"Máy chủ xKiro đang bảo trì (HTTP {statusCode})"),
                    _ => ("Unavailable", $"Endpoint phản hồi HTTP {statusCode} ({res.ReasonPhrase})")
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
                    Message = $"Chưa thể kết nối endpoint: {ex.Message}",
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

            var (apiKey, baseUrl) = AIConfigHelper.GetXkioConfig();
            string endpoint = AIConfigHelper.BuildEndpoint(baseUrl, "models");

            try
            {
                // GET /v1/models là public endpoint theo tài liệu xKiro (có thể gửi kèm key nếu có)
                using var req = new HttpRequestMessage(HttpMethod.Get, endpoint);
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                }

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(7));
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

                            string modality = item.TryGetProperty("modality", out var modProp) ? (modProp.GetString() ?? "chat") : "chat";
                            // Loại bỏ các mô hình hình ảnh (image), giọng nói (tts, stt) hoặc embedding
                            if (!string.Equals(modality, "chat", StringComparison.OrdinalIgnoreCase))
                                continue;

                            string displayName = item.TryGetProperty("display_name", out var dnProp) ? (dnProp.GetString() ?? id) : id;
                            string ownedBy = item.TryGetProperty("owned_by", out var obProp) ? (obProp.GetString() ?? "xkiro") : "xkiro";
                            string accessTier = item.TryGetProperty("access_tier", out var atProp) ? (atProp.GetString() ?? "free") : "free";
                            int? contextLength = item.TryGetProperty("context_length", out var clProp) ? clProp.GetInt32() : (int?)null;

                            bool isDefault = (id == DefaultModel) || (id == "qwen/qwen3.5-flash:free");
                            bool isFree = id.EndsWith(":free", StringComparison.OrdinalIgnoreCase) || string.Equals(accessTier, "free", StringComparison.OrdinalIgnoreCase);

                            dynamicList.Add(new AIModelInfo
                            {
                                Id = id, // Giữ nguyên đầy đủ tiền tố vendor/model
                                DisplayName = isDefault && !displayName.Contains("Khuyên dùng") ? $"{displayName} (Khuyên dùng)" : displayName,
                                ProviderName = ProviderName,
                                ProviderType = ProviderType,
                                OwnedBy = ownedBy,
                                AccessTier = isFree ? "free" : accessTier,
                                Modality = modality,
                                ContextLength = contextLength,
                                IsDynamic = true,
                                DefaultModel = isDefault
                            });
                        }

                        if (dynamicList.Count > 0)
                        {
                            // Sắp xếp mô hình mặc định (Khuyên dùng) và các mô hình Free lên đầu
                            _cachedModels = dynamicList
                                .OrderByDescending(x => x.DefaultModel)
                                .ThenByDescending(x => x.AccessTier == "free")
                                .ThenBy(x => x.DisplayName)
                                .ToList();
                            _lastCacheTime = DateTime.Now;
                            IsCatalogDynamic = true;
                            CatalogStatusMessage = $"Đồng bộ trực tiếp từ xKiro Gateway API ({_cachedModels.Count} mô hình chat)";
                            return _cachedModels;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[XkioProvider.GetModelsAsync] Lỗi gọi catalog động: {ex.Message}");
            }

            // Fallback danh sách mô hình xKiro chuẩn hóa đã xác minh
            var fallback = GetFallbackModels();
            _cachedModels = fallback;
            _lastCacheTime = DateTime.Now;
            IsCatalogDynamic = false;
            CatalogStatusMessage = "Không thể kết nối API catalog — Dùng danh mục dự phòng đã xác minh từ xKiro Docs";
            return fallback;
        }

        private List<AIModelInfo> GetFallbackModels()
        {
            return new List<AIModelInfo>
            {
                new AIModelInfo { Id = "qwen/qwen3.5-flash:free", DisplayName = "Qwen 3.5 Flash (Miễn phí • Khuyên dùng)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "qwen", AccessTier = "free", Modality = "chat", ContextLength = 1000000, DefaultModel = true, IsDynamic = false },
                new AIModelInfo { Id = "qwen/qwen3.8-max:free", DisplayName = "Qwen 3.8 Max (Miễn phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "qwen", AccessTier = "free", Modality = "chat", ContextLength = 1000000, IsDynamic = false },
                new AIModelInfo { Id = "qwen/qwen3.7-plus:free", DisplayName = "Qwen 3.7 Plus (Miễn phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "qwen", AccessTier = "free", Modality = "chat", ContextLength = 1000000, IsDynamic = false },
                new AIModelInfo { Id = "qwen/qwen3.7-flash:free", DisplayName = "Qwen 3.7 Flash (Miễn phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "qwen", AccessTier = "free", Modality = "chat", ContextLength = 1000000, IsDynamic = false },
                new AIModelInfo { Id = "cohere/command-r7b-12-2024", DisplayName = "Cohere Command R7B (Miễn phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "cohere", AccessTier = "free", Modality = "chat", ContextLength = 132000, IsDynamic = false },
                new AIModelInfo { Id = "nvidia/nemotron-3-ultra", DisplayName = "Nvidia Nemotron 3 Ultra (Trả phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "nvidia", AccessTier = "paid", Modality = "chat", ContextLength = 1000000, IsDynamic = false },
                new AIModelInfo { Id = "stealth/big-pickle", DisplayName = "Big Pickle (Miễn phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "stealth", AccessTier = "free", Modality = "chat", ContextLength = 200000, IsDynamic = false },
                new AIModelInfo { Id = "mistralai/ministral-8b", DisplayName = "Mistralai Ministral 8B (Trả phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "mistralai", AccessTier = "paid", Modality = "chat", ContextLength = 128000, IsDynamic = false },
                new AIModelInfo { Id = "z-ai/glm-5.3", DisplayName = "Z-AI GLM 5.3 (Trả phí)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "z-ai", AccessTier = "paid", Modality = "chat", ContextLength = 200000, IsDynamic = false }
            };
        }
    }
}
