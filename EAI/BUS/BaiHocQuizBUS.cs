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
            Action<string>? onStatusUpdate = null,
            bool forceRegenerate = false)
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

            // BƯỚC 3: KIỂM TRA TRONG DATABASE (CACHE PERSISTENT) - Bỏ qua nếu forceRegenerate
            if (!forceRegenerate)
            {
                var cached = BaiHocQuizDAL.Instance.LayBoCauHoi(baiHoc.MaBaiHoc, contentHash, loaiBo);
                if (cached != null && cached.DanhSachCauHoi != null && cached.DanhSachCauHoi.Count == 10)
                {
                    System.Diagnostics.Debug.WriteLine($"[BaiHocQuizBUS] Tải thành công {tenLoaiHienThi} từ CSDL cho bài học #{baiHoc.MaBaiHoc} (Hash: {contentHash.Substring(0, 8)}). Không gọi AI.");
                    return (cached, true, null);
                }
            }

            // BƯỚC 4: CƠ CHẾ KHÓA PHÂN TÁN CHỐNG TẠO TRÙNG ĐỒNG THỜI (CONCURRENCY LEASE)
            string tenBo = loaiBo == "VanDung" 
                ? $"Bộ câu hỏi Vận dụng thực tế: {baiHoc.TieuDe}" 
                : $"Bộ câu hỏi Củng cố kiến thức: {baiHoc.TieuDe}";

            if (!forceRegenerate)
            {
                var (leaseStatus, leaseData) = BaiHocQuizDAL.Instance.KiemTraVaGiuKhoaXuLy(baiHoc.MaBaiHoc, contentHash, loaiBo, tenBo);
                if (leaseStatus == AILeaseStatus.AlreadyCompleted && leaseData != null && leaseData.DanhSachCauHoi.Count == 10)
                {
                    return (leaseData, true, null);
                }

                if (leaseStatus == AILeaseStatus.CurrentlyProcessing)
                {
                    onStatusUpdate?.Invoke($"Hệ thống đang chuẩn bị {tenLoaiHienThi}. Đang đồng bộ kết quả...");
                    for (int i = 0; i < 3; i++)
                    {
                        await Task.Delay(1500);
                        var polled = BaiHocQuizDAL.Instance.LayBoCauHoi(baiHoc.MaBaiHoc, contentHash, loaiBo);
                        if (polled != null && polled.DanhSachCauHoi.Count == 10)
                        {
                            return (polled, true, null);
                        }
                    }
                    return (null, false, "Bộ câu hỏi đang được tạo trên một phiên khác. Vui lòng chờ rồi nhấn Thử lại.");
                }
            }

            // BƯỚC 5: GỌI AI BIÊN SOẠN BỘ CÂU HỎI CHẤT LƯỢNG CAO THEO MÔ HÌNH GIẢNG VIÊN ĐÃ LƯU
            AI.AIServiceManager.Instance.SyncActiveConfigFromStorage();
            string moHinhHienTai = AI.AIServiceManager.Instance.ActiveModel ?? "Mặc định";
            onStatusUpdate?.Invoke($"Trợ lý AI ({AI.AIServiceManager.Instance.ActiveProviderType} - {moHinhHienTai}) đang biên soạn {tenLoaiHienThi} (10 câu hỏi)...");

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

                // Ưu tiên gọi đúng mô hình AI mà Giảng viên đã chọn và lưu
                var aiResp = await AI.AIServiceManager.Instance.ExecutePromptAsync(prompt, actionName: $"SinhCauHoiQuiz_{loaiBo}");
                string? response = (aiResp != null && aiResp.Success && !string.IsNullOrWhiteSpace(aiResp.Text)) ? aiResp.Text : null;

                // Không tự bù câu mẫu khi dịch vụ lỗi hoặc trả thiếu câu.
                if (string.IsNullOrWhiteSpace(response))
                    throw new InvalidOperationException(aiResp?.ErrorMessage ?? "Dịch vụ AI chưa trả về câu hỏi. Vui lòng thử lại.");

                var questions = ParseAndValidateQuestions(response, skill, trinhDo);
                if (!LessonQuizQuality.IsValidSet(questions))
                {
                    onStatusUpdate?.Invoke("Đang yêu cầu AI biên soạn lại bộ câu hỏi chưa đạt yêu cầu...");
                    var retry = await AI.AIServiceManager.Instance.ExecutePromptAsync(
                        prompt + "\nIMPORTANT: Return exactly 10 DISTINCT complete questions with four distinct options, correct answer index and specific explanation. Never use placeholders or generic sample questions.",
                        actionName: $"SinhCauHoiQuiz_{loaiBo}_Retry");
                    if (retry == null || !retry.Success || string.IsNullOrWhiteSpace(retry.Text))
                        throw new InvalidOperationException(retry?.ErrorMessage ?? "Không nhận được câu hỏi khi thử lại. Vui lòng thử lại sau.");
                    questions = ParseAndValidateQuestions(retry.Text, skill, trinhDo);
                }
                if (!LessonQuizQuality.IsValidSet(questions))
                    throw new InvalidOperationException("AI chưa tạo đủ 10 câu hỏi khác nhau với đáp án và lời giải hợp lệ. Vui lòng nhấn Thử lại; bộ câu hỏi lỗi chưa được lưu.");
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
                return finalQuiz != null
                    ? (finalQuiz, false, null)
                    : (null, false, "Đã gửi lưu câu hỏi nhưng chưa tải lại được bộ hoàn chỉnh. Vui lòng thử lại.");
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

            string? filePath = baiHoc.DuongDanTepVatLy;
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                filePath = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(baiHoc);
                if (!string.IsNullOrWhiteSpace(filePath)) baiHoc.DuongDanTepVatLy = filePath;
            }
            string rawContent = KhoHocLieuSyncHelper.DocNoiDungTep(filePath, baiHoc.NoiDung);
            string noiDung = KhoHocLieuSyncHelper.ChuyenDoiHtmlSangVanBan(rawContent);
            if (noiDung.Length > 5000) noiDung = noiDung.Substring(0, 5000);

            string moTaLoaiBo = "";
            if (loaiBo == "VanDung")
            {
                string negativeList = "";
                if (cauHoiBoACu != null && cauHoiBoACu.Count > 0)
                {
                    negativeList = "\nCÁC CÂU HỎI CỦA BỘ A (CỦNG CỐ) ĐÃ CÓ (TUYỆT ĐỐI KHÔNG LẶP LẠI HOẶC SAO CHÉP):\n" +
                                   string.Join("\n", cauHoiBoACu.Select((q, idx) => $"- Câu {idx + 1}: {q}"));
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
            if (string.IsNullOrWhiteSpace(rawJson)) return new();
            try
            {
                var json = rawJson.Trim();
                if (json.StartsWith("```"))
                {
                    int newline = json.IndexOf('\n');
                    if (newline < 0) return new();
                    json = json.Substring(newline + 1).Trim();
                    if (json.EndsWith("```")) json = json.Substring(0, json.Length - 3).Trim();
                }
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in root.EnumerateObject())
                    {
                        if (new[] { "questions", "danhsachcauhoi", "items" }.Contains(property.Name.ToLowerInvariant()) && property.Value.ValueKind == JsonValueKind.Array)
                        { root = property.Value; break; }
                    }
                }
                if (root.ValueKind != JsonValueKind.Array) return new();
                var result = new List<CauHoiDTO>();
                foreach (var item in root.EnumerateArray())
                {
                    if (item.ValueKind != JsonValueKind.Object) continue;
                    try
                    {
                        var q = JsonSerializer.Deserialize<CauHoiDTO>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (q == null) continue;
                        // Chấp nhận tên thuộc tính tiếng Anh của một số model.
                        foreach (var p in item.EnumerateObject())
                        {
                            if (p.Name.Equals("Question", StringComparison.OrdinalIgnoreCase) && p.Value.ValueKind == JsonValueKind.String) q.NoiDungCauHoi = p.Value.GetString() ?? "";
                            if (p.Name.Equals("Options", StringComparison.OrdinalIgnoreCase) && p.Value.ValueKind == JsonValueKind.Array) q.DanhSachLuaChon = JsonSerializer.Deserialize<List<string>>(p.Value.GetRawText()) ?? new();
                            if (p.Name.Equals("CorrectIndex", StringComparison.OrdinalIgnoreCase) && p.Value.TryGetInt32(out var answer)) q.DapAnDungIndex = answer;
                            if (p.Name.Equals("Explanation", StringComparison.OrdinalIgnoreCase) && p.Value.ValueKind == JsonValueKind.String) q.GiaiThichChiTiet = p.Value.GetString() ?? "";
                            if (p.Name.Equals("DapAnDung", StringComparison.OrdinalIgnoreCase) && p.Value.ValueKind == JsonValueKind.String)
                            {
                                var letter = (p.Value.GetString() ?? "").Trim().ToUpperInvariant();
                                if (letter.Length == 1 && letter[0] >= 'A' && letter[0] <= 'D') q.DapAnDungIndex = letter[0] - 'A';
                            }
                        }
                        if (!LessonQuizQuality.IsValidQuestion(q)) continue;
                        q.DanhSachLuaChon = q.DanhSachLuaChon.Select(StripOptionPrefix).ToList();
                        q.KyNang = skill;
                        q.LoaiCauHoi = "TracNghiem";
                        if (string.IsNullOrWhiteSpace(q.DoKho)) q.DoKho = "TrungBinh";
                        result.Add(q);
                    }
                    catch (Exception ex) when (ex is JsonException || ex is InvalidOperationException || ex is FormatException) { }
                }
                return result;
            }
            catch (JsonException) { return new(); }
        }
        public static bool KetQuaThuocBoCauHoi(BaiHocQuizDTO quiz, KetQuaLamBaiBoCauHoiDTO ketQua)
        {
            if (string.IsNullOrWhiteSpace(ketQua.ChiTietTraLoiJson)) return false;
            try
            {
                var choices = JsonSerializer.Deserialize<List<ChiTietLuaChonQuizDTO>>(ketQua.ChiTietTraLoiJson);
                return choices != null && choices.Count == quiz.DanhSachCauHoi.Count &&
                    choices.Select(c => c.MaCauHoi).ToHashSet().SetEquals(quiz.DanhSachCauHoi.Select(q => q.MaCauHoi));
            }
            catch (JsonException) { return false; }
        }

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
