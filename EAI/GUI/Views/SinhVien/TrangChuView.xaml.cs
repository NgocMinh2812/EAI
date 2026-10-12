using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using BUS;

namespace GUI.Views
{
    public partial class TrangChuView : Page
    {
        public TrangChuView()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (SessionHelper.CurrentUser != null)
            {
                txtWelcomeUser.Text = SessionHelper.CurrentUser.HoTen;
            }

            if (SessionHelper.CurrentHoSo != null)
            {
                txtTargetLevel.Text = SessionHelper.CurrentHoSo.HienThiMucTieu;
            }
            else
            {
                txtTargetLevel.Text = "Khởi đầu học tập";
            }

            bool daHoanThanhKhaoSat = (SessionHelper.CurrentUser != null && !SessionHelper.CurrentUser.LaLanDauDangNhap);

            if (daHoanThanhKhaoSat)
            {
                // Thành viên đã đăng ký / đã làm khảo sát: Không ép buộc làm khảo sát nữa
                txtBannerEmoji.Text = "✅";
                iconBannerBadge.Background = (Brush)new BrushConverter().ConvertFromString("#E0F2FE")!;
                bannerAssessment.Background = (Brush)new BrushConverter().ConvertFromString("#F0F9FF")!;
                bannerAssessment.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#BAE6FD")!;
                txtBannerTitle.Text = "Lộ trình năng lực cá nhân hóa của bạn";
                txtBannerTitle.Foreground = (Brush)new BrushConverter().ConvertFromString("#0369A1")!;
                string currentLevelDisplay = SessionHelper.CurrentHoSo?.HienThiTrinhDoDauVao ?? SessionHelper.CurrentHoSo?.HienThiMucTieu ?? "CEFR B1";
                txtBannerDesc.Text = $"Bạn đã hoàn thành đánh giá năng lực ({currentLevelDisplay}). Các bài học và đề xuất đã được cá nhân hóa sẵn sàng cho bạn!";
                txtBannerDesc.Foreground = (Brush)new BrushConverter().ConvertFromString("#0284C7")!;
                btnAssessmentAction.Background = (Brush)new BrushConverter().ConvertFromString("#0284C7")!;
                txtBannerBtnText.Text = "Kiểm tra lại trình độ 🔄";
            }
            else
            {
                // Người dùng chưa làm khảo sát
                txtBannerEmoji.Text = "🎯";
                iconBannerBadge.Background = (Brush)new BrushConverter().ConvertFromString("#DCFCE7")!;
                bannerAssessment.Background = (Brush)new BrushConverter().ConvertFromString("#F0FDF4")!;
                bannerAssessment.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#BBF7D0")!;
                txtBannerTitle.Text = "Khảo sát Đánh giá Năng lực Đầu vào (Placement Test)";
                txtBannerTitle.Foreground = (Brush)new BrushConverter().ConvertFromString("#166534")!;
                txtBannerDesc.Text = "Hãy chọn mức độ hiện tại và làm bài khảo sát ngắn để AI xác định cấp độ và đề xuất bài học tối ưu cho bạn.";
                txtBannerDesc.Foreground = (Brush)new BrushConverter().ConvertFromString("#15803D")!;
                btnAssessmentAction.Background = (Brush)new BrushConverter().ConvertFromString("#16A34A")!;
                txtBannerBtnText.Text = "Làm bài kiểm tra ngay ➔";
            }
        }

        private void AssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DanhGiaNangLucView(forceNewSurvey: true));
        }

        private void BtnHocTap_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new HocTapView());
        }

        private void BtnLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new LuyenTapView());
        }

        private void BtnLuyenTap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new LuyenTapView());
        }

        private void BtnThiThu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new ThiThuView());
        }

        private void BtnBaiVietAI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new BaiVietAIView());
        }

        private void BtnAIChat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void ItemBaiHoc1_Click(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new HocTapView(1));
        }

        private void ItemBaiHoc2_Click(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new HocTapView(2));
        }

        private void AIChatButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
