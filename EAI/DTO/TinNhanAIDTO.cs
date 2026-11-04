using System;

namespace DTO
{
    public class TinNhanAIDTO
    {
        public long MaTinNhan { get; set; }
        public long MaPhienHoiThoai { get; set; }
        public string LoaiNguoiGui { get; set; } = "NguoiDung"; // NguoiDung / AI
        public string NoiDungTinNhan { get; set; } = string.Empty;
        public int? SoTokenDauVao { get; set; }
        public int? SoTokenDauRa { get; set; }
        public int TongSoToken => (SoTokenDauVao ?? 0) + (SoTokenDauRa ?? 0);
        public DateTime NgayGui { get; set; } = DateTime.Now;

        public bool IsUser => string.Equals(LoaiNguoiGui, "NguoiDung", StringComparison.OrdinalIgnoreCase);
    }
}

