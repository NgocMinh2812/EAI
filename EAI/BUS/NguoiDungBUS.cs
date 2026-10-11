using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DAL;
using DTO;

namespace BUS
{
    public class NguoiDungBUS
    {
        private static NguoiDungBUS? _instance;
        public static NguoiDungBUS Instance => _instance ??= new NguoiDungBUS();

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public bool DangNhap(string usernameOrEmail, string password, out string errorMessage, out NguoiDungDTO? user)
        {
            user = null;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(usernameOrEmail))
            {
                errorMessage = "Vui lòng nhập Email hoặc Tên đăng nhập!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Vui lòng nhập mật khẩu!";
                return false;
            }

            string passwordHash = HashPassword(password);
            var foundUser = NguoiDungDAL.Instance.DangNhap(usernameOrEmail.Trim(), passwordHash, password.Trim());

            if (foundUser == null)
            {
                errorMessage = "Tên đăng nhập / Email hoặc mật khẩu không chính xác!";
                return false;
            }

            if (!foundUser.TrangThaiHoatDong)
            {
                errorMessage = "Tài khoản của bạn hiện đang bị khóa. Vui lòng liên hệ quản trị viên!";
                return false;
            }

            // Thiết lập Session
            SessionHelper.CurrentUser = foundUser;
            SessionHelper.CurrentHoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(foundUser.MaNguoiDung);

            user = foundUser;
            return true;
        }

        public bool DangKy(string hoTen, string email, string password, string confirmPassword, out string errorMessage, out NguoiDungDTO? newUser)
        {
            newUser = null;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                errorMessage = "Vui lòng nhập họ và tên!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                errorMessage = "Vui lòng nhập địa chỉ email!";
                return false;
            }

