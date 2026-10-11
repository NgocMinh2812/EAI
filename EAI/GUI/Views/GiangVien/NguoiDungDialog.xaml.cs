using System.Windows;
using System.Windows.Controls;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class NguoiDungDialog : Window
    {
        public NguoiDungDTO UserData { get; private set; }
        private readonly bool _isEditMode;

        public NguoiDungDialog(NguoiDungDTO? existingUser = null)
        {
            InitializeComponent();
            _isEditMode = existingUser != null;

            if (_isEditMode && existingUser != null)
            {
                UserData = existingUser;
                txtDialogTitle.Text = "Cập nhật Hồ sơ Người dùng";
                txtHoTen.Text = existingUser.HoTen;
                txtTenDangNhap.Text = existingUser.TenDangNhap;
                txtTenDangNhap.IsEnabled = false; // Không đổi tên đăng nhập khi sửa
                txtEmail.Text = existingUser.Email;
                txtSoDienThoai.Text = existingUser.SoDienThoai ?? "";
                chkHoatDong.IsChecked = existingUser.TrangThaiHoatDong;

                // Chọn vai trò
                if (existingUser.TenVaiTro == "GiangVien")
                    cboVaiTro.SelectedIndex = 1;
                else
                    cboVaiTro.SelectedIndex = 0;

                // Chọn trình độ
                string level = existingUser.TrinhDoMucTieu;
                for (int i = 0; i < cboTrinhDo.Items.Count; i++)
                {
                    if (cboTrinhDo.Items[i] is ComboBoxItem item && item.Content.ToString()!.StartsWith(level))
                    {
                        cboTrinhDo.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                UserData = new NguoiDungDTO();
                txtDialogTitle.Text = "Tạo mới Tài khoản Người dùng";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string email = txtEmail.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtHoTen.Focus();
                return;
            }

            if (!_isEditMode && (string.IsNullOrWhiteSpace(tenDangNhap) || tenDangNhap.Length < 3))
            {
                MessageBox.Show("Tên đăng nhập phải có ít nhất 3 ký tự.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email hợp lệ.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }

            UserData.HoTen = hoTen;
            if (!_isEditMode)
            {
                UserData.TenDangNhap = tenDangNhap;
                UserData.MatKhauMaHoa = "123456";
            }
            UserData.Email = email;
            UserData.SoDienThoai = string.IsNullOrEmpty(sdt) ? null : sdt;
            UserData.TrangThaiHoatDong = chkHoatDong.IsChecked == true;

            // Vai trò
            if (cboVaiTro.SelectedItem is ComboBoxItem roleItem && roleItem.Tag != null)
            {
                UserData.MaVaiTro = int.Parse(roleItem.Tag.ToString()!);
                UserData.TenVaiTro = UserData.MaVaiTro == 1 ? "GiangVien" : "HocVien";
            }

            // Trình độ mục tiêu
            if (cboTrinhDo.SelectedItem is ComboBoxItem levelItem)
            {
                string text = levelItem.Content.ToString()!;
                UserData.TrinhDoMucTieu = text.Substring(0, 2); // A1, A2, B1, B2, C1
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
