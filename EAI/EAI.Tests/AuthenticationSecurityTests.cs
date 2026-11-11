using System;
using BUS;
using DAL;
using DTO;
using Xunit;

namespace EAI.Tests
{
    public class AuthenticationSecurityTests
    {
        [Fact]
        public void HashPassword_ValidInput_Returns64CharHexString()
        {
            string hash = NguoiDungBUS.HashPassword("123456");
            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.Equal("8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92", hash, ignoreCase: true);
        }

        [Fact]
        public void HashPassword_EmptyOrNull_ReturnsEmptyString()
        {
            Assert.Empty(NguoiDungBUS.HashPassword(""));
            Assert.Empty(NguoiDungBUS.HashPassword(null!));
        }

        [Fact]
        public void DangNhap_BackdoorPasswordAdmin_MustBeRejected()
        {
            // Verify that entering 'admin' as password does NOT bypass authentication
            // when the database hash is a real SHA-256 hash (not 'admin')
            string fakePasswordHash = NguoiDungBUS.HashPassword("DefinitelyNotTheRealPassword!@#123");
            var user = NguoiDungDAL.Instance.DangNhap("admin", fakePasswordHash, "admin");

            // Backdoor fix verification: Must be null
            Assert.Null(user);
        }

        [Fact]
        public void DangNhap_ChangeMeHash_MustBeRejected()
        {
            var user = NguoiDungDAL.Instance.DangNhap("admin", "CHANGE_ME_HASH", "123456");
            Assert.Null(user);
        }

        [Fact]
        public void ResetMatKhau_NonAdminUser_MustBeForbidden()
        {
            // Simulate a logged-in non-admin student (HocVien)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 999,
                TenDangNhap = "student_test",
                TenVaiTro = "HocVien",
                MaVaiTro = 2
            };

            // Student attempts to reset another user's password (e.g. user 1)
            var (success, message) = NguoiDungBUS.Instance.ResetMatKhau(1, "newpassword");

            Assert.False(success);
            Assert.Contains("quyền quản trị", message);

            // Clean up session
            SessionHelper.Logout();
        }

        [Fact]
        public void XoaTaiKhoan_NonAdminUser_MustBeForbidden()
        {
            // Simulate a logged-in non-admin user
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 888,
                TenDangNhap = "giangvien_normal",
                TenVaiTro = "GiangVien",
                MaVaiTro = 3
            };

            var (success, message) = NguoiDungBUS.Instance.XoaTaiKhoan(2);

            Assert.False(success);
            Assert.Contains("Quản trị viên", message);

            SessionHelper.Logout();
        }
    }
}

