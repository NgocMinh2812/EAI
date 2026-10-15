using System;
using System.Windows;
using System.Windows.Input;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class ProfileGiangVienDialog : Window
    {
        private readonly NguoiDungDTO _user;
        private readonly NguoiDungBUS _bus = new NguoiDungBUS();

        public ProfileGiangVienDialog()
        {
            InitializeComponent();
            _user = SessionHelper.CurrentUser ?? new NguoiDungDTO();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (_user == null) return;

            txtProfileAvatarInitials.Text = _user.Initials;
            txtProfileFullNameHeader.Text = _user.HoTen;
            txtProfileEmailHeader.Text = _user.Email;
            txtProfileRoleBadge.Text = _user.RoleText;

            string idText = _user.TenVaiTro == "QuanTriVien" ? $"ADM-{_user.MaNguoiDung:D4}" : $"GV-{_user.MaNguoiDung:D4}";
            txtProfileJoinedDate.Text = $"Mã định danh: {idText} • Tham gia: {_user.NgayTao:dd/MM/yyyy}";

            txtHoTen.Text = _user.HoTen;
            txtEmail.Text = _user.Email;
            txtTenDangNhap.Text = _user.TenDangNhap;
            txtSoDienThoai.Text = _user.SoDienThoai ?? "";
            dpNgaySinh.SelectedDate = _user.NgaySinh;

            // Tải thống kê bài học / câu hỏi
            try
            {
                var (sbh, sch, sdt) = _bus.LayThongKeGiangVien(_user.MaNguoiDung);
                txtStatBaiHoc.Text = sbh.ToString();
                txtStatCauHoi.Text = sch.ToString();
                txtStatDeThi.Text = sdt.ToString();
            }
            catch
            {
                txtStatBaiHoc.Text = "—";
                txtStatCauHoi.Text = "—";
                txtStatDeThi.Text = "—";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            txtStatusMessage.Text = "";

            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            DateTime? ngaySinh = dpNgaySinh.SelectedDate;

            string newPass = pbMatKhauMoi.Password;
            string confirmPass = pbXacNhanMatKhau.Password;

            var (success, msg) = _bus.CapNhatHoSoGiangVien(
                _user.MaNguoiDung,
                hoTen,
                string.IsNullOrEmpty(sdt) ? null : sdt,
                ngaySinh,
                string.IsNullOrEmpty(newPass) ? null : newPass,
                string.IsNullOrEmpty(confirmPass) ? null : confirmPass
            );

            if (!success)
            {
                txtStatusMessage.Text = msg;
                return;
            }

            Views.Common.ThongBaoDialog.ThanhCong(this, msg, "Cập nhật hồ sơ");
            this.DialogResult = true;
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
