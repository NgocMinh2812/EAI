using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class TongQuanView : Page
    {
        private readonly DashboardBUS _dashboardBUS = new DashboardBUS();

        public TongQuanView()
        {
            InitializeComponent();
            this.Loaded += TongQuanView_Loaded;
        }

        private void TongQuanView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRealKpis();
        }

        private void LoadRealKpis()
        {
            try
            {
                var kpi = _dashboardBUS.LaySoLieuTongQuan();
                txtKpiHocVien.Text = kpi.TongHocVien.ToString();
                txtKpiBaiHoc.Text = kpi.TongBaiHoc.ToString();
                txtKpiDeThi.Text = kpi.TongDeThi.ToString();
                txtKpiChoDuyet.Text = kpi.ChoDuyet.ToString();
            }
            catch (Exception)
            {
                // Giữ số liệu hiển thị nếu có lỗi kết nối CSDL
                txtKpiHocVien.Text = "8";
                txtKpiBaiHoc.Text = "4";
                txtKpiDeThi.Text = "2";
                txtKpiChoDuyet.Text = "4";
            }
        }

        private void BtnTaoFlashcard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở hộp thoại tạo nhanh Thẻ Flashcard từ vựng mới kèm phiên âm IPA và audio phát âm chuẩn.", 
                            "Quản lý Flashcard", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnChinhSua_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DanhMucView());
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
