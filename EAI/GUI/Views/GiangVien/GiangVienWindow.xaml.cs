using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BUS;

namespace GUI.Views.GiangVien
{
    public partial class GiangVienWindow : Window
    {
        private bool _isSidebarCollapsed = false;
        private FrameworkElement[] _navIcons = Array.Empty<FrameworkElement>();
        private TextBlock[] _navLabels = Array.Empty<TextBlock>();
        private StackPanel[] _navPanels = Array.Empty<StackPanel>();

        public GiangVienWindow()
        {
            InitializeComponent();
            InitSidebarElements();
            ApplyRolePermissions();
            AdminMainFrame.Navigated += AdminMainFrame_Navigated;
            // Mở mặc định trang Tổng quan
            AdminMainFrame.Navigate(new TongQuanView());
        }

        private void InitSidebarElements()
        {
            _navIcons = new FrameworkElement[] {
                icoNavTongQuan, icoNavNguoiDung, icoNavDanhMuc, icoNavNoiDung,
                icoNavNganHang, icoNavDeThi, icoNavKhaoSat, icoNavAISinhDe,
                icoNavGiamSat, icoNavToStudentHome
            };
            _navLabels = new TextBlock[] {
                lblNavTongQuan, lblNavNguoiDung, lblNavDanhMuc, lblNavNoiDung,
                lblNavNganHang, lblNavDeThi, lblNavKhaoSat, lblNavAISinhDe,
                lblNavGiamSat, lblNavToStudentHome
            };
            _navPanels = new StackPanel[] {
                spNavTongQuan, spNavNguoiDung, spNavDanhMuc, spNavNoiDung,
                spNavNganHang, spNavDeThi, spNavKhaoSat, spNavAISinhDe,
                spNavGiamSat, spNavToStudentHome
            };
        }

        private void BtnToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            SetSidebarCollapsed(!_isSidebarCollapsed);
        }

        private void SetSidebarCollapsed(bool collapsed)
        {
            _isSidebarCollapsed = collapsed;
            var user = SessionHelper.CurrentUser;
            bool isAdmin = user != null && string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);

