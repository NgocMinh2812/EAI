using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using BUS;
using DTO;

namespace GUI.Views
{
    public partial class ThiThuView : Page
    {
        // =========================================================================
        // VIEW MODELS FOR DATA-BINDING
        // =========================================================================
        public class ExamItemViewModel
        {
            public int MaBaiTapDeThi { get; set; }
            public string TenBaiTapDeThi { get; set; } = string.Empty;
            public string LoaiHinh { get; set; } = "ThiThu";
            public string? MoTa { get; set; }
            public int ThoiGianLamBaiPhut { get; set; } = 30;
            public int SoLuongCauHoi { get; set; } = 10;
            public string MaTrinhDo { get; set; } = "B2";

            public string FormatIcon => LoaiHinh == "ThiThu" ? (TenBaiTapDeThi.Contains("IELTS") ? "🇬🇧" : "🏢") : "📝";
            public Brush FormatBadgeBg => LoaiHinh == "ThiThu" 
                ? (TenBaiTapDeThi.Contains("IELTS") ? new SolidColorBrush(Color.FromRgb(254, 242, 242)) : new SolidColorBrush(Color.FromRgb(239, 246, 255)))
                : new SolidColorBrush(Color.FromRgb(204, 251, 241));

            public string FormatTag => TenBaiTapDeThi.Contains("IELTS") ? "IELTS ACADEMIC" : (TenBaiTapDeThi.Contains("TOEIC") ? "TOEIC FORMAT" : "NGỮ PHÁP CHUYÊN SÂU");
            public Brush FormatTagBg => TenBaiTapDeThi.Contains("IELTS") 
                ? new SolidColorBrush(Color.FromRgb(254, 226, 226)) 
                : (TenBaiTapDeThi.Contains("TOEIC") ? new SolidColorBrush(Color.FromRgb(219, 234, 254)) : new SolidColorBrush(Color.FromRgb(204, 251, 241)));
            public Brush FormatTagFg => TenBaiTapDeThi.Contains("IELTS") 
                ? new SolidColorBrush(Color.FromRgb(185, 28, 28)) 
                : (TenBaiTapDeThi.Contains("TOEIC") ? new SolidColorBrush(Color.FromRgb(29, 78, 216)) : new SolidColorBrush(Color.FromRgb(15, 118, 110)));

            public string LevelTag => $"Chuẩn {MaTrinhDo}";
            public string ExamSubtitle => $"⏱️ {ThoiGianLamBaiPhut} phút • {SoLuongCauHoi} câu hỏi • {MoTa ?? "Bộ đề kiểm tra chuẩn hóa năng lực"}";
        }

        public class ReviewQuestionViewModel
        {
            public int ThuTuCauHoi { get; set; }
            public string QuestionNumberText => $"Câu {ThuTuCauHoi}";
            public string KyNangTag => $"KỸ NĂNG: {KyNang?.ToUpper() ?? "CHUNG"}";
            public string? KyNang { get; set; }
            public string? DoanVanDoc { get; set; }
            public Visibility PassageVisibility => string.IsNullOrWhiteSpace(DoanVanDoc) ? Visibility.Collapsed : Visibility.Visible;
            public string NoiDungCauHoi { get; set; } = string.Empty;

            public string StatusBadgeText { get; set; } = "⚪ CHƯA LÀM";
            public Brush StatusBadgeBg { get; set; } = Brushes.LightGray;
            public Brush StatusBadgeFg { get; set; } = Brushes.Black;

            public string StudentChoiceText { get; set; } = "(Chưa chọn đáp án)";
            public Brush StudentChoiceBg { get; set; } = new SolidColorBrush(Color.FromRgb(248, 250, 252));
            public Brush StudentChoiceBorder { get; set; } = new SolidColorBrush(Color.FromRgb(226, 232, 240));
            public Brush StudentChoiceFg { get; set; } = new SolidColorBrush(Color.FromRgb(100, 116, 139));

            public string CorrectChoiceText { get; set; } = string.Empty;
            public string ExplanationText { get; set; } = "Chưa có lời giải chi tiết cho câu hỏi này.";
        }

