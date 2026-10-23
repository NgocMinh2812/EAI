using System;
using System.Collections.Generic;

namespace DTO
{
    /// <summary>
    /// Biểu diễn bộ câu hỏi củng cố (Bộ A) hoặc bộ câu hỏi vận dụng (Bộ B) gắn với bài học
    /// </summary>
    public class BaiHocQuizDTO
    {
        public int MaBoCauHoi { get; set; }
        public int MaBaiHoc { get; set; }
        public string LoaiBoCauHoi { get; set; } = "CungCo"; // "CungCo" (Bộ A) hoặc "VanDung" (Bộ B)
        public string ContentHash { get; set; } = string.Empty;
        public string TenBoCauHoi { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string TrangThai { get; set; } = "HoanThanh"; // "HoanThanh", "DangXuLy", "ThatBai"
        public string? ThongBaoLoi { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? NgayCapNhat { get; set; }
        public List<CauHoiDTO> DanhSachCauHoi { get; set; } = new List<CauHoiDTO>();

        public bool IsBoCungCo => string.Equals(LoaiBoCauHoi, "CungCo", StringComparison.OrdinalIgnoreCase);
        public bool IsBoVanDung => string.Equals(LoaiBoCauHoi, "VanDung", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Biểu diễn kết quả và lịch sử làm bài tập riêng biệt của từng sinh viên đối với một bộ câu hỏi
    /// </summary>
    public class KetQuaLamBaiBoCauHoiDTO
    {
        public int MaKetQua { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaBoCauHoi { get; set; }
        public int MaBaiHoc { get; set; }
        public string LoaiBoCauHoi { get; set; } = "CungCo";
        public string ContentHash { get; set; } = string.Empty;
        public int TongSoCau { get; set; } = 10;
        public int SoCauDung { get; set; } = 0;
        public decimal DiemSo { get; set; } = 0.0m;
        public int ThoiGianLamGiay { get; set; } = 0;
        public string TrangThai { get; set; } = "DaNop";
        public string? ChiTietTraLoiJson { get; set; }
        public DateTime NgayNop { get; set; } = DateTime.Now;

        // Display Helpers
        public string DiemHienThi => $"{DiemSo:0.0} / 10.0";
        public string TyLeChinhXac => TongSoCau > 0 ? $"{SoCauDung * 100 / TongSoCau}%" : "0%";
        public string ThoiGianHienThi => $"{ThoiGianLamGiay / 60:D2}:{ThoiGianLamGiay % 60:D2}";
        public bool LaDatYeuCau => DiemSo >= 5.0m;
        public string NhanKetQua => LaDatYeuCau ? "ĐẠT YÊU CẦU" : "CẦN ÔN TẬP THÊM";
    }

    /// <summary>
    /// Chi tiết lựa chọn của người học cho từng câu hỏi khi lưu và phục hồi
    /// </summary>
    public class ChiTietLuaChonQuizDTO
    {
        public int MaCauHoi { get; set; }
        public int ThuTu { get; set; }
        public int LuaChonCuaNguoiDung { get; set; } = -1; // Index 0..3 (A, B, C, D)
        public int DapAnDungIndex { get; set; }
        public bool DaTraLoiDung { get; set; }
    }
}
