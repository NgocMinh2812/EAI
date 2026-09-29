using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.GiangVien
{
    public partial class AISinhDeView : Page
    {
        private int _pendingCount = 2;

        public AISinhDeView()
        {
            InitializeComponent();
        }

        private void TabAISinhDe_Click(object sender, RoutedEventArgs e)
        {
            pnlAISinhDe.Visibility = Visibility.Visible;
            pnlNhapTep.Visibility = Visibility.Collapsed;

            tabAISinhDe.Background = (Brush)FindResource("AdminDarkGreen");
            tabAISinhDe.Foreground = Brushes.White;

            tabNhapTep.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            tabNhapTep.Foreground = (Brush)FindResource("TextDark");
        }

        private void TabNhapTep_Click(object sender, RoutedEventArgs e)
        {
            pnlAISinhDe.Visibility = Visibility.Collapsed;
            pnlNhapTep.Visibility = Visibility.Visible;

            tabNhapTep.Background = (Brush)FindResource("AdminDarkGreen");
            tabNhapTep.Foreground = Brushes.White;

            tabAISinhDe.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            tabAISinhDe.Foreground = (Brush)FindResource("TextDark");
        }

        private void BtnSinhCauHoi_Click(object sender, RoutedEventArgs e)
        {
            string prompt = txtPrompt.Text.Trim();
            if (string.IsNullOrEmpty(prompt))
            {
                MessageBox.Show("Vui lòng nhập chủ đề hoặc yêu cầu sinh câu hỏi.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"AI đang phân tích và sinh bộ câu hỏi trắc nghiệm theo yêu cầu: \"{prompt}\"...\nĐã sinh thành công thêm câu hỏi mới và đưa vào danh sách kiểm duyệt!", 
                            "AI Sinh đề thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            _pendingCount++;
            txtPendingCount.Text = $"{_pendingCount} chờ duyệt";
        }

        private void BtnDuyetQ1_Click(object sender, RoutedEventArgs e)
        {
            cardQ1.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
            MessageBox.Show("✓ Câu hỏi 'Present Perfect' đã được phê duyệt và lưu vào Ngân hàng câu hỏi!", "Phê duyệt thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnTuChoiQ1_Click(object sender, RoutedEventArgs e)
        {
            cardQ1.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
        }

        private void BtnSuaQ1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở trình biên tập câu hỏi để chỉnh sửa câu hỏi, đáp án hoặc giải thích chi tiết.", "Chỉnh sửa câu hỏi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnDuyetQ2_Click(object sender, RoutedEventArgs e)
        {
            cardQ2.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
            MessageBox.Show("✓ Câu hỏi 'Từ vựng Môi trường' đã được phê duyệt và lưu vào Ngân hàng câu hỏi!", "Phê duyệt thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnTuChoiQ2_Click(object sender, RoutedEventArgs e)
        {
            cardQ2.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
        }

        private void BtnSuaQ2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở trình biên tập câu hỏi để chỉnh sửa câu hỏi, đáp án hoặc giải thích chi tiết.", "Chỉnh sửa câu hỏi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdatePendingCount()
        {
            if (_pendingCount <= 0)
            {
                txtPendingCount.Text = "Đã duyệt hết";
                txtPendingCount.Foreground = (Brush)FindResource("AdminMint");
            }
            else
            {
                txtPendingCount.Text = $"{_pendingCount} chờ duyệt";
            }
        }

        private void BtnChonFile_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hệ thống đã nhận diện tệp tải lên: 'IELTS_Reading_Cam18_Test1.xlsx'.\nĐang tự động bóc tách dữ liệu câu hỏi trắc nghiệm...", 
                            "Bóc tách tệp thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnDownloadTemplate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã tải xuống file mẫu chuẩn hóa gồm các cột: STT, Kỹ năng, Trình độ, Nội dung câu hỏi, Đáp án A, B, C, D, Đáp án đúng, Giải thích chi tiết.", 
                            "Tải file mẫu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLuuTepVaoDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🎉 Toàn bộ 30 câu hỏi đã được nạp thành công vào Cơ sở dữ liệu và sẵn sàng đưa vào Đề thi thử!", 
                            "Lưu dữ liệu thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
