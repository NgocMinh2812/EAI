using System;
using System.Collections.Generic;

namespace DTO
{
    public class LuotLamBaiDTO
    {
        public int MaLuotLamBai { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaBaiTapDeThi { get; set; }
        public string TenBaiTapDeThi { get; set; } = string.Empty;
        public string HinhThucLam { get; set; } = "ThiThu";
        public DateTime ThoiDiemBatDau { get; set; } = DateTime.Now;
        public DateTime? ThoiDiemNopBai { get; set; }
        public int ThoiGianLamGiay { get; set; }
        public int TongSoCau { get; set; }
        public int SoCauDung { get; set; }
        public int SoCauSai { get; set; }
        public decimal DiemSo { get; set; }
        public string TrangThai { get; set; } = "DangLam"; // DangLam / DaNop / DaHuy

        public List<ChiTietTraLoiDTO> DanhSachChiTiet { get; set; } = new List<ChiTietTraLoiDTO>();

        // Display Helpers
        public string DiemHienThi => $"{DiemSo:0.0} / 10.0";
        public string ThoiGianHienThi => $"{ThoiGianLamGiay / 60:D2}:{ThoiGianLamGiay % 60:D2}";
        public string TyLeChinhXac => TongSoCau > 0 ? $"{(SoCauDung * 100.0 / TongSoCau):0}%" : "0%";
        public string DanhGiaTongQuan => DiemSo >= 8.0m ? "Xuất sắc! Đạt chuẩn band mong muốn."
                                       : DiemSo >= 6.5m ? "Tốt! Đạt ngưỡng điểm mục tiêu."
                                       : DiemSo >= 5.0m ? "Khá! Cần củng cố thêm từ vựng và ngữ pháp."
                                       : "Cần nỗ lực hơn, hãy xem lại các câu giải thích chi tiết.";
    }
}
