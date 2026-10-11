using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DAL;

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
            LoadDataFromDatabase();
            ApplyFilter();
        }

        private void LoadDataFromDatabase()
        {
            _allUsers = new List<UserViewModel>();
            try
            {
                DataTable dt = NguoiDungDAL.Instance.LayDanhSachNguoiDung();
                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["MaNguoiDung"]);
                    string fullName = row["HoTen"]?.ToString() ?? "";
                    string email = row["Email"]?.ToString() ?? "";
                    string role = row["TenVaiTro"]?.ToString() ?? "HocVien";
                    bool isActive = Convert.ToBoolean(row["TrangThaiHoatDong"]);
                    string trinhDo = row["TrinhDoMucTieu"] == DBNull.Value || string.IsNullOrEmpty(row["TrinhDoMucTieu"]?.ToString()) ? "—" : row["TrinhDoMucTieu"].ToString()!;
                    
                    string lastLoginStr = "Chưa đăng nhập";
                    if (row["LanDangNhapCuoi"] != DBNull.Value)
                    {
                        var lastLogin = Convert.ToDateTime(row["LanDangNhapCuoi"]);
                        lastLoginStr = lastLogin.Date == DateTime.Today ? $"Hôm nay, {lastLogin:HH:mm}" : lastLogin.ToString("dd/MM/yyyy HH:mm");
                    }

                    string initials = "U";
                    if (!string.IsNullOrWhiteSpace(fullName))
                    {
                        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        initials = parts.Length > 1 ? $"{parts[0][0]}{parts[^1][0]}".ToUpper() : parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
                    }

                    bool isAdmin = string.Equals(role, "QuanTriVien", StringComparison.OrdinalIgnoreCase) || 
                                   string.Equals(role, "GiangVien", StringComparison.OrdinalIgnoreCase);

                    _allUsers.Add(new UserViewModel
                    {
                        Id = id,
                        FullName = fullName,
                        Email = email,
                        Initials = initials,
                        AvatarBg = isAdmin ? "#F3E8FF" : "#DCFCE7",
                        AvatarFg = isAdmin ? "#7E22CE" : "#15803D",
                        RoleText = isAdmin ? "Quản trị viên" : "Học viên",
                        RoleBg = isAdmin ? "#F3E8FF" : "#EFF6FF",
                        RoleFg = isAdmin ? "#6B21A8" : "#1D4ED8",
                        LevelText = trinhDo,
                        LastLogin = lastLoginStr,
                        StatusText = isActive ? "Hoạt động" : "Đã khóa",
                        StatusBg = isActive ? "#DCFCE7" : "#FEE2E2",
                        StatusFg = isActive ? "#15803D" : "#B91C1C",
                        LockIcon = isActive ? "🔒" : "🔓"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tải người dùng: {ex.Message}");
            }
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
                    bool isCurrentlyActive = (user.StatusText == "Hoạt động");
                    bool newStatus = !isCurrentlyActive;

                    NguoiDungDAL.Instance.DoiTrangThaiKhoa(id, newStatus);

                    if (isCurrentlyActive)
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
