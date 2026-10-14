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

            txtRegNameError.Visibility = Visibility.Collapsed;
            txtRegEmailError.Visibility = Visibility.Collapsed;
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
            txtRegNameError.Visibility = Visibility.Collapsed;
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
                txtRegNameError.Visibility = Visibility.Collapsed;
            }
            else
            {
                string name = txtRegName.Text.Trim();
                if (name != NamePlaceholder)
                {
                    if (!NguoiDungBUS.KiemTraDinhDangHoTen(name, out string error, out string normalizedName))
                    {
                        txtRegNameError.Text = $"⚠️ {error}";
                        txtRegNameError.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtRegName.Text = normalizedName;
                        txtRegNameError.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void TxtRegEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtRegEmailError.Visibility = Visibility.Collapsed;
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
                txtRegEmailError.Visibility = Visibility.Collapsed;
            }
            else
            {
                string email = txtRegEmail.Text.Trim();
                if (email != EmailPlaceholder && NguoiDungBUS.Instance.KiemTraEmailTonTai(email))
                {
                    txtRegEmailError.Text = "⚠️ Email này đã được đăng ký. Vui lòng chuyển sang Đăng nhập!";
                    txtRegEmailError.Visibility = Visibility.Visible;
                }
                else
                {
                    txtRegEmailError.Visibility = Visibility.Collapsed;
                }
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
            if (name == NamePlaceholder) name = "";
            string email = txtRegEmail.Text.Trim();
            if (email == EmailPlaceholder) email = "";
            string password = txtRegPassword.Password;
            if (password == PasswordPlaceholder) password = "";
            string confirmPassword = txtRegConfirmPassword.Password;
            if (confirmPassword == PasswordPlaceholder) confirmPassword = "";

            if (!NguoiDungBUS.KiemTraDinhDangHoTen(name, out string nameError, out string normalizedName))
            {
                txtRegNameError.Text = $"⚠️ {nameError}";
                txtRegNameError.Visibility = Visibility.Visible;
                MessageBox.Show(nameError, "Họ và tên không hợp lệ", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtRegName.Focus();
                return;
            }
            name = normalizedName;
            txtRegName.Text = normalizedName;
            txtRegNameError.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrEmpty(email) && NguoiDungBUS.Instance.KiemTraEmailTonTai(email))
            {
                txtRegEmailError.Text = "⚠️ Email này đã được đăng ký trên hệ thống!";
                txtRegEmailError.Visibility = Visibility.Visible;
                var res = MessageBox.Show($"Email '{email}' đã được đăng ký trên hệ thống!\n\nBạn có muốn chuyển sang màn hình Đăng nhập ngay bây giờ không?", 
                                          "Email đã được đăng ký", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    SwitchToLoginTab();
                    txtEmail.Text = email;
                    txtEmail.Foreground = (Brush)FindResource("TextDark");
                    txtPassword.Password = "";
                    txtPassword.Focus();
                }
                return;
            }

            bool success = NguoiDungBUS.Instance.DangKy(name, email, password, confirmPassword, out string errorMessage, out NguoiDungDTO? newUser);
            if (!success || newUser == null)
            {
                MessageBox.Show(errorMessage, "Đăng ký thất bại", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            txtRegEmailError.Visibility = Visibility.Collapsed;
            MessageBox.Show($"Chúc mừng {newUser.HoTen} đã đăng ký tài khoản thành công! Hệ thống sẽ tiến hành đánh giá năng lực ban đầu để đề xuất lộ trình và bài học phù hợp cho bạn.", "Đăng ký thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            // Học viên mới đăng ký: Chuyển ngay vào luồng khảo sát đánh giá năng lực (Hỏi mức độ -> Làm khảo sát -> Đề xuất bài học)
            MainWindow mainWindow = new MainWindow(isFirstLogin: true);
            mainWindow.Show();
            this.Close();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }

        private void TxtRegConfirmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegisterButton_Click(sender, e);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
