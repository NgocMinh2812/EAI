using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.GiangVien
{
    public partial class NganHangCauHoiView : Page
    {
        public NganHangCauHoiView()
        {
            InitializeComponent();
        }

        private void ResetExamBorders()
        {
            itemExam1.Background = Brushes.White;
            itemExam2.Background = Brushes.White;
            itemExam3.Background = Brushes.White;
            itemExam4.Background = Brushes.White;
        }

        private void ExamFile1_Click(object sender, MouseButtonEventArgs e)
        {
            ResetExamBorders();
            itemExam1.Background = new SolidColorBrush(Color.FromRgb(240, 253, 244));

            txtInspectExamTitle.Text = "Cambridge_IELTS_18_Test1_Reading.pdf";
            txtInspectExamMeta.Text = "Bộ đề nguồn: IELTS Academic • 40 câu hỏi đã trích xuất thành công • AI đã phân tích lời giải";
        }

        private void ExamFile2_Click(object sender, MouseButtonEventArgs e)
        {
            ResetExamBorders();
            itemExam2.Background = new SolidColorBrush(Color.FromRgb(239, 246, 255));

            txtInspectExamTitle.Text = "ETS_TOEIC_Part5_6_GrammarDrills.docx";
            txtInspectExamMeta.Text = "Bộ đề nguồn: TOEIC 2026 Format • 50 câu hỏi trắc nghiệm ngữ pháp & từ vựng hoàn thành câu";
        }

        private void ExamFile3_Click(object sender, MouseButtonEventArgs e)
        {
            ResetExamBorders();
            itemExam3.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));

            txtInspectExamTitle.Text = "DeThi_VSTEP_B2_ChuanBoGDDT_2026.pdf";
            txtInspectExamMeta.Text = "Bộ đề nguồn: VSTEP B2 Chuẩn Bộ GD&ĐT • 35 câu hỏi đọc hiểu 4 đoạn văn";
        }

        private void ExamFile4_Click(object sender, MouseButtonEventArgs e)
        {
            ResetExamBorders();
            itemExam4.Background = new SolidColorBrush(Color.FromRgb(239, 246, 255));

            txtInspectExamTitle.Text = "Business_English_Contract_Vocab.docx";
            txtInspectExamMeta.Text = "Bộ đề nguồn: Tiếng Anh Thương Mại • 25 câu hỏi từ vựng hợp đồng kinh tế và email";
        }

        private void BtnDownloadExam_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin($"Đang tải tệp đề thi gốc '{txtInspectExamTitle.Text}' về máy...", "Tải đề thi");
        }

        private void BtnAddAllToMockTest_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThanhCong($"✓ Đã đồng bộ tất cả câu hỏi từ '{txtInspectExamTitle.Text}' vào Phòng Thi thử trực tuyến!", "Đưa vào phòng thi");
        }

        private void BtnImportExamFile_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin(
                "Mở hộp thoại chọn tệp đề thi (.pdf, .docx): Hệ thống sẽ sử dụng AI bóc tách đề bài, 4 phương án, đáp án và lời giải thích tự động.", 
                "Nhập đề thi từ tệp");
        }

        private void BtnCreateManualQuestion_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin(
                "Mở biểu mẫu thêm câu hỏi thủ công: Nhập câu hỏi, gán nhãn kỹ năng và độ khó.", 
                "Thêm câu hỏi mới");
        }
    }
}
