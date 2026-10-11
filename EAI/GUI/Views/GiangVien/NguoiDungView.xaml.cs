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
        private List<NguoiDungDTO> _allUsers = new List<NguoiDungDTO>();
        private string _statusFilter = "All";

        public NguoiDungView()
        {
            InitializeComponent();
            LoadUsersFromDatabase();
            ApplyFilter();
        }

        private void LoadUsersFromDatabase()
        {
            try
            {
                _allUsers = _bus.LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối cơ sở dữ liệu để tải danh sách người dùng:\n{ex.Message}", 
                                "Lỗi kết nối CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
                _allUsers = new List<NguoiDungDTO>();
            }
        }

        private void ApplyFilter()
        {
            if (icUserList == null) return;

            var query = _allUsers.AsEnumerable();

            // Tìm kiếm theo tên / email / tên đăng nhập
            if (txtSearchUser != null)
            {
                string search = txtSearchUser.Text.Trim();
                if (!string.IsNullOrEmpty(search) && search != SearchPlaceholder)
                {
                    query = query.Where(u => u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                             u.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                             u.TenDangNhap.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
            }

            // Lọc theo Vai trò
            if (cboRoleFilter != null && cboRoleFilter.SelectedItem is ComboBoxItem item)
            {
                string role = item.Content?.ToString() ?? "Tất cả";
                if (role == "Học viên") query = query.Where(u => u.RoleText == "Học viên");
                else if (role == "Quản trị viên" || role == "Giảng viên") 
                    query = query.Where(u => u.RoleText == "Quản trị viên" || u.RoleText == "Giảng viên");
            }

            // Lọc theo Trạng thái hoạt động
            if (_statusFilter == "Active") query = query.Where(u => u.TrangThaiHoatDong);
            else if (_statusFilter == "Locked") query = query.Where(u => !u.TrangThaiHoatDong);

            var list = query.ToList();
            icUserList.ItemsSource = list;

            int total = _allUsers.Count;
            int active = _allUsers.Count(u => u.TrangThaiHoatDong);
            txtStatsSummary.Text = $"{total} tài khoản • {active} đang hoạt động";
            txtCounter.Text = $"Hiển thị {list.Count} / {total} tài khoản";
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
            ApplyFilter();
        }

        private void CboRoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void FilterStatus_All(object sender, RoutedEventArgs e)
        {
            _statusFilter = "All";
            ApplyFilter();
        }

        private void FilterStatus_Active(object sender, RoutedEventArgs e)
        {
            _statusFilter = "Active";
            ApplyFilter();
        }

        private void FilterStatus_Locked(object sender, RoutedEventArgs e)
        {
            _statusFilter = "Locked";
            ApplyFilter();
        }

        private void BtnTaoTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NguoiDungDialog();
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true)
            {
                var (success, msg) = _bus.TaoTaiKhoan(dlg.UserData);
                if (success)
                {
                    LoadUsersFromDatabase();
                    ApplyFilter();
                    MessageBox.Show(msg, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi tạo tài khoản", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _allUsers.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var dlg = new NguoiDungDialog(user);
                    dlg.Owner = Window.GetWindow(this);
                    if (dlg.ShowDialog() == true)
                    {
                        var (success, msg) = _bus.CapNhatTaiKhoan(dlg.UserData);
                        if (success)
                        {
                            LoadUsersFromDatabase();
                            ApplyFilter();
                            MessageBox.Show(msg, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Lỗi cập nhật", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private void BtnToggleStatus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _allUsers.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    bool newStatus = !user.TrangThaiHoatDong;
                    string actionName = newStatus ? "mở khóa" : "khóa";

                    var confirm = MessageBox.Show($"Bạn có chắc chắn muốn {actionName} tài khoản của {user.FullName} ({user.Email})?",
                                                  "Xác nhận trạng thái tài khoản", 
                                                  MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (confirm == MessageBoxResult.Yes)
                    {
                        try
                        {
                            bool ok = _bus.KhoaHoacMoTaiKhoan(id, newStatus);
                            if (ok)
                            {
                                LoadUsersFromDatabase();
                                ApplyFilter();
                                MessageBox.Show($"Đã {actionName} tài khoản {user.FullName} thành công trên cơ sở dữ liệu!", 
                                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show($"Không thể cập nhật trạng thái.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _allUsers.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa vĩnh viễn tài khoản của {user.FullName}?", 
                                                 "Xác nhận xóa tài khoản", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        var (success, msg) = _bus.XoaTaiKhoan(id);
                        if (success)
                        {
                            LoadUsersFromDatabase();
                            ApplyFilter();
                            MessageBox.Show(msg, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Lỗi khi xóa", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
