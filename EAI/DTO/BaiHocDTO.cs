using System;
using System.Collections.Generic;

namespace DTO
{
    public class CauHoiDTO
    {
        public int MaCauHoi { get; set; }
        public string NoiDungCauHoi { get; set; } = string.Empty;
        public List<string> DanhSachLuaChon { get; set; } = new List<string>();
        public int DapAnDungIndex { get; set; } // 0: A, 1: B, 2: C, 3: D
        public string GiaiThichChiTiet { get; set; } = string.Empty;
        public int LuaChonCuaNguoiDung { get; set; } = -1; // -1: Chưa chọn
        public bool? DaTraLoiDung { get; set; } = null;
    }

    public class BaiHocDTO
    {
        public int MaBaiHoc { get; set; }
        public int MaChuDe { get; set; }
        public int MaTrinhDoId { get; set; } = 3;
        public int? NguoiTao { get; set; }
        public string TieuDe { get; set; } = string.Empty;
        public string LoaiBaiHoc { get; set; } = string.Empty; // TuVung, NguPhap, DocHieu, NgheHieu, VietLuan
        public string TenLoaiHienThi { get; set; } = string.Empty;
        public string MaTrinhDo { get; set; } = string.Empty; // A1, A2, B1, B2, C1
        public string TenChuDe { get; set; } = string.Empty;
        public string TomTat { get; set; } = string.Empty;
        public string NoiDung { get; set; } = string.Empty;
        public int ThoiGianUocTinhPhut { get; set; } = 15;
        public bool DaXuatBan { get; set; } = true;
        public double PhanTramTienDo { get; set; } = 0;
        public string TrangThai { get; set; } = "Chưa học"; // Chưa học, Đang học, Hoàn thành
        public string DiemSo { get; set; } = "--";
        public string NgayHocGanNhat { get; set; } = "Chưa học";

        // Helpers for UI display
        public string BadgeBackground { get; set; } = "#E6F7F3";
        public string BadgeForeground { get; set; } = "#0F766E";
        public string IconSkill { get; set; } = "📖";
        public string StatusColor { get; set; } = "#64748B";
        public string StatusBackground { get; set; } = "#F1F5F9";
        public string ButtonActionText { get; set; } = "Vào học ngay ▶";

        public List<CauHoiDTO> DanhSachCauHoi { get; set; } = new List<CauHoiDTO>();
    }
}
