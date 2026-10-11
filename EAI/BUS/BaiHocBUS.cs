using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DTO;

namespace BUS
{
    public class BaiHocBUS
    {
        private static List<BaiHocDTO>? _cacheBaiHoc;
        private static readonly DAL.BaiHocDAL _baiHocDAL = new DAL.BaiHocDAL();

        public static List<BaiHocDTO> LayDanhSachTuDatabase(int? maChuDe = null, string? search = null)
        {
            try
            {
                var list = _baiHocDAL.GetAll(maChuDe, search);
                if (list != null && list.Count > 0)
                {
                    return list;
                }
            }
            catch
            {
                // Fallback nếu có sự cố
            }
            return LayDanhSachBaiHoc();
        }

        public static List<BaiHocDTO> LayDanhSachBaiHoc()
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            _cacheBaiHoc = BaiHocDAL.Instance.LayDanhSachBaiHoc(userId);
            return _cacheBaiHoc;
        }

        public static BaiHocDTO? LayChiTietBaiHoc(int maBaiHoc)
        {
            var list = LayDanhSachBaiHoc();
            var item = list.FirstOrDefault(b => b.MaBaiHoc == maBaiHoc);
            if (item != null) return item;

            var allFallbacks = new[] { "A1", "A2", "B1", "B2", "C1" }
                .SelectMany(BaiHocDAL.TaoDanhSachBaiHocDuPhongTheoTrinhDo);
            return allFallbacks.FirstOrDefault(b => b.MaBaiHoc == maBaiHoc);
        }

        public static void CapNhatTienDo(int maBaiHoc, double phanTram, string trangThai, string diemSo)
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            string dbTrangThai = phanTram >= 100 ? "HoanThanh" : (phanTram > 0 ? "DangHoc" : "ChuaHoc");

            if (userId > 0)
            {
                BaiHocDAL.Instance.CapNhatTienDo(userId, maBaiHoc, phanTram, dbTrangThai);
            }

            var baiHoc = LayChiTietBaiHoc(maBaiHoc);
            if (baiHoc != null)
            {
                baiHoc.PhanTramTienDo = Math.Min(100, Math.Max(0, phanTram));
                baiHoc.TrangThai = trangThai;
                baiHoc.DiemSo = diemSo;
                baiHoc.NgayHocGanNhat = "Hôm nay";

                if (baiHoc.PhanTramTienDo >= 100)
                {
                    baiHoc.TrangThai = "Hoàn thành";
                    baiHoc.StatusColor = "#0F766E";
                    baiHoc.StatusBackground = "#E6F7F3";
                    baiHoc.ButtonActionText = "Ôn tập lại";
                }
                else if (baiHoc.PhanTramTienDo > 0)
                {
                    baiHoc.TrangThai = "Đang học";
                    baiHoc.StatusColor = "#2563EB";
                    baiHoc.StatusBackground = "#EFF6FF";
                    baiHoc.ButtonActionText = "Học tiếp";
                }
            }
        }
    }
}