        // =========================================================================
        // STATE FIELDS
        // =========================================================================
        private List<BaiTapDeThiDTO> _rawExams = new List<BaiTapDeThiDTO>();
        private BaiTapDeThiDTO? _currentExam;
        private List<CauHoiKhaoSatDTO> _questions = new List<CauHoiKhaoSatDTO>();
        private int _currentIndex = 0;

        // Lưu câu trả lời của thí sinh: Key = MaCauHoi, Value = MaLuaChonDaChon
        private Dictionary<int, int> _userAnswers = new Dictionary<int, int>();
        // Key = MaCauHoi, Value = 0, 1, 2, 3 (Index A, B, C, D)
        private Dictionary<int, int> _userAnswerIndices = new Dictionary<int, int>();

        // Timer & Time tracking
        private DispatcherTimer? _examTimer;
        private int _secondsRemaining = 1500; // 25 phút
        private int _totalExamSeconds = 1500;
        private DateTime _examStartTime;
        private bool _isSubmitting = false;

        // Flagged Questions (TASK-206)
        private readonly HashSet<int> _flaggedQuestions = new HashSet<int>();

        // Navigation Matrix Buttons cache
        private readonly List<Button> _navButtons = new List<Button>();

        public ThiThuView()
        {
            InitializeComponent();
            LoadExamList();
            UpdateUserExamStats();
        }

        // =========================================================================
        // VIEW 1: LOAD & FILTER EXAM LIST FROM DATABASE
        // =========================================================================
        private void LoadExamList(string? filterKeyword = null)
        {
            try
            {
                _rawExams = BaiTapDeThiBUS.Instance.LayDanhSachDeThi();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ThiThuView] Lỗi lấy danh sách đề: {ex.Message}");
                _rawExams = new List<BaiTapDeThiDTO>();
            }

