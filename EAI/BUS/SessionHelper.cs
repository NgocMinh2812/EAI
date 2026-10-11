using System;
using DTO;

namespace BUS
{
    public static class SessionHelper
    {
        public static NguoiDungDTO? CurrentUser { get; set; }
        public static HoSoHocTapDTO? CurrentHoSo { get; set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static bool IsGiangVien => 
            CurrentUser != null && 
            (string.Equals(CurrentUser.TenVaiTro, "GiangVien", StringComparison.OrdinalIgnoreCase) || 
             string.Equals(CurrentUser.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase));

        public static void Logout()
        {
            CurrentUser = null;
            CurrentHoSo = null;
        }
    }
}