            if (collapsed)
            {
                // Thu gọn về 68px
                colSidebar.Width = new GridLength(68);
                colSidebar.MinWidth = 68;
                colSidebar.MaxWidth = 68;

                // Header
                pnlBrandFull.Visibility = Visibility.Collapsed;
                btnToggleSidebarMini.Visibility = Visibility.Visible;

                // Section Headers
                headerChinh.Visibility = Visibility.Collapsed;
                headerQuanLy.Visibility = Visibility.Collapsed;
                headerTriTueAI.Visibility = Visibility.Collapsed;
                headerHeThong.Visibility = Visibility.Collapsed;
                headerChuyenDoi.Visibility = Visibility.Collapsed;

                // Separators phân cách giữa các nhóm
                sep1.Visibility = Visibility.Visible;
                sep2.Visibility = Visibility.Visible;
                sep3.Visibility = Visibility.Visible;
                sep4.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                sep5.Visibility = Visibility.Visible;

                // Ẩn chữ nhãn, căn giữa icon
                if (_navLabels != null)
                {
                    foreach (var lbl in _navLabels)
                        lbl.Visibility = Visibility.Collapsed;
                }
                if (_navIcons != null)
                {
                    foreach (var ico in _navIcons)
                        ico.Margin = new Thickness(0);
                }
                if (_navPanels != null)
                {
                    foreach (var pnl in _navPanels)
                        pnl.HorizontalAlignment = HorizontalAlignment.Center;
                }
                badgeNavAISinhDe.Visibility = Visibility.Collapsed;

                // Footer User Profile
                pnlUserFull.Visibility = Visibility.Collapsed;
                pnlUserCollapsed.Visibility = Visibility.Visible;
            }
            else
            {
                // Mở rộng về 240px
                colSidebar.Width = new GridLength(240);
                colSidebar.MinWidth = 200;
                colSidebar.MaxWidth = 280;

                // Header
                pnlBrandFull.Visibility = Visibility.Visible;
                btnToggleSidebarMini.Visibility = Visibility.Collapsed;

                // Section Headers
                headerChinh.Visibility = Visibility.Visible;
                headerQuanLy.Visibility = Visibility.Visible;
                headerTriTueAI.Visibility = Visibility.Visible;
                headerHeThong.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                headerChuyenDoi.Visibility = Visibility.Visible;

                // Separators
                sep1.Visibility = Visibility.Collapsed;
                sep2.Visibility = Visibility.Collapsed;
                sep3.Visibility = Visibility.Collapsed;
                sep4.Visibility = Visibility.Collapsed;
                sep5.Visibility = Visibility.Collapsed;

                // Hiện lại chữ nhãn, căn trái icon
                if (_navLabels != null)
                {
                    foreach (var lbl in _navLabels)
                        lbl.Visibility = Visibility.Visible;
                }
                if (_navIcons != null)
                {
                    foreach (var ico in _navIcons)
                        ico.Margin = new Thickness(0, 0, 10, 0);
                }
                if (_navPanels != null)
                {
                    foreach (var pnl in _navPanels)
                        pnl.HorizontalAlignment = HorizontalAlignment.Left;
                }
                badgeNavAISinhDe.Visibility = Visibility.Visible;

                // Footer User Profile
                pnlUserFull.Visibility = Visibility.Visible;
                pnlUserCollapsed.Visibility = Visibility.Collapsed;
            }
        }

        private void ApplyRolePermissions()
        {
            var user = SessionHelper.CurrentUser;
            bool isAdmin = user != null && string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);

            if (!isAdmin)
            {
                // Giảng viên -> Ẩn mục Giám sát hệ thống
                headerHeThong.Visibility = Visibility.Collapsed;
                btnNavGiamSat.Visibility = Visibility.Collapsed;
            }

            if (user != null)
            {
                txtCurrentUserName.Text = user.HoTen;
                txtCurrentUserRole.Text = user.RoleText;
                txtAvatarInitials.Text = user.Initials;
                txtAvatarInitialsMini.Text = user.Initials;
                pnlUserCollapsedAvatar.ToolTip = $"{user.HoTen} ({user.RoleText})";
                txtTopAvatar.Text = user.Initials;
                txtBrandSubtitle.Text = isAdmin ? "Admin Dashboard" : "Cổng Giảng Viên";
            }
        }

        public void ShowModalOverlay()
        {
            OverlayDimLayer.Visibility = Visibility.Visible;
            MainContentContainer.Effect = new System.Windows.Media.Effects.BlurEffect 
            { 
                Radius = 8, 
                RenderingBias = System.Windows.Media.Effects.RenderingBias.Performance 
            };
        }

        public void HideModalOverlay()
        {
            OverlayDimLayer.Visibility = Visibility.Collapsed;
            MainContentContainer.Effect = null;
        }

        private void AdminMainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            ResetNavButtons();

            if (e.Content is TongQuanView)
            {
                SetActiveButton(btnNavTongQuan);
                txtBreadcrumb.Text = "Tổng quan";
            }
            else if (e.Content is NguoiDungView)
            {
                SetActiveButton(btnNavNguoiDung);
                txtBreadcrumb.Text = "Người dùng";
            }
            else if (e.Content is DanhMucView)
            {
                SetActiveButton(btnNavDanhMuc);
                txtBreadcrumb.Text = "Danh mục & Cấu trúc";
            }
            else if (e.Content is NoiDungHocTapView)
            {
                SetActiveButton(btnNavNoiDung);
                txtBreadcrumb.Text = "Nội dung học tập";
            }
            else if (e.Content is NganHangCauHoiView)
            {
                SetActiveButton(btnNavNganHang);
                txtBreadcrumb.Text = "Ngân hàng câu hỏi";
            }
            else if (e.Content is DeThiLuyenTapView)
            {
                SetActiveButton(btnNavDeThi);
                txtBreadcrumb.Text = "Đề thi & Luyện tập";
            }
            else if (e.Content is KhaoSatDauVaoView)
            {
                SetActiveButton(btnNavKhaoSat);
                txtBreadcrumb.Text = "Khảo sát đầu vào";
            }
            else if (e.Content is AISinhDeView)
            {
                SetActiveButton(btnNavAISinhDe);
                txtBreadcrumb.Text = "AI sinh đề";
            }
            else if (e.Content is GiamSatHeThongView)
            {
                SetActiveButton(btnNavGiamSat);
                txtBreadcrumb.Text = "Giám sát hệ thống";
            }
        }

        private void SetActiveButton(Button btn)
        {
            if (btn != null && TryFindResource("AdminActiveNavItemStyle") is Style activeStyle)
            {
                btn.Style = activeStyle;
            }
        }

        private void ResetNavButtons()
        {
            if (TryFindResource("AdminNavItemStyle") is Style normalStyle)
            {
                btnNavTongQuan.Style = normalStyle;
                btnNavNguoiDung.Style = normalStyle;
                btnNavDanhMuc.Style = normalStyle;
                btnNavNoiDung.Style = normalStyle;
                btnNavNganHang.Style = normalStyle;
                btnNavDeThi.Style = normalStyle;
                btnNavKhaoSat.Style = normalStyle;
                btnNavAISinhDe.Style = normalStyle;
                btnNavGiamSat.Style = normalStyle;
            }
        }

        private void NavTongQuan_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not TongQuanView)
                AdminMainFrame.Navigate(new TongQuanView());
        }

        private void NavNguoiDung_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not NguoiDungView)
                AdminMainFrame.Navigate(new NguoiDungView());
        }

        private void NavDanhMuc_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not DanhMucView)
                AdminMainFrame.Navigate(new DanhMucView());
        }

        private void NavNoiDung_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not NoiDungHocTapView)
                AdminMainFrame.Navigate(new NoiDungHocTapView());
        }

        private void NavNganHang_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not NganHangCauHoiView)
                AdminMainFrame.Navigate(new NganHangCauHoiView());
        }

        private void NavDeThi_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not DeThiLuyenTapView)
                AdminMainFrame.Navigate(new DeThiLuyenTapView());
        }

        private void NavKhaoSat_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not KhaoSatDauVaoView)
                AdminMainFrame.Navigate(new KhaoSatDauVaoView());
        }

        private void NavAISinhDe_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not AISinhDeView)
                AdminMainFrame.Navigate(new AISinhDeView());
        }

        private void NavGiamSat_Click(object sender, RoutedEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            bool isAdmin = user != null && string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);
            if (!isAdmin)
            {
                Common.ThongBaoDialog.CanhBao(this, "Chức năng 'Giám sát hệ thống' chỉ dành cho Quản trị viên hệ thống!", "Không có quyền truy cập");
                return;
            }

            if (AdminMainFrame.Content is not GiamSatHeThongView)
                AdminMainFrame.Navigate(new GiamSatHeThongView());
        }

        private void TopNavTroLyAI_Click(object sender, RoutedEventArgs e)
        {
            NavAISinhDe_Click(sender, e);
        }

        private void NavToStudentHome_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void PnlUserProfile_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            bool isAdmin = user != null && string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);

            var dlg = new ProfileGiangVienDialog();
            if (Common.ModalHelper.ShowDialogWithDim(dlg, this) == true)
            {
                // Đồng bộ cập nhật thông tin hiển thị trên thanh sidebar và header
                ApplyRolePermissions();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            bool confirm = Common.ThongBaoDialog.XacNhan(
                this, 
                "Bạn có chắc chắn muốn đăng xuất khỏi phiên làm việc hiện tại?", 
                "Xác nhận đăng xuất", 
                confirmText: "Đăng xuất", 
                cancelText: "Ở lại");

            if (confirm)
            {
                SessionHelper.Logout();
                var dangNhapWindow = new DangNhapWindow();
                dangNhapWindow.Show();
                this.Close();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
