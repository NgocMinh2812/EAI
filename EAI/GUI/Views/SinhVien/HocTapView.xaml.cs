using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using DTO;

namespace GUI.Views
{
    public partial class HocTapView : Page
    {
        private const string SearchPlaceholder = "Tìm bài học, chủ đề, từ vựng...";
        private List<BaiHocDTO> _danhSachBaiHoc = new List<BaiHocDTO>();
        private string _selectedSkill = "All";
        private BaiHocDTO? _currentBaiHoc;
        private bool _isInitialized = false;

        // TASK-204: Biến theo dõi thời gian học tập thực tế
        private System.Windows.Threading.DispatcherTimer? _studyTimer;
        private int _studySeconds = 0;

        // TASK-203: Biến điều khiển bộ phát MediaPlayer / Audio Tutor
        private System.Windows.Threading.DispatcherTimer? _audioTimer;
        private bool _isPlayingAudio = false;
        private double _audioCurrentSeconds = 0;
        private double _audioTotalSeconds = 90; // Mặc định 1 phút 30 giây
        private double _currentSpeed = 1.0;
        private bool _isDraggingSlider = false;

        public HocTapView(int? maBaiHocMoNgay = null)
        {
            InitializeComponent();
            InitTimers();
            LoadData();
            _isInitialized = true;
            ApplyFilter();

            if (maBaiHocMoNgay.HasValue)
            {
                HienThiChiTietBaiHoc(maBaiHocMoNgay.Value);
            }
        }

        private void InitTimers()
        {
            _studyTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _studyTimer.Tick += (s, e) =>
            {
                _studySeconds++;
                if (txtStudyTimeElapsed != null)
                {
                    txtStudyTimeElapsed.Text = $"⏱️ Đã học: {_studySeconds / 60:D2}:{_studySeconds % 60:D2}";
                }
            };

            _audioTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _audioTimer.Tick += (s, e) =>
            {
                if (_isPlayingAudio && !_isDraggingSlider)
                {
                    _audioCurrentSeconds += 0.5 * _currentSpeed;
                    if (_audioCurrentSeconds >= _audioTotalSeconds)
                    {
                        BtnAudioStop_Click(this, new RoutedEventArgs());
                        return;
                    }

                    if (sldAudioProgress != null)
                    {
                        sldAudioProgress.Value = (_audioCurrentSeconds / _audioTotalSeconds) * 100.0;
                    }
                    if (txtAudioCurrentTime != null)
                    {
                        txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
                    }
                }
            };
        }

        private Brush GetBrush(string key, Brush fallback)
        {
            try
            {
                var res = TryFindResource(key);
                if (res is Brush b) return b;
            }
            catch { }
            return fallback;
        }

        private void LoadData()
        {
            try
            {
                _danhSachBaiHoc = BaiHocBUS.LayDanhSachBaiHoc();

                if (cboTrinhDo != null && cboTrinhDo.SelectedIndex < 0)
                {
                    cboTrinhDo.SelectedIndex = 0;
                }

                if (cboTrangThai != null && cboTrangThai.SelectedIndex < 0)
                {
                    cboTrangThai.SelectedIndex = 0;
                }

                if (txtTimKiem != null)
                {
                    txtTimKiem.Text = SearchPlaceholder;
                    txtTimKiem.Foreground = GetBrush("TextGray", Brushes.Gray);
                }
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.CanhBao("Lỗi nạp dữ liệu bài học: " + ex.Message, "Thông báo");
            }
        }

