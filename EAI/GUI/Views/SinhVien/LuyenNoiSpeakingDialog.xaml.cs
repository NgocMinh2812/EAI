using System;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using BUS.AI;
using DTO;
using GUI.Views.Common;

namespace GUI.Views.SinhVien
{
    public partial class LuyenNoiSpeakingDialog : Window
    {
        private readonly string _targetSentence;
        private readonly string? _translation;
        private readonly MicrophoneRecorder _recorder = new();
        private readonly MediaPlayer _ownPlayer = new();
        private SpeechSynthesizer? _speechSynth;
        private string? _recordedWavPath;
        private bool _isRecording;
        private bool _isPlayingOwn;

        public LuyenNoiSpeakingDialog(string targetSentence, string? translation = null)
        {
            InitializeComponent();
            _targetSentence = targetSentence ?? string.Empty;
            _translation = translation;

            txtTargetSentence.Text = _targetSentence;
            txtTranslation.Text = string.IsNullOrEmpty(_translation) ? "" : _translation;
            txtTranslation.Visibility = string.IsNullOrEmpty(_translation) ? Visibility.Collapsed : Visibility.Visible;

            _ownPlayer.MediaEnded += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _isPlayingOwn = false;
                    btnPlayOwnRecording.Content = "▶ Nghe lại bản ghi";
                });
            };

            Closed += LuyenNoiSpeakingDialog_Closed;
        }

        private void LuyenNoiSpeakingDialog_Closed(object? sender, EventArgs e)
        {
            try { _ownPlayer.Close(); } catch { }
            try { _speechSynth?.Dispose(); } catch { }
            _recorder.CleanupTempFile();
        }

        private void BtnListenSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _speechSynth ??= new SpeechSynthesizer();
                _speechSynth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                _speechSynth.SpeakAsyncCancelAll();
                _speechSynth.SpeakAsync(_targetSentence);
            }
            catch (Exception ex)
            {
                ThongBaoDialog.ThongTin(this, $"Không thể phát âm mẫu trên thiết bị: {ex.Message}", "Thông báo");
            }
        }

        private async void BtnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!_isRecording)
            {
                // BẮT ĐẦU GHI ÂM
                bool started = _recorder.StartRecording(out string err);
                if (started)
                {
                    _isRecording = true;
                    btnRecord.Content = "⏹ Dừng & Gửi phân tích";
                    btnRecord.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                    txtRecordStatus.Text = "🔴 Đang ghi âm... Hãy đọc câu mẫu vào micro của bạn!";
                    txtRecordStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                    btnPlayOwnRecording.IsEnabled = false;
                }
                else
                {
                    ThongBaoDialog.CanhBao(this, $"Không thể khởi động microphone để ghi âm: {err}", "Không thể ghi âm");
                }
            }
            else
            {
                // DỪNG GHI ÂM VÀ GỬI PHÂN TÍCH
                bool stopped = _recorder.StopRecording(out string recordedFile, out string err);
                _isRecording = false;
                btnRecord.Content = "🎙️ Đọc lại lần nữa";
                btnRecord.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F766E"));
                txtRecordStatus.Text = "⏳ Đang gửi file âm thanh tới Groq STT để phân tích...";
                txtRecordStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F766E"));

                if (!stopped || string.IsNullOrEmpty(recordedFile))
                {
                    txtRecordStatus.Text = $"✗ Không thu được âm thanh: {err}";
                    txtRecordStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                    return;
                }

                _recordedWavPath = recordedFile;
                btnPlayOwnRecording.IsEnabled = true;

                // Chuẩn bị player nghe lại
                try
                {
                    _ownPlayer.Open(new Uri(_recordedWavPath));
                }
                catch { }

                // GỬI SANG GROQ SPEECH-TO-TEXT
                btnRecord.IsEnabled = false;
                try
                {
                    var (success, result, sttErr) = await GroqAudioService.Instance.TranscribeAudioAsync(
                        _recordedWavPath, 
                        model: GroqAudioService.DefaultSttModel, 
                        language: "en", 
                        forceRefresh: true);

                    if (success && result != null && !string.IsNullOrWhiteSpace(result.TranscriptText))
                    {
                        txtRecordStatus.Text = "✓ Đã nhận dạng và đối chiếu hoàn tất!";
                        txtRecordStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));
                        HienThiKetQuaDanhGia(result.TranscriptText);
                    }
                    else
                    {
                        txtRecordStatus.Text = $"✗ Nhận dạng thất bại: {sttErr}";
                        txtRecordStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                        ThongBaoDialog.CanhBao(this, $"Chưa nhận dạng được giọng nói của bạn:\n\n{sttErr}", "Chưa nhận diện được");
                    }
                }
                catch (Exception ex)
                {
                    txtRecordStatus.Text = $"✗ Lỗi phân tích: {ex.Message}";
                    txtRecordStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                }
                finally
                {
                    btnRecord.IsEnabled = true;
                }
            }
        }

        private void HienThiKetQuaDanhGia(string recognizedText)
        {
            txtRecognizedSpeech.Text = $"\"{recognizedText}\"";

            var eval = SpeechComparisonHelper.EvaluateSpokenSentence(_targetSentence, recognizedText);

            txtAccuracyScore.Text = $"{eval.MatchPercentage:F0}%";
            txtEvaluationComment.Text = eval.FeedbackComment;

            // Đổi màu badge điểm
            if (eval.MatchPercentage >= 80)
            {
                bdrAccuracyScore.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCFCE7"));
                txtAccuracyScore.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));
            }
            else if (eval.MatchPercentage >= 50)
            {
                bdrAccuracyScore.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF3C7"));
                txtAccuracyScore.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D97706"));
            }
            else
            {
                bdrAccuracyScore.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEE2E2"));
                txtAccuracyScore.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
            }

            // Hiển thị từng từ dưới dạng chip
            pnlWordChips.Children.Clear();
            foreach (var w in eval.WordDetails)
            {
                var border = new Border
                {
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(8, 4, 8, 4),
                    Margin = new Thickness(0, 0, 6, 6)
                };

                var sp = new StackPanel { Orientation = Orientation.Vertical };
                var txtWord = new TextBlock
                {
                    Text = w.TargetWord,
                    FontSize = 13,
                    FontWeight = FontWeights.Bold
                };
                var txtBadge = new TextBlock
                {
                    Text = w.StatusBadge,
                    FontSize = 9.5,
                    FontWeight = FontWeights.SemiBold
                };

                switch (w.Status)
                {
                    case SpokenWordStatus.Correct:
                        border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5"));
                        border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A7F3D0"));
                        border.BorderThickness = new Thickness(1);
                        txtWord.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#047857"));
                        txtBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#047857"));
                        break;
                    case SpokenWordStatus.Missing:
                        border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF2F2"));
                        border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FECACA"));
                        border.BorderThickness = new Thickness(1);
                        txtWord.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B91C1C"));
                        txtBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B91C1C"));
                        break;
                    default:
                        border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFBEB"));
                        border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDE68A"));
                        border.BorderThickness = new Thickness(1);
                        txtWord.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B45309"));
                        txtBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B45309"));
                        break;
                }

                sp.Children.Add(txtWord);
                sp.Children.Add(txtBadge);
                border.Child = sp;
                pnlWordChips.Children.Add(border);
            }
        }

        private void BtnPlayOwnRecording_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_recordedWavPath) || !File.Exists(_recordedWavPath)) return;

            if (_isPlayingOwn)
            {
                _ownPlayer.Pause();
                _isPlayingOwn = false;
                btnPlayOwnRecording.Content = "▶ Nghe lại bản ghi";
            }
            else
            {
                _ownPlayer.Position = TimeSpan.Zero;
                _ownPlayer.Play();
                _isPlayingOwn = true;
                btnPlayOwnRecording.Content = "⏸ Tạm dừng";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

