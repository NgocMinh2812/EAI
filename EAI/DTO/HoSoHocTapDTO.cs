using System;

namespace DTO
{
    public class HoSoHocTapDTO
    {
        public int MaHoSo { get; set; }
        public int MaNguoiDung { get; set; }
        public decimal? DiemMucTieu { get; set; }
        public string? KhungNangLucMucTieu { get; set; } // IELTS / TOEIC / CEFR...
        public string? TrinhDoMucTieu { get; set; }     // A1-C1...
        public int SoPhutHocTichLuy { get; set; }
        public string? GhiChuCaNhan { get; set; }
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;
    }
}
