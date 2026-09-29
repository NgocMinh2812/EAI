using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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

            MainFrame.Navigated += MainFrame_Navigated;

            if (isFirstLogin)
            {
                MainFrame.Navigate(new DanhGiaNangLucView());
            }
            else
            {
                MainFrame.Navigate(new TrangChuView());
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is DanhGiaNangLucView)
            {
                // Khi làm bài kiểm tra đánh giá năng lực, ẩn thanh menu trái để làm bài tập trung
                SidebarBorder.Visibility = Visibility.Collapsed;
                colSidebar.Width = new GridLength(0);
            }
            else
            {
                // Hiện menu trái dùng chung
                SidebarBorder.Visibility = Visibility.Visible;
                colSidebar.Width = new GridLength(240);

                if (e.Content is TrangChuView)
                {
                    SetActiveMenu("TrangChu");
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
        }

        public void SetActiveMenu(string menuKey)
        {
            var activeStyle = (Style)FindResource("ActiveSidebarItemStyle");
            var normalStyle = (Style)FindResource("SidebarItemStyle");

            btnNavTrangChu.Style = (menuKey == "TrangChu") ? activeStyle : normalStyle;
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
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            this.Close();
        }
    }
}