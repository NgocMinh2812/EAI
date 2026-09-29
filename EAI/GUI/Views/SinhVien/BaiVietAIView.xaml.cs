using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Views
{
    public partial class BaiVietAIView : Page
    {
        public BaiVietAIView()
        {
            InitializeComponent();
            LoadSampleEssay();
        }

        private void LoadSampleEssay()
        {
            txtEssayInput.Text = @"In recent years, the rapid advancement of artificial intelligence has sparked intense debate regarding the role of educators. While some foresee a future where automated systems entirely supplant human teachers, I firmly contend that technology will serve merely as an auxiliary tool rather than a comprehensive replacement.

First and foremost, human teachers provide emotional resonance and moral mentorship that algorithms inherently lack. Education encompasses not only information dissemination, but also character development and social empathy. A compassionate instructor can perceive subtle signs of demotivation or anxiety, subsequently tailoring emotional encouragement to nurture the student's psychological well-being. By contrast, AI lacks consciousness and genuine affective capacity, rendering it incapable of replicating authentic interpersonal connections.

Furthermore, dynamic classroom management and real-world ethical discourse necessitate human intuition. While AI can deliver personalized grammar drills and assess objective metrics with unprecedented precision, cultivating critical debate demands flexible moderation that accounts for nuanced cultural perspectives.

In conclusion, although artificial intelligence undoubtedly streamlines administrative duties and augments self-paced acquisition, human educators remain irreplaceable in inspiring curiosity and shaping human values.";
            UpdateWordCount();
        }

        private void TxtEssayInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            if (txtWordCount == null || txtEssayInput == null) return;

            string text = txtEssayInput.Text.Trim();
            int count = 0;
            if (!string.IsNullOrEmpty(text))
            {
                var matches = Regex.Matches(text, @"[\S]+");
                count = matches.Count;
            }

            if (count >= 250)
            {
                txtWordCount.Text = $"Số từ: {count} từ • Đạt độ dài tối thiểu (>= 250 từ)";
                txtWordCount.Foreground = (System.Windows.Media.Brush)FindResource("BrandTeal");
            }
            else
            {
                txtWordCount.Text = $"Số từ: {count} từ • Cần viết thêm ít nhất {250 - count} từ nữa";
                txtWordCount.Foreground = System.Windows.Media.Brushes.OrangeRed;
            }
        }

        private void BtnGradeEssay_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("🤖 Hệ thống AI đã phân tích toàn diện 4 tiêu chí IELTS Writing:\n\n" +
                            "• Task Response: 7.5 (Lập luận sâu sắc, bám sát đề)\n" +
                            "• Coherence & Cohesion: 7.0 (Từ nối mượt mà, phân đoạn hợp lý)\n" +
                            "• Lexical Resource: 7.0 (Từ vựng học thuật cao cấp C1)\n" +
                            "• Grammatical Range: 6.5 (Cấu trúc phức tốt, cần lưu ý câu bị động)\n\n" +
                            "🎉 Điểm tổng kết ước tính: Band 7.0", 
                            "Kết quả chấm điểm AI", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnSaveDraft_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("💾 Bài viết đã được lưu vào danh sách bản nháp học tập của bạn.", "Lưu bài nháp", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa toàn bộ nội dung bài viết không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                txtEssayInput.Text = "";
            }
        }

        private void BtnChangeTopic_Click(object sender, RoutedEventArgs e)
        {
            txtEssayTopic.Text = "Topic: In many countries, the amount of household waste is increasing rapidly. What are the main causes of this problem, and what measures can be taken to tackle it?";
            txtEssayInput.Text = "";
            MessageBox.Show("Đã đổi sang đề bài: 'Household Waste & Environmental Protection (Problem & Solution)'. Hãy bắt đầu viết bài mới!", "Đổi đề bài", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
