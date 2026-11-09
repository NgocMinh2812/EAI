using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Configuration;
using DAL;
using DTO;

namespace BUS.AI
{
    public class GroqAudioService
    {
        private static GroqAudioService? _instance;
        public static GroqAudioService Instance => _instance ??= new GroqAudioService();

        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromMinutes(3) };
        private const string GroqBaseUrl = "https://api.groq.com/openai/v1";

        public const string DefaultSttModel = "whisper-large-v3-turbo";
        public const string SecondarySttModel = "whisper-large-v3";
        public const string DefaultTtsModel = "canopylabs/orpheus-v1-english";

        public static readonly IReadOnlyList<string> SupportedVoices = new[]
        {
            "diana",   // Nữ - Giọng ấm, truyền cảm
            "autumn",  // Nữ - Giọng tươi sáng, tự nhiên
            "hannah",  // Nữ - Giọng học thuật, rõ chữ
            "daniel",  // Nam - Giọng trầm, bản xứ Anh-Mỹ
            "austin",  // Nam - Giọng trẻ trung, năng động
            "troy"     // Nam - Giọng chuyên gia, thuyết trình
        };

        /// <summary>
        /// Thư mục lưu trữ các tệp âm thanh do AI sinh ra hoặc người dùng tải lên
        /// </summary>
        public static string GetAudioStorageDirectory()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dir = Path.Combine(baseDir, "Data", "AudioResources");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        #region 1. SPEECH-TO-TEXT (STT) - CHUYỂN AUDIO THÀNH VĂN BẢN

        /// <summary>
        /// Chuyển đổi tệp âm thanh thành bản chép lời (transcript) và phân đoạn thời gian (segments).
        /// Có cơ chế Cache tự động theo SHA256 AudioHash để không gọi lại API khi audio không đổi.
        /// </summary>
        public async Task<(bool Success, AudioTranscriptDTO? Result, string ErrorMessage)> TranscribeAudioAsync(
            string audioFilePath, 
            int? maBaiHoc = null, 
            string? model = null, 
            string language = "en", 
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(audioFilePath) || !File.Exists(audioFilePath))
            {
                return (false, null, "Không tìm thấy tệp âm thanh nguồn để tạo bản chép lời.");
            }

            var fileInfo = new FileInfo(audioFilePath);
            if (fileInfo.Length == 0)
            {
                return (false, null, "Tệp âm thanh nguồn bị rỗng (0 bytes).");
            }
            if (fileInfo.Length > 95 * 1024 * 1024)
            {
                return (false, null, "Dung lượng tệp âm thanh vượt quá giới hạn cho phép của hệ thống (tối đa 95MB).");
            }

            string ext = fileInfo.Extension.ToLowerInvariant();
            if (ext != ".mp3" && ext != ".wav" && ext != ".m4a" && ext != ".ogg" && ext != ".flac")
            {
                return (false, null, $"Định dạng tệp '{ext}' không được hỗ trợ. Vui lòng sử dụng tệp MP3, WAV, M4A, OGG hoặc FLAC.");
            }

            string audioHash = WavAudioHelper.TinhFileHash(audioFilePath);
            string targetModel = string.IsNullOrWhiteSpace(model) ? DefaultSttModel : model;

            // BƯỚC 1: KIỂM TRA CACHE TRONG DATABASE (Không gọi AI nếu audio không đổi)
            if (!forceRefresh && !string.IsNullOrEmpty(audioHash))
            {
                var cached = AudioTranscriptDAL.Instance.LayTheoAudioHash(audioHash);
                if (cached != null && cached.HasTranscript)
                {
                    System.Diagnostics.Debug.WriteLine($"[GroqAudioService.STT] Tái sử dụng Transcript đã lưu (Hash: {audioHash[..8]}). Không gọi Groq API.");
                    return (true, cached, string.Empty);
                }

                // Nếu kiểm tra theo đường dẫn audio
                var cachedByPath = AudioTranscriptDAL.Instance.LayTheoDuongDanAudio(audioFilePath);
                if (cachedByPath != null && cachedByPath.HasTranscript)
                {
                    System.Diagnostics.Debug.WriteLine($"[GroqAudioService.STT] Tái sử dụng Transcript theo đường dẫn {audioFilePath}. Không gọi Groq API.");
                    return (true, cachedByPath, string.Empty);
                }
            }

