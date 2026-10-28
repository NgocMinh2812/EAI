using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BUS.AI
{
    public class NoiDungHocTapAIService
    {
        private static NoiDungHocTapAIService? _instance;
        public static NoiDungHocTapAIService Instance => _instance ??= new NoiDungHocTapAIService();

        /// <summary>
        /// Tạo mã băm SHA256 nhận diện nội dung nguồn của bài học.
        /// Bất kỳ thay đổi nào từ giảng viên (tiêu đề, kỹ năng, trình độ, nội dung tóm tắt/chi tiết, audio)
        /// sẽ tự động làm thay đổi hash, kích hoạt phân tích mới khi sinh viên mở bài.
        /// </summary>
        public static string TinhContentHash(BaiHocDTO baiHoc)
        {
            if (baiHoc == null) return string.Empty;

            string raw = $"{baiHoc.TieuDe?.Trim()}|{baiHoc.LoaiBaiHoc?.Trim()}|{baiHoc.MaTrinhDo?.Trim()}|" +
                         $"{baiHoc.TomTat?.Trim()}|{baiHoc.NoiDung?.Trim()}|" +
                         $"{baiHoc.AudioPath?.Trim()}|{baiHoc.DuongDanAudio?.Trim()}";

            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Chuẩn hóa mã kỹ năng từ thông tin bài học
        /// </summary>
        public static string ChuanHoaKyNang(BaiHocDTO baiHoc)
        {
            string loai = (baiHoc.LoaiBaiHoc ?? "").Trim().ToLowerInvariant();
            string tenLoai = (baiHoc.TenLoaiHienThi ?? "").Trim().ToLowerInvariant();
            string tieuDe = (baiHoc.TieuDe ?? "").Trim().ToLowerInvariant();

            if (loai.Contains("nghe") || loai.Contains("listen") || tenLoai.Contains("nghe") || tieuDe.Contains("nghe") || tieuDe.Contains("listening"))
                return "Listening";
            if (loai.Contains("doc") || loai.Contains("read") || tenLoai.Contains("đọc") || tieuDe.Contains("reading"))
                return "Reading";
            if (loai.Contains("viet") || loai.Contains("write") || tenLoai.Contains("viết") || tieuDe.Contains("writing"))
                return "Writing";
            if (loai.Contains("noi") || loai.Contains("speak") || tenLoai.Contains("nói") || tieuDe.Contains("speaking"))
                return "Speaking";
            if (loai.Contains("tuvung") || loai.Contains("vocab") || tenLoai.Contains("từ vựng") || tieuDe.Contains("vocabulary"))
                return "Vocabulary";
            if (loai.Contains("nguphap") || loai.Contains("grammar") || tenLoai.Contains("ngữ pháp") || tieuDe.Contains("grammar"))
                return "Grammar";

            return "Reading";
        }

        /// <summary>
        /// Quy trình chính:
        /// 1. Kiểm tra cache trong Database theo ContentHash. Nếu đã có -> trả về ngay, không gọi AI.
        /// 2. Nếu chưa có -> áp dụng cơ chế khóa phân tán trong DB chống gọi AI trùng lặp.
        /// 3. Yêu cầu AI phân tích và trả về cấu trúc thẻ học tập phù hợp với từng kỹ năng.
        /// 4. Kiểm tra tính hợp lệ dữ liệu và lưu vào database.
        /// </summary>
        public async Task<BaiHocAIContentDTO> LayHoacPhanTichNoiDungAsync(
            BaiHocDTO baiHoc, 
            Action<string>? onStatusUpdate = null,
            bool forceReanalyze = false)
        {
            if (baiHoc == null || baiHoc.MaBaiHoc < 0)
            {
                throw new ArgumentException("Thông tin bài học không hợp lệ.");
            }

            string contentHash = TinhContentHash(baiHoc);
            string skill = ChuanHoaKyNang(baiHoc);

            // BƯỚC 1: Kiểm tra cache trong DB (chỉ áp dụng nếu không yêu cầu phân tích lại)
            if (!forceReanalyze)
            {
                var existing = BaiHocAIContentDAL.Instance.LayNoiDungAI(baiHoc.MaBaiHoc, contentHash);
                if (existing != null && existing.DanhSachTheHoc != null && existing.DanhSachTheHoc.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[NoiDungHocTapAIService] Tái sử dụng nội dung AI đã lưu từ DB cho bài học {baiHoc.MaBaiHoc} (Hash: {contentHash.Substring(0, 8)}). Không gọi AI.");
                    return existing;
                }
            }

            // BƯỚC 2: Kiểm tra khóa và xử lý trùng lặp (Concurrency lease)
            if (!forceReanalyze)
            {
                var (leaseStatus, leaseData) = BaiHocAIContentDAL.Instance.KiemTraVaGiuKhoaXuLy(baiHoc.MaBaiHoc, contentHash);
                if (leaseStatus == AILeaseStatus.AlreadyCompleted && leaseData != null)
                {
                    return leaseData;
                }

                if (leaseStatus == AILeaseStatus.CurrentlyProcessing)
                {
                    // Có tiến trình khác đang phân tích bài học này, chờ tối đa 4-5 giây
                    onStatusUpdate?.Invoke("Bài học đang được hệ thống phân tích. Đang đồng bộ kết quả...");
                    for (int i = 0; i < 3; i++)
                    {
                        await Task.Delay(1500);
                        var polled = BaiHocAIContentDAL.Instance.LayNoiDungAI(baiHoc.MaBaiHoc, contentHash);
                        if (polled != null && polled.DanhSachTheHoc.Count > 0)
                        {
                            return polled;
                        }
                    }
                }
            }

            // BƯỚC 3: Đồng bộ cấu hình AI mới nhất do Giảng viên lưu và gọi AI phân tích
            AIServiceManager.Instance.SyncActiveConfigFromStorage();
            string moHinhHienTai = AIServiceManager.Instance.ActiveModel ?? "Mặc định";
            onStatusUpdate?.Invoke($"Trợ lý AI ({AIServiceManager.Instance.ActiveProviderType} - {moHinhHienTai}) đang phân tích bài học {skill}...");

            try
            {
                string prompt = TaoPromptPhanTichTheoKyNang(baiHoc, skill);

                // Gọi mô hình AI theo đúng cấu hình mà giảng viên đã lưu
                var aiResponse = await AIServiceManager.Instance.ExecutePromptAsync(prompt, actionName: $"PhanTichBaiHoc_{skill}");
                string? response = aiResponse.Success ? aiResponse.Text : null;

                // Nếu mô hình được chọn gặp sự cố, thử fallback sang GeminiAIService
                if (string.IsNullOrWhiteSpace(response))
                {
                    response = await GeminiAIService.GoiGeminiApiAsync(prompt, "NoiDungHocTap");
                }

                string tenMoHinhDung = aiResponse.Success 
                    ? $"{aiResponse.ProviderName} ({aiResponse.ModelUsed})" 
                    : (!string.IsNullOrWhiteSpace(AIServiceManager.Instance.ActiveModel)
                        ? $"{AIServiceManager.Instance.ActiveProviderType} ({AIServiceManager.Instance.ActiveModel})"
                        : "Trợ lý AI");

                BaiHocAIContentDTO resultDto = new BaiHocAIContentDTO
                {
                    MaBaiHoc = baiHoc.MaBaiHoc,
                    ContentHash = contentHash,
                    KyNang = skill,
                    MoHinhAI = tenMoHinhDung,
                    AudioPathGoc = baiHoc.AudioPath ?? baiHoc.DuongDanAudio
                };

                List<LearningCardDTO> cards = new();

                if (!string.IsNullOrWhiteSpace(response))
                {
                    cards = BopTachTheHocTuJson(response, skill, baiHoc);
                }

                // Nếu AI không trả về json hợp lệ, kích hoạt bộ phân tích nội dung nguồn thông minh tại chỗ
                if (cards == null || cards.Count == 0)
                {
                    cards = TaoTheHocTuNoiDungNguon(baiHoc, skill);
                }

                if (cards.Count == 0)
                {
                    throw new InvalidOperationException("Không thể cấu trúc hóa nội dung học tập từ tài liệu nguồn của bài học.");
                }

                // Gán danh sách thẻ học và trích xuất mục tiêu chính
                resultDto.DanhSachTheHoc = cards;
                var objCard = cards.FirstOrDefault(c => c.CardType == "Objective");
                resultDto.MucTieuBaiHoc = objCard?.Content ?? $"Làm chủ kiến thức trọng tâm bài học {baiHoc.TieuDe}.";
                resultDto.TrangThai = "HoanThanh";

                // BƯỚC 4: Lưu đầy đủ vào Database
                bool saveOk = BaiHocAIContentDAL.Instance.LuuKetQuaHoanThanh(resultDto);
                if (!saveOk)
                {
                    System.Diagnostics.Debug.WriteLine($"[NoiDungHocTapAIService] Cảnh báo: Lưu kết quả vào DB thất bại cho bài học {baiHoc.MaBaiHoc}.");
                }

                return resultDto;
            }
            catch (Exception ex)
            {
                BaiHocAIContentDAL.Instance.LuuTrangThaiThatBai(baiHoc.MaBaiHoc, ex.Message);
                throw new Exception($"Không thể chuẩn bị nội dung bài học bằng AI: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo prompt chuyên biệt theo từng kỹ năng học tập dựa trên yêu cầu thiết kế thẻ học tập
        /// </summary>
        private static string TaoPromptPhanTichTheoKyNang(BaiHocDTO baiHoc, string skill)
        {
            string? filePath = baiHoc.DuongDanTepVatLy;
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                filePath = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(baiHoc);
                if (!string.IsNullOrWhiteSpace(filePath)) baiHoc.DuongDanTepVatLy = filePath;
            }
            string rawContent = KhoHocLieuSyncHelper.DocNoiDungTep(filePath, baiHoc.NoiDung);
            string cleanContent = KhoHocLieuSyncHelper.ChuyenDoiHtmlSangVanBan(rawContent);
            if (cleanContent.Length > 4000)
            {
                cleanContent = cleanContent.Substring(0, 4000);
            }

            string skillSpecificGuide = skill switch
            {
                "Listening" => @"
- KỸ NĂNG NGHE HIỂU (LISTENING):
  + Card 1: 'Objective' (Mục tiêu nghe: nhận diện thông tin chính, chi tiết, số liệu hoặc ý định người nói phù hợp với độ khó).
  + Card 2: 'AudioSection' (Hướng dẫn nghe audio bài học do giảng viên đính kèm; lưu ý không tạo audio mới).
  + Card 3: 'Vocabulary' (Từ khóa & biểu đạt cần nghe: từ vựng then chốt, tín hiệu thời gian/số liệu).
  + Card 4: 'Strategy' (Chiến lược nghe: bắt từ khóa, dự đoán ý, nối âm/nuốt âm khi có cơ sở).
  + Card 5: 'Pitfall' (Bẫy nghe / thông tin gây nhiễu: chỉ tạo nếu bài học có thông tin sửa đổi hoặc dễ hiểu lầm).
  + Card 6: 'Dialogue' (Mẫu câu & hội thoại giao tiếp xuất hiện trong bài).",

                "Reading" => @"
- KỸ NĂNG ĐỌC HIỂU (READING):
  + Card 1: 'Objective' (Mục tiêu đọc: tìm ý chính, quét chi tiết, suy luận ngữ cảnh).
  + Card 2: 'Passage' (Đoạn văn đọc hiểu & cấu trúc bài: trích xuất đoạn văn chính từ nguồn).
  + Card 3: 'Vocabulary' (Từ vựng ngữ cảnh: trích các từ học thuật, collocations quan trọng trong bài đọc).
  + Card 4: 'Strategy' (Chiến lược đọc: Skimming tìm đại ý, Scanning định vị dữ liệu cứng, tham chiếu đại từ).
  + Card 5: 'Pitfall' (Điểm dễ hiểu lầm: từ đa nghĩa, cấu trúc phủ định, bẫy logic).",

                "Writing" => @"
- KỸ NĂNG VIẾT (WRITING):
  + Card 1: 'Objective' (Mục tiêu viết: thể loại essay, email, báo cáo và chuẩn năng lực cần đạt).
  + Card 2: 'CoreKnowledge' (Phân tích đề bài, cấu trúc bố cục và cách tổ chức ý các đoạn).
  + Card 3: 'Vocabulary' (Mẫu câu & cụm từ hữu ích: từ nối, diễn đạt quan điểm, phản biện).
  + Card 4: 'Strategy' (Ngữ pháp và liên kết ý: phương tiện tạo mạch lạc, đa dạng cấu trúc).
  + Card 5: 'Pitfall' (Lỗi diễn đạt thường gặp và cách cải thiện).
  + Card 6: 'Sample' (Bài mẫu minh họa chuẩn band điểm).",

                "Speaking" => @"
- KỸ NĂNG NÓI (SPEAKING):
  + Card 1: 'Objective' (Mục tiêu giao tiếp và độ trôi chảy).
  + Card 2: 'CoreKnowledge' (Tình huống giao tiếp và ngữ cảnh thực tế).
  + Card 3: 'Vocabulary' (Mẫu câu phản xạ và khung câu nói).
  + Card 4: 'Dialogue' (Hội thoại mẫu minh họa).
  + Card 5: 'Strategy' (Chiến lược mở đầu, duy trì và phát âm/ngữ điệu).
  + Card 6: 'PracticePrompt' (Chủ đề gợi ý thực hành nói).",

                "Vocabulary" => @"
- KỸ NĂNG TỪ VỰNG (VOCABULARY):
  + Card 1: 'Objective' (Mục tiêu làm chủ từ vựng theo chủ đề).
  + Card 2: 'Vocabulary' (Danh sách từ vựng trọng tâm trích từ bài: từ, phiên âm IPA, từ loại, nghĩa tiếng Việt, câu ví dụ và collocation).
  + Card 3: 'Strategy' (Phương pháp ghi nhớ từ vựng qua ngữ cảnh và từ gia đình).
  + Card 4: 'Pitfall' (Các từ dễ nhầm lẫn, bẫy collocations).",

                "Grammar" => @"
- KỸ NĂNG NGỮ PHÁP (GRAMMAR):
  + Card 1: 'Objective' (Mục tiêu làm chủ cấu trúc ngữ pháp).
  + Card 2: 'CoreKnowledge' (Quy tắc & công thức cốt lõi, cách dùng và ngữ cảnh áp dụng).
  + Card 3: 'Strategy' (Ví dụ minh họa ngữ cảnh thực tế).
  + Card 4: 'Pitfall' (Các lỗi sai ngữ pháp kinh điển và cách sửa đúng).
  + Card 5: 'Vocabulary' (Các dấu hiệu nhận biết và cụm từ đi kèm).",

                _ => @"
- BÀI HỌC TỔNG HỢP:
  + Card 1: 'Objective' (Mục tiêu bài học).
  + Card 2: 'CoreKnowledge' (Kiến thức trọng tâm).
  + Card 3: 'Vocabulary' (Từ vựng quan trọng).
  + Card 4: 'Strategy' (Chiến lược học tập hiệu quả)."
            };

            return $@"Bạn là trợ lý AI sư phạm cao cấp của nền tảng Tiếng Anh EAI (TiếngAnhAI).
Nhiệm vụ: Phân tích bài học tiếng Anh sau đây và phân bổ thành các THẺ HỌC TẬP THÔNG MINH phù hợp với kỹ năng {skill} và trình độ {baiHoc.MaTrinhDo}.

THÔNG TIN BÀI HỌC:
- Tiêu đề: {baiHoc.TieuDe}
- Kỹ năng chính: {skill}
- Trình độ: {baiHoc.MaTrinhDo}
- Chủ đề: {baiHoc.TenChuDe}
- Tóm tắt: {baiHoc.TomTat}
- Nội dung gốc:
{cleanContent}

NGUYÊN TẮC BẮT BUỘC (TUÂN THỦ TUYỆT ĐỐI):
1. Không áp dụng cùng một mẫu cứng nhắc cho mọi bài. Lựa chọn các thẻ phù hợp với kỹ năng {skill}.
2. Nếu bài Listening có đính kèm audio, nhắc nhở sử dụng audio gốc của giảng viên. Tuyệt đối KHÔNG tạo audio mới và KHÔNG bịa đặt transcript nếu nguồn không có.
3. Không bịa đặt nội dung không có trong tài liệu. Đánh dấu rõ nội dung nào lấy từ nguồn và phần nào là phân tích/giải thích bổ sung của AI.
4. Trả về DUY NHẤT một khối JSON hợp lệ theo schema sau (không kèm markdown ngoài JSON):

```json
{{
  ""mucTieu"": ""Mục tiêu cốt lõi của bài học"",
  ""kyNang"": ""{skill}"",
  ""theHocTap"": [
    {{
      ""cardType"": ""Objective / Passage / CoreKnowledge / Vocabulary / Strategy / Pitfall / Sample / Dialogue / PracticePrompt"",
      ""title"": ""Tiêu đề thẻ (viết hoa nổi bật)"",
      ""content"": ""Nội dung chi tiết của thẻ"",
      ""icon"": ""🎯 / 📰 / 📖 / 💬 / 💡 / ⚠️ / ✍️ / 🗣️"",
      ""headerColor"": ""#0F766E"",
      ""backgroundColor"": ""#F0FDF4"",
      ""borderColor"": ""#BBF7D0"",
      ""orderIndex"": 1,
      ""isAiSupplemented"": false,
      ""keyPoints"": [""Điểm then chốt 1"", ""Điểm then chốt 2""],
      ""vocabularyItems"": [
        {{
          ""word"": ""từ"",
          ""ipa"": ""/phiên âm/"",
          ""partOfSpeech"": ""loại từ"",
          ""meaning"": ""nghĩa"",
          ""example"": ""ví dụ"",
          ""collocation"": ""cụm từ đi kèm""
        }}
      ],
      ""dialogueLines"": [
        {{
          ""speaker"": ""Tên người nói"",
          ""text"": ""Câu tiếng Anh"",
          ""translation"": ""Bản dịch tiếng Việt""
        }}
      ]
    }}
  ]
}}
```

HƯỚNG DẪN CÁC THẺ THEO KỸ NĂNG:
{skillSpecificGuide}";
        }

        /// <summary>
        /// Bóc tách JSON từ phản hồi của AI và chuẩn hóa các trường màu sắc/biểu tượng
        /// </summary>
        private static List<LearningCardDTO> BopTachTheHocTuJson(string jsonText, string skill, BaiHocDTO baiHoc)
        {
            var list = new List<LearningCardDTO>();
            try
            {
                // Loại bỏ markdown code blocks ```json ... ```
                string cleanJson = jsonText.Trim();
                if (cleanJson.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
                {
                    cleanJson = cleanJson.Substring(7);
                }
                else if (cleanJson.StartsWith("```", StringComparison.OrdinalIgnoreCase))
                {
                    cleanJson = cleanJson.Substring(3);
                }
                if (cleanJson.EndsWith("```"))
                {
                    cleanJson = cleanJson.Substring(0, cleanJson.Length - 3);
                }
                cleanJson = cleanJson.Trim();

                // Trích xuất khối { ... } an toàn đề phòng text mở đầu/kết thúc từ LLM
                int startObj = cleanJson.IndexOf('{');
                int endObj = cleanJson.LastIndexOf('}');
                if (startObj >= 0 && endObj > startObj)
                {
                    cleanJson = cleanJson.Substring(startObj, endObj - startObj + 1);
                }

                using var doc = JsonDocument.Parse(cleanJson);
                var root = doc.RootElement;

                JsonElement cardsArray;
                bool foundCards = root.TryGetProperty("theHocTap", out cardsArray) ||
                                  root.TryGetProperty("TheHocTap", out cardsArray) ||
                                  root.TryGetProperty("learningCards", out cardsArray) ||
                                  root.TryGetProperty("cards", out cardsArray);

                if (foundCards && cardsArray.ValueKind == JsonValueKind.Array)
                {
                    int index = 1;
                    foreach (var elem in cardsArray.EnumerateArray())
                    {
                        var card = new LearningCardDTO
                        {
                            CardType = elem.TryGetProperty("cardType", out var cType) ? cType.GetString() ?? "CoreKnowledge" : "CoreKnowledge",
                            Title = elem.TryGetProperty("title", out var title) ? title.GetString() ?? "" : "",
                            Content = elem.TryGetProperty("content", out var cnt) ? cnt.GetString() ?? "" : "",
                            Icon = elem.TryGetProperty("icon", out var ic) ? ic.GetString() ?? "📖" : "📖",
                            OrderIndex = index++
                        };

                        // Gán màu sắc chuẩn thương hiệu theo loại thẻ
                        GanGiaoDienTheHoc(card);

                        // Trích xuất key points
                        if (elem.TryGetProperty("keyPoints", out var kpArr) && kpArr.ValueKind == JsonValueKind.Array)
                        {
                            card.KeyPoints = new List<string>();
                            foreach (var kp in kpArr.EnumerateArray())
                            {
                                string? s = kp.GetString();
                                if (!string.IsNullOrWhiteSpace(s)) card.KeyPoints.Add(s);
                            }
                        }

                        // Trích xuất từ vựng
                        if (elem.TryGetProperty("vocabularyItems", out var vArr) && vArr.ValueKind == JsonValueKind.Array)
                        {
                            card.VocabularyItems = new List<VocabularyItemDTO>();
                            foreach (var v in vArr.EnumerateArray())
                            {
                                card.VocabularyItems.Add(new VocabularyItemDTO
                                {
                                    Word = v.TryGetProperty("word", out var w) ? w.GetString() ?? "" : "",
                                    Ipa = v.TryGetProperty("ipa", out var ip) ? ip.GetString() ?? "" : "",
                                    PartOfSpeech = v.TryGetProperty("partOfSpeech", out var pos) ? pos.GetString() ?? "" : "",
                                    Meaning = v.TryGetProperty("meaning", out var m) ? m.GetString() ?? "" : "",
                                    Example = v.TryGetProperty("example", out var ex) ? ex.GetString() ?? "" : "",
                                    Collocation = v.TryGetProperty("collocation", out var col) ? col.GetString() ?? "" : ""
                                });
                            }
                        }

                        // Trích xuất hội thoại
                        if (elem.TryGetProperty("dialogueLines", out var dArr) && dArr.ValueKind == JsonValueKind.Array)
                        {
                            card.DialogueLines = new List<DialogueLineDTO>();
                            foreach (var d in dArr.EnumerateArray())
                            {
                                card.DialogueLines.Add(new DialogueLineDTO
                                {
                                    Speaker = d.TryGetProperty("speaker", out var spk) ? spk.GetString() ?? "" : "",
                                    Text = d.TryGetProperty("text", out var tx) ? tx.GetString() ?? "" : "",
                                    Translation = d.TryGetProperty("translation", out var tr) ? tr.GetString() ?? "" : ""
                                });
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(card.Title) || !string.IsNullOrWhiteSpace(card.Content) || (card.VocabularyItems != null && card.VocabularyItems.Count > 0))
                        {
                            list.Add(card);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NoiDungHocTapAIService.BopTachTheHocTuJson] Lỗi parse: {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Tạo thẻ học tập thông minh trực tiếp từ tài liệu nguồn nếu AI gặp gián đoạn mạng
        /// </summary>
        public static List<LearningCardDTO> TaoTheHocTuNoiDungNguon(BaiHocDTO baiHoc, string skill)
        {
            var cards = new List<LearningCardDTO>();
            string? filePath = baiHoc.DuongDanTepVatLy;
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                filePath = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(baiHoc);
                if (!string.IsNullOrWhiteSpace(filePath)) baiHoc.DuongDanTepVatLy = filePath;
            }
            string rawContent = KhoHocLieuSyncHelper.DocNoiDungTep(filePath, baiHoc.NoiDung);
            string cleanContent = KhoHocLieuSyncHelper.ChuyenDoiHtmlSangVanBan(rawContent);

            // 1. Thẻ mục tiêu bài học
            string objective = !string.IsNullOrWhiteSpace(baiHoc.TomTat) 
                ? baiHoc.TomTat 
                : $"Nắm vững bản chất kiến thức và phản xạ vận dụng cho bài học {baiHoc.TieuDe}.";

            cards.Add(new LearningCardDTO
            {
                CardType = "Objective",
                Title = "🎯 MỤC TIÊU BÀI HỌC",
                Content = objective,
                Icon = "🎯",
                HeaderColor = "#166534",
                BackgroundColor = "#F0FDF4",
                BorderColor = "#BBF7D0",
                OrderIndex = 1
            });

            // 2. Thẻ đặc thù kỹ năng
            if (skill == "Listening")
            {
                cards.Add(new LearningCardDTO
                {
                    CardType = "AudioSection",
                    Title = "🎧 TỆP ÂM THANH BÀI GIẢNG GỐC",
                    Content = !string.IsNullOrWhiteSpace(baiHoc.AudioPath ?? baiHoc.DuongDanAudio)
                        ? "Bài học có đính kèm file âm thanh bản xứ. Hãy sử dụng bảng điều khiển audio bên dưới để nghe bài giảng, tua lại và điều chỉnh tốc độ nghe."
                        : "Bài học chưa có file âm thanh đính kèm từ giảng viên.",
                    Icon = "🎧",
                    HeaderColor = "#0F766E",
                    BackgroundColor = "#F0FDFA",
                    BorderColor = "#CCFBF1",
                    OrderIndex = 2
                });
            }
            else if (skill == "Reading")
            {
                cards.Add(new LearningCardDTO
                {
                    CardType = "Passage",
                    Title = "📰 ĐOẠN VĂN ĐỌC HIỂU TIẾNG ANH (READING PASSAGE)",
                    Content = !string.IsNullOrWhiteSpace(cleanContent) ? cleanContent : "Nội dung đoạn văn đọc hiểu đang được cập nhật.",
                    Icon = "📰",
                    HeaderColor = "#1E293B",
                    BackgroundColor = "#F8FAFC",
                    BorderColor = "#CBD5E1",
                    OrderIndex = 2
                });
            }
            else if (skill == "Writing")
            {
                cards.Add(new LearningCardDTO
                {
                    CardType = "CoreKnowledge",
                    Title = "✍️ HƯỚNG DẪN BỐ CỤC & TỔ CHỨC Ý (WRITING OUTLINE)",
                    Content = cleanContent,
                    Icon = "✍️",
                    HeaderColor = "#1D4ED8",
                    BackgroundColor = "#EFF6FF",
                    BorderColor = "#BFDBFE",
                    OrderIndex = 2
                });
            }
            else
            {
                cards.Add(new LearningCardDTO
                {
                    CardType = "CoreKnowledge",
                    Title = "📖 CẤU TRÚC & KIẾN THỨC CỐT LÕI (CORE RULES)",
                    Content = cleanContent,
                    Icon = "📖",
                    HeaderColor = "#0F766E",
                    BackgroundColor = "#F8FAFC",
                    BorderColor = "#E2E8F0",
                    OrderIndex = 2
                });
            }

            // 3. Thẻ chiến lược học tập
            cards.Add(new LearningCardDTO
            {
                CardType = "Strategy",
                Title = "💡 CHIẾN LƯỢC HỌC TẬP & PHẢN XẠ HIỆU QUẢ",
                Content = skill switch
                {
                    "Listening" => "• Nghe lần 1: Nắm bắt ý tổng quát, không dừng lại ở từ mới.\n• Nghe lần 2: Tập trung bắt các từ khóa trọng tâm và các chi tiết số liệu.\n• Nghe lần 3: Đối chiếu với đoạn hội thoại để nhận diện các hiện tượng nối âm, nuốt âm.",
                    "Reading" => "• Áp dụng Skimming (đọc lướt 60s) để nắm mạch triển khai ý của tác giả.\n• Dùng Scanning quét các dữ liệu cứng như tên riêng, số liệu thống kê hoặc từ khóa in hoa.",
                    "Writing" => "• Lập dàn ý 3 phần (Mở bài, Thân bài, Kết luận) trước khi viết chi tiết.\n• Sử dụng linh hoạt các từ nối chuyển ý (However, Furthermore, Consequently) để tăng tính mạch lạc.",
                    _ => "• Ôn tập nhắc lại ngắt quãng để ghi nhớ lâu dài.\n• Tự đặt câu ví dụ gắn liền với hoạt động thường ngày để biến kiến thức thành phản xạ tự nhiên."
                },
                Icon = "💡",
                HeaderColor = "#1D4ED8",
                BackgroundColor = "#EFF6FF",
                BorderColor = "#BFDBFE",
                OrderIndex = 3
            });

            // 4. Thẻ lưu ý / bẫy thường gặp
            cards.Add(new LearningCardDTO
            {
                CardType = "Pitfall",
                Title = "⚠️ BẪY THƯỜNG GẶP & NGUYÊN TẮC QUAN TRỌNG",
                Content = "• Chú ý các từ hạn định tuyệt đối (always, never, all) và các thông tin gây nhiễu trong bài.\n• Tránh dịch từng từ một (word-by-word) mà hãy học theo cụm từ (collocations) hoàn chỉnh.",
                Icon = "⚠️",
                HeaderColor = "#92400E",
                BackgroundColor = "#FEF3C7",
                BorderColor = "#FDE68A",
                OrderIndex = 4
            });

            return cards;
        }

        private static void GanGiaoDienTheHoc(LearningCardDTO card)
        {
            switch (card.CardType)
            {
                case "Objective":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "🎯" : card.Icon;
                    card.HeaderColor = "#166534";
                    card.BackgroundColor = "#F0FDF4";
                    card.BorderColor = "#BBF7D0";
                    break;
                case "AudioSection":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "🎧" : card.Icon;
                    card.HeaderColor = "#0F766E";
                    card.BackgroundColor = "#F0FDFA";
                    card.BorderColor = "#CCFBF1";
                    break;
                case "Passage":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "📰" : card.Icon;
                    card.HeaderColor = "#1E293B";
                    card.BackgroundColor = "#F8FAFC";
                    card.BorderColor = "#CBD5E1";
                    break;
                case "CoreKnowledge":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "📖" : card.Icon;
                    card.HeaderColor = "#0F766E";
                    card.BackgroundColor = "#F8FAFC";
                    card.BorderColor = "#E2E8F0";
                    break;
                case "Vocabulary":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "💬" : card.Icon;
                    card.HeaderColor = "#B45309";
                    card.BackgroundColor = "#FFFBEB";
                    card.BorderColor = "#FDE68A";
                    break;
                case "Strategy":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "💡" : card.Icon;
                    card.HeaderColor = "#1D4ED8";
                    card.BackgroundColor = "#EFF6FF";
                    card.BorderColor = "#BFDBFE";
                    break;
                case "Pitfall":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "⚠️" : card.Icon;
                    card.HeaderColor = "#92400E";
                    card.BackgroundColor = "#FEF3C7";
                    card.BorderColor = "#FDE68A";
                    break;
                case "Sample":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "✍️" : card.Icon;
                    card.HeaderColor = "#4338CA";
                    card.BackgroundColor = "#EEF2FF";
                    card.BorderColor = "#C7D2FE";
                    break;
                case "Dialogue":
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "🗣️" : card.Icon;
                    card.HeaderColor = "#0369A1";
                    card.BackgroundColor = "#F0F9FF";
                    card.BorderColor = "#BAE6FD";
                    break;
                default:
                    card.Icon = string.IsNullOrEmpty(card.Icon) ? "📌" : card.Icon;
                    card.HeaderColor = "#334155";
                    card.BackgroundColor = "#F8FAFC";
                    card.BorderColor = "#E2E8F0";
                    break;
            }
        }
    }
}
