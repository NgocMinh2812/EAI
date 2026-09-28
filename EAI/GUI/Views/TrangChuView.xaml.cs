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
            this.NavigationService?.Navigate(new HocTapView());
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
            MessageBox.Show("Tính năng Trợ lý AI Chat thông minh hỗ trợ giải đáp 24/7 đang sẵn sàng phục vụ bạn!", "EnglishAI Chat", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
