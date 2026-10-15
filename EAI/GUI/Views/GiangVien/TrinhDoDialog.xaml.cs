using System;
using System.Windows;
using System.Windows.Input;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class TrinhDoDialog : Window
    {
        public TrinhDoDTO TrinhDoData { get; private set; }
        private readonly bool _isEditMode;

        public TrinhDoDialog(TrinhDoDTO? td = null)
        {
            InitializeComponent();
            _isEditMode = td != null;
            TrinhDoData = td ?? new TrinhDoDTO();

            if (_isEditMode)
            {
                txtDialogTitle.Text = "Chỉnh Sửa Trình Độ";
                txtMaCapDo.Text = TrinhDoData.MaCapDo;
                txtTenTrinhDo.Text = TrinhDoData.TenTrinhDo;
                txtMoTa.Text = TrinhDoData.MoTa ?? "";
            }
            else
            {
                txtDialogTitle.Text = "Thêm Trình Độ Mới";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            txtErrorMessage.Text = "";

            string capDo = txtMaCapDo.Text.Trim().ToUpper();
            string ten = txtTenTrinhDo.Text.Trim();
            string moTa = txtMoTa.Text.Trim();

            if (string.IsNullOrWhiteSpace(capDo))
            {
                txtErrorMessage.Text = "Vui lòng nhập mã cấp độ (ví dụ: A1, B2, C1).";
                txtMaCapDo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ten))
            {
                txtErrorMessage.Text = "Vui lòng nhập tên trình độ (ví dụ: Sơ cấp, Trung cấp).";
                txtTenTrinhDo.Focus();
                return;
            }

            TrinhDoData.MaCapDo = capDo;
            TrinhDoData.TenTrinhDo = ten;
            TrinhDoData.MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa;

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