            email = email.Trim().ToLower();
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorMessage = "Định dạng email không hợp lệ!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                errorMessage = "Mật khẩu phải có độ dài tối thiểu 6 ký tự!";
                return false;
            }

            if (password != confirmPassword)
            {
                errorMessage = "Mật khẩu xác nhận không khớp!";
                return false;
            }

            if (NguoiDungDAL.Instance.KiemTraEmailTonTai(email))
            {
                errorMessage = "Email này đã được đăng ký trên hệ thống. Vui lòng chọn email khác hoặc chuyển sang Đăng nhập!";
                return false;
            }

            // Tạo tên đăng nhập từ email (phần trước @)
            string baseUsername = email.Split('@')[0];
            string username = baseUsername;
            int counter = 1;
            while (NguoiDungDAL.Instance.KiemTraTenDangNhapTonTai(username))
            {
                username = $"{baseUsername}{counter++}";
            }

            var dto = new NguoiDungDTO
            {
                HoTen = hoTen.Trim(),
                Email = email,
                TenDangNhap = username,
                MatKhauMaHoa = HashPassword(password),
                TrangThaiHoatDong = true,
                LaLanDauDangNhap = true
            };

            int newId = NguoiDungDAL.Instance.DangKy(dto);
            if (newId <= 0)
            {
                errorMessage = "Có lỗi xảy ra khi tạo tài khoản. Vui lòng thử lại!";
                return false;
            }

            dto.MaNguoiDung = newId;
            dto.TenVaiTro = "HocVien";

            // Khởi tạo hồ sơ học tập ban đầu
            HoSoHocTapDAL.Instance.LuuKetQuaDanhGiaBanDau(newId, "A1", "IELTS", 5.0m);

            // Thiết lập Session
            SessionHelper.CurrentUser = dto;
            SessionHelper.CurrentHoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(newId);

            newUser = dto;
            return true;
        }

        public bool KiemTraEmailTonTai(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return NguoiDungDAL.Instance.KiemTraEmailTonTai(email.Trim().ToLower());
        }

        public bool CapNhatThongTinCaNhan(int maNguoiDung, string hoTen, string soDienThoai)
        {
            bool ok = NguoiDungDAL.Instance.CapNhatThongTinCaNhan(maNguoiDung, hoTen, soDienThoai);
            if (ok && SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.MaNguoiDung == maNguoiDung)
            {
                SessionHelper.CurrentUser.HoTen = hoTen;
                SessionHelper.CurrentUser.SoDienThoai = soDienThoai;
            }
            return ok;
        }

        public void CapNhatLaLanDauDangNhap(int maNguoiDung, bool laLanDau)
        {
            NguoiDungDAL.Instance.CapNhatTrangThaiLanDauDangNhap(maNguoiDung, laLanDau);
            if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.MaNguoiDung == maNguoiDung)
            {
                SessionHelper.CurrentUser.LaLanDauDangNhap = laLanDau;
            }
        }

        private readonly NguoiDungDAL _dal = new NguoiDungDAL();

        public List<NguoiDungDTO> LayDanhSach(string? keyword = null, string? role = null, bool? isActive = null)
        {
            return _dal.GetAll(keyword, role, isActive);
        }

        public (List<NguoiDungDTO> Items, int TotalCount) LayDanhSachPhanTrang(
            int pageIndex, 
            int pageSize, 
            string? keyword = null, 
            string? role = null, 
            bool? isActive = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            return _dal.GetPaged(pageIndex, pageSize, keyword, role, isActive);
        }

        public NguoiDungDTO? LayChiTiet(int userId)
        {
            return _dal.GetById(userId);
        }

        public bool KhoaHoacMoTaiKhoan(int userId, bool newStatus)
        {
            var user = _dal.GetById(userId);
            if (user == null) return false;

            // Không cho phép khóa tài khoản quản trị viên tối cao
            if (user.TenVaiTro == "QuanTriVien" && !newStatus)
            {
                throw new InvalidOperationException("Không thể khóa tài khoản Quản trị viên hệ thống!");
            }

            return _dal.ToggleStatus(userId, newStatus);
        }

        public (bool Success, string Message) TaoTaiKhoan(NguoiDungDTO user)
        {
            if (string.IsNullOrWhiteSpace(user.HoTen))
                return (false, "Họ và tên không được để trống.");

            if (string.IsNullOrWhiteSpace(user.TenDangNhap) || user.TenDangNhap.Trim().Length < 3)
                return (false, "Tên đăng nhập phải có ít nhất 3 ký tự.");

            if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Địa chỉ email không đúng định dạng hợp lệ.");

            if (_dal.ExistsUsernameOrEmail(user.TenDangNhap.Trim(), user.Email.Trim()))
                return (false, "Tên đăng nhập hoặc email đã tồn tại trong hệ thống.");

            if (user.MaVaiTro <= 0)
            {
                user.MaVaiTro = 2; // Mặc định: Học viên
            }

            bool ok = _dal.Insert(user);
            if (ok)
            {
                return (true, "Tạo tài khoản thành công.");
            }
            return (false, "Lỗi lưu tài khoản vào cơ sở dữ liệu.");
        }

        public (bool Success, string Message) CapNhatTaiKhoan(NguoiDungDTO user)
        {
            if (user.MaNguoiDung <= 0)
                return (false, "Mã người dùng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(user.HoTen))
                return (false, "Họ và tên không được để trống.");

            if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Địa chỉ email không đúng định dạng hợp lệ.");

            if (_dal.ExistsUsernameOrEmail(user.TenDangNhap.Trim(), user.Email.Trim(), user.MaNguoiDung))
                return (false, "Email này đã được sử dụng bởi một tài khoản khác.");

            bool ok = _dal.Update(user);
            if (ok)
            {
                return (true, "Cập nhật tài khoản thành công.");
            }
            return (false, "Lỗi cập nhật dữ liệu.");
        }

        public (bool Success, string Message) XoaTaiKhoan(int userId)
        {
            var user = _dal.GetById(userId);
            if (user == null)
                return (false, "Tài khoản không tồn tại.");

            if (user.TenVaiTro == "QuanTriVien")
                return (false, "Không được phép xóa tài khoản Quản trị viên hệ thống.");

            bool ok = _dal.Delete(userId);
            if (ok)
            {
                return (true, "Đã xóa tài khoản thành công.");
            }
            return (false, "Không thể xóa tài khoản do có dữ liệu ràng buộc.");
        }

        public (bool Success, string Message, NguoiDungDTO? User) DangNhap(string emailOrUsername, string password)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername))
                return (false, "Vui lòng nhập email hoặc tên đăng nhập.", null);

            if (string.IsNullOrWhiteSpace(password))
                return (false, "Vui lòng nhập mật khẩu.", null);

            var user = _dal.GetByCredentials(emailOrUsername, password);
            if (user == null)
            {
                return (false, "Email/tên đăng nhập hoặc mật khẩu không chính xác.", null);
            }

            if (!user.TrangThaiHoatDong)
            {
                return (false, "Tài khoản của bạn hiện đang bị khóa. Vui lòng liên hệ quản trị viên để được hỗ trợ.", null);
            }

            SessionHelper.CurrentUser = user;
            try
            {
                SessionHelper.CurrentHoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(user.MaNguoiDung);
            }
            catch { }

            return (true, "Đăng nhập thành công.", user);
        }

        public bool KiemTraQuyenGiangVien(int userId)
        {
            var user = _dal.GetById(userId);
            return user != null && (user.TenVaiTro == "GiangVien" || user.TenVaiTro == "QuanTriVien");
        }
    }
}
