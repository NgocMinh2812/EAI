using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using BUS;
using BUS.AI;
using DAL;
using DTO;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class QuanLyAudioTranscriptDialog : Window
    {
        private readonly BaiHocDTO _baiHoc;
        private readonly MediaPlayer _sttPlayer = new();
        private readonly MediaPlayer _ttsPlayer = new();
        private readonly DispatcherTimer _sttTimer = new();
        private bool _isSttSeeking;
        private bool _isSttPlaying;
        private bool _isTtsPlaying;

        private AudioTranscriptDTO? _currentTranscriptDto;
        private AudioTranscriptDTO? _generatedTtsDto;

        public QuanLyAudioTranscriptDialog(BaiHocDTO baiHoc)
        {
            InitializeComponent();
            _baiHoc = baiHoc ?? throw new ArgumentNullException(nameof(baiHoc));

            txtLessonTitle.Text = $"Bài học: {_baiHoc.TieuDe} (Trình độ {_baiHoc.MaTrinhDo})";
            CapNhatThongTinAudioNguon();

            _sttTimer.Interval = TimeSpan.FromMilliseconds(250);
            _sttTimer.Tick += SttTimer_Tick;

            _sttPlayer.MediaOpened += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_sttPlayer.NaturalDuration.HasTimeSpan)
                    {
                        var ts = _sttPlayer.NaturalDuration.TimeSpan;
                        sldSttTimeline.Maximum = ts.TotalSeconds;
                        txtSttTotalTime.Text = FormatTime(ts);
                    }
                });
            };

            _sttPlayer.MediaEnded += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _isSttPlaying = false;
                    btnPlayAudio.Content = "▶️ Nghe file âm thanh";
                    _sttTimer.Stop();
                    sldSttTimeline.Value = 0;
                    txtSttCurrentTime.Text = "00:00";
                });
            };

            _ttsPlayer.MediaEnded += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _isTtsPlaying = false;
                    btnPreviewTts.Content = "▶️ Nghe thử bài vừa tạo";
                });
            };

            Loaded += QuanLyAudioTranscriptDialog_Loaded;
            Closed += (s, e) =>
            {
                try { _sttPlayer.Close(); } catch { }
                try { _ttsPlayer.Close(); } catch { }
                _sttTimer.Stop();
            };
        }

        private void CapNhatThongTinAudioNguon()
        {
            if (string.IsNullOrWhiteSpace(_baiHoc.AudioPath) && string.IsNullOrWhiteSpace(_baiHoc.DuongDanAudio))
            {
                txtAudioFileInfo.Text = "File âm thanh bài học: Chưa có file đính kèm";
            }
            else
            {
                string path = !string.IsNullOrWhiteSpace(_baiHoc.AudioPath) ? _baiHoc.AudioPath : _baiHoc.DuongDanAudio!;
                txtAudioFileInfo.Text = $"File âm thanh bài học: {Path.GetFileName(path)}";
            }
        }

        private void QuanLyAudioTranscriptDialog_Loaded(object sender, RoutedEventArgs e)
        {
            NapTranscriptDaLuu();
            NapScriptMacDinhTuBaiHoc();
            CapNhatGiaoDienTheoCheDo();
        }

        private void NapTranscriptDaLuu()
        {
            if (_baiHoc.MaBaiHoc <= 0) return;

            // Kiểm tra trong DB theo mã bài học
            _currentTranscriptDto = AudioTranscriptDAL.Instance.LayAudioChinhCuaBaiHoc(_baiHoc.MaBaiHoc);

            // Hoặc kiểm tra theo AudioPath nếu chưa có
            if (_currentTranscriptDto == null && !string.IsNullOrEmpty(_baiHoc.AudioPath))
            {
                _currentTranscriptDto = AudioTranscriptDAL.Instance.LayTheoDuongDanAudio(_baiHoc.AudioPath);
            }

            if (_currentTranscriptDto != null)
            {
                txtFullTranscript.Text = _currentTranscriptDto.TranscriptText ?? string.Empty;
                lstSegments.ItemsSource = _currentTranscriptDto.Segments;
                chkAllowStudentView.IsChecked = _currentTranscriptDto.ChoPhepSinhVienXem;
                CapNhatBadgeTrangThai(_currentTranscriptDto.TrangThaiDuyet);
            }
            else
            {
                CapNhatBadgeTrangThai("ChuaCo");
            }

            // Mở audio nếu có file
            string? audioPath = !string.IsNullOrEmpty(_baiHoc.AudioPath) ? _baiHoc.AudioPath : _baiHoc.DuongDanAudio;
            if (!string.IsNullOrEmpty(audioPath) && File.Exists(audioPath))
            {
                try
                {
                    _sttPlayer.Open(new Uri(audioPath));
                }
                catch { }
            }
        }

        private void NapScriptMacDinhTuBaiHoc()
        {
            if (string.IsNullOrWhiteSpace(txtTtsScript.Text))
            {
                if (!string.IsNullOrWhiteSpace(_baiHoc.NoiDung))
                {
                    txtTtsScript.Text = _baiHoc.NoiDung.Trim();
                }
                else if (!string.IsNullOrWhiteSpace(_baiHoc.TomTat))
                {
                    txtTtsScript.Text = _baiHoc.TomTat.Trim();
                }
            }
        }

        private void CapNhatBadgeTrangThai(string status)
        {
            switch (status)
            {
                case "DaDuyet":
                    bdrTranscriptStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCFCE7"));
                    txtTranscriptStatus.Text = "✓ Đã duyệt cho học viên xem";
                    txtTranscriptStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));
                    break;
                case "ChoDuyet":
                    bdrTranscriptStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF3C7"));
                    txtTranscriptStatus.Text = "⏳ Bản nháp đang xem lại";
                    txtTranscriptStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92400E"));
                    break;
                default:
                    bdrTranscriptStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9"));
                    txtTranscriptStatus.Text = "Chưa có bản chép lời";
                    txtTranscriptStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
                    break;
            }
        }

        #region Speech-to-Text Events

        private void BtnPlayAudio_Click(object sender, RoutedEventArgs e)
        {
            string? audioPath = !string.IsNullOrEmpty(_baiHoc.AudioPath) ? _baiHoc.AudioPath : _baiHoc.DuongDanAudio;
            if (string.IsNullOrEmpty(audioPath) || !File.Exists(audioPath))
            {
                ThongBaoDialog.ThongTin(this, 
                    "Bài học này hiện chưa có file âm thanh trên máy. Bạn có thể sang thẻ 'Tạo bài nghe mới bằng giọng đọc AI' để tạo file âm thanh bài học nhé!", 
                    "Chưa có file âm thanh");
                return;
            }

            if (_isSttPlaying)
            {
                _sttPlayer.Pause();
                _isSttPlaying = false;
                _sttTimer.Stop();
                btnPlayAudio.Content = "▶️ Nghe file âm thanh";
            }
            else
            {
                _sttPlayer.Play();
                _isSttPlaying = true;
                _sttTimer.Start();
                btnPlayAudio.Content = "⏸ Tạm dừng";
            }
        }

        private void SttTimer_Tick(object? sender, EventArgs e)
        {
            if (!_isSttSeeking && _sttPlayer.NaturalDuration.HasTimeSpan)
            {
                double pos = _sttPlayer.Position.TotalSeconds;
                sldSttTimeline.Value = pos;
                txtSttCurrentTime.Text = FormatTime(_sttPlayer.Position);
            }
        }

        private void SldSttTimeline_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isSttSeeking = true;
        }

        private void SldSttTimeline_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isSttSeeking = false;
            _sttPlayer.Position = TimeSpan.FromSeconds(sldSttTimeline.Value);
            txtSttCurrentTime.Text = FormatTime(_sttPlayer.Position);
        }

        private async void BtnGenerateStt_Click(object sender, RoutedEventArgs e)
        {
            await ThucHienTaoStt(forceRefresh: false);
        }

        private async void BtnRefreshStt_Click(object sender, RoutedEventArgs e)
        {
            await ThucHienTaoStt(forceRefresh: true);
        }

        private async System.Threading.Tasks.Task ThucHienTaoStt(bool forceRefresh)
        {
            string? audioPath = !string.IsNullOrEmpty(_baiHoc.AudioPath) ? _baiHoc.AudioPath : _baiHoc.DuongDanAudio;
            if (string.IsNullOrEmpty(audioPath) || !File.Exists(audioPath))
            {
                ThongBaoDialog.CanhBao(this, 
                    "Vui lòng đảm bảo bài học đã có file âm thanh hợp lệ trước khi yêu cầu nhận diện lời thoại.", 
                    "Chưa có file âm thanh");
                return;
            }

            btnGenerateStt.IsEnabled = false;
            btnRefreshStt.IsEnabled = false;
            txtGlobalMessage.Text = "⏳ Đang trích xuất bản chép lời từ file âm thanh bài học...";
            txtGlobalMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F766E"));

            try
            {
                var (success, result, err) = await GroqAudioService.Instance.TranscribeAudioAsync(
                    audioPath, 
                    _baiHoc.MaBaiHoc, 
                    GroqAudioService.DefaultSttModel, 
                    "en", 
                    forceRefresh);

                if (success && result != null)
                {
                    _currentTranscriptDto = result;
                    txtFullTranscript.Text = result.TranscriptText ?? string.Empty;
                    lstSegments.ItemsSource = result.Segments;
                    CapNhatBadgeTrangThai(result.TrangThaiDuyet);

                    txtGlobalMessage.Text = "✓ Đã nhận diện lời bài nghe thành công! Bạn có thể nghe đối chiếu và bấm 'Phê duyệt & Áp dụng'.";
                    txtGlobalMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));

                    ThongBaoDialog.ThanhCong(this, 
                        "Đã trích xuất nội dung bài nghe thành công kèm theo thời điểm của từng câu thoại!\nBạn có thể nghe kiểm tra và bấm 'Phê duyệt & Áp dụng cho bài học'.", 
                        "Nhận diện thành công");
                }
                else
                {
                    txtGlobalMessage.Text = $"✗ Thất bại: {err}";
                    txtGlobalMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                    ThongBaoDialog.Loi(this, $"Chưa thể trích xuất bản chép lời từ file âm thanh:\n\n{err}", "Không thể nhận diện");
                }
            }
            catch (Exception ex)
            {
                txtGlobalMessage.Text = $"✗ Lỗi: {ex.Message}";
                txtGlobalMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                ThongBaoDialog.Loi(this, $"Đã xảy ra lỗi trong quá trình xử lý: {ex.Message}", "Lỗi hệ thống");
            }
            finally
            {
                btnGenerateStt.IsEnabled = true;
                btnRefreshStt.IsEnabled = true;
            }
        }

        private void LstSegments_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstSegments.SelectedItem is TranscriptSegmentItemDTO segment)
            {
                _sttPlayer.Position = TimeSpan.FromSeconds(segment.Start);
                _sttPlayer.Play();
                _isSttPlaying = true;
                _sttTimer.Start();
                btnPlayAudio.Content = "⏸ Tạm dừng";
                sldSttTimeline.Value = segment.Start;
                txtSttCurrentTime.Text = FormatTime(TimeSpan.FromSeconds(segment.Start));
            }
        }

        private void BtnMarkApproved_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullTranscript.Text))
            {
                ThongBaoDialog.CanhBao(this, 
                    "Nội dung bản chép lời hiện đang để trống. Bạn hãy bấm nhận diện lời bài nghe hoặc nhập văn bản trước khi phê duyệt nhé!", 
                    "Chưa có nội dung");
                return;
            }

            string audioPath = !string.IsNullOrEmpty(_baiHoc.AudioPath) ? _baiHoc.AudioPath : (_baiHoc.DuongDanAudio ?? string.Empty);

            if (_currentTranscriptDto == null)
            {
                _currentTranscriptDto = new AudioTranscriptDTO
                {
                    MaBaiHoc = _baiHoc.MaBaiHoc,
                    AudioPath = audioPath,
                    LoaiAudio = "Uploaded",
                    TieuDeAudio = Path.GetFileName(audioPath),
                    TranscriptText = txtFullTranscript.Text.Trim(),
                    Provider = "Groq",
                    TrangThaiDuyet = "DaDuyet",
                    ChoPhepSinhVienXem = chkAllowStudentView.IsChecked ?? true
                };
                int id = AudioTranscriptDAL.Instance.LuuAudioTranscript(_currentTranscriptDto);
                _currentTranscriptDto.MaAudioTranscript = id;
            }
            else
            {
                _currentTranscriptDto.TranscriptText = txtFullTranscript.Text.Trim();
                _currentTranscriptDto.TrangThaiDuyet = "DaDuyet";
                _currentTranscriptDto.ChoPhepSinhVienXem = chkAllowStudentView.IsChecked ?? true;
                AudioTranscriptDAL.Instance.LuuAudioTranscript(_currentTranscriptDto);
            }

            CapNhatBadgeTrangThai("DaDuyet");
            txtGlobalMessage.Text = "✓ Đã phê duyệt và lưu bản chép lời chính thức vào bài học!";
            txtGlobalMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));

            ThongBaoDialog.ThanhCong(this, 
                "Bản chép lời đã được phê duyệt và lưu vào bài học thành công!\nHọc viên sẽ có thể vừa nghe vừa đọc tương tác khi mở bài học này.", 
                "Phê duyệt thành công");
        }

        #endregion

        #region Text-to-Speech Events & Multi-Speaker Configuration

        private void CboAudioMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CapNhatGiaoDienTheoCheDo();
        }

        private void CapNhatGiaoDienTheoCheDo()
        {
            if (cboAudioMode == null || pnlSoloConfig == null || pnlDialogueConfig == null) return;

            int mode = cboAudioMode.SelectedIndex; // 0: Solo, 1: Dialogue2, 2: Dialogue3
            if (mode == 0)
            {
                pnlSoloConfig.Visibility = Visibility.Visible;
                pnlDialogueConfig.Visibility = Visibility.Collapsed;
            }
            else
            {
                pnlSoloConfig.Visibility = Visibility.Collapsed;
                pnlDialogueConfig.Visibility = Visibility.Visible;

                bool is3 = (mode == 2);
                if (bdrSpeaker3 != null) bdrSpeaker3.Visibility = is3 ? Visibility.Visible : Visibility.Collapsed;
                if (colSpeaker3 != null) colSpeaker3.Width = is3 ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            }
        }

        private void TxtTtsScript_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCharCount.Text = $"{txtTtsScript.Text.Length} ký tự";
        }

        private void BtnFillSampleScript_Click(object sender, RoutedEventArgs e)
        {
            int mode = cboAudioMode?.SelectedIndex ?? 1;

            if (mode == 0)
            {
                // Solo Mode
                txtTtsScript.Text = "Traveling is one of the most rewarding experiences in life. It allows us to explore new cultures, meet diverse people, and taste delicious local cuisines. Whether you are walking through the historic streets of Hanoi or relaxing on a sunny beach in Da Nang, every journey creates unforgettable memories and helps broaden your perspective on the world.";
            }
            else if (mode == 1)
            {
                // Dialogue 2 persons
                txtSpeaker1Name.Text = "Sarah";
                txtSpeaker2Name.Text = "Mark";
                txtTtsScript.Text = "Sarah: Hey Mark, do you have a minute? I've been thinking about our weekend trip.\n" +
                                   "Mark: Sure, Sarah! I was just checking the weather forecast. It looks like it's going to be sunny and warm.\n" +
                                   "Sarah: That's perfect. Have you decided where we should stay in town?\n" +
                                   "Mark: Yes, I found a lovely hotel near the beach. We can book it online right now.\n" +
                                   "Sarah: Wonderful! Let's finalize the booking so we can relax on Saturday.\n" +
                                   "Mark: Agreed. I'll send you the reservation details right away!";
            }
            else
            {
                // Dialogue 3 persons
                txtSpeaker1Name.Text = "Sarah";
                txtSpeaker2Name.Text = "Mark";
                txtSpeaker3Name.Text = "David";
                txtTtsScript.Text = "Sarah: Hi Mark! Hi David! Are you both ready for our group presentation tomorrow?\n" +
                                   "Mark: Yes, Sarah. I've finished the slides for the introduction and market analysis.\n" +
                                   "David: And I just completed the summary and conclusion. Everything looks great!\n" +
                                   "Sarah: That's fantastic teamwork. Let's do a quick rehearsal together before class.\n" +
                                   "Mark: Sounds like a plan. Let's practice from the first slide.\n" +
                                   "David: I agree. I'm ready whenever you two are!";
            }

            ThongBaoDialog.ThongTin(this, 
                "Đã điền mẫu kịch bản tiếng Anh chuẩn vào ô soạn thảo. Bạn có thể thoải mái chỉnh sửa lại câu từ theo nhu cầu của bài học nhé!", 
                "Điền mẫu thành công");
        }

        private async void BtnGenerateTts_Click(object sender, RoutedEventArgs e)
        {
            string script = txtTtsScript.Text.Trim();
            if (string.IsNullOrWhiteSpace(script))
            {
                ThongBaoDialog.CanhBao(this, 
                    "Vui lòng nhập hoặc dán nội dung kịch bản bài nghe trước khi tạo âm thanh.", 
                    "Chưa có nội dung");
                return;
            }

            if (script.Length > 4000)
            {
                bool acceptLong = ThongBaoDialog.XacNhan(this, 
                    $"Kịch bản hiện tại khá dài ({script.Length} ký tự). Quá trình tổng hợp âm thanh giọng đọc có thể mất từ 10 - 20 giây.\nBạn có muốn tiếp tục tạo không?", 
                    "Kịch bản dài", 
                    "Tiếp tục tạo", 
                    "Để tôi rút ngắn");
                if (!acceptLong) return;
            }

            int mode = cboAudioMode?.SelectedIndex ?? 0;

            btnGenerateTts.IsEnabled = false;
            btnPreviewTts.IsEnabled = false;
            btnSaveTtsToLesson.IsEnabled = false;
            txtTtsStatus.Text = "⏳ Đang tạo file bài nghe bằng giọng đọc bản xứ...";
            txtTtsStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F766E"));

            try
            {
                if (mode == 0)
                {
                    // Chế độ 1 người đọc (Đơn thoại)
                    string voice = (cboSingleVoice.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "diana";

                    var (success, result, err) = await GroqAudioService.Instance.GenerateSpeechAsync(
                        script, 
                        voice, 
                        _baiHoc.MaBaiHoc, 
                        GroqAudioService.DefaultTtsModel);

                    XuLyKetQuaTts(success, result, err);
                }
                else
                {
                    // Chế độ hội thoại 2 hoặc 3 người
                    bool is3Person = (mode == 2);

                    // Phân tách các dòng thoại
                    var rawLines = script.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    int linesWithColon = rawLines.Count(l => l.Contains(':'));

                    if (linesWithColon < 2)
                    {
                        ThongBaoDialog.CanhBao(this, 
                            "Kịch bản hội thoại cần có tên nhân vật ở đầu mỗi câu kèm dấu hai chấm.\n\nVí dụ:\nSarah: Hello Mark, how are you today?\nMark: Hi Sarah, I am doing great!\n\nBạn có thể bấm nút '✨ Điền mẫu kịch bản' ở phía trên để xem cách trình bày chuẩn nhé!", 
                            "Cần định dạng kịch bản hội thoại");
                        txtTtsStatus.Text = "Chờ nhập đúng định dạng kịch bản...";
                        txtTtsStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
                        return;
                    }

                    // Tên và giọng của các nhân vật đã cấu hình
                    string s1Name = string.IsNullOrWhiteSpace(txtSpeaker1Name.Text) ? "Sarah" : txtSpeaker1Name.Text.Trim();
                    string s1Voice = (cboSpeaker1Voice.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "diana";

                    string s2Name = string.IsNullOrWhiteSpace(txtSpeaker2Name.Text) ? "Mark" : txtSpeaker2Name.Text.Trim();
                    string s2Voice = (cboSpeaker2Voice.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "daniel";

                    string s3Name = string.IsNullOrWhiteSpace(txtSpeaker3Name.Text) ? "David" : txtSpeaker3Name.Text.Trim();
                    string s3Voice = (cboSpeaker3Voice.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "austin";

                    // Thu thập các tên người nói xuất hiện thực tế trong kịch bản
                    var detectedSpeakers = new List<string>();
                    foreach (var raw in rawLines)
                    {
                        if (raw.Contains(':'))
                        {
                            string spk = raw[..raw.IndexOf(':')].Trim();
                            if (!string.IsNullOrEmpty(spk) && !detectedSpeakers.Any(d => string.Equals(d, spk, StringComparison.OrdinalIgnoreCase)))
                            {
                                detectedSpeakers.Add(spk);
                            }
                        }
                    }

                    // Ràng buộc số lượng nhân vật
                    if (!is3Person && detectedSpeakers.Count < 2)
                    {
                        ThongBaoDialog.CanhBao(this, 
                            $"Hệ thống mới chỉ nhận diện được lời thoại của 1 nhân vật trong kịch bản. Chế độ đối thoại cần ít nhất 2 nhân vật (Ví dụ: {s1Name} và {s2Name}).", 
                            "Cần đủ 2 nhân vật");
                        txtTtsStatus.Text = "Kịch bản chưa đủ 2 nhân vật đối thoại.";
                        txtTtsStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
                        return;
                    }

                    if (is3Person && detectedSpeakers.Count < 3)
                    {
                        bool tiepTuc = ThongBaoDialog.XacNhan(this, 
                            $"Bạn đang chọn chế độ hội thoại 3 người, nhưng kịch bản hiện mới có lời thoại của {detectedSpeakers.Count} nhân vật ({string.Join(", ", detectedSpeakers)}).\nBạn có muốn tiếp tục tạo file nghe với các nhân vật này không?", 
                            "Xác nhận số nhân vật", 
                            "Tiếp tục tạo", 
                            "Để tôi bổ sung thêm");
                        if (!tiepTuc)
                        {
                            txtTtsStatus.Text = "Đã dừng để bổ sung kịch bản.";
                            txtTtsStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
                            return;
                        }
                    }

                    // Xây dựng danh sách lượt thoại có gán giọng đọc tương ứng
                    var lines = new List<DialogueVoiceLineDTO>();

                    foreach (var raw in rawLines)
                    {
                        string line = raw.Trim();
                        if (string.IsNullOrEmpty(line)) continue;

                        string speaker = s1Name;
                        string text = line;

                        if (line.Contains(':'))
                        {
                            int colonIdx = line.IndexOf(':');
                            speaker = line[..colonIdx].Trim();
                            text = line[(colonIdx + 1)..].Trim();
                        }

                        if (string.IsNullOrWhiteSpace(text)) continue;

                        // Xác định giọng đọc dựa trên nhân vật
                        string lineVoice = s1Voice;

                        if (string.Equals(speaker, s1Name, StringComparison.OrdinalIgnoreCase) || 
                            speaker.Equals("1", StringComparison.OrdinalIgnoreCase) || 
                            speaker.Equals("A", StringComparison.OrdinalIgnoreCase) ||
                            speaker.IndexOf("1", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            lineVoice = s1Voice;
                        }
                        else if (string.Equals(speaker, s2Name, StringComparison.OrdinalIgnoreCase) || 
                                 speaker.Equals("2", StringComparison.OrdinalIgnoreCase) || 
                                 speaker.Equals("B", StringComparison.OrdinalIgnoreCase) ||
                                 speaker.IndexOf("2", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            lineVoice = s2Voice;
                        }
                        else if (is3Person && (string.Equals(speaker, s3Name, StringComparison.OrdinalIgnoreCase) || 
                                               speaker.Equals("3", StringComparison.OrdinalIgnoreCase) || 
                                               speaker.Equals("C", StringComparison.OrdinalIgnoreCase) ||
                                               speaker.IndexOf("3", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            lineVoice = s3Voice;
                        }
                        else
                        {
                            // Nếu tên không trùng hệt với 3 ô nhập, so sánh thứ tự xuất hiện
                            int order = detectedSpeakers.FindIndex(d => string.Equals(d, speaker, StringComparison.OrdinalIgnoreCase));
                            if (order == 0) lineVoice = s1Voice;
                            else if (order == 1) lineVoice = s2Voice;
                            else if (order == 2 && is3Person) lineVoice = s3Voice;
                            else lineVoice = (order % 2 == 0) ? s1Voice : s2Voice;
                        }

                        lines.Add(new DialogueVoiceLineDTO
                        {
                            Speaker = speaker,
                            Voice = lineVoice,
                            Text = text,
                            PauseAfterSeconds = 0.45
                        });
                    }

                    var (success, result, err) = await GroqAudioService.Instance.GenerateDialogueSpeechAsync(
                        lines, 
                        _baiHoc.MaBaiHoc, 
                        GroqAudioService.DefaultTtsModel);

                    XuLyKetQuaTts(success, result, err);
                }
            }
            catch (Exception ex)
            {
                txtTtsStatus.Text = $"✗ Lỗi: {ex.Message}";
                txtTtsStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                ThongBaoDialog.Loi(this, $"Đã xảy ra sự cố khi tạo âm thanh: {ex.Message}", "Lỗi xử lý");
            }
            finally
            {
                btnGenerateTts.IsEnabled = true;
            }
        }

        private void XuLyKetQuaTts(bool success, AudioTranscriptDTO? result, string err)
        {
            if (success && result != null)
            {
                _generatedTtsDto = result;
                btnPreviewTts.IsEnabled = true;
                btnSaveTtsToLesson.IsEnabled = true;

                txtTtsStatus.Text = $"✓ Đã tạo bài nghe thành công ({result.ThoiLuongGiay}s)! Hãy nghe thử trước khi lưu.";
                txtTtsStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));

                // Mở player nghe thử
                try
                {
                    _ttsPlayer.Open(new Uri(result.AudioPath));
                }
                catch { }

                ThongBaoDialog.ThanhCong(this, 
                    $"Đã tạo file âm thanh bài nghe thành công (Thời lượng: {result.ThoiLuongGiay} giây)!\n\nBạn hãy bấm nút '▶️ Nghe thử bài vừa tạo' để kiểm tra trước khi lưu chính thức vào bài học nhé.", 
                    "Tạo file nghe thành công");
            }
            else
            {
                txtTtsStatus.Text = $"✗ Thất bại: {err}";
                txtTtsStatus.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626"));
                ThongBaoDialog.Loi(this, $"Chưa thể tạo bài nghe bằng giọng đọc AI:\n\n{err}", "Không thể tạo âm thanh");
            }
        }

        private void BtnPreviewTts_Click(object sender, RoutedEventArgs e)
        {
            if (_generatedTtsDto == null || !File.Exists(_generatedTtsDto.AudioPath)) return;

            if (_isTtsPlaying)
            {
                _ttsPlayer.Pause();
                _isTtsPlaying = false;
                btnPreviewTts.Content = "▶️ Nghe thử bài vừa tạo";
            }
            else
            {
                _ttsPlayer.Position = TimeSpan.Zero;
                _ttsPlayer.Play();
                _isTtsPlaying = true;
                btnPreviewTts.Content = "⏸ Tạm dừng nghe thử";
            }
        }

        private void BtnSaveTtsToLesson_Click(object sender, RoutedEventArgs e)
        {
            if (_generatedTtsDto == null || !File.Exists(_generatedTtsDto.AudioPath))
            {
                ThongBaoDialog.CanhBao(this, "Chưa có file âm thanh nào được tạo để lưu. Vui lòng bấm tạo file âm thanh trước nhé!", "Chưa có file");
                return;
            }

            // Nếu bài học đã có file âm thanh cũ khác với file mới, hỏi xác nhận thay thế
            string? currentAudio = !string.IsNullOrEmpty(_baiHoc.AudioPath) ? _baiHoc.AudioPath : _baiHoc.DuongDanAudio;
            if (!string.IsNullOrEmpty(currentAudio) && File.Exists(currentAudio) && 
                !string.Equals(currentAudio, _generatedTtsDto.AudioPath, StringComparison.OrdinalIgnoreCase))
            {
                bool dongY = ThongBaoDialog.XacNhan(this, 
                    "Bài học này hiện đã có một file âm thanh trước đó.\nBạn có chắc chắn muốn thay thế bằng bài nghe mới do AI vừa tạo không?", 
                    "Xác nhận thay thế file nghe", 
                    "Đồng ý thay thế", 
                    "Giữ file cũ");
                if (!dongY) return;
            }

            // Gán tệp âm thanh vào bài học
            _baiHoc.AudioPath = _generatedTtsDto.AudioPath;
            _baiHoc.DuongDanAudio = _generatedTtsDto.AudioPath;

            // Đánh dấu bản ghi TTS là đã duyệt
            _generatedTtsDto.TrangThaiDuyet = "DaDuyet";
            AudioTranscriptDAL.Instance.CapNhatTrangThaiDuyet(_generatedTtsDto.MaAudioTranscript, "DaDuyet", true);

            // Cập nhật bài học vào Database
            BaiHocBUS.LuuBaiHoc(_baiHoc, out _);

            CapNhatThongTinAudioNguon();
            txtGlobalMessage.Text = "✓ Đã lưu file nghe vào bài học và áp dụng bản chép lời thành công!";
            txtGlobalMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#15803D"));

            ThongBaoDialog.ThanhCong(this, 
                "Tệp âm thanh bài nghe đã được lưu và đính kèm chính thức vào bài học!\nHọc viên có thể nghe và xem lời thoại ngay lập tức.", 
                "Lưu thành công");
        }

        #endregion

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private static string FormatTime(TimeSpan ts)
        {
            return ts.Hours > 0 
                ? $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}" 
                : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }
}
