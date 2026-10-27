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
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
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
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
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
        /// [Yêu cầu 3.5 & 3.6] Tự động chấm điểm thi chuẩn hóa dựa trên điểm số cấu hình của từng câu hỏi
        /// </summary>
        public (int MaLuotLamBai, decimal DiemSo, int SoCauDung, int TongSoCau, bool LaDatYeuCau) ChamVaLuuKetQuaThi(
            int maDeThi,
            string hinhThucLam,
            DateTime thoiDiemBatDau,
            int thoiGianGiay,
            List<CauHoiKhaoSatDTO> questions,
            Dictionary<int, int> userAnswers, // key: MaCauHoi, value: MaLuaChonDaChon
            Dictionary<int, string>? userTextAnswers = null, // key: MaCauHoi, value: CauTraLoiVanBan
            int? maNguoiDung = null)
        {
            int userId = maNguoiDung ?? (SessionHelper.CurrentUser?.MaNguoiDung ?? 1);
            int tongSoCau = questions.Count;
            int soCauDung = 0;

            // Lấy thông tin cấu hình đề thi nếu có
            var deThi = BaiTapDeThiDAL.Instance.LayChiTietDeThi(maDeThi);
            double tongDiemDeThi = deThi != null && deThi.TongDiem > 0 ? deThi.TongDiem : 10.0;
            double diemDat = deThi != null ? deThi.DiemDat : 5.0;

            // Tính tổng trọng số điểm của các câu hỏi
            double tongTrongSoCauHoi = questions.Sum(q => q.Diem > 0 ? q.Diem : 1.0);
            if (tongTrongSoCauHoi <= 0) tongTrongSoCauHoi = tongSoCau > 0 ? tongSoCau : 1.0;

            var chiTietList = new List<(int MaCauHoi, int? MaLuaChon, string? CauTraLoiVanBan, bool DungSai, decimal DiemCau)>();
            decimal tongDiemDatDuoc = 0m;

            foreach (var q in questions)
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
                    foreach (var opt in q.DanhSachLuaChon ?? new List<LuaChonDapAnDTO>())
                    {
                        if (opt.LaDapAnDung && selectedOptId.HasValue && opt.MaLuaChon == selectedOptId.Value)
                        {
                            isCorrect = true;
                            break;
                        }
                    }
                }
                // 2. Dạng câu hỏi nhập văn bản (DienKhuyet, TraLoiNgan, VietLaiCau, SuaLoiSai)
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
                // 3. Dạng câu hỏi Tự luận / Viết bài (TuLuan)
                else if (q.IsDangTuLuan)
                {
                    int wordCount = string.IsNullOrWhiteSpace(textAnswer) ? 0 : textAnswer.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    if (wordCount >= 20)
                    {
                        isCorrect = true;
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
                    tongSoCau, soCauDung, diemSoFinal, chiTietList);
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
            return BaiTapDeThiDAL.Instance.LayChiTietKetQuaLamBai(maLuotLamBai);
        }

        public List<LuotLamBaiDTO> LayLichSuThi(int top = 10)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            if (userId <= 0) return new List<LuotLamBaiDTO>();
            return BaiTapDeThiDAL.Instance.LayLichSuThi(userId, top);
        }
    }
}
