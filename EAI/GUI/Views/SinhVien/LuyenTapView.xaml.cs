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

        // Overall Score & Init Flag
        private int _score = 150;
        private bool _isInitialized = false;

        // =========================================================================
        // AUDIO ENGINE STATE (MediaPlayer + AI Speech Synthesizer Dual Mode)
        // =========================================================================
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        private DispatcherTimer? _audioTimer;
        private dynamic? _speechSynth = null;
        private bool _isSpeechSynthesizerActive = false;

        // Quiz Audio Player State
        private bool _isPlayingQuizAudio = false;
        private double _quizAudioCurrentSeconds = 0;
        private double _quizAudioTotalSeconds = 30;
        private double _quizAudioSpeed = 1.0;
        private bool _isDraggingQuizSlider = false;

        // Dictation Audio Player State
        private bool _isPlayingDictationAudio = false;
        private double _dictationAudioCurrentSeconds = 0;
        private double _dictationAudioTotalSeconds = 8;
        private double _dictationAudioSpeed = 1.0;
        private bool _isDraggingDictationSlider = false;

        public LuyenTapView()
        {
            InitializeComponent();
            _isInitialized = true;
            InitAudioEngine();
            LoadFlashcards();
            LoadQuizQuestions();
            InitDictationSentences();
            DisplayCurrentQuiz();
            DisplayCurrentDictation();
        }

        // =========================================================================
        // AUDIO ENGINE INITIALIZATION & TIMER
        // =========================================================================
        private void InitAudioEngine()
        {
            _mediaPlayer.MediaOpened += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_mediaPlayer.NaturalDuration.HasTimeSpan && _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                    {
                        double duration = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        if (_isPlayingQuizAudio)
                        {
                            _quizAudioTotalSeconds = duration;
                            if (txtQuizAudioTotalTime != null)
                                txtQuizAudioTotalTime.Text = $"{(int)duration / 60:D2}:{(int)duration % 60:D2}";
                        }
                        else if (_isPlayingDictationAudio)
                        {
                            _dictationAudioTotalSeconds = duration;
                            if (txtDictationTotalTime != null)
                                txtDictationTotalTime.Text = $"{(int)duration / 60:D2}:{(int)duration % 60:D2}";
                        }
                    }
                });
            };

            _mediaPlayer.MediaEnded += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_isPlayingQuizAudio)
                        StopQuizAudio();
                    if (_isPlayingDictationAudio)
                        StopDictationAudio();
                });
            };

            _mediaPlayer.MediaFailed += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"[MediaPlayer] Lỗi media: {e.ErrorException?.Message}");
            };

            try
            {
                var type = Type.GetType("System.Speech.Synthesis.SpeechSynthesizer, System.Speech");
                if (type != null)
                {
                    _speechSynth = Activator.CreateInstance(type);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Speech] Không khởi tạo được synthesizer: {ex.Message}");
            }

            // DispatcherTimer cập nhật thanh tua audio mượt mà
            _audioTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(250)
            };
            _audioTimer.Tick += AudioTimer_Tick;
        }

        private void AudioTimer_Tick(object? sender, EventArgs e)
        {
            // Cập nhật tiến độ Quiz Audio
            if (_isPlayingQuizAudio && !_isDraggingQuizSlider)
            {
                if (!_isSpeechSynthesizerActive)
                {
                    try
                    {
                        if (_mediaPlayer.NaturalDuration.HasTimeSpan && _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                        {
                            _quizAudioCurrentSeconds = _mediaPlayer.Position.TotalSeconds;
                            _quizAudioTotalSeconds = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        }
                        else
                        {
                            _quizAudioCurrentSeconds += 0.25 * _quizAudioSpeed;
                        }
                    }
                    catch
                    {
                        _quizAudioCurrentSeconds += 0.25 * _quizAudioSpeed;
                    }
                }
                else
                {
                    _quizAudioCurrentSeconds += 0.25 * _quizAudioSpeed;
                }

                if (_quizAudioCurrentSeconds >= _quizAudioTotalSeconds)
                {
                    StopQuizAudio();
                    return;
                }

                if (sldQuizAudioProgress != null && _quizAudioTotalSeconds > 0)
                {
                    sldQuizAudioProgress.Value = (_quizAudioCurrentSeconds / _quizAudioTotalSeconds) * 100.0;
                }
                if (txtQuizAudioCurrentTime != null)
                {
                    txtQuizAudioCurrentTime.Text = $"{(int)_quizAudioCurrentSeconds / 60:D2}:{(int)_quizAudioCurrentSeconds % 60:D2}";
                }
            }

            // Cập nhật tiến độ Dictation Audio
            if (_isPlayingDictationAudio && !_isDraggingDictationSlider)
            {
                if (!_isSpeechSynthesizerActive)
                {
                    try
                    {
                        if (_mediaPlayer.NaturalDuration.HasTimeSpan && _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                        {
                            _dictationAudioCurrentSeconds = _mediaPlayer.Position.TotalSeconds;
                            _dictationAudioTotalSeconds = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        }
                        else
                        {
                            _dictationAudioCurrentSeconds += 0.25 * _dictationAudioSpeed;
                        }
                    }
                    catch
                    {
                        _dictationAudioCurrentSeconds += 0.25 * _dictationAudioSpeed;
                    }
                }
                else
                {
                    _dictationAudioCurrentSeconds += 0.25 * _dictationAudioSpeed;
                }

                if (_dictationAudioCurrentSeconds >= _dictationAudioTotalSeconds)
                {
                    StopDictationAudio();
                    return;
                }

                if (sldDictationProgress != null && _dictationAudioTotalSeconds > 0)
                {
                    sldDictationProgress.Value = (_dictationAudioCurrentSeconds / _dictationAudioTotalSeconds) * 100.0;
                }
                if (txtDictationCurrentTime != null)
                {
                    txtDictationCurrentTime.Text = $"{(int)_dictationAudioCurrentSeconds / 60:D2}:{(int)_dictationAudioCurrentSeconds % 60:D2}";
                }
            }
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
        // TAB SWITCHING
        // =========================================================================
        private void TabFlashcard_Click(object sender, RoutedEventArgs e)
        {
            _quizTimer?.Stop();
            StopQuizAudio();
            StopDictationAudio();
            SetActiveTab(tabFlashcard, pnlFlashcard);
        }

        private void TabQuiz_Click(object sender, RoutedEventArgs e)
        {
            StopDictationAudio();
            SetActiveTab(tabQuiz, pnlQuiz);
            StartQuizTimer();
            DisplayCurrentQuiz();
        }

        private void TabDictation_Click(object sender, RoutedEventArgs e)
        {
            _quizTimer?.Stop();
            StopQuizAudio();
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
            txtUserScore.Text = $"Điểm: {_score} pts";

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

                StopQuizAudio();
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
                    Common.ThongBaoDialog.ThongTin("⏱️ Hết 60 giây phản xạ! Hãy kiểm tra kết quả và chuyển câu hỏi tiếp theo.", "Hết thời gian phản xạ");
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
                pnlQuizAudioPlayer.Visibility = Visibility.Collapsed;
                return;
            }

            if (_currentQuizIndex >= _quizQuestions.Count)
            {
                _currentQuizIndex = 0;
            }

            var q = _quizQuestions[_currentQuizIndex];
            txtQuizScore.Text = $"• Câu {_currentQuizIndex + 1} / {_quizQuestions.Count}";
            txtQuizQuestion.Text = q.NoiDungCauHoi;

            // KIỂM TRA HIỂN THỊ AUDIO PLAYER CHO BÀI NGHE
            bool isListeningQuestion = q.CoAudio 
                || string.Equals(q.KyNang, "Nghe", StringComparison.OrdinalIgnoreCase)
                || string.Equals(q.KyNang, "Listening", StringComparison.OrdinalIgnoreCase)
                || !string.IsNullOrWhiteSpace(q.DuongDanAudio)
                || !string.IsNullOrWhiteSpace(q.AudioPath);

            StopQuizAudio();

            if (isListeningQuestion)
            {
                pnlQuizAudioPlayer.Visibility = Visibility.Visible;
                string transcript = !string.IsNullOrWhiteSpace(q.DoanVanNghe) 
                    ? q.DoanVanNghe 
                    : (!string.IsNullOrWhiteSpace(q.DoanVanDoc) ? q.DoanVanDoc : q.NoiDungCauHoi);
                txtQuizTranscript.Text = transcript;
                pnlQuizTranscript.Visibility = Visibility.Collapsed;
                btnToggleQuizTranscript.Content = "📝 Transcript";

                // Tính thời lượng ước lượng
                int wordCount = transcript.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                _quizAudioTotalSeconds = Math.Max(8.0, wordCount / 2.3);
                txtQuizAudioTotalTime.Text = $"{(int)_quizAudioTotalSeconds / 60:D2}:{(int)_quizAudioTotalSeconds % 60:D2}";
                txtQuizAudioCurrentTime.Text = "00:00";
                sldQuizAudioProgress.Value = 0;

                // Ẩn box đoạn văn đọc vì bài nghe hiển thị qua trình phát và transcript
                pnlQuizPassage.Visibility = Visibility.Collapsed;
            }
            else
            {
                pnlQuizAudioPlayer.Visibility = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(q.DoanVanDoc))
                {
                    pnlQuizPassage.Visibility = Visibility.Visible;
                    txtQuizPassage.Text = q.DoanVanDoc;
                }
                else
                {
                    pnlQuizPassage.Visibility = Visibility.Collapsed;
                }
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
                clickedBtn.Background = new SolidColorBrush(Color.FromRgb(209, 250, 229));
                clickedBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                _score += 20;
                txtUserScore.Text = $"Điểm: {_score} pts";
                txtQuizFeedback.Text = $"🎉 Chính xác! {q.GiaiThichDapAn ?? "Bạn đã chọn đúng câu trả lời."}";
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
                txtQuizFeedback.Text = $"❌ Chưa chính xác. {q.GiaiThichDapAn ?? "Hãy chú ý cấu trúc ngữ pháp và từ vựng của câu."}";
            }

            pnlQuizFeedback.Visibility = Visibility.Visible;
        }

        private void BtnNextQuiz_Click(object sender, RoutedEventArgs e)
        {
            StopQuizAudio();
            _currentQuizIndex++;
            if (_currentQuizIndex >= _quizQuestions.Count)
            {
                Common.ThongBaoDialog.ThanhCong($"🏆 Chúc mừng bạn đã hoàn thành {_quizQuestions.Count} câu hỏi phản xạ!\nĐiểm hiện tại: {_score} pts", "Hoàn thành");
                _currentQuizIndex = 0;
            }
            StartQuizTimer();
            DisplayCurrentQuiz();
        }

        // =========================================================================
        // QUIZ AUDIO PLAYER EVENT HANDLERS
        // =========================================================================
        private void BtnQuizAudioPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (_quizQuestions == null || _quizQuestions.Count == 0) return;
            var q = _quizQuestions[_currentQuizIndex];

            // Dừng dictation nếu đang phát
            if (_isPlayingDictationAudio) StopDictationAudio();

            _isPlayingQuizAudio = !_isPlayingQuizAudio;
            if (_isPlayingQuizAudio)
            {
                string? audioPath = !string.IsNullOrWhiteSpace(q.DuongDanAudio) ? q.DuongDanAudio : q.AudioPath;
                bool fileExists = !string.IsNullOrWhiteSpace(audioPath) && (System.IO.File.Exists(audioPath) || Uri.TryCreate(audioPath, UriKind.Absolute, out _));

                if (fileExists)
                {
                    _isSpeechSynthesizerActive = false;
                    try
                    {
                        _mediaPlayer.Open(new Uri(audioPath!, UriKind.RelativeOrAbsolute));
                        _mediaPlayer.Volume = sldQuizAudioVolume?.Value ?? 0.8;
                        _mediaPlayer.SpeedRatio = _quizAudioSpeed;
                        _mediaPlayer.Position = TimeSpan.FromSeconds(_quizAudioCurrentSeconds);
                        _mediaPlayer.Play();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[MediaPlayer] Lỗi mở tệp: {ex.Message}");
                        PlayQuizSpeechFallback(q);
                    }
                }
                else
                {
                    PlayQuizSpeechFallback(q);
                }

                btnQuizAudioPlayPause.Content = "⏸️ Tạm dừng";
                txtQuizAudioStatus.Text = "🎧 ĐANG PHÁT BÀI NGHE (AI NATIVE SPEAKER • IPA)";
                _audioTimer?.Start();
            }
            else
            {
                PauseQuizAudio();
            }
        }

        private void PlayQuizSpeechFallback(CauHoiKhaoSatDTO q)
        {
            _isSpeechSynthesizerActive = true;
            string text = !string.IsNullOrWhiteSpace(q.DoanVanNghe) 
                ? q.DoanVanNghe 
                : (!string.IsNullOrWhiteSpace(q.DoanVanDoc) ? q.DoanVanDoc : q.NoiDungCauHoi);

            text = text.Replace("Audio Transcript:", "").Replace("Audio Transcript", "").Trim();

            int wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            _quizAudioTotalSeconds = Math.Max(6.0, (wordCount / 2.3) / _quizAudioSpeed);
            if (txtQuizAudioTotalTime != null)
            {
                txtQuizAudioTotalTime.Text = $"{(int)_quizAudioTotalSeconds / 60:D2}:{(int)_quizAudioTotalSeconds % 60:D2}";
            }

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    if (_speechSynth != null)
                    {
                        _speechSynth.SpeakAsyncCancelAll();
                        _speechSynth.Volume = (int)((sldQuizAudioVolume?.Value ?? 0.8) * 100);
                        _speechSynth.Rate = (int)((_quizAudioSpeed - 1.0) * 4);
                        _speechSynth.SpeakAsync(text);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Speech] Lỗi: {ex.Message}");
                }
            });
        }

        private void PauseQuizAudio()
        {
            _isPlayingQuizAudio = false;
            try
            {
                _mediaPlayer.Pause();
            }
            catch { }
            try
            {
                if (_speechSynth != null)
                {
                    _speechSynth.SpeakAsyncCancelAll();
                }
            }
            catch { }
            btnQuizAudioPlayPause.Content = "▶️ Tiếp tục nghe";
            txtQuizAudioStatus.Text = "⏸️ Đã tạm dừng bài nghe";
        }

        private void StopQuizAudio()
        {
            _isPlayingQuizAudio = false;
            try
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Position = TimeSpan.Zero;
            }
            catch { }
            try
            {
                if (_speechSynth != null)
                {
                    _speechSynth.SpeakAsyncCancelAll();
                }
            }
            catch { }

            _quizAudioCurrentSeconds = 0;
            if (sldQuizAudioProgress != null) sldQuizAudioProgress.Value = 0;
            if (txtQuizAudioCurrentTime != null) txtQuizAudioCurrentTime.Text = "00:00";
            if (btnQuizAudioPlayPause != null) btnQuizAudioPlayPause.Content = "▶️ Nghe bài tập";
            if (txtQuizAudioStatus != null) txtQuizAudioStatus.Text = "🎧 BÀI TẬP NGHE HIỂU (LISTENING COMPREHENSION)";
        }

        private void BtnQuizAudioStop_Click(object sender, RoutedEventArgs e)
        {
            StopQuizAudio();
        }

        private void BtnQuizAudioRewind_Click(object sender, RoutedEventArgs e)
        {
            _quizAudioCurrentSeconds = Math.Max(0, _quizAudioCurrentSeconds - 5);
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_quizAudioCurrentSeconds);
            }
            catch { }
            if (sldQuizAudioProgress != null && _quizAudioTotalSeconds > 0)
            {
                sldQuizAudioProgress.Value = (_quizAudioCurrentSeconds / _quizAudioTotalSeconds) * 100.0;
            }
            if (txtQuizAudioCurrentTime != null)
            {
                txtQuizAudioCurrentTime.Text = $"{(int)_quizAudioCurrentSeconds / 60:D2}:{(int)_quizAudioCurrentSeconds % 60:D2}";
            }
        }

        private void BtnQuizAudioForward_Click(object sender, RoutedEventArgs e)
        {
            _quizAudioCurrentSeconds = Math.Min(_quizAudioTotalSeconds, _quizAudioCurrentSeconds + 5);
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_quizAudioCurrentSeconds);
            }
            catch { }
            if (sldQuizAudioProgress != null && _quizAudioTotalSeconds > 0)
            {
                sldQuizAudioProgress.Value = (_quizAudioCurrentSeconds / _quizAudioTotalSeconds) * 100.0;
            }
            if (txtQuizAudioCurrentTime != null)
            {
                txtQuizAudioCurrentTime.Text = $"{(int)_quizAudioCurrentSeconds / 60:D2}:{(int)_quizAudioCurrentSeconds % 60:D2}";
            }
        }

        private void SldQuizAudioVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldQuizAudioVolume != null)
            {
                try
                {
                    _mediaPlayer.Volume = Math.Clamp(sldQuizAudioVolume.Value, 0.0, 1.0);
                }
                catch { }
            }
        }

        private void BtnQuizAudioSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (_quizAudioSpeed == 1.0) _quizAudioSpeed = 1.25;
            else if (_quizAudioSpeed == 1.25) _quizAudioSpeed = 0.75;
            else if (_quizAudioSpeed == 0.75) _quizAudioSpeed = 0.5;
            else _quizAudioSpeed = 1.0;

            try
            {
                _mediaPlayer.SpeedRatio = _quizAudioSpeed;
            }
            catch { }

            if (btnQuizAudioSpeed != null)
                btnQuizAudioSpeed.Content = $"{_quizAudioSpeed:0.00}x";

            if (_isPlayingQuizAudio)
            {
                PauseQuizAudio();
                BtnQuizAudioPlayPause_Click(sender, e);
            }
        }

        private void BtnToggleQuizTranscript_Click(object sender, RoutedEventArgs e)
        {
            if (pnlQuizTranscript.Visibility == Visibility.Visible)
            {
                pnlQuizTranscript.Visibility = Visibility.Collapsed;
                btnToggleQuizTranscript.Content = "📝 Transcript";
            }
            else
            {
                pnlQuizTranscript.Visibility = Visibility.Visible;
                btnToggleQuizTranscript.Content = "🙈 Ẩn Transcript";
            }
        }

        private void SldQuizAudioProgress_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingQuizSlider = true;
        }

        private void SldQuizAudioProgress_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingQuizSlider = false;
            _quizAudioCurrentSeconds = (sldQuizAudioProgress.Value / 100.0) * _quizAudioTotalSeconds;
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_quizAudioCurrentSeconds);
            }
            catch { }
        }

        private void SldQuizAudioProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraggingQuizSlider)
            {
                _quizAudioCurrentSeconds = (sldQuizAudioProgress.Value / 100.0) * _quizAudioTotalSeconds;
                if (txtQuizAudioCurrentTime != null)
                {
                    txtQuizAudioCurrentTime.Text = $"{(int)_quizAudioCurrentSeconds / 60:D2}:{(int)_quizAudioCurrentSeconds % 60:D2}";
                }
            }
        }

        // =========================================================================
        // TAB 3: DICTATION - NGHE CHÉP CHÍNH TẢ
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

            StopDictationAudio();

            txtDictationIndex.Text = $"Câu {_currentDictationIndex + 1} / {_dictationSentences.Count}";
            txtDictationInput.Text = string.Empty;
            bdrDictationResult.Visibility = Visibility.Collapsed;

            string sentence = _dictationSentences[_currentDictationIndex];
            int wordCount = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            _dictationAudioTotalSeconds = Math.Max(4.0, wordCount / 1.8);
            if (txtDictationTotalTime != null)
            {
                txtDictationTotalTime.Text = $"{(int)_dictationAudioTotalSeconds / 60:D2}:{(int)_dictationAudioTotalSeconds % 60:D2}";
            }
            if (txtDictationCurrentTime != null) txtDictationCurrentTime.Text = "00:00";
            if (sldDictationProgress != null) sldDictationProgress.Value = 0;
        }

        // =========================================================================
        // DICTATION AUDIO PLAYER EVENT HANDLERS
        // =========================================================================
        private void BtnDictationPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlayingDictationAudio)
            {
                StopDictationAudio();
            }
            else
            {
                PlayDictationWithSpeed(1.0);
            }
        }

        private void BtnDictationSlow_Click(object sender, RoutedEventArgs e)
        {
            PlayDictationWithSpeed(0.75);
        }

        private void BtnDictationSlower_Click(object sender, RoutedEventArgs e)
        {
            PlayDictationWithSpeed(0.5);
        }

        private void PlayDictationWithSpeed(double speed)
        {
            if (_dictationSentences == null || _dictationSentences.Count == 0) return;
            if (_isPlayingQuizAudio) StopQuizAudio();

            _dictationAudioSpeed = speed;
            string sentence = _dictationSentences[_currentDictationIndex];

            StopDictationAudio();

            _isPlayingDictationAudio = true;
            _isSpeechSynthesizerActive = true;

            int wordCount = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            _dictationAudioTotalSeconds = Math.Max(4.0, (wordCount / 1.8) / speed);
            if (txtDictationTotalTime != null)
            {
                txtDictationTotalTime.Text = $"{(int)_dictationAudioTotalSeconds / 60:D2}:{(int)_dictationAudioTotalSeconds % 60:D2}";
            }

            btnDictationPlayPause.Content = "⏸️ Tạm dừng";
            txtDictationAudioStatus.Text = $"🎧 ĐANG PHÁT CÂU MẪU ({speed:0.00}X • IPA CHUẨN)";

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    if (_speechSynth != null)
                    {
                        _speechSynth.SpeakAsyncCancelAll();
                        _speechSynth.Volume = (int)((sldDictationVolume?.Value ?? 0.85) * 100);
                        _speechSynth.Rate = (int)((speed - 1.0) * 4);
                        _speechSynth.SpeakAsync(sentence);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Dictation Speech] Lỗi: {ex.Message}");
                }
            });

            _audioTimer?.Start();
        }

        private void StopDictationAudio()
        {
            _isPlayingDictationAudio = false;
            try
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Position = TimeSpan.Zero;
            }
            catch { }
            try
            {
                if (_speechSynth != null)
                {
                    _speechSynth.SpeakAsyncCancelAll();
                }
            }
            catch { }

            _dictationAudioCurrentSeconds = 0;
            if (sldDictationProgress != null) sldDictationProgress.Value = 0;
            if (txtDictationCurrentTime != null) txtDictationCurrentTime.Text = "00:00";
            if (btnDictationPlayPause != null) btnDictationPlayPause.Content = "▶️ Nghe câu mẫu";
            if (txtDictationAudioStatus != null) txtDictationAudioStatus.Text = "🎧 AUDIO DICTATION (GIỌNG ĐỌC BẢN XỨ CHUẨN IPA)";
        }

        private void BtnDictationStop_Click(object sender, RoutedEventArgs e)
        {
            StopDictationAudio();
        }

        private void BtnDictationRepeat_Click(object sender, RoutedEventArgs e)
        {
            PlayDictationWithSpeed(_dictationAudioSpeed);
        }

        private void SldDictationVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldDictationVolume != null)
            {
                try
                {
                    _mediaPlayer.Volume = Math.Clamp(sldDictationVolume.Value, 0.0, 1.0);
                }
                catch { }
            }
        }

        private void SldDictationProgress_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingDictationSlider = true;
        }

        private void SldDictationProgress_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingDictationSlider = false;
            _dictationAudioCurrentSeconds = (sldDictationProgress.Value / 100.0) * _dictationAudioTotalSeconds;
        }

        private void SldDictationProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraggingDictationSlider)
            {
                _dictationAudioCurrentSeconds = (sldDictationProgress.Value / 100.0) * _dictationAudioTotalSeconds;
                if (txtDictationCurrentTime != null)
                {
                    txtDictationCurrentTime.Text = $"{(int)_dictationAudioCurrentSeconds / 60:D2}:{(int)_dictationAudioCurrentSeconds % 60:D2}";
                }
            }
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
            StopDictationAudio();
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
