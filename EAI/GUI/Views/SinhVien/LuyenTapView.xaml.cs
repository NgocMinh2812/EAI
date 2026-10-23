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
    public partial class LuyenTapView : Page
    {
        // =========================================================================
        // VIEW MODELS FOR PRACTICE HUB
        // =========================================================================
        public class SessionQuestionResultViewModel
        {
            public int QuestionNumber { get; set; }
            public string QuestionNumberText => $"Câu {QuestionNumber}";
            public string? KyNang { get; set; }
            public string KyNangTag => $"KỸ NĂNG: {KyNang?.ToUpper() ?? "CHUNG"}";
            public string NoiDungCauHoi { get; set; } = string.Empty;
            public string StudentChoiceText { get; set; } = string.Empty;
            public string CorrectChoiceText { get; set; } = string.Empty;
            public string ExplanationText { get; set; } = string.Empty;
            public bool IsCorrect { get; set; }

            public string StatusBadgeText => IsCorrect ? "✅ ĐÚNG" : "❌ SAI";
            public Brush StatusBadgeBg => IsCorrect ? new SolidColorBrush(Color.FromRgb(209, 250, 229)) : new SolidColorBrush(Color.FromRgb(254, 226, 226));
            public Brush StatusBadgeFg => IsCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : new SolidColorBrush(Color.FromRgb(239, 68, 68));
            public Brush StudentChoiceBorder => IsCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : new SolidColorBrush(Color.FromRgb(239, 68, 68));
            public Brush StudentChoiceFg => IsCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : new SolidColorBrush(Color.FromRgb(239, 68, 68));
        }

        public class WrongQuestionViewModel
        {
            public int MaCauHoi { get; set; }
            public string? KyNang { get; set; }
            public string KyNangTag => $"KỸ NĂNG: {KyNang?.ToUpper() ?? "CHUNG"}";
            public string NoiDungCauHoi { get; set; } = string.Empty;
            public string GiaiThichText { get; set; } = string.Empty;
            public string ThoiGianSaiText { get; set; } = string.Empty;
        }

        public class PracticeHistoryViewModel
        {
            public string TenBaiTap { get; set; } = string.Empty;
            public string ThoiGianNopFormat { get; set; } = string.Empty;
            public string ThoiLuongFormat { get; set; } = string.Empty;
            public string KetQuaSoCau { get; set; } = string.Empty;
            public string TyLeDungText { get; set; } = string.Empty;
        }

        // =========================================================================
        // STATE FIELDS: PRACTICE HUB (REQUIREMENT 4)
        // =========================================================================
        private List<CauHoiKhaoSatDTO> _sessionQuestions = new List<CauHoiKhaoSatDTO>();
        private int _sessionIndex = 0;
        private readonly Dictionary<int, int> _sessionUserAnswers = new Dictionary<int, int>();
        private readonly Dictionary<int, string> _sessionUserTextAnswers = new Dictionary<int, string>();
        private DateTime _sessionStartTime;
        private DispatcherTimer? _sessionDurationTimer;
        private int _sessionElapsedSeconds = 0;
        private bool _isInstantFeedbackMode = true;
        private bool _isCurrentQuestionAnswered = false;

        // List of wrong questions in current session for targeted re-practice
        private readonly List<CauHoiKhaoSatDTO> _currentSessionWrongQuestions = new List<CauHoiKhaoSatDTO>();

        // Cache for overall wrong questions in DB
        private List<CauHoiKhaoSatDTO> _dbWrongQuestions = new List<CauHoiKhaoSatDTO>();

        // =========================================================================
        // STATE FIELDS: FLASHCARD & DICTATION
        // =========================================================================
        private List<FlashcardDTO> _flashcards = new List<FlashcardDTO>();
        private int _currentCardIndex = 0;
        private List<string> _dictationSentences = new List<string>();
        private int _currentDictationIndex = 0;

        // User Score
        private int _score = 150;
        private bool _isInitialized = false;

        public LuyenTapView()
        {
            InitializeComponent();
            _isInitialized = true;

            // Nạp dữ liệu ban đầu
            UpdateLiveQuestionCount();
            LoadFlashcards();
            InitDictationSentences();
            DisplayCurrentDictation();
        }

        // =========================================================================
        // SPEECH SYNTHESIS HELPER
        // =========================================================================
        private void SpeakText(string text, double speed = 1.0)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var type = Type.GetType("System.Speech.Synthesis.SpeechSynthesizer, System.Speech");
                    if (type != null)
                    {
                        dynamic synth = Activator.CreateInstance(type)!;
                        synth.Volume = 100;
                        synth.Rate = (int)((speed - 1.0) * 4);
                        synth.Speak(text);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Speech] Lỗi phát âm: {ex.Message}");
                }
            });
        }

        // =========================================================================
        // TAB NAVIGATION
        // =========================================================================
        private void TabHub_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabHub, pnlHub);
            UpdateLiveQuestionCount();
        }

        private void TabWrongQuestions_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabWrongQuestions, pnlWrongQuestions);
            LoadWrongQuestions();
        }

        private void TabHistory_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabHistory, pnlHistory);
            LoadPracticeHistory();
        }

        private void TabFlashcard_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabFlashcard, pnlFlashcard);
        }

        private void TabDictation_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabDictation, pnlDictation);
            DisplayCurrentDictation();
        }

        private void SetActiveTab(Button activeBtn, StackPanel activePnl)
        {
            if (!_isInitialized) return;
            var brandTeal = (SolidColorBrush)FindResource("BrandTeal");
            var textGray = (SolidColorBrush)FindResource("TextGray");

            tabHub.BorderBrush = Brushes.Transparent; tabHub.Foreground = textGray; tabHub.FontWeight = FontWeights.Medium;
            tabWrongQuestions.BorderBrush = Brushes.Transparent; tabWrongQuestions.Foreground = textGray; tabWrongQuestions.FontWeight = FontWeights.Medium;
            tabHistory.BorderBrush = Brushes.Transparent; tabHistory.Foreground = textGray; tabHistory.FontWeight = FontWeights.Medium;
            tabFlashcard.BorderBrush = Brushes.Transparent; tabFlashcard.Foreground = textGray; tabFlashcard.FontWeight = FontWeights.Medium;
            tabDictation.BorderBrush = Brushes.Transparent; tabDictation.Foreground = textGray; tabDictation.FontWeight = FontWeights.Medium;

            activeBtn.BorderBrush = brandTeal;
            activeBtn.Foreground = brandTeal;
            activeBtn.FontWeight = FontWeights.SemiBold;

            pnlHub.Visibility = Visibility.Collapsed;
            pnlWrongQuestions.Visibility = Visibility.Collapsed;
            pnlHistory.Visibility = Visibility.Collapsed;
            pnlFlashcard.Visibility = Visibility.Collapsed;
            pnlDictation.Visibility = Visibility.Collapsed;

            activePnl.Visibility = Visibility.Visible;
        }

        // =========================================================================
        // TAB 1: ADAPTIVE PRACTICE ENGINE (YÊU CẦU 4.1 - 4.4)
        // =========================================================================
        private (string? KyNang, string? CapDo, string? DoKho, int SoLuong, bool IsInstant) GetPracticeCriteria()
        {
            string? kyNang = null;
            if (cboPracticeSkill?.SelectedItem is ComboBoxItem itemSkill)
            {
                string text = itemSkill.Content?.ToString() ?? "";
                if (text.Contains("Reading") || text.Contains("Đọc")) kyNang = "Doc";
                else if (text.Contains("Listening") || text.Contains("Nghe")) kyNang = "Nghe";
                else if (text.Contains("Grammar") || text.Contains("Ngữ pháp")) kyNang = "NguPhap";
                else if (text.Contains("Vocab") || text.Contains("Từ vựng")) kyNang = "TuVung";
                else if (text.Contains("Writing") || text.Contains("Viết")) kyNang = "Viet";
            }

            string? capDo = null;
            if (cboPracticeLevel?.SelectedItem is ComboBoxItem itemLevel)
            {
                string text = itemLevel.Content?.ToString() ?? "";
                if (text.StartsWith("A1")) capDo = "A1";
                else if (text.StartsWith("A2")) capDo = "A2";
                else if (text.StartsWith("B1")) capDo = "B1";
                else if (text.StartsWith("B2")) capDo = "B2";
                else if (text.StartsWith("C1")) capDo = "C1";
            }

            string? doKho = null;
            if (cboPracticeDifficulty?.SelectedItem is ComboBoxItem itemDiff)
            {
                string text = itemDiff.Content?.ToString() ?? "";
                if (text.Contains("Dễ") || text.Contains("Easy")) doKho = "De";
                else if (text.Contains("Trung bình") || text.Contains("Medium")) doKho = "TrungBinh";
                else if (text.Contains("Khó") || text.Contains("Hard")) doKho = "Kho";
            }

            int soLuong = 10;
            if (cboPracticeCount?.SelectedItem is ComboBoxItem itemCount)
            {
                string text = itemCount.Content?.ToString() ?? "";
                if (text.StartsWith("5")) soLuong = 5;
                else if (text.StartsWith("10")) soLuong = 10;
                else if (text.StartsWith("15")) soLuong = 15;
                else if (text.StartsWith("20")) soLuong = 20;
            }

            bool isInstant = true;
            if (cboPracticeFeedbackMode?.SelectedItem is ComboBoxItem itemMode)
            {
                string text = itemMode.Content?.ToString() ?? "";
                if (text.Contains("cuối phiên")) isInstant = false;
            }

            return (kyNang, capDo, doKho, soLuong, isInstant);
        }

        private void PracticeFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            UpdateLiveQuestionCount();
        }

        private void UpdateLiveQuestionCount()
        {
            if (txtLiveAvailableCount == null) return;
            var (kyNang, capDo, doKho, _, _) = GetPracticeCriteria();

            try
            {
                int count = LuyenTapBUS.Instance.DemSoCauKhaDung(kyNang, capDo, doKho);
                if (count > 0)
                {
                    txtLiveAvailableCount.Text = $"💡 Đang có {count} câu hỏi phù hợp sẵn sàng trong Ngân hàng câu hỏi.";
                    txtLiveAvailableCount.Foreground = new SolidColorBrush(Color.FromRgb(22, 101, 52));
                }
                else
                {
                    txtLiveAvailableCount.Text = "⚠️ Chưa tìm thấy câu hỏi phù hợp với bộ lọc hiện tại. Hãy thử chọn tất cả hoặc nới lỏng bộ lọc.";
                    txtLiveAvailableCount.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));
                }
            }
            catch (Exception ex)
            {
                txtLiveAvailableCount.Text = $"Lỗi kiểm tra câu hỏi: {ex.Message}";
            }
        }

        private void BtnQuickPractice_Click(object sender, RoutedEventArgs e)
        {
            StartPracticeWithQuestions(null, null, null, 5, isInstant: true);
        }

        private void BtnStartPracticeSession_Click(object sender, RoutedEventArgs e)
        {
            var (kyNang, capDo, doKho, soLuong, isInstant) = GetPracticeCriteria();
            StartPracticeWithQuestions(kyNang, capDo, doKho, soLuong, isInstant);
        }

        private void StartPracticeWithQuestions(string? kyNang, string? capDo, string? doKho, int soLuong, bool isInstant)
        {
            try
            {
                int availableCount = LuyenTapBUS.Instance.DemSoCauKhaDung(kyNang, capDo, doKho);
                if (availableCount == 0)
                {
                    Common.ThongBaoDialog.CanhBao(
                        "Không có câu hỏi nào trong Ngân hàng câu hỏi thỏa mãn bộ lọc đã chọn.\n" +
                        "Vui lòng nới lỏng tiêu chí (chọn tất cả kỹ năng hoặc tất cả độ khó).", 
                        "Không có câu hỏi phù hợp");
                    return;
                }

                if (availableCount < soLuong)
                {
                    bool xacNhan = Common.ThongBaoDialog.XacNhan(
                        $"Ngân hàng câu hỏi hiện chỉ có {availableCount} câu phù hợp với tiêu chí của bạn.\n" +
                        $"Bạn có muốn bắt đầu phiên luyện tập với {availableCount} câu này không?",
                        "Thông báo số lượng câu hỏi",
                        "Bắt đầu ngay", "Đổi bộ lọc");
                    if (!xacNhan) return;
                }

                _sessionQuestions = LuyenTapBUS.Instance.LayDanhSachCauHoiLuyenTap(kyNang, capDo, doKho, soLuong: soLuong);
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.Loi($"Lỗi khi lấy câu hỏi luyện tập: {ex.Message}", "Lỗi");
                return;
            }

            if (_sessionQuestions == null || _sessionQuestions.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Không thể tải danh sách câu hỏi. Vui lòng kiểm tra lại kết nối cơ sở dữ liệu.", "Thông báo");
                return;
            }

            // Khởi tạo phiên luyện tập
            _sessionIndex = 0;
            _sessionUserAnswers.Clear();
            _sessionUserTextAnswers.Clear();
            _currentSessionWrongQuestions.Clear();
            _isInstantFeedbackMode = isInstant;
            _sessionStartTime = DateTime.Now;
            _sessionElapsedSeconds = 0;

            // Khởi động đồng hồ thời gian làm bài (đếm xuôi)
            StartSessionTimer();

            // Chuyển view
            pnlHubSetup.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSession.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = $"Đang trong phiên luyện tập ({_sessionQuestions.Count} câu)";

            DisplaySessionQuestion(_sessionIndex);
        }

        private void StartSessionTimer()
        {
            _sessionDurationTimer?.Stop();
            _sessionDurationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _sessionDurationTimer.Tick += (s, e) =>
            {
                _sessionElapsedSeconds++;
                int m = _sessionElapsedSeconds / 60;
                int sec = _sessionElapsedSeconds % 60;
                txtSessionDuration.Text = $"⏱️ {m:D2}:{sec:D2}";
            };
            _sessionDurationTimer.Start();
        }

        private void DisplaySessionQuestion(int index)
        {
            if (index < 0 || index >= _sessionQuestions.Count) return;

            _sessionIndex = index;
            _isCurrentQuestionAnswered = false;
            var q = _sessionQuestions[index];

            txtSessionProgress.Text = $"CÂU {index + 1} / {_sessionQuestions.Count}";
            txtSessionSkillBadge.Text = $"KỸ NĂNG: {q.KyNangHienThi.ToUpper()}";
            txtSessionModeBadge.Text = _isInstantFeedbackMode ? "CHẾ ĐỘ: PHẢN HỒI NGAY" : "CHẾ ĐỘ: TỔNG KẾT CUỐI PHIÊN";
            txtPracticeQuestionPrompt.Text = $"Câu {index + 1}: {q.NoiDungCauHoi}";

            // Passage reading box
            if (!string.IsNullOrWhiteSpace(q.DoanVanDoc) && !q.IsKyNangNghe)
            {
                pnlPracticePassage.Visibility = Visibility.Visible;
                txtPracticePassage.Text = q.DoanVanDoc;
            }
            else
            {
                pnlPracticePassage.Visibility = Visibility.Collapsed;
            }

            // Listening audio box
            if (q.IsKyNangNghe)
            {
                pnlPracticeAudio.Visibility = Visibility.Visible;
                txtPracticeAudioStatus.Text = "🎧 Bấm để nghe phát âm bài nói chuẩn bản xứ";
            }
            else
            {
                pnlPracticeAudio.Visibility = Visibility.Collapsed;
            }

            // Choice questions vs text questions
            if (q.IsDangLuaChon)
            {
                pnlPracticeChoices.Visibility = Visibility.Visible;
                pnlPracticeTextInput.Visibility = Visibility.Collapsed;

                ResetPracticeOptionButtons();
                var options = q.DanhSachLuaChon;

                btnChoiceA.Content = options.Count > 0 ? $"A. {CauHoiDTO.StripOptionPrefix(options[0].NoiDungLuaChon)}" : "A.";
                btnChoiceA.Tag = options.Count > 0 ? options[0] : null;

                btnChoiceB.Content = options.Count > 1 ? $"B. {CauHoiDTO.StripOptionPrefix(options[1].NoiDungLuaChon)}" : "B.";
                btnChoiceB.Tag = options.Count > 1 ? options[1] : null;

                btnChoiceC.Content = options.Count > 2 ? $"C. {CauHoiDTO.StripOptionPrefix(options[2].NoiDungLuaChon)}" : "C.";
                btnChoiceC.Tag = options.Count > 2 ? options[2] : null;

                btnChoiceD.Content = options.Count > 3 ? $"D. {CauHoiDTO.StripOptionPrefix(options[3].NoiDungLuaChon)}" : "D.";
                btnChoiceD.Tag = options.Count > 3 ? options[3] : null;

                btnChoiceC.Visibility = options.Count > 2 ? Visibility.Visible : Visibility.Collapsed;
                btnChoiceD.Visibility = options.Count > 3 ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                pnlPracticeChoices.Visibility = Visibility.Collapsed;
                pnlPracticeTextInput.Visibility = Visibility.Visible;
                txtPracticeStudentText.Text = string.Empty;
                btnCheckPracticeText.IsEnabled = true;
            }

            pnlPracticeFeedbackBox.Visibility = Visibility.Collapsed;
            btnNextPracticeQuestion.Content = (index == _sessionQuestions.Count - 1) 
                ? "Hoàn thành & Xem kết quả ➔" 
                : "Câu tiếp theo ➔";
        }

        private void ResetPracticeOptionButtons()
        {
            var defaultBg = Brushes.White;
            var defaultBorder = (SolidColorBrush)FindResource("BorderLight");

            btnChoiceA.Background = defaultBg; btnChoiceA.BorderBrush = defaultBorder; btnChoiceA.IsEnabled = true;
            btnChoiceB.Background = defaultBg; btnChoiceB.BorderBrush = defaultBorder; btnChoiceB.IsEnabled = true;
            btnChoiceC.Background = defaultBg; btnChoiceC.BorderBrush = defaultBorder; btnChoiceC.IsEnabled = true;
            btnChoiceD.Background = defaultBg; btnChoiceD.BorderBrush = defaultBorder; btnChoiceD.IsEnabled = true;
        }

        private void BtnPracticeChoice_Click(object sender, RoutedEventArgs e)
        {
            if (_isCurrentQuestionAnswered) return;
            if (sender is not Button clickedBtn || clickedBtn.Tag is not LuaChonDapAnDTO selectedOpt) return;

            var q = _sessionQuestions[_sessionIndex];
            _sessionUserAnswers[q.MaCauHoi] = selectedOpt.MaLuaChon;

            bool isCorrect = selectedOpt.LaDapAnDung;
            if (!isCorrect)
            {
                if (!_currentSessionWrongQuestions.Any(w => w.MaCauHoi == q.MaCauHoi))
                    _currentSessionWrongQuestions.Add(q);
            }

            if (_isInstantFeedbackMode)
            {
                _isCurrentQuestionAnswered = true;

                // Disable all option buttons
                btnChoiceA.IsEnabled = false; btnChoiceB.IsEnabled = false; btnChoiceC.IsEnabled = false; btnChoiceD.IsEnabled = false;

                // Highlight correct button
                Button? correctBtn = null;
                if (btnChoiceA.Tag is LuaChonDapAnDTO oA && oA.LaDapAnDung) correctBtn = btnChoiceA;
                else if (btnChoiceB.Tag is LuaChonDapAnDTO oB && oB.LaDapAnDung) correctBtn = btnChoiceB;
                else if (btnChoiceC.Tag is LuaChonDapAnDTO oC && oC.LaDapAnDung) correctBtn = btnChoiceC;
                else if (btnChoiceD.Tag is LuaChonDapAnDTO oD && oD.LaDapAnDung) correctBtn = btnChoiceD;

                if (isCorrect)
                {
                    clickedBtn.Background = new SolidColorBrush(Color.FromRgb(209, 250, 229));
                    clickedBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));

                    txtPracticeFeedbackTitle.Text = "✅ CHÍNH XÁC!";
                    txtPracticeFeedbackTitle.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                    pnlPracticeFeedbackBox.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                    pnlPracticeFeedbackBox.BorderBrush = new SolidColorBrush(Color.FromRgb(110, 231, 183));

                    _score += 15;
                    txtUserScore.Text = $"Điểm tích lũy: {_score} pts";
                }
                else
                {
                    clickedBtn.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226));
                    clickedBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));

                    if (correctBtn != null)
                    {
                        correctBtn.Background = new SolidColorBrush(Color.FromRgb(209, 250, 229));
                        correctBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                    }

                    txtPracticeFeedbackTitle.Text = "❌ CHƯA CHÍNH XÁC";
                    txtPracticeFeedbackTitle.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                    pnlPracticeFeedbackBox.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                    pnlPracticeFeedbackBox.BorderBrush = new SolidColorBrush(Color.FromRgb(252, 165, 165));
                }

                var correctOpt = q.DanhSachLuaChon.FirstOrDefault(o => o.LaDapAnDung);
                txtPracticeCorrectAnswerText.Text = correctOpt != null 
                    ? $"Đáp án chính xác: {correctOpt.KyHieuLuaChon}. {correctOpt.NoiDungLuaChon}" 
                    : "";
                txtPracticeExplanationText.Text = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) 
                    ? $"💡 Giải thích: {q.GiaiThichDapAn}" 
                    : "💡 Giải thích: Đáp án chuẩn theo ngân hàng câu hỏi đề thi.";

                pnlPracticeFeedbackBox.Visibility = Visibility.Visible;
            }
            else
            {
                // Delayed feedback: highlight selection and allow Next
                ResetPracticeOptionButtons();
                clickedBtn.Background = (SolidColorBrush)FindResource("BrandTealLight");
                clickedBtn.BorderBrush = (SolidColorBrush)FindResource("BrandTeal");

                // Auto advance or wait for Next
                _isCurrentQuestionAnswered = true;
                btnNextPracticeQuestion.Visibility = Visibility.Visible;
                pnlPracticeFeedbackBox.Visibility = Visibility.Visible;
                txtPracticeFeedbackTitle.Text = "Đã ghi nhận lựa chọn.";
                txtPracticeFeedbackTitle.Foreground = (SolidColorBrush)FindResource("TextDark");
                pnlPracticeFeedbackBox.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
                pnlPracticeFeedbackBox.BorderBrush = (SolidColorBrush)FindResource("BorderLight");
                txtPracticeCorrectAnswerText.Text = "Đáp án và lời giải sẽ được hiển thị ở bảng tổng kết cuối phiên.";
                txtPracticeExplanationText.Text = string.Empty;
            }
        }

        private void BtnCheckPracticeText_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionIndex < 0 || _sessionIndex >= _sessionQuestions.Count) return;
            var q = _sessionQuestions[_sessionIndex];
            string answer = txtPracticeStudentText.Text.Trim();

            _sessionUserTextAnswers[q.MaCauHoi] = answer;
            var checkResult = LuyenTapBUS.Instance.KiemTraDapAn(q, null, answer);
            bool isCorrect = checkResult.IsCorrect;
            if (!isCorrect)
            {
                if (!_currentSessionWrongQuestions.Any(w => w.MaCauHoi == q.MaCauHoi))
                    _currentSessionWrongQuestions.Add(q);
            }

            _isCurrentQuestionAnswered = true;
            btnCheckPracticeText.IsEnabled = false;

            if (_isInstantFeedbackMode)
            {
                if (isCorrect)
                {
                    txtPracticeFeedbackTitle.Text = "✅ CHÍNH XÁC!";
                    txtPracticeFeedbackTitle.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                    pnlPracticeFeedbackBox.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                    pnlPracticeFeedbackBox.BorderBrush = new SolidColorBrush(Color.FromRgb(110, 231, 183));
                    _score += 15;
                    txtUserScore.Text = $"Điểm tích lũy: {_score} pts";
                }
                else
                {
                    txtPracticeFeedbackTitle.Text = "❌ CHƯA CHÍNH XÁC";
                    txtPracticeFeedbackTitle.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                    pnlPracticeFeedbackBox.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                    pnlPracticeFeedbackBox.BorderBrush = new SolidColorBrush(Color.FromRgb(252, 165, 165));
                }

                txtPracticeCorrectAnswerText.Text = $"Đáp án mẫu: {q.DapAnDungText ?? "(Theo barem chuẩn)"}";
                txtPracticeExplanationText.Text = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) ? $"💡 {q.GiaiThichDapAn}" : "💡 Hãy chú ý chính tả và ngữ pháp.";
                pnlPracticeFeedbackBox.Visibility = Visibility.Visible;
            }
            else
            {
                pnlPracticeFeedbackBox.Visibility = Visibility.Visible;
                txtPracticeFeedbackTitle.Text = "Đã ghi nhận câu trả lời.";
                txtPracticeFeedbackTitle.Foreground = (SolidColorBrush)FindResource("TextDark");
                pnlPracticeFeedbackBox.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
                pnlPracticeFeedbackBox.BorderBrush = (SolidColorBrush)FindResource("BorderLight");
                txtPracticeCorrectAnswerText.Text = "Kết quả chi tiết sẽ hiển thị ở cuối phiên.";
                txtPracticeExplanationText.Text = string.Empty;
            }
        }

        private void BtnNextPracticeQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionIndex < _sessionQuestions.Count - 1)
            {
                DisplaySessionQuestion(_sessionIndex + 1);
            }
            else
            {
                FinishPracticeSession();
            }
        }

        private void BtnStopPracticeSession_Click(object sender, RoutedEventArgs e)
        {
            bool xacNhan = Common.ThongBaoDialog.XacNhan(
                "Bạn có chắc chắn muốn dừng phiên luyện tập hiện tại?\nKết quả các câu đã làm sẽ được lưu lại.",
                "Dừng phiên luyện tập",
                "Dừng ngay", "Tiếp tục làm");
            if (xacNhan)
            {
                FinishPracticeSession();
            }
        }

        private void FinishPracticeSession()
        {
            _sessionDurationTimer?.Stop();

            int timeSpent = Math.Max(5, _sessionElapsedSeconds);
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            string title = $"Luyện tập {_sessionQuestions.FirstOrDefault()?.KyNangHienThi ?? "kỹ năng tổng hợp"}";

            // Lưu kết quả phiên luyện tập vào database (HinhThucLam = 'LuyenTap')
            try
            {
                LuyenTapBUS.Instance.LuuKetQuaPhienLuyenTap(
                    userId,
                    title,
                    _sessionStartTime,
                    timeSpent,
                    _sessionQuestions,
                    _sessionUserAnswers,
                    _sessionUserTextAnswers);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapView] Lỗi lưu kết quả phiên: {ex.Message}");
            }

            // Tính toán tổng kết
            int total = _sessionQuestions.Count;
            int correct = 0;
            var resultItems = new List<SessionQuestionResultViewModel>();

            for (int i = 0; i < _sessionQuestions.Count; i++)
            {
                var q = _sessionQuestions[i];
                int? userOptId = _sessionUserAnswers.ContainsKey(q.MaCauHoi) ? _sessionUserAnswers[q.MaCauHoi] : null;
                string? userText = _sessionUserTextAnswers.ContainsKey(q.MaCauHoi) ? _sessionUserTextAnswers[q.MaCauHoi] : null;

                var checkResult = LuyenTapBUS.Instance.KiemTraDapAn(q, userOptId, userText);
                bool isCorrect = checkResult.IsCorrect;
                if (isCorrect) correct++;

                string studentChoiceText = "(Chưa trả lời)";
                if (userOptId.HasValue)
                {
                    var opt = q.DanhSachLuaChon.FirstOrDefault(o => o.MaLuaChon == userOptId.Value);
                    if (opt != null) studentChoiceText = $"{opt.KyHieuLuaChon}. {opt.NoiDungLuaChon}";
                }
                else if (!string.IsNullOrWhiteSpace(userText))
                {
                    studentChoiceText = userText;
                }

                string correctText;
                if (q.IsDangLuaChon)
                {
                    var cOpt = q.DanhSachLuaChon.FirstOrDefault(o => o.LaDapAnDung);
                    correctText = cOpt != null ? $"{cOpt.KyHieuLuaChon}. {cOpt.NoiDungLuaChon}" : "Đang cập nhật";
                }
                else
                {
                    correctText = q.DapAnDungText ?? "(Theo barem chuẩn)";
                }

                resultItems.Add(new SessionQuestionResultViewModel
                {
                    QuestionNumber = i + 1,
                    KyNang = q.KyNang,
                    NoiDungCauHoi = q.NoiDungCauHoi,
                    StudentChoiceText = studentChoiceText,
                    CorrectChoiceText = correctText,
                    ExplanationText = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) ? $"💡 {q.GiaiThichDapAn}" : "💡 Đáp án chuẩn.",
                    IsCorrect = isCorrect
                });
            }

            int wrong = total - correct;
            int accuracyPct = total > 0 ? (correct * 100) / total : 0;

            // Hiển thị Card tổng kết
            txtResultAccuracyPct.Text = $"{accuracyPct}%";
            txtResultCorrectText.Text = $"✅ Đúng: {correct} câu";
            txtResultWrongText.Text = $"❌ Sai: {wrong} câu";
            txtResultTimeSpent.Text = $"⏱️ Thời gian: {timeSpent / 60:D2}:{timeSpent % 60:D2}";

            if (accuracyPct >= 80)
            {
                txtResultAiComment.Text = "AI Nhận xét: Bạn phản xạ rất nhanh và chuẩn xác! Hãy duy trì phong độ và thử thách với các câu hỏi khó hơn.";
            }
            else if (accuracyPct >= 50)
            {
                txtResultAiComment.Text = "AI Nhận xét: Khá tốt! Hãy chú ý các bẫy liên từ và từ đồng nghĩa, đồng thời luyện lại các câu sai bên dưới.";
            }
            else
            {
                txtResultAiComment.Text = "AI Nhận xét: Bạn cần củng cố thêm ngữ pháp và vốn từ vựng nền tảng. Hãy bấm 'Luyện lại các câu sai' để làm chủ các kiến thức này.";
            }

            // Nút luyện lại câu sai
            btnRePracticeWrongInSession.IsEnabled = (_currentSessionWrongQuestions.Count > 0);
            btnRePracticeWrongInSession.Visibility = (_currentSessionWrongQuestions.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

            icSessionQuestionResults.ItemsSource = resultItems;

            pnlHubSession.Visibility = Visibility.Collapsed;
            pnlHubSetup.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = "Tổng kết phiên luyện tập phản xạ";
        }

        private void BtnRePracticeWrongInSession_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSessionWrongQuestions.Count == 0) return;

            _sessionQuestions = new List<CauHoiKhaoSatDTO>(_currentSessionWrongQuestions);
            _sessionIndex = 0;
            _sessionUserAnswers.Clear();
            _sessionUserTextAnswers.Clear();
            _currentSessionWrongQuestions.Clear();
            _isInstantFeedbackMode = true; // Luôn dùng phản hồi ngay khi ôn câu sai
            _sessionStartTime = DateTime.Now;
            _sessionElapsedSeconds = 0;

            StartSessionTimer();

            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSetup.Visibility = Visibility.Collapsed;
            pnlHubSession.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = $"Ôn lại {_sessionQuestions.Count} câu sai trong phiên vừa rồi";

            DisplaySessionQuestion(0);
        }

        private void BtnNewPracticeSession_Click(object sender, RoutedEventArgs e)
        {
            _sessionDurationTimer?.Stop();
            pnlHubSession.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSetup.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = "Trung tâm luyện tập linh hoạt & Kỹ năng";
            UpdateLiveQuestionCount();
        }

        private void BtnPlayPracticeAudio_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionIndex < 0 || _sessionIndex >= _sessionQuestions.Count) return;
            var q = _sessionQuestions[_sessionIndex];
            string content = !string.IsNullOrWhiteSpace(q.DoanVanDoc) ? q.DoanVanDoc : q.NoiDungCauHoi;
            txtPracticeAudioStatus.Text = "🎧 Đang phát bài nghe (TTS)...";
            SpeakText(content);
        }

        private void BtnSpeakExplanation_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionIndex < 0 || _sessionIndex >= _sessionQuestions.Count) return;
            var q = _sessionQuestions[_sessionIndex];
            if (!string.IsNullOrWhiteSpace(q.GiaiThichDapAn))
            {
                SpeakText(q.GiaiThichDapAn);
            }
        }

        // =========================================================================
        // TAB 2: REVIEW WRONG QUESTIONS (TASK 4.5)
        // =========================================================================
        private void LoadWrongQuestions()
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            try
            {
                _dbWrongQuestions = LuyenTapBUS.Instance.LayDanhSachCauHoiSaiCuaNguoiDung(userId, top: 30);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapView] Lỗi lấy câu hỏi sai: {ex.Message}");
                _dbWrongQuestions = new List<CauHoiKhaoSatDTO>();
            }

            if (_dbWrongQuestions.Count == 0)
            {
                pnlNoWrongQuestions.Visibility = Visibility.Visible;
                icWrongQuestionsList.Visibility = Visibility.Collapsed;
                btnDrillAllWrong.IsEnabled = false;
            }
            else
            {
                pnlNoWrongQuestions.Visibility = Visibility.Collapsed;
                icWrongQuestionsList.Visibility = Visibility.Visible;
                btnDrillAllWrong.IsEnabled = true;

                var vms = _dbWrongQuestions.Select(q => new WrongQuestionViewModel
                {
                    MaCauHoi = q.MaCauHoi,
                    KyNang = q.KyNang,
                    NoiDungCauHoi = q.NoiDungCauHoi,
                    GiaiThichText = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) 
                        ? $"💡 Giải thích: {q.GiaiThichDapAn}" 
                        : "💡 Hãy chú ý cấu trúc và từ vựng của câu hỏi này.",
                    ThoiGianSaiText = $"Mã câu hỏi: #{q.MaCauHoi} • Trình độ {q.MaTrinhDo ?? "B1"}"
                }).ToList();

                icWrongQuestionsList.ItemsSource = vms;
            }
        }

        private void BtnDrillAllWrong_Click(object sender, RoutedEventArgs e)
        {
            if (_dbWrongQuestions.Count == 0) return;

            // Chuyển sang Tab Hub và bắt đầu phiên với toàn bộ câu hỏi sai
            SetActiveTab(tabHub, pnlHub);

            _sessionQuestions = new List<CauHoiKhaoSatDTO>(_dbWrongQuestions);
            _sessionIndex = 0;
            _sessionUserAnswers.Clear();
            _sessionUserTextAnswers.Clear();
            _currentSessionWrongQuestions.Clear();
            _isInstantFeedbackMode = true;
            _sessionStartTime = DateTime.Now;
            _sessionElapsedSeconds = 0;

            StartSessionTimer();

            pnlHubSetup.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSession.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = $"Ôn tập {_sessionQuestions.Count} câu hỏi bạn đã từng làm sai";

            DisplaySessionQuestion(0);
        }

        private void BtnDrillSingleWrong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag == null || !int.TryParse(btn.Tag.ToString(), out int maCauHoi)) return;

            var targetQ = _dbWrongQuestions.FirstOrDefault(q => q.MaCauHoi == maCauHoi);
            if (targetQ == null) return;

            SetActiveTab(tabHub, pnlHub);

            _sessionQuestions = new List<CauHoiKhaoSatDTO> { targetQ };
            _sessionIndex = 0;
            _sessionUserAnswers.Clear();
            _sessionUserTextAnswers.Clear();
            _currentSessionWrongQuestions.Clear();
            _isInstantFeedbackMode = true;
            _sessionStartTime = DateTime.Now;
            _sessionElapsedSeconds = 0;

            StartSessionTimer();

            pnlHubSetup.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSession.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = $"Luyện tập trọng điểm: Câu hỏi #{targetQ.MaCauHoi}";

            DisplaySessionQuestion(0);
        }

        // =========================================================================
        // TAB 3: PRACTICE HISTORY
        // =========================================================================
        private void LoadPracticeHistory()
        {
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            List<LuotLamBaiDTO> history;
            try
            {
                history = LuyenTapBUS.Instance.LayLichSuLuyenTap(userId, top: 20);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapView] Lỗi nạp lịch sử luyện tập: {ex.Message}");
                history = new List<LuotLamBaiDTO>();
            }

            if (history == null || history.Count == 0)
            {
                pnlNoHistory.Visibility = Visibility.Visible;
                icPracticeHistory.Visibility = Visibility.Collapsed;
            }
            else
            {
                pnlNoHistory.Visibility = Visibility.Collapsed;
                icPracticeHistory.Visibility = Visibility.Visible;

                var vms = history.Select(h =>
                {
                    int m = h.ThoiGianLamGiay / 60;
                    int s = h.ThoiGianLamGiay % 60;
                    return new PracticeHistoryViewModel
                    {
                        TenBaiTap = h.TenBaiTap,
                        ThoiGianNopFormat = $"📅 {h.ThoiGianKetThuc:dd/MM/yyyy HH:mm}",
                        ThoiLuongFormat = $"⏱️ {m:D2}:{s:D2}",
                        KetQuaSoCau = $"✅ {h.SoCauDung} / {h.TongSoCau} câu",
                        TyLeDungText = $"{h.TyLeChinhXacText} chính xác"
                    };
                }).ToList();

                icPracticeHistory.ItemsSource = vms;
            }
        }

        // =========================================================================
        // TAB 4: FLASHCARD VOCABULARY (DATABASE LOADED)
        // =========================================================================
        private void LoadFlashcards(string? trinhDo = null)
        {
            try
            {
                _flashcards = TuVungBUS.Instance.LayDanhSachFlashcard(trinhDo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi nạp flashcard: {ex.Message}");
                _flashcards = new List<FlashcardDTO>();
            }

            if (_flashcards.Count == 0)
            {
                _flashcards = TuVungBUS.Instance.LayDanhSachFlashcard(null);
            }

            _currentCardIndex = 0;
            UpdateCardView();
        }

        private void CboLevelFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            if (cboLevelFilter?.SelectedItem is ComboBoxItem item)
            {
                string text = item.Content?.ToString() ?? "";
                string? trinhDo = null;
                if (text.Contains("A1")) trinhDo = "A1";
                else if (text.Contains("A2")) trinhDo = "A2";
                else if (text.Contains("B1")) trinhDo = "B1";
                else if (text.Contains("B2")) trinhDo = "B2";
                else if (text.Contains("C1")) trinhDo = "C1";

                LoadFlashcards(trinhDo);
            }
        }

        private void UpdateCardView()
        {
            if (!_isInitialized || txtCardWord == null || txtCardIndex == null) return;
            if (_flashcards == null || _flashcards.Count == 0)
            {
                txtCardWord.Text = "Chưa có từ vựng";
                txtCardIndex.Text = "0 / 0";
                return;
            }

            if (_currentCardIndex < 0) _currentCardIndex = 0;
            if (_currentCardIndex >= _flashcards.Count) _currentCardIndex = 0;

            var card = _flashcards[_currentCardIndex];
            txtCardIndex.Text = $"{_currentCardIndex + 1} / {_flashcards.Count}";
            txtCardTopic.Text = card.TopicBadge;
            txtCardWord.Text = card.Tu;
            txtCardIpa.Text = card.IpaFormatted;
            txtCardMeaning.Text = card.Nghia;
            txtCardExample.Text = !string.IsNullOrWhiteSpace(card.CauViDu) ? $"\"{card.CauViDu}\"" : "\"No example provided.\"";
            txtCardCollocation.Text = !string.IsNullOrWhiteSpace(card.CumTuDiKem) ? $"• {card.CumTuDiKem}" : "• Standard usage";

            cardFront.Visibility = Visibility.Visible;
            cardBack.Visibility = Visibility.Collapsed;
        }

        private void BtnFlipCard_Click(object sender, RoutedEventArgs e)
        {
            cardFront.Visibility = Visibility.Collapsed;
            cardBack.Visibility = Visibility.Visible;
        }

        private void BtnRateCard_Click(object sender, RoutedEventArgs e)
        {
            if (_flashcards.Count == 0) return;

            var card = _flashcards[_currentCardIndex];
            bool daNho = false;
            if (sender is Button btn && btn.Tag != null)
            {
                bool.TryParse(btn.Tag.ToString(), out daNho);
            }

            TuVungBUS.Instance.CapNhatKetQuaFlashcard(card.MaTuVung, daNho);

            _score += daNho ? 15 : 5;
            txtUserScore.Text = $"Điểm tích lũy: {_score} pts";

            _currentCardIndex = (_currentCardIndex + 1) % _flashcards.Count;
            UpdateCardView();
        }

        private void BtnShuffleCard_Click(object sender, RoutedEventArgs e)
        {
            if (_flashcards.Count <= 1) return;
            var rnd = new Random();
            _flashcards = _flashcards.OrderBy(x => rnd.Next()).ToList();
            _currentCardIndex = 0;
            UpdateCardView();
        }

        private void BtnPlayAudio_Click(object sender, RoutedEventArgs e)
        {
            if (_flashcards.Count == 0) return;
            var card = _flashcards[_currentCardIndex];
            SpeakText(card.Tu, 1.0);
        }

        private void BtnPlayExampleAudio_Click(object sender, RoutedEventArgs e)
        {
            if (_flashcards.Count == 0) return;
            var card = _flashcards[_currentCardIndex];
            if (!string.IsNullOrWhiteSpace(card.CauViDu))
            {
                SpeakText(card.CauViDu, 0.9);
            }
        }

        // =========================================================================
        // TAB 5: DICTATION - NGHE CHÉP CHÍNH TẢ
        // =========================================================================
        private void InitDictationSentences()
        {
            _dictationSentences = new List<string>
            {
                "Smartphones have become ubiquitous in daily modern life.",
                "The research paper required meticulous attention to detail.",
                "Governments must take a pragmatic and balanced approach to sustainability.",
                "Subsequent geological studies corroborated the theory proposed by scientists.",
                "Excessive screen time has a detrimental effect on sleep quality.",
                "The government introduced strict policies to mitigate the adverse impacts of pollution."
            };

            if (_flashcards != null && _flashcards.Count > 0)
            {
                foreach (var f in _flashcards)
                {
                    if (!string.IsNullOrWhiteSpace(f.CauViDu) && !_dictationSentences.Contains(f.CauViDu))
                    {
                        _dictationSentences.Add(f.CauViDu);
                    }
                }
            }

            _currentDictationIndex = 0;
        }

        private void DisplayCurrentDictation()
        {
            if (!_isInitialized || txtDictationIndex == null || txtDictationInput == null) return;
            if (_dictationSentences.Count == 0) return;
            if (_currentDictationIndex >= _dictationSentences.Count) _currentDictationIndex = 0;

            txtDictationIndex.Text = $"Câu {_currentDictationIndex + 1} / {_dictationSentences.Count}";
            txtDictationInput.Text = string.Empty;
            bdrDictationResult.Visibility = Visibility.Collapsed;
        }

        private void BtnPlayDictationNormal_Click(object sender, RoutedEventArgs e)
        {
            if (_dictationSentences.Count == 0) return;
            string sentence = _dictationSentences[_currentDictationIndex];
            SpeakText(sentence, 1.0);
        }

        private void BtnPlayDictationSlow_Click(object sender, RoutedEventArgs e)
        {
            if (_dictationSentences.Count == 0) return;
            string sentence = _dictationSentences[_currentDictationIndex];
            SpeakText(sentence, 0.75);
        }

        private void BtnCheckDictation_Click(object sender, RoutedEventArgs e)
        {
            string input = txtDictationInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                Common.ThongBaoDialog.CanhBao("Vui lòng gõ nội dung bạn nghe được trước khi kiểm tra.", "Thông báo");
                return;
            }

            string original = _dictationSentences[_currentDictationIndex];
            int accuracy = TinhDoChinhXac(input, original);

            bdrDictationResult.Visibility = Visibility.Visible;
            if (accuracy >= 80)
            {
                txtTranscriptResult.Text = $"🎉 Xuất sắc! Độ chính xác: {accuracy}%\n\n• Bạn đã gõ: \"{input}\"\n• Bản gốc: \"{original}\"";
                _score += 25;
            }
            else
            {
                txtTranscriptResult.Text = $"⚠️ Độ chính xác: {accuracy}%\n\n• Bạn đã gõ: \"{input}\"\n• Bản gốc: \"{original}\"\n💡 Hãy bấm 'Nghe chậm' để nghe rõ từng âm vị.";
                _score += 10;
            }
            txtUserScore.Text = $"Điểm tích lũy: {_score} pts";
        }

        private int TinhDoChinhXac(string input, string target)
        {
            char[] sep = new[] { ' ', '.', ',', '!', '?', ';', ':', '\"', '\'' };
            var words1 = input.ToLower().Split(sep, StringSplitOptions.RemoveEmptyEntries);
            var words2 = target.ToLower().Split(sep, StringSplitOptions.RemoveEmptyEntries);

            if (words2.Length == 0) return 0;
            int matched = words1.Count(w => words2.Contains(w));
            int pct = (int)((matched * 100.0) / Math.Max(words1.Length, words2.Length));
            return Math.Min(100, Math.Max(0, pct));
        }

        private void BtnShowTranscript_Click(object sender, RoutedEventArgs e)
        {
            if (_dictationSentences.Count == 0) return;
            string original = _dictationSentences[_currentDictationIndex];
            bdrDictationResult.Visibility = Visibility.Visible;
            txtTranscriptResult.Text = $"📝 Bản gốc tiếng Anh: \"{original}\"";
        }

        private void BtnNextDictation_Click(object sender, RoutedEventArgs e)
        {
            _currentDictationIndex++;
            if (_currentDictationIndex >= _dictationSentences.Count)
            {
                Common.ThongBaoDialog.ThanhCong("🎉 Bạn đã luyện tập hết danh sách câu nghe chép chính tả đợt này!", "Hoàn thành");
                _currentDictationIndex = 0;
            }
            DisplayCurrentDictation();
        }
    }
}
