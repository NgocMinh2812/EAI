using System;

namespace DTO
{
    public class NguoiDungDTO
    {
        public int MaNguoiDung { get; set; }
        public int MaVaiTro { get; set; }
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

        // Aliases for XAML UI DataBinding compatibility
        public int Id => MaNguoiDung;
        public string FullName => HoTen;
        public string LastLogin => LastLoginDisplay;

        // Bổ sung thông tin từ các bảng liên kết (VaiTro, HoSoHocTap)
        public string TenVaiTro { get; set; } = "HocVien";
        public string TrinhDoMucTieu { get; set; } = "—";
        public decimal? DiemMucTieu { get; set; }
        public string? KhungNangLucMucTieu { get; set; }

        // Tiện ích hiển thị giao diện (WPF Binding Helpers)
        public string Initials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(HoTen)) return "ND";
                var parts = HoTen.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
                return $"{parts[^2][0]}{parts[^1][0]}".ToUpper();
            }
        }

        public string AvatarBg => (MaNguoiDung % 5) switch
        {
            0 => "#DCFCE7",
            1 => "#DBEAFE",
            2 => "#FEF3C7",
            3 => "#F3E8FF",
            _ => "#E0E7FF"
        };

        public string AvatarFg => (MaNguoiDung % 5) switch
        {
            0 => "#15803D",
            1 => "#1E40AF",
            2 => "#B45309",
            3 => "#6B21A8",
            _ => "#3730A3"
        };

        public string RoleText => TenVaiTro switch
        {
            "GiangVien" => "Giảng viên",
            "QuanTriVien" => "Quản trị viên",
            _ => "Học viên"
        };

        public string RoleBg => TenVaiTro switch
        {
            "GiangVien" => "#FEF3C7",
            "QuanTriVien" => "#F3E8FF",
            _ => "#EFF6FF"
        };

        public string RoleFg => TenVaiTro switch
        {
            "GiangVien" => "#B45309",
            "QuanTriVien" => "#6B21A8",
            _ => "#1D4ED8"
        };

        public string LevelText => string.IsNullOrWhiteSpace(TrinhDoMucTieu) ? "—" : TrinhDoMucTieu;

        public string StatusText => TrangThaiHoatDong ? "Hoạt động" : "Đã khóa";
        public string StatusBg => TrangThaiHoatDong ? "#DCFCE7" : "#FEE2E2";
        public string StatusFg => TrangThaiHoatDong ? "#15803D" : "#B91C1C";
        public string LockIcon => TrangThaiHoatDong ? "🔒" : "🔓";
        public string LockTooltip => TrangThaiHoatDong ? "Khóa tài khoản" : "Mở khóa tài khoản";

        public string LastLoginDisplay
        {
            get
            {
                if (!LanDangNhapCuoi.HasValue) return "Chưa đăng nhập";
                var diff = DateTime.Now - LanDangNhapCuoi.Value;
                if (diff.TotalMinutes < 60) return $"Vừa xong ({diff.Minutes}p trước)";
                if (diff.TotalHours < 24) return $"Hôm nay, {LanDangNhapCuoi.Value:HH:mm}";
                if (diff.TotalDays < 2) return $"Hôm qua, {LanDangNhapCuoi.Value:HH:mm}";
                return LanDangNhapCuoi.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
}
