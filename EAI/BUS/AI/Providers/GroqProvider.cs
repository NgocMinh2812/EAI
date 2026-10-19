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
            "allam-2-7b"
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

            var requestObj = new
            {
                model = targetModel,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.4
            };

            string requestJson = JsonSerializer.Serialize(requestObj);
            var sw = Stopwatch.StartNew();

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
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
    }
}
