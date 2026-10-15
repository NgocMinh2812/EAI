using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class NguoiDungView : Page
    {
        private const string SearchPlaceholder = "Tìm theo tên hoặc email...";
        private readonly NguoiDungBUS _bus = new NguoiDungBUS();
        private List<NguoiDungDTO> _currentPageUsers = new List<NguoiDungDTO>();
        private string _statusFilter = "All";
        private int _currentPage = 1;
        private const int PageSize = 10;
        private int _totalCount = 0;
        private int _totalPages = 1;

        public NguoiDungView()
        {
            InitializeComponent();
            ApplyFilter(resetPage: true);
        }

        private void ApplyFilter(bool resetPage = false)
        {
            if (icUserList == null) return;
            if (resetPage) _currentPage = 1;

            string? search = null;
            if (txtSearchUser != null)
            {
                string raw = txtSearchUser.Text.Trim();
                if (!string.IsNullOrEmpty(raw) && raw != SearchPlaceholder)
                {
                    search = raw;
                }
            }

            string? role = null;
            if (cboRoleFilter != null && cboRoleFilter.SelectedItem is ComboBoxItem item)
            {
                string r = item.Content?.ToString() ?? "Tất cả";
                if (r != "Tất cả vai trò" && r != "Tất cả")
                {
                    role = r;
                }
            }

            bool? isActive = _statusFilter switch
            {
                "Active" => true,
                "Locked" => false,
                _ => null
            };

            try
            {
                // Gọi hàm phân trang SQL OFFSET FETCH NEXT từ BUS và DAL
                var result = _bus.LayDanhSachPhanTrang(_currentPage, PageSize, search, role, isActive);
                _currentPageUsers = result.Items;
                _totalCount = result.TotalCount;
                _totalPages = Math.Max(1, (int)Math.Ceiling((double)_totalCount / PageSize));

                if (_currentPage > _totalPages) _currentPage = _totalPages;

                icUserList.ItemsSource = _currentPageUsers;

                int fromIndex = _totalCount == 0 ? 0 : (_currentPage - 1) * PageSize + 1;
                int toIndex = Math.Min(_totalCount, _currentPage * PageSize);

                if (txtCounter != null)
                {
                    txtCounter.Text = $"Hiển thị {fromIndex} - {toIndex} / {_totalCount} tài khoản";
                }

                if (txtPageInfo != null)
                {
                    txtPageInfo.Text = $"Trang {_currentPage} / {_totalPages}";
                }

                if (btnPrevPage != null) btnPrevPage.IsEnabled = _currentPage > 1;
                if (btnNextPage != null) btnNextPage.IsEnabled = _currentPage < _totalPages;

                if (txtStatsSummary != null)
                {
                    txtStatsSummary.Text = $"{_totalCount} tài khoản trong hệ thống";
                }
            }
            catch (Exception ex)
            {
                Views.Common.ThongBaoDialog.Loi(
                    Window.GetWindow(this),
                    $"Không thể kết nối cơ sở dữ liệu để tải danh sách người dùng:\n{ex.Message}", 
                    "Lỗi kết nối CSDL");
                _currentPageUsers = new List<NguoiDungDTO>();
                icUserList.ItemsSource = _currentPageUsers;
            }
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                ApplyFilter();
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                ApplyFilter();
            }
        }

        private void TxtSearchUser_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchUser.Text == SearchPlaceholder)
            {
                txtSearchUser.Text = "";
                txtSearchUser.Foreground = (System.Windows.Media.Brush)FindResource("TextDark");
            }
        }

        private void TxtSearchUser_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchUser.Text))
            {
                txtSearchUser.Text = SearchPlaceholder;
                txtSearchUser.Foreground = (System.Windows.Media.Brush)FindResource("TextGray");
            }
        }

        private void TxtSearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter(resetPage: true);
        }

        private void CboRoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter(resetPage: true);
        }

        private void FilterStatus_All(object sender, RoutedEventArgs e)
        {
            _statusFilter = "All";
            ApplyFilter(resetPage: true);
        }

        private void FilterStatus_Active(object sender, RoutedEventArgs e)
        {
            _statusFilter = "Active";
            ApplyFilter(resetPage: true);
        }

        private void FilterStatus_Locked(object sender, RoutedEventArgs e)
        {
            _statusFilter = "Locked";
            ApplyFilter(resetPage: true);
        }

        private void BtnResetMatKhau_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _currentPageUsers.FirstOrDefault(u => u.Id == id) ?? _bus.LayChiTiet(id);
                if (user != null)
                {
                    bool confirm = Views.Common.ThongBaoDialog.XacNhan(
                        Window.GetWindow(this),
                        $"Bạn có chắc chắn muốn đặt lại mật khẩu cho tài khoản của '{user.FullName}' ({user.Email})?\n\nMật khẩu sẽ được thiết lập về mặc định: 123456",
                        "Xác nhận đặt lại mật khẩu",
                        confirmText: "Đặt lại mật khẩu",
                        cancelText: "Hủy bỏ");

                    if (confirm)
                    {
                        var (success, msg) = _bus.ResetMatKhau(id, "123456");
                        if (success)
                        {
                            Views.Common.ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Reset Mật khẩu thành công");
                        }
                        else
                        {
                            Views.Common.ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi reset mật khẩu");
                        }
                    }
                }
            }
        }

        private void BtnTaoTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NguoiDungDialog();
            if (Views.Common.ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                var (success, msg) = _bus.TaoTaiKhoan(dlg.UserData);
                if (success)
                {
                    ApplyFilter(resetPage: true);
                    string thongBao = $"Tạo tài khoản thành công cho '{dlg.UserData.HoTen}'!\n\n🔑 Mật khẩu khởi tạo mặc định: 123456\n(Người dùng có thể đổi mật khẩu bất kỳ lúc nào sau khi đăng nhập).";
                    Views.Common.ThongBaoDialog.ThanhCong(Window.GetWindow(this), thongBao, "Tạo tài khoản thành công");
                }
                else
                {
                    Views.Common.ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi tạo tài khoản");
                }
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _currentPageUsers.FirstOrDefault(u => u.Id == id) ?? _bus.LayChiTiet(id);
                if (user != null)
                {
                    var dlg = new NguoiDungDialog(user);
                    if (Views.Common.ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
                    {
                        var (success, msg) = _bus.CapNhatTaiKhoan(dlg.UserData);
                        if (success)
                        {
                            ApplyFilter();
                            Views.Common.ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Cập nhật thành công");
                        }
                        else
                        {
                            Views.Common.ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi cập nhật");
                        }
                    }
                }
            }
        }

        private void BtnToggleStatus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _currentPageUsers.FirstOrDefault(u => u.Id == id) ?? _bus.LayChiTiet(id);
                if (user != null)
                {
                    // Kiểm tra không cho phép tự khóa chính mình
                    if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.MaNguoiDung == id)
                    {
                        Views.Common.ThongBaoDialog.CanhBao(Window.GetWindow(this), "Bạn không thể tự khóa tài khoản của chính mình!", "Cảnh báo bảo mật");
                        return;
                    }

                    bool newStatus = !user.TrangThaiHoatDong;
                    string actionName = newStatus ? "mở khóa" : "khóa";

                    bool confirm = Views.Common.ThongBaoDialog.XacNhan(
                        Window.GetWindow(this), 
                        $"Bạn có chắc chắn muốn {actionName} tài khoản của {user.FullName} ({user.Email})?",
                        "Xác nhận trạng thái tài khoản",
                        confirmText: $"Đồng ý {actionName}",
                        cancelText: "Hủy bỏ");

                    if (confirm)
                    {
                        try
                        {
                            bool ok = _bus.KhoaHoacMoTaiKhoan(id, newStatus);
                            if (ok)
                            {
                                ApplyFilter();
                                Views.Common.ThongBaoDialog.ThanhCong(
                                    Window.GetWindow(this),
                                    $"Đã {actionName} tài khoản {user.FullName} thành công trên cơ sở dữ liệu!", 
                                    "Cập nhật trạng thái");
                            }
                            else
                            {
                                Views.Common.ThongBaoDialog.Loi(Window.GetWindow(this), "Không thể cập nhật trạng thái tài khoản.", "Lỗi");
                            }
                        }
                        catch (Exception ex)
                        {
                            Views.Common.ThongBaoDialog.CanhBao(Window.GetWindow(this), ex.Message, "Cảnh báo");
                        }
                    }
                }
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _currentPageUsers.FirstOrDefault(u => u.Id == id) ?? _bus.LayChiTiet(id);
                if (user != null)
                {
                    bool result = Views.Common.ThongBaoDialog.XacNhanXoa(
                        Window.GetWindow(this),
                        $"Bạn có chắc chắn muốn xóa vĩnh viễn tài khoản của {user.FullName}?\nThao tác này sẽ xóa hồ sơ và không thể hoàn tác.", 
                        "Xác nhận xóa tài khoản");

                    if (result)
                    {
                        try
                        {
                            var (success, msg) = _bus.XoaTaiKhoan(id);
                            if (success)
                            {
                                ApplyFilter();
                                Views.Common.ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Đã xóa tài khoản");
                            }
                            else
                            {
                                Views.Common.ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Không thể xóa");
                            }
                        }
                        catch (Exception ex)
                        {
                            Views.Common.ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi phát sinh khi xóa tài khoản:\n{ex.Message}", "Lỗi hệ thống");
                        }
                    }
                }
            }
        }
    }
}
