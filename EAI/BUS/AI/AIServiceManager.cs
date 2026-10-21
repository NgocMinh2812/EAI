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

                string prompt = $@"Bạn là chuyên gia khảo thí và sư phạm tiếng Anh hàng đầu chuẩn quốc tế (CEFR / Cambridge / IELTS).
Nhiệm vụ của bạn là phân tích tài liệu bài học sau và tạo ra đúng {soLuong} câu hỏi trắc nghiệm tiếng Anh 4 lựa chọn (A, B, C, D) chất lượng cao đạt chuẩn khảo thí.

THÔNG TIN BÀI HỌC:
- Tiêu đề: {tieuDe}
- Trình độ CEFR: {trinhDo} ({levelInstruction})
- Kỹ năng trọng tâm: {normSkill}

NỘI DUNG TÀI LIỆU CẦN RA ĐỀ:
{cleanContent}

YÊU CẦU THEO KỸ NĂNG:
{skillInstruction}

QUY TẮC BẮT BUỘC ĐỂ ĐA DẠNG HÓA CÂU HỎI:
1. ĐA DẠNG HÓA LOẠI CÂU HỎI VÀ GÓC ĐỘ ĐÁNH GIÁ:
   - TUYỆT ĐỐI KHÔNG tạo toàn bộ câu hỏi về nghĩa từ vựng.
   - Phải phân bổ đa dạng qua các khía cạnh:
     + Ý chính bài học / bối cảnh tổng quan (Main Idea / Context)
     + Dẫn chứng và thông tin chi tiết (Factual Details & Evidence)
     + Từ vựng và Cụm từ kết hợp tự nhiên trong ngữ cảnh (Vocabulary & Collocations in context)
     + Ngữ pháp, cấu trúc câu hoặc chia thì (Grammar in context / Sentence structure)
     + Suy luận logic và thái độ/mục đích của tác giả/người nói (Inference & Author's Tone)
   - Đa dạng loại câu hỏi ('LoaiCauHoi'): 'TracNghiem' (Trắc nghiệm chuẩn), 'DienTu' (Điền từ khuyết), 'TimLoiSai' (Tìm lỗi sai trong câu), 'HoanThanhCau' (Hoàn thành câu).
2. ĐỘ KHÓ PHÂN BỔ THEO CEFR {trinhDo}:
   - ~20-30% câu Dễ ('De')
   - ~50-60% câu Trung bình ('TrungBinh')
   - ~20% câu Khó phân hóa ('Kho')
3. NẾU KỸ NĂNG LÀ ĐỌC HIỂU ('Doc'):
   - Bắt buộc trích xuất đoạn văn đọc hiểu hoặc ngữ cảnh cụ thể từ tài liệu vào trường 'DoanVanDoc'.
4. MỖI CÂU HỎI PHẢI CÓ 4 PHƯƠNG ÁN TRẢ LỜI ĐỘC LẬP (A, B, C, D):
   - Không được lặp lại cùng các phương án giữa các câu hỏi khác nhau.
   - Luôn có đúng 1 đáp án đúng duy nhất (DapAnDungIndex: 0 cho A, 1 cho B, 2 cho C, 3 cho D).
5. Trả về DUY NHẤT một mảng JSON thuần túy (không kèm markdown văn bản bên ngoài):
[
  {{
    ""NoiDungCauHoi"": ""Question 1: ..."",
    ""DanhSachLuaChon"": [""A. ..."", ""B. ..."", ""C. ..."", ""D. ...""],
    ""DapAnDungIndex"": 0,
    ""GiaiThichChiTiet"": ""Giải thích chi tiết kèm trích dẫn văn bản..."",
    ""KyNang"": ""{normSkill}"",
    ""DoKho"": ""TrungBinh"",
    ""LoaiCauHoi"": ""TracNghiem"",
    ""DoanVanDoc"": ""Đoạn văn đọc hiểu trích xuất từ bài học (nếu là kỹ năng Đọc hiểu)""
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

                    var questions = JsonSerializer.Deserialize<List<CauHoiDTO>>(cleanJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (questions != null && questions.Count > 0)
                    {
                        string passage = BoCauHoiBUS.TrichXuatDoanVanDocHieu(cleanContent);
                        for (int i = 0; i < questions.Count; i++)
                        {
                            var q = questions[i];
                            if (string.IsNullOrWhiteSpace(q.KyNang)) q.KyNang = normSkill;
                            if (string.IsNullOrWhiteSpace(q.DoKho)) q.DoKho = i < 2 ? "De" : (i >= 8 ? "Kho" : "TrungBinh");
                            if (string.IsNullOrWhiteSpace(q.LoaiCauHoi)) q.LoaiCauHoi = "TracNghiem";
                            if (string.IsNullOrWhiteSpace(q.DoanVanDoc) && (q.KyNang == "Doc" || normSkill == "Doc"))
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
            return GeminiAIService.TaoCauHoiThongMinhTheoNoiDung(tieuDe, cleanContent, trinhDo, loaiBaiHoc);
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
