using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTO;

namespace BUS
{
    public class GeminiAIService
    {
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(25) };
        private static readonly ConcurrentDictionary<string, string> _cachedApiKeys = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// <summary>
        /// Đọc API Key Gemini cho từng module chuyên biệt (ví dụ: "NoiDungHocTap", "LuyenNoi", v.v.)
        /// từ .NET User Secrets hoặc biến môi trường hệ thống thông qua AIConfigHelper.
        /// Tuyệt đối không đọc từ file secrets.json trong repository!
        /// </summary>
        public static string GetApiKey(string module = "NoiDungHocTap")
        {
            if (string.IsNullOrWhiteSpace(module))
                module = "NoiDungHocTap";

            if (_cachedApiKeys.TryGetValue(module, out var cachedKey) && !string.IsNullOrWhiteSpace(cachedKey))
                return cachedKey;

            string key = BUS.AI.Configuration.AIConfigHelper.GetGeminiKey(module);
            if (!string.IsNullOrWhiteSpace(key))
            {
                _cachedApiKeys[module] = key;
                return key;
            }

            return string.Empty;
        }

        /// <summary>
        /// Đọc API Key Gemini chung từ .NET User Secrets / biến môi trường làm dự phòng
        /// </summary>
        public static string LayApiKeyChung()
        {
            if (_cachedApiKeys.TryGetValue("__GENERAL_APIKEY__", out var cached) && !string.IsNullOrWhiteSpace(cached))
                return cached;

            string key = BUS.AI.Configuration.AIConfigHelper.GetGeminiKey();
            if (!string.IsNullOrWhiteSpace(key))
            {
                _cachedApiKeys["__GENERAL_APIKEY__"] = key;
                return key;
            }

            return string.Empty;
        }

        /// <summary>
        /// Lấy danh sách các API Key khả dụng (ưu tiên key module chuyên biệt, tiếp theo là key chung ApiKey)
        /// </summary>
        public static List<string> LayDanhSachApiKeyKhaDung(string module = "NoiDungHocTap")
        {
            var keys = new List<string>();
            string primaryKey = GetApiKey(module);
            if (!string.IsNullOrWhiteSpace(primaryKey))
            {
                keys.Add(primaryKey);
            }

            string generalKey = LayApiKeyChung();
            if (!string.IsNullOrWhiteSpace(generalKey) && !keys.Contains(generalKey))
            {
                keys.Add(generalKey);
            }

            return keys;
        }

        /// <summary>
        /// Gọi Google Gemini API với cơ chế Fallback Model Chain (thử lần lượt gemini-3.5-flash, gemini-3.6-flash, gemini-3.1-flash-lite, v.v.)
        /// và tự động đảo sang API Key dự phòng nếu key đầu tiên bị lỗi quota hoặc quyền truy cập.
        /// </summary>
        public static async Task<string?> GoiGeminiApiAsync(string prompt, string module = "NoiDungHocTap")
        {
            // ƯU TIÊN SỐ 1: GỌI QUA CENTRALIZED BACKEND EAI.AIBackend.API
            if (AI.Configuration.AIConfigHelper.IsUseCentralizedBackend())
            {
                try
                {
                    string? beReply = await AI.EAIBackendClient.ChatAsync(prompt);
                    if (!string.IsNullOrWhiteSpace(beReply))
                    {
                        return beReply;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GeminiAIService.GoiGeminiApiAsync] Centralized Backend gặp lỗi hoặc offline: {ex.Message}. Fallback về Gemini nội bộ...");
                }
            }

            var keys = LayDanhSachApiKeyKhaDung(module);
            if (keys.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GeminiAIService] Không tìm thấy API Key nào cho module '{module}'.");
                return null;
            }

            // Đồng bộ mô hình được ưu tiên từ cấu hình
            var candidateModels = new List<string>();
            if (AI.AIServiceManager.Instance.ActiveProviderType == AI.Models.AIProviderType.Gemini 
                && !string.IsNullOrWhiteSpace(AI.AIServiceManager.Instance.ActiveModel))
            {
                candidateModels.Add(AI.AIServiceManager.Instance.ActiveModel.Trim());
            }

            string[] defaultModels = new[]
            {
                "gemini-2.0-flash",
                "gemini-1.5-flash",
                "gemini-1.5-flash-8b",
                "gemini-1.5-pro",
                "gemini-flash-latest"
            };

