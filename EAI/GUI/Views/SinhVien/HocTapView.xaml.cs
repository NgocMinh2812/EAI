using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using DTO;
using GUI.Services;

namespace GUI.Views
{
    public partial class HocTapView : Page
    {
        private const string SearchPlaceholder = "Tìm bài học, chủ đề, từ vựng...";
        private List<BaiHocDTO> _danhSachBaiHoc = new List<BaiHocDTO>();
        private string _selectedSkill = "All";
        private BaiHocDTO? _currentBaiHoc;
        private bool _isInitialized = false;
        private readonly DAL.DanhMucDAL _danhMucDAL = new DAL.DanhMucDAL();

        // TASK-204: Biến theo dõi thời gian học tập thực tế
        private System.Windows.Threading.DispatcherTimer? _studyTimer;
        private int _studySeconds = 0;

        // TASK-203: Biến điều khiển bộ phát MediaPlayer / Audio Tutor
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        private System.Windows.Threading.DispatcherTimer? _audioTimer;
        private bool _isPlayingAudio = false;
        private double _audioCurrentSeconds = 0;
        private double _audioTotalSeconds = 90; // Mặc định 1 phút 30 giây
        private double _currentSpeed = 1.0;
        private bool _isDraggingSlider = false;

        public HocTapView(int? maBaiHocMoNgay = null)
        {
            InitializeComponent();
            InitMediaPlayer();
            InitTimers();
            LoadData();
            _isInitialized = true;
            ApplyFilter();

            if (maBaiHocMoNgay.HasValue)
            {
                HienThiChiTietBaiHoc(maBaiHocMoNgay.Value);
            }
        }

        private void InitMediaPlayer()
        {
            _mediaPlayer.MediaOpened += (s, e) =>
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan && _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                {
                    _audioTotalSeconds = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    if (txtAudioTotalTime != null)
                    {
                        txtAudioTotalTime.Text = $"{(int)_audioTotalSeconds / 60:D2}:{(int)_audioTotalSeconds % 60:D2}";
                    }
                }
            };
            _mediaPlayer.MediaEnded += (s, e) =>
            {
                BtnAudioStop_Click(this, new RoutedEventArgs());
            };
            _mediaPlayer.MediaFailed += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("MediaPlayer MediaFailed: " + e.ErrorException?.Message);
            };
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
                    try
                    {
                        if (_mediaPlayer.NaturalDuration.HasTimeSpan && _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                        {
                            _audioCurrentSeconds = _mediaPlayer.Position.TotalSeconds;
                            _audioTotalSeconds = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        }
                        else
                        {
                            _audioCurrentSeconds += 0.5 * _currentSpeed;
                        }
                    }
                    catch
                    {
                        _audioCurrentSeconds += 0.5 * _currentSpeed;
                    }

                    if (_audioCurrentSeconds >= _audioTotalSeconds)
                    {
                        BtnAudioStop_Click(this, new RoutedEventArgs());
                        return;
                    }

                    if (sldAudioProgress != null && _audioTotalSeconds > 0)
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

                if (txtTimKiem != null && string.IsNullOrEmpty(txtTimKiem.Text))
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

                // 1. Lọc theo Kỹ năng (All / TuVung / NguPhap / Doc / Nghe)
                if (!string.IsNullOrEmpty(_selectedSkill) && _selectedSkill != "All")
                {
                    query = query.Where(b => b.LoaiBaiHoc.Equals(_selectedSkill, StringComparison.OrdinalIgnoreCase));
                }

                // 2. Lọc theo Trình độ (ComboBox)
                if (cboTrinhDo != null && cboTrinhDo.SelectedItem is ComboBoxItem itemTrinhDo)
                {
                    string trinhDo = itemTrinhDo.Content?.ToString() ?? "Tất cả";
                    if (trinhDo != "Tất cả")
                    {
                        query = query.Where(b => b.MaTrinhDo.Equals(trinhDo, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // 3. Lọc theo Trạng thái (ComboBox)
                if (cboTrangThai != null && cboTrangThai.SelectedItem is ComboBoxItem itemTrangThai)
                {
                    string trangThai = itemTrangThai.Content?.ToString() ?? "Tất cả";
                    if (trangThai != "Tất cả")
                    {
                        query = query.Where(b => b.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // 4. Lọc theo từ khóa tìm kiếm
                if (txtTimKiem != null)
                {
                    string keyword = txtTimKiem.Text.Trim();
                    if (!string.IsNullOrEmpty(keyword) && keyword != SearchPlaceholder)
                    {
                        query = query.Where(b => (b.TieuDe != null && b.TieuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                                                 (b.TenChuDe != null && b.TenChuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                                                 (b.TomTat != null && b.TomTat.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
                    }
                }

                var filteredList = query.ToList();
                icDanhSachBaiHoc.ItemsSource = filteredList;

                // Xử lý Empty State
                if (pnlEmptyLessons != null)
                {
                    pnlEmptyLessons.Visibility = filteredList.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                    icDanhSachBaiHoc.Visibility = filteredList.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

                    if (txtEmptyReason != null)
                    {
                        txtEmptyReason.Text = "Không có bài học nào phù hợp với bộ lọc hiện tại.";
                    }
                }

                // Cập nhật số liệu thống kê theo danh sách đang hiển thị
                int tong = filteredList.Count;
                int hoanThanh = filteredList.Count(b => b.TrangThai == "Hoàn thành");
                txtTongSoBai.Text = tong.ToString();
                txtSoBaiHoanThanh.Text = $"{hoanThanh} ({(tong > 0 ? hoanThanh * 100 / tong : 0)}%)";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ApplyFilter Error: " + ex.Message);
            }
        }

        private void SyncSkillButtonsVisual(string skillCode)
        {
            _selectedSkill = skillCode;

            if (pnlSkillChips == null) return;
            var textGray = GetBrush("TextGray", Brushes.Gray);
            var borderLight = GetBrush("BorderLight", Brushes.LightGray);
            var tealLight = GetBrush("BrandTealLight", new SolidColorBrush(Color.FromRgb(230, 247, 243)));
            var teal = GetBrush("BrandTeal", new SolidColorBrush(Color.FromRgb(15, 118, 110)));

            foreach (var child in pnlSkillChips.Children)
            {
                if (child is Button b)
                {
                    bool isMatch = string.Equals(b.Tag?.ToString(), skillCode, StringComparison.OrdinalIgnoreCase);
                    b.Background = isMatch ? tealLight : Brushes.White;
                    b.Foreground = isMatch ? teal : textGray;
                    b.FontWeight = isMatch ? FontWeights.SemiBold : FontWeights.Medium;
                    b.BorderBrush = isMatch ? teal : borderLight;
                }
            }
        }

        private void BtnTreeReset_Click(object sender, RoutedEventArgs e)
        {
            _selectedSkill = "All";

            if (cboTrinhDo != null) cboTrinhDo.SelectedIndex = 0;
            if (cboTrangThai != null) cboTrangThai.SelectedIndex = 0;
            if (txtTimKiem != null)
            {
                txtTimKiem.Text = SearchPlaceholder;
                txtTimKiem.Foreground = GetBrush("TextGray", Brushes.Gray);
            }

            SyncSkillButtonsVisual("All");

            if (txtCurrentNavCategory != null)
            {
                txtCurrentNavCategory.Text = "LỘ TRÌNH TẤT CẢ BÀI HỌC";
            }
            if (txtBannerTitle != null)
            {
                txtBannerTitle.Text = "Kho Bài Học Tương Tác & Cá Nhân Hóa";
            }
            if (txtBannerSubtitle != null)
            {
                txtBannerSubtitle.Text = "Luyện tập toàn diện 4 kỹ năng: Từ vựng, Ngữ pháp, Đọc hiểu và Nghe hiểu có AI hướng dẫn chi tiết từng câu.";
            }

            ApplyFilter();
        }

        private void FilterSkill_Click(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized) return;

            if (sender is Button btn && btn.Tag is string tag)
            {
                string mappedSkill = tag switch
                {
                    "DocHieu" => "Doc",
                    "NgheHieu" => "Nghe",
                    _ => tag
                };

                SyncSkillButtonsVisual(mappedSkill);

                if (txtCurrentNavCategory != null)
                {
                    txtCurrentNavCategory.Text = mappedSkill switch
                    {
                        "TuVung" => "KỸ NĂNG: TỪ VỰNG (VOCABULARY)",
                        "NguPhap" => "KỸ NĂNG: NGỮ PHÁP (GRAMMAR)",
                        "Doc" => "KỸ NĂNG: ĐỌC HIỂU (READING)",
                        "Nghe" => "KỸ NĂNG: NGHE HIỂU (LISTENING)",
                        _ => "LỘ TRÌNH TẤT CẢ BÀI HỌC"
                    };
                }

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

            // TASK-203: Kiểm tra loại bài học: CHỈ HIỂN THỊ BỘ PHÁT AUDIO KHI HỌC NGHE HIỂU
            bool isListening = string.Equals(_currentBaiHoc.LoaiBaiHoc, "Nghe", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(_currentBaiHoc.LoaiBaiHoc, "NgheHieu", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(_currentBaiHoc.TenLoaiHienThi, "Nghe hiểu", StringComparison.OrdinalIgnoreCase)
                            || (_currentBaiHoc.LoaiBaiHoc != null && _currentBaiHoc.LoaiBaiHoc.IndexOf("Nghe", StringComparison.OrdinalIgnoreCase) >= 0);

            if (pnlAudioPlayerController != null)
            {
                pnlAudioPlayerController.Visibility = isListening ? Visibility.Visible : Visibility.Collapsed;
            }

            // Dừng phát âm thanh khi chuyển bài học hoặc khi bài học không phải là Nghe
            BtnAudioStop_Click(this, new RoutedEventArgs());

            if (isListening)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(_currentBaiHoc.DuongDanAudio))
                    {
                        Uri audioUri;
                        if (Uri.IsWellFormedUriString(_currentBaiHoc.DuongDanAudio, UriKind.Absolute))
                        {
                            audioUri = new Uri(_currentBaiHoc.DuongDanAudio, UriKind.Absolute);
                        }
                        else
                        {
                            string fullPath = System.IO.Path.IsPathRooted(_currentBaiHoc.DuongDanAudio)
                                ? _currentBaiHoc.DuongDanAudio
                                : System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _currentBaiHoc.DuongDanAudio);
                            audioUri = new Uri(fullPath);
                        }
                        _mediaPlayer.Open(audioUri);
                    }
                }
                catch { }

                _audioTotalSeconds = Math.Max(60, (_currentBaiHoc.ThoiGianUocTinhPhut > 0 ? _currentBaiHoc.ThoiGianUocTinhPhut : 15) * 6);
                if (txtAudioTotalTime != null)
                {
                    txtAudioTotalTime.Text = $"{(int)_audioTotalSeconds / 60:D2}:{(int)_audioTotalSeconds % 60:D2}";
                }
            }

            // Cập nhật thông tin tiêu đề, thể loại, chủ đề
            txtChiTietTieuDe.Text = _currentBaiHoc.TieuDe;
            txtChiTietTomTat.Text = _currentBaiHoc.TomTat;
            txtChiTietLoai.Text = _currentBaiHoc.TenLoaiHienThi;
            txtChiTietTrinhDo.Text = $"Trình độ {_currentBaiHoc.MaTrinhDo}";
            txtChiTietChuDe.Text = $"Chủ đề: {_currentBaiHoc.TenChuDe}";
            txtChiTietTienDo.Text = $"{_currentBaiHoc.PhanTramTienDo:0}%";
            txtChiTietTrangThai.Text = _currentBaiHoc.TrangThai;

            // TASK-202: Nạp nội dung chi tiết bài học đầy đủ, cụ thể cho từng ngữ pháp, từ vựng
            var detailed = BaiHocDetailProvider.GetDetailedContent(_currentBaiHoc);
            txtLectureObjective.Text = detailed.Objective;

            if (txtLectureFormulaTitle != null && !string.IsNullOrEmpty(detailed.FormulaTitle))
                txtLectureFormulaTitle.Text = detailed.FormulaTitle;
            txtLectureFormula.Text = detailed.Formula;

            if (txtLectureExampleTitle != null && !string.IsNullOrEmpty(detailed.ExampleTitle))
                txtLectureExampleTitle.Text = detailed.ExampleTitle;
            txtLectureExample.Text = detailed.Example;

            if (txtLectureTipsTitle != null && !string.IsNullOrEmpty(detailed.TipsTitle))
                txtLectureTipsTitle.Text = detailed.TipsTitle;
            txtLectureTips.Text = detailed.Tips;

            // Bổ sung đoạn văn tiếng Anh cho phần Đọc hiểu (Reading Passage)
            if (bdrReadingPassage != null)
            {
                bool isReading = string.Equals(_currentBaiHoc.LoaiBaiHoc, "Doc", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(_currentBaiHoc.LoaiBaiHoc, "DocHieu", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(_currentBaiHoc.TenLoaiHienThi, "Đọc hiểu", StringComparison.OrdinalIgnoreCase)
                              || !string.IsNullOrWhiteSpace(detailed.ReadingPassageContent);

                if (isReading && !string.IsNullOrWhiteSpace(detailed.ReadingPassageContent))
                {
                    bdrReadingPassage.Visibility = Visibility.Visible;
                    if (txtReadingPassageTitle != null)
                        txtReadingPassageTitle.Text = detailed.ReadingPassageTitle ?? "Reading Comprehension Passage";
                    if (txtReadingPassageContent != null)
                        txtReadingPassageContent.Text = detailed.ReadingPassageContent;
                }
                else
                {
                    bdrReadingPassage.Visibility = Visibility.Collapsed;
                }
            }

            txtChiTietNoiDung.Text = !string.IsNullOrWhiteSpace(detailed.FullContent) 
                ? detailed.FullContent 
                : BaiHocDetailProvider.CleanHtml(_currentBaiHoc.NoiDung);

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

            // Đảm bảo bài tập củng cố được tạo và lấy trực tiếp từ nội dung bài học (quy mô 5-10 câu)
            if (baiHoc.DanhSachCauHoi == null || baiHoc.DanhSachCauHoi.Count < 5)
            {
                baiHoc.DanhSachCauHoi = BaiHocDetailProvider.GetPracticeQuestions(baiHoc);
            }

            if (baiHoc.DanhSachCauHoi == null || baiHoc.DanhSachCauHoi.Count == 0)
            {
                var emptyNotice = new TextBlock
                {
                    Text = "Chưa có câu hỏi trắc nghiệm cho bài học này.",
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
            // TASK-204: Ghi nhận thời gian học tích lũy nếu đã học ít nhất 15s
            if (_studySeconds >= 15 && _currentBaiHoc != null)
            {
                int minutes = Math.Max(1, _studySeconds / 60);
                BaiHocBUS.CongDonThoiGianHoc(minutes);

                int totalQuestions = _currentBaiHoc.DanhSachCauHoi?.Count ?? 0;
                int answeredCount = _currentBaiHoc.DanhSachCauHoi?.Count(q => q.LuaChonCuaNguoiDung >= 0) ?? 0;

                // Nếu chưa hoàn thành 100% (chưa nộp bài tập củng cố) thì chỉ ghi nhận "Đang học" và tối đa 50-80%
                if (_currentBaiHoc.PhanTramTienDo < 100)
                {
                    double tienDo = (answeredCount > 0 && totalQuestions > 0)
                        ? Math.Min(80, 50 + (double)answeredCount / totalQuestions * 30)
                        : 50;
                    BaiHocBUS.CapNhatTienDo(_currentBaiHoc.MaBaiHoc, tienDo, "Đang học", $"{answeredCount}/{totalQuestions}");
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
                try
                {
                    _mediaPlayer.Play();
                }
                catch { }
                btnAudioPlayPause.Content = "⏸️ Tạm dừng";
                txtAudioStatus.Text = "Đang phát âm thanh bài giảng (Audio Tutor • MediaPlayer)...";
                _audioTimer?.Start();
            }
            else
            {
                try
                {
                    _mediaPlayer.Pause();
                }
                catch { }
                btnAudioPlayPause.Content = "▶️ Tiếp tục nghe";
                txtAudioStatus.Text = "Đã tạm dừng phát âm thanh.";
                _audioTimer?.Stop();
            }
        }

        private void BtnAudioStop_Click(object sender, RoutedEventArgs e)
        {
            _isPlayingAudio = false;
            _audioTimer?.Stop();
            try
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Position = TimeSpan.Zero;
            }
            catch { }
            _audioCurrentSeconds = 0;
            if (sldAudioProgress != null) sldAudioProgress.Value = 0;
            if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = "00:00";
            if (btnAudioPlayPause != null) btnAudioPlayPause.Content = "▶️ Nghe bài giảng";
            if (txtAudioStatus != null) txtAudioStatus.Text = "Bộ phát âm thanh bài học (Audio Tutor • MediaPlayer)";
        }

        private void BtnAudioRewind_Click(object sender, RoutedEventArgs e)
        {
            _audioCurrentSeconds = Math.Max(0, _audioCurrentSeconds - 5);
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
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
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
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
            if (sldAudioVolume != null)
            {
                try
                {
                    _mediaPlayer.Volume = Math.Clamp(sldAudioVolume.Value, 0.0, 1.0);
                }
                catch { }
            }
        }

        private void BtnAudioSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSpeed == 1.0) _currentSpeed = 1.25;
            else if (_currentSpeed == 1.25) _currentSpeed = 1.5;
            else if (_currentSpeed == 1.5) _currentSpeed = 0.8;
            else _currentSpeed = 1.0;

            try
            {
                _mediaPlayer.SpeedRatio = _currentSpeed;
            }
            catch { }
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
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
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
        // YÊU CẦU: PHẢI LÀM BÀI TẬP CỦNG CỐ MỚI ĐƯỢC HOÀN THÀNH 100%
        // =========================================================================
        private void BtnHoanThanhBaiHoc_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBaiHoc == null) return;

            int totalQuestions = _currentBaiHoc.DanhSachCauHoi?.Count ?? 0;
            int answeredCount = _currentBaiHoc.DanhSachCauHoi?.Count(q => q.LuaChonCuaNguoiDung >= 0) ?? 0;

            // KIỂM TRA ĐIỀU KIỆN: Phải hoàn thành toàn bộ bài tập củng cố thì mới cho hoàn thành 100%
            if (totalQuestions > 0 && answeredCount < totalQuestions)
            {
                Common.ThongBaoDialog.CanhBao(
                    $"⚠️ BẠN CHƯA HOÀN THÀNH BÀI TẬP CỦNG CỐ!\n\n" +
                    $"• Bạn mới trả lời: {answeredCount}/{totalQuestions} câu hỏi.\n" +
                    $"• Quy định: Bạn bắt buộc phải trả lời đầy đủ tất cả câu hỏi bài tập củng cố để kiểm tra kiến thức trước khi được hệ thống ghi nhận hoàn thành 100% bài học.\n\n" +
                    $"Hệ thống sẽ chuyển bạn sang tab 'Bài tập thực hành' ngay bây giờ.",
                    "Yêu cầu làm bài tập củng cố");

                SwitchToTab(isLyThuyet: false);
                return;
            }

            // Đã hoàn thành toàn bộ câu hỏi: Tính điểm thực tế
            int correctCount = _currentBaiHoc.DanhSachCauHoi?.Count(q => q.DaTraLoiDung == true) ?? 0;
            double scoreOutOf10 = totalQuestions > 0 ? Math.Round((double)correctCount / totalQuestions * 10, 1) : 10.0;
            string diemSo = $"{scoreOutOf10:0.0}/10 ({correctCount}/{totalQuestions} đúng)";

            int minutesSpent = Math.Max(1, _studySeconds / 60);
            BaiHocBUS.CongDonThoiGianHoc(minutesSpent);
            BaiHocBUS.CapNhatTienDo(_currentBaiHoc.MaBaiHoc, 100, "Hoàn thành", diemSo);

            _currentBaiHoc.PhanTramTienDo = 100;
            _currentBaiHoc.TrangThai = "Hoàn thành";
            txtChiTietTienDo.Text = "100%";
            txtChiTietTrangThai.Text = "Hoàn thành";

            _studyTimer?.Stop();
            BtnAudioStop_Click(this, new RoutedEventArgs());

            Common.ThongBaoDialog.ThanhCong(
                $"🎉 CHÚC MỪNG BẠN ĐÃ HOÀN THÀNH BÀI HỌC 100%!\n\n" +
                $"• Bài học: {_currentBaiHoc.TieuDe}\n" +
                $"• Kết quả bài tập củng cố: {correctCount}/{totalQuestions} câu đúng ({scoreOutOf10:0.0}/10 điểm)\n" +
                $"• Thời gian học tập thực tế: {minutesSpent} phút (đã cộng vào hồ sơ cá nhân)\n" +
                $"• Trạng thái ghi nhận: 100% (Hoàn thành)", 
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
