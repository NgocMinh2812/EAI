using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class BaiTapDeThiBUS
    {
        private static BaiTapDeThiBUS? _instance;
        public static BaiTapDeThiBUS Instance => _instance ??= new BaiTapDeThiBUS();

        public List<BaiTapDeThiDTO> LayDanhSachDeThi(string? loaiHinh = null, string? trinhDo = null)
        {
            return BaiTapDeThiDAL.Instance.LayDanhSachDeThi(loaiHinh, trinhDo);
        }

        public BaiTapDeThiDTO? LayChiTietDeThi(int maDeThi)
        {
            return BaiTapDeThiDAL.Instance.LayChiTietDeThi(maDeThi);
        }

        public List<CauHoiKhaoSatDTO> LayDanhSachCauHoiCuaDeThi(int maDeThi)
        {
            return BaiTapDeThiDAL.Instance.LayDanhSachCauHoiCuaDeThi(maDeThi);
        }

        public (int MaLuotLamBai, decimal DiemSo, int SoCauDung, int TongSoCau) ChamVaLuuKetQuaThi(
            int maDeThi,
            string hinhThucLam,
            DateTime thoiDiemBatDau,
            int thoiGianGiay,
            List<CauHoiKhaoSatDTO> questions,
            Dictionary<int, int> userAnswers) // key: MaCauHoi, value: MaLuaChonDaChon
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            int tongSoCau = questions.Count;
            int soCauDung = 0;

            var chiTietList = new List<(int MaCauHoi, int? MaLuaChon, bool DungSai)>();

            foreach (var q in questions)
            {
                int? selectedOptId = null;
                if (userAnswers.TryGetValue(q.MaCauHoi, out int optId) && optId > 0)
                {
                    selectedOptId = optId;
                }

                // Check correct
                bool isCorrect = false;
                foreach (var opt in q.DanhSachLuaChon)
                {
                    if (opt.LaDapAnDung && selectedOptId.HasValue && opt.MaLuaChon == selectedOptId.Value)
                    {
                        isCorrect = true;
                        break;
                    }
                }

                if (isCorrect) soCauDung++;
                chiTietList.Add((q.MaCauHoi, selectedOptId, isCorrect));
            }

            decimal diemSo = tongSoCau > 0 ? Math.Round((decimal)soCauDung * 10.0m / tongSoCau, 2) : 0;

            int maLuot = 0;
            if (userId > 0)
            {
                maLuot = BaiTapDeThiDAL.Instance.LuuKetQuaThi(
                    userId, maDeThi, hinhThucLam, thoiDiemBatDau, thoiGianGiay,
                    tongSoCau, soCauDung, diemSo, chiTietList);
            }

            return (maLuot, diemSo, soCauDung, tongSoCau);
        }

        public LuotLamBaiDTO? LayChiTietKetQuaLamBai(int maLuotLamBai)
        {
            return BaiTapDeThiDAL.Instance.LayChiTietKetQuaLamBai(maLuotLamBai);
        }

        public List<LuotLamBaiDTO> LayLichSuThi(int top = 5)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            if (userId <= 0) return new List<LuotLamBaiDTO>();
            return BaiTapDeThiDAL.Instance.LayLichSuThi(userId, top);
        }
    }
}
