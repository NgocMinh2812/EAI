using System;
using System.Collections.Generic;

namespace DTO
{
    public class BoCauHoiDTO
    {
        public int MaBoCauHoi { get; set; }
        public string TenBoCauHoi { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string NguonTao { get; set; } = "AI"; // "AI_DanhMuc", "AI_BaiHoc", "ThuCong"
        public string? TenNguon { get; set; } // Tên tài liệu / bài học nguồn
        public int? MaBaiHoc { get; set; }
        public int? MaChuDe { get; set; }
        public int? MaTrinhDo { get; set; }
        public int? NguoiTao { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? NgayCapNhat { get; set; }
        public int SoLuongCauHoi { get; set; }

        public string NguonTaoHienThi => NguonTao switch
        {
            "AI_DanhMuc" => "AI • Danh mục",
            "AI_BaiHoc" => "AI • Bài học",
            "ThuCong" => "Thủ công",
            _ => NguonTao
        };

        public string NgayTaoHienThi => NgayTao.ToString("dd/MM/yyyy HH:mm");
    }
}
