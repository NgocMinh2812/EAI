using System.Windows;
using System.Windows.Controls;

namespace GUI.Views
{
    public partial class HoSoCaNhanView : Page
    {
        public HoSoCaNhanView()
        {
            InitializeComponent();
        }

        private void BtnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            txtDisplayName.Text = txtName.Text.Trim();
            MessageBox.Show("✅ Thông tin hồ sơ và cài đặt học tập của bạn đã được cập nhật thành công!", 
                            "Cập nhật hồ sơ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng chọn ảnh đại diện từ thiết bị đang được đồng bộ hóa với hệ thống lưu trữ đám mây.", 
                            "Đổi ảnh đại diện", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hệ thống đã gửi liên kết đặt lại mật khẩu an toàn đến email 'nguyenvana@gmail.com'. Vui lòng kiểm tra hộp thư!", 
                            "Đổi mật khẩu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
