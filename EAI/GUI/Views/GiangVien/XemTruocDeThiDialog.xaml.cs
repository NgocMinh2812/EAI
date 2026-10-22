using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class XemTruocDeThiDialog : Window
    {
        public XemTruocDeThiDialog(BaiTapDeThiDTO deThi, List<CauHoiKhaoSatDTO> dsCauHoi)
        {
            InitializeComponent();
            txtTitle.Text = $"Xem trước: {deThi?.TenBaiTapDeThi ?? ""}";
            txtSubtitle.Text = $"{deThi?.LoaiHinhHienThi ?? ""} • {deThi?.ThoiGianHienThi ?? ""} • {dsCauHoi?.Count ?? 0} câu hỏi";
            icQuestions.ItemsSource = dsCauHoi;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
