using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.GiangVien
{
    public partial class DanhMucView : Page
    {
        public DanhMucView()
        {
            InitializeComponent();
        }

        private void BtnCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                ResetCategoryStyles();
                if (TryFindResource("ActiveCategoryItemButton") is Style activeStyle)
                {
                    btn.Style = activeStyle;
                }
            }
        }

        private void ResetCategoryStyles()
        {
            if (TryFindResource("CategoryItemButton") is Style normalStyle)
            {
                btnCatAll.Style = normalStyle;
                btnCatIELTS.Style = normalStyle;
                btnCatTOEIC.Style = normalStyle;
                btnCatVSTEP.Style = normalStyle;
                btnCatGrammar.Style = normalStyle;
                btnCatVocab.Style = normalStyle;
            }
        }

        private void SetActiveFileItem(int index)
        {
            // Reset backgrounds
            itemFile1.Background = Brushes.White;
            itemFile2.Background = Brushes.White;
            itemFile3.Background = Brushes.White;
            itemFile4.Background = Brushes.White;
            itemFile5.Background = Brushes.White;

            // Reset "Đang xem" badges
            badgeViewing1.Visibility = Visibility.Collapsed;
            badgeViewing2.Visibility = Visibility.Collapsed;
            badgeViewing3.Visibility = Visibility.Collapsed;
            badgeViewing4.Visibility = Visibility.Collapsed;
            badgeViewing5.Visibility = Visibility.Collapsed;

            switch (index)
            {
                case 1:
                    itemFile1.Background = new SolidColorBrush(Color.FromRgb(240, 253, 244));
                    badgeViewing1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    itemFile2.Background = new SolidColorBrush(Color.FromRgb(239, 246, 255));
                    badgeViewing2.Visibility = Visibility.Visible;
                    break;
                case 3:
                    itemFile3.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                    badgeViewing3.Visibility = Visibility.Visible;
                    break;
                case 4:
                    itemFile4.Background = new SolidColorBrush(Color.FromRgb(240, 253, 244));
                    badgeViewing4.Visibility = Visibility.Visible;
                    break;
                case 5:
                    itemFile5.Background = new SolidColorBrush(Color.FromRgb(250, 245, 255));
                    badgeViewing5.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void FileItem1_Click(object sender, MouseButtonEventArgs e)
        {
            SetActiveFileItem(1);

            txtPreviewDocTitle.Text = "IELTS_Reading_Cam18_Test01.pdf";
            txtPreviewDocMeta.Text = "Định dạng: Adobe PDF • Dung lượng: 1.8 MB • Cập nhật: Hôm nay 10:30";
            txtSimulationModule.Text = "ACADEMIC READING MODULE — PRACTICE TEST #01";
            txtSimulationPassage.Text = "Reading Passage 1: You should spend about 20 minutes on Questions 1–13.";

            txtPassageTitle.Text = "The Secrets of Ancient Roman Concrete & Architectural Acoustics";
            txtPassageBody.Text = "For centuries, researchers have puzzled over how ancient Roman concrete structures such as the Colosseum and the Pantheon have remained structurally sound for over two millennia, often under harsh marine conditions...\n\nIn a groundbreaking 2023 study published by MIT and Harvard researchers, analytical electron microscopy revealed that lime clasts within the concrete matrix acted as an active calcium reservoir, sealing fractures autonomously.";
        }

        private void FileItem2_Click(object sender, MouseButtonEventArgs e)
        {
            SetActiveFileItem(2);

            txtPreviewDocTitle.Text = "TOEIC_Part7_Reading_Drills_2026.docx";
            txtPreviewDocMeta.Text = "Định dạng: Microsoft Word (.docx) • Dung lượng: 920 KB • ETS 2026 Format";
            txtSimulationModule.Text = "TOEIC PART 7 — DOUBLE PASSAGES DRILL #2026";
            txtSimulationPassage.Text = "Questions 153–155 refer to the following business email and contract notice.";

            txtPassageTitle.Text = "Email Inquiry & Business Contract Renewal Notice (Part 7 Double Passage)";
            txtPassageBody.Text = "To: logistics-ops@novatech.corp\nFrom: supply-chain@apexlogistics.com\nSubject: Formal Quotation for Freight Forwarding Services Q3\n\nDear Ms. Henderson,\nThank you for reaching out regarding the multimodal shipment of electronic components from Incheon to Hamburg. Below is our comprehensive fee breakdown including customs clearance and door-to-door insurance coverage...";
        }

        private void FileItem3_Click(object sender, MouseButtonEventArgs e)
        {
            SetActiveFileItem(3);

            txtPreviewDocTitle.Text = "VSTEP_B2_Full_Mock_Test_BộGDĐT.pdf";
            txtPreviewDocMeta.Text = "Định dạng: Adobe PDF • Dung lượng: 2.4 MB • Khung năng lực 6 bậc Việt Nam";
            txtSimulationModule.Text = "VSTEP B2 — BỘ ĐỀ THI CHUẨN ĐẠI HỌC QUỐC GIA";
            txtSimulationPassage.Text = "Reading Paper: 60 minutes for 4 Reading Passages (40 items).";

            txtPassageTitle.Text = "Đề thi Đánh giá Năng lực Tiếng Anh Bậc 4 (B2) — Phần thi Đọc hiểu & Viết";
            txtPassageBody.Text = "BÀI ĐỌC SỐ 1: Tác động của công nghệ số đến hành vi mua sắm của thế hệ Gen Z.\n\nThời gian làm bài: 60 phút gồm 4 bài đọc (40 câu hỏi trắc nghiệm). Thí sinh không được sử dụng từ điển hoặc tài liệu hỗ trợ. Toàn bộ câu hỏi đã được mã hóa vào hệ thống để AI tự động chấm.";
        }

        private void FileItem4_Click(object sender, MouseButtonEventArgs e)
        {
            SetActiveFileItem(4);

            txtPreviewDocTitle.Text = "Academic_Vocabulary_AWL_570Words.xlsx";
            txtPreviewDocMeta.Text = "Định dạng: Microsoft Excel (.xlsx) • Dung lượng: 620 KB • 570 Từ vựng AWL";
            txtSimulationModule.Text = "ACADEMIC WORD LIST (AWL) — FLASHCARD REVISION";
            txtSimulationPassage.Text = "570 Headwords categorised by frequency sublists (Sublist 1 to 10).";

            txtPassageTitle.Text = "Bảng dữ liệu trích xuất: Thẻ Flashcard Từ vựng Học thuật (Sublist 1–10)";
            txtPassageBody.Text = "Cột 1: Word (approach, benefit, concept, data, establish, factor, interpret, method, proceed, variable...)\nCột 2: IPA Pronunciation (/əˈprəʊtʃ/, /ˈbenɪfɪt/...)\nCột 3: Word Family (approachable, unapproachable)\nCột 4: Contextual Example & Audio file link.";
        }

        private void FileItem5_Click(object sender, MouseButtonEventArgs e)
        {
            SetActiveFileItem(5);

            txtPreviewDocTitle.Text = "IELTS_Section3_Audio_Transcript.mp3";
            txtPreviewDocMeta.Text = "Định dạng: MPEG Audio (.mp3) • Dung lượng: 8.4 MB • Thời lượng: 12 phút 40 giây";
            txtSimulationModule.Text = "IELTS LISTENING SECTION 3 — AUDIO TRANSCRIPT";
            txtSimulationPassage.Text = "University tutorial discussion between tutor and two students.";

            txtPassageTitle.Text = "Listening Section 3 Audio Transcript: Academic Discussion on Marine Biology";
            txtPassageBody.Text = "TUTOR: Good morning Sarah and Jack. Let's look at the field trip report you submitted on coral reef bleaching.\nSARAH: Yes, we found that temperature fluctuations in the summer months were significantly higher than historical averages...\nJACK: And the pH levels near the coastal runoff zones showed clear signs of localized acidification.";
        }

        private void BtnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở hộp thoại tải lên tệp: Chọn file PDF, DOCX, XLSX hoặc MP3 từ máy tính để phân tích và bóc tách tự động.", 
                            "Tải lên học liệu mới", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnNewFolder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở hộp thoại tạo mới Thư mục chuyên mục đề thi (VD: 'IELTS Cambridge 19', 'TOEIC Economy 2026').", 
                            "Thêm thư mục mới", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnDownloadCurrent_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Đang tải xuống tệp '{txtPreviewDocTitle.Text}' về máy tính...", "Tải tài liệu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPrintCurrent_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Đang kết nối máy in để in đề thi '{txtPreviewDocTitle.Text}' kèm đáp án và phiếu trả lời trắc nghiệm chuẩn...", "In ấn đề thi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAddToExam_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"✓ Đã đưa thành công toàn bộ câu hỏi và bài đọc từ '{txtPreviewDocTitle.Text}' vào Phòng Thi thử (Mock Test)!", "Nạp vào đề thi thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
