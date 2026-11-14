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
        private readonly System.Collections.Generic.Dictionary<Type, Page> _pageCache = new();

        public Page GetOrCreatePage<T>() where T : Page, new()
        {
            var type = typeof(T);
            if (!_pageCache.TryGetValue(type, out var page))
            {
                page = new T();
                _pageCache[type] = page;
            }
            return page;
        }

        public void NavigateToCached<T>() where T : Page, new()
        {
            var page = GetOrCreatePage<T>();
            if (MainFrame.Content != page)
            {
                MainFrame.Navigate(page);
            }
        }

        public MainWindow(bool isFirstLogin = false)
        {
            InitializeComponent();
            Instance = this;
            this.WindowState = WindowState.Maximized;
            this.StateChanged += (s, e) => UpdateMaximizeRestoreState();
            UpdateMaximizeRestoreState();

            CapNhatThongTinHocVien();

            MainFrame.Navigated += MainFrame_Navigated;
            MainFrame.Navigating += MainFrame_Navigating;

            // Đăng ký sự kiện đóng sidebar Trợ lý AI
            aiAssistantSidebar.CloseRequested += DongAIAssistantSidebar;

            if (isFirstLogin)
            {
                // Cơ chế kiểm soát Lần đầu đăng nhập: tự động ẩn Sidebar và điều hướng vào làm bài test đầu vào
                HideSidebar();
                MainFrame.Navigate(new DanhGiaNangLucView(isFirstTime: true));
            }
            else
            {
                // Thành viên đã đăng ký: Vào thẳng Trang chủ sử dụng page cache
                ShowSidebar();
                MainFrame.Navigate(GetOrCreatePage<TrangChuView>());
            }
            Closed += (s, e) => _pageCache.Clear();
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

        public bool CanNavigateAway()
        {
            if (MainFrame.Content is ThiThuView thiThu && thiThu.IsExamInProgress)
            {
                return thiThu.RequestExitExam();
            }
            if (MainFrame.Content is LuyenTapView luyenTap && luyenTap.IsPracticeInProgress)
            {
                return luyenTap.RequestExitPractice();
            }
            if (MainFrame.Content is DanhGiaNangLucView khaoSat && khaoSat.IsSurveyInProgress)
            {
                return khaoSat.RequestExitSurvey();
            }
            return true;
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (MainFrame.Content is ThiThuView thiThu && thiThu.IsExamInProgress)
            {
                if (!thiThu.RequestExitExam())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (MainFrame.Content is LuyenTapView luyenTap && luyenTap.IsPracticeInProgress)
            {
                if (!luyenTap.RequestExitPractice())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (MainFrame.Content is DanhGiaNangLucView khaoSat && khaoSat.IsSurveyInProgress)
            {
                if (!khaoSat.RequestExitSurvey())
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is DanhGiaNangLucView { IsFirstTime: true })
            {
                // Tài khoản mới đang làm test đầu vào: ẩn Sidebar
                HideSidebar();
            }
            else if (e.Content is ThiThuView thiThu && thiThu.IsExamInProgress)
            {
                HideSidebar();
            }
            else if (e.Content is LuyenTapView luyenTap && luyenTap.IsPracticeInProgress)
            {
                HideSidebar();
            }
            else
            {
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

            // Đồng bộ trang hiện tại cho Trợ lý AI
            aiAssistantSidebar?.UpdateCurrentPage(e.Content?.GetType().Name);
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
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not TrangChuView)
            {
                MainFrame.Navigate(GetOrCreatePage<TrangChuView>());
            }
        }

        private void NavDanhGia_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not DanhGiaNangLucView)
            {
                MainFrame.Navigate(GetOrCreatePage<DanhGiaNangLucView>());
            }
        }

        private void BtnSwitchToAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            var gvWindow = new GUI.Views.GiangVien.GiangVienWindow();
            gvWindow.Show();
            this.Close();
        }

        private void NavHocTap_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not HocTapView)
            {
                MainFrame.Navigate(GetOrCreatePage<HocTapView>());
            }
        }

        private void NavLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not LuyenTapView)
            {
                MainFrame.Navigate(GetOrCreatePage<LuyenTapView>());
            }
        }

        private void NavThiThu_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not ThiThuView)
            {
                MainFrame.Navigate(GetOrCreatePage<ThiThuView>());
            }
        }

        private void NavBaiViet_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not BaiVietAIView)
            {
                MainFrame.Navigate(GetOrCreatePage<BaiVietAIView>());
            }
        }

        private void NavTienDo_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not TienDoHocView)
            {
                MainFrame.Navigate(GetOrCreatePage<TienDoHocView>());
            }
        }

        private bool _isAssistantOpen = false;

        public void ToggleAIAssistantSidebar()
        {
            if (_isAssistantOpen)
            {
                DongAIAssistantSidebar();
            }
            else
            {
                MoAIAssistantSidebar();
            }
        }

        public void MoAIAssistantSidebar()
        {
            _isAssistantOpen = true;
            aiAssistantSidebar.Visibility = Visibility.Visible;
            colRightAssistant.Width = new GridLength(680);
            aiAssistantSidebar.UpdateCurrentPage(MainFrame.Content?.GetType().Name);
        }

        public void DongAIAssistantSidebar()
        {
            _isAssistantOpen = false;
            colRightAssistant.Width = new GridLength(0);
            aiAssistantSidebar.Visibility = Visibility.Collapsed;
        }

        private void NavAIChat_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not TroLyAIView)
            {
                MainFrame.Navigate(GetOrCreatePage<TroLyAIView>());
            }
        }

        private void NavHoSo_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;
            if (MainFrame.Content is not HoSoCaNhanView)
            {
                MainFrame.Navigate(GetOrCreatePage<HoSoCaNhanView>());
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CanNavigateAway()) return;

            bool confirm = GUI.Views.Common.ThongBaoDialog.XacNhan(
                this, 
                "Bạn có chắc chắn muốn đăng xuất khỏi tài khoản học viên hiện tại?", 
                "Xác nhận đăng xuất", 
                confirmText: "Đăng xuất", 
                cancelText: "Ở lại");

            if (!confirm) return;

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
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!CanNavigateAway())
            {
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
        }
    }
}