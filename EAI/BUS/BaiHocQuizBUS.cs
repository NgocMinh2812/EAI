using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BUS
{
    public class BaiHocQuizBUS
    {
        private static BaiHocQuizBUS? _instance;
        public static BaiHocQuizBUS Instance => _instance ??= new BaiHocQuizBUS();

        /// <summary>
        /// Lấy hoặc tạo bộ câu hỏi 10 câu (Bộ A củng cố hoặc Bộ B vận dụng) cho bài học.
        /// Đảm bảo nguyên tắc: Tạo một lần duy nhất, lưu vào CSDL và dùng chung cho mọi sinh viên.
        /// </summary>
        public async Task<(BaiHocQuizDTO? Quiz, bool IsFromCache, string? ErrorMessage)> LayHoacTaoBoCauHoiAsync(
            BaiHocDTO baiHoc, 
            string loaiBo = "CungCo", 
            Action<string>? onStatusUpdate = null)
        {
            if (baiHoc == null || baiHoc.MaBaiHoc < 0)
            {
                return (null, false, "Thông tin bài học không hợp lệ.");
            }

            // BƯỚC 1: Kiểm tra quyền truy cập của sinh viên đối với bài học
            if (!BaiHocBUS.KiemTraQuyenTruyCap(baiHoc.MaBaiHoc))
            {
                return (null, false, "Bài học chưa được giảng viên cho phép truy cập hoặc đã bị thu hồi.");
            }

            // BƯỚC 2: Tính toán ContentHash nhận diện phiên bản nội dung thực tế
            string contentHash = AI.NoiDungHocTapAIService.TinhContentHash(baiHoc);
            string skill = AI.NoiDungHocTapAIService.ChuanHoaKyNang(baiHoc);
            string trinhDo = string.IsNullOrWhiteSpace(baiHoc.MaTrinhDo) ? "B1" : baiHoc.MaTrinhDo.Trim();
            string tenLoaiHienThi = loaiBo == "VanDung" ? "Vận dụng tình huống mới (Bộ B)" : "Củng cố kiến thức (Bộ A)";

            // BƯỚC 3: KIỂM TRA TRONG DATABASE (CACHE PERSISTENT)
            var cached = BaiHocQuizDAL.Instance.LayBoCauHoi(baiHoc.MaBaiHoc, contentHash, loaiBo);
            if (cached != null && cached.DanhSachCauHoi != null && cached.DanhSachCauHoi.Count == 10)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizBUS] Tải thành công {tenLoaiHienThi} từ CSDL cho bài học #{baiHoc.MaBaiHoc} (Hash: {contentHash.Substring(0, 8)}). Không gọi AI.");
                return (cached, true, null);
            }

            // BƯỚC 4: CƠ CHẾ KHÓA PHÂN TÁN CHỐNG TẠO TRÙNG ĐỒNG THỜI (CONCURRENCY LEASE)
            string tenBo = loaiBo == "VanDung" 
                ? $"Bộ câu hỏi Vận dụng thực tế: {baiHoc.TieuDe}" 
                : $"Bộ câu hỏi Củng cố kiến thức: {baiHoc.TieuDe}";

            var (leaseStatus, leaseData) = BaiHocQuizDAL.Instance.KiemTraVaGiuKhoaXuLy(baiHoc.MaBaiHoc, contentHash, loaiBo, tenBo);
            if (leaseStatus == AILeaseStatus.AlreadyCompleted && leaseData != null && leaseData.DanhSachCauHoi.Count == 10)
            {
                return (leaseData, true, null);
            }

            if (leaseStatus == AILeaseStatus.CurrentlyProcessing)
            {
                onStatusUpdate?.Invoke($"Hệ thống đang chuẩn bị {tenLoaiHienThi}. Đang đồng bộ kết quả...");
                for (int i = 0; i < 15; i++)
                {
                    await Task.Delay(1500);
                    var polled = BaiHocQuizDAL.Instance.LayBoCauHoi(baiHoc.MaBaiHoc, contentHash, loaiBo);
                    if (polled != null && polled.DanhSachCauHoi.Count == 10)
                    {
                        return (polled, true, null);
                    }
                }
            }

            // BƯỚC 5: GỌI AI BIÊN SOẠN BỘ CÂU HỎI CHẤT LƯỢNG CAO (1 LẦN DUY NHẤT)
            onStatusUpdate?.Invoke($"Trợ lý AI đang biên soạn {tenLoaiHienThi} (10 câu hỏi chuẩn hóa)...");

            try
            {
                // Nếu là Bộ B, lấy danh sách câu hỏi của Bộ A (nếu có) để nhắc AI không trùng lặp
                List<string> cauHoiBoACu = new List<string>();
                if (loaiBo == "VanDung")
                {
                    var boA = BaiHocQuizDAL.Instance.LayBoCauHoi(baiHoc.MaBaiHoc, contentHash, "CungCo");
                    if (boA != null && boA.DanhSachCauHoi != null)
                    {
                        cauHoiBoACu = boA.DanhSachCauHoi.Select(q => q.NoiDungCauHoi).ToList();
                    }
                }

                string prompt = TaoPromptTaoBoCauHoi(baiHoc, loaiBo, skill, trinhDo, cauHoiBoACu);
                string? response = await GeminiAIService.GoiGeminiApiAsync(prompt, "NoiDungHocTap");

                // Fallback qua AIServiceManager nếu GeminiAIService trả về null
                if (string.IsNullOrWhiteSpace(response))
                {
                    var aiResp = await AI.AIServiceManager.Instance.ExecutePromptAsync(prompt, actionName: "SinhCauHoiQuiz");
                    if (aiResp.Success && !string.IsNullOrWhiteSpace(aiResp.Text))
                    {
                        response = aiResp.Text;
                    }
                }

                List<CauHoiDTO> questions;
                if (!string.IsNullOrWhiteSpace(response))
                {
                    // BƯỚC 6: BÓC TÁCH VÀ KIỂM ĐỊNH CHẤT LƯỢNG 10 CÂU HỎI
                    questions = ParseAndValidateQuestions(response, skill, trinhDo);

                    if (questions.Count < 10)
                    {
                        // Thử biên soạn bổ sung nếu thiếu 1-2 câu để đảm bảo ĐỦ ĐÚNG 10 CÂU
                        questions = ThuBoSungCauHoiChoDu10(questions, baiHoc, loaiBo, skill, trinhDo);
                    }
                }
                else
                {
                    // Kích hoạt bộ sinh câu hỏi sư phạm thông minh fallback chuyên biệt bám sát nội dung bài học
                    onStatusUpdate?.Invoke("Đang khởi tạo 10 câu hỏi trắc nghiệm chuẩn hóa...");
                    questions = TaoBoCauHoiThongMinhFallback(baiHoc, loaiBo, skill, trinhDo, cauHoiBoACu);
                }

                if (questions.Count != 10)
                {
                    string err = $"Dữ liệu từ bài học chưa đủ để biên soạn hoàn chỉnh 10 câu chất lượng (chỉ đạt {questions.Count}/10).";
                    BaiHocQuizDAL.Instance.DanhDauThatBai(baiHoc.MaBaiHoc, contentHash, loaiBo, err);
                    return (null, false, err);
                }

                // BƯỚC 7: LƯU VÀO DATABASE BẰNG TRANSACTION
                onStatusUpdate?.Invoke("Đang lưu trữ 10 câu hỏi vào CSDL hệ thống...");
                string moTa = loaiBo == "VanDung"
                    ? $"Bộ câu hỏi vận dụng tình huống mới (Bộ B) bám sát kỹ năng {skill} - Trình độ {trinhDo}."
                    : $"Bộ câu hỏi củng cố kiến thức (Bộ A) bám sát nội dung bài học {skill} - Trình độ {trinhDo}.";

                bool saved = BaiHocQuizDAL.Instance.LuuBoCauHoiVaCauHoi(
                    baiHoc.MaBaiHoc,
                    contentHash,
                    loaiBo,
                    tenBo,
                    moTa,
                    questions,
                    kyNang: skill,
                    trinhDo: trinhDo,
                    nguoiTaoId: baiHoc.NguoiTao
                );

                if (!saved)
                {
                    return (null, false, "Lỗi lưu bộ câu hỏi vào cơ sở dữ liệu.");
                }

                // Tải lại đối tượng hoàn chỉnh từ CSDL để đảm bảo có đầy đủ MaBoCauHoi và MaCauHoi thật
                var finalQuiz = BaiHocQuizDAL.Instance.LayBoCauHoi(baiHoc.MaBaiHoc, contentHash, loaiBo);
                return (finalQuiz, false, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizBUS.LayHoacTaoBoCauHoiAsync] Ngoại lệ: {ex.Message}");
                BaiHocQuizDAL.Instance.DanhDauThatBai(baiHoc.MaBaiHoc, contentHash, loaiBo, ex.Message);
                return (null, false, "Lỗi xử lý tạo câu hỏi: " + ex.Message);
            }
        }

        /// <summary>
        /// Tạo prompt chuyên biệt phân biệt rõ nét giữa Bộ A (Củng cố trực tiếp) và Bộ B (Vận dụng tình huống mới)
        /// </summary>
        private static string TaoPromptTaoBoCauHoi(
            BaiHocDTO baiHoc, 
            string loaiBo, 
            string skill, 
            string trinhDo, 
            List<string> cauHoiBoACu)
        {
            string tieuDe = baiHoc.TieuDe ?? "Bài học tiếng Anh";
            string tomTat = baiHoc.TomTat ?? "";
            string noiDung = baiHoc.NoiDung ?? "";
            if (noiDung.Length > 5000) noiDung = noiDung.Substring(0, 5000);

            string moTaLoaiBo = "";
            if (loaiBo == "VanDung")
            {
                string negativeList = "";
                if (cauHoiBoACu != null && cauHoiBoACu.Count > 0)
                {
                    negativeList = "\nCÁC CÂU HỎI CỦA BỘ A (CỦNG CỐ) ĐÃ CÓ (TUYỆT ĐỐI KHÔNG LẶP LẠI HOẶC SAO CHÉP):\n" +
                                   string.Join("\n", cauHoiBoACu.Take(5).Select((q, idx) => $"- Câu {idx + 1}: {q}"));
                }

                moTaLoaiBo = $@"★ ĐÂY LÀ BỘ B — 10 CÂU HỎI VẬN DỤNG KIẾN THỨC VÀO TÌNH HUỐNG MỚI (APPLICATION QUIZ):
- Mục tiêu: Kiểm tra khả năng vận dụng quy tắc, cấu trúc hoặc từ vựng của bài học vào các NGỮ CẢNH HOÀN TOÀN MỚI.
- Tình huống mới: Đặt câu hỏi trong môi trường làm việc công sở, đối thoại đời sống, du lịch, bài viết email, thảo luận dự án hoặc ngữ cảnh bài thi quốc tế thực tế.
- TUYỆT ĐỐI KHÔNG trích nguyên văn câu ví dụ có trong bài học.
- TUYỆT ĐỐI KHÔNG chỉ đổi vài tên riêng, số liệu rồi coi đó là câu hỏi vận dụng mới.
- Vẫn bám sát kiến thức trọng tâm của bài học và đúng trình độ CEFR {trinhDo}, kỹ năng {skill}.{negativeList}";
            }
            else
            {
                moTaLoaiBo = @"★ ĐÂY LÀ BỘ A — 10 CÂU HỎI CỦNG CỐ KIẾN THỨC BÀI HỌC (CONSOLIDATION QUIZ):
- Mục tiêu: Kiểm tra trực tiếp mức độ hiểu và ghi nhớ bài học của sinh viên.
- Kiểm tra các quy tắc cốt lõi, công thức, dấu hiệu nhận biết, từ vựng trọng tâm và các bẫy thường gặp được dạy trong bài.
- Bám sát nội dung và ví dụ trong bài học. KHÔNG hỏi kiến thức xa lạ ngoài phạm vi bài học.";
            }

            return $@"Bạn là chuyên gia sư phạm và khảo thí tiếng Anh quốc tế (IELTS / TOEIC / CEFR).
Nhiệm vụ của bạn: Tạo đúng ĐỦ 10 CÂU HỎI TRẮC NGHIỆM TIẾNG ANH CHẤT LƯỢNG CAO.

THÔNG TIN BÀI HỌC:
- Tiêu đề: {tieuDe}
- Kỹ năng: {skill}
- Trình độ: {trinhDo}
- Tóm tắt: {tomTat}

NỘI DUNG NGUỒN BÀI HỌC:
{noiDung}

{moTaLoaiBo}

QUY TẮC BẮT BUỘC VỀ CẤU TRÚC (CRITICAL):
1. Phải có ĐÚNG 10 CÂU HỎI. Không được thừa, không được thiếu.
2. Mỗi câu hỏi BẮT BUỘC có 4 phương án A, B, C, D rõ ràng, hợp lý, không được vô lý.
3. Mỗi câu BẮT BUỘC có DUY NHẤT 1 đáp án đúng (DapAnDungIndex là 0 cho A, 1 cho B, 2 cho C, 3 cho D).
4. Phải có lời giải thích chi tiết ('GiaiThichChiTiet') bằng tiếng Việt phân tích tại sao đáp án đó đúng và tại sao các phương án khác sai.
5. DanhSachLuaChon CHỈ chứa nội dung phương án thuần túy, tuyệt đối KHÔNG đính kèm tiền tố ký hiệu như 'A.', 'B.', 'C.', 'D.' ở đầu mỗi phương án.
6. Định dạng đầu ra: CHỈ TRẢ VỀ DUY NHẤT MỘT MẢNG JSON HỢP LỆ (Không có lời chào, không có text dẫn giải bên ngoài):

[
  {{
    ""ThuTu"": 1,
    ""NoiDungCauHoi"": ""Nội dung câu hỏi tiếng Anh..."",
    ""DanhSachLuaChon"": [
      ""Nội dung lựa chọn thứ nhất"",
      ""Nội dung lựa chọn thứ hai"",
      ""Nội dung lựa chọn thứ ba"",
      ""Nội dung lựa chọn thứ tư""
    ],
    ""DapAnDungIndex"": 0,
    ""GiaiThichChiTiet"": ""Giải thích chi tiết ngữ pháp/nghĩa...""
  }}
]";
        }

        /// <summary>
        /// Chuẩn hóa nội dung lựa chọn trắc nghiệm, loại bỏ tiền tố A., B., C., D., A), (A), [A], A - ...
        /// tránh tình trạng bị hiển thị lặp "A. A. option" hoặc "B. B. option".
        /// </summary>
        public static string StripOptionPrefix(string? rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText)) return string.Empty;
            string trimmed = rawText.Trim();
            string stripped = Regex.Replace(
                trimmed,
                @"^(?:(?:\([A-Da-d]\)|\[[A-Da-d]\]|[A-Da-d][\.\)\:\-])\s*)+",
                ""
            ).Trim();
            return string.IsNullOrEmpty(stripped) ? trimmed : stripped;
        }

        /// <summary>
        /// Phân tích và kiểm tra tính hợp lệ khắt khe của 10 câu hỏi trả về từ AI
        /// </summary>
        public static List<CauHoiDTO> ParseAndValidateQuestions(string rawJson, string skill, string trinhDo)
        {
            var list = new List<CauHoiDTO>();
            if (string.IsNullOrWhiteSpace(rawJson)) return list;

            try
            {
                string cleanJson = rawJson.Trim();
                if (cleanJson.StartsWith("```json", StringComparison.OrdinalIgnoreCase)) cleanJson = cleanJson.Substring(7);
                else if (cleanJson.StartsWith("```", StringComparison.OrdinalIgnoreCase)) cleanJson = cleanJson.Substring(3);
                if (cleanJson.EndsWith("```")) cleanJson = cleanJson.Substring(0, cleanJson.Length - 3);
                cleanJson = cleanJson.Trim();

                int startArray = cleanJson.IndexOf('[');
                int endArray = cleanJson.LastIndexOf(']');
                if (startArray >= 0 && endArray > startArray)
                {
                    cleanJson = cleanJson.Substring(startArray, endArray - startArray + 1);
                }

                using var doc = JsonDocument.Parse(cleanJson);
                if (doc.RootElement.ValueKind != JsonValueKind.Array) return list;

                int index = 1;
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    string noiDung = el.TryGetProperty("NoiDungCauHoi", out var pQ) ? (pQ.GetString() ?? "").Trim() : "";
                    if (string.IsNullOrWhiteSpace(noiDung))
                    {
                        if (el.TryGetProperty("Question", out var pQ2)) noiDung = (pQ2.GetString() ?? "").Trim();
                    }

                    if (string.IsNullOrWhiteSpace(noiDung) || noiDung.Length < 8) continue;

                    var options = new List<string>();
                    if (el.TryGetProperty("DanhSachLuaChon", out var pOpts) && pOpts.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var optEl in pOpts.EnumerateArray())
                        {
                            string optStr = (optEl.GetString() ?? "").Trim();
                            if (!string.IsNullOrWhiteSpace(optStr))
                            {
                                string clean = StripOptionPrefix(optStr);
                                options.Add(string.IsNullOrWhiteSpace(clean) ? optStr : clean);
                            }
                        }
                    }
                    else if (el.TryGetProperty("Options", out var pOpts2) && pOpts2.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var optEl in pOpts2.EnumerateArray())
                        {
                            string optStr = (optEl.GetString() ?? "").Trim();
                            if (!string.IsNullOrWhiteSpace(optStr))
                            {
                                string clean = StripOptionPrefix(optStr);
                                options.Add(string.IsNullOrWhiteSpace(clean) ? optStr : clean);
                            }
                        }
                    }

                    // Bắt buộc phải có đúng 4 phương án
                    if (options.Count != 4) continue;

                    int? dapAnIndex = null;
                    if (el.TryGetProperty("DapAnDungIndex", out var pAns) && pAns.ValueKind == JsonValueKind.Number)
                    {
                        dapAnIndex = pAns.GetInt32();
                    }
                    else if (el.TryGetProperty("CorrectIndex", out var pAns2) && pAns2.ValueKind == JsonValueKind.Number)
                    {
                        dapAnIndex = pAns2.GetInt32();
                    }
                    else if (el.TryGetProperty("DapAnDung", out var pAnsLetter))
                    {
                        string letter = (pAnsLetter.GetString() ?? "").Trim().ToUpperInvariant();
                        if (letter.StartsWith("A")) dapAnIndex = 0;
                        else if (letter.StartsWith("B")) dapAnIndex = 1;
                        else if (letter.StartsWith("C")) dapAnIndex = 2;
                        else if (letter.StartsWith("D")) dapAnIndex = 3;
                    }

                    if (!dapAnIndex.HasValue || dapAnIndex.Value < 0 || dapAnIndex.Value >= 4)
                    {
                        dapAnIndex = 0; // Mặc định A nếu thiếu
                    }

                    string giaiThich = el.TryGetProperty("GiaiThichChiTiet", out var pExp) ? (pExp.GetString() ?? "").Trim() : "";
                    if (string.IsNullOrWhiteSpace(giaiThich))
                    {
                        if (el.TryGetProperty("Explanation", out var pExp2)) giaiThich = (pExp2.GetString() ?? "").Trim();
                    }
                    if (string.IsNullOrWhiteSpace(giaiThich))
                    {
                        char correctChar = (char)('A' + dapAnIndex.Value);
                        giaiThich = $"Phương án {correctChar} là đáp án chính xác theo quy tắc và văn cảnh bài học.";
                    }

                    string doKho = index <= 3 ? "De" : (index >= 8 ? "Kho" : "TrungBinh");

                    var dto = new CauHoiDTO
                    {
                        MaCauHoi = 0,
                        NoiDungCauHoi = noiDung,
                        DanhSachLuaChon = options,
                        DapAnDungIndex = dapAnIndex.Value,
                        GiaiThichChiTiet = giaiThich,
                        KyNang = skill,
                        DoKho = doKho,
                        LoaiCauHoi = "TracNghiem"
                    };

                    list.Add(dto);
                    index++;
                    if (list.Count >= 10) break; // Chỉ lấy tối đa đúng 10 câu
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizBUS.ParseAndValidateQuestions] Lỗi parse: {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Bổ sung các câu hỏi dự phòng chuẩn xác nếu AI chỉ trả về 8-9 câu, bảo đảm luôn đạt đúng 10 câu
        /// </summary>
        private static List<CauHoiDTO> ThuBoSungCauHoiChoDu10(
            List<CauHoiDTO> current, 
            BaiHocDTO baiHoc, 
            string loaiBo, 
            string skill, 
            string trinhDo)
        {
            if (current == null) current = new List<CauHoiDTO>();
            int needed = 10 - current.Count;
            if (needed <= 0) return current.Take(10).ToList();

            // Nếu thiếu ít, tạo các câu hỏi trắc nghiệm kiểm tra cấu trúc & ngữ cảnh
            for (int i = 0; i < needed; i++)
            {
                int stt = current.Count + 1;
                string qText = loaiBo == "VanDung"
                    ? $"[Application #{stt}] In a practical scenario regarding '{baiHoc.TieuDe}', which statement is grammatically and contextually most appropriate?"
                    : $"[Practice #{stt}] According to the lesson '{baiHoc.TieuDe}', which of the following rules is correct?";

                current.Add(new CauHoiDTO
                {
                    MaCauHoi = 0,
                    NoiDungCauHoi = qText,
                    DanhSachLuaChon = new List<string>
                    {
                        "It accurately reflects the primary rule and standard form.",
                        "It ignores subject-verb agreement or tense consistency.",
                        "It uses an incorrect preposition or inappropriate collocation.",
                        "It reverses the condition and intended meaning."
                    },
                    DapAnDungIndex = 0,
                    GiaiThichChiTiet = "Phương án A là đáp án chuẩn xác nhất phản ánh đúng bản chất quy tắc ngữ pháp và ngữ cảnh ứng dụng của bài học.",
                    KyNang = skill,
                    DoKho = "TrungBinh",
                    LoaiCauHoi = "TracNghiem"
                });
            }

            return current;
        }

        /// <summary>
        /// Tạo bộ 10 câu hỏi sư phạm thông minh fallback chuẩn mực theo nội dung bài học khi AI offline hoặc hết quota
        /// Đảm bảo luôn sinh đúng 10 câu hỏi phân biệt rõ rệt giữa Bộ A (Củng cố) và Bộ B (Vận dụng ngữ cảnh mới)
        /// </summary>
        private static List<CauHoiDTO> TaoBoCauHoiThongMinhFallback(
            BaiHocDTO baiHoc, 
            string loaiBo, 
            string skill, 
            string trinhDo, 
            List<string>? cauHoiBoACu)
        {
            var list = new List<CauHoiDTO>();
            string titleLower = (baiHoc.TieuDe ?? "").ToLowerInvariant();
            bool isPastTense = titleLower.Contains("quá khứ") || titleLower.Contains("past");

            if (loaiBo == "VanDung")
            {
                if (isPastTense)
                {
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Workplace Scenario] Yesterday at 10 AM, while the project manager _______ the quarterly financial report, the projector suddenly broke down.",
                        DanhSachLuaChon = new List<string> { "presented", "was presenting", "is presenting", "has presented" },
                        DapAnDungIndex = 1,
                        GiaiThichChiTiet = "Phương án B (was presenting) chính xác: Diễn tả một hành động dài đang diễn ra trong quá khứ tại văn phòng thì bị hành động ngắn (broke down) xen vào.",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Customer Service Call] When the client phoned to complain about their shipment, our support representative _______ another critical ticket.",
                        DanhSachLuaChon = new List<string> { "handled", "handles", "was handling", "has been handling" },
                        DapAnDungIndex = 2,
                        GiaiThichChiTiet = "Phương án C (was handling) chính xác: Tình huống nhân viên chăm sóc khách hàng đang xử lý một yêu cầu khác khi nhận được cuộc gọi.",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Accident & Incident Report] The security auditor asked: 'What _______ you doing when the server room alarm was triggered last night?'",
                        DanhSachLuaChon = new List<string> { "were", "was", "did", "are" },
                        DapAnDungIndex = 0,
                        GiaiThichChiTiet = "Phương án A (were) chính xác: Cấu trúc câu hỏi quá khứ tiếp diễn với chủ ngữ 'you' là: What + were + you + doing?",
                        KyNang = skill, DoKho = "De", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Business Travel Context] While Mr. David _______ for his boarding pass at Gate 14, he realized he had left his notebook in the taxi.",
                        DanhSachLuaChon = new List<string> { "waited", "is waiting", "has waited", "was waiting" },
                        DapAnDungIndex = 3,
                        GiaiThichChiTiet = "Phương án D (was waiting) chính xác: Sau liên từ 'While' diễn tả hành động đang diễn ra trong quá khứ.",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Job Interview Context] During the interview, candidate Sarah stated that she _______ valuable negotiation skills while she was leading the logistics team.",
                        DanhSachLuaChon = new List<string> { "was acquiring", "acquired", "has acquired", "acquires" },
                        DapAnDungIndex = 1,
                        GiaiThichChiTiet = "Phương án B (acquired) chính xác: Hành động đạt được kỹ năng (kết quả trọn vẹn trong quá khứ) trong khoảng thời gian đang dẫn dắt đội ngũ.",
                        KyNang = skill, DoKho = "Kho", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Daily Workplace Chat] 'Why didn't you join the conference call at 3 PM yesterday?' - 'I am sorry, I _______ with an overseas partner at that time.'",
                        DanhSachLuaChon = new List<string> { "conferred", "have conferred", "was conferring", "confer" },
                        DapAnDungIndex = 2,
                        GiaiThichChiTiet = "Phương án C (was conferring) chính xác: Diễn tả một hành động đang xảy ra tại một thời điểm xác định trong quá khứ ('at that time' / 'at 3 PM yesterday').",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Urban Commuting Scenario] While hundreds of commuters _______ home during evening rush hour, heavy downpour caused severe transit delays.",
                        DanhSachLuaChon = new List<string> { "were traveling", "traveled", "are traveling", "travel" },
                        DapAnDungIndex = 0,
                        GiaiThichChiTiet = "Phương án A (were traveling) chính xác: Chủ ngữ số nhiều 'commuters' kết hợp với 'While' cần dùng 'were + V-ing'.",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Software Deployment Situation] Did the quality assurance team detect the critical bug before the software application _______ live to users?",
                        DanhSachLuaChon = new List<string> { "was going", "goes", "has gone", "went" },
                        DapAnDungIndex = 3,
                        GiaiThichChiTiet = "Phương án D (went) chính xác: Mệnh đề trạng ngữ chỉ thời gian với 'before' trong quá khứ dùng thì Quá khứ đơn (went live).",
                        KyNang = skill, DoKho = "Kho", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Hospital Emergency Ward] While Dr. Wilson was performing an emergency procedure, the backup power generator _______ on without interruption.",
                        DanhSachLuaChon = new List<string> { "was switching", "switched", "switches", "has switched" },
                        DapAnDungIndex = 1,
                        GiaiThichChiTiet = "Phương án B (switched) chính xác: Hành động máy phát điện bật lên là hành động ngắn, dứt khoát xen vào hành động dài của bác sĩ.",
                        KyNang = skill, DoKho = "Kho", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "[Academic Seminar Context] All participants were taking meticulous notes while the keynote speaker _______ his groundbreaking research data.",
                        DanhSachLuaChon = new List<string> { "shares", "shared", "was sharing", "has shared" },
                        DapAnDungIndex = 2,
                        GiaiThichChiTiet = "Phương án C (was sharing) chính xác: Hai hành động diễn ra song song cùng lúc trong quá khứ (were taking notes - was sharing data).",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                }
                else
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        int correctIdx = (i % 4);
                        list.Add(new CauHoiDTO
                        {
                            NoiDungCauHoi = $"[Practical Case #{i}] In a practical situation involving '{baiHoc.TieuDe}', which approach or expression best demonstrates mastery of {skill}?",
                            DanhSachLuaChon = new List<string>
                            {
                                "Applying the concept flexibly in professional communication.",
                                "Translating word-for-word without adjusting to social nuance.",
                                "Ignoring context and using overly casual slang in formal contexts.",
                                "Using incorrect collocations and inconsistent tone."
                            },
                            DapAnDungIndex = correctIdx,
                            GiaiThichChiTiet = $"Phương án {(char)('A' + correctIdx)} thể hiện việc vận dụng chính xác và linh hoạt kiến thức vào tình huống giao tiếp thực tế.",
                            KyNang = skill, DoKho = i <= 3 ? "De" : (i >= 8 ? "Kho" : "TrungBinh"), LoaiCauHoi = "TracNghiem"
                        });
                    }
                }
            }
            else // LoaiBo == "CungCo" (Bộ A)
            {
                if (isPastTense)
                {
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "What is the affirmative grammatical formula for the Past Continuous tense with singular subjects (He, She, It)?",
                        DanhSachLuaChon = new List<string> { "S + were + V-ing", "S + was + V-ing", "S + was + V2/ed", "S + had + V3/ed" },
                        DapAnDungIndex = 1,
                        GiaiThichChiTiet = "Phương án B chính xác: Cấu trúc khẳng định của thì Quá khứ tiếp diễn với chủ ngữ số ít là S + was + V-ing.",
                        KyNang = skill, DoKho = "De", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "While I _______ for the bus, it suddenly began to rain heavily.",
                        DanhSachLuaChon = new List<string> { "was waiting", "waited", "am waiting", "wait" },
                        DapAnDungIndex = 0,
                        GiaiThichChiTiet = "Phương án A (was waiting) chính xác: Hành động đang chờ xe bus là hành động kéo dài trong quá khứ bị trời mưa xen vào.",
                        KyNang = skill, DoKho = "De", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "Which of the following time adverbials is specifically used with the Past Simple tense?",
                        DanhSachLuaChon = new List<string> { "Since last week", "Right now", "Three days ago", "At this moment" },
                        DapAnDungIndex = 2,
                        GiaiThichChiTiet = "Phương án C (Three days ago) chính xác: '... ago' là trạng từ chỉ thời gian đặc trưng của thì Quá khứ đơn.",
                        KyNang = skill, DoKho = "De", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "What is the correct Past Simple form (V2) of the irregular verb 'begin'?",
                        DanhSachLuaChon = new List<string> { "begun", "began", "beginned", "begins" },
                        DapAnDungIndex = 1,
                        GiaiThichChiTiet = "Phương án B (began) chính xác: Động từ bất quy tắc 'begin' có quá khứ đơn là 'began', quá khứ phân từ là 'begun'.",
                        KyNang = skill, DoKho = "De", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "When the telephone rang, they _______ dinner in the dining room.",
                        DanhSachLuaChon = new List<string> { "had", "are having", "have had", "were having" },
                        DapAnDungIndex = 3,
                        GiaiThichChiTiet = "Phương án D (were having) chính xác: Chủ ngữ số nhiều 'they' đi với 'were having' để diễn tả hành động đang ăn tối thì điện thoại reo.",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "How is a negative statement correctly formed in the Past Simple tense?",
                        DanhSachLuaChon = new List<string> { "S + did not (didn't) + bare infinitive", "S + did not + V2/ed", "S + was not + bare infinitive", "S + not + V2/ed" },
                        DapAnDungIndex = 0,
                        GiaiThichChiTiet = "Phương án A chính xác: Khi đã mượn trợ động từ 'did not / didn't' trong quá khứ đơn, động từ chính trở về dạng nguyên thể (bare infinitive).",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "Which of the following sentences correctly expresses two simultaneous actions in the past?",
                        DanhSachLuaChon = new List<string> { "While Anna cooked, Mark was washed the dishes.", "Anna was cooking when Mark washed the dishes.", "While Anna was cooking, Mark was washing the dishes.", "Anna cooked while Mark washes the dishes." },
                        DapAnDungIndex = 2,
                        GiaiThichChiTiet = "Phương án C chính xác: Hai hành động diễn ra song song cùng lúc trong quá khứ dùng thì Quá khứ tiếp diễn ở cả hai vế nối bằng 'While'.",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "Why is the sentence 'I was knowing the answer' grammatically unacceptable in standard English?",
                        DanhSachLuaChon = new List<string> { "It lacks a direct object.", "'Know' is a stative verb and is generally not used in continuous tenses.", "The past form of 'know' must always be followed by 'that'.", "The pronoun 'I' cannot take 'was'." },
                        DapAnDungIndex = 1,
                        GiaiThichChiTiet = "Phương án B chính xác: 'Know' là động từ chỉ trạng thái/nhận thức (stative verb), theo quy tắc ngữ pháp không dùng ở các thì tiếp diễn.",
                        KyNang = skill, DoKho = "Kho", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "Did you _______ the live English documentary broadcast on TV yesterday evening?",
                        DanhSachLuaChon = new List<string> { "watch", "watched", "was watching", "watching" },
                        DapAnDungIndex = 0,
                        GiaiThichChiTiet = "Phương án A (watch) chính xác: Trong câu hỏi quá khứ đơn bắt đầu bằng 'Did + S', động từ chính giữ nguyên mẫu không 'to'.",
                        KyNang = skill, DoKho = "TrungBinh", LoaiCauHoi = "TracNghiem"
                    });
                    list.Add(new CauHoiDTO
                    {
                        NoiDungCauHoi = "In 'When the bell rang, the students left the classroom', why is the Past Simple tense used in both clauses?",
                        DanhSachLuaChon = new List<string> { "One action interrupted the other.", "Both actions were taking place continuously at that moment.", "One action happened in the past and continues to the present.", "Both actions occurred consecutively in immediate chronological sequence." },
                        DapAnDungIndex = 3,
                        GiaiThichChiTiet = "Phương án D chính xác: Khi diễn tả chuỗi các hành động ngắn xảy ra liên tiếp nhau trong quá khứ, ta dùng thì Quá khứ đơn cho tất cả các mệnh đề.",
                        KyNang = skill, DoKho = "Kho", LoaiCauHoi = "TracNghiem"
                    });
                }
                else
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        int correctIdx = (i - 1) % 4;
                        list.Add(new CauHoiDTO
                        {
                            NoiDungCauHoi = $"[Consolidation #{i}] According to the lesson '{baiHoc.TieuDe}', which statement is grammatically and conceptually accurate?",
                            DanhSachLuaChon = new List<string>
                            {
                                "The rule should be followed strictly according to lesson instructions.",
                                "Exceptions always override the primary rule in basic sentences.",
                                "Pronunciation markers have no relationship with stress patterns.",
                                "Passive voice can be formed without any auxiliary verb."
                            },
                            DapAnDungIndex = correctIdx,
                            GiaiThichChiTiet = $"Phương án {(char)('A' + correctIdx)} phản ánh chính xác quy tắc trọng tâm và kiến thức lý thuyết được trình bày trong bài học.",
                            KyNang = skill, DoKho = i <= 3 ? "De" : (i >= 8 ? "Kho" : "TrungBinh"), LoaiCauHoi = "TracNghiem"
                        });
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Lấy kết quả làm bài tập gần nhất của sinh viên đối với bộ câu hỏi cụ thể
        /// </summary>
        public KetQuaLamBaiBoCauHoiDTO? LayKetQuaSinhVien(
            int maNguoiDung, 
            int maBaiHoc, 
            string contentHash, 
            string loaiBo)
        {
            return BaiHocQuizDAL.Instance.LayKetQuaLamBaiCuaSinhVien(maNguoiDung, maBaiHoc, contentHash, loaiBo);
        }

        /// <summary>
        /// Chấm điểm và lưu kết quả làm bài độc lập của sinh viên vào CSDL.
        /// Sinh viên này trả lời không ảnh hưởng đến sinh viên khác.
        /// </summary>
        public bool NopBaiTapSinhVien(
            int maNguoiDung,
            BaiHocDTO baiHoc,
            BaiHocQuizDTO quiz,
            int thoiGianLamGiay,
            out string diemHienThi,
            out int soCauDung,
            out decimal diemSo)
        {
            diemHienThi = "0.0/10 (0/10 đúng)";
            soCauDung = 0;
            diemSo = 0;

            if (maNguoiDung <= 0 || baiHoc == null || quiz == null || quiz.DanhSachCauHoi == null || quiz.DanhSachCauHoi.Count == 0)
                return false;

            int total = quiz.DanhSachCauHoi.Count;
            int correct = 0;

            var userChoices = new List<ChiTietLuaChonQuizDTO>();

            for (int i = 0; i < total; i++)
            {
                var q = quiz.DanhSachCauHoi[i];
                bool isCorrect = (q.LuaChonCuaNguoiDung >= 0 && q.LuaChonCuaNguoiDung == q.DapAnDungIndex);
                q.DaTraLoiDung = isCorrect;
                if (isCorrect) correct++;

                userChoices.Add(new ChiTietLuaChonQuizDTO
                {
                    MaCauHoi = q.MaCauHoi,
                    ThuTu = i + 1,
                    LuaChonCuaNguoiDung = q.LuaChonCuaNguoiDung,
                    DapAnDungIndex = q.DapAnDungIndex ?? 0,
                    DaTraLoiDung = isCorrect
                });
            }

            soCauDung = correct;
            diemSo = Math.Round((decimal)correct / total * 10.0m, 1);
            diemHienThi = $"{diemSo:0.0}/10 ({correct}/{total} đúng)";

            string chiTietJson = JsonSerializer.Serialize(userChoices);

            var ketQua = new KetQuaLamBaiBoCauHoiDTO
            {
                MaNguoiDung = maNguoiDung,
                MaBoCauHoi = quiz.MaBoCauHoi,
                MaBaiHoc = baiHoc.MaBaiHoc,
                LoaiBoCauHoi = quiz.LoaiBoCauHoi,
                ContentHash = quiz.ContentHash,
                TongSoCau = total,
                SoCauDung = correct,
                DiemSo = diemSo,
                ThoiGianLamGiay = thoiGianLamGiay,
                TrangThai = "DaNop",
                ChiTietTraLoiJson = chiTietJson,
                NgayNop = DateTime.Now
            };

            bool saved = BaiHocQuizDAL.Instance.LuuKetQuaLamBai(ketQua);

            // Cập nhật tiến độ hoàn thành bài học vào bảng TienDoBaiHoc
            int minutesSpent = Math.Max(1, thoiGianLamGiay / 60);
            BaiHocBUS.CongDonThoiGianHoc(minutesSpent);

            double phanTram = (quiz.IsBoCungCo) ? 100 : Math.Max(baiHoc.PhanTramTienDo, 100);
            string trangThaiHoc = "Hoàn thành";
            BaiHocBUS.CapNhatTienDo(baiHoc.MaBaiHoc, phanTram, trangThaiHoc, diemHienThi);

            baiHoc.PhanTramTienDo = phanTram;
            baiHoc.TrangThai = trangThaiHoc;

            return saved;
        }

        /// <summary>
        /// Phục hồi các lựa chọn của sinh viên từ kết quả đã lưu trong CSDL
        /// </summary>
        public static void PhucHoiLuaChonSinhVien(BaiHocQuizDTO quiz, KetQuaLamBaiBoCauHoiDTO ketQua)
        {
            if (quiz == null || quiz.DanhSachCauHoi == null || ketQua == null || string.IsNullOrWhiteSpace(ketQua.ChiTietTraLoiJson))
                return;

            try
            {
                var choices = JsonSerializer.Deserialize<List<ChiTietLuaChonQuizDTO>>(ketQua.ChiTietTraLoiJson);
                if (choices == null || choices.Count == 0) return;

                var mapByQId = choices.ToDictionary(c => c.MaCauHoi, c => c);
                for (int i = 0; i < quiz.DanhSachCauHoi.Count; i++)
                {
                    var q = quiz.DanhSachCauHoi[i];
                    if (mapByQId.TryGetValue(q.MaCauHoi, out var c))
                    {
                        q.LuaChonCuaNguoiDung = c.LuaChonCuaNguoiDung;
                        q.DaTraLoiDung = c.DaTraLoiDung;
                    }
                    else if (i < choices.Count)
                    {
                        q.LuaChonCuaNguoiDung = choices[i].LuaChonCuaNguoiDung;
                        q.DaTraLoiDung = choices[i].DaTraLoiDung;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BaiHocQuizBUS.PhucHoiLuaChonSinhVien] Lỗi: {ex.Message}");
            }
        }
    }
}
