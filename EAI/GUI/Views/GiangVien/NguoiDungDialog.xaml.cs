using System.Windows;
using System.Windows.Controls;
using BUS;
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

            DataObject.AddPastingHandler(txtSoDienThoai, OnPhonePaste);
            DataObject.AddPastingHandler(txtTenDangNhap, OnUsernamePaste);

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
                if (existingUser.TenVaiTro == "QuanTriVien" || existingUser.MaVaiTro == 2)
                    cboVaiTro.SelectedIndex = 2;
                else if (existingUser.TenVaiTro == "GiangVien" || existingUser.MaVaiTro == 0)
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
                pnlDefaultPasswordNote.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserData = new NguoiDungDTO();
                txtDialogTitle.Text = "Tạo mới Tài khoản Người dùng";
                pnlDefaultPasswordNote.Visibility = Visibility.Visible;
            }
        }

        private void TxtSoDienThoai_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (sender is TextBox tb)
            {
                bool isPlusAtStart = e.Text == "+" && tb.SelectionStart == 0 && !tb.Text.Contains("+");
                if (!char.IsDigit(e.Text, 0) && !isPlusAtStart)
                {
                    e.Handled = true;
                }
            }
        }

        private void TxtTenDangNhap_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[a-zA-Z0-9_.]+$"))
            {
                e.Handled = true;
            }
        }

        private void OnPhonePaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = ((string)e.DataObject.GetData(DataFormats.Text)).Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^(\+)?[0-9]+$"))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void OnUsernamePaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = ((string)e.DataObject.GetData(DataFormats.Text)).Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^[a-zA-Z0-9_.]+$"))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string email = txtEmail.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();

            // 1. Kiểm tra Họ và tên
            if (!NguoiDungBUS.KiemTraDinhDangHoTen(hoTen, out string nameErr, out string normalizedHoTen))
            {
                Views.Common.ThongBaoDialog.CanhBao(this, nameErr, "Họ và tên không hợp lệ");
                txtHoTen.Focus();
                txtHoTen.SelectAll();
                return;
            }
            hoTen = normalizedHoTen;
            txtHoTen.Text = normalizedHoTen;

            // 2. Kiểm tra Tên đăng nhập (khi tạo mới)
            if (!_isEditMode)
            {
                if (string.IsNullOrWhiteSpace(tenDangNhap) || tenDangNhap.Length < 3)
                {
                    Views.Common.ThongBaoDialog.CanhBao(this, "Tên đăng nhập phải có ít nhất 3 ký tự.", "Tên đăng nhập không hợp lệ");
                    txtTenDangNhap.Focus();
                    txtTenDangNhap.SelectAll();
                    return;
                }
                if (tenDangNhap.Length > 50)
                {
                    Views.Common.ThongBaoDialog.CanhBao(this, "Tên đăng nhập không được vượt quá 50 ký tự.", "Tên đăng nhập không hợp lệ");
                    txtTenDangNhap.Focus();
                    txtTenDangNhap.SelectAll();
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(tenDangNhap, @"^[a-zA-Z0-9_.]+$"))
                {
                    Views.Common.ThongBaoDialog.CanhBao(this, "Tên đăng nhập chỉ được chứa chữ cái không dấu, chữ số, dấu gạch dưới (_) hoặc dấu chấm (.), không được chứa khoảng trắng hay ký tự đặc biệt.", "Tên đăng nhập không hợp lệ");
                    txtTenDangNhap.Focus();
                    txtTenDangNhap.SelectAll();
                    return;
                }
            }

            // 3. Kiểm tra Địa chỉ Email
            if (string.IsNullOrWhiteSpace(email) || !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                Views.Common.ThongBaoDialog.CanhBao(this, "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: user@domain.com).", "Email không hợp lệ");
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }
            if (email.Length > 100)
            {
                Views.Common.ThongBaoDialog.CanhBao(this, "Địa chỉ email không được vượt quá 100 ký tự.", "Email không hợp lệ");
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }

            // 4. Kiểm tra Số điện thoại
            if (!string.IsNullOrWhiteSpace(sdt))
            {
                if (!NguoiDungBUS.KiemTraDinhDangSoDienThoai(sdt, out string sdtErr))
                {
                    Views.Common.ThongBaoDialog.CanhBao(this, sdtErr, "Số điện thoại không hợp lệ");
                    txtSoDienThoai.Focus();
                    txtSoDienThoai.SelectAll();
                    return;
                }
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
                UserData.TenVaiTro = UserData.MaVaiTro switch
                {
                    0 => "GiangVien",
                    2 => "QuanTriVien",
                    _ => "HocVien"
                };
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

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