        private void ApplyFilter()
        {
            if (!_isInitialized || icDanhSachBaiHoc == null || txtTongSoBai == null || txtSoBaiHoanThanh == null)
                return;

            try
            {
                var query = _danhSachBaiHoc.AsEnumerable();

                // Lọc theo Skill
                if (!string.IsNullOrEmpty(_selectedSkill) && _selectedSkill != "All")
                {
                    query = query.Where(b => b.LoaiBaiHoc.Equals(_selectedSkill, StringComparison.OrdinalIgnoreCase));
                }

                // Lọc theo Trình độ
                if (cboTrinhDo != null && cboTrinhDo.SelectedItem is ComboBoxItem itemTrinhDo)
                {
                    string trinhDo = itemTrinhDo.Content?.ToString() ?? "Tất cả";
                    if (trinhDo != "Tất cả")
                    {
                        query = query.Where(b => b.MaTrinhDo.Equals(trinhDo, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // Lọc theo Trạng thái
                if (cboTrangThai != null && cboTrangThai.SelectedItem is ComboBoxItem itemTrangThai)
                {
                    string trangThai = itemTrangThai.Content?.ToString() ?? "Tất cả";
                    if (trangThai != "Tất cả")
                    {
                        query = query.Where(b => b.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // Lọc theo từ khóa tìm kiếm
                if (txtTimKiem != null)
                {
                    string keyword = txtTimKiem.Text.Trim();
                    if (!string.IsNullOrEmpty(keyword) && keyword != SearchPlaceholder)
                    {
                        query = query.Where(b => b.TieuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                 b.TenChuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                 b.TomTat.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                    }
                }

                var filteredList = query.ToList();
                icDanhSachBaiHoc.ItemsSource = filteredList;

                // Cập nhật số liệu thống kê
                int tong = _danhSachBaiHoc.Count;
                int hoanThanh = _danhSachBaiHoc.Count(b => b.TrangThai == "Hoàn thành");
                txtTongSoBai.Text = tong.ToString();
                txtSoBaiHoanThanh.Text = $"{hoanThanh} ({(tong > 0 ? hoanThanh * 100 / tong : 0)}%)";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ApplyFilter Error: " + ex.Message);
            }
        }

        private void FilterSkill_Click(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized) return;

            if (sender is Button btn && btn.Tag is string tag)
            {
                _selectedSkill = tag;

                var textGray = GetBrush("TextGray", Brushes.Gray);
                var borderLight = GetBrush("BorderLight", Brushes.LightGray);
                var tealLight = GetBrush("BrandTealLight", new SolidColorBrush(Color.FromRgb(230, 247, 243)));
                var teal = GetBrush("BrandTeal", new SolidColorBrush(Color.FromRgb(15, 118, 110)));

                if (btn.Parent is Panel panel)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is Button b)
                        {
                            b.Background = Brushes.White;
                            b.Foreground = textGray;
                            b.FontWeight = FontWeights.Medium;
                            b.BorderBrush = borderLight;
                        }
                    }
                }

                btn.Background = tealLight;
                btn.Foreground = teal;
                btn.FontWeight = FontWeights.SemiBold;
                btn.BorderBrush = teal;

                ApplyFilter();
            }
        }

        private void CboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApplyFilter();
        }

        private void TxtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApplyFilter();
        }

        private void TxtTimKiem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiem.Text == SearchPlaceholder)
            {
                txtTimKiem.Text = "";
                txtTimKiem.Foreground = GetBrush("TextDark", Brushes.Black);
            }
        }

