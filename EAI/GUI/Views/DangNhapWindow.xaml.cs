using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BUS;
using DTO;

namespace GUI.Views
{
    public partial class DangNhapWindow : Window
    {
        private readonly NguoiDungBUS _nguoiDungBUS = new NguoiDungBUS();
        private const string EmailPlaceholder = "ten@email.com";
        private const string PasswordPlaceholder = "password";

        private const string NamePlaceholder = "Nguyễn Văn A";

        public DangNhapWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void TabLogin_Click(object sender, MouseButtonEventArgs e)
        {
            SwitchToLoginTab();
        }

        private void TabRegister_Click(object sender, MouseButtonEventArgs e)
        {
            SwitchToRegisterTab();
        }

        private void SwitchToLoginTab()
        {
            pnlLogin.Visibility = Visibility.Visible;
            pnlRegister.Visibility = Visibility.Collapsed;

            btnLogin.IsDefault = true;
            btnRegister.IsDefault = false;

            tabLoginBorder.Background = Brushes.White;
            tabLoginShadow.Opacity = 0.06;
            tabLoginText.FontWeight = FontWeights.SemiBold;
            tabLoginText.Foreground = (Brush)FindResource("TextDark");

            tabRegisterBorder.Background = Brushes.Transparent;
            tabRegisterShadow.Opacity = 0;
            tabRegisterText.FontWeight = FontWeights.Medium;
            tabRegisterText.Foreground = (Brush)FindResource("TextGray");

            footerLogin.Visibility = Visibility.Visible;
            footerRegister.Visibility = Visibility.Collapsed;
        }

        private void SwitchToRegisterTab()
        {
            pnlLogin.Visibility = Visibility.Collapsed;
            pnlRegister.Visibility = Visibility.Visible;

            btnLogin.IsDefault = false;
            btnRegister.IsDefault = true;

            tabRegisterBorder.Background = Brushes.White;
            tabRegisterShadow.Opacity = 0.06;
            tabRegisterText.FontWeight = FontWeights.SemiBold;
            tabRegisterText.Foreground = (Brush)FindResource("TextDark");

            tabLoginBorder.Background = Brushes.Transparent;
            tabLoginShadow.Opacity = 0;
            tabLoginText.FontWeight = FontWeights.Medium;
            tabLoginText.Foreground = (Brush)FindResource("TextGray");

            footerLogin.Visibility = Visibility.Collapsed;
            footerRegister.Visibility = Visibility.Visible;
        }

        private void TxtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == EmailPlaceholder)
            {
                txtEmail.Text = "";
                txtEmail.Foreground = (Brush)FindResource("TextDark");
            }
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = EmailPlaceholder;
                txtEmail.Foreground = (Brush)FindResource("TextMuted");
            }
        }

        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == PasswordPlaceholder)
            {
                txtPassword.Password = "";
                txtPassword.Foreground = (Brush)FindResource("TextDark");
            }
        }

        private void TxtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                txtPassword.Password = PasswordPlaceholder;
                txtPassword.Foreground = (Brush)FindResource("TextMuted");
            }
        }

        private void TxtRegName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRegName.Text == NamePlaceholder)
            {
                txtRegName.Text = "";
                txtRegName.Foreground = (Brush)FindResource("TextDark");
            }
        }

        private void TxtRegName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegName.Text))
            {
                txtRegName.Text = NamePlaceholder;
                txtRegName.Foreground = (Brush)FindResource("TextMuted");
            }
        }

        private void TxtRegEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRegEmail.Text == EmailPlaceholder)
            {
                txtRegEmail.Text = "";
                txtRegEmail.Foreground = (Brush)FindResource("TextDark");
            }
        }

        private void TxtRegEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegEmail.Text))
            {
                txtRegEmail.Text = EmailPlaceholder;
                txtRegEmail.Foreground = (Brush)FindResource("TextMuted");
            }
        }

        private void TxtRegPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRegPassword.Password == PasswordPlaceholder)
            {
                txtRegPassword.Password = "";
                txtRegPassword.Foreground = (Brush)FindResource("TextDark");
            }
        }

        private void TxtRegPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegPassword.Password))
            {
                txtRegPassword.Password = PasswordPlaceholder;
                txtRegPassword.Foreground = (Brush)FindResource("TextMuted");
            }
        }

        private void TxtRegConfirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRegConfirmPassword.Password == PasswordPlaceholder)
            {
                txtRegConfirmPassword.Password = "";
                txtRegConfirmPassword.Foreground = (Brush)FindResource("TextDark");
            }
        }

        private void TxtRegConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegConfirmPassword.Password))
            {
                txtRegConfirmPassword.Password = PasswordPlaceholder;
                txtRegConfirmPassword.Foreground = (Brush)FindResource("TextMuted");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string emailOrUser = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            if (emailOrUser == EmailPlaceholder || string.IsNullOrEmpty(emailOrUser))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email hoặc tên đăng nhập.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }

            if (password == PasswordPlaceholder || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return;
            }

            var (success, msg, user) = _nguoiDungBUS.DangNhap(emailOrUser, password);
            if (!success || user == null)
            {
                MessageBox.Show(msg, "Đăng nhập không thành công", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Phân quyền theo vai trò trong CSDL
            if (user.TenVaiTro == "GiangVien" || user.TenVaiTro == "QuanTriVien")
            {
                var gvWindow = new GUI.Views.GiangVien.GiangVienWindow();
                gvWindow.Show();
                this.Close();
            }
            else
            {
                MainWindow mainWindow = new MainWindow(isFirstLogin: user.LaLanDauDangNhap);
                mainWindow.Show();
                this.Close();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = txtRegName.Text.Trim();
            string email = txtRegEmail.Text.Trim();
            string password = txtRegPassword.Password;
            string confirmPassword = txtRegConfirmPassword.Password;

            if (name == NamePlaceholder || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập họ và tên của bạn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (email == EmailPlaceholder || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Chúc mừng {name} đã đăng ký tài khoản thành công! Hãy bắt đầu với bài kiểm tra đánh giá năng lực.", "Đăng ký thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            MainWindow mainWindow = new MainWindow(isFirstLogin: true);
            mainWindow.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
