using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Configuration;
using BUS.AI.Models;

namespace BUS.AI.Providers
{
    /// <summary>
    /// Nhà cung cấp mô hình AI cục bộ ngoại tuyến thông qua Ollama (Local / Offline LLM)
    /// Hoạt động 100% không cần kết nối Internet khi người dùng cài đặt Ollama trên máy tính.
    /// </summary>
    public class OllamaProvider : IAIProvider
    {
        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(35) };

        public string ProviderName => "Ollama (Offline/Local LLM)";
        public AIProviderType ProviderType => AIProviderType.Ollama;

        // Ollama chạy trực tiếp trên Localhost nên không bắt buộc API Key
        public bool IsConfigured => true;

        public IReadOnlyList<string> SupportedModels => new[]
        {
            "llama3.2:latest",
            "llama3.2:3b",
            "llama3.2:1b",
            "qwen2.5:latest",
            "qwen2.5:7b",
            "qwen2.5:1.5b",
            "mistral:latest",
            "phi3:latest",
            "deepseek-r1:latest",
            "gemma2:latest"
        };

        public string DefaultModel => "llama3.2:latest";

        public async Task<AIResponse> GenerateContentAsync(string prompt, string? model = null, CancellationToken ct = default)
        {
            var (baseUrl, defaultModel) = AIConfigHelper.GetOllamaConfig();
            string targetModel = string.IsNullOrWhiteSpace(model) ? defaultModel : model;

            var sw = Stopwatch.StartNew();

            // 1. Thử gọi qua endpoint /api/generate của Ollama
            try
            {
                string endpoint = $"{baseUrl}/api/generate";
                var requestObj = new
                {
                    model = targetModel,
                    prompt = prompt,
                    stream = false,
                    options = new
                    {
                        temperature = 0.4
                    }
                };

                string requestJson = JsonSerializer.Serialize(requestObj);
                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
                };

                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(45));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);

                var response = await _httpClient.SendAsync(request, linkedCts.Token);
                sw.Stop();

                string responseBody = await response.Content.ReadAsStringAsync(ct);
                int statusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(responseBody);
                    if (doc.RootElement.TryGetProperty("response", out var respProp))
                    {
                        string text = respProp.GetString() ?? string.Empty;
                        return AIResponse.Ok(text, ProviderName, targetModel, sw.ElapsedMilliseconds, statusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[OllamaProvider.GenerateContentAsync] /api/generate thất bại: {ex.Message}. Thử endpoint /api/chat...");
            }

            // 2. Thử tiếp qua endpoint /api/chat của Ollama
            try
            {
                if (!sw.IsRunning) sw.Restart();

                string endpoint = $"{baseUrl}/api/chat";
                var chatObj = new
                {
                    model = targetModel,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    stream = false,
                    options = new
                    {
                        temperature = 0.4
                    }
                };

                string chatJson = JsonSerializer.Serialize(chatObj);
                using var chatReq = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(chatJson, Encoding.UTF8, "application/json")
                };

                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(45));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);

                var chatRes = await _httpClient.SendAsync(chatReq, linkedCts.Token);
                sw.Stop();

                string chatBody = await chatRes.Content.ReadAsStringAsync(ct);
                int chatStatus = (int)chatRes.StatusCode;

                if (chatRes.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(chatBody);
                    if (doc.RootElement.TryGetProperty("message", out var msgProp) &&
                        msgProp.TryGetProperty("content", out var contentProp))
                    {
                        string text = contentProp.GetString() ?? string.Empty;
                        return AIResponse.Ok(text, ProviderName, targetModel, sw.ElapsedMilliseconds, chatStatus);
                    }
                }

                return AIResponse.Fail(
                    ProviderName, 
                    $"Ollama trả về mã lỗi HTTP {chatStatus}. Vui lòng kiểm tra mô hình '{targetModel}' đã được tải chưa bằng lệnh: ollama pull {targetModel}", 
                    chatStatus);
            }
            catch (Exception ex)
            {
                sw.Stop();
                return AIResponse.Fail(
                    ProviderName, 
                    $"Không thể kết nối đến máy chủ Ollama tại '{baseUrl}'. Vui lòng đảm bảo Ollama đã được cài đặt và đang chạy (chạy lệnh 'ollama serve' hoặc mở ứng dụng Ollama từ khay hệ thống). Chi tiết: {ex.Message}", 
                    503);
            }
        }

        public async Task<AIHealthStatus> CheckHealthAsync(CancellationToken ct = default)
        {
            var (baseUrl, defaultModel) = AIConfigHelper.GetOllamaConfig();
            var sw = Stopwatch.StartNew();

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

                // Kiểm tra danh sách tags để biết Ollama có sống và có mô hình nào không
                string endpoint = $"{baseUrl}/api/tags";
                using var req = new HttpRequestMessage(HttpMethod.Get, endpoint);
                var res = await _httpClient.SendAsync(req, linkedCts.Token);
                sw.Stop();

                if (res.IsSuccessStatusCode)
                {
                    string body = await res.Content.ReadAsStringAsync(ct);
                    int modelCount = 0;
                    string firstModel = defaultModel;

                    try
                    {
                        using var doc = JsonDocument.Parse(body);
                        if (doc.RootElement.TryGetProperty("models", out var modelsArr) && modelsArr.ValueKind == JsonValueKind.Array)
                        {
                            modelCount = modelsArr.GetArrayLength();
                            if (modelCount > 0)
                            {
                                var first = modelsArr[0];
                                if (first.TryGetProperty("name", out var nameProp))
                                {
                                    firstModel = nameProp.GetString() ?? defaultModel;
                                }
                            }
                        }
                    }
                    catch { }

                    string msg = modelCount > 0 
                        ? $"Ollama đang chạy cục bộ ({modelCount} mô hình sẵn sàng)"
                        : "Ollama đang chạy nhưng chưa tải mô hình nào (dùng 'ollama pull llama3.2')";

                    return new AIHealthStatus
                    {
                        ProviderName = ProviderName,
                        ProviderType = ProviderType,
                        IsConfigured = true,
                        Health = "Healthy",
                        LatencyMs = sw.ElapsedMilliseconds,
                        Message = msg,
                        ActiveModel = firstModel,
                        LastChecked = DateTime.Now
                    };
                }

                return new AIHealthStatus
                {
                    ProviderName = ProviderName,
                    ProviderType = ProviderType,
                    IsConfigured = true,
                    Health = "Degraded",
                    LatencyMs = sw.ElapsedMilliseconds,
                    Message = $"Ollama phản hồi mã HTTP {(int)res.StatusCode}",
                    ActiveModel = defaultModel,
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
                    Message = $"Chưa khởi động ({baseUrl}). Cài đặt tại ollama.com hoặc chạy 'ollama serve'",
                    ActiveModel = defaultModel,
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

            var (baseUrl, defaultModel) = AIConfigHelper.GetOllamaConfig();

            try
            {
                string endpoint = $"{baseUrl}/api/tags";
                using var req = new HttpRequestMessage(HttpMethod.Get, endpoint);

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

                var res = await _httpClient.SendAsync(req, linkedCts.Token);
                if (res.IsSuccessStatusCode)
                {
                    string body = await res.Content.ReadAsStringAsync(ct);
                    using var doc = JsonDocument.Parse(body);

                    if (doc.RootElement.TryGetProperty("models", out var modelsArr) && modelsArr.ValueKind == JsonValueKind.Array)
                    {
                        var dynamicList = new List<AIModelInfo>();
                        foreach (var item in modelsArr.EnumerateArray())
                        {
                            string name = item.TryGetProperty("name", out var nProp) ? (nProp.GetString() ?? "") : "";
                            if (string.IsNullOrWhiteSpace(name)) continue;

                            string family = "Local";
                            string paramSize = "";
                            if (item.TryGetProperty("details", out var detailsProp))
                            {
                                if (detailsProp.TryGetProperty("family", out var fProp)) family = fProp.GetString() ?? "Local";
                                if (detailsProp.TryGetProperty("parameter_size", out var pProp)) paramSize = pProp.GetString() ?? "";
                            }

                            bool isDefault = (name == defaultModel) || name.StartsWith("llama3.2", StringComparison.OrdinalIgnoreCase);

                            dynamicList.Add(new AIModelInfo
                            {
                                Id = name,
                                DisplayName = $"{name} ({family}{(string.IsNullOrWhiteSpace(paramSize) ? "" : " " + paramSize)}) [Offline]",
                                ProviderName = ProviderName,
                                ProviderType = ProviderType,
                                OwnedBy = family,
                                AccessTier = "offline",
                                Modality = "chat",
                                IsDynamic = true,
                                DefaultModel = isDefault
                            });
                        }

                        if (dynamicList.Count > 0)
                        {
                            _cachedModels = dynamicList
                                .OrderByDescending(x => x.DefaultModel)
                                .ThenBy(x => x.DisplayName)
                                .ToList();
                            _lastCacheTime = DateTime.Now;
                            IsCatalogDynamic = true;
                            CatalogStatusMessage = $"Đồng bộ trực tiếp từ Ollama Local ({_cachedModels.Count} mô hình máy tính có sẵn)";
                            return _cachedModels;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[OllamaProvider.GetModelsAsync] Không thể lấy danh sách mô hình từ Ollama: {ex.Message}");
            }

            // Fallback sang các mô hình thông dụng khuyên dùng khi chưa kết nối được
            var fallback = GetFallbackModels();
            _cachedModels = fallback;
            _lastCacheTime = DateTime.Now;
            IsCatalogDynamic = false;
            CatalogStatusMessage = "Chưa kết nối Ollama — Dùng danh mục mô hình chuẩn bị sẵn (Khuyên dùng: Llama 3.2, Qwen 2.5)";
            return fallback;
        }

        private List<AIModelInfo> GetFallbackModels()
        {
            return new List<AIModelInfo>
            {
                new() { Id = "llama3.2:latest", DisplayName = "Llama 3.2 (Meta - Khuyên dùng Offline)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "Meta", AccessTier = "offline", Modality = "chat", DefaultModel = true, IsDynamic = false },
                new() { Id = "qwen2.5:latest", DisplayName = "Qwen 2.5 (Alibaba - Đa ngôn ngữ Offline)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "Alibaba", AccessTier = "offline", Modality = "chat", IsDynamic = false },
                new() { Id = "mistral:latest", DisplayName = "Mistral 7B (Mistral AI - Lập luận Offline)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "Mistral", AccessTier = "offline", Modality = "chat", IsDynamic = false },
                new() { Id = "phi3:latest", DisplayName = "Phi-3 Mini (Microsoft - Siêu nhẹ Offline)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "Microsoft", AccessTier = "offline", Modality = "chat", IsDynamic = false },
                new() { Id = "deepseek-r1:latest", DisplayName = "DeepSeek R1 (DeepSeek - Suy luận Toán/Code Offline)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "DeepSeek", AccessTier = "offline", Modality = "chat", IsDynamic = false },
                new() { Id = "gemma2:latest", DisplayName = "Gemma 2 (Google - Tối ưu CPU Offline)", ProviderName = ProviderName, ProviderType = ProviderType, OwnedBy = "Google", AccessTier = "offline", Modality = "chat", IsDynamic = false }
            };
        }
    }
}

