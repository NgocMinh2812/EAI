using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Configuration;
using DTO;

namespace BUS.AI
{
    /// <summary>
    /// Centralized Client kết nối EAI Desktop trực tiếp với EAI.AIBackend.API (http://localhost:5281)
    /// Quản lý toàn bộ tác vụ Chat AI, Sinh đề/Câu hỏi, Bóc băng Whisper STT, và Phát âm TTS.
    /// Giúp EAI Desktop không còn phải lưu trữ hay quản lý bất kỳ API Key nào cục bộ!
    /// </summary>
    public static class EAIBackendClient
    {
        private static readonly HttpClient _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(120)
        };

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        public static string BaseUrl => AIConfigHelper.BackendBaseUrl;

        /// <summary>
        /// Kiểm tra nhanh xem Centralized Backend AI có đang hoạt động trên máy hay không
        /// </summary>
        public static async Task<bool> IsBackendAvailableAsync(CancellationToken ct = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                cts.CancelAfter(TimeSpan.FromSeconds(2));
                var res = await _httpClient.GetAsync($"{BaseUrl}/api/health", cts.Token);
                return res.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gọi Chat AI / Soạn bài học / Trợ lý học tập thông minh
        /// </summary>
        public static async Task<string?> ChatAsync(
            string prompt, 
            string? preferredProvider = null, 
            string? preferredModel = null, 
            CancellationToken ct = default)
        {
            var req = new
            {
                prompt = prompt,
                preferredProvider = preferredProvider,
                preferredModel = preferredModel
            };

            var json = JsonSerializer.Serialize(req);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/api/ai/chat", content, ct);
            var resBody = await response.Content.ReadAsStringAsync(ct);

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(resBody);
                if (doc.RootElement.TryGetProperty("data", out var data) &&
                    data.TryGetProperty("message", out var msg))
                {
                    return msg.GetString() ?? string.Empty;
                }
            }

            throw new Exception($"[EAIBackendClient] Chat thất bại ({response.StatusCode}): {resBody}");
        }

        /// <summary>
        /// Sinh nội dung học tập theo tiêu đề, trình độ, kỹ năng
        /// </summary>
        public static async Task<string?> GenerateLearningContentAsync(
            string title, 
            string level = "B1", 
            string lessonType = "Doc", 
            CancellationToken ct = default)
        {
            var req = new
            {
                title = title,
                level = level,
                lessonType = lessonType
            };

            var json = JsonSerializer.Serialize(req);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/api/ai/generate-learning-content", content, ct);
            var resBody = await response.Content.ReadAsStringAsync(ct);

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(resBody);
                if (doc.RootElement.TryGetProperty("data", out var data) &&
                    data.TryGetProperty("message", out var msg))
                {
                    return msg.GetString() ?? string.Empty;
                }
            }

