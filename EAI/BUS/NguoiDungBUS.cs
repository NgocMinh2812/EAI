using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Kiểm tra định dạng họ tên:
        /// - Viết hoa chữ cái đầu mỗi từ, các chữ cái tiếp theo viết thường (Title Case)
        /// - Tối thiểu 2 từ (họ và tên)
        /// - Không chứa chữ số hoặc ký tự đặc biệt (chỉ cho phép ký tự chữ cái và khoảng trắng)
        /// </summary>
        public static bool KiemTraDinhDangHoTen(string? hoTen, out string errorMessage, out string normalizedHoTen)
        {
            errorMessage = string.Empty;
            normalizedHoTen = string.Empty;

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                errorMessage = "Vui lòng nhập họ và tên!";
                return false;
            }

            // Chuẩn hóa Unicode Form C và loại bỏ khoảng trắng thừa
            string cleaned = hoTen.Normalize(NormalizationForm.FormC).Trim();
            cleaned = Regex.Replace(cleaned, @"\s+", " ");

            if (cleaned.Length < 2)
            {
                errorMessage = "Họ và tên phải có tối thiểu 2 ký tự!";
                return false;
            }

            // Kiểm tra: Chỉ cho phép chữ cái tiếng Việt/quốc tế, khoảng trắng và dấu gạch nối
            if (cleaned.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c) && c != '-' && c != '\''))
            {
                errorMessage = "Họ và tên không được chứa chữ số hoặc ký tự đặc biệt!";
                return false;
            }

            // Tự động chuẩn hóa viết hoa chữ cái đầu mỗi từ thay vì bắt lỗi người dùng
            string[] words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                string w = words[i];
                if (w.Length > 0)
                {
                    words[i] = char.ToUpper(w[0]) + (w.Length > 1 ? w.Substring(1).ToLower() : "");
                }
            }

            normalizedHoTen = string.Join(" ", words);
            return true;
        }

        public bool DangKy(string hoTen, string email, string password, string confirmPassword, out string errorMessage, out NguoiDungDTO? newUser)
        {
            newUser = null;
            errorMessage = string.Empty;

            if (!KiemTraDinhDangHoTen(hoTen, out errorMessage, out string normalizedHoTen))
            {
                return false;
            }
            hoTen = normalizedHoTen;

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
            HoSoHocTapDAL.Instance.LuuKetQuaDanhGiaBanDau(newId, "A1", "IELTS", 5.0m, trinhDoDauVao: "A1");

            // Tải lại thông tin đầy đủ từ CSDL và thiết lập Session đăng nhập
            var fullUser = NguoiDungDAL.Instance.LayTheoId(newId) ?? dto;
            fullUser.MaNguoiDung = newId;
            fullUser.TenVaiTro = "HocVien";
            fullUser.TrinhDoDauVao = "A1";

            SessionHelper.CurrentUser = fullUser;
            SessionHelper.CurrentHoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(newId);

            newUser = fullUser;
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
            if (!KiemTraDinhDangHoTen(user.HoTen, out string nameErr, out string validName))
                return (false, nameErr);
            user.HoTen = validName;

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
                return (true, "Tạo tài khoản thành công! Mật khẩu khởi tạo mặc định là: 123456 (Người dùng có thể đổi mật khẩu sau khi đăng nhập).");
            }
            return (false, "Lỗi lưu tài khoản vào cơ sở dữ liệu.");
        }

        public (bool Success, string Message) CapNhatTaiKhoan(NguoiDungDTO user)
        {
            if (user.MaNguoiDung <= 0)
                return (false, "Mã người dùng không hợp lệ.");

            if (!KiemTraDinhDangHoTen(user.HoTen, out string nameErr, out string validName))
                return (false, nameErr);
            user.HoTen = validName;

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
                return (false, "Tài khoản không tồn tại trong hệ thống.");

            if (user.TenVaiTro == "QuanTriVien" || user.MaNguoiDung == 1 || string.Equals(user.TenDangNhap, "admin", StringComparison.OrdinalIgnoreCase))
                return (false, "Không được phép xóa tài khoản Quản trị viên hệ thống.");

            if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.MaNguoiDung == userId)
                return (false, "Bạn không thể tự xóa tài khoản đang đăng nhập hiện tại.");

            try
            {
                bool ok = _dal.Delete(userId);
                if (ok)
                {
                    return (true, $"Đã xóa vĩnh viễn tài khoản {user.HoTen} và toàn bộ dữ liệu liên quan thành công.");
                }
                return (false, "Không thể xóa tài khoản. Vui lòng kiểm tra lại.");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi xóa tài khoản: {ex.Message}");
            }
        }

        public (bool Success, string Message, NguoiDungDTO? User) DangNhap(string emailOrUsername, string password)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername))
                return (false, "Vui lòng nhập email hoặc tên đăng nhập.", null);

            if (string.IsNullOrWhiteSpace(password))
                return (false, "Vui lòng nhập mật khẩu.", null);

            bool ok = DangNhap(emailOrUsername, password, out string errorMessage, out NguoiDungDTO? user);
            if (!ok || user == null)
            {
                return (false, string.IsNullOrEmpty(errorMessage) ? "Email/tên đăng nhập hoặc mật khẩu không chính xác." : errorMessage, null);
            }

            return (true, "Đăng nhập thành công.", user);
        }

        public bool KiemTraQuyenGiangVien(int userId)
        {
            var user = _dal.GetById(userId);
            return user != null && (user.TenVaiTro == "GiangVien" || user.TenVaiTro == "QuanTriVien");
        }

        public (bool Success, string Message) ResetMatKhau(int userId, string newPassword = "123456")
        {
            if (userId <= 0) return (false, "Mã người dùng không hợp lệ.");

            var user = _dal.GetById(userId);
            if (user == null) return (false, "Tài khoản không tồn tại.");

            string hashed = HashPassword(newPassword);
            bool ok = _dal.ResetMatKhau(userId, hashed);
            if (ok)
            {
                return (true, $"Đã đặt lại mật khẩu cho tài khoản '{user.HoTen}' thành công! Mật khẩu mới: {newPassword}");
            }
            return (false, "Lỗi khi cập nhật mật khẩu mới vào cơ sở dữ liệu.");
        }

        public (bool Success, string Message) CapNhatHoSoGiangVien(
            int userId, 
            string hoTen, 
            string? soDienThoai, 
            DateTime? ngaySinh, 
            string? newPassword, 
            string? confirmPassword)
        {
            if (userId <= 0) return (false, "Mã người dùng không hợp lệ.");

            if (!KiemTraDinhDangHoTen(hoTen, out string nameErr, out string validName))
                return (false, nameErr);
            hoTen = validName;

            if (!string.IsNullOrWhiteSpace(soDienThoai))
            {
                string cleanPhone = soDienThoai.Trim();
                if (!Regex.IsMatch(cleanPhone, @"^(0|\+84)[0-9]{9,10}$"))
                    return (false, "Số điện thoại không hợp lệ (Ví dụ: 0912345678).");
            }

            string? hashedNewPass = null;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword.Length < 6)
                    return (false, "Mật khẩu mới phải có tối thiểu 6 ký tự.");

                if (newPassword != confirmPassword)
                    return (false, "Mật khẩu xác nhận không khớp với mật khẩu mới.");

                hashedNewPass = HashPassword(newPassword);
            }

            bool ok = _dal.CapNhatHoSoGiangVien(userId, hoTen, soDienThoai?.Trim(), ngaySinh, hashedNewPass);
            if (ok)
            {
                // Cập nhật lại Session người dùng hiện hành
                if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.MaNguoiDung == userId)
                {
                    SessionHelper.CurrentUser.HoTen = hoTen;
                    SessionHelper.CurrentUser.SoDienThoai = soDienThoai?.Trim();
                    SessionHelper.CurrentUser.NgaySinh = ngaySinh;
                    if (!string.IsNullOrEmpty(hashedNewPass))
                    {
                        SessionHelper.CurrentUser.MatKhauMaHoa = hashedNewPass;
                    }
                }
                return (true, "Cập nhật hồ sơ giảng viên thành công!");
            }
            return (false, "Lỗi khi cập nhật hồ sơ vào cơ sở dữ liệu.");
        }

        public (int SoBaiHoc, int SoCauHoi, int SoDeThi) LayThongKeGiangVien(int userId)
        {
            return _dal.LayThongKeGiangVien(userId);
        }
    }
}
