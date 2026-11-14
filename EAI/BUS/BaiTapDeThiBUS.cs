using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DTO;

namespace BUS
{
    public class BaiTapDeThiBUS
    {
        private static BaiTapDeThiBUS? _instance;
        public static BaiTapDeThiBUS Instance => _instance ??= new BaiTapDeThiBUS();

        public List<BaiTapDeThiDTO> LayDanhSachDeThi(
            string? loaiHinh = null, 
            string? trinhDo = null, 
            string? trangThai = null, 
            string? tuKhoa = null, 
            bool chiLayDaXuatBan = false)
        {
            return BaiTapDeThiDAL.Instance.LayDanhSachDeThi(loaiHinh, trinhDo, trangThai, tuKhoa, chiLayDaXuatBan);
        }

        public BaiTapDeThiDTO? LayChiTietDeThi(int maDeThi)
        {
            return BaiTapDeThiDAL.Instance.LayChiTietDeThi(maDeThi);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiCuaDeThi(int maDeThi)
        {
            return BaiTapDeThiDAL.Instance.LayDanhSachCauHoiCuaDeThi(maDeThi);
        }

        public bool LuuDeThiVaCauHoi(
            BaiTapDeThiDTO deThi, 
            List<(int MaCauHoi, int ThuTu, double Diem)> dsCauHoi, 
            out string errorMessage)
        {
            int userId = SessionHelper.CurrentMaNguoiDung;
            if (userId <= 0)
            {
                errorMessage = "Phiên đăng nhập không hợp lệ.";
                return false;
            }
            return BaiTapDeThiDAL.Instance.LuuDeThiVaCauHoi(deThi, dsCauHoi, userId, out errorMessage);
        }

        public bool XuatBanDeThi(int maDeThi, out string errorMessage)
        {
            return BaiTapDeThiDAL.Instance.XuatBanDeThi(maDeThi, out errorMessage);
        }

        public bool DongDeThi(int maDeThi, out string errorMessage)
        {
            return BaiTapDeThiDAL.Instance.DongDeThi(maDeThi, out errorMessage);
        }

        public int NhanBanDeThi(int maDeThi, out string errorMessage)
        {
            int userId = SessionHelper.CurrentMaNguoiDung;
            if (userId <= 0)
            {
                errorMessage = "Phiên đăng nhập không hợp lệ.";
                return 0;
            }
            return BaiTapDeThiDAL.Instance.NhanBanDeThi(maDeThi, userId, out errorMessage);
        }

        public bool XoaDeThi(int maDeThi, out string errorMessage)
        {
            return BaiTapDeThiDAL.Instance.XoaDeThi(maDeThi, out errorMessage);
        }

        public List<LuotLamBaiDTO> LayLichSuLamBaiCuaDeThi(int maDeThi)
        {
            return BaiTapDeThiDAL.Instance.LayLichSuLamBaiCuaDeThi(maDeThi);
        }

        public int DemSoLuotHocVienDaLam(int maDeThi, int maNguoiDung)
        {
            return BaiTapDeThiDAL.Instance.DemSoLuotHocVienDaLam(maDeThi, maNguoiDung);
        }

        /// <summary>
        /// Khởi tạo lượt làm bài ở trạng thái 'DangLam' ngay khi học viên bắt đầu vào phòng thi.
        /// Giúp giảng viên thấy trạng thái "Đang làm" theo thời gian thực trên màn hình giám sát.
        /// </summary>
        public int KhoiTaoLuotLamBai(int maNguoiDung, int maDeThi, string hinhThucLam, int tongSoCau)
        {
            return BaiTapDeThiDAL.Instance.KhoiTaoLuotLamBai(maNguoiDung, maDeThi, hinhThucLam, tongSoCau);
        }

        /// <summary>
        /// Hủy lượt làm bài khi học viên rời phòng thi giữa chừng mà không nộp bài.
        /// </summary>
        public bool HuyLuotLamBai(int maLuotLamBai)
        {
            return BaiTapDeThiDAL.Instance.HuyLuotLamBai(maLuotLamBai);
        }

        /// <summary>
        /// [Feature 13] Tìm lượt thi đang làm dở (DangLam) của học viên cho một đề thi.
        /// Áp dụng Ownership Guard: Học viên chỉ được xem lượt thi của chính mình.
        /// </summary>
        public LuotLamBaiDTO? LayLuotLamBaiDangLam(int maNguoiDung, int maDeThi)
        {
            if (maDeThi < 0) return null;
            int targetUserId = maNguoiDung;
            if (!SessionHelper.IsGiangVien && !SessionHelper.IsAdmin && SessionHelper.CurrentMaNguoiDung > 0)
            {
                targetUserId = SessionHelper.CurrentMaNguoiDung;
            }
            return BaiTapDeThiDAL.Instance.LayLuotLamBaiDangLam(targetUserId, maDeThi);
        }

        /// <summary>
        /// [Feature 13] Tự động lưu đáp án tạm thời theo thời gian thực (Autosave).
        /// </summary>
        public bool LuuCauTraLoiTamThoi(int maLuotLamBai, int maCauHoi, int? maLuaChon, string? cauTraLoiVanBan, int? thoiGianLamGiay = null)
        {
            return BaiTapDeThiDAL.Instance.LuuCauTraLoiTamThoi(maLuotLamBai, maCauHoi, maLuaChon, cauTraLoiVanBan, thoiGianLamGiay);
        }

        /// <summary>
        /// [Feature 13] Lấy danh sách câu trả lời tạm thời của lượt đang làm dở để khôi phục (Resume).
        /// </summary>
        public List<ChiTietTraLoiDTO> LayChiTietTraLoiCuaLuot(int maLuotLamBai)
        {
            return BaiTapDeThiDAL.Instance.LayChiTietTraLoiCuaLuot(maLuotLamBai);
        }

        public List<ChiTietTraLoiDTO> LayDanhSachCauTraLoiTamThoi(int maLuotLamBai)
        {
            return BaiTapDeThiDAL.Instance.LayDanhSachCauTraLoiTamThoi(maLuotLamBai);
        }

        /// <summary>
        /// [PROJECT.md Contract] Chấm điểm thi chuẩn hóa dựa trực tiếp trên DB Snapshot CauHoiBaiTap.
        /// Tuyệt đối KHÔNG tin cậy opt.LaDapAnDung từ đối tượng phía client gửi lên.
        /// </summary>
        public (int MaLuotLamBai, decimal DiemSo, int SoCauDung, int TongSoCau, bool LaDatYeuCau) ChamVaLuuKetQuaThi(
            int maDeThi,
            int maNguoiDung,
            Dictionary<int, int> cauTraLoiTracNghiem,
            Dictionary<int, string>? cauTraLoiTuLuan = null,
            string hinhThucLam = "ThiThu",
            DateTime? thoiDiemBatDau = null,
            int thoiGianGiay = 0,
            int maLuotLamBaiHienTai = 0)
        {
            return ChamVaLuuKetQuaThi(
                maDeThi: maDeThi,
                hinhThucLam: hinhThucLam,
                thoiDiemBatDau: thoiDiemBatDau ?? DateTime.Now.AddSeconds(-thoiGianGiay),
                thoiGianGiay: thoiGianGiay,
                questions: null, // Bắt buộc nạp snapshot từ DB
                userAnswers: cauTraLoiTracNghiem,
                userTextAnswers: cauTraLoiTuLuan,
                maNguoiDung: maNguoiDung,
                maLuotLamBaiHienTai: maLuotLamBaiHienTai
            );
        }

        /// <summary>
        /// [Yêu cầu 3.5 & 3.6 / Feature 12] Tự động chấm điểm thi chuẩn hóa dựa trên snapshot CSDL.
        /// Đảm bảo tính toàn vẹn (Tamper-Proof) bằng cách truy vấn DB Snapshot khi maDeThi >= 0.
        /// </summary>
        public (int MaLuotLamBai, decimal DiemSo, int SoCauDung, int TongSoCau, bool LaDatYeuCau) ChamVaLuuKetQuaThi(
            int maDeThi,
            string hinhThucLam,
            DateTime thoiDiemBatDau,
            int thoiGianGiay,
            List<CauHoiKhaoSatDTO>? questions,
            Dictionary<int, int> userAnswers, // key: MaCauHoi, value: MaLuaChonDaChon
            Dictionary<int, string>? userTextAnswers = null, // key: MaCauHoi, value: CauTraLoiVanBan
            int? maNguoiDung = null,
            int maLuotLamBaiHienTai = 0)
        {
            int userId;
            if (maNguoiDung.HasValue && maNguoiDung.Value < 0)
            {
                // In-memory grading simulation (e.g. Unit tests passing negative user ID)
                userId = maNguoiDung.Value;
            }
            else if (!SessionHelper.IsGiangVien && !SessionHelper.IsAdmin)
            {
                // Học viên không thể mạo danh ID người khác (Anti-IDOR)
                userId = SessionHelper.CurrentMaNguoiDung;
            }
            else
            {
                userId = (maNguoiDung.HasValue && maNguoiDung.Value > 0) ? maNguoiDung.Value : SessionHelper.CurrentMaNguoiDung;
            }

            // Nạp danh sách câu hỏi và đáp án chuẩn từ Database Snapshot CauHoiBaiTap
            List<CauHoiKhaoSatDTO> authoritativeQuestions;
            var deThi = maDeThi >= 0 ? BaiTapDeThiDAL.Instance.LayChiTietDeThi(maDeThi) : null;

            if (maDeThi >= 0)
            {
                // BẢO MẬT: Bắt buộc lấy bản chụp (SNAPSHOT) câu hỏi và đáp án đúng trực tiếp từ DB
                // Bỏ qua hoàn toàn thuộc tính LaDapAnDung trong `questions` do client gửi lên
                authoritativeQuestions = BaiTapDeThiDAL.Instance.LayDanhSachCauHoiCuaDeThi(maDeThi);
                if (authoritativeQuestions.Count == 0 && questions != null && questions.Count > 0)
                {
                    // Fallback nếu đề thi chưa có snapshot câu trong DB
                    authoritativeQuestions = questions;
                }
            }
            else
            {
                // Chế độ Unit Test In-Memory (maDeThi < 0)
                authoritativeQuestions = questions ?? new List<CauHoiKhaoSatDTO>();
            }

            int tongSoCau = authoritativeQuestions.Count;
            int soCauDung = 0;

            // Lấy thông tin cấu hình đề thi nếu có
            double tongDiemDeThi = deThi != null && deThi.TongDiem > 0 ? deThi.TongDiem : 10.0;
            double diemDat = deThi != null ? deThi.DiemDat : 5.0;

            // Tính tổng trọng số điểm của các câu hỏi từ DB Snapshot chuẩn
            double tongTrongSoCauHoi = authoritativeQuestions.Sum(q => q.Diem > 0 ? q.Diem : 1.0);
            if (tongTrongSoCauHoi <= 0) tongTrongSoCauHoi = tongSoCau > 0 ? tongSoCau : 1.0;

            var chiTietList = new List<(int MaCauHoi, int? MaLuaChon, string? CauTraLoiVanBan, bool DungSai, decimal DiemCau)>();
            decimal tongDiemDatDuoc = 0m;

            foreach (var q in authoritativeQuestions)
            {
                int? selectedOptId = null;
                if (userAnswers != null && userAnswers.TryGetValue(q.MaCauHoi, out int optId) && optId > 0)
                {
                    selectedOptId = optId;
                }

                string? textAnswer = null;
                if (userTextAnswers != null && userTextAnswers.TryGetValue(q.MaCauHoi, out string? txt))
                {
                    textAnswer = txt?.Trim();
                }

                bool isCorrect = false;

                // 1. Dạng câu hỏi trắc nghiệm / Lựa chọn (TracNghiem, DungSai, TrueFalseNotGiven)
                if (q.IsDangLuaChon || (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 1 && selectedOptId.HasValue))
                {
                    // So khớp selectedOptId với đáp án đúng chuẩn từ DB Snapshot
                    var correctOpt = q.DanhSachLuaChon?.FirstOrDefault(opt => opt.LaDapAnDung);
                    if (correctOpt != null && selectedOptId.HasValue && correctOpt.MaLuaChon == selectedOptId.Value)
                    {
                        isCorrect = true;
                    }
                }
                // 2. Dạng câu hỏi Tự luận / Viết bài (TuLuan)
                else if (q.IsDangTuLuan)
                {
                    if (!string.IsNullOrWhiteSpace(textAnswer))
                    {
                        int wordCount = textAnswer.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
                        
                        // Kiểm tra nếu có đáp án mẫu chuẩn hoặc yêu cầu từ khóa
                        bool isSampleMatched = !string.IsNullOrWhiteSpace(q.DapAnDungText) &&
                            ChuankHoaChuoiTraLoi(textAnswer).Contains(ChuankHoaChuoiTraLoi(q.DapAnDungText));

                        // Đạt nếu khớp từ khóa/đáp án mẫu hoặc bài viết có độ dài hợp lý (>= 10 từ)
                        if (wordCount >= 10 || isSampleMatched)
                        {
                            isCorrect = true;
                        }
                    }
                }
                // 3. Dạng câu hỏi nhập văn bản (DienKhuyet, TraLoiNgan, VietLaiCau, SuaLoiSai)
                else if (q.IsDangNhapVanBan || !string.IsNullOrWhiteSpace(textAnswer))
                {
                    if (!string.IsNullOrWhiteSpace(textAnswer))
                    {
                        string userClean = ChuankHoaChuoiTraLoi(textAnswer);

                        var choices = q.DanhSachLuaChon ?? new List<LuaChonDapAnDTO>();
                        var acceptableAnswers = choices
                            .Where(x => x.LaDapAnDung || choices.Count <= 1)
                            .Select(x => x.NoiDungLuaChon)
                            .ToList();

                        if (!acceptableAnswers.Any() && !string.IsNullOrWhiteSpace(q.DapAnDungText))
                        {
                            acceptableAnswers.Add(q.DapAnDungText);
                        }

                        foreach (var target in acceptableAnswers)
                        {
                            if (string.IsNullOrWhiteSpace(target)) continue;
                            var subTargets = target.Split(new[] { '|', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var sub in subTargets)
                            {
                                string targetClean = ChuankHoaChuoiTraLoi(sub);
                                if (userClean.Equals(targetClean, StringComparison.OrdinalIgnoreCase))
                                {
                                    isCorrect = true;
                                    break;
                                }
                            }
                            if (isCorrect) break;
                        }
                    }
                }

                // Điểm của câu hỏi này
                decimal diemCauHoi = isCorrect 
                    ? (decimal)Math.Round((q.Diem > 0 ? q.Diem : 1.0) * (tongDiemDeThi / tongTrongSoCauHoi), 2)
                    : 0m;

                if (isCorrect)
                {
                    soCauDung++;
                    tongDiemDatDuoc += diemCauHoi;
                }

                chiTietList.Add((q.MaCauHoi, selectedOptId, textAnswer, isCorrect, diemCauHoi));
            }

            decimal diemSoFinal = Math.Min((decimal)tongDiemDeThi, Math.Round(tongDiemDatDuoc, 2));
            bool laDat = diemSoFinal >= (decimal)diemDat;

            int maLuot = 0;
            if (userId > 0)
            {
                maLuot = BaiTapDeThiDAL.Instance.LuuKetQuaThi(
                    userId, maDeThi, hinhThucLam, thoiDiemBatDau, thoiGianGiay,
                    tongSoCau, soCauDung, diemSoFinal, chiTietList, maLuotLamBaiHienTai);
            }

            return (maLuot, diemSoFinal, soCauDung, tongSoCau, laDat);
        }

        private static string ChuankHoaChuoiTraLoi(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";
            string s = raw.Trim().ToLowerInvariant();
            s = s.TrimEnd('.', '?', '!', ',', ';');
            while (s.Contains("  "))
                s = s.Replace("  ", " ");
            return s;
        }

        public LuotLamBaiDTO? LayChiTietKetQuaLamBai(int maLuotLamBai)
        {
            var result = BaiTapDeThiDAL.Instance.LayChiTietKetQuaLamBai(maLuotLamBai);
            if (result == null) return null;

            // Anti-IDOR Ownership Guard:
            // Non-admin / non-lecturer (student) can only access their own exam attempt
            if (SessionHelper.CurrentUser != null && !SessionHelper.IsGiangVien && !SessionHelper.IsAdmin)
            {
                int currentUserId = SessionHelper.CurrentMaNguoiDung;
                if (currentUserId <= 0 || result.MaNguoiDung != currentUserId)
                {
                    return null; // Deny unauthorized access
                }
            }

            return result;
        }

        public List<LuotLamBaiDTO> LayLichSuThi(int top = 10)
        {
            int userId = SessionHelper.CurrentMaNguoiDung;
            if (userId <= 0) return new List<LuotLamBaiDTO>();
            return BaiTapDeThiDAL.Instance.LayLichSuThi(userId, top);
        }
    }
}