            var displayList = _rawExams;
            if (!string.IsNullOrWhiteSpace(filterKeyword) && filterKeyword != "All")
            {
                displayList = _rawExams.Where(x => 
                    x.TenBaiTapDeThi.Contains(filterKeyword, StringComparison.OrdinalIgnoreCase) ||
                    x.LoaiHinh.Contains(filterKeyword, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(x.MaCapDo) && x.MaCapDo.Contains(filterKeyword, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            var viewModels = displayList.Select(e => new ExamItemViewModel
            {
                MaBaiTapDeThi = e.MaBaiTapDeThi,
                TenBaiTapDeThi = e.TenBaiTapDeThi,
                LoaiHinh = e.LoaiHinh,
                MoTa = e.MoTa,
                ThoiGianLamBaiPhut = e.ThoiGianLamBaiPhut > 0 ? e.ThoiGianLamBaiPhut : 25,
                SoLuongCauHoi = e.SoLuongCauHoi > 0 ? e.SoLuongCauHoi : 10,
                MaTrinhDo = !string.IsNullOrEmpty(e.MaCapDo) ? e.MaCapDo : "B2"
            }).ToList();

            icDanhSachDeThi.ItemsSource = viewModels;
        }

        private void UpdateUserExamStats()
        {
            try
            {
                var history = BaiTapDeThiBUS.Instance.LayLichSuThi(top: 10);
                if (history != null && history.Count > 0)
                {
                    txtTotalExamsTaken.Text = $"🏆 Đã thi: {history.Count} bài";
                    decimal maxScore = history.Max(h => h.DiemSo);
                    txtHighestScore.Text = $"🎯 Điểm cao nhất: {maxScore:F1} / 10";
                }
                else
                {
                    txtTotalExamsTaken.Text = "🏆 Đã thi: 0 bài";
                    txtHighestScore.Text = "🎯 Điểm cao nhất: --";
                }
            }
            catch { }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            // Reset tab styles
            var brandTeal = (SolidColorBrush)FindResource("BrandTeal");
            var brandTealLight = (SolidColorBrush)FindResource("BrandTealLight");
            var textGray = (SolidColorBrush)FindResource("TextGray");
            var borderLight = (SolidColorBrush)FindResource("BorderLight");

            btnFilterAll.Background = Brushes.White; btnFilterAll.Foreground = textGray; btnFilterAll.BorderThickness = new Thickness(1);
            btnFilterIelts.Background = Brushes.White; btnFilterIelts.Foreground = textGray; btnFilterIelts.BorderThickness = new Thickness(1);
            btnFilterToeic.Background = Brushes.White; btnFilterToeic.Foreground = textGray; btnFilterToeic.BorderThickness = new Thickness(1);
            btnFilterGrammar.Background = Brushes.White; btnFilterGrammar.Foreground = textGray; btnFilterGrammar.BorderThickness = new Thickness(1);

            btn.Background = brandTealLight;
            btn.Foreground = brandTeal;
            btn.BorderThickness = new Thickness(0);

            if (btn == btnFilterAll) LoadExamList("All");
            else if (btn == btnFilterIelts) LoadExamList("IELTS");
            else if (btn == btnFilterToeic) LoadExamList("TOEIC");
            else if (btn == btnFilterGrammar) LoadExamList("Ngữ pháp");
        }

        private void BtnQuickTest_Click(object sender, RoutedEventArgs e)
        {
            // Bắt đầu làm ngay đề thi đầu tiên trong danh sách
            if (_rawExams != null && _rawExams.Count > 0)
            {
                StartExamWithId(_rawExams[0].MaBaiTapDeThi);
            }
            else
            {
                MessageBox.Show("Hiện chưa có đề thi sẵn sàng trong cơ sở dữ liệu.", "Thông báo");
            }
        }

        // =========================================================================
        // VIEW 2: TASK-206 - ĐỘNG CƠ THI THỬ & ĐỒNG HỒ ĐẾM NGƯỢC
        // =========================================================================
        private void BtnStartExam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null && int.TryParse(btn.Tag.ToString(), out int maDe))
            {
                StartExamWithId(maDe);
            }
        }

        private void StartExamWithId(int maDeThi)
        {
            _currentExam = _rawExams.FirstOrDefault(x => x.MaBaiTapDeThi == maDeThi) 
                           ?? BaiTapDeThiBUS.Instance.LayChiTietDeThi(maDeThi);

            if (_currentExam == null)
            {
                MessageBox.Show("Không tìm thấy thông tin bài thi đã chọn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Nạp danh sách câu hỏi của đề từ Database
            try
            {
                _questions = BaiTapDeThiBUS.Instance.LayDanhSachCauHoiCuaDeThi(maDeThi);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ThiThuView] Lỗi nạp câu hỏi: {ex.Message}");
                _questions = new List<CauHoiKhaoSatDTO>();
            }

            if (_questions == null || _questions.Count == 0)
            {
                MessageBox.Show("Bài thi này hiện chưa được gán câu hỏi trong hệ thống. Vui lòng thử đề thi khác.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Reset trạng thái làm bài (TASK-206)
            _userAnswers.Clear();
            _userAnswerIndices.Clear();
            _flaggedQuestions.Clear();
            _currentIndex = 0;
            _isSubmitting = false;
            _examStartTime = DateTime.Now;

            int examMinutes = _currentExam.ThoiGianLamBaiPhut > 0 ? _currentExam.ThoiGianLamBaiPhut : 25;
            _totalExamSeconds = examMinutes * 60;
            _secondsRemaining = _totalExamSeconds;

            // Cập nhật Header
            txtExamTitle.Text = $"Đề thi: {_currentExam.TenBaiTapDeThi}";
            txtCountdownTimer.Text = $"{_secondsRemaining / 60:D2}:{_secondsRemaining % 60:D2}";
            txtCountdownTimer.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));

            // Sinh ma trận câu hỏi (Question Navigation Grid)
            BuildQuestionNavMatrix();

            // Chuyển màn hình sang phòng thi
            pnlDanhSachDeThi.Visibility = Visibility.Collapsed;
            pnlXemLaiBaiThi.Visibility = Visibility.Collapsed;
            pnlLamBaiThi.Visibility = Visibility.Visible;
            txtHeaderBreadcrumb.Text = $"Đang làm bài: {_currentExam.TenBaiTapDeThi}";

            // Khởi chạy đồng hồ đếm ngược
            StartCountdownTimer();

            // Nạp câu hỏi đầu tiên
            LoadQuestion(_currentIndex);
        }

        private void StartCountdownTimer()
        {
            _examTimer?.Stop();
            _examTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _examTimer.Tick += (s, e) =>
            {
                if (_secondsRemaining > 0)
                {
                    _secondsRemaining--;
                    int m = _secondsRemaining / 60;
                    int sec = _secondsRemaining % 60;
                    txtCountdownTimer.Text = $"{m:D2}:{sec:D2}";

                    // Đổi màu cảnh báo khi còn dưới 3 phút
                    if (_secondsRemaining <= 180)
                    {
                        txtCountdownTimer.Foreground = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                    }
                }
                else
                {
                    // TASK-209: Tự động nộp bài khi hết giờ
                    _examTimer.Stop();
                    MessageBox.Show("⏱️ ĐÃ HẾT THỜI GIAN LÀM BÀI!\nHệ thống sẽ tự động thu bài và chấm điểm ngay bây giờ.", 
                                    "Hết giờ làm bài", MessageBoxButton.OK, MessageBoxImage.Information);
                    SubmitExam(tuDongNopHetGio: true);
                }
            };
            _examTimer.Start();
        }

        private void BuildQuestionNavMatrix()
        {
            pnlQuestionNavButtons.Children.Clear();
            _navButtons.Clear();

            for (int i = 0; i < _questions.Count; i++)
            {
                int qIndex = i;
                var btn = new Button
                {
                    Content = (i + 1).ToString(),
                    Width = 42,
                    Height = 42,
                    Margin = new Thickness(0, 0, 8, 8),
                    Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)),
                    Foreground = (SolidColorBrush)FindResource("TextDark"),
                    BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1),
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 13,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = qIndex
                };

                btn.Click += (s, ev) =>
                {
                    LoadQuestion(qIndex);
                };

                _navButtons.Add(btn);
                pnlQuestionNavButtons.Children.Add(btn);
            }

