using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace GUI.Views.GiangVien
{
    public partial class TongQuanView : Page
    {
        public TongQuanView()
        {
            InitializeComponent();
        }

        private void BtnTaoFlashcard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở hộp thoại tạo nhanh Thẻ Flashcard từ vựng mới kèm phiên âm IPA và audio phát âm chuẩn.", 
                            "Quản lý Flashcard", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnChinhSua_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new NoiDungHocTapView());
        }

        private void BtnPhanTich_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new NguoiDungView());
        }

        private void BtnQuickTaoCauHoi_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new NganHangCauHoiView());
        }

        private void BtnQuickTaoDeThi_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeThiLuyenTapView());
        }

        private void BtnQuickAISinh_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new AISinhDeView());
        }

        private void BtnQuickGiamSat_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new GiamSatHeThongView());
        }
    }
}
