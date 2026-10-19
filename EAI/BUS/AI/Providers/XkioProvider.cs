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
            "google/gemini-2.5-flash",
            "google/gemini-3.5-flash",
            "deepseek/deepseek-v4.1-flash",
            "openai/gpt-5.4-mini",
            "mistralai/ministral-8b"
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
    }
}
