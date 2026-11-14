using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DTO;

namespace BUS
{
    public class LuyenTapBUS
    {
        private static LuyenTapBUS? _instance;
        public static LuyenTapBUS Instance => _instance ??= new LuyenTapBUS();

        public int DemSoCauKhaDung(int? maChuDe, int? maBoCauHoi, string? kyNang, string? trinhDo, string? doKho)
        {
            return LuyenTapDAL.Instance.DemSoCauKhaDung(maChuDe, maBoCauHoi, kyNang, trinhDo, doKho);
        }

        public int DemSoCauKhaDung(string? kyNang, string? trinhDo, string? doKho, int? maBoCauHoi = null, int? maChuDe = null)
        {
            return DemSoCauKhaDung(maChuDe, maBoCauHoi, kyNang, trinhDo, doKho);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiLuyenTap(
            int? maChuDe, 
            int? maBoCauHoi, 
            string? kyNang, 
            string? trinhDo, 
            string? doKho, 
            int soLuong)
        {
            return LuyenTapDAL.Instance.LayDanhSachCauHoiLuyenTap(maChuDe, maBoCauHoi, kyNang, trinhDo, doKho, soLuong);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiLuyenTap(
            string? kyNang, 
            string? trinhDo, 
            string? doKho, 
            int? maBoCauHoi = null, 
            int? maChuDe = null, 
            int soLuong = 10)
        {
            return LayDanhSachCauHoiLuyenTap(maChuDe, maBoCauHoi, kyNang, trinhDo, doKho, soLuong);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiSai(int? maNguoiDung = null, int top = 20)
        {
            int userId = maNguoiDung ?? (SessionHelper.CurrentUser?.MaNguoiDung ?? 0);
            return LuyenTapDAL.Instance.LayDanhSachCauHoiSaiCuaNguoiDung(userId, top);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiSaiCuaNguoiDung(int? maNguoiDung = null, int top = 20)
        {
            return LayDanhSachCauHoiSai(maNguoiDung, top);
        }

        /// <summary>
        /// [Yêu cầu 4.3] Kiểm tra tính đúng/sai và lời giải chi tiết cho câu hỏi luyện tập (hỗ trợ phản hồi ngay lập tức)
        /// </summary>
        public (bool IsCorrect, string GiaiThich, string KyHieuDung, string NoiDungDung) KiemTraDapAn(
            CauHoiKhaoSatDTO q, 
            int? maLuaChon, 
            string? textAnswer)
        {
            bool isCorrect = false;
            string kyHieuDung = "";
            string noiDungDung = "";

            var correctOpt = q.DanhSachLuaChon?.FirstOrDefault(x => x.LaDapAnDung);
            if (correctOpt != null)
            {
                kyHieuDung = correctOpt.KyHieuLuaChon;
                noiDungDung = correctOpt.NoiDungLuaChon;
            }
            else if (!string.IsNullOrWhiteSpace(q.DapAnDungText))
            {
                noiDungDung = q.DapAnDungText;
            }

            // 1. Trắc nghiệm / Lựa chọn
            if (q.IsDangLuaChon || (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 1))
            {
                if (maLuaChon.HasValue && correctOpt != null && maLuaChon.Value == correctOpt.MaLuaChon)
                {
                    isCorrect = true;
                }
            }
            // 2. Điền khuyết / Trả lời ngắn
            else if (!string.IsNullOrWhiteSpace(textAnswer))
            {
                string userClean = ChuankHoaChuoiTraLoi(textAnswer);
                var choices = q.DanhSachLuaChon ?? new List<LuaChonDapAnDTO>();
                var acceptableAnswers = choices
                    .Where(x => x.LaDapAnDung || choices.Count <= 1)
                    .Select(x => x.NoiDungLuaChon)
                    .ToList();

                if (!acceptableAnswers.Any() && !string.IsNullOrWhiteSpace(noiDungDung))
                {
                    acceptableAnswers.Add(noiDungDung);
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

            string giaiThich = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) 
                ? q.GiaiThichDapAn 
                : "Không có lời giải chi tiết cho câu hỏi này.";

            return (isCorrect, giaiThich, kyHieuDung, noiDungDung);
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

        /// <summary>
        /// [Yêu cầu 4.4] Lưu kết quả phiên luyện tập
        /// </summary>
        public int LuuKetQuaPhienLuyenTap(
            int thoiGianGiay, 
            int tongSoCau, 
            int soCauDung, 
            decimal tyLeChinhXac,
            List<(int MaCauHoi, int? MaLuaChon, string? CauTraLoiVanBan, bool DungSai)> chiTiet)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            if (userId <= 0) return 0;

            return LuyenTapDAL.Instance.LuuKetQuaPhienLuyenTap(
                userId, thoiGianGiay, tongSoCau, soCauDung, tyLeChinhXac, chiTiet);
        }

        public int KhoiTaoLuotLuyenTap(int maNguoiDung, string tieuDe, int tongSoCau)
        {
            return LuyenTapDAL.Instance.KhoiTaoLuotLuyenTap(maNguoiDung, tieuDe, tongSoCau);
        }

        public bool HuyLuotLuyenTap(int maLuotLamBai)
        {
            return LuyenTapDAL.Instance.HuyLuotLuyenTap(maLuotLamBai);
        }

        public int LuuKetQuaPhienLuyenTap(
            int userId,
            string tieuDe,
            DateTime batDau,
            int thoiLuongGiay,
            List<CauHoiKhaoSatDTO> danhSachCauHoi,
            Dictionary<int, int> userAnswers,
            Dictionary<int, string> userTextAnswers,
            int maLuotLamBaiHienTai = 0)
        {
            if (userId <= 0)
                userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;

            var chiTiet = new List<(int MaCauHoi, int? MaLuaChon, string? CauTraLoiVanBan, bool DungSai)>();
            int soCauDung = 0;

            foreach (var q in danhSachCauHoi)
            {
                int? userOptId = userAnswers.ContainsKey(q.MaCauHoi) ? userAnswers[q.MaCauHoi] : null;
                string? userText = userTextAnswers.ContainsKey(q.MaCauHoi) ? userTextAnswers[q.MaCauHoi] : null;

                var result = KiemTraDapAn(q, userOptId, userText);
                if (result.IsCorrect) soCauDung++;

                chiTiet.Add((q.MaCauHoi, userOptId, userText, result.IsCorrect));
            }

            int tongSoCau = danhSachCauHoi.Count;
            decimal tyLe = tongSoCau > 0 ? (decimal)soCauDung * 100m / tongSoCau : 0m;

            return LuyenTapDAL.Instance.LuuKetQuaPhienLuyenTap(
                userId, tieuDe, thoiLuongGiay, tongSoCau, soCauDung, tyLe, chiTiet, maLuotLamBaiHienTai);
        }

        /// <summary>
        /// [Yêu cầu 4.4] Lấy lịch sử các phiên luyện tập
        /// </summary>
        public List<LuotLamBaiDTO> LayLichSuLuyenTap(int top = 20)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            if (userId <= 0) return new List<LuotLamBaiDTO>();

            return LuyenTapDAL.Instance.LayLichSuLuyenTap(userId, top);
        }

        public List<LuotLamBaiDTO> LayLichSuLuyenTap(int? maNguoiDung, int top = 20)
        {
            int userId = maNguoiDung ?? (SessionHelper.CurrentUser?.MaNguoiDung ?? 0);
            if (userId <= 0) return new List<LuotLamBaiDTO>();

            return LuyenTapDAL.Instance.LayLichSuLuyenTap(userId, top);
        }
    }
}
