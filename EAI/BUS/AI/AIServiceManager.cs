using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Models;
using BUS.AI.Providers;
using DTO;

namespace BUS.AI
{
    public class AIServiceManager
    {
        private static readonly Lazy<AIServiceManager> _instance = new(() => new AIServiceManager());
        public static AIServiceManager Instance => _instance.Value;

        private readonly Dictionary<AIProviderType, IAIProvider> _providers = new();

        public AIProviderType ActiveProviderType { get; set; } = AIProviderType.Gemini;
        public string? ActiveModel { get; set; } = null;
        public bool EnableFallback { get; set; } = true;

        public AIRuntimeMetrics Metrics { get; } = new();

        private AIServiceManager()
        {
            RegisterProvider(new GeminiProvider());
            RegisterProvider(new GroqProvider());
            RegisterProvider(new XkioProvider());

            try
            {
                var (savedProvider, savedModel, savedFallback) = Configuration.AIConfigHelper.LoadActiveModelConfig();
                if (!string.IsNullOrWhiteSpace(savedProvider) && Enum.TryParse<AIProviderType>(savedProvider, true, out var pType))
                {
                    ActiveProviderType = pType;
                }
                if (!string.IsNullOrWhiteSpace(savedModel))
                {
                    ActiveModel = savedModel;
                }
                if (savedFallback.HasValue)
                {
                    EnableFallback = savedFallback.Value;
                }
            }
            catch { }
        }

        public void RegisterProvider(IAIProvider provider)
        {
            _providers[provider.ProviderType] = provider;
        }

        public IAIProvider GetProvider(AIProviderType type)
        {
            if (_providers.TryGetValue(type, out var p))
                return p;
            throw new NotSupportedException($"Nhà cung cấp AI '{type}' chưa được đăng ký trong hệ thống.");
        }

        public IEnumerable<IAIProvider> GetAllProviders()
        {
            return _providers.Values;
        }

        public async Task<IReadOnlyList<AIModelInfo>> GetModelsForProviderAsync(AIProviderType type, bool forceRefresh = false, CancellationToken ct = default)
        {
            var provider = GetProvider(type);
            return await provider.GetModelsAsync(forceRefresh, ct);
        }

        public async Task<AIResponse> ExecutePromptAsync(
            string prompt, 
            string? preferredProvider = null, 
            string? preferredModel = null, 
            string actionName = "AI Generation", 
            CancellationToken ct = default)
        {
            AIProviderType targetType = ActiveProviderType;
            if (!string.IsNullOrWhiteSpace(preferredProvider) && Enum.TryParse<AIProviderType>(preferredProvider, true, out var parsedType))
            {
                targetType = parsedType;
            }

            var provider = GetProvider(targetType);
            string? targetModel = !string.IsNullOrWhiteSpace(preferredModel) ? preferredModel : (targetType == ActiveProviderType ? ActiveModel : null);

            var sw = Stopwatch.StartNew();
            AIResponse response = await provider.GenerateContentAsync(prompt, targetModel, ct);
            sw.Stop();

            // Nếu thất bại và bật cơ chế chuyển đổi dự phòng (Fallback)
            if (!response.Success && EnableFallback)
            {
                foreach (var fallbackProvider in _providers.Values)
                {
                    if (fallbackProvider.ProviderType == targetType || !fallbackProvider.IsConfigured)
                        continue;

                    System.Diagnostics.Debug.WriteLine($"[AIServiceManager] Chuyển đổi dự phòng từ '{provider.ProviderName}' sang '{fallbackProvider.ProviderName}'...");
                    var fallbackResponse = await fallbackProvider.GenerateContentAsync(prompt, null, ct);
                    if (fallbackResponse.Success)
                    {
                        fallbackResponse.IsFallback = true;
                        response = fallbackResponse;
                        break;
                    }
                }
            }

            // Ghi nhận telemetry vào Metrics phục vụ Giám sát hệ thống
            var log = new AIRequestLog
            {
                ActionName = actionName,
                Provider = response.ProviderName,
                Model = response.ModelUsed,
                LatencyMs = response.LatencyMs,
                Success = response.Success,
                StatusCode = response.StatusCode,
                PromptPreview = prompt.Length > 120 ? prompt.Substring(0, 120) + "..." : prompt,
                ErrorMessage = response.ErrorMessage,
                IsFallback = response.IsFallback
            };
            Metrics.Record(log);

            return response;
        }

        /// <summary>
        /// Sinh nội dung giáo trình mẫu (Text Sample) bằng hệ thống đa mô hình AI
        /// </summary>
        public async Task<string> SinhNoiDungMauAsync(
            string tieuDe, 
            string trinhDo = "B1", 
            string loaiBaiHoc = "Doc", 
            string? preferredProvider = null,
            string? preferredModel = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(tieuDe))
            {
                return GeminiAIService.TaoNoiDungMauThongMinh(tieuDe, trinhDo, loaiBaiHoc);
            }