            UpdateNavProgress();
        }

        private void UpdateNavProgress()
        {
            int answeredCount = _userAnswers.Count(x => x.Value > 0);
            txtNavProgress.Text = $"Đã làm: {answeredCount} / {_questions.Count} câu";

            for (int i = 0; i < _navButtons.Count; i++)
            {
                var btn = _navButtons[i];
                var q = _questions[i];
                bool isAnswered = _userAnswers.ContainsKey(q.MaCauHoi) && _userAnswers[q.MaCauHoi] > 0;
                bool isFlagged = _flaggedQuestions.Contains(q.MaCauHoi);
                bool isCurrent = (i == _currentIndex);

                if (isCurrent)
                {
                    btn.BorderBrush = (SolidColorBrush)FindResource("BrandTeal");
                    btn.BorderThickness = new Thickness(2.5);
                }
                else
                {
                    btn.BorderBrush = isFlagged ? new SolidColorBrush(Color.FromRgb(245, 158, 11)) : (SolidColorBrush)FindResource("BorderLight");
                    btn.BorderThickness = isFlagged ? new Thickness(2) : new Thickness(1);
                }

                if (isFlagged)
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(254, 243, 199));
                    btn.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));
                    btn.Content = $"{i + 1}🚩";
                }
                else if (isAnswered)
                {
                    btn.Background = (SolidColorBrush)FindResource("BrandTeal");
                    btn.Foreground = Brushes.White;
                    btn.Content = (i + 1).ToString();
                }
                else
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
                    btn.Foreground = (SolidColorBrush)FindResource("TextDark");
                    btn.Content = (i + 1).ToString();
                }
            }
        }

        private void LoadQuestion(int index)
        {
            if (index < 0 || index >= _questions.Count) return;

            _currentIndex = index;
            var q = _questions[index];

            txtExamProgressCount.Text = $"Câu {index + 1} / {_questions.Count}";
            txtQuestionSkill.Text = $"KỸ NĂNG: {q.KyNang?.ToUpper() ?? "CHUNG"}";
            txtQuestionLevel.Text = $"TRÌNH ĐỘ {q.MaTrinhDo ?? "B2"}";
            txtExamQuestionPrompt.Text = $"Câu {index + 1}: {q.NoiDungCauHoi}";

            // Cập nhật trạng thái gắn cờ (TASK-206)
            if (_flaggedQuestions.Contains(q.MaCauHoi))
            {
                btnFlagQuestion.Content = "🚩 Đã gắn cờ";
                btnFlagQuestion.Background = new SolidColorBrush(Color.FromRgb(254, 215, 170));
                btnFlagQuestion.Foreground = new SolidColorBrush(Color.FromRgb(194, 65, 12));
            }
            else
            {
                btnFlagQuestion.Content = "🚩 Đặt cờ xem lại";
                btnFlagQuestion.Background = new SolidColorBrush(Color.FromRgb(254, 243, 199));
                btnFlagQuestion.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));
            }

            // Hiển thị đoạn văn nếu có
            if (!string.IsNullOrWhiteSpace(q.DoanVanDoc))
            {
                pnlExamPassage.Visibility = Visibility.Visible;
                txtExamPassage.Text = q.DoanVanDoc;
            }
            else
            {
                pnlExamPassage.Visibility = Visibility.Collapsed;
            }

            // Gán 4 phương án trắc nghiệm
            var options = q.DanhSachLuaChon;
            txtOptA.Text = options.Count > 0 ? $"A. {options[0].NoiDungLuaChon}" : "A.";
            txtOptB.Text = options.Count > 1 ? $"B. {options[1].NoiDungLuaChon}" : "B.";
            txtOptC.Text = options.Count > 2 ? $"C. {options[2].NoiDungLuaChon}" : "C.";
            txtOptD.Text = options.Count > 3 ? $"D. {options[3].NoiDungLuaChon}" : "D.";

            // Tạm hủy event để tránh trigger ngược khi set IsChecked
            rbOptA.Checked -= ExamOption_Checked;
            rbOptB.Checked -= ExamOption_Checked;
            rbOptC.Checked -= ExamOption_Checked;
            rbOptD.Checked -= ExamOption_Checked;

            int selectedOptIdx = _userAnswerIndices.ContainsKey(q.MaCauHoi) ? _userAnswerIndices[q.MaCauHoi] : -1;
            rbOptA.IsChecked = (selectedOptIdx == 0);
            rbOptB.IsChecked = (selectedOptIdx == 1);
            rbOptC.IsChecked = (selectedOptIdx == 2);
            rbOptD.IsChecked = (selectedOptIdx == 3);

            rbOptA.Checked += ExamOption_Checked;
            rbOptB.Checked += ExamOption_Checked;
            rbOptC.Checked += ExamOption_Checked;
            rbOptD.Checked += ExamOption_Checked;

            btnPrevQuestion.IsEnabled = (index > 0);
            btnNextQuestion.Content = (index == _questions.Count - 1) ? "Xem lại câu 1 ➔" : "Câu tiếp theo ➔";

            UpdateNavProgress();
        }

        private void BtnFlagQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex >= _questions.Count) return;
            var q = _questions[_currentIndex];

            if (_flaggedQuestions.Contains(q.MaCauHoi))
            {
                _flaggedQuestions.Remove(q.MaCauHoi);
                btnFlagQuestion.Content = "🚩 Đặt cờ xem lại";
                btnFlagQuestion.Background = new SolidColorBrush(Color.FromRgb(254, 243, 199));
                btnFlagQuestion.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));
            }
            else
            {
                _flaggedQuestions.Add(q.MaCauHoi);
                btnFlagQuestion.Content = "🚩 Đã gắn cờ";
                btnFlagQuestion.Background = new SolidColorBrush(Color.FromRgb(254, 215, 170));
                btnFlagQuestion.Foreground = new SolidColorBrush(Color.FromRgb(194, 65, 12));
            }

            UpdateNavProgress();
        }

        // TASK-209: KIỂM THỬ TỰ ĐỘNG NỘP KHI HẾT GIỜ (COUNTDOWN EXPIRY)
        private void BtnTestTimeout_Click(object sender, RoutedEventArgs e)
        {
            _secondsRemaining = 5;
            txtCountdownTimer.Text = "00:05";
            txtCountdownTimer.Foreground = new SolidColorBrush(Color.FromRgb(239, 68, 68));
            MessageBox.Show("⚡ Đã kích hoạt kịch bản kiểm thử (TASK-209):\nThời gian làm bài còn 5 giây. Hệ thống sẽ tự động thu bài và chấm điểm khi đếm về 00:00!", 
                            "Kiểm thử tự động nộp bài", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExamOption_Checked(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex >= _questions.Count) return;
            var q = _questions[_currentIndex];

            int optIdx = -1;
            if (rbOptA.IsChecked == true) optIdx = 0;
            else if (rbOptB.IsChecked == true) optIdx = 1;
            else if (rbOptC.IsChecked == true) optIdx = 2;
            else if (rbOptD.IsChecked == true) optIdx = 3;

            if (optIdx >= 0 && optIdx < q.DanhSachLuaChon.Count)
            {
                _userAnswerIndices[q.MaCauHoi] = optIdx;
                _userAnswers[q.MaCauHoi] = q.DanhSachLuaChon[optIdx].MaLuaChon;
            }

            UpdateNavProgress();
        }

        private void BtnPrevQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex > 0)
            {
                LoadQuestion(_currentIndex - 1);
            }
        }

        private void BtnNextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < _questions.Count - 1)
            {
                LoadQuestion(_currentIndex + 1);
            }
            else
            {
                LoadQuestion(0);
            }
        }

        // =========================================================================
        // TASK-207: CHẤM ĐIỂM TỰ ĐỘNG & LƯU LỊCH SỬ KHẢO THÍ VÀO DATABASE
        // =========================================================================
        private void BtnSubmitExam_Click(object sender, RoutedEventArgs e)
        {
            int unanswered = _questions.Count - _userAnswers.Count;
            string confirmMsg = unanswered > 0
                ? $"Bạn còn {unanswered} câu hỏi chưa trả lời.\nBạn có chắc chắn muốn nộp bài thi ngay bây giờ?"
                : "Bạn đã hoàn thành tất cả các câu hỏi.\nBạn có muốn nộp bài thi để hệ thống chấm điểm?";

            var res = MessageBox.Show(confirmMsg, "Xác nhận nộp bài thi", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                SubmitExam(tuDongNopHetGio: false);
            }
        }

        private void SubmitExam(bool tuDongNopHetGio)
        {
            if (_isSubmitting || _currentExam == null) return;
            _isSubmitting = true;
            _examTimer?.Stop();

            int timeSpentSeconds = Math.Max(5, _totalExamSeconds - _secondsRemaining);

            // Chấm điểm và lưu kết quả thông qua BUS & DAL vào LuotLamBai, ChiTietTraLoi, LichSuHocTap
            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                _currentExam.MaBaiTapDeThi,
                _currentExam.LoaiHinh ?? "ThiThu",
                _examStartTime,
                timeSpentSeconds,
                _questions,
                _userAnswers
            );

            // Cập nhật thống kê người dùng trên header
            UpdateUserExamStats();

            // TASK-208: Mở màn hình xem lại chi tiết bài thi (Review Mode)
            HienThiReviewMode(result.MaLuotLamBai, result.DiemSo, result.SoCauDung, result.TongSoCau, timeSpentSeconds);
        }

        // =========================================================================
        // TASK-208: MÀN HÌNH XEM LẠI CHI TIẾT BÀI THI (REVIEW MODE)
        // =========================================================================
        private void HienThiReviewMode(int maLuotLamBai, decimal score, int correct, int total, int timeSpent)
        {
            pnlLamBaiThi.Visibility = Visibility.Collapsed;
            pnlDanhSachDeThi.Visibility = Visibility.Collapsed;
            pnlXemLaiBaiThi.Visibility = Visibility.Visible;
            txtHeaderBreadcrumb.Text = $"Xem lại chi tiết bài thi (Review Mode)";

            // 1. Điền thông tin bảng điểm
            txtReviewScore.Text = $"{score:F1}";
            txtReviewTestTitle.Text = _currentExam?.TenBaiTapDeThi ?? "Bài thi thử";
            txtReviewCorrectCount.Text = $"✅ Đúng: {correct}/{total} câu";
            txtReviewWrongCount.Text = $"❌ Sai: {total - correct}/{total} câu";
            txtReviewTimeSpent.Text = $"⏱️ Thời gian làm: {timeSpent / 60:D2}:{timeSpent % 60:D2}";

            int percentage = total > 0 ? (correct * 100) / total : 0;
            if (percentage >= 80)
            {
                txtReviewVerdict.Text = "KẾT QUẢ: XUẤT SẮC (BAND C1/B2+)";
                txtReviewAiFeedback.Text = "AI Đánh giá: Bạn có phản xạ xuất sắc và vốn từ vựng học thuật rất tốt. Khả năng đọc hiểu chi tiết và suy luận mạch lạc giúp bạn đạt điểm gần như tuyệt đối!";
            }
            else if (percentage >= 50)
            {
                txtReviewVerdict.Text = "KẾT QUẢ: ĐẠT CHUẨN (BAND B1/B2)";
                txtReviewAiFeedback.Text = "AI Đánh giá: Bạn nắm vững các ngữ cảnh thông thường. Hãy chú ý đọc kỹ các bẫy liên từ và từ đồng nghĩa trong đoạn văn để nâng cao thêm điểm số.";
            }
            else
            {
                txtReviewVerdict.Text = "KẾT QUẢ: CẦN CỐ GẮNG THÊM";
                txtReviewAiFeedback.Text = "AI Đánh giá: Bạn cần ôn luyện thêm phần Ngữ pháp cấu trúc câu và từ vựng chuyên ngành. Hãy xem kỹ phần giải thích chi tiết bên dưới để rút kinh nghiệm.";
            }

            // 2. Nạp chi tiết câu hỏi và đối chiếu đáp án từ DB
            LuotLamBaiDTO? chiTietLuot = null;
            if (maLuotLamBai > 0)
            {
                chiTietLuot = BaiTapDeThiBUS.Instance.LayChiTietKetQuaLamBai(maLuotLamBai);
            }

            var reviewItems = new List<ReviewQuestionViewModel>();

            if (chiTietLuot != null && chiTietLuot.DanhSachChiTiet.Count > 0)
            {
                foreach (var ct in chiTietLuot.DanhSachChiTiet)
                {
                    bool isCorrect = ct.IsCorrect;
                    bool isAnswered = ct.IsAnswered;

                    var vm = new ReviewQuestionViewModel
                    {
                        ThuTuCauHoi = ct.ThuTuCauHoi,
                        KyNang = ct.KyNang,
                        DoanVanDoc = ct.DoanVanDoc,
                        NoiDungCauHoi = ct.NoiDungCauHoi,
                        CorrectChoiceText = $"{ct.KyHieuDapAnDung}. {ct.NoiDungDapAnDung}",
                        ExplanationText = !string.IsNullOrWhiteSpace(ct.GiaiThichDapAn) ? ct.GiaiThichDapAn : "Đáp án chuẩn theo ngân hàng câu hỏi đề thi.",
                        StatusBadgeText = isCorrect ? "✅ ĐÚNG (+1.0 đ)" : (isAnswered ? "❌ SAI (0.0 đ)" : "⚪ CHƯA LÀM (0.0 đ)"),
                        StatusBadgeBg = isCorrect ? new SolidColorBrush(Color.FromRgb(209, 250, 229)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(254, 226, 226)) : new SolidColorBrush(Color.FromRgb(241, 245, 249))),
                        StatusBadgeFg = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(239, 68, 68)) : new SolidColorBrush(Color.FromRgb(100, 116, 139))),
                        StudentChoiceText = isAnswered ? $"{ct.KyHieuDaChon}. {ct.NoiDungDaChon}" : "(Thí sinh bỏ trống)",
                        StudentChoiceBg = isCorrect ? new SolidColorBrush(Color.FromRgb(236, 253, 245)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(254, 242, 242)) : new SolidColorBrush(Color.FromRgb(248, 250, 252))),
                        StudentChoiceBorder = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(239, 68, 68)) : new SolidColorBrush(Color.FromRgb(226, 232, 240))),
                        StudentChoiceFg = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(239, 68, 68)) : new SolidColorBrush(Color.FromRgb(100, 116, 139)))
                    };
                    reviewItems.Add(vm);
                }
            }
            else
            {
                // Fallback từ bộ nhớ hiện tại nếu chưa lưu kịp
                for (int i = 0; i < _questions.Count; i++)
                {
                    var q = _questions[i];
                    var correctOpt = q.DanhSachLuaChon.FirstOrDefault(o => o.LaDapAnDung);
                    int? selectedOptId = _userAnswers.ContainsKey(q.MaCauHoi) ? _userAnswers[q.MaCauHoi] : null;
                    var selectedOpt = q.DanhSachLuaChon.FirstOrDefault(o => o.MaLuaChon == selectedOptId);

                    bool isCorrect = selectedOpt != null && selectedOpt.LaDapAnDung;
                    bool isAnswered = selectedOpt != null;

                    reviewItems.Add(new ReviewQuestionViewModel
                    {
                        ThuTuCauHoi = i + 1,
                        KyNang = q.KyNang,
                        DoanVanDoc = q.DoanVanDoc,
                        NoiDungCauHoi = q.NoiDungCauHoi,
                        CorrectChoiceText = correctOpt != null ? $"{correctOpt.KyHieuLuaChon}. {correctOpt.NoiDungLuaChon}" : "Đang cập nhật",
                        ExplanationText = q.GiaiThichDapAn ?? "Không có giải thích chi tiết.",
                        StatusBadgeText = isCorrect ? "✅ ĐÚNG (+1.0 đ)" : (isAnswered ? "❌ SAI (0.0 đ)" : "⚪ CHƯA LÀM"),
                        StatusBadgeBg = isCorrect ? new SolidColorBrush(Color.FromRgb(209, 250, 229)) : new SolidColorBrush(Color.FromRgb(254, 226, 226)),
                        StatusBadgeFg = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                        StudentChoiceText = isAnswered ? $"{selectedOpt!.KyHieuLuaChon}. {selectedOpt.NoiDungLuaChon}" : "(Thí sinh bỏ trống)",
                        StudentChoiceBg = isCorrect ? new SolidColorBrush(Color.FromRgb(236, 253, 245)) : new SolidColorBrush(Color.FromRgb(254, 242, 242)),
                        StudentChoiceBorder = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                        StudentChoiceFg = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : new SolidColorBrush(Color.FromRgb(239, 68, 68))
                    });
                }
            }

            icReviewQuestions.ItemsSource = reviewItems;
        }

        private void BtnRetakeExam_Click(object sender, RoutedEventArgs e)
        {
            if (_currentExam != null)
            {
                StartExamWithId(_currentExam.MaBaiTapDeThi);
            }
        }

        private void BtnBackToExamList_Click(object sender, RoutedEventArgs e)
        {
            _examTimer?.Stop();
            pnlLamBaiThi.Visibility = Visibility.Collapsed;
            pnlXemLaiBaiThi.Visibility = Visibility.Collapsed;
            pnlDanhSachDeThi.Visibility = Visibility.Visible;
            txtHeaderBreadcrumb.Text = "Phòng thi thử & Khảo thí chuẩn hóa";
            LoadExamList();
            UpdateUserExamStats();
        }
    }
}