            // BƯỚC 1.5: ƯU TIÊN GỌI QUA CENTRALIZED BACKEND EAI.AIBackend.API (http://localhost:5281)
            if (AIConfigHelper.IsUseCentralizedBackend())
            {
                try
                {
                    var (beSuccess, beTranscript, beErr) = await EAIBackendClient.TranscribeAudioAsync(audioFilePath, language, ct);
                    if (beSuccess && beTranscript != null)
                    {
                        beTranscript.MaBaiHoc = maBaiHoc;
                        beTranscript.AudioPath = audioFilePath;
                        beTranscript.AudioHash = audioHash;

                        try
                        {
                            AudioTranscriptDAL.Instance.LuuAudioTranscript(beTranscript);
                        }
                        catch { }

                        return (true, beTranscript, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GroqAudioService.STT] Centralized Backend gặp lỗi hoặc offline: {ex.Message}. Fallback về Groq trực tiếp...");
                }
            }

            // BƯỚC 2: KIỂM TRA GROQ API KEY
            string apiKey = AIConfigHelper.GetGroqKey();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return (false, null, "Chưa cấu hình Groq API Key trong hệ thống. Vui lòng thiết lập biến GROQ_API_KEY hoặc trong User Secrets.");
            }

            // BƯỚC 3: GỬI REQUEST MULTIPART TỚI GROQ TRANSCRIPTIONS ENDPOINT
            try
            {
                string endpoint = $"{GroqBaseUrl}/audio/transcriptions";
                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                using var form = new MultipartFormDataContent();
                byte[] fileBytes = await File.ReadAllBytesAsync(audioFilePath, ct);
                var audioContent = new ByteArrayContent(fileBytes);
                audioContent.Headers.ContentType = new MediaTypeHeaderValue(GetMimeType(ext));
                form.Add(audioContent, "file", fileInfo.Name);

                form.Add(new StringContent(targetModel), "model");
                form.Add(new StringContent("verbose_json"), "response_format");
                form.Add(new StringContent(language), "language");
                form.Add(new StringContent("0.0"), "temperature");
                form.Add(new StringContent("segment"), "timestamp_granularities[]");

                request.Content = form;

                var response = await _httpClient.SendAsync(request, ct);
                string responseBody = await response.Content.ReadAsStringAsync(ct);
                int statusCode = (int)response.StatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    string errDetail = ParseGroqError(statusCode, responseBody);
                    return (false, null, errDetail);
                }

                // BƯỚC 4: PARSE VERBOSE_JSON VÀ SEGMENTS
                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;
                string fullText = root.TryGetProperty("text", out var tProp) ? tProp.GetString() ?? "" : "";
                double? duration = root.TryGetProperty("duration", out var dProp) && dProp.TryGetDouble(out double dur) ? dur : null;

                var segmentsList = new List<TranscriptSegmentItemDTO>();
                if (root.TryGetProperty("segments", out var segProp) && segProp.ValueKind == JsonValueKind.Array)
                {
                    int segId = 0;
                    foreach (var s in segProp.EnumerateArray())
                    {
                        double start = s.TryGetProperty("start", out var stP) && stP.TryGetDouble(out double st) ? st : 0;
                        double end = s.TryGetProperty("end", out var endP) && endP.TryGetDouble(out double ed) ? ed : 0;
                        string segText = s.TryGetProperty("text", out var txP) ? txP.GetString()?.Trim() ?? "" : "";

                        if (!string.IsNullOrEmpty(segText))
                        {
                            segmentsList.Add(new TranscriptSegmentItemDTO
                            {
                                Id = segId++,
                                Start = Math.Round(start, 2),
                                End = Math.Round(end, 2),
                                Text = segText
                            });
                        }
                    }
                }

