using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.GiangVien
{
    public partial class NoiDungHocTapView : Page
    {
        public NoiDungHocTapView()
        {
            InitializeComponent();
        }

        private void ResetCardBackgrounds()
        {
            cardLesson1.Background = Brushes.White;
            cardLesson2.Background = Brushes.White;
            cardLesson3.Background = Brushes.White;
        }

        private void LessonCard1_Click(object sender, MouseButtonEventArgs e)
        {
            BtnViewLesson1_Click(sender, e);
        }

        private void BtnViewLesson1_Click(object sender, RoutedEventArgs e)
        {
            ResetCardBackgrounds();
            cardLesson1.Background = new SolidColorBrush(Color.FromRgb(240, 253, 244));

            txtReaderDocTitle.Text = "GiaoTrinh_PresentPerfect.pdf";
            txtReaderDocMeta.Text = "Đính kèm bài học BH-003 • 8 trang lý thuyết • File gốc định dạng PDF";
            txtChapterHeader.Text = "CHƯƠNG 3: THÌ HIỆN TẠI HOÀN THÀNH (THE PRESENT PERFECT TENSE)";
        }

        private void LessonCard2_Click(object sender, MouseButtonEventArgs e)
        {
            BtnViewLesson2_Click(sender, e);
        }

        private void BtnViewLesson2_Click(object sender, RoutedEventArgs e)
        {
            ResetCardBackgrounds();
            cardLesson2.Background = new SolidColorBrush(Color.FromRgb(245, 243, 255));

            txtReaderDocTitle.Text = "TuVung_AWL_Handbook.pdf";
            txtReaderDocMeta.Text = "Đính kèm bài học BH-002 • 14 trang học thuật • Kèm file Excel Flashcard & Audio MP3";
            txtChapterHeader.Text = "CHƯƠNG 2: ACADEMIC WORD LIST (AWL) — TỪ VỰNG HỌC THUẬT NHÓM 1 & 2";
        }

        private void LessonCard3_Click(object sender, MouseButtonEventArgs e)
        {
            BtnViewLesson3_Click(sender, e);
        }

        private void BtnViewLesson3_Click(object sender, RoutedEventArgs e)
        {
            ResetCardBackgrounds();
            cardLesson3.Background = new SolidColorBrush(Color.FromRgb(239, 246, 255));

            txtReaderDocTitle.Text = "LyThuyet_Conditionals_B2.pdf";
            txtReaderDocMeta.Text = "Đính kèm bài học BH-001 • 10 trang chuyên đề • Kèm phiếu bài tập Word (.docx)";
            txtChapterHeader.Text = "CHƯƠNG 1: CÂU ĐIỀU KIỆN & ĐẢO NGỮ CÂU ĐIỀU KIỆN (CONDITIONALS & INVERSION)";
        }

        private void BtnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Đang tải tệp tài liệu '{txtReaderDocTitle.Text}' về máy...", "Tải học liệu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnEditFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Mở trình soạn thảo nội dung bài học và tệp đính kèm '{txtReaderDocTitle.Text}'.", "Chỉnh sửa bài giảng", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPreviewStudent_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở giao diện phòng học mô phỏng của học viên để kiểm tra tính hiển thị của bài giảng và câu hỏi.", "Mô phỏng học viên", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnThemBaiHoc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở biểu mẫu thêm mới bài giảng: Nhập tên bài, chuyên mục, tải lên file giáo trình PDF và phiếu bài tập Word.", 
                            "Thêm bài học mới", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
