using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using BUS;
using DTO;
using GUI.Views;

namespace GUI
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }

        public MainWindow(bool isFirstLogin = false)
        {
            InitializeComponent();
            Instance = this;
            this.WindowState = WindowState.Maximized;
            this.StateChanged += (s, e) => UpdateMaximizeRestoreState();
            UpdateMaximizeRestoreState();

            CapNhatThongTinHocVien();

            MainFrame.Navigated += MainFrame_Navigated;

            if (isFirstLogin)
            {
                // Cơ chế kiểm soát Lần đầu đăng nhập: tự động ẩn Sidebar và điều hướng vào làm bài test đầu vào
                HideSidebar();
                MainFrame.Navigate(new DanhGiaNangLucView(isFirstTime: true));
            }
            else
            {
                // Thành viên đã đăng ký: Vào thẳng Trang chủ, hiển thị Sidebar bình thường
                ShowSidebar();
                MainFrame.Navigate(new TrangChuView());
            }
        }

        public void HideSidebar()
        {
            SidebarBorder.Visibility = Visibility.Collapsed;
            colSidebar.Width = new GridLength(0);
        }

        public void ShowSidebar()
        {
            SidebarBorder.Visibility = Visibility.Visible;
            colSidebar.Width = new GridLength(240);
        }

        public void CapNhatThongTinHocVien()
        {
            if (SessionHelper.CurrentUser != null)
            {
                txtUserName.Text = SessionHelper.CurrentUser.HoTen;
                txtUserAvatar.Text = SessionHelper.CurrentUser.AvatarInitials;
            }

            if (SessionHelper.CurrentHoSo != null && !string.IsNullOrEmpty(SessionHelper.CurrentHoSo.TrinhDoMucTieu) && SessionHelper.CurrentHoSo.TrinhDoMucTieu != "—")
            {
                txtUserTarget.Text = SessionHelper.CurrentHoSo.HienThiMucTieu;
            }
            else
            {
                txtUserTarget.Text = SessionHelper.IsGiangVien ? (SessionHelper.CurrentUser?.RoleText ?? "Quản lý") : "Học viên mới";
            }

            if (SessionHelper.IsGiangVien)
            {
                btnSwitchToAdmin.Visibility = Visibility.Visible;
                bool isAdmin = SessionHelper.CurrentUser != null && 
                               string.Equals(SessionHelper.CurrentUser.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);
                txtSwitchToAdmin.Text = isAdmin ? "Về trang Quản trị" : "Về trang Giảng viên";
            }
            else
            {
                btnSwitchToAdmin.Visibility = Visibility.Collapsed;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is DanhGiaNangLucView { IsFirstTime: true })
            {
                // Tài khoản mới đang làm test đầu vào: ẩn Sidebar
                HideSidebar();
            }
            else
            {
                // Các màn hình khác hoặc khảo sát tự nguyện: hiển thị Sidebar
                ShowSidebar();
            }

            if (e.Content is TrangChuView)
            {
                SetActiveMenu("TrangChu");
            }
            else if (e.Content is DanhGiaNangLucView)
            {
                SetActiveMenu("DanhGia");
            }
            else if (e.Content is HocTapView)
            {
                SetActiveMenu("HocTap");
            }
            else if (e.Content is LuyenTapView)
            {
                SetActiveMenu("LuyenTap");
            }
            else if (e.Content is ThiThuView)
            {
                SetActiveMenu("ThiThu");
            }
            else if (e.Content is BaiVietAIView)
            {
                SetActiveMenu("BaiViet");
            }
            else if (e.Content is TienDoHocView)
            {
                SetActiveMenu("TienDo");
            }
            else if (e.Content is TroLyAIView)
            {
                SetActiveMenu("AIChat");
            }
            else if (e.Content is HoSoCaNhanView)
            {
                SetActiveMenu("HoSo");
            }
        }

        public void SetActiveMenu(string menuKey)
        {
            var activeStyle = (Style)FindResource("ActiveSidebarItemStyle");
            var normalStyle = (Style)FindResource("SidebarItemStyle");

            btnNavTrangChu.Style = (menuKey == "TrangChu") ? activeStyle : normalStyle;
            btnNavDanhGia.Style = (menuKey == "DanhGia") ? activeStyle : normalStyle;
            btnNavHocTap.Style = (menuKey == "HocTap") ? activeStyle : normalStyle;
            btnNavLuyenTap.Style = (menuKey == "LuyenTap") ? activeStyle : normalStyle;
            btnNavThiThu.Style = (menuKey == "ThiThu") ? activeStyle : normalStyle;
            btnNavBaiViet.Style = (menuKey == "BaiViet") ? activeStyle : normalStyle;
            btnNavTienDo.Style = (menuKey == "TienDo") ? activeStyle : normalStyle;
            btnNavAIChat.Style = (menuKey == "AIChat") ? activeStyle : normalStyle;
            btnNavHoSo.Style = (menuKey == "HoSo") ? activeStyle : normalStyle;
        }

        private void NavTrangChu_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not TrangChuView)
            {
                MainFrame.Navigate(new TrangChuView());
            }
        }

        private void NavDanhGia_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not DanhGiaNangLucView)
            {
                MainFrame.Navigate(new DanhGiaNangLucView());
            }
        }

        private void BtnSwitchToAdmin_Click(object sender, RoutedEventArgs e)
        {
            var gvWindow = new GUI.Views.GiangVien.GiangVienWindow();
            gvWindow.Show();
            this.Close();
        }

        private void NavHocTap_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not HocTapView)
            {
                MainFrame.Navigate(new HocTapView());
            }
        }

        private void NavLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not LuyenTapView)
            {
                MainFrame.Navigate(new LuyenTapView());
            }
        }

        private void NavThiThu_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not ThiThuView)
            {
                MainFrame.Navigate(new ThiThuView());
            }
        }

        private void NavBaiViet_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not BaiVietAIView)
            {
                MainFrame.Navigate(new BaiVietAIView());
            }
        }

        private void NavTienDo_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not TienDoHocView)
            {
                MainFrame.Navigate(new TienDoHocView());
            }
        }

        private void NavAIChat_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not TroLyAIView)
            {
                MainFrame.Navigate(new TroLyAIView());
            }
        }

        private void NavHoSo_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not HoSoCaNhanView)
            {
                MainFrame.Navigate(new HoSoCaNhanView());
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Logout();
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Visual visual)
            {
                DependencyObject current = visual;
                while (current != null && current != this)
                {
                    if (current is Button || current is TextBox || current is ScrollBar)
                    {
                        return;
                    }
                    current = VisualTreeHelper.GetParent(current);
                }
            }

            if (e.ClickCount == 2)
            {
                ToggleMaximizeRestore();
                return;
            }

            if (e.ButtonState == MouseButtonState.Pressed && this.WindowState != WindowState.Maximized)
            {
                this.DragMove();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximizeRestore_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximizeRestore();
        }

        private void ToggleMaximizeRestore()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
            UpdateMaximizeRestoreState();
        }

        private void UpdateMaximizeRestoreState()
        {
            if (btnMaximizeRestore == null) return;

            bool isMax = (this.WindowState == WindowState.Maximized);
            btnMaximizeRestore.ToolTip = isMax ? "Thu nhỏ màn hình" : "Phóng to toàn màn hình";

            if (icoSquare != null)
            {
                icoSquare.Visibility = isMax ? Visibility.Collapsed : Visibility.Visible;
            }
            if (icoRestore != null)
            {
                icoRestore.Visibility = isMax ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}