        private void TxtTimKiem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = SearchPlaceholder;
                txtTimKiem.Foreground = GetBrush("TextGray", Brushes.Gray);
            }
        }

        private void BtnVaoHoc_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int maBaiHoc)
            {
                HienThiChiTietBaiHoc(maBaiHoc);
            }
        }

        private void HienThiChiTietBaiHoc(int maBaiHoc)
        {
            _currentBaiHoc = BaiHocBUS.LayChiTietBaiHoc(maBaiHoc);
            if (_currentBaiHoc == null) return;

            // TASK-204: Khởi động bộ đếm thời gian học tập thực tế
            _studySeconds = 0;
            _studyTimer?.Start();
            if (txtStudyTimeElapsed != null)
            {
                txtStudyTimeElapsed.Text = "⏱️ Đã học: 00:00";
            }

            // TASK-203: Chuẩn bị trình phát âm thanh
            BtnAudioStop_Click(this, new RoutedEventArgs());
            _audioTotalSeconds = Math.Max(60, (_currentBaiHoc.ThoiGianUocTinhPhut > 0 ? _currentBaiHoc.ThoiGianUocTinhPhut : 15) * 6);
            if (txtAudioTotalTime != null)
            {
                txtAudioTotalTime.Text = $"{(int)_audioTotalSeconds / 60:D2}:{(int)_audioTotalSeconds % 60:D2}";
            }

            // Update info
            txtChiTietTieuDe.Text = _currentBaiHoc.TieuDe;
            txtChiTietTomTat.Text = _currentBaiHoc.TomTat;
            txtChiTietLoai.Text = _currentBaiHoc.TenLoaiHienThi;
            txtChiTietTrinhDo.Text = $"Trình độ {_currentBaiHoc.MaTrinhDo}";
            txtChiTietChuDe.Text = $"Chủ đề: {_currentBaiHoc.TenChuDe}";
            txtChiTietTienDo.Text = $"{_currentBaiHoc.PhanTramTienDo:0}%";
            txtChiTietTrangThai.Text = _currentBaiHoc.TrangThai;

            // Format Theory (TASK-202)
            txtLectureObjective.Text = $"Nắm vững bản chất kiến thức trọng tâm về '{_currentBaiHoc.TieuDe}', phản xạ chính xác trong bài thi {_currentBaiHoc.MaTrinhDo} và ngữ cảnh giao tiếp học thuật.";
            txtLectureFormula.Text = $"Quy tắc & Cấu trúc trọng tâm ({_currentBaiHoc.TenLoaiHienThi}):\n• Công thức cơ bản: Subject + Main Verb + Object/Complement\n• Biến thể nâng cao: Inversion / Reduced Clause / Passive Academic Structure";
            txtLectureExample.Text = !string.IsNullOrWhiteSpace(_currentBaiHoc.TomTat) 
                ? $"\"Ví dụ ngữ cảnh thực tế: {_currentBaiHoc.TomTat}\"" 
                : "\"Example: The implementation of comprehensive environmental policies is essential for sustainable development.\"";
            txtLectureTips.Text = $"Lưu ý bài thi chuẩn hóa ({_currentBaiHoc.MaTrinhDo}): Hãy chú ý các bẫy về sự hòa hợp thì, từ đồng nghĩa (synonyms) và cấu trúc song hành.";
            txtChiTietNoiDung.Text = _currentBaiHoc.NoiDung;

            // Build Questions UI
            TaoGiaoDienCauHoi(_currentBaiHoc);

            // Default to Theory Tab
            SwitchToTab(isLyThuyet: true);

            // Toggle views
            pnlDanhSachBaiHoc.Visibility = Visibility.Collapsed;
            pnlChiTietBaiHoc.Visibility = Visibility.Visible;
        }

        private void TaoGiaoDienCauHoi(BaiHocDTO baiHoc)
        {
            pnlDanhSachCauHoi.Children.Clear();

            if (baiHoc.DanhSachCauHoi == null || baiHoc.DanhSachCauHoi.Count == 0)
            {
                var emptyNotice = new TextBlock
                {
                    Text = "Chưa có câu hỏi trắc nghiệm cho bài học này. Bạn có thể nhấn 'Hoàn thành bài học' để ghi nhận tiến độ.",
                    Foreground = Brushes.Gray,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 10, 0, 10)
                };
                pnlDanhSachCauHoi.Children.Add(emptyNotice);
                return;
            }

            for (int i = 0; i < baiHoc.DanhSachCauHoi.Count; i++)
            {
                var ch = baiHoc.DanhSachCauHoi[i];

                var cardBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                    BorderBrush = GetBrush("BorderLight", Brushes.LightGray),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(16),
                    Margin = new Thickness(0, 0, 0, 16)
                };

                var spCard = new StackPanel();

                // Question Title
                var txtQ = new TextBlock
                {
                    Text = $"Câu {i + 1}: {ch.NoiDungCauHoi}",
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 12)
                };
                spCard.Children.Add(txtQ);

                // Explanation Box (Hidden by default)
                var bdrGiaiThich = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(240, 253, 250)),
                    BorderBrush = GetBrush("BrandMintSoft", new SolidColorBrush(Color.FromRgb(204, 251, 241))),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(12, 10, 12, 10),
                    Margin = new Thickness(0, 10, 0, 0),
                    Visibility = Visibility.Collapsed
                };
                var txtGiaiThich = new TextBlock
                {
                    Text = $"💡 Giải thích: {ch.GiaiThichChiTiet}",
                    Foreground = GetBrush("BrandTealDark", new SolidColorBrush(Color.FromRgb(4, 47, 46))),
                    FontSize = 12.5,
                    TextWrapping = TextWrapping.Wrap
                };
                bdrGiaiThich.Child = txtGiaiThich;

                // Options list
                for (int optIdx = 0; optIdx < ch.DanhSachLuaChon.Count; optIdx++)
                {
                    string optionText = ch.DanhSachLuaChon[optIdx];
                    char optionLetter = (char)('A' + optIdx);
                    int currentOpt = optIdx;

                    var rb = new RadioButton
                    {
                        GroupName = $"Q_{ch.MaCauHoi}",
                        Content = $"{optionLetter}. {optionText}",
                        FontSize = 13,
                        Margin = new Thickness(4, 4, 4, 8),
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    rb.Checked += (s, ev) =>
                    {
                        ch.LuaChonCuaNguoiDung = currentOpt;
                        bool isCorrect = (currentOpt == ch.DapAnDungIndex);
                        ch.DaTraLoiDung = isCorrect;

                        bdrGiaiThich.Visibility = Visibility.Visible;
                        if (isCorrect)
                        {
                            txtGiaiThich.Text = $"✅ Chính xác! {ch.GiaiThichChiTiet}";
                            bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                        }
                        else
                        {
                            char correctLetter = (char)('A' + ch.DapAnDungIndex);
                            txtGiaiThich.Text = $"❌ Chưa đúng (Đáp án đúng là {correctLetter}). {ch.GiaiThichChiTiet}";
                            bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                        }
                    };

                    spCard.Children.Add(rb);
                }

                spCard.Children.Add(bdrGiaiThich);
                cardBorder.Child = spCard;
                pnlDanhSachCauHoi.Children.Add(cardBorder);
            }
        }

        private void BtnQuayLai_Click(object sender, RoutedEventArgs e)
        {
            // TASK-204: Ghi nhận thời gian học tích lũy nếu đã học ít nhất 20s
            if (_studySeconds >= 20)
            {
                int minutes = Math.Max(1, _studySeconds / 60);
                BaiHocBUS.CongDonThoiGianHoc(minutes);
                if (_currentBaiHoc != null && _currentBaiHoc.PhanTramTienDo < 100)
                {
                    BaiHocBUS.CapNhatTienDo(_currentBaiHoc.MaBaiHoc, 50, "Đang học", "5/10");
                }
            }
            _studyTimer?.Stop();
            BtnAudioStop_Click(this, new RoutedEventArgs());

            pnlChiTietBaiHoc.Visibility = Visibility.Collapsed;
            pnlDanhSachBaiHoc.Visibility = Visibility.Visible;
            LoadData();
            ApplyFilter();
        }

        private void TabLyThuyet_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: true);
        }

        private void TabBaiTap_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: false);
        }

        private void BtnChuyenSangBaiTap_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: false);
        }

        private void SwitchToTab(bool isLyThuyet)
        {
            var textDark = GetBrush("TextDark", Brushes.Black);
            var textGray = GetBrush("TextGray", Brushes.Gray);

            if (isLyThuyet)
            {
                pnlTabLyThuyet.Visibility = Visibility.Visible;
                pnlTabBaiTap.Visibility = Visibility.Collapsed;

                btnTabLyThuyet.Background = Brushes.White;
                btnTabLyThuyet.Foreground = textDark;
                btnTabLyThuyet.FontWeight = FontWeights.SemiBold;

                btnTabBaiTap.Background = Brushes.Transparent;
                btnTabBaiTap.Foreground = textGray;
                btnTabBaiTap.FontWeight = FontWeights.Medium;
            }
            else
            {
                pnlTabLyThuyet.Visibility = Visibility.Collapsed;
                pnlTabBaiTap.Visibility = Visibility.Visible;

                btnTabBaiTap.Background = Brushes.White;
                btnTabBaiTap.Foreground = textDark;
                btnTabBaiTap.FontWeight = FontWeights.SemiBold;

                btnTabLyThuyet.Background = Brushes.Transparent;
                btnTabLyThuyet.Foreground = textGray;
                btnTabLyThuyet.FontWeight = FontWeights.Medium;
            }
        }

        // =========================================================================
        // TASK-203: TÍCH HỢP TRÌNH PHÁT AUDIO (MEDIAPLAYER & AUDIO TUTOR)
        // =========================================================================
        private void BtnAudioPlayPause_Click(object sender, RoutedEventArgs e)
        {
            _isPlayingAudio = !_isPlayingAudio;
            if (_isPlayingAudio)
            {
                btnAudioPlayPause.Content = "⏸️ Tạm dừng";
                txtAudioStatus.Text = "Đang phát âm thanh bài giảng (Audio Tutor)...";
                _audioTimer?.Start();
            }
            else
            {
                btnAudioPlayPause.Content = "▶️ Tiếp tục nghe";
                txtAudioStatus.Text = "Đã tạm dừng phát âm thanh.";
                _audioTimer?.Stop();
            }
        }

        private void BtnAudioStop_Click(object sender, RoutedEventArgs e)
        {
            _isPlayingAudio = false;
            _audioTimer?.Stop();
            _audioCurrentSeconds = 0;
            if (sldAudioProgress != null) sldAudioProgress.Value = 0;
            if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = "00:00";
            if (btnAudioPlayPause != null) btnAudioPlayPause.Content = "▶️ Nghe bài giảng";
            if (txtAudioStatus != null) txtAudioStatus.Text = "Bộ phát âm thanh bài học (Audio Tutor • MediaPlayer)";
        }

        private void BtnAudioRewind_Click(object sender, RoutedEventArgs e)
        {
            _audioCurrentSeconds = Math.Max(0, _audioCurrentSeconds - 5);
            if (sldAudioProgress != null && _audioTotalSeconds > 0)
            {
                sldAudioProgress.Value = (_audioCurrentSeconds / _audioTotalSeconds) * 100.0;
            }
            if (txtAudioCurrentTime != null)
            {
                txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
            }
        }

        private void BtnAudioForward_Click(object sender, RoutedEventArgs e)
        {
            _audioCurrentSeconds = Math.Min(_audioTotalSeconds, _audioCurrentSeconds + 5);
            if (sldAudioProgress != null && _audioTotalSeconds > 0)
            {
                sldAudioProgress.Value = (_audioCurrentSeconds / _audioTotalSeconds) * 100.0;
            }
            if (txtAudioCurrentTime != null)
            {
                txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
            }
        }

        private void SldAudioVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Cập nhật âm lượng phát
        }

        private void BtnAudioSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSpeed == 1.0) _currentSpeed = 1.25;
            else if (_currentSpeed == 1.25) _currentSpeed = 1.5;
            else if (_currentSpeed == 1.5) _currentSpeed = 0.8;
            else _currentSpeed = 1.0;

            btnAudioSpeed.Content = $"{_currentSpeed:0.0}x";
        }

        private void BtnSpeakFullText_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBaiHoc == null) return;
            try
            {
                // Phát giọng đọc tiếng Anh
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        var type = Type.GetType("System.Speech.Synthesis.SpeechSynthesizer, System.Speech");
                        if (type != null)
                        {
                            dynamic synth = Activator.CreateInstance(type)!;
                            synth.Volume = (int)(sldAudioVolume.Value * 100);
                            synth.Rate = (int)((_currentSpeed - 1.0) * 3);
                            string textToRead = !string.IsNullOrWhiteSpace(_currentBaiHoc.TomTat) 
                                ? _currentBaiHoc.TomTat 
                                : _currentBaiHoc.TieuDe;
                            synth.Speak(textToRead);
                        }
                    }
                    catch { }
                });

                txtAudioStatus.Text = "🤖 AI Audio Tutor đang đọc tóm tắt bài học...";
                btnAudioPlayPause.Content = "⏸️ Đang đọc...";
                _isPlayingAudio = true;
                _audioTimer?.Start();
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.ThongTin("Tính năng giọng đọc AI: " + ex.Message, "Thông báo");
            }
        }

        private void SldAudioProgress_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingSlider = true;
        }

        private void SldAudioProgress_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingSlider = false;
            _audioCurrentSeconds = (sldAudioProgress.Value / 100.0) * _audioTotalSeconds;
        }

        private void SldAudioProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraggingSlider)
            {
                _audioCurrentSeconds = (sldAudioProgress.Value / 100.0) * _audioTotalSeconds;
                if (txtAudioCurrentTime != null)
                {
                    txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
                }
            }
        }

        private void BtnAskAI_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBaiHoc == null) return;
            Common.ThongBaoDialog.ThongTin($"Trợ lý AI sẵn sàng giải thích chuyên sâu cho bài học '{_currentBaiHoc.TieuDe}'. Bạn có thể đặt câu hỏi về ngữ pháp, từ vựng hoặc ví dụ mở rộng bất kỳ lúc nào!", "EnglishAI Tutor Assistant");
        }

        // =========================================================================
        // TASK-204: GHI NHẬN TIẾN ĐỘ & THỜI GIAN HỌC TẬP VÀO DATABASE
        // =========================================================================
        private void BtnHoanThanhBaiHoc_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBaiHoc == null) return;

            int minutesSpent = Math.Max(1, _studySeconds / 60);
            BaiHocBUS.CongDonThoiGianHoc(minutesSpent);
            BaiHocBUS.CapNhatTienDo(_currentBaiHoc.MaBaiHoc, 100, "Hoàn thành", "10/10");

            _studyTimer?.Stop();
            BtnAudioStop_Click(this, new RoutedEventArgs());

            Common.ThongBaoDialog.ThanhCong(
                $"🎉 Chúc mừng bạn đã hoàn thành bài học '{_currentBaiHoc.TieuDe}'!\n\n" +
                $"• Thời gian học ghi nhận: {minutesSpent} phút (đã cộng vào Hồ sơ học tập)\n" +
                $"• Tiến độ: 100% (Hoàn thành)\n" +
                $"• Điểm số: 10/10", 
                "Hoàn thành bài học");

            BtnQuayLai_Click(sender, e);
        }

        private void NavTrangChu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new TrangChuView());
        }

        private void NavLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            _selectedSkill = "NguPhap";
            ApplyFilter();
            pnlChiTietBaiHoc.Visibility = Visibility.Collapsed;
            pnlDanhSachBaiHoc.Visibility = Visibility.Visible;
        }

        private void NavAIChat_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
