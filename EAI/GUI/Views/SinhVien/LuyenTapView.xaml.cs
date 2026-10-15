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
        // Data sources loaded from Database
        private List<FlashcardDTO> _flashcards = new List<FlashcardDTO>();
        private int _currentCardIndex = 0;

        // Quiz Mode State
        private List<CauHoiKhaoSatDTO> _quizQuestions = new List<CauHoiKhaoSatDTO>();
        private int _currentQuizIndex = 0;
        private DispatcherTimer? _quizTimer;
        private int _quizSecondsRemaining = 60;
        private bool _quizAnswered = false;

        // Dictation Mode State
        private List<string> _dictationSentences = new List<string>();
        private int _currentDictationIndex = 0;

        // Overall Score
        private int _score = 150;
        private bool _isInitialized = false;

        public LuyenTapView()
        {
            InitializeComponent();
            _isInitialized = true;
            LoadFlashcards();
            LoadQuizQuestions();
            InitDictationSentences();
            DisplayCurrentQuiz();
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
                        synth.Rate = (int)((speed - 1.0) * 4); // Map speed 0.75 -> -1, 1.0 -> 0
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
        // TAB SWITCHING
        // =========================================================================
        private void TabFlashcard_Click(object sender, RoutedEventArgs e)
        {
            _quizTimer?.Stop();
            SetActiveTab(tabFlashcard, pnlFlashcard);
        }

        private void TabQuiz_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(tabQuiz, pnlQuiz);
            StartQuizTimer();
            DisplayCurrentQuiz();
        }

        private void TabDictation_Click(object sender, RoutedEventArgs e)
        {
            _quizTimer?.Stop();
            SetActiveTab(tabDictation, pnlDictation);
            DisplayCurrentDictation();
        }

        private void SetActiveTab(Button activeBtn, StackPanel activePnl)
        {
            if (!_isInitialized || tabFlashcard == null) return;
            var brandTeal = (SolidColorBrush)FindResource("BrandTeal");
            var textGray = (SolidColorBrush)FindResource("TextGray");

            tabFlashcard.BorderBrush = Brushes.Transparent;
            tabFlashcard.Foreground = textGray;
            tabFlashcard.FontWeight = FontWeights.Medium;

            tabQuiz.BorderBrush = Brushes.Transparent;
            tabQuiz.Foreground = textGray;
            tabQuiz.FontWeight = FontWeights.Medium;

            tabDictation.BorderBrush = Brushes.Transparent;
            tabDictation.Foreground = textGray;
            tabDictation.FontWeight = FontWeights.Medium;

            activeBtn.BorderBrush = brandTeal;
            activeBtn.Foreground = brandTeal;
            activeBtn.FontWeight = FontWeights.SemiBold;

            pnlFlashcard.Visibility = Visibility.Collapsed;
            pnlQuiz.Visibility = Visibility.Collapsed;
            pnlDictation.Visibility = Visibility.Collapsed;

            activePnl.Visibility = Visibility.Visible;
        }

        // =========================================================================
        // TAB 1: FLASHCARD VOCABULARY (DATABASE LOADED)
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
                // Fallback nếu chưa có từ theo cấp độ
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

            // Ghi nhận xuống Database
            TuVungBUS.Instance.CapNhatKetQuaFlashcard(card.MaTuVung, daNho);

            // Tăng điểm thưởng phản xạ
            _score += daNho ? 15 : 5;
            txtUserScore.Text = $"Điểm: {_score} pts";

            // Chuyển thẻ tiếp theo
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
        // TAB 2: QUICK REFLEX QUIZ (DATABASE LOADED - TASK-205)
        // =========================================================================
        private void LoadQuizQuestions(string? kyNang = null)
        {
            try
            {
                _quizQuestions = TuVungBUS.Instance.LayCauHoiLuyenTap(kyNang: kyNang, top: 10);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi nạp câu hỏi trắc nghiệm: {ex.Message}");
                _quizQuestions = new List<CauHoiKhaoSatDTO>();
            }
            _currentQuizIndex = 0;
        }

        private void CboQuizSkillFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            if (cboQuizSkillFilter?.SelectedItem is ComboBoxItem item)
            {
                string text = item.Content?.ToString() ?? "";
                string? kyNang = null;
                if (text.Contains("Reading") || text.Contains("Đọc")) kyNang = "Doc";
                else if (text.Contains("Grammar") || text.Contains("Ngữ pháp")) kyNang = "NguPhap";
                else if (text.Contains("Vocab") || text.Contains("Từ vựng")) kyNang = "TuVung";
                else if (text.Contains("Listening") || text.Contains("Nghe")) kyNang = "Nghe";

                LoadQuizQuestions(kyNang);
                StartQuizTimer();
                DisplayCurrentQuiz();
            }
        }

        private void StartQuizTimer()
        {
            _quizTimer?.Stop();
            _quizSecondsRemaining = 60;
            txtQuizTimer.Text = $"⏱️ 00:{_quizSecondsRemaining:D2}s";

            _quizTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _quizTimer.Tick += (s, e) =>
            {
                if (_quizSecondsRemaining > 0)
                {
                    _quizSecondsRemaining--;
                    txtQuizTimer.Text = $"⏱️ 00:{_quizSecondsRemaining:D2}s";
                }
                else
                {
                    _quizTimer.Stop();
                    MessageBox.Show("⏱️ Hết 60 giây phản xạ! Hãy kiểm tra kết quả và chuyển câu hỏi tiếp theo.", "Hết thời gian phản xạ", MessageBoxButton.OK, MessageBoxImage.Information);
                    pnlQuizFeedback.Visibility = Visibility.Visible;
                }
            };
            _quizTimer.Start();
        }

        private void DisplayCurrentQuiz()
        {
            if (!_isInitialized || txtQuizQuestion == null || btnOptA == null) return;
            if (_quizQuestions == null || _quizQuestions.Count == 0)
            {
                txtQuizQuestion.Text = "Hiện chưa có câu hỏi trắc nghiệm trong cơ sở dữ liệu.";
                return;
            }

            if (_currentQuizIndex >= _quizQuestions.Count)
            {
                _currentQuizIndex = 0;
            }

            var q = _quizQuestions[_currentQuizIndex];
            txtQuizScore.Text = $"• Câu {_currentQuizIndex + 1} / {_quizQuestions.Count}";
            txtQuizQuestion.Text = q.NoiDungCauHoi;

            if (!string.IsNullOrWhiteSpace(q.DoanVanDoc))
            {
                pnlQuizPassage.Visibility = Visibility.Visible;
                txtQuizPassage.Text = q.DoanVanDoc;
            }
            else
            {
                pnlQuizPassage.Visibility = Visibility.Collapsed;
            }

            // Gán lựa chọn cho 4 nút A, B, C, D
            ResetQuizOptionButtons();
            var options = q.DanhSachLuaChon;

            btnOptA.Content = options.Count > 0 ? $"A. {options[0].NoiDungLuaChon}" : "A.";
            btnOptA.Tag = options.Count > 0 ? options[0] : null;

            btnOptB.Content = options.Count > 1 ? $"B. {options[1].NoiDungLuaChon}" : "B.";
            btnOptB.Tag = options.Count > 1 ? options[1] : null;

            btnOptC.Content = options.Count > 2 ? $"C. {options[2].NoiDungLuaChon}" : "C.";
            btnOptC.Tag = options.Count > 2 ? options[2] : null;

            btnOptD.Content = options.Count > 3 ? $"D. {options[3].NoiDungLuaChon}" : "D.";
            btnOptD.Tag = options.Count > 3 ? options[3] : null;

            pnlQuizFeedback.Visibility = Visibility.Collapsed;
            _quizAnswered = false;
        }

        private void ResetQuizOptionButtons()
        {
            var defaultBg = Brushes.White;
            var defaultBorder = (SolidColorBrush)FindResource("BorderLight");

            btnOptA.Background = defaultBg; btnOptA.BorderBrush = defaultBorder; btnOptA.IsEnabled = true;
            btnOptB.Background = defaultBg; btnOptB.BorderBrush = defaultBorder; btnOptB.IsEnabled = true;
            btnOptC.Background = defaultBg; btnOptC.BorderBrush = defaultBorder; btnOptC.IsEnabled = true;
            btnOptD.Background = defaultBg; btnOptD.BorderBrush = defaultBorder; btnOptD.IsEnabled = true;
        }

        private void BtnQuizAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (_quizAnswered) return;
            if (sender is not Button clickedBtn || clickedBtn.Tag is not LuaChonDapAnDTO selectedOpt) return;

            _quizAnswered = true;
            var q = _quizQuestions[_currentQuizIndex];

            // Tìm nút đáp án đúng
            Button? correctBtn = null;
            if (btnOptA.Tag is LuaChonDapAnDTO optA && optA.LaDapAnDung) correctBtn = btnOptA;
            else if (btnOptB.Tag is LuaChonDapAnDTO optB && optB.LaDapAnDung) correctBtn = btnOptB;
            else if (btnOptC.Tag is LuaChonDapAnDTO optC && optC.LaDapAnDung) correctBtn = btnOptC;
            else if (btnOptD.Tag is LuaChonDapAnDTO optD && optD.LaDapAnDung) correctBtn = btnOptD;

            if (selectedOpt.LaDapAnDung)
            {
                clickedBtn.Background = new SolidColorBrush(Color.FromRgb(209, 250, 229)); // Xanh nhạt
                clickedBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                _score += 20;
                txtUserScore.Text = $"Điểm: {_score} pts";
                txtQuizFeedback.Text = $"🎉 Chính xác! {q.GiaiThichDapAn ?? "Bạn đã chọn đúng câu trả lời."}";
            }
            else
            {
                clickedBtn.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226)); // Đỏ nhạt
                clickedBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));

                if (correctBtn != null)
                {
                    correctBtn.Background = new SolidColorBrush(Color.FromRgb(209, 250, 229));
                    correctBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                }
                txtQuizFeedback.Text = $"❌ Chưa chính xác. {q.GiaiThichDapAn ?? "Hãy chú ý cấu trúc ngữ pháp và từ vựng của câu."}";
            }

            pnlQuizFeedback.Visibility = Visibility.Visible;
        }

        private void BtnNextQuiz_Click(object sender, RoutedEventArgs e)
        {
            _currentQuizIndex++;
            if (_currentQuizIndex >= _quizQuestions.Count)
            {
                MessageBox.Show($"🏆 Chúc mừng bạn đã hoàn thành {_quizQuestions.Count} câu hỏi phản xạ!\nĐiểm hiện tại: {_score} pts", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                _currentQuizIndex = 0;
            }
            StartQuizTimer();
            DisplayCurrentQuiz();
        }

        // =========================================================================
        // TAB 3: DICTATION - NGHE CHÉP CHÍNH TẢ
        // =========================================================================
        private void InitDictationSentences()
        {
            // Lấy từ vựng thực tế và ví dụ từ DB
            _dictationSentences = new List<string>
            {
                "Smartphones have become ubiquitous in daily modern life.",
                "The research paper required meticulous attention to detail.",
                "Governments must take a pragmatic and balanced approach to sustainability.",
                "Subsequent geological studies corroborated the theory proposed by scientists.",
                "Excessive screen time has a detrimental effect on sleep quality.",
                "The government introduced strict policies to mitigate the adverse impacts of pollution."
            };

            // Nếu DB có thêm câu ví dụ từ flashcards, bổ sung vào
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
                MessageBox.Show("Vui lòng gõ nội dung bạn nghe được trước khi kiểm tra.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
            txtUserScore.Text = $"Điểm: {_score} pts";
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
                MessageBox.Show("🎉 Bạn đã luyện tập hết danh sách câu nghe chép chính tả đợt này!", "Hoàn thành", MessageBoxButton.OK, MessageBoxImage.Information);
                _currentDictationIndex = 0;
            }
            DisplayCurrentDictation();
        }
    }
}
