using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class LichSuBaiNopDialog : Window
    {
        public LichSuBaiNopDialog(BaiTapDeThiDTO deThi, List<LuotLamBaiDTO> dsLichSu)
        {
            InitializeComponent();
            txtTitle.Text = $"Danh sách bài nộp: {deThi?.TenBaiTapDeThi ?? ""}";

            if (dsLichSu == null || dsLichSu.Count == 0)
            {
                pnlEmpty.Visibility = Visibility.Visible;
                lstAttempts.Visibility = Visibility.Collapsed;
            }
            else
            {
                pnlEmpty.Visibility = Visibility.Collapsed;
                lstAttempts.Visibility = Visibility.Visible;
                lstAttempts.ItemsSource = dsLichSu;
            }
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