            throw new Exception($"[EAIBackendClient] Sinh nội dung học tập thất bại: {resBody}");
        }

        /// <summary>
        /// Sinh danh sách câu hỏi trắc nghiệm / tự luận khảo thí (Khớp 100% với CauHoiDTO của EAI)
        /// </summary>
        public static async Task<List<CauHoiDTO>> GenerateQuestionsAsync(
            string topic,
            string content,
            string level = "B1",
            int count = 10,
            string skill = "Grammar",
            string format = "Auto",
            CancellationToken ct = default)
        {
            var req = new
            {
                topic = topic,
                content = content,
                level = level,
                questionCount = count,
                skillType = skill,
                questionFormat = format
            };

            var json = JsonSerializer.Serialize(req);
            using var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/api/ai/generate-questions", httpContent, ct);
            var resBody = await response.Content.ReadAsStringAsync(ct);

            if (response.IsSuccessStatusCode)
            {
                using var doc = JsonDocument.Parse(resBody);
                if (doc.RootElement.TryGetProperty("data", out var data))
                {
                    var items = JsonSerializer.Deserialize<List<BackendQuestionItemDto>>(data.GetRawText(), _jsonOpts);
                    var result = new List<CauHoiDTO>();

                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            result.Add(new CauHoiDTO
                            {
                                NoiDungCauHoi = item.NoiDungCauHoi ?? string.Empty,
                                LoaiCauHoi = item.LoaiCauHoi ?? "TracNghiem",
                                DanhSachLuaChon = item.DanhSachLuaChon ?? new List<string>(),
                                DapAnDungIndex = item.DapAnDungIndex,
                                DapAnVanBan = item.DapAnVanBan,
                                GiaiThichChiTiet = item.GiaiThichChiTiet,
                                TrinhDo = item.MucDo ?? level,
                                KyNang = skill
                            });
                        }
                    }
                    return result;
                }
            }

            throw new Exception($"[EAIBackendClient] Sinh câu hỏi thất bại: {resBody}");
        }

        /// <summary>
        /// Chuyển đổi tệp âm thanh thành văn bản qua Whisper STT
        /// </summary>
        public static async Task<(bool Success, AudioTranscriptDTO? Result, string ErrorMessage)> TranscribeAudioAsync(
            string audioFilePath, 
            string language = "en", 
            CancellationToken ct = default)
        {
            if (!File.Exists(audioFilePath))
            {
                return (false, null, "Không tìm thấy tệp âm thanh nguồn.");
            }

            try
            {
                using var form = new MultipartFormDataContent();
                byte[] bytes = await File.ReadAllBytesAsync(audioFilePath, ct);
                var fileContent = new ByteArrayContent(bytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

                form.Add(fileContent, "File", Path.GetFileName(audioFilePath));
                form.Add(new StringContent(language), "Language");

                var response = await _httpClient.PostAsync($"{BaseUrl}/api/ai/speech-to-text", form, ct);
                var resBody = await response.Content.ReadAsStringAsync(ct);

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(resBody);
                    if (doc.RootElement.TryGetProperty("data", out var data))
                    {
                        string text = data.TryGetProperty("text", out var t) ? t.GetString() ?? "" : "";
                        double duration = data.TryGetProperty("durationSeconds", out var d) ? d.GetDouble() : 0;
                        string segJson = data.TryGetProperty("segments", out var s) ? s.GetRawText() : "[]";

                        var dto = new AudioTranscriptDTO
                        {
                            TranscriptText = text,
                            ThoiLuongGiay = duration,
                            TranscriptSegmentsJson = segJson,
                            Provider = "EAI.AIBackend (Groq Whisper)",
                            MoHinhSTT = "whisper-large-v3",
                            TrangThaiDuyet = "DaDuyet"
                        };
                        return (true, dto, string.Empty);
                    }
                }

                return (false, null, $"Lỗi Backend STT: {resBody}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Lỗi kết nối STT: {ex.Message}");
            }
        }

        /// <summary>
        /// Phát âm văn bản bằng AI (Text-to-Speech) và lưu ra tệp MP3
        /// </summary>
        public static async Task<(bool Success, string AudioPath, string ErrorMessage)> GenerateSpeechAsync(
            string text, 
            string voice = "troy", 
            CancellationToken ct = default)
        {
            try
            {
                var req = new
                {
                    text = text,
                    voice = voice,
                    audioFormat = "mp3"
                };

                var json = JsonSerializer.Serialize(req);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/api/ai/text-to-speech?rawAudio=true", content, ct);
                if (response.IsSuccessStatusCode)
                {
                    byte[] audioBytes = await response.Content.ReadAsByteArrayAsync(ct);
                    string targetDir = GroqAudioService.GetAudioStorageDirectory();
                    string fileName = $"BE_TTS_{Guid.NewGuid().ToString("N").Substring(0, 8)}.mp3";
                    string filePath = Path.Combine(targetDir, fileName);

                    await File.WriteAllBytesAsync(filePath, audioBytes, ct);
                    return (true, filePath, string.Empty);
                }

                var err = await response.Content.ReadAsStringAsync(ct);
                return (false, string.Empty, $"Lỗi TTS: {err}");
            }
            catch (Exception ex)
            {
                return (false, string.Empty, $"Lỗi kết nối TTS: {ex.Message}");
            }
        }

        private class BackendQuestionItemDto
        {
            public string? NoiDungCauHoi { get; set; }
            public string? LoaiCauHoi { get; set; }
            public List<string>? DanhSachLuaChon { get; set; }
            public int? DapAnDungIndex { get; set; }
            public string? DapAnVanBan { get; set; }
            public string? GiaiThichChiTiet { get; set; }
            public string? MucDo { get; set; }
        }
    }
}

