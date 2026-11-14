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
            // Simulate a logged-in non-admin student (HocVien: MaVaiTro = 1)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 999,
                TenDangNhap = "student_test",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
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
            // Simulate a logged-in lecturer (GiangVien: MaVaiTro = 0)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 888,
                TenDangNhap = "giangvien_normal",
                TenVaiTro = "GiangVien",
                MaVaiTro = 0
            };

            var (success, message) = NguoiDungBUS.Instance.XoaTaiKhoan(2);

            Assert.False(success);
            Assert.Contains("Quản trị viên", message);

            SessionHelper.Logout();
        }

        [Fact]
        public void SessionHelper_StudentRole_MustNotHaveGiangVienOrAdminRights()
        {
            // Set student user session (MaVaiTro = 1, TenVaiTro = HocVien)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 4,
                TenDangNhap = "hocvien",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            Assert.False(SessionHelper.IsGiangVien, "Student must NOT have GiangVien privileges.");
            Assert.False(SessionHelper.IsAdmin, "Student must NOT have Admin privileges.");
            Assert.Equal(4, SessionHelper.CurrentMaNguoiDung);
            Assert.True(SessionHelper.IsLoggedIn);

            SessionHelper.Logout();
        }

        [Fact]
        public void SessionHelper_NullUser_MustReturnSafeNegativeOneId()
        {
            SessionHelper.Logout();

            Assert.Equal(-1, SessionHelper.CurrentMaNguoiDung);
            Assert.False(SessionHelper.IsLoggedIn);
            Assert.False(SessionHelper.IsGiangVien);
            Assert.False(SessionHelper.IsAdmin);
        }

        [Fact]
        public void SessionHelper_AdminRole_MustHaveAdminAndGiangVienRights()
        {
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 0,
                TenDangNhap = "admin",
                TenVaiTro = "QuanTriVien",
                MaVaiTro = 2
            };

            Assert.True(SessionHelper.IsAdmin);
            Assert.True(SessionHelper.IsGiangVien);

            SessionHelper.Logout();
        }

        [Fact]
        public void SessionHelper_LecturerRole_MustHaveGiangVienRights_NotAdmin()
        {
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 1,
                TenDangNhap = "giangvien",
                TenVaiTro = "GiangVien",
                MaVaiTro = 0
            };

            Assert.True(SessionHelper.IsGiangVien);
            Assert.False(SessionHelper.IsAdmin);

            SessionHelper.Logout();
        }

        [Fact]
        public void AntiIDOR_ExamResult_StudentACannotViewStudentBResult()
        {
            // Login as Student A (User 4: hocvien)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 4,
                TenDangNhap = "hocvien",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            // Attempt 9 belongs to User 4 (Student A) -> must succeed
            var ownResult = BaiTapDeThiBUS.Instance.LayChiTietKetQuaLamBai(9);
            Assert.NotNull(ownResult);
            Assert.Equal(4, ownResult.MaNguoiDung);

            // Attempt 6 belongs to User 5 (Student B) -> must be denied (returns null)
            var otherResult = BaiTapDeThiBUS.Instance.LayChiTietKetQuaLamBai(6);
            Assert.Null(otherResult);

            // Admin login can access both
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 0,
                TenDangNhap = "admin",
                TenVaiTro = "QuanTriVien",
                MaVaiTro = 2
            };
            var adminViewOther = BaiTapDeThiBUS.Instance.LayChiTietKetQuaLamBai(6);
            Assert.NotNull(adminViewOther);
            Assert.Equal(5, adminViewOther.MaNguoiDung);

            SessionHelper.Logout();
        }

        [Fact]
        public void AntiIDOR_PlacementSurvey_StudentACannotViewStudentBSurveyAnswers()
        {
            // Login as Student A (User 4: hocvien)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 4,
                TenDangNhap = "hocvien",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            // Survey Attempt 9 belongs to User 4 -> allowed
            var ownSurvey = KhaoSatBUS.Instance.LayChiTietKhaoSatHocVien(9);
            Assert.NotNull(ownSurvey);

            // Survey Attempt 1 belongs to User 5 -> must be denied (empty list)
            var otherSurvey = KhaoSatBUS.Instance.LayChiTietKhaoSatHocVien(1);
            Assert.NotNull(otherSurvey);
            Assert.Empty(otherSurvey);

            // Admin login can view User 5's survey details
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 0,
                TenDangNhap = "admin",
                TenVaiTro = "QuanTriVien",
                MaVaiTro = 2
            };
            var adminViewSurvey = KhaoSatBUS.Instance.LayChiTietKhaoSatHocVien(1);
            Assert.NotNull(adminViewSurvey);
            Assert.NotEmpty(adminViewSurvey);

            SessionHelper.Logout();
        }

        [Fact]
        public void AntiIDOR_LearningProgress_StudentCannotQueryOtherStudentData()
        {
            // Login as Student A (User 4: hocvien)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 4,
                TenDangNhap = "hocvien",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            // Student A specifies maNguoiDung = 5 in progress query
            // TienDoHocBUS must force query to user 4
            var progress = TienDoHocBUS.Instance.LayTongKetTienDo(maNguoiDung: 5);
            Assert.NotNull(progress);

            var examHistory = TienDoHocBUS.Instance.LayLichSuDiemThi(maNguoiDung: 5);
            Assert.NotNull(examHistory);
            // Must NOT contain exam attempt 6 (which belongs to user 5)
            Assert.DoesNotContain(examHistory, x => x.MaLuotLamBai == 6);
            // Must contain user 4's exam attempt 9
            Assert.Contains(examHistory, x => x.MaLuotLamBai == 9);

            SessionHelper.Logout();
        }

        [Fact]
        public void AntiIDOR_ProfileUpdate_StudentCannotUpdateOtherStudentProfile()
        {
            // Login as Student A (User 4: hocvien)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 4,
                TenDangNhap = "hocvien",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            // Attempt to update User 5's profile (Student B)
            var (success, msg) = NguoiDungBUS.Instance.CapNhatHoSoSinhVien(
                userId: 5,
                hoTen: "Malicious Hacker Name",
                email: "hacker@test.com",
                soDienThoai: "0912345678",
                ngaySinh: DateTime.Now.AddYears(-20)
            );

            Assert.False(success);
            Assert.Contains("không có quyền", msg);

            // Also test DoiMatKhau IDOR
            var (pwSuccess, pwMsg) = NguoiDungBUS.Instance.DoiMatKhauHocVien(
                userId: 5,
                currentPassword: "any",
                newPassword: "newpassword123",
                confirmPassword: "newpassword123"
            );
            Assert.False(pwSuccess);
            Assert.Contains("không có quyền", pwMsg);

            SessionHelper.Logout();
        }

        [Fact]
        public void AntiIDOR_AIEssayHistory_StudentCannotAccessOtherStudentEssays()
        {
            // Login as Student A (User 4)
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 4,
                TenDangNhap = "hocvien",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            // User 4 queries history of User 5
            // BaiVietAIBUS must force query to user 4
            var history = BaiVietAIBUS.Instance.LayLichSuBaiViet(5);
            Assert.NotNull(history);
            foreach (var item in history)
            {
                Assert.Equal(4, item.MaNguoiDung);
            }

            SessionHelper.Logout();
        }

        [Theory]
        [InlineData("abcd")]
        [InlineData("091234abcd")]
        [InlineData("hello world")]
        [InlineData("090-abc-def")]
        [InlineData("!@#$%^&*()")]
        public void KiemTraDinhDangSoDienThoai_LettersOrSpecialChars_MustBeRejected(string invalidPhone)
        {
            bool ok = NguoiDungBUS.KiemTraDinhDangSoDienThoai(invalidPhone, out string errorMsg);
            Assert.False(ok);
            Assert.False(string.IsNullOrWhiteSpace(errorMsg));
        }

        [Theory]
        [InlineData("0988777666")]
        [InlineData("0912345678")]
        [InlineData("0388123456")]
        [InlineData("0777123456")]
        [InlineData("+84988777666")]
        [InlineData("")]
        [InlineData(null)]
        public void KiemTraDinhDangSoDienThoai_ValidPhonesOrEmpty_MustBeAccepted(string? validPhone)
        {
            bool ok = NguoiDungBUS.KiemTraDinhDangSoDienThoai(validPhone, out string errorMsg);
            Assert.True(ok);
            Assert.Empty(errorMsg);
        }

        [Fact]
        public void TaoTaiKhoan_PhoneWithLetters_MustFail()
        {
            var user = new NguoiDungDTO
            {
                HoTen = "Nguyễn Văn Test",
                TenDangNhap = "test_phone_err",
                Email = "test_phone_err@domain.com",
                SoDienThoai = "0987abcdef",
                MaVaiTro = 2
            };

            var (success, message) = new NguoiDungBUS().TaoTaiKhoan(user);

            Assert.False(success);
            Assert.Contains("Số điện thoại", message);
        }

        [Fact]
        public void TaoTaiKhoan_InvalidUsernameWithSpaces_MustFail()
        {
            var user = new NguoiDungDTO
            {
                HoTen = "Nguyễn Văn Test",
                TenDangNhap = "user name with spaces",
                Email = "test_space@domain.com",
                SoDienThoai = "0912345678",
                MaVaiTro = 2
            };

            var (success, message) = new NguoiDungBUS().TaoTaiKhoan(user);

            Assert.False(success);
            Assert.Contains("Tên đăng nhập", message);
        }
    }
}

