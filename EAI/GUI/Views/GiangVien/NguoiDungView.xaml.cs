using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Views.GiangVien
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string AvatarBg { get; set; } = "#DCFCE7";
        public string AvatarFg { get; set; } = "#15803D";
        public string RoleText { get; set; } = "Học viên";
        public string RoleBg { get; set; } = "#EFF6FF";
        public string RoleFg { get; set; } = "#1D4ED8";
        public string LevelText { get; set; } = "B2";
        public string LastLogin { get; set; } = "Hôm nay, 08:42";
        public string StatusText { get; set; } = "Hoạt động";
        public string StatusBg { get; set; } = "#DCFCE7";
        public string StatusFg { get; set; } = "#15803D";
        public string LockIcon { get; set; } = "🔒";
    }

    public partial class NguoiDungView : Page
    {
        private const string SearchPlaceholder = "Tìm theo tên hoặc email...";
        private List<UserViewModel> _allUsers = new List<UserViewModel>();
        private string _statusFilter = "All";

        public NguoiDungView()
        {
            InitializeComponent();
            LoadMockUsers();
            ApplyFilter();
        }

        private void LoadMockUsers()
        {
            _allUsers = new List<UserViewModel>
            {
                new UserViewModel
                {
                    Id = 1,
                    FullName = "Nguyễn Thị Lan",
                    Email = "lan.nguyen@gmail.com",
                    Initials = "TL",
                    AvatarBg = "#DCFCE7",
                    AvatarFg = "#15803D",
                    RoleText = "Học viên",
                    RoleBg = "#EFF6FF",
                    RoleFg = "#1D4ED8",
                    LevelText = "B2",
                    LastLogin = "Hôm nay, 08:42",
                    StatusText = "Hoạt động",
                    StatusBg = "#DCFCE7",
                    StatusFg = "#15803D",
                    LockIcon = "🔒"
                },
                new UserViewModel
                {
                    Id = 2,
                    FullName = "Trần Minh Khoa",
                    Email = "khoa.tran@outlook.com",
                    Initials = "MK",
                    AvatarBg = "#DBEAFE",
                    AvatarFg = "#1E40AF",
                    RoleText = "Học viên",
                    RoleBg = "#EFF6FF",
                    RoleFg = "#1D4ED8",
                    LevelText = "B1",
                    LastLogin = "Hôm nay, 10:15",
                    StatusText = "Hoạt động",
                    StatusBg = "#DCFCE7",
                    StatusFg = "#15803D",
                    LockIcon = "🔒"
                },
                new UserViewModel
                {
                    Id = 3,
                    FullName = "Phạm Đức Anh",
                    Email = "ducanh.pham@gmail.com",
                    Initials = "DA",
                    AvatarBg = "#FEF3C7",
                    AvatarFg = "#B45309",
                    RoleText = "Học viên",
                    RoleBg = "#EFF6FF",
                    RoleFg = "#1D4ED8",
                    LevelText = "A2",
                    LastLogin = "15/08/2025",
                    StatusText = "Đã khóa",
                    StatusBg = "#FEE2E2",
                    StatusFg = "#B91C1C",
                    LockIcon = "🔓"
                },
                new UserViewModel
                {
                    Id = 4,
                    FullName = "Lê Thu Hương",
                    Email = "huong.le@yahoo.com",
                    Initials = "TH",
                    AvatarBg = "#F3E8FF",
                    AvatarFg = "#6B21A8",
                    RoleText = "Học viên",
                    RoleBg = "#EFF6FF",
                    RoleFg = "#1D4ED8",
                    LevelText = "C1",
                    LastLogin = "Hôm qua, 22:30",
                    StatusText = "Hoạt động",
                    StatusBg = "#DCFCE7",
                    StatusFg = "#15803D",
                    LockIcon = "🔒"
                },
                new UserViewModel
                {
                    Id = 5,
                    FullName = "Vũ Hoàng Nam",
                    Email = "nam.vu@company.vn",
                    Initials = "HN",
                    AvatarBg = "#E0E7FF",
                    AvatarFg = "#3730A3",
                    RoleText = "Học viên",
                    RoleBg = "#EFF6FF",
                    RoleFg = "#1D4ED8",
                    LevelText = "B1",
                    LastLogin = "Hôm nay, 07:55",
                    StatusText = "Hoạt động",
                    StatusBg = "#DCFCE7",
                    StatusFg = "#15803D",
                    LockIcon = "🔒"
                },
                new UserViewModel
                {
                    Id = 6,
                    FullName = "Đặng Thị Mai",
                    Email = "mai.dang@gmail.com",
                    Initials = "TM",
                    AvatarBg = "#FCE7F3",
                    AvatarFg = "#BE185D",
                    RoleText = "Học viên",
                    RoleBg = "#EFF6FF",
                    RoleFg = "#1D4ED8",
                    LevelText = "B2",
                    LastLogin = "02/09/2025",
                    StatusText = "Đã khóa",
                    StatusBg = "#FEE2E2",
                    StatusFg = "#B91C1C",
                    LockIcon = "🔓"
                },
                new UserViewModel
                {
                    Id = 7,
                    FullName = "Hoàng Văn Tùng",
                    Email = "tung.hoang@edu.vn",
                    Initials = "VT",
                    AvatarBg = "#F3E8FF",
                    AvatarFg = "#7E22CE",
                    RoleText = "Quản trị viên",
                    RoleBg = "#F3E8FF",
                    RoleFg = "#6B21A8",
                    LevelText = "—",
                    LastLogin = "Hôm nay, 06:00",
                    StatusText = "Hoạt động",
                    StatusBg = "#DCFCE7",
                    StatusFg = "#15803D",
                    LockIcon = "🔒"
                }
            };
        }

        private void ApplyFilter()
        {
            if (icUserList == null) return;

            var query = _allUsers.AsEnumerable();

            // Search by name/email
            if (txtSearchUser != null)
            {
                string search = txtSearchUser.Text.Trim();
                if (!string.IsNullOrEmpty(search) && search != SearchPlaceholder)
                {
                    query = query.Where(u => u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                             u.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
            }

            // Role Filter
            if (cboRoleFilter != null && cboRoleFilter.SelectedItem is ComboBoxItem item)
            {
                string role = item.Content?.ToString() ?? "Tất cả";
                if (role == "Học viên") query = query.Where(u => u.RoleText == "Học viên");
                else if (role == "Quản trị viên") query = query.Where(u => u.RoleText == "Quản trị viên");
            }

            // Status Filter
            if (_statusFilter == "Active") query = query.Where(u => u.StatusText == "Hoạt động");
            else if (_statusFilter == "Locked") query = query.Where(u => u.StatusText == "Đã khóa");

            var list = query.ToList();
            icUserList.ItemsSource = list;

            int total = _allUsers.Count;
            int active = _allUsers.Count(u => u.StatusText == "Hoạt động");
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
            MessageBox.Show("Mở biểu mẫu tạo mới tài khoản học viên / giảng viên kèm phân quyền hệ thống.", 
                            "Tạo tài khoản mới", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var user = _allUsers.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    MessageBox.Show($"Chỉnh sửa thông tin hồ sơ cho: {user.FullName} ({user.Email})", 
                                    "Chỉnh sửa tài khoản", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    if (user.StatusText == "Hoạt động")
                    {
                        user.StatusText = "Đã khóa";
                        user.StatusBg = "#FEE2E2";
                        user.StatusFg = "#B91C1C";
                        user.LockIcon = "🔓";
                    }
                    else
                    {
                        user.StatusText = "Hoạt động";
                        user.StatusBg = "#DCFCE7";
                        user.StatusFg = "#15803D";
                        user.LockIcon = "🔒";
                    }
                    ApplyFilter();
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
                    var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản của {user.FullName}?", 
                                                 "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _allUsers.Remove(user);
                        ApplyFilter();
                    }
                }
            }
        }
    }
}