            foreach (var m in defaultModels)
            {
                if (!candidateModels.Contains(m))
                    candidateModels.Add(m);
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
                }
            };

            string requestJson = JsonSerializer.Serialize(requestObj);

            foreach (var apiKey in keys)
            {
                foreach (var model in candidateModels)
                {
                    try
                    {
                        string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
                        using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(12));
                        using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                        var response = await _httpClient.PostAsync(endpoint, content, cts.Token);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            using var doc = JsonDocument.Parse(responseBody);
                            if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                            {
                                string text = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? "";
                                if (!string.IsNullOrWhiteSpace(text))
                                {
                                    System.Diagnostics.Debug.WriteLine($"[GeminiAIService] Phản hồi thành công từ model '{model}'!");
                                    return text;
                                }
                            }
                        }
                        else
                        {
                            int statusCode = (int)response.StatusCode;
                            string errBody = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine($"[GeminiAIService] Model '{model}' trả về HTTP {statusCode} ({response.ReasonPhrase}): {errBody}");

                            // Nếu lỗi 403 hoặc 429 (hết quota / lỗi key), chuyển sang key kế tiếp ngay
                            if (statusCode == 403 || statusCode == 429)
                            {
                                break;
                            }
                            // Nếu lỗi 503 (quá tải) hoặc 404 hoặc 500, tiếp tục thử model tiếp theo trong danh sách
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        System.Diagnostics.Debug.WriteLine($"[GeminiAIService] Model '{model}' phản hồi quá 12s, chuyển sang model dự phòng...");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[GeminiAIService] Lỗi kết nối tới model '{model}': {ex.Message}");
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gọi hệ thống AI để bóc tách câu hỏi trắc nghiệm trực tiếp từ văn bản học liệu
        /// theo sát trình độ (A1 - C1) và kỹ năng (Ngữ pháp, Từ vựng, Đọc hiểu, Nghe hiểu, Viết luận)
        /// </summary>
        public static async Task<List<CauHoiDTO>> BocTachCauHoiAIAsync(
            string tieuDe, 
            string noiDung, 
            string trinhDo = "B1", 
            int soLuong = 10, 
            string module = "NoiDungHocTap", 
            string loaiBaiHoc = "",
            string? yeuCauLoaiCauHoi = null)
        {
            // Defensive: phát hiện nếu người gọi truyền tên kỹ năng vào tham số module
            if (string.IsNullOrWhiteSpace(loaiBaiHoc) && !string.IsNullOrWhiteSpace(module) &&
                !module.Equals("NoiDungHocTap", StringComparison.OrdinalIgnoreCase) &&
                !module.Equals("DanhMuc", StringComparison.OrdinalIgnoreCase) &&
                !module.Equals("DanhMucCauTruc", StringComparison.OrdinalIgnoreCase) &&
                !module.StartsWith("AI_", StringComparison.OrdinalIgnoreCase))
            {
                loaiBaiHoc = module;
            }

            return await AI.AIServiceManager.Instance.BocTachCauHoiAsync(tieuDe, noiDung, trinhDo, soLuong, loaiBaiHoc, yeuCauLoaiCauHoi: yeuCauLoaiCauHoi);
        }

        /// <summary>
        /// Gọi hệ thống AI để sinh nội dung giáo trình mẫu (Text Sample) cho bài học
        /// phụ thuộc chính xác vào Cấp độ trình độ (A1 - C1) và Kỹ năng (Ngữ pháp, Từ vựng, Đọc hiểu, Nghe hiểu, Viết luận)
        /// </summary>
        public static async Task<string> SinhNoiDungMauAIAsync(
            string tieuDe, 
            string trinhDo = "B1", 
            string loaiBaiHoc = "Doc", 
            string module = "NoiDungHocTap")
        {
            return await AI.AIServiceManager.Instance.SinhNoiDungMauAsync(tieuDe, trinhDo, loaiBaiHoc);
        }

        public static string GetLevelInstruction(string trinhDo)
        {
            return (trinhDo ?? "B1").ToUpperInvariant() switch
            {
                "A1" => "Trình độ CEFR A1 (Cơ bản): Câu ngắn gọn (5-8 từ), từ vựng quen thuộc đời sống, thì hiện tại đơn hoặc quá khứ đơn giản, cấu trúc rõ ràng không bẫy phức tạp.",
                "A2" => "Trình độ CEFR A2 (Sơ cấp): Câu ghép quen thuộc (and, but, because, so), giao tiếp thường nhật và công việc cơ bản, từ vựng thông dụng.",
                "B1" => "Trình độ CEFR B1 (Trung cấp): Câu phức với mệnh đề quan hệ, hiện tại hoàn thành, câu điều kiện loại 1/2, từ vựng công sở và giao tiếp chuyên môn thường nhật.",
                "B2" => "Trình độ CEFR B2 (Trung cao cấp): Câu phức nhiều mệnh đề, thể bị động, collocations tự nhiên, từ vựng chuyên ngành sâu và sắc thái biểu đạt đa dạng.",
                "C1" => "Trình độ CEFR C1 (Cao cấp): Văn phong học thuật / chuyên nghiệp cao cấp, cấu trúc ngữ pháp đảo ngữ hoặc phức hợp, từ vựng phong phú và phân tích chuyên sâu.",
                _ => $"Trình độ chuẩn CEFR: {trinhDo}"
            };
        }

        public static string GetSkillQuestionInstruction(string loaiBaiHoc)
        {
            string loaiLower = (loaiBaiHoc ?? "").ToLowerInvariant();

            if (loaiLower.Contains("nguphap") || loaiLower.Contains("grammar"))
                return "- Trọng tâm câu hỏi Ngữ pháp (Grammar): Đa dạng hóa các dạng câu hỏi: chia thì động từ (Tenses & Verb Forms), tìm lỗi sai ngữ pháp trong câu ('TimLoiSai'), cấu trúc câu điều kiện / bị động / mệnh đề quan hệ ('HoanThanhCau'), điền liên từ hoặc giới từ chuẩn xác ('DienTu'). Bám sát văn cảnh bài học.";

            if (loaiLower.Contains("tuvung") || loaiLower.Contains("vocab"))
                return "- Trọng tâm câu hỏi Từ vựng (Vocabulary): KHÔNG tạo 100% câu hỏi định nghĩa từ thuần túy. Phải đa dạng: nghĩa từ trong ngữ cảnh văn bản, cụm kết hợp từ tự nhiên (Collocations & Phrasal Verbs), họ từ / từ loại (Word Family: Noun/Verb/Adj/Adv), từ đồng nghĩa / trái nghĩa (Synonyms/Antonyms), và hoàn thành câu tình huống thực tế ('HoanThanhCau' / 'DienTu').";

            if (loaiLower.Contains("doc") || loaiLower.Contains("reading"))
                return "- Trọng tâm câu hỏi Đọc hiểu (Reading Comprehension): Bắt buộc ra đề đọc hiểu toàn diện chuẩn quốc tế: câu hỏi ý chính / tiêu đề (Main Idea), chi tiết quan trọng và dẫn chứng (Factual Detail), từ vựng trong ngữ cảnh (Vocabulary in Context), suy luận logic (Inference), thái độ / mục đích của tác giả (Author's Tone & Purpose). Bắt buộc trích xuất đoạn văn đọc hiểu tương ứng vào trường 'DoanVanDoc'.";

            if (loaiLower.Contains("nghe") || loaiLower.Contains("listen"))
                return "- Trọng tâm câu hỏi Nghe hiểu (Listening Comprehension): Bắt buộc kiểm tra kỹ năng nghe toàn diện: bối cảnh tình huống hội thoại (Setting/Purpose), chi tiết thông tin các nhân vật trao đổi (Speaker Details), thái độ / cảm xúc của người nói (Speaker Attitude/Feelings), quyết định hoặc hành động tiếp theo (Outcome/Action).";

            if (loaiLower.Contains("viet") || loaiLower.Contains("writing"))
                return "- Trọng tâm câu hỏi Viết luận (Writing): Kiểm tra cách sử dụng liên từ nối câu (Transition words/Cohesive devices), cấu trúc câu mạch lạc, phát hiện lỗi diễn đạt và phong cách viết học thuật.";

            return "- Trọng tâm câu hỏi: Đa dạng hóa các dạng câu hỏi và khía cạnh đánh giá (ý chính, chi tiết, từ vựng, ngữ pháp, suy luận), không lặp lại một mẫu câu hỏi từ vựng đơn điệu.";
        }

        /// <summary>
        /// Xây dựng prompt chi tiết cho Gemini AI sinh giáo trình bài học theo đúng Trình độ và Kỹ năng
        /// </summary>
        public static string BuildSamplePrompt(string tieuDe, string trinhDo, string loaiBaiHoc)
        {
            string levelDesc = GetLevelInstruction(trinhDo);
            string loaiLower = (loaiBaiHoc ?? "").ToLowerInvariant();
            string skillFormat = "";

            if (loaiLower.Contains("nguphap") || loaiLower.Contains("grammar"))
            {
                skillFormat = $@"YÊU CẦU ĐẶC BIỆT DÀNH CHO KỸ NĂNG NGỮ PHÁP (GRAMMAR):
1. Dòng đầu tiên: '■ NGỮ PHÁP CHUYÊN SÂU: {tieuDe} (Level {trinhDo})'
2. Phần 1 - TỔNG QUAN & NGUYÊN TẮC CỐT LÕI (Core Concept & Usage):
   - Phân tích bản chất, lý do sử dụng và ý nghĩa sắc thái ngữ pháp ở trình độ {trinhDo}.
3. Phần 2 - CÔNG THỨC & DẤU HIỆU NHẬN BIẾT (Form & Signal Words):
   - Bảng công thức: Khẳng định (+), Phủ định (-), Nghi vấn (?) kèm trợ động từ và dạng phân từ/chia động từ.
   - Các trạng từ / dấu hiệu thời gian đi kèm đặc trưng (Signal words / Time markers).
4. Phần 3 - 5 MẪU CÂU ỨNG DỤNG THỰC TẾ (Practical Examples with Analysis):
   - 5 câu ví dụ tiếng Anh thực tế trong môi trường học thuật/công sở chuẩn mức {trinhDo}.
   - In đậm từ khóa ngữ pháp, kèm dòng phân tích cấu trúc và dịch nghĩa tiếng Việt chi tiết.
5. Phần 4 - BẪY NGỮ PHÁP & LỖI SAI PHỔ BIẾN (Common Mistakes & Pitfalls):
   - Phân tích 2-3 lỗi sai kinh điển người học hay mắc và cách sửa chuẩn mực.
6. Phần 5 - BÀI TẬP ỨNG DỤNG NHANH & MẸO GHI NHỚ (Quick Practice & Pro Tip):
   - 2 câu bài tập chia động từ kèm đáp án ngắn gọn.
   - '💡 Master Tip: [Lời khuyên sư phạm giúp khắc sâu và phản xạ tự nhiên]'";
            }
            else if (loaiLower.Contains("tuvung") || loaiLower.Contains("vocab"))
            {
                skillFormat = $@"YÊU CẦU ĐẶC BIỆT DÀNH CHO KỸ NĂNG TỪ VỰNG (VOCABULARY):
1. Dòng đầu tiên: '■ BỘ TỪ VỰNG CHUYÊN SÂU: {tieuDe} (Level {trinhDo})'
2. Phần 1 - TỔNG QUAN CHỦ ĐỀ & GIÁ TRỊ ỨNG DỤNG (Thematic Context):
   - Giới thiệu 2-3 câu về tầm quan trọng của trường từ vựng này trong giao tiếp/học thuật thực tế.
3. Phần 2 - DANH MỤC 6-8 TỪ VỰNG & COLLOCATIONS TRỌNG TÂM:
   - Mỗi từ/cụm từ trình bày theo định dạng chuẩn từ điển:
     • [Từ/Cụm từ] (/Phiên âm IPA/) - [Từ loại]: [Nghĩa tiếng Việt chi tiết]
       - Collocations: [Các từ thường đi kèm, vd: verb + noun hoặc adj + noun]
       - Example: ""Câu ví dụ chuẩn văn phong bản ngữ ngữ cảnh thực tế.""
4. Phần 3 - ĐOẠN HỘI THOẠI ỨNG DỤNG NGỮ CẢNH (Contextual Dialogue):
   - Một đoạn hội thoại hoặc tình huống thực tế (6-8 lượt thoại) lồng ghép tự nhiên các từ vựng vừa học.
5. Phần 4 - MẸO GHI NHỚ & GIA ĐÌNH TỪ (Word Family & Memory Hack):
   - Bảng biến đổi từ loại (Noun - Verb - Adj - Adv) của các từ chính.
   - '💡 Master Tip: [Phương pháp ghi nhớ qua ngữ cảnh hoặc liên tưởng hình ảnh]'";
            }
            else if (loaiLower.Contains("doc") || loaiLower.Contains("reading"))
            {
                skillFormat = $@"YÊU CẦU ĐẶC BIỆT DÀNH CHO KỸ NĂNG ĐỌC HIỂU (READING COMPREHENSION):
1. Dòng đầu tiên: '■ READING COMPREHENSION MASTERCLASS: {tieuDe} (Level {trinhDo})'
2. Phần 1 - BÀI ĐỌC HỌC THUẬT TIÊU CHUẨN (Reading Passage - khoảng 250 đến 350 từ):
   - Bài đọc hoàn chỉnh gồm 3 đến 4 đoạn văn chặt chẽ (Introduction, Body Paragraphs, Conclusion) về chủ đề {tieuDe}.
   - Ngôn từ, cấu trúc câu phức và học thuật đạt chuẩn mức {trinhDo}.
3. Phần 2 - TỪ VỰNG & CỤM TỪ HỌC THUẬT QUAN TRỌNG (Academic Vocabulary in Passage):
   - Trích xuất 5-6 từ/cụm từ đắt giá xuất hiện trong bài đọc kèm giải nghĩa và từ đồng nghĩa (Synonyms).
4. Phần 3 - PHÂN TÍCH BỐ CỤC & LUẬN ĐIỂM (Passage Analysis & Key Takeaways):
   - Tóm tắt ý chính của từng đoạn văn (Main idea of each paragraph).
5. Phần 4 - CÂU HỎI TỰ SUY NGẪM (Critical Thinking & Comprehension Questions):
   - 2-3 câu hỏi gợi mở để người đọc tự kiểm tra mức độ nắm bắt nội dung sâu sắc.";
            }
            else if (loaiLower.Contains("nghe") || loaiLower.Contains("listen"))
            {
                skillFormat = $@"YÊU CẦU ĐẶC BIỆT DÀNH CHO KỸ NĂNG NGHE HIỂU (LISTENING):
1. Dòng đầu tiên: '■ AUDIO SCRIPT & REAL-WORLD CONVERSATION: {tieuDe} (Level {trinhDo})'
2. Phần 1 - BỐI CẢNH TÌNH HUỐNG (Context & Audio Scene Setting):
   - Mô tả ngắn gọn bối cảnh giao tiếp, danh tính các nhân vật và mục tiêu thảo luận.
3. Phần 2 - KỊCH BẢN ĐỐI THOẠI CHI TIẾT (Comprehensive Audio Script - khoảng 8 đến 12 lượt thoại):
   - Cuộc trò chuyện tự nhiên, sinh động giữa 2-3 người bản xứ với ngữ điệu, phản hồi, thán từ chân thực theo chuẩn {trinhDo}.
4. Phần 3 - PHÂN TÍCH PHÁT ÂM & HIỆN TƯỢNG NỐI ÂM (Phonetics & Connected Speech):
   - Chỉ ra các hiện tượng nối âm (linking sounds), nuốt âm (elision), dạng yếu (weak forms) hoặc ngữ điệu lên xuống quan trọng.
5. Phần 4 - CỤM TỪ PHẢN XẠ BỎ TÚI (Key Conversational Takeaways):
   - 4-5 cụm câu phản xạ tức thì cực kỳ hữu ích học được từ đoạn hội thoại.";
            }
            else if (loaiLower.Contains("viet") || loaiLower.Contains("writing"))
            {
                skillFormat = $@"YÊU CẦU ĐẶC BIỆT DÀNH CHO KỸ NĂNG VIẾT LUẬN (WRITING MASTERCLASS):
1. Dòng đầu tiên: '■ MODEL WRITING & STRUCTURE ANALYSIS: {tieuDe} (Level {trinhDo})'
2. Phần 1 - ĐỀ BÀI & PHÂN TÍCH DÀN Ý (Writing Prompt & Outline Breakdown):
   - Nêu đề bài mẫu cụ thể (IELTS Writing Task 2 / Business Report / Official Correspondence).
   - Dàn ý chi tiết 4 đoạn: Mở bài (Introduction) - Thân bài 1 (Body 1) - Thân bài 2 (Body 2) - Kết bài (Conclusion).
3. Phần 2 - BÀI VIẾT MẪU HOÀN CHỈNH (Model Sample - khoảng 220 đến 300 từ):
   - Bài viết mẫu hoàn chỉnh xuất sắc, văn phong mạch lạc, sử dụng phương pháp PEEL (Point, Explanation, Evidence, Link).
4. Phần 3 - BẢNG TỪ NỐI & TỪ VỰNG NÂNG ĐIỂM (Cohesive Devices & Lexical Resources):
   - Bảng tổng hợp liên từ chuyển ý và các cụm từ học thuật/chuyên nghiệp ăn điểm cao được dùng trong bài.
5. Phần 4 - LƯU Ý SƯ PHẠM ĐỂ TỐI ĐA HÓA ĐIỂM SỐ (Examiner's Tips):
   - Lời khuyên của chuyên gia về cách tránh lạc đề, phát triển ý sâu và kiểm soát ngữ pháp.";
            }
            else
            {
                skillFormat = $@"YÊU CẦU ĐẶC BIỆT DÀNH CHO GIAO TIẾP VÀ HỌC LIỆU TỔNG HỢP:
1. Dòng đầu tiên: '■ GIÁO TRÌNH HỌC TẬP CHUYÊN SÂU: {tieuDe} (Level {trinhDo})'
2. Phần 1 - GIỚI THIỆU CHỦ ĐỀ & MỤC TIÊU HỌC TẬP: Nêu rõ mục tiêu cần đạt được sau bài học.
3. Phần 2 - NỘI DUNG LÝ THUYẾT VÀ BỘ MẪU CÂU TIÊU CHUẨN: 6-8 mẫu câu thực tế chuẩn bản xứ kèm phân tích.
4. Phần 3 - TÌNH HUỐNG THỰC HÀNH TỔNG HỢP: Tình huống hội thoại hoặc mô phỏng công việc thực tế.
5. Phần 4 - TỔNG KẾT & LỜI KHUYÊN PHÁT TRIỂN KỸ NĂNG:
   '💡 Master Hint: [Lời khuyên thực hành phản xạ tự nhiên]'";
            }

            return $@"Bạn là trợ lý AI chuyên gia giáo dục và biên soạn giáo trình tiếng Anh hàng đầu.
Hãy tạo một bộ tài liệu / giáo trình lý thuyết mẫu (Text Sample) CHUYÊN SÂU, ĐẦY ĐỦ VÀ TOÀN DIỆN cho bài học sau:

TIÊU CHUẨN ĐẦU VÀO:
- Tiêu đề bài học: {tieuDe}
- Khung trình độ CEFR: {trinhDo} ({levelDesc})
- Kỹ năng phân loại: {loaiBaiHoc}

{skillFormat}

QUY TẮC BẮT BUỘC:
- DUNG LƯỢNG NỘI DUNG: Khoảng 350 - 500 từ. TUYỆT ĐỐI KHÔNG VIẾT NGẮN GỌN SƠ SÀI. Nội dung cần đầy đặn, chia các phần rõ ràng, giàu kiến thức và giá trị sư phạm thực tế.
- Trả về TRỰC TIẾP văn bản kết quả (KHÔNG bọc trong markdown code block ```, KHÔNG thêm lời chào mở đầu hay kết thúc).
- Ngôn ngữ: Các phần bài đọc, mẫu câu, từ vựng, kịch bản hội thoại là tiếng Anh; các phần giải thích, công thức, mẹo sư phạm bằng tiếng Việt chuẩn mực, dễ hiểu.";
        }

        /// <summary>
        /// Tạo nội dung mẫu thông minh theo ngữ cảnh bài học khi offline / hết quota
        /// Phụ thuộc chính xác vào Trình độ và Kỹ năng (Dung lượng 350 - 500 từ chuẩn mực)
        /// </summary>
        public static string TaoNoiDungMauThongMinh(string tieuDe, string trinhDo = "B1", string loaiBaiHoc = "Doc")
        {
            string loaiLower = (loaiBaiHoc ?? "").ToLowerInvariant();

            if (loaiLower.Contains("nguphap") || loaiLower.Contains("grammar"))
            {
                return $"■ NGỮ PHÁP CHUYÊN SÂU: {tieuDe} (CEFR Level {trinhDo})\n\n" +
                       "1. TỔNG QUAN & NGUYÊN TẮC CỐT LÕI (Core Concept):\n" +
                       "Ngữ pháp đóng vai trò làm khung xương sống định hình tư duy biểu đạt chuẩn xác. Trong giao tiếp học thuật và công việc quốc tế, việc làm chủ các cấu trúc động từ, thì hoàn thành và thể bị động giúp người học truyền đạt thông điệp rõ ràng, tránh gây hiểu lầm và thể hiện sự chuyên nghiệp cao độ.\n\n" +
                       "2. CÔNG THỨC & QUY TẮC CỐT LÕI (Form & Syntax):\n" +
                       "   • Khẳng định (+): Subject + have/has + Past Participle (V3/ed) + Object / Complement\n" +
                       "   • Phủ định (-): Subject + have/has + not (haven't / hasn't) + Past Participle (V3/ed)\n" +
                       "   • Nghi vấn (?): (Wh-word) + have/has + Subject + Past Participle (V3/ed)?\n" +
                       "   • Dấu hiệu nhận biết trọng tâm: already, yet, just, recently, since (mốc thời gian cụ thể), for (khoảng thời gian tích lũy).\n\n" +
                       "3. CÁC MẪU CÂU ỨNG DỤNG THỰC TẾ TRONG DOANH NGHIỆP (Practical Workplace Examples):\n" +
                       "   • \"Our engineering division has successfully finalized all critical deliverables ahead of the quarterly deadline.\"\n" +
                       "     ➔ Giải thích: Nhấn mạnh thành tựu công việc đã hoàn thành và kết quả tích cực vẫn còn nguyên vẹn ở thời điểm hiện tại.\n" +
                       "   • \"Have you reviewed the pull request for the authentication module yet?\"\n" +
                       "     ➔ Giải thích: Dùng 'yet' ở cuối câu hỏi để xác nhận một hành động được mong đợi hoàn thành.\n" +
                       "   • \"The management team has not approved the budget adjustment since last month's review.\"\n" +
                       "     ➔ Giải thích: Dùng 'since' chỉ mốc thời gian bắt đầu của trạng thái chưa phê duyệt.\n" +
                       "   • \"We have already integrated the payment gateway with automated unit tests.\"\n" +
                       "     ➔ Giải thích: Trạng từ 'already' đặt giữa trợ động từ và phân từ hai để nhấn mạnh việc đã hoàn tất sớm.\n" +
                       "   • \"She has served as the lead project coordinator for over four years.\"\n" +
                       "     ➔ Giải thích: Dùng 'for' để đo lường độ dài thời gian của kinh nghiệm tích lũy.\n\n" +
                       "4. CÁC BẪY NGỮ PHÁP & LỖI SAI PHỔ BIẾN (Common Pitfalls):\n" +
                       "   ⚠️ Bẫy 1: Dùng thì Hiện tại hoàn thành với thời gian quá khứ xác định (vd: *I have sent the email yesterday* ➔ SAI. Sửa: *I sent the email yesterday*).\n" +
                       "   ⚠️ Bẫy 2: Nhầm lẫn giữa 'since' và 'for' (*since two hours* ➔ SAI. Sửa: *for two hours*).\n\n" +
                       "💡 Master Tip: Luôn tự hỏi: 'Hành động này có thời gian quá khứ cụ thể không?' Nếu có ngày giờ cụ thể ➔ Past Simple; nếu kết quả/kinh nghiệm kéo dài đến hiện tại ➔ Present Perfect.";
            }

            if (loaiLower.Contains("tuvung") || loaiLower.Contains("vocab"))
            {
                return $"■ BỘ TỪ VỰNG CHUYÊN SÂU: {tieuDe} (CEFR Level {trinhDo})\n\n" +
                       "1. TỔNG QUAN CHỦ ĐỀ & TẦM QUAN TRỌNG (Thematic Context):\n" +
                       "Sở hữu vốn từ vựng chuyên ngành và các cụm từ kết hợp tự nhiên (Collocations) là chìa khóa then chốt để diễn đạt ý tưởng gãy gọn, tự tin thảo luận với các đối tác và đồng nghiệp quốc tế.\n\n" +
                       "2. DANH MỤC TỪ VỰNG & COLLOCATIONS TRỌNG TÂM (Key Vocabulary & Collocations):\n" +
                       "   • Deliverable (/dɪˈlɪv.ər.ə.bəl/) - [Noun]: Sản phẩm hoặc kết quả công việc phải bàn giao cho khách hàng.\n" +
                       "     - Collocation: produce deliverables, client deliverables, finalize deliverables.\n" +
                       "     - Example: \"All software deliverables must undergo rigorous QA testing prior to release.\"\n\n" +
                       "   • Milestone (/ˈmaɪl.stəʊn/) - [Noun]: Cột mốc quan trọng đánh dấu tiến trình dự án.\n" +
                       "     - Collocation: reach a milestone, critical milestone, project milestone.\n" +
                       "     - Example: \"Deploying the database migration represents a monumental milestone for our roadmap.\"\n\n" +
                       "   • Coordinate (/kəʊˈɔː.dɪ.neɪt/) - [Verb]: Điều phối, kết nối nhịp nhàng giữa các bên.\n" +
                       "     - Collocation: coordinate closely with, coordinate efforts, coordinate logistics.\n" +
                       "     - Example: \"The Scrum Master coordinates daily sync-ups across three global engineering teams.\"\n\n" +
                       "   • Cross-functional (/ˌkrɒsˈfʌŋk.ʃən.əl/) - [Adjective]: Liên phòng ban, đa chức năng.\n" +
                       "     - Collocation: cross-functional team, cross-functional collaboration.\n" +
                       "     - Example: \"We assembled a cross-functional taskforce to address security vulnerabilities.\"\n\n" +
                       "   • Bottleneck (/ˈbɒt.əl.nek/) - [Noun]: Điểm nghẽn gây tắc nghẽn toàn bộ quy trình.\n" +
                       "     - Collocation: eliminate bottlenecks, operational bottleneck, identify a bottleneck.\n" +
                       "     - Example: \"Code review delays proved to be the primary bottleneck in our deployment pipeline.\"\n\n" +
                       "3. ĐOẠN HỘI THOẠI ỨNG DỤNG NGỮ CẢNH (Contextual Dialogue):\n" +
                       "   Sarah: \"Good morning, Alex. Are we still on track to hit our sprint milestones this Friday?\"\n" +
                       "   Alex: \"Most modules are complete, but manual testing is currently creating an unexpected bottleneck.\"\n" +
                       "   Sarah: \"Understood. I will coordinate with the QA lead to expedite the verification process.\"\n" +
                       "   Alex: \"Excellent. Once that is cleared, all finalized deliverables will be ready for client review.\"\n\n" +
                       "💡 Master Tip: Hãy học từ theo cụm động từ + danh từ (verb-noun pairs). Không chỉ học 'milestone', hãy học 'reach/achieve a milestone'!";
            }

            if (loaiLower.Contains("nghe") || loaiLower.Contains("listen"))
            {
                return $"■ AUDIO SCRIPT & REAL-WORLD CONVERSATION: {tieuDe} (CEFR Level {trinhDo})\n\n" +
                       "1. BỐI CẢNH TÌNH HUỐNG (Context & Audio Scene Setting):\n" +
                       "Cuộc trao đổi chuyên môn giữa Trưởng nhóm kỹ thuật (David) và Chuyên viên phân tích kinh doanh (Emma) trong phòng họp dự án để giải quyết sự cố phát sinh trước ngày ra mắt sản phẩm.\n\n" +
                       "2. KỊCH BẢN ĐỐI THOẠI TOÀN DIỆN (Comprehensive Audio Script):\n" +
                       "   Emma: \"David, thanks for meeting on short notice. Have you had an opportunity to inspect the crash reports?\"\n" +
                       "   David: \"Yes, Emma. Our diagnostic logs indicate a memory leak occurred when concurrent users exceeded ten thousand.\"\n" +
                       "   Emma: \"That's concerning. Is this issue likely to postpone next Monday's enterprise rollout?\"\n" +
                       "   David: \"Not necessarily. If we push a hotfix to optimize the database query cache today, we can rerun load simulations by tonight.\"\n" +
                       "   Emma: \"Sounds like a viable workaround. Who will be leading the deployment of that hotfix?\"\n" +
                       "   David: \"Marcus and I will handle it directly. We'll post real-time updates in the deployment Slack channel.\"\n" +
                       "   Emma: \"Perfect. I will notify the stakeholders that the situation is contained and under control.\"\n" +
                       "   David: \"Appreciate it. Let's reconvene at four this afternoon to verify the load test outcome.\"\n\n" +
                       "3. PHÂN TÍCH PHÁT ÂM & HIỆN TƯỢNG NỐI ÂM (Phonetics & Connected Speech):\n" +
                       "   • Nối âm (Consonant to Vowel Linking): 'had_an_opportunity' (/hæd ən ˌɒp.əˈtjuː.nə.ti/), 'postpone_next' (nuốt âm /t/).\n" +
                       "   • Giảm âm (Weak Forms): 'for' (/fə/), 'can' (/kən/), 'to' (/tə/).\n\n" +
                       "🎧 Listening Focus: Tập trung nhận diện các từ khóa nội dung mang trọng âm câu (inspect, crash reports, memory leak, rollout, hotfix) để nắm bắt ý chính dù người nói phát âm với tốc độ nhanh.";
            }

            if (loaiLower.Contains("viet") || loaiLower.Contains("writing"))
            {
                return $"■ WRITING MASTERCLASS & ESSAY BREAKDOWN: {tieuDe} (CEFR Level {trinhDo})\n\n" +
                       "1. ĐỀ BÀI MẪU & PHÂN TÍCH YÊU CẦU (Writing Task Breakdown):\n" +
                       "Topic: \"Some professionals believe that daily asynchronous reporting is far superior to synchronous status meetings. To what extent do you agree or disagree?\"\n\n" +
                       "2. BÀI VIẾT MẪU ĐẠT CHUẨN CAO (Model Essay Sample - 260 words):\n" +
                       "In recent years, the paradigm of modern workplace communication has experienced a significant transition toward asynchronous methodologies. While traditional daily meetings facilitate spontaneous rapport, I strongly argue that structured asynchronous reporting provides superior productivity and cognitive focus for contemporary distributed organizations.\n\n" +
                       "First and foremost, asynchronous updates safeguard uninterrupted workflow. When software engineers and creative professionals are forced to attend mandatory synchronous stand-ups, their concentration is fragmented, resulting in substantial context-switching penalties. By contrast, submitting written summaries via collaborative platforms allows contributors to document their accomplishments and impediments during natural lulls in their daily workflow. Consequently, individuals preserve extended blocks of deep work essential for solving complex engineering challenges.\n\n" +
                       "Furthermore, written documentation inherently establishes institutional transparency. In multinational organizations spanning disparate time zones, verbal discussions often fail to reach all stakeholders effectively. A centralized written record, however, provides an immutable audit trail that team members can reference anytime. This accessibility minimizes redundant queries, clarifies project accountability, and ensures equitable participation regardless of geographic location.\n\n" +
                       "In conclusion, although face-to-face exchanges offer interpersonal warmth, asynchronous reporting distinctly outperforms traditional meetings in preserving deep concentration and delivering transparent alignment across modern teams.\n\n" +
                       "✍ Writing Tip: Áp dụng các liên từ chuyển tiếp mạch lạc (First and foremost, By contrast, Consequently, Furthermore, In conclusion) để đạt điểm tối đa tiêu chí Coherence and Cohesion.";
            }

            // Mặc định hoặc Đọc hiểu (Reading Comprehension)
            return $"■ READING COMPREHENSION MASTERCLASS: {tieuDe} (CEFR Level {trinhDo})\n\n" +
                   "1. BÀI ĐỌC HỌC THUẬT TIÊU CHUẨN (Academic Reading Passage - 310 words):\n" +
                   "Effective organizational communication serves as the fundamental bedrock of sustainable enterprise success. In the era of digital transformation and distributed remote workforces, traditional hierarchical top-down directives have rapidly yielded to agile, collaborative communication ecosystems. Modern organizations increasingly recognize that the velocity of product innovation is directly correlated with how seamlessly information traverses organizational boundaries.\n\n" +
                   "Central to this dynamic is the daily stand-up meeting, a cornerstone ceremony within agile methodologies. Far from being a mere ritualistic administrative obligation, the stand-up functions as a rapid synchronization mechanism designed to cultivate transparency, reinforce individual accountability, and surface hidden impediments. By adhering strictly to a focused three-question format—what was accomplished yesterday, what is planned for today, and what blockers exist—participants maintain hyper-alignment while preserving valuable operational momentum.\n\n" +
                   "Nevertheless, the efficacy of agile ceremonies hinges upon psychological safety within the team. When team members fear reprimand for reporting project delays or architectural obstacles, problems remain concealed until they escalate into catastrophic bottlenecks. Conversely, environments that champion transparent escalation empower cross-functional teams to proactively address impediments collaboratively. Ultimately, fostering an organizational culture rooted in candid, constructive dialogue transforms communication from a potential vulnerability into a competitive market advantage.\n\n" +
                   "2. TỪ VỰNG HỌC THUẬT ĐẮT GIÁ TRONG BÀI (Academic Vocabulary):\n" +
                   "   • Bedrock (/ˈbed.rɒk/) - [Noun]: Nền tảng vững chắc, nền móng cốt lõi.\n" +
                   "   • Traverse (/trəˈvɜːs/) - [Verb]: Đi qua, truyền tải xuyên suốt.\n" +
                   "   • Ceremony (/ˈser.ɪ.mə.ni/) - [Noun]: Nghi thức, quy trình định kỳ trong phương pháp Agile.\n" +
                   "   • Impediment (/ɪmˈped.ɪ.mənt/) - [Noun]: Trở ngại, rào cản tiến độ.\n" +
                   "   • Efficacy (/ˈef.ɪ.kə.si/) - [Noun]: Tính hiệu quả, hiệu năng thực tế.\n\n" +
                   "📌 Key Takeaways: Giao tiếp minh bạch, an toàn tâm lý và giải quyết trở ngại chủ động là 3 yếu tố cốt lõi giúp các đội ngũ đạt hiệu suất cao.";
        }

        /// <summary>
        /// Tạo bộ 10 câu hỏi trắc nghiệm chất lượng cao bám sát nội dung, trình độ và kỹ năng
        /// Sử dụng làm bộ đề chuẩn hóa khi offline / hết quota
        /// </summary>
        public static List<CauHoiDTO> TaoCauHoiThongMinhTheoNoiDung(string tieuDe, string noiDung, string trinhDo = "B1", string loaiBaiHoc = "", int soLuong = 10)
        {
            var list = new List<CauHoiDTO>();
            string loaiLower = (loaiBaiHoc ?? "").ToLowerInvariant();

            if (loaiLower.Contains("nguphap") || loaiLower.Contains("grammar"))
            {
                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = $"Question 1 (Grammar Form - {tieuDe}):\nWhich sentence correctly applies the present perfect structure for a completed action with present relevance?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. \"Our engineering division has successfully finalized all critical deliverables ahead of the quarterly deadline.\" (Đúng ✓)",
                        "B. \"Our engineering division have successfully finalized all critical deliverables ahead of the quarterly deadline.\"",
                        "C. \"Our engineering division has finalize all critical deliverables yesterday.\"",
                        "D. \"Our engineering division was finalized all critical deliverables since two days.\""
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Chủ ngữ số ít 'division' đi với trợ động từ 'has' + phân từ hai 'finalized'. Trạng từ 'successfully' đứng giữa trợ động từ và động từ chính theo đúng trật tự chuẩn."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 2 (Question Structure & Time Adverbs):\nChoose the grammatically correct question to inquire about the status of a code review:",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. \"Did you finished the code review yet?\"",
                        "B. \"Have you reviewed the pull request for the authentication module yet?\" (Đúng ✓)",
                        "C. \"Have you review the pull request for the authentication module already?\"",
                        "D. \"Were you reviewed the pull request for the authentication module yet?\""
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Câu hỏi thì Hiện tại hoàn thành có cấu trúc 'Have + S + V3/ed + ... + yet?' để hỏi về việc được kỳ vọng hoàn thành hay chưa."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 3 (Negative Form & Adverb Position):\nIdentify the sentence with correct negative syntax and adverb placement:",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. \"The management team has not yet approved the proposed budget adjustment.\" (Đúng ✓)",
                        "B. \"The management team has approved not yet the proposed budget adjustment.\"",
                        "C. \"The management team not has approved yet the proposed budget adjustment.\"",
                        "D. \"The management team does not have approved the proposed budget adjustment.\""
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Thể phủ định 'has not yet approved' hoặc 'has not approved ... yet' là dạng chuẩn mực trong tiếng Anh thương mại."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 4 (Signal Words - Since vs. For):\nFill in the blank: \"She has served as the lead project coordinator ________ over four years.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. since",
                        "B. for (Đúng ✓)",
                        "C. during",
                        "D. from"
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Dùng 'for' đi kèm với một khoảng thời gian (for over four years). 'Since' chỉ đi với mốc thời gian cụ thể (since 2020, since last Monday)."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 5 (Subject-Verb Agreement):\nSelect the correct option to complete the statement: \"Neither the developer nor the system architects ________ able to reproduce the error.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. has been (Đúng ✓)",
                        "B. have been",
                        "C. was being",
                        "D. were not"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Trong cấu trúc 'Neither... nor...', động từ chia theo chủ ngữ gần nó nhất. Tuy nhiên nếu danh từ gần nhất là số nhiều/ít cần đối chiếu kỹ; ở đây chia theo danh từ đứng liền kề."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 6 (Passive Voice Transformation):\nConvert into passive voice: \"The DevOps team has already deployed the update to production.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. \"The update has already been deployed to production by the DevOps team.\" (Đúng ✓)",
                        "B. \"The update has already being deployed to production by the DevOps team.\"",
                        "C. \"The update was already been deployed to production by the DevOps team.\"",
                        "D. \"The update had already deployed to production by the DevOps team.\""
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Thể bị động của thì Hiện tại hoàn thành: S + have/has + been + V3/ed."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 7 (Error Identification):\nFind the error in the following sentence: \"I [A] have sent the revised [B] proposal to the client [C] yesterday morning [D].\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. [A] have sent (Đúng ✓)",
                        "B. [B] revised",
                        "C. [C] proposal",
                        "D. [D] yesterday morning"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Với mốc thời gian quá khứ xác định 'yesterday morning', bắt buộc phải dùng Quá khứ đơn (sent), không được dùng Hiện tại hoàn thành (have sent)."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 8 (Reduced Relative Clauses):\nChoose the correct reduced relative clause: \"The security protocols ________ by our department ensure comprehensive compliance.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. establishing",
                        "B. established (Đúng ✓)",
                        "C. were established",
                        "D. which established"
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Mệnh đề quan hệ bị động 'which were established by our department' được rút gọn thành quá khứ phân từ 'established'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 9 (Conditional Clause in Work Context):\nComplete the sentence: \"Unless the client ________ the revised scope today, we will not be able to deliver on Friday.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. approves (Đúng ✓)",
                        "B. will approve",
                        "C. approved",
                        "D. does not approve"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Mệnh đề chứa 'Unless' (nếu không) trong câu điều kiện loại 1 chia ở thì Hiện tại đơn và mang nghĩa phủ định sẵn, không dùng trợ động từ phủ định kèm theo."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 10 (Advanced Inversion for Emphasis):\nWhich sentence uses inverted word order correctly for formal emphasis?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. \"Seldom we have witnessed such tremendous progress in a single sprint.\"",
                        "B. \"Seldom have we witnessed such tremendous progress in a single sprint.\" (Đúng ✓)",
                        "C. \"Seldom did we witnessed such tremendous progress in a single sprint.\"",
                        "D. \"Seldom have witnessed we such tremendous progress in a single sprint.\""
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Khi trạng từ phủ định/bán phủ định 'Seldom' đứng đầu câu, ta đảo trợ động từ lên trước chủ ngữ: Seldom + have + S + V3/ed."
                });
            }
            else if (loaiLower.Contains("tuvung") || loaiLower.Contains("vocab"))
            {
                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = $"Question 1 (Vocabulary Definition - {tieuDe}):\nWhat is the exact professional definition of 'deliverables' in project management?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. Quantifiable goods, services, or results that must be provided upon project completion. (Đúng ✓)",
                        "B. Postal packages returned due to incorrect recipient addresses.",
                        "C. Personal opinions expressed informally during breaks.",
                        "D. Office stationary distributed to newly hired staff."
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: 'Deliverables' (sản phẩm bàn giao) là các hạng mục, báo cáo hoặc sản phẩm hoàn thiện bắt buộc phải bàn giao cho khách hàng hoặc ban điều hành."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 2 (Collocation & Phrasal Verbs):\nWhich phrasal verb best completes the sentence: \"I will ________ with the QA lead to ensure the bug fix was properly verified.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. follow up (Đúng ✓)",
                        "B. break down",
                        "C. give away",
                        "D. look forward"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Cụm 'follow up with someone' có nghĩa là liên hệ theo dõi, kiểm tra tiến độ của một công việc trước đó."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 3 (Verb-Noun Collocation):\nSelect the natural verb that collocates with 'milestone': \"Our cross-functional squad managed to ________ a major milestone this quarter.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. reach (Đúng ✓)",
                        "B. take",
                        "C. make",
                        "D. perform"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Cụm 'reach/achieve a milestone' là collocation tự nhiên mang nghĩa đạt được một cột mốc quan trọng."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 4 (Word Formation):\nChoose the correct part of speech: \"Seamless ________ between designers and engineers prevented costly rework.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. coordinate",
                        "B. coordination (Đúng ✓)",
                        "C. coordinator",
                        "D. coordinatingly"
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Đứng sau tính từ 'Seamless' cần một danh từ chỉ sự việc ('coordination' - sự phối hợp)."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 5 (Idiomatic Workplace Expression):\nWhat does it mean when team members are 'on the same page'?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. They are reading identical physical textbooks in the library.",
                        "B. They share the same understanding, goals, and expectations. (Đúng ✓)",
                        "C. They disagree strongly on project priorities.",
                        "D. They are writing code on the same physical computer."
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Thành ngữ 'on the same page' chỉ sự thấu hiểu đồng thuận và có chung góc nhìn, mục tiêu làm việc."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 6 (Technical Jargon Analysis):\nIn workflow optimization, what does a 'bottleneck' refer to?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. A point of congestion in a system that stops or severely slows progress. (Đúng ✓)",
                        "B. The physical narrow neck of a glass container.",
                        "C. A bonus payment given to high-performing staff.",
                        "D. A celebration party at the end of a sprint."
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: 'Bottleneck' (nút thắt cổ chai) là điểm tắc nghẽn làm giảm tốc độ của cả chu trình làm việc."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 7 (Preposition Pairings):\nFill in the preposition: \"All distributed team members must comply ________ industry security standards.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. to",
                        "B. with (Đúng ✓)",
                        "C. for",
                        "D. on"
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: Cụm động từ cố định 'comply with something' mang nghĩa tuân thủ quy định, tiêu chuẩn."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 8 (Antonym in Context):\nWhat is the OPPOSITE in meaning to 'transparency' in communication?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. Clarity",
                        "B. Ambiguity (Đúng ✓)",
                        "C. Honesty",
                        "D. Directness"
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: 'Transparency' là sự minh bạch, rõ ràng; trái nghĩa với nó là 'Ambiguity' (sự mơ hồ, mập mờ)."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 9 (Business Synonyms):\nWhich word can best replace 'expedite' in: \"We need to expedite the release process\"?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. delay",
                        "B. accelerate (Đúng ✓)",
                        "C. cancel",
                        "D. inspect"
                    },
                    DapAnDungIndex = 1,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: 'Expedite' và 'Accelerate' đều mang nghĩa xúc tiến, thúc đẩy tiến độ nhanh hơn."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 10 (Confusing Word Nuance):\nChoose the correct word: \"The new automated testing suite will ________ our continuous deployment pipeline.\"",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. complement (Đúng ✓)",
                        "B. compliment",
                        "C. comprehend",
                        "D. complicate"
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "➔ Dẫn chứng: Câu hỏi luyện tập không có dẫn chứng trực tiếp từ nội dung bài học. - Giải thích: 'Complement' (với chữ 'e') nghĩa là bổ sung hoàn thiện; còn 'Compliment' (với chữ 'i') là khen ngợi."
                });
            }
            else if (loaiLower.Contains("nghe") || loaiLower.Contains("listen"))
            {
                // Bộ câu hỏi kỹ năng Nghe hiểu (Listening): Trắc nghiệm, Đúng/Sai, Điền từ, Trả lời ngắn
                string audioScript = "Audio Dialogue:\n" +
                                     "Emma: \"David, thanks for meeting on short notice. Have you had an opportunity to inspect the crash reports?\"\n" +
                                     "David: \"Yes, Emma. Our diagnostic logs indicate a memory leak occurred when concurrent users exceeded ten thousand.\"\n" +
                                     "Emma: \"Is this issue likely to postpone next Monday's enterprise rollout?\"\n" +
                                     "David: \"Not necessarily. If we push a hotfix to optimize the database query cache today, we can rerun load simulations by tonight.\"\n" +
                                     "Emma: \"Sounds like a viable workaround. Who will be leading the deployment?\"\n" +
                                     "David: \"Marcus and I will handle it directly. We'll post real-time updates in the deployment Slack channel.\"";

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = $"Question 1 (Listening - Setting & Purpose - {tieuDe}):\nWhere does the conversation most likely take place and what is its primary focus?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. In an engineering office discussing an urgent software crash issue before launch. (Đúng ✓)",
                        "B. At a customer service center handling warranty claims.",
                        "C. In a hardware warehouse ordering physical server racks.",
                        "D. At an annual general meeting announcing financial dividends."
                    },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: Cuộc trao đổi giữa Emma và David tập trung vào 'crash reports', 'memory leak' và phương án 'hotfix' trước ngày ra mắt."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 2 (Listening - True / False):\nAccording to David, the enterprise rollout scheduled for next Monday MUST definitely be postponed.",
                    DanhSachLuaChon = new List<string> { "A. True", "B. False (Đúng ✓)" },
                    DapAnDungIndex = 1,
                    LoaiCauHoi = "DungSai",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: David trả lời rõ ràng: 'Not necessarily. If we push a hotfix... we can rerun load simulations by tonight'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 3 (Listening - True / False):\nMarcus and David will personally supervise the deployment of the database hotfix.",
                    DanhSachLuaChon = new List<string> { "A. True (Đúng ✓)", "B. False" },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "DungSai",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: David xác nhận: 'Marcus and I will handle it directly'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 4 (Listening - Information Completion):\nFill in the missing technical cause heard in the dialogue: The system crash was caused by a ________ leak when concurrent users exceeded ten thousand.",
                    DapAnVanBan = "memory",
                    LoaiCauHoi = "DienKhuyet",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: 'Our diagnostic logs indicate a memory leak occurred'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 5 (Listening - Short Answer):\nWhere will the engineering team post real-time updates regarding the deployment status?",
                    DapAnVanBan = "Slack channel|deployment Slack channel|Slack",
                    LoaiCauHoi = "TraLoiNgan",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: 'We'll post real-time updates in the deployment Slack channel'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 6 (Listening - Speaker Attitude & Perception):\nHow does Emma perceive David's proposed mitigation strategy?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. As a viable and pragmatic workaround to keep the launch on track. (Đúng ✓)",
                        "B. As an unrealistic proposal that must be rejected immediately.",
                        "C. As an unnecessary delay that will upset clients.",
                        "D. As a costly measure requiring board authorization."
                    },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: Emma đánh giá: 'Sounds like a viable workaround'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 7 (Listening - Technical Condition & Prerequisite):\nWhat condition must be satisfied before load simulations can be rerun tonight?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. A hotfix to optimize the database query cache must be pushed today. (Đúng ✓)",
                        "B. New physical servers must be ordered from the vendor.",
                        "C. All concurrent users must be disconnected from the network.",
                        "D. Emma must submit an incident ticket to senior management."
                    },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: David nói: 'If we push a hotfix to optimize the database query cache today, we can rerun load simulations by tonight'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 8 (Listening - True / False / Not Given):\nThe memory leak caused customer financial data to be exposed during the crash.",
                    DanhSachLuaChon = new List<string> { "A. True", "B. False", "C. Not Given (Đúng ✓)" },
                    DapAnDungIndex = 2,
                    LoaiCauHoi = "TrueFalseNotGiven",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: Cuộc đối thoại chỉ nhắc đến lỗi bộ nhớ do quá tải người dùng, hoàn toàn không đề cập đến việc dữ liệu tài chính bị lộ -> Not Given."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 9 (Listening - Information Completion):\nComplete the phrase heard in the recording: David and Marcus will push a hotfix to optimize the database query ________ today.",
                    DapAnVanBan = "cache",
                    LoaiCauHoi = "DienKhuyet",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: '...to optimize the database query cache today'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 10 (Listening - Short Answer):\nWhen is the major enterprise rollout originally scheduled to occur?",
                    DapAnVanBan = "next Monday|Monday",
                    LoaiCauHoi = "TraLoiNgan",
                    KyNang = "Nghe",
                    DoanVanDoc = audioScript,
                    GiaiThichChiTiet = "➔ Dẫn chứng bài nghe: Emma hỏi: 'Is this issue likely to postpone next Monday's enterprise rollout?'."
                });
            }
            else if (loaiLower.Contains("viet") || loaiLower.Contains("writing"))
            {
                // Bộ câu hỏi kỹ năng Viết (Writing): Viết lại câu, Sửa lỗi sai, Bài luận tự luận có Rubric
                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = $"Question 1 (Sentence Transformation - {tieuDe}):\nRewrite the sentence using 'COMPLY WITH' so that it means the same as the original:\n\"All engineers must follow industry security standards.\"",
                    DapAnVanBan = "All engineers must comply with industry security standards.",
                    LoaiCauHoi = "VietLaiCau",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Cấu trúc viết lại: 'follow standards' = 'comply with standards' (tuân thủ tiêu chuẩn)."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 2 (Error Correction):\nFind and correct the grammatical error in this sentence: \"Neither the manager nor the developers was informed about the server maintenance.\"",
                    DapAnVanBan = "were|was informed -> were informed",
                    LoaiCauHoi = "SuaLoiSai",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Giải thích sửa lỗi: Với cấu trúc 'Neither... nor...', động từ chia theo danh từ gần nhất 'the developers' (số nhiều) nên phải dùng 'were informed'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 3 (Sentence Transformation):\nRewrite the sentence beginning with 'DESPITE':\n\"Although the deadline was extremely tight, the QA team completed all test cases.\"",
                    DapAnVanBan = "Despite the extremely tight deadline, the QA team completed all test cases.|Despite the tight deadline, the QA team completed all test cases.",
                    LoaiCauHoi = "VietLaiCau",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Cấu trúc viết lại: 'Although + S + V' chuyển thành 'Despite + Noun phrase'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 4 (Error Correction):\nIdentify and correct the erroneous word: \"The continuous deployment pipeline compliments the automated testing framework perfectly.\"",
                    DapAnVanBan = "complements|compliments -> complements",
                    LoaiCauHoi = "SuaLoiSai",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Giải thích sửa lỗi: 'Compliments' nghĩa là khen ngợi; từ đúng phải là 'complements' (bổ sung hoàn thiện)."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = $"Question 5 (Academic Essay Writing Task - {tieuDe}):\nTopic: \"Some organizations believe that daily synchronous stand-ups improve team alignment, while others argue that structured asynchronous updates are far more productive. Discuss both views and give your opinion.\"\n(Write at least 150 words. Organize your essay logically with clear transition devices).",
                    LoaiCauHoi = "TuLuan",
                    KyNang = "Viet",
                    TieuChiCham = "Tiêu chí chấm (IELTS Rubric): Task Response (>=150 từ, lập luận 2 chiều), Coherence & Cohesion (phân đoạn, liên từ nối), Lexical Resource (từ vựng học thuật C1/B2), Grammatical Range & Accuracy (câu phức, bị động, mệnh đề quan hệ).",
                    GiaiThichChiTiet = "➔ Hướng dẫn chấm bài tự luận: Đạt điểm cao khi nêu đủ 2 quan điểm (ưu điểm của họp trực tiếp vs lợi ích của báo cáo bất đồng bộ), đưa ra quan điểm cá nhân rõ ràng và bảo đảm độ dài tối thiểu 150 từ."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 6 (Sentence Transformation - Purpose Clause):\nRewrite the sentence using 'IN ORDER TO' without changing its meaning:\n\"The DevOps team optimized database queries so that they could reduce response latency.\"",
                    DapAnVanBan = "The DevOps team optimized database queries in order to reduce response latency.",
                    LoaiCauHoi = "VietLaiCau",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Cấu trúc viết lại: 'so that + S + could + V' chuyển thành 'in order to + V-infinitive'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 7 (Error Correction - Subjunctive Mood):\nIdentify and correct the grammatical error in this recommendation: \"The technical lead recommended that the developer tests the backup system before deployment.\"",
                    DapAnVanBan = "test|tests -> test",
                    LoaiCauHoi = "SuaLoiSai",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Giải thích sửa lỗi: Sau động từ 'recommend that', mệnh đề dùng thể giả định (subjunctive) với động từ nguyên thể không chia 'test'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 8 (Sentence Transformation - Inversion with Negative Adverbs):\nRewrite the sentence beginning with 'NEVER BEFORE':\n\"Our team had never before encountered such an unprecedented system outage.\"",
                    DapAnVanBan = "Never before had our team encountered such an unprecedented system outage.",
                    LoaiCauHoi = "VietLaiCau",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Cấu trúc đảo ngữ: 'Never before + had + S + V3/ed'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 9 (Academic Lexical Choice in Technical Writing):\nSelect the most formal transition word to show strong contrast in a formal report:",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. Conversely (Đúng ✓)",
                        "B. But also",
                        "C. In a nutshell",
                        "D. By the way"
                    },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Giải thích: 'Conversely' (ngược lại) là liên từ mang sắc thái học thuật và văn phong trang trọng chuẩn mực."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 10 (Sentence Transformation - Impersonal Passive):\nRewrite using impersonal passive: \"People widely recognize that regular code audits minimize security vulnerabilities.\"",
                    DapAnVanBan = "It is widely recognized that regular code audits minimize security vulnerabilities.",
                    LoaiCauHoi = "VietLaiCau",
                    KyNang = "Viet",
                    GiaiThichChiTiet = "➔ Cấu trúc bị động khách quan: 'It is widely recognized that + Clause'."
                });
            }
            else
            {
                // Mặc định hoặc Đọc hiểu (Reading Comprehension): Trắc nghiệm, True/False/Not Given, Trả lời ngắn, Điền từ
                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = $"Question 1 (Main Idea & Primary Purpose - {tieuDe}):\nWhat is the primary message conveyed in the lesson material?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. Transparent communication, psychological safety, and rapid blocker removal drive team excellence. (Đúng ✓)",
                        "B. Individual contributors should operate in total isolation to minimize conversations.",
                        "C. Project deadlines should always be pushed back whenever challenges occur.",
                        "D. Daily meetings are merely bureaucratic formalities with no real practical value."
                    },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: Toàn bộ bài học khẳng định giao tiếp minh bạch, văn hóa an toàn tâm lý và giải quyết vướng mắc sớm là nền tảng cốt lõi của đội ngũ xuất sắc."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 2 (Factual Detail & Evidence):\nAccording to agile best practices, what are the three core questions addressed in a stand-up?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. Salary expectations, holiday plans, and office equipment requests.",
                        "B. Accomplishments since last meeting, planned tasks for today, and existing impediments. (Đúng ✓)",
                        "C. Historical background of the company, client contract details, and competitor pricing.",
                        "D. Personal opinions on company policy, software preferences, and social events."
                    },
                    DapAnDungIndex = 1,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: 3 câu hỏi kinh điển của Daily Stand-up gồm: hôm qua làm gì, hôm nay làm gì, và có trở ngại (blockers/impediments) nào không."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 3 (Reading - True / False / Not Given):\nTransparent escalation within agile teams helps eliminate critical project bottlenecks early.",
                    DanhSachLuaChon = new List<string> { "A. True (Đúng ✓)", "B. False", "C. Not Given" },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TrueFalseNotGiven",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: 'Environments that champion transparent escalation empower cross-functional teams to proactively address impediments'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 4 (Reading - True / False / Not Given):\nThe CEO of the organization personally attends every daily stand-up meeting.",
                    DanhSachLuaChon = new List<string> { "A. True", "B. False", "C. Not Given (Đúng ✓)" },
                    DapAnDungIndex = 2,
                    LoaiCauHoi = "TrueFalseNotGiven",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: Bài đọc hoàn toàn không đề cập đến việc CEO có trực tiếp tham gia buổi stand-up hàng ngày hay không -> Not Given."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 5 (Reading - Sentence Completion):\nComplete the statement based on the text: Agile ceremonies function best when participants maintain disciplined ________.",
                    DapAnVanBan = "conciseness|clarity",
                    LoaiCauHoi = "DienKhuyet",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: 'Agile ceremonies function best when participants maintain disciplined conciseness'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 6 (Reading - Short Answer):\nWhich academic vocabulary word used in the passage means 'obstacles or hindrances that slow down progress'?",
                    DapAnVanBan = "impediments|impediment",
                    LoaiCauHoi = "TraLoiNgan",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: 'Impediment (/ɪmˈped.ɪ.mənt/) - [Noun]: Trở ngại, rào cản tiến độ'."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 7 (Reading - Vocabulary in Context):\nIn organizational development, what does the phrase 'psychological safety' specifically imply?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. A shared belief that the team is safe for interpersonal risk-taking without fear of humiliation. (Đúng ✓)",
                        "B. Having mandatory physical security guards stationed outside meeting rooms.",
                        "C. Suppressing opposing opinions to prevent arguments during discussions.",
                        "D. Guaranteeing lifetime employment regardless of work performance."
                    },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: 'Psychological safety' (an toàn tâm lý) là trạng thái các thành viên thoải mái đóng góp ý kiến, nêu lỗi sai hoặc đặt câu hỏi mà không sợ bị phán xét hay khiển trách."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 8 (Reading - Logical Inference):\nWhat can be logically inferred about teams that fail to practice timely problem escalation?",
                    DanhSachLuaChon = new List<string>
                    {
                        "A. Minor blockers will accumulate into critical bottlenecks that jeopardize release schedules. (Đúng ✓)",
                        "B. They will inevitably outperform organizations using agile frameworks.",
                        "C. Their daily meetings will become shorter and much more productive.",
                        "D. Project clients will automatically waive all deliverable requirements."
                    },
                    DapAnDungIndex = 0,
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng suy luận: Khi các rào cản (impediments) không được báo cáo và tháo gỡ kịp thời, chúng sẽ tích tụ và gây tê liệt tiến độ bàn giao của cả dự án."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 9 (Reading - True / False):\nDaily stand-up meetings are designed to be in-depth technical debugging sessions lasting over one hour.",
                    DanhSachLuaChon = new List<string> { "A. True", "B. False (Đúng ✓)" },
                    DapAnDungIndex = 1,
                    LoaiCauHoi = "DungSai",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: Cuộc họp stand-up được thiết kế ngắn gọn (15 phút), không phải là buổi tranh luận kỹ thuật chuyên sâu kéo dài."
                });

                list.Add(new CauHoiDTO
                {
                    NoiDungCauHoi = "Question 10 (Reading - Short Answer):\nAccording to the passage, what type of communication culture empowers cross-functional teams to proactively address impediments?",
                    DapAnVanBan = "transparent escalation|transparent|transparent communication",
                    LoaiCauHoi = "TraLoiNgan",
                    KyNang = "Doc",
                    GiaiThichChiTiet = "➔ Dẫn chứng bài học: 'Environments that champion transparent escalation empower cross-functional teams to proactively address impediments'."
                });
            }

            string determinedKyNang = BoCauHoiBUS.ChuankHoaKyNang(loaiBaiHoc);
            string passage = BoCauHoiBUS.TrichXuatDoanVanDocHieu(noiDung);

            for (int i = 0; i < list.Count; i++)
            {
                var q = list[i];
                if (string.IsNullOrWhiteSpace(q.KyNang)) q.KyNang = determinedKyNang;
                if (string.IsNullOrWhiteSpace(q.DoKho)) q.DoKho = (i < 2) ? "De" : ((i >= 4) ? "Kho" : "TrungBinh");
                if (string.IsNullOrWhiteSpace(q.DoanVanDoc) && (q.KyNang == "Doc" || determinedKyNang == "Doc" || q.KyNang == "Nghe" || determinedKyNang == "Nghe"))
                {
                    q.DoanVanDoc = passage;
                }
            }

            if (soLuong > 0 && list.Count > soLuong)
            {
                return list.Take(soLuong).ToList();
            }

            return list;
        }
    }
}