            try
            {
                string prompt = GeminiAIService.BuildSamplePrompt(tieuDe, trinhDo, loaiBaiHoc);
                var response = await ExecutePromptAsync(prompt, preferredProvider, preferredModel, "Tạo bài học bằng AI", ct);

                if (response.Success && !string.IsNullOrWhiteSpace(response.Text))
                {
                    string cleanText = response.Text.Trim();
                    if (cleanText.StartsWith("```markdown", StringComparison.OrdinalIgnoreCase)) cleanText = cleanText.Substring(11);
                    else if (cleanText.StartsWith("```", StringComparison.OrdinalIgnoreCase)) cleanText = cleanText.Substring(3);
                    if (cleanText.EndsWith("```")) cleanText = cleanText.Substring(0, cleanText.Length - 3);
                    cleanText = cleanText.Trim();

                    if (!string.IsNullOrWhiteSpace(cleanText))
                        return cleanText;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AIServiceManager.SinhNoiDungMauAsync] Lỗi: {ex.Message}");
            }

            // Fallback nội bộ nếu toàn bộ AI offline
            return GeminiAIService.TaoNoiDungMauThongMinh(tieuDe, trinhDo, loaiBaiHoc);
        }

        /// <summary>
        public async Task<List<CauHoiDTO>> BocTachCauHoiAsync(
            string tieuDe, 
            string noiDung, 
            string trinhDo = "B1", 
            int soLuong = 10, 
            string loaiBaiHoc = "", 
            string? preferredProvider = null,
            string? preferredModel = null,
            string? yeuCauLoaiCauHoi = null,
            CancellationToken ct = default)
        {
            string cleanContent = string.IsNullOrWhiteSpace(noiDung) ? tieuDe : noiDung;
            if (cleanContent.Length > 8000)
                cleanContent = cleanContent.Substring(0, 8000);

            try
            {
                string normSkill = BoCauHoiBUS.ChuankHoaKyNang(loaiBaiHoc);
                string skillInstruction = GeminiAIService.GetSkillQuestionInstruction(normSkill);
                string levelInstruction = GeminiAIService.GetLevelInstruction(trinhDo);

                string typeInstruction = "";
                if (!string.IsNullOrWhiteSpace(yeuCauLoaiCauHoi) && !yeuCauLoaiCauHoi.Equals("Auto", StringComparison.OrdinalIgnoreCase))
                {
                    typeInstruction = $"YÊU CẦU ĐẶC BIỆT: Ưu tiên tạo 100% câu hỏi thuộc đúng dạng được chỉ định: '{yeuCauLoaiCauHoi}'.";
                }
                else
                {
                    typeInstruction = normSkill switch
                    {
                        "Nghe" => "TỰ ĐỘNG LỰA CHỌN CÁC DẠNG CÂU HỎI PHÙ HỢP KỸ NĂNG NGHE (LISTENING):\n" +
                                  "- Tạo kịch bản hội thoại/audio (Audio Script) hoàn chỉnh trong trường 'DoanVanDoc'.\n" +
                                  "- Phân bổ đa dạng các dạng:\n" +
                                  "  + 'TracNghiem' (Nghe và chọn đáp án A, B, C, D)\n" +
                                  "  + 'DungSai' (True/False - Xác định thông tin đúng/sai theo bài nghe)\n" +
                                  "  + 'DienKhuyet' (Information Completion - Điền tên, thời gian, địa điểm, số liệu còn thiếu, có 'DapAnVanBan')\n" +
                                  "  + 'TraLoiNgan' (Short Answer - Nghe và trả lời ngắn 1-3 từ, có 'DapAnVanBan')",
                        "Doc" => "TỰ ĐỘNG LỰA CHỌN CÁC DẠNG CÂU HỎI PHÙ HỢP KỸ NĂNG ĐỌC (READING):\n" +
                                 "- Trích xuất đoạn văn đọc hiểu hoàn chỉnh (Academic Passage) trong 'DoanVanDoc'.\n" +
                                 "- Phân bổ đa dạng các dạng:\n" +
                                 "  + 'TracNghiem' (Ý chính Main Idea, suy luận Inference, từ vựng theo ngữ cảnh)\n" +
                                 "  + 'TrueFalseNotGiven' (True / False / Not Given - Phân biệt Đúng, Sai, Không đề cập)\n" +
                                 "  + 'TraLoiNgan' (Short Answer - Trả lời ngắn thông tin trích xuất từ bài đọc)\n" +
                                 "  + 'DienKhuyet' (Sentence Completion - Hoàn thành câu theo bài đọc, có 'DapAnVanBan')",
                        "Viet" => "TỰ ĐỘNG LỰA CHỌN CÁC DẠNG BÀI TẬP PHÙ HỢP KỸ NĂNG VIẾT (WRITING):\n" +
                                  "- Phân bổ đa dạng các dạng:\n" +
                                  "  + 'VietLaiCau' (Sentence Transformation - Viết lại câu giữ nguyên nghĩa, có 'DapAnVanBan')\n" +
                                  "  + 'SuaLoiSai' (Error Correction - Tìm và sửa lỗi sai trong câu, có 'DapAnVanBan')\n" +
                                  "  + 'TuLuan' (Paragraph Writing hoặc Essay Writing - Yêu cầu viết đoạn văn hoặc bài luận hoàn chỉnh, kèm giới hạn số từ và tiêu chí chấm trong 'TieuChiCham')",
                        "Noi" => "TỰ ĐỘNG LỰA CHỌN DẠNG BÀI PHÙ HỢP KỸ NĂNG NÓI (SPEAKING):\n" +
                                 "- Kịch bản đối thoại phản xạ, phát âm trọng âm, nối âm chuẩn IPA và câu trả lời mở ('TraLoiNgan' / 'TracNghiem').",
                        _ => "TỰ ĐỘNG KẾT HỢP ĐA DẠNG: 'TracNghiem' (Trắc nghiệm 4 lựa chọn), 'DienKhuyet' (Điền từ khuyết), 'SuaLoiSai' (Tìm & sửa lỗi sai), 'VietLaiCau' (Viết lại câu)."
                    };
                }

                string prompt = $@"Bạn là chuyên gia khảo thí và sư phạm tiếng Anh hàng đầu chuẩn quốc tế (CEFR / Cambridge / IELTS).
Nhiệm vụ của bạn là phân tích tài liệu bài học sau và tạo ra đúng {soLuong} câu hỏi / bài tập tiếng Anh chất lượng cao đạt chuẩn khảo thí, phù hợp với kỹ năng, cấp độ và nội dung bài học.

THÔNG TIN BÀI HỌC:
- Tiêu đề: {tieuDe}
- Trình độ CEFR: {trinhDo} ({levelInstruction})
- Kỹ năng trọng tâm: {normSkill}

NỘI DUNG TÀI LIỆU CẦN RA ĐỀ:
{cleanContent}

YÊU CẦU THEO KỸ NĂNG:
{skillInstruction}

ĐỊNH HƯỚNG DẠNG CÂU HỎI THÔNG MINH:
{typeInstruction}

QUY TẮC BẮT BUỘC ĐỂ ĐA DẠNG HÓA CÂU HỎI:
1. ĐA DẠNG HÓA LOẠI CÂU HỎI ('LoaiCauHoi'):
   - 'TracNghiem': Trắc nghiệm 4 phương án A, B, C, D (DapAnDungIndex từ 0 đến 3).
   - 'DungSai': Đúng / Sai dựa trên nội dung (DanhSachLuaChon: [""A. True"", ""B. False""], DapAnDungIndex: 0 hoặc 1).
   - 'TrueFalseNotGiven': True / False / Not Given (DanhSachLuaChon: [""A. True"", ""B. False"", ""C. Not Given""], DapAnDungIndex: 0, 1 hoặc 2).
   - 'DienKhuyet': Điền từ khuyết vào chỗ trống. Cung cấp câu hỏi có chỗ trống (ví dụ: 'The manager decided to ______ the deadline.') và điền đáp án chuẩn vào trường 'DapAnVanBan'.
   - 'TraLoiNgan': Trả lời ngắn bằng 1 từ hoặc cụm từ ngắn. Điền đáp án chuẩn vào trường 'DapAnVanBan'.
   - 'VietLaiCau': Viết lại câu giữ nguyên nghĩa. Điền câu viết lại chuẩn vào trường 'DapAnVanBan'.
   - 'SuaLoiSai': Tìm và sửa lỗi sai trong câu. Điền phần sửa chuẩn vào trường 'DapAnVanBan'.
   - 'TuLuan': Đề bài viết đoạn văn / bài luận. Điền tiêu chí chấm (Rubric: Task Achievement, Coherence, Vocabulary, Grammar) và số từ tối thiểu vào trường 'TieuChiCham'.
2. ĐỘ KHÓ PHÂN BỔ THEO CEFR {trinhDo}:
   - ~20-30% câu Dễ ('De')
   - ~50-60% câu Trung bình ('TrungBinh')
   - ~20% câu Khó phân hóa ('Kho')
3. NGUYÊN TẮC GIẢI THÍCH VÀ DẪN CHỨNG BẮT BUỘC ('GiaiThichChiTiet'):
   - Nếu câu hỏi có dẫn chứng trực tiếp từ tài liệu: ghi rõ ""➔ Dẫn chứng bài học: '[Trích dẫn câu]' - Giải thích: [Giải thích]"".
   - Nếu là câu hỏi luyện tập / vận dụng kiến thức mở rộng: ghi rõ ""➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: [Giải thích]"".
4. NẾU KỸ NĂNG LÀ ĐỌC HOẶC NGHE:
   - Bắt buộc điền bài đọc hoàn chỉnh hoặc kịch bản audio thoại vào trường 'DoanVanDoc'.
5. Trả về DUY NHẤT một mảng JSON thuần túy (không kèm markdown văn bản bên ngoài):
[
  {{
    ""NoiDungCauHoi"": ""Nội dung câu hỏi / yêu cầu bài tập"",
    ""LoaiCauHoi"": ""TracNghiem"" (hoặc ""DungSai"", ""TrueFalseNotGiven"", ""DienKhuyet"", ""TraLoiNgan"", ""VietLaiCau"", ""SuaLoiSai"", ""TuLuan""),
    ""DanhSachLuaChon"": [""A. ..."", ""B. ..."", ""C. ..."", ""D. ...""],
    ""DapAnDungIndex"": 0,
    ""DapAnVanBan"": ""Đáp án chữ chính xác cho dạng điền từ / trả lời ngắn / viết lại / sửa lỗi (nếu có)"",
    ""TieuChiCham"": ""Tiêu chí chấm và số từ tối thiểu nếu là tự luận"",
    ""GiaiThichChiTiet"": ""➔ Dẫn chứng bài học: '...' - Giải thích: ..."",
    ""KyNang"": ""{normSkill}"",
    ""DoKho"": ""TrungBinh"",
    ""DoanVanDoc"": ""Đoạn văn đọc hiểu hoặc kịch bản audio thoại""
  }}
]";

                var response = await ExecutePromptAsync(prompt, preferredProvider, preferredModel, "Tạo câu hỏi bằng AI", ct);
                if (response.Success && !string.IsNullOrWhiteSpace(response.Text))
                {
                    string cleanJson = response.Text.Trim();
                    if (cleanJson.StartsWith("```json", StringComparison.OrdinalIgnoreCase)) cleanJson = cleanJson.Substring(7);
                    else if (cleanJson.StartsWith("```")) cleanJson = cleanJson.Substring(3);
                    if (cleanJson.EndsWith("```")) cleanJson = cleanJson.Substring(0, cleanJson.Length - 3);
                    cleanJson = cleanJson.Trim();

                    int firstBracket = cleanJson.IndexOf('[');
                    int lastBracket = cleanJson.LastIndexOf(']');
                    if (firstBracket >= 0 && lastBracket > firstBracket)
                    {
                        cleanJson = cleanJson.Substring(firstBracket, lastBracket - firstBracket + 1);
                    }

                    var questions = JsonSerializer.Deserialize<List<CauHoiDTO>>(cleanJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    });

                    if (questions != null && questions.Count > 0)
                    {
                        string passage = BoCauHoiBUS.TrichXuatDoanVanDocHieu(cleanContent);
                        for (int i = 0; i < questions.Count; i++)
                        {
                            var q = questions[i];
                            q.DanhSachLuaChon ??= new List<string>();
                            if (string.IsNullOrWhiteSpace(q.KyNang)) q.KyNang = normSkill;
                            if (string.IsNullOrWhiteSpace(q.DoKho)) q.DoKho = i < 2 ? "De" : (i >= 8 ? "Kho" : "TrungBinh");
                            if (string.IsNullOrWhiteSpace(q.LoaiCauHoi)) q.LoaiCauHoi = "TracNghiem";
                            if (string.IsNullOrWhiteSpace(q.DoanVanDoc) && (q.KyNang == "Doc" || normSkill == "Doc" || q.KyNang == "Nghe" || normSkill == "Nghe"))
                            {
                                q.DoanVanDoc = passage;
                            }
                        }
                        return questions;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AIServiceManager.BocTachCauHoiAsync] Lỗi: {ex.Message}");
            }

            // Fallback thông minh nội bộ
            return GeminiAIService.TaoCauHoiThongMinhTheoNoiDung(tieuDe, cleanContent, trinhDo, loaiBaiHoc, soLuong);
        }

        /// <summary>
        /// Kiểm tra trạng thái hoạt động thực tế của tất cả các AI Providers
        /// </summary>
        public async Task<List<AIHealthStatus>> CheckAllProvidersHealthAsync(CancellationToken ct = default)
        {
            var tasks = _providers.Values.Select(p => p.CheckHealthAsync(ct)).ToList();
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }
    }
}
