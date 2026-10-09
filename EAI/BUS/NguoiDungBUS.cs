using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DAL;
using DTO;

namespace BUS
{
    public class NguoiDungBUS
    {
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

            return (true, "Đăng nhập thành công.", user);
        }

        public bool KiemTraQuyenGiangVien(int userId)
        {
            var user = _dal.GetById(userId);
            return user != null && (user.TenVaiTro == "GiangVien" || user.TenVaiTro == "QuanTriVien");
        }
    }
}