                // BƯỚC 5: TẠO BẢN GHI DTO VÀ LƯU VÀO DATABASE (Trạng thái: ChoDuyet để Giảng viên kiểm tra)
                var dto = new AudioTranscriptDTO
                {
                    MaBaiHoc = maBaiHoc,
                    AudioPath = audioFilePath,
                    LoaiAudio = "Uploaded",
                    TieuDeAudio = fileInfo.Name,
                    TranscriptText = fullText.Trim(),
                    Provider = "Groq",
                    MoHinhSTT = targetModel,
                    AudioHash = audioHash,
                    TrangThaiDuyet = "ChoDuyet", // Bản nháp chờ giáo viên duyệt
                    ChoPhepSinhVienXem = true,
                    ThoiLuongGiay = duration,
                    DungLuongBytes = fileInfo.Length,
                    Segments = segmentsList
                };

                int savedId = AudioTranscriptDAL.Instance.LuuAudioTranscript(dto);
                dto.MaAudioTranscript = savedId;

                return (true, dto, string.Empty);
            }
            catch (OperationCanceledException)
            {
                return (false, null, "Quá trình chuyển âm thanh thành văn bản bị hủy hoặc quá thời gian chờ.");
            }
            catch (Exception ex)
            {
                return (false, null, $"Lỗi xử lý Speech-to-Text: {ex.Message}");
            }
        }

        #endregion

        #region 2. TEXT-TO-SPEECH (TTS) - TẠO BÀI NGHE TỪ KỊCH BẢN

        /// <summary>
        /// Tạo file audio giọng bản xứ từ kịch bản tiếng Anh sử dụng Groq Orpheus TTS.
        /// Tự động chia nhỏ script thành các câu/vế < 175 ký tự, gọi API và ghép file WAV nguyên bản.
        /// </summary>
        public async Task<(bool Success, AudioTranscriptDTO? Result, string ErrorMessage)> GenerateSpeechAsync(
            string script, 
            string voice = "diana", 
            int? maBaiHoc = null, 
            string? model = null, 
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(script))
            {
                return (false, null, "Kịch bản (script) để tạo âm thanh không được để trống.");
            }

            string targetModel = string.IsNullOrWhiteSpace(model) ? DefaultTtsModel : model;
            string targetVoice = string.IsNullOrWhiteSpace(voice) ? "diana" : voice.ToLowerInvariant().Trim();

            // Tính mã băm kết hợp kịch bản, giọng đọc và mô hình để kiểm tra cache
            string scriptHash = WavAudioHelper.TinhStringHash($"{targetModel}_{targetVoice}_{script.Trim()}");

            // BƯỚC 1: KIỂM TRA CACHE TRONG DATABASE
            if (!forceRefresh)
            {
                var cached = AudioTranscriptDAL.Instance.LayTheoAudioHash(scriptHash);
                if (cached != null && !string.IsNullOrEmpty(cached.AudioPath) && File.Exists(cached.AudioPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[GroqAudioService.TTS] Tái sử dụng Audio đã tạo từ Cache (Hash: {scriptHash[..8]}). Không gọi Groq API.");
                    return (true, cached, string.Empty);
                }
            }

            // BƯỚC 1.5: ƯU TIÊN GỌI QUA CENTRALIZED BACKEND EAI.AIBackend.API (http://localhost:5281)
            if (AIConfigHelper.IsUseCentralizedBackend())
            {
                try
                {
                    var (beOk, bePath, beErr) = await EAIBackendClient.GenerateSpeechAsync(script, targetVoice, ct);
                    if (beOk && !string.IsNullOrEmpty(bePath) && File.Exists(bePath))
                    {
                        var resultDto = new AudioTranscriptDTO
                        {
                            MaBaiHoc = maBaiHoc,
                            AudioPath = bePath,
                            LoaiAudio = "AiTts",
                            TieuDeAudio = script.Length > 60 ? script.Substring(0, 60) + "..." : script,
                            ScriptGoc = script,
                            Provider = "EAI.AIBackend (Groq Orpheus)",
                            MoHinhTTS = targetModel,
                            VoiceName = targetVoice,
                            AudioHash = scriptHash,
                            TrangThaiDuyet = "DaDuyet"
                        };

                        try
                        {
                            AudioTranscriptDAL.Instance.LuuAudioTranscript(resultDto);
                        }
                        catch { }

                        return (true, resultDto, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GroqAudioService.TTS] Centralized Backend gặp lỗi hoặc offline: {ex.Message}. Fallback về Groq trực tiếp...");
                }
            }

            // BƯỚC 2: KIỂM TRA GROQ API KEY
            string apiKey = AIConfigHelper.GetGroqKey();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return (false, null, "Chưa cấu hình Groq API Key trong hệ thống.");
            }

            // BƯỚC 3: CHUNKING KỊCH BẢN (Mỗi request < 175 ký tự để không chạm giới hạn 200 chars của Orpheus)
            var chunks = WavAudioHelper.TachScriptThanhChunks(script, 175);
            if (chunks.Count == 0)
            {
                return (false, null, "Không thể phân tách câu từ kịch bản được cung cấp.");
            }

            try
            {
                string endpoint = $"{GroqBaseUrl}/audio/speech";
                var wavChunks = new List<byte[]>();

                for (int i = 0; i < chunks.Count; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    string chunkText = chunks[i];

                    var payload = new
                    {
                        model = targetModel,
                        input = chunkText,
                        voice = targetVoice,
                        response_format = "wav"
                    };

                    string jsonPayload = JsonSerializer.Serialize(payload);
                    using var req = new HttpRequestMessage(HttpMethod.Post, endpoint);
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    req.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var res = await _httpClient.SendAsync(req, ct);
                    if (!res.IsSuccessStatusCode)
                    {
                        string errBody = await res.Content.ReadAsStringAsync(ct);
                        string parsedError = ParseGroqError((int)res.StatusCode, errBody);
                        return (false, null, $"Lỗi khi tạo audio đoạn {i + 1}/{chunks.Count}: {parsedError}");
                    }

                    byte[] chunkAudio = await res.Content.ReadAsByteArrayAsync(ct);
                    wavChunks.Add(chunkAudio);
                }

                // BƯỚC 4: GHÉP CÁC ĐOẠN WAV THÀNH FILE DUY NHẤT
                byte[] finalWavBytes = WavAudioHelper.GhepDanhSachWav(wavChunks, 24000, 1, 16);
                if (finalWavBytes.Length == 0)
                {
                    return (false, null, "Không thể ghép nối dữ liệu âm thanh đã nhận từ AI.");
                }

                // BƯỚC 5: LƯU TỆP ÂM THANH RA ĐĨA
                string storageDir = GetAudioStorageDirectory();
                string fileName = $"tts_{DateTime.Now:yyyyMMdd_HHmmss}_{targetVoice}_{scriptHash[..8]}.wav";
                string fullPath = Path.Combine(storageDir, fileName);

                await File.WriteAllBytesAsync(fullPath, finalWavBytes, ct);

                // Ước tính thời lượng (24000 samples/sec * 1 channel * 2 bytes/sample = 48000 bytes/sec)
                double thoiLuongUocTinh = Math.Round((double)(finalWavBytes.Length - 44) / 48000.0, 2);

                // BƯỚC 6: TẠO BẢN GHI METADATA TRONG DATABASE
                var dto = new AudioTranscriptDTO
                {
                    MaBaiHoc = maBaiHoc,
                    AudioPath = fullPath,
                    LoaiAudio = "AiTts",
                    TieuDeAudio = $"AI Audio ({targetVoice}) - {DateTime.Now:dd/MM/yyyy HH:mm}",
                    ScriptGoc = script,
                    TranscriptText = script,
                    Provider = "Groq",
                    MoHinhTTS = targetModel,
                    VoiceName = targetVoice,
                    AudioHash = scriptHash,
                    TrangThaiDuyet = "ChoDuyet", // Giảng viên nghe thử trước khi duyệt
                    ChoPhepSinhVienXem = true,
                    ThoiLuongGiay = thoiLuongUocTinh,
                    DungLuongBytes = finalWavBytes.Length
                };

                int savedId = AudioTranscriptDAL.Instance.LuuAudioTranscript(dto);
                dto.MaAudioTranscript = savedId;

                return (true, dto, string.Empty);
            }
            catch (OperationCanceledException)
            {
                return (false, null, "Quá trình tạo audio bị hủy bỏ.");
            }
            catch (Exception ex)
            {
                return (false, null, $"Lỗi tạo Audio Text-to-Speech: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo bài nghe hội thoại nhiều nhân vật với các giọng đọc khác nhau và khoảng lặng giữa các lượt nói
        /// </summary>
        public async Task<(bool Success, AudioTranscriptDTO? Result, string ErrorMessage)> GenerateDialogueSpeechAsync(
            List<DialogueVoiceLineDTO> lines, 
            int? maBaiHoc = null, 
            string? model = null, 
            CancellationToken ct = default)
        {
            if (lines == null || lines.Count == 0)
            {
                return (false, null, "Danh sách các lượt thoại không được để trống.");
            }

            string targetModel = string.IsNullOrWhiteSpace(model) ? DefaultTtsModel : model;
            string apiKey = AIConfigHelper.GetGroqKey();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return (false, null, "Chưa cấu hình Groq API Key.");
            }

            var combinedWavChunks = new List<byte[]>();
            var fullScriptSb = new StringBuilder();

            try
            {
                string endpoint = $"{GroqBaseUrl}/audio/speech";

                for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
                {
                    ct.ThrowIfCancellationRequested();
                    var line = lines[lineIndex];
                    if (string.IsNullOrWhiteSpace(line.Text)) continue;

                    string voice = string.IsNullOrWhiteSpace(line.Voice) ? "diana" : line.Voice.ToLowerInvariant().Trim();
                    fullScriptSb.AppendLine($"{line.Speaker}: {line.Text}");

                    // Chunking nếu lời thoại dài
                    var chunks = WavAudioHelper.TachScriptThanhChunks(line.Text, 175);
                    foreach (var chunkText in chunks)
                    {
                        var payload = new
                        {
                            model = targetModel,
                            input = chunkText,
                            voice = voice,
                            response_format = "wav"
                        };

                        using var req = new HttpRequestMessage(HttpMethod.Post, endpoint);
                        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                        req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                        var res = await _httpClient.SendAsync(req, ct);
                        if (!res.IsSuccessStatusCode)
                        {
                            string errBody = await res.Content.ReadAsStringAsync(ct);
                            return (false, null, $"Lỗi lượt thoại {lineIndex + 1} ({line.Speaker}): {ParseGroqError((int)res.StatusCode, errBody)}");
                        }

                        byte[] chunkBytes = await res.Content.ReadAsByteArrayAsync(ct);
                        combinedWavChunks.Add(chunkBytes);
                    }

                    // Chèn khoảng lặng tự nhiên giữa các lượt nói nếu chưa phải câu cuối
                    if (lineIndex < lines.Count - 1 && line.PauseAfterSeconds > 0)
                    {
                        byte[] silence = WavAudioHelper.TaoKhoangLangWav(line.PauseAfterSeconds, 24000, 1, 16);
                        combinedWavChunks.Add(silence);
                    }
                }

                byte[] finalWavBytes = WavAudioHelper.GhepDanhSachWav(combinedWavChunks, 24000, 1, 16);
                if (finalWavBytes.Length == 0)
                {
                    return (false, null, "Không tạo được dữ liệu âm thanh từ hội thoại.");
                }

                string storageDir = GetAudioStorageDirectory();
                string scriptHash = WavAudioHelper.TinhStringHash(fullScriptSb.ToString());
                string fileName = $"dialogue_{DateTime.Now:yyyyMMdd_HHmmss}_{scriptHash[..8]}.wav";
                string fullPath = Path.Combine(storageDir, fileName);

                await File.WriteAllBytesAsync(fullPath, finalWavBytes, ct);

                double thoiLuong = Math.Round((double)(finalWavBytes.Length - 44) / 48000.0, 2);

                var dto = new AudioTranscriptDTO
                {
                    MaBaiHoc = maBaiHoc,
                    AudioPath = fullPath,
                    LoaiAudio = "AiTts",
                    TieuDeAudio = $"Hội thoại AI ({lines.Count} lượt thoại) - {DateTime.Now:dd/MM/yyyy HH:mm}",
                    ScriptGoc = fullScriptSb.ToString(),
                    TranscriptText = fullScriptSb.ToString(),
                    Provider = "Groq",
                    MoHinhTTS = targetModel,
                    VoiceName = "Multi-Voice",
                    AudioHash = scriptHash,
                    TrangThaiDuyet = "ChoDuyet",
                    ChoPhepSinhVienXem = true,
                    ThoiLuongGiay = thoiLuong,
                    DungLuongBytes = finalWavBytes.Length
                };

                int savedId = AudioTranscriptDAL.Instance.LuuAudioTranscript(dto);
                dto.MaAudioTranscript = savedId;

                return (true, dto, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, null, $"Lỗi tạo Audio hội thoại: {ex.Message}");
            }
        }

        #endregion

        #region Helpers

        private static string GetMimeType(string extension) => extension switch
        {
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".m4a" => "audio/m4a",
            ".ogg" => "audio/ogg",
            ".flac" => "audio/flac",
            _ => "application/octet-stream"
        };

        private static string ParseGroqError(int statusCode, string responseBody)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseBody);
                if (doc.RootElement.TryGetProperty("error", out var errObj))
                {
                    string msg = errObj.TryGetProperty("message", out var mProp) ? mProp.GetString() ?? "" : "";
                    string code = errObj.TryGetProperty("code", out var cProp) ? cProp.GetString() ?? "" : "";

                    if (code == "model_terms_required" || msg.Contains("terms acceptance", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Mô hình Groq Orpheus TTS yêu cầu chấp nhận điều khoản sử dụng. Vui lòng truy cập https://console.groq.com/playground?model=canopylabs%2Forpheus-v1-english để Accept Terms.";
                    }

                    if (!string.IsNullOrEmpty(msg))
                    {
                        return $"Groq báo lỗi: {msg}";
                    }
                }
            }
            catch { }

            return statusCode switch
            {
                400 => "Yêu cầu không hợp lệ hoặc dữ liệu vượt quá giới hạn mô hình (HTTP 400).",
                401 => "API Key Groq không hợp lệ hoặc đã hết hạn (HTTP 401).",
                403 => "Không có quyền truy cập tài nguyên âm thanh Groq (HTTP 403).",
                404 => "Không tìm thấy mô hình hoặc endpoint âm thanh trên Groq (HTTP 404).",
                429 => "Đã vượt quá hạn mức tốc độ gọi API Groq (HTTP 429 Rate Limit/Quota).",
                >= 500 => $"Máy chủ Groq gặp sự cố nội bộ (HTTP {statusCode}).",
                _ => $"Lỗi HTTP {statusCode}: {responseBody}"
            };
        }

        #endregion
    }
}

