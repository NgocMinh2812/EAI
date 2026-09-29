using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GUI.Views
{
    public partial class TrangChuView : Page
    {
        public TrangChuView()
        {
            InitializeComponent();
        }

        private void AssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DanhGiaNangLucView());
        }

        private void BtnHocTap_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new HocTapView());
        }

        private void BtnLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new LuyenTapView());
        }

        private void BtnLuyenTap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new LuyenTapView());
        }

        private void BtnThiThu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new ThiThuView());
        }

        private void BtnBaiVietAI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new BaiVietAIView());
        }

        private void BtnAIChat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void ItemBaiHoc1_Click(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new HocTapView(1));
        }

        private void ItemBaiHoc2_Click(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new HocTapView(2));
        }

        private void AIChatButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
