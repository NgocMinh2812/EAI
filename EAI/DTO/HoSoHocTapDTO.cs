using System;

namespace DTO
{
    public class HoSoHocTapDTO
    {
        public int MaHoSo { get; set; }
        public int MaNguoiDung { get; set; }
        public decimal? DiemMucTieu { get; set; } = 6.5m;
        public string? KhungNangLucMucTieu { get; set; } = "IELTS";
        public string? TrinhDoMucTieu { get; set; } = "B2";
        public decimal? DiemDanhGiaDauVao { get; set; }
        public int SoPhutHocTichLuy { get; set; } = 0;
        public string? GhiChuCaNhan { get; set; }
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public string HienThiMucTieu
        {
            get
            {
                if (!string.IsNullOrEmpty(KhungNangLucMucTieu) && DiemMucTieu.HasValue)
                {
                    return $"{KhungNangLucMucTieu} Target {DiemMucTieu:F1}";
                }
                if (!string.IsNullOrEmpty(TrinhDoMucTieu))
                {
                    return $"CEFR {TrinhDoMucTieu}";
                }
                return "Chưa đặt mục tiêu";
            }
        }
    }
}
