using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BUS;

namespace GUI.Views.GiangVien
{
    public partial class GiangVienWindow : Window
    {
        public GiangVienWindow()
        {
            InitializeComponent();
            ApplyRolePermissions();
            AdminMainFrame.Navigated += AdminMainFrame_Navigated;
            // Mở mặc định trang Tổng quan
            AdminMainFrame.Navigate(new TongQuanView());
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
                txtTopAvatar.Text = user.Initials;
                txtBrandSubtitle.Text = isAdmin ? "Admin Dashboard" : "Cổng Giảng Viên";
            }
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
                MessageBox.Show("Chức năng 'Giám sát hệ thống' chỉ dành cho Quản trị viên hệ thống!", "Không có quyền truy cập", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Logout();
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            this.Close();
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
