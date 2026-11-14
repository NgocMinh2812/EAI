using System;
using DTO;

namespace BUS
{
    public static class SessionHelper
    {
        public static NguoiDungDTO? CurrentUser { get; set; }
        public static HoSoHocTapDTO? CurrentHoSo { get; set; }

        public static int CurrentMaNguoiDung => CurrentUser?.MaNguoiDung ?? -1;
        public static string CurrentRole => CurrentUser?.TenVaiTro ?? "Khach";

        public static bool IsLoggedIn => CurrentUser != null && CurrentUser.MaNguoiDung > 0;

        public static bool IsGiangVien => 
            CurrentUser != null && 
            CurrentUser.MaVaiTro != 1 &&
            !string.Equals(CurrentUser.TenVaiTro, "HocVien", StringComparison.OrdinalIgnoreCase) &&
            (string.Equals(CurrentUser.TenVaiTro, "GiangVien", StringComparison.OrdinalIgnoreCase) || 
             string.Equals(CurrentUser.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase) ||
             CurrentUser.MaVaiTro == 0 ||
             CurrentUser.MaVaiTro == 2);

        public static bool IsAdmin =>
            CurrentUser != null && 
            CurrentUser.MaVaiTro != 1 &&
            !string.Equals(CurrentUser.TenVaiTro, "HocVien", StringComparison.OrdinalIgnoreCase) &&
            (string.Equals(CurrentUser.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase) ||
             CurrentUser.MaVaiTro == 2 ||
             string.Equals(CurrentUser.TenDangNhap, "admin", StringComparison.OrdinalIgnoreCase));

        public static void Logout()
        {
            CurrentUser = null;
            CurrentHoSo = null;
        }
    }
}
