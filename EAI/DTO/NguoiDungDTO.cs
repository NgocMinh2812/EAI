using System;

namespace DTO
{
    public class NguoiDungDTO
    {
        public int MaNguoiDung { get; set; }
        public int MaVaiTro { get; set; }
        public string TenVaiTro { get; set; } = string.Empty; // "GiangVien" hoặc "HocVien"
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhauMaHoa { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DuongDanAnhDaiDien { get; set; }
        public bool TrangThaiHoatDong { get; set; } = true;
        public bool LaLanDauDangNhap { get; set; } = true;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? LanDangNhapCuoi { get; set; }

        public string AvatarInitials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(HoTen)) return "U";
                var parts = HoTen.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
                return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
            }
        }
    }
}
