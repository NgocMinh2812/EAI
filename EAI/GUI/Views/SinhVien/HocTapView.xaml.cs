using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using BUS.AI;
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

        // Quản lý Bộ câu hỏi Củng cố (Bộ A) và Vận dụng (Bộ B) dùng chung qua CSDL
        private string _currentQuizMode = "CungCo"; // "CungCo" (Bộ A) hoặc "VanDung" (Bộ B)
        private BaiHocQuizDTO? _currentQuizA;
        private BaiHocQuizDTO? _currentQuizB;
        private bool _isLoadingQuiz = false;

        public HocTapView() : this(null)
        {
        }

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
                    string totalTimeStr = $"{(int)_audioTotalSeconds / 60:D2}:{(int)_audioTotalSeconds % 60:D2}";
                    if (txtAudioTotalTime != null) txtAudioTotalTime.Text = totalTimeStr;
                    if (txtAudioTotalTimeQuiz != null) txtAudioTotalTimeQuiz.Text = totalTimeStr;
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

                    CapNhatTienDoAudioUI();
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
                if (!BaiHocBUS.KiemTraQuyenTruyCap(maBaiHoc))
                {
                    GUI.Views.Common.ThongBaoDialog.CanhBao(
                        Window.GetWindow(this),
                        "Bài học này chưa được giảng viên cho phép truy cập hoặc đã bị thu hồi quyền truy cập.",
                        "Không có quyền truy cập"
                    );
                    LoadData();
                    ApplyFilter();
                    return;
                }
                HienThiChiTietBaiHoc(maBaiHoc);
            }
        }

        private void HienThiChiTietBaiHoc(int maBaiHoc)
        {
            // Kiểm tra quyền truy cập ở cả tầng nghiệp vụ và dữ liệu
            if (!BaiHocBUS.KiemTraQuyenTruyCap(maBaiHoc))
            {
                GUI.Views.Common.ThongBaoDialog.CanhBao(
                    Window.GetWindow(this),
                    "Bài học này chưa được giảng viên cho phép học viên truy cập hoặc đã bị thu hồi quyền truy cập.",
                    "Không có quyền truy cập"
                );
                pnlChiTietBaiHoc.Visibility = Visibility.Collapsed;
                pnlDanhSachBaiHoc.Visibility = Visibility.Visible;
                LoadData();
                ApplyFilter();
                return;
            }

            _currentBaiHoc = BaiHocBUS.LayChiTietBaiHoc(maBaiHoc);
            if (_currentBaiHoc == null)
            {
                GUI.Views.Common.ThongBaoDialog.CanhBao(
                    Window.GetWindow(this),
                    "Không tìm thấy bài học hoặc bài học không còn khả dụng.",
                    "Thông báo"
                );
                return;
            }

            // TASK-204: Khởi động bộ đếm thời gian học tập thực tế
            _studySeconds = 0;
            _studyTimer?.Start();
            if (txtStudyTimeElapsed != null)
            {
                txtStudyTimeElapsed.Text = "⏱️ Đã học: 00:00";
            }

            // TASK-203: Kiểm tra loại bài học: CHỈ HIỂN THỊ BỘ PHÁT AUDIO KHI HỌC NGHE HIỂU HOẶC CÓ AUDIO GỐC
            bool isListening = string.Equals(_currentBaiHoc.LoaiBaiHoc, "Nghe", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(_currentBaiHoc.LoaiBaiHoc, "NgheHieu", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(_currentBaiHoc.TenLoaiHienThi, "Nghe hiểu", StringComparison.OrdinalIgnoreCase)
                            || (_currentBaiHoc.LoaiBaiHoc != null && _currentBaiHoc.LoaiBaiHoc.IndexOf("Nghe", StringComparison.OrdinalIgnoreCase) >= 0);

            if (pnlAudioPlayerController != null)
            {
                pnlAudioPlayerController.Visibility = isListening ? Visibility.Visible : Visibility.Collapsed;
            }
            if (pnlAudioPlayerQuiz != null)
            {
                pnlAudioPlayerQuiz.Visibility = isListening ? Visibility.Visible : Visibility.Collapsed;
            }

            // Dừng phát âm thanh cũ khi chuyển bài học
            BtnAudioStop_Click(this, new RoutedEventArgs());

            if (isListening)
            {
                string rawAudio = _currentBaiHoc.AudioPath ?? _currentBaiHoc.DuongDanAudio ?? "";
                if (!string.IsNullOrWhiteSpace(rawAudio))
                {
                    Uri? audioUri = ResolveAudioUri(rawAudio);
                    if (audioUri != null)
                    {
                        try
                        {
                            _mediaPlayer.Open(audioUri);
                            if (txtAudioSubtitle != null)
                                txtAudioSubtitle.Text = "Tệp âm thanh bài học gốc do giảng viên cung cấp. Hỗ trợ tạm dừng, tua lại và điều chỉnh tốc độ.";
                        }
                        catch { }
                    }
                    else
                    {
                        if (txtAudioSubtitle != null)
                            txtAudioSubtitle.Text = "Không tìm thấy tệp âm thanh gốc trên thiết bị lưu trữ.";
                    }
                }
                else
                {
                    if (txtAudioSubtitle != null)
                        txtAudioSubtitle.Text = "Bài học chưa có tệp âm thanh đính kèm từ giảng viên.";
                }

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

            // NẠP VÀ HIỂN THỊ NỘI DUNG PHÂN TÍCH AI (LƯU VÀ TÁI SỬ DỤNG TRONG DATABASE)
            _ = TaiVaHienThiNoiDungAIAsync(_currentBaiHoc);

            // Khởi tạo trạng thái Bộ câu hỏi Củng cố (Bộ A) và Vận dụng (Bộ B)
            _currentQuizA = null;
            _currentQuizB = null;
            _currentQuizMode = "CungCo";
            CapNhatTrangThaiNutQuiz();

            // Mặc định tab lý thuyết
            SwitchToTab(isLyThuyet: true);

            // Hiển thị panel chi tiết
            pnlDanhSachBaiHoc.Visibility = Visibility.Collapsed;
            pnlChiTietBaiHoc.Visibility = Visibility.Visible;
        }

        private async Task TaiVaHienThiNoiDungAIAsync(BaiHocDTO baiHoc)
        {
            if (baiHoc == null) return;

            // Kiểm tra quyền lại một lần nữa trước khi phân tích
            if (!BaiHocBUS.KiemTraQuyenTruyCap(baiHoc.MaBaiHoc))
            {
                GUI.Views.Common.ThongBaoDialog.CanhBao(
                    Window.GetWindow(this),
                    "Quyền truy cập bài học đã bị thu hồi.",
                    "Không có quyền truy cập"
                );
                BtnQuayLai_Click(this, new RoutedEventArgs());
                return;
            }

            if (pnlAiGeneratingContent != null) pnlAiGeneratingContent.Visibility = Visibility.Visible;
            if (pnlAiContentError != null) pnlAiContentError.Visibility = Visibility.Collapsed;
            if (pnlDynamicCards != null)
            {
                pnlDynamicCards.Visibility = Visibility.Collapsed;
                pnlDynamicCards.Children.Clear();
            }

            if (txtAiGeneratingTitle != null)
                txtAiGeneratingTitle.Text = $"Đang chuẩn bị nội dung bài học: {baiHoc.TieuDe}...";
            if (txtAiGeneratingSub != null)
                txtAiGeneratingSub.Text = "Đang kiểm tra kết quả phân tích trong CSDL...";

            try
            {
                var aiData = await NoiDungHocTapAIService.Instance.LayHoacPhanTichNoiDungAsync(baiHoc, status =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (txtAiGeneratingSub != null) txtAiGeneratingSub.Text = status;
                    });
                });

                // Kiểm tra lại quyền trước khi hiển thị (phòng trường hợp giảng viên tắt quyền trong lúc AI xử lý)
                if (!BaiHocBUS.KiemTraQuyenTruyCap(baiHoc.MaBaiHoc))
                {
                    if (pnlAiGeneratingContent != null) pnlAiGeneratingContent.Visibility = Visibility.Collapsed;
                    GUI.Views.Common.ThongBaoDialog.CanhBao(
                        Window.GetWindow(this),
                        "Quyền truy cập bài học đã bị thu hồi.",
                        "Thông báo"
                    );
                    BtnQuayLai_Click(this, new RoutedEventArgs());
                    return;
                }

                if (pnlAiGeneratingContent != null) pnlAiGeneratingContent.Visibility = Visibility.Collapsed;

                if (aiData != null && aiData.DanhSachTheHoc != null && aiData.DanhSachTheHoc.Count > 0 && pnlDynamicCards != null)
                {
                    pnlDynamicCards.Children.Clear();
                    foreach (var card in aiData.DanhSachTheHoc)
                    {
                        var cardElement = TaoGiaoDienTheHocTap(card);
                        pnlDynamicCards.Children.Add(cardElement);
                    }
                    pnlDynamicCards.Visibility = Visibility.Visible;
                }
                else
                {
                    // Fallback giao diện lý thuyết nếu chưa có thẻ động
                    if (pnlFormattedLecture != null) pnlFormattedLecture.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                if (pnlAiGeneratingContent != null) pnlAiGeneratingContent.Visibility = Visibility.Collapsed;
                if (pnlAiContentError != null) pnlAiContentError.Visibility = Visibility.Visible;
                if (txtAiContentErrorMsg != null) txtAiContentErrorMsg.Text = ex.Message;
            }
        }

        private void BtnThuLaiPhanTich_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBaiHoc != null)
            {
                _ = TaiVaHienThiNoiDungAIAsync(_currentBaiHoc);
            }
        }

        public static Uri? ResolveAudioUri(string? audioPath)
        {
            if (string.IsNullOrWhiteSpace(audioPath)) return null;

            string path = audioPath.Trim();
            if (System.IO.File.Exists(path))
            {
                return new Uri(System.IO.Path.GetFullPath(path));
            }

            if (Uri.TryCreate(path, UriKind.Absolute, out var absUri) && 
                (absUri.Scheme == Uri.UriSchemeHttp || absUri.Scheme == Uri.UriSchemeHttps || absUri.Scheme == Uri.UriSchemeFile))
            {
                return absUri;
            }

            string fileName = System.IO.Path.GetFileName(path);
            foreach (var root in KhoHocLieuSyncHelper.GetAllHocLieuRoots())
            {
                if (System.IO.Directory.Exists(root))
                {
                    try
                    {
                        var found = System.IO.Directory.GetFiles(root, fileName, System.IO.SearchOption.AllDirectories).FirstOrDefault();
                        if (found != null && System.IO.File.Exists(found))
                        {
                            return new Uri(System.IO.Path.GetFullPath(found));
                        }
                    }
                    catch { }
                }
            }

            return null;
        }

        private UIElement TaoGiaoDienTheHocTap(LearningCardDTO card)
        {
            Color bgColor = ColorFromHex(card.BackgroundColor, Color.FromRgb(248, 250, 252));
            Color borderColor = ColorFromHex(card.BorderColor, Color.FromRgb(226, 232, 240));
            Color headerColor = ColorFromHex(card.HeaderColor, Color.FromRgb(15, 118, 110));

            var borderCard = new Border
            {
                Background = new SolidColorBrush(bgColor),
                BorderBrush = new SolidColorBrush(borderColor),
                BorderThickness = new Thickness(1.5),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(22, 18, 22, 18),
                Margin = new Thickness(0, 0, 0, 16),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 8,
                    ShadowDepth = 1,
                    Opacity = 0.04,
                    Color = Colors.Black
                }
            };

            var spCard = new StackPanel();

            // 1. Tiêu đề thẻ & Icon & Badge AI nếu có
            var gridHeader = new Grid { Margin = new Thickness(0, 0, 0, 10) };
            gridHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var spTitle = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };
            spTitle.Children.Add(new TextBlock
            {
                Text = card.Icon,
                FontSize = 16,
                Margin = new Thickness(0, 0, 8, 0),
                VerticalAlignment = VerticalAlignment.Center
            });
            spTitle.Children.Add(new TextBlock
            {
                Text = card.Title,
                FontWeight = FontWeights.Bold,
                FontSize = 13.5,
                Foreground = new SolidColorBrush(headerColor),
                VerticalAlignment = VerticalAlignment.Center
            });
            Grid.SetColumn(spTitle, 0);
            gridHeader.Children.Add(spTitle);

            if (card.IsAiSupplemented)
            {
                var bdrAiBadge = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(254, 243, 199)),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(8, 3, 8, 3),
                    VerticalAlignment = VerticalAlignment.Center
                };
                bdrAiBadge.Child = new TextBlock
                {
                    Text = "💡 Bổ sung bởi AI",
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9))
                };
                Grid.SetColumn(bdrAiBadge, 1);
                gridHeader.Children.Add(bdrAiBadge);
            }

            spCard.Children.Add(gridHeader);

            // 2. Nội dung đoạn văn / diễn giải
            if (!string.IsNullOrWhiteSpace(card.Content))
            {
                if (card.CardType == "Passage")
                {
                    // Đối với bài đọc, đóng khung trắng chuyên nghiệp
                    var bdrPassage = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(8),
                        Padding = new Thickness(16, 14, 16, 14),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(203, 213, 225)),
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(0, 4, 0, 8)
                    };
                    bdrPassage.Child = new TextBlock
                    {
                        Text = card.Content,
                        FontSize = 13.5,
                        LineHeight = 24,
                        Foreground = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
                        TextWrapping = TextWrapping.Wrap
                    };
                    spCard.Children.Add(bdrPassage);
                }
                else
                {
                    spCard.Children.Add(new TextBlock
                    {
                        Text = card.Content,
                        FontSize = 13.5,
                        LineHeight = 22,
                        Foreground = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 6)
                    });
                }
            }

            // 3. Danh sách các điểm then chốt (KeyPoints)
            if (card.KeyPoints != null && card.KeyPoints.Count > 0)
            {
                var spPoints = new StackPanel { Margin = new Thickness(0, 6, 0, 6) };
                foreach (var pt in card.KeyPoints)
                {
                    var gPt = new Grid { Margin = new Thickness(0, 2, 0, 4) };
                    gPt.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(22) });
                    gPt.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var iconPt = new TextBlock
                    {
                        Text = "•",
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(headerColor),
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    Grid.SetColumn(iconPt, 0);
                    gPt.Children.Add(iconPt);

                    var textPt = new TextBlock
                    {
                        Text = pt,
                        FontSize = 13,
                        Foreground = new SolidColorBrush(Color.FromRgb(51, 65, 85)),
                        TextWrapping = TextWrapping.Wrap,
                        LineHeight = 20
                    };
                    Grid.SetColumn(textPt, 1);
                    gPt.Children.Add(textPt);

                    spPoints.Children.Add(gPt);
                }
                spCard.Children.Add(spPoints);
            }

            // 4. Bảng hoặc danh sách từ vựng (VocabularyItems)
            if (card.VocabularyItems != null && card.VocabularyItems.Count > 0)
            {
                var spVocab = new StackPanel { Margin = new Thickness(0, 8, 0, 4) };
                foreach (var v in card.VocabularyItems)
                {
                    var bdrItem = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(8),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(226, 232, 240)),
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(12, 10, 12, 10),
                        Margin = new Thickness(0, 0, 0, 8)
                    };

                    var spVocabItem = new StackPanel();

                    // Dòng 1: Từ vựng + Phiên âm IPA + Từ loại
                    var wpTop = new WrapPanel { Margin = new Thickness(0, 0, 0, 4) };
                    wpTop.Children.Add(new TextBlock
                    {
                        Text = v.Word,
                        FontWeight = FontWeights.Bold,
                        FontSize = 14,
                        Foreground = new SolidColorBrush(Color.FromRgb(15, 23, 42)),
                        Margin = new Thickness(0, 0, 8, 0)
                    });
                    if (!string.IsNullOrWhiteSpace(v.Ipa))
                    {
                        wpTop.Children.Add(new TextBlock
                        {
                            Text = v.Ipa,
                            FontStyle = FontStyles.Italic,
                            FontSize = 12.5,
                            Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139)),
                            Margin = new Thickness(0, 0, 8, 0)
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(v.PartOfSpeech))
                    {
                        var bdrPos = new Border
                        {
                            Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)),
                            CornerRadius = new CornerRadius(4),
                            Padding = new Thickness(6, 1, 6, 1)
                        };
                        bdrPos.Child = new TextBlock
                        {
                            Text = v.PartOfSpeech,
                            FontSize = 11,
                            FontWeight = FontWeights.SemiBold,
                            Foreground = new SolidColorBrush(Color.FromRgb(71, 85, 105))
                        };
                        wpTop.Children.Add(bdrPos);
                    }
                    spVocabItem.Children.Add(wpTop);

                    // Dòng 2: Nghĩa tiếng Việt
                    if (!string.IsNullOrWhiteSpace(v.Meaning))
                    {
                        spVocabItem.Children.Add(new TextBlock
                        {
                            Text = $"Nghĩa: {v.Meaning}",
                            FontWeight = FontWeights.SemiBold,
                            FontSize = 13,
                            Foreground = new SolidColorBrush(Color.FromRgb(15, 118, 110)),
                            Margin = new Thickness(0, 0, 0, 3)
                        });
                    }

                    // Dòng 3: Cụm từ đi kèm (Collocation)
                    if (!string.IsNullOrWhiteSpace(v.Collocation))
                    {
                        spVocabItem.Children.Add(new TextBlock
                        {
                            Text = $"Cụm từ: {v.Collocation}",
                            FontSize = 12.5,
                            Foreground = new SolidColorBrush(Color.FromRgb(71, 85, 105)),
                            Margin = new Thickness(0, 0, 0, 3)
                        });
                    }

                    // Dòng 4: Ví dụ minh họa
                    if (!string.IsNullOrWhiteSpace(v.Example))
                    {
                        spVocabItem.Children.Add(new TextBlock
                        {
                            Text = $"Ví dụ: \"{v.Example}\"",
                            FontStyle = FontStyles.Italic,
                            FontSize = 12.5,
                            Foreground = new SolidColorBrush(Color.FromRgb(51, 65, 85)),
                            TextWrapping = TextWrapping.Wrap
                        });
                    }

                    bdrItem.Child = spVocabItem;
                    spVocab.Children.Add(bdrItem);
                }
                spCard.Children.Add(spVocab);
            }

            // 5. Đoạn hội thoại mẫu (DialogueLines)
            if (card.DialogueLines != null && card.DialogueLines.Count > 0)
            {
                var spDialogue = new StackPanel { Margin = new Thickness(0, 6, 0, 4) };
                foreach (var line in card.DialogueLines)
                {
                    var bdrLine = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(8),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(226, 232, 240)),
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(12, 8, 12, 8),
                        Margin = new Thickness(0, 0, 0, 6)
                    };

                    var spLine = new StackPanel();
                    spLine.Children.Add(new TextBlock
                    {
                        Text = line.Speaker,
                        FontWeight = FontWeights.Bold,
                        FontSize = 12,
                        Foreground = new SolidColorBrush(headerColor),
                        Margin = new Thickness(0, 0, 0, 2)
                    });
                    spLine.Children.Add(new TextBlock
                    {
                        Text = line.Text,
                        FontSize = 13,
                        Foreground = new SolidColorBrush(Color.FromRgb(15, 23, 42)),
                        TextWrapping = TextWrapping.Wrap
                    });
                    if (!string.IsNullOrWhiteSpace(line.Translation))
                    {
                        spLine.Children.Add(new TextBlock
                        {
                            Text = line.Translation,
                            FontStyle = FontStyles.Italic,
                            FontSize = 11.5,
                            Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139)),
                            Margin = new Thickness(0, 2, 0, 0),
                            TextWrapping = TextWrapping.Wrap
                        });
                    }

                    bdrLine.Child = spLine;
                    spDialogue.Children.Add(bdrLine);
                }
                spCard.Children.Add(spDialogue);
            }

            borderCard.Child = spCard;
            return borderCard;
        }

        private static Color ColorFromHex(string hex, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(hex)) return fallback;
            try
            {
                hex = hex.Trim().TrimStart('#');
                if (hex.Length == 6)
                {
                    byte r = Convert.ToByte(hex.Substring(0, 2), 16);
                    byte g = Convert.ToByte(hex.Substring(2, 2), 16);
                    byte b = Convert.ToByte(hex.Substring(4, 2), 16);
                    return Color.FromRgb(r, g, b);
                }
                else if (hex.Length == 8)
                {
                    byte a = Convert.ToByte(hex.Substring(0, 2), 16);
                    byte r = Convert.ToByte(hex.Substring(2, 2), 16);
                    byte g = Convert.ToByte(hex.Substring(4, 2), 16);
                    byte b = Convert.ToByte(hex.Substring(6, 2), 16);
                    return Color.FromArgb(a, r, g, b);
                }
            }
            catch { }
            return fallback;
        }

        private bool KiemTraBaiHocNgheHieu(BaiHocDTO? baiHoc)
        {
            if (baiHoc == null) return false;
            return string.Equals(baiHoc.LoaiBaiHoc, "Nghe", StringComparison.OrdinalIgnoreCase)
                || string.Equals(baiHoc.LoaiBaiHoc, "NgheHieu", StringComparison.OrdinalIgnoreCase)
                || string.Equals(baiHoc.TenLoaiHienThi, "Nghe hiểu", StringComparison.OrdinalIgnoreCase)
                || (!string.IsNullOrEmpty(baiHoc.LoaiBaiHoc) && baiHoc.LoaiBaiHoc.IndexOf("Nghe", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void CapNhatTrangThaiNutQuiz()
        {
            if (_currentBaiHoc == null) return;

            bool isListening = KiemTraBaiHocNgheHieu(_currentBaiHoc);

            if (isListening)
            {
                if (btnChonBoB != null) btnChonBoB.Visibility = Visibility.Collapsed;
                if (btnChonBoA != null)
                {
                    btnChonBoA.Content = "🎧 Bộ 10 câu nghe hiểu";
                    btnChonBoA.Margin = new Thickness(0);
                }
            }
            else
            {
                if (btnChonBoA != null)
                {
                    btnChonBoA.Content = "📘 Bộ 10 câu củng cố";
                    btnChonBoA.Margin = new Thickness(0, 0, 4, 0);
                }
                if (btnChonBoB != null)
                {
                    btnChonBoB.Visibility = Visibility.Visible;
                    string contentHash = NoiDungHocTapAIService.TinhContentHash(_currentBaiHoc);
                    var existingB = DAL.BaiHocQuizDAL.Instance.LayBoCauHoi(_currentBaiHoc.MaBaiHoc, contentHash, "VanDung");

                    if (existingB != null && existingB.DanhSachCauHoi.Count == 10)
                    {
                        btnChonBoB.Content = "🚀 Bộ 10 câu vận dụng (Đã tạo)";
                    }
                    else
                    {
                        btnChonBoB.Content = "✨ AI tạo thêm 10 câu hỏi nữa";
                    }
                }
            }
        }

        private void BtnChonBoA_Click(object sender, RoutedEventArgs e)
        {
            _ = ChuyenCheDoQuizAsync("CungCo");
        }

        private void BtnChonBoB_Click(object sender, RoutedEventArgs e)
        {
            if (KiemTraBaiHocNgheHieu(_currentBaiHoc)) return;
            _ = ChuyenCheDoQuizAsync("VanDung");
        }

        private void BtnRetryQuiz_Click(object sender, RoutedEventArgs e)
        {
            _ = ChuyenCheDoQuizAsync(_currentQuizMode, forceRetry: true);
        }

        private void BtnLamLaiQuiz_Click(object sender, RoutedEventArgs e)
        {
            var activeQuiz = _currentQuizMode == "VanDung" ? _currentQuizB : _currentQuizA;
            if (activeQuiz == null || activeQuiz.DanhSachCauHoi == null) return;

            // Đặt lại các lựa chọn của học viên trong bộ câu hỏi
            foreach (var q in activeQuiz.DanhSachCauHoi)
            {
                q.LuaChonCuaNguoiDung = -1;
                q.DaTraLoiDung = null;
            }

            if (pnlQuizResultSummary != null) pnlQuizResultSummary.Visibility = Visibility.Collapsed;
            if (btnLamLaiFooter != null) btnLamLaiFooter.Visibility = Visibility.Collapsed;
            if (btnNopBai != null) btnNopBai.Content = "🎯 Nộp bài & Xem kết quả";

            HienThiGiaoDienQuiz(activeQuiz, isSubmitted: false);
        }

        private async Task ChuyenCheDoQuizAsync(string loaiBo, bool forceRetry = false)
        {
            if (_currentBaiHoc == null) return;
            if (_isLoadingQuiz) return;

            bool isListening = KiemTraBaiHocNgheHieu(_currentBaiHoc);
            if (isListening)
            {
                loaiBo = "CungCo";
            }

            _currentQuizMode = loaiBo;

            // Cập nhật giao diện chuyển tab Bộ A / Bộ B
            var teal = GetBrush("BrandTeal", new SolidColorBrush(Color.FromRgb(15, 118, 110)));
            var tealLight = GetBrush("BrandTealLight", new SolidColorBrush(Color.FromRgb(230, 247, 243)));
            var textDark = GetBrush("TextDark", Brushes.Black);

            if (loaiBo == "CungCo")
            {
                if (btnChonBoA != null)
                {
                    btnChonBoA.Background = tealLight;
                    btnChonBoA.Foreground = teal;
                    btnChonBoA.FontWeight = FontWeights.SemiBold;
                    btnChonBoA.BorderBrush = teal;
                }
                if (btnChonBoB != null)
                {
                    btnChonBoB.Background = Brushes.White;
                    btnChonBoB.Foreground = textDark;
                    btnChonBoB.FontWeight = FontWeights.Medium;
                    btnChonBoB.BorderBrush = GetBrush("BorderLight", Brushes.LightGray);
                    if (isListening) btnChonBoB.Visibility = Visibility.Collapsed;
                }
                if (txtQuizModeBadge != null)
                {
                    txtQuizModeBadge.Text = isListening
                        ? "Bài tập nghe hiểu — 10 câu hỏi kiểm tra kỹ năng nghe dựa trên nội dung audio"
                        : "Củng cố kiến thức — 10 câu hỏi trắc nghiệm biên soạn trực tiếp từ bài học";
                }
            }
            else
            {
                if (btnChonBoB != null)
                {
                    btnChonBoB.Background = tealLight;
                    btnChonBoB.Foreground = teal;
                    btnChonBoB.FontWeight = FontWeights.SemiBold;
                    btnChonBoB.BorderBrush = teal;
                }
                if (btnChonBoA != null)
                {
                    btnChonBoA.Background = Brushes.White;
                    btnChonBoA.Foreground = textDark;
                    btnChonBoA.FontWeight = FontWeights.Medium;
                    btnChonBoA.BorderBrush = GetBrush("BorderLight", Brushes.LightGray);
                }
                if (txtQuizModeBadge != null)
                {
                    txtQuizModeBadge.Text = "Luyện vận dụng — 10 câu hỏi mới áp dụng kiến thức vào tình huống thực tế";
                }
            }

            var currentQuiz = loaiBo == "VanDung" ? _currentQuizB : _currentQuizA;

            // Nếu đã có trong bộ nhớ và không yêu cầu thử lại
            if (currentQuiz != null && currentQuiz.DanhSachCauHoi.Count == 10 && !forceRetry)
            {
                KiemTraKetQuaVaHienThi(currentQuiz);
                return;
            }

            // Hiển thị trạng thái đang tải
            _isLoadingQuiz = true;
            if (pnlQuizLoading != null)
            {
                pnlQuizLoading.Visibility = Visibility.Visible;
                txtQuizLoadingTitle.Text = loaiBo == "VanDung"
                    ? "🤖 AI đang biên soạn 10 câu hỏi vận dụng tình huống mới (Bộ B)..."
                    : "🤖 AI đang biên soạn 10 câu hỏi củng cố kiến thức (Bộ A)...";
                txtQuizLoadingSub.Text = "Đang kiểm tra kết quả trong CSDL / Trợ lý AI đang xử lý...";
            }
            if (pnlQuizError != null) pnlQuizError.Visibility = Visibility.Collapsed;
            if (pnlQuizResultSummary != null) pnlQuizResultSummary.Visibility = Visibility.Collapsed;
            if (pnlDanhSachCauHoi != null) pnlDanhSachCauHoi.Children.Clear();

            try
            {
                var (quiz, isFromCache, errorMsg) = await BaiHocQuizBUS.Instance.LayHoacTaoBoCauHoiAsync(
                    _currentBaiHoc, 
                    loaiBo, 
                    status =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (txtQuizLoadingSub != null) txtQuizLoadingSub.Text = status;
                        });
                    });

                if (pnlQuizLoading != null) pnlQuizLoading.Visibility = Visibility.Collapsed;

                if (quiz != null && quiz.DanhSachCauHoi != null && quiz.DanhSachCauHoi.Count == 10)
                {
                    if (loaiBo == "VanDung")
                    {
                        _currentQuizB = quiz;
                        if (btnChonBoB != null) btnChonBoB.Content = "🚀 Bộ 10 câu vận dụng (Đã tạo)";
                    }
                    else
                    {
                        _currentQuizA = quiz;
                    }

                    // Đồng bộ sang _currentBaiHoc.DanhSachCauHoi để duy trì tính tương thích
                    _currentBaiHoc.DanhSachCauHoi = quiz.DanhSachCauHoi;

                    KiemTraKetQuaVaHienThi(quiz);
                }
                else
                {
                    if (pnlQuizError != null)
                    {
                        pnlQuizError.Visibility = Visibility.Visible;
                        txtQuizErrorMsg.Text = errorMsg ?? "Không thể tạo bộ câu hỏi lúc này.";
                    }
                }
            }
            catch (Exception ex)
            {
                if (pnlQuizLoading != null) pnlQuizLoading.Visibility = Visibility.Collapsed;
                if (pnlQuizError != null)
                {
                    pnlQuizError.Visibility = Visibility.Visible;
                    txtQuizErrorMsg.Text = ex.Message;
                }
            }
            finally
            {
                _isLoadingQuiz = false;
            }
        }

        private void KiemTraKetQuaVaHienThi(BaiHocQuizDTO quiz)
        {
            if (quiz == null || _currentBaiHoc == null) return;

            int currentUserId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
            string contentHash = NoiDungHocTapAIService.TinhContentHash(_currentBaiHoc);

            // Kiểm tra xem sinh viên này đã có lịch sử làm bài tập cho bộ này chưa
            var previousResult = BaiHocQuizBUS.Instance.LayKetQuaSinhVien(currentUserId, _currentBaiHoc.MaBaiHoc, contentHash, quiz.LoaiBoCauHoi);

            if (previousResult != null && !string.IsNullOrWhiteSpace(previousResult.ChiTietTraLoiJson))
            {
                // Phục hồi lựa chọn của sinh viên
                BaiHocQuizBUS.PhucHoiLuaChonSinhVien(quiz, previousResult);

                if (pnlQuizResultSummary != null)
                {
                    pnlQuizResultSummary.Visibility = Visibility.Visible;
                    txtQuizResultScore.Text = $"{previousResult.DiemSo:0.0}";
                    txtQuizResultDetails.Text = $"{previousResult.SoCauDung}/{previousResult.TongSoCau} câu đúng ({previousResult.TyLeChinhXac})";
                    txtQuizResultFeedback.Text = previousResult.LaDatYeuCau
                        ? "🎉 Xuất sắc! Bạn đã vượt qua bài tập và ghi nhận kết quả thành công vào hồ sơ học tập."
                        : "⚠️ Bạn chưa đạt điểm tối ưu. Hãy đọc kỹ phần giải thích chi tiết bên dưới hoặc làm lại bài.";
                }

                if (btnLamLaiFooter != null) btnLamLaiFooter.Visibility = Visibility.Visible;
                if (btnNopBai != null) btnNopBai.Content = "🎯 Cập nhật bài làm";

                HienThiGiaoDienQuiz(quiz, isSubmitted: true);
            }
            else
            {
                if (pnlQuizResultSummary != null) pnlQuizResultSummary.Visibility = Visibility.Collapsed;
                if (btnLamLaiFooter != null) btnLamLaiFooter.Visibility = Visibility.Collapsed;
                if (btnNopBai != null) btnNopBai.Content = "🎯 Nộp bài & Xem kết quả";

                HienThiGiaoDienQuiz(quiz, isSubmitted: false);
            }
        }

        private void HienThiGiaoDienQuiz(BaiHocQuizDTO quiz, bool isSubmitted)
        {
            pnlDanhSachCauHoi.Children.Clear();

            if (quiz == null || quiz.DanhSachCauHoi == null || quiz.DanhSachCauHoi.Count == 0)
            {
                var emptyNotice = new TextBlock
                {
                    Text = "Chưa có câu hỏi cho bộ này. Vui lòng bấm 'Thử lại' hoặc 'AI tạo thêm 10 câu hỏi nữa'.",
                    Foreground = Brushes.Gray,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 10, 0, 10)
                };
                pnlDanhSachCauHoi.Children.Add(emptyNotice);
                return;
            }

            for (int i = 0; i < quiz.DanhSachCauHoi.Count; i++)
            {
                var ch = quiz.DanhSachCauHoi[i];
                int cauSo = i + 1;

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

                // Question Header (Số thứ tự + Độ khó chip)
                var headerGrid = new Grid { Margin = new Thickness(0, 0, 0, 8) };
                var spHeaderLeft = new StackPanel { Orientation = Orientation.Horizontal };

                var bdrBadge = new Border
                {
                    Background = GetBrush("BrandTealLight", new SolidColorBrush(Color.FromRgb(230, 247, 243))),
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(6, 2, 6, 2),
                    Margin = new Thickness(0, 0, 8, 0)
                };
                bdrBadge.Child = new TextBlock
                {
                    Text = $"CÂU {cauSo} / 10",
                    FontWeight = FontWeights.Bold,
                    FontSize = 11,
                    Foreground = GetBrush("BrandTeal", new SolidColorBrush(Color.FromRgb(15, 118, 110)))
                };
                spHeaderLeft.Children.Add(bdrBadge);

                string doKhoText = ch.DoKho == "De" ? "🟢 Dễ" : (ch.DoKho == "Kho" ? "🔴 Nâng cao" : "🟡 Trung bình");
                spHeaderLeft.Children.Add(new TextBlock
                {
                    Text = doKhoText,
                    FontSize = 11.5,
                    Foreground = GetBrush("TextGray", Brushes.Gray),
                    VerticalAlignment = VerticalAlignment.Center
                });
                headerGrid.Children.Add(spHeaderLeft);

                spCard.Children.Add(headerGrid);

                // Question Content
                var txtQ = new TextBlock
                {
                    Text = ch.NoiDungCauHoi,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 12),
                    Foreground = GetBrush("TextDark", Brushes.Black)
                };
                spCard.Children.Add(txtQ);

                // Explanation Box
                var bdrGiaiThich = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(240, 253, 250)),
                    BorderBrush = GetBrush("BrandMintSoft", new SolidColorBrush(Color.FromRgb(204, 251, 241))),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(14, 10, 14, 10),
                    Margin = new Thickness(0, 12, 0, 0),
                    Visibility = isSubmitted ? Visibility.Visible : Visibility.Collapsed
                };

                string expText = !string.IsNullOrWhiteSpace(ch.GiaiThichChiTiet) 
                    ? ch.GiaiThichChiTiet 
                    : $"Đáp án đúng là {(char)('A' + (ch.DapAnDungIndex ?? 0))}.";

                if (isSubmitted)
                {
                    bool isCorrect = (ch.LuaChonCuaNguoiDung >= 0 && ch.LuaChonCuaNguoiDung == ch.DapAnDungIndex);
                    char correctLetter = (char)('A' + (ch.DapAnDungIndex ?? 0));
                    if (isCorrect)
                    {
                        bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                        bdrGiaiThich.BorderBrush = new SolidColorBrush(Color.FromRgb(167, 243, 208));
                        expText = $"✅ Chính xác! {expText}";
                    }
                    else
                    {
                        bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                        bdrGiaiThich.BorderBrush = new SolidColorBrush(Color.FromRgb(254, 202, 202));
                        expText = $"❌ Chưa chính xác (Đáp án đúng là {correctLetter}). {expText}";
                    }
                }

                var txtGiaiThich = new TextBlock
                {
                    Text = $"💡 Lời giải chi tiết: {expText}",
                    Foreground = GetBrush("BrandTealDark", new SolidColorBrush(Color.FromRgb(4, 47, 46))),
                    FontSize = 12.5,
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 18
                };
                bdrGiaiThich.Child = txtGiaiThich;

                // 4 Options (A, B, C, D)
                for (int optIdx = 0; optIdx < (ch.DanhSachLuaChon?.Count ?? 0); optIdx++)
                {
                    string optionText = CauHoiDTO.StripOptionPrefix(ch.DanhSachLuaChon![optIdx]);
                    char optionLetter = (char)('A' + optIdx);
                    int currentOpt = optIdx;

                    var optBorder = new Border
                    {
                        CornerRadius = new CornerRadius(6),
                        Padding = new Thickness(6, 4, 6, 4),
                        Margin = new Thickness(0, 2, 0, 4)
                    };

                    var rb = new RadioButton
                    {
                        GroupName = $"QuizQ_{quiz.LoaiBoCauHoi}_{cauSo}",
                        Content = $"{optionLetter}. {optionText}",
                        FontSize = 13,
                        Cursor = System.Windows.Input.Cursors.Hand,
                        IsChecked = (ch.LuaChonCuaNguoiDung == currentOpt)
                    };

                    // Nếu đã nộp bài, tô màu đáp án đúng và lựa chọn sai của người dùng
                    if (isSubmitted)
                    {
                        if (currentOpt == ch.DapAnDungIndex)
                        {
                            optBorder.Background = new SolidColorBrush(Color.FromRgb(220, 252, 231)); // Green soft
                            optBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
                            optBorder.BorderThickness = new Thickness(1);
                            rb.FontWeight = FontWeights.SemiBold;
                            rb.Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61));
                        }
                        else if (ch.LuaChonCuaNguoiDung == currentOpt && currentOpt != ch.DapAnDungIndex)
                        {
                            optBorder.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226)); // Red soft
                            optBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                            optBorder.BorderThickness = new Thickness(1);
                            rb.Foreground = new SolidColorBrush(Color.FromRgb(185, 28, 28));
                        }
                    }

                    rb.Checked += (s, ev) =>
                    {
                        ch.LuaChonCuaNguoiDung = currentOpt;
                        ch.DaTraLoiDung = (currentOpt == ch.DapAnDungIndex);
                    };

                    optBorder.Child = rb;
                    spCard.Children.Add(optBorder);
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
            _ = ChuyenCheDoQuizAsync(_currentQuizMode);
        }

        private void BtnChuyenSangBaiTap_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: false);
            _ = ChuyenCheDoQuizAsync("CungCo");
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
        private void CapNhatAudioPlayState(bool isPlaying)
        {
            if (isPlaying)
            {
                if (btnAudioPlayPause != null) btnAudioPlayPause.Content = "⏸️ Tạm dừng";
                if (btnAudioPlayPauseQuiz != null) btnAudioPlayPauseQuiz.Content = "⏸️ Tạm dừng";
                if (txtAudioStatus != null) txtAudioStatus.Text = "Đang phát âm thanh bài giảng (Audio Tutor • MediaPlayer)...";
                if (txtAudioStatusQuiz != null) txtAudioStatusQuiz.Text = "🎧 Đang phát âm thanh bài nghe...";
            }
            else
            {
                if (btnAudioPlayPause != null) btnAudioPlayPause.Content = "▶️ Tiếp tục nghe";
                if (btnAudioPlayPauseQuiz != null) btnAudioPlayPauseQuiz.Content = "▶️ Tiếp tục nghe";
                if (txtAudioStatus != null) txtAudioStatus.Text = "Đã tạm dừng phát âm thanh.";
                if (txtAudioStatusQuiz != null) txtAudioStatusQuiz.Text = "Đã tạm dừng phát âm thanh bài nghe.";
            }
        }

        private void CapNhatTienDoAudioUI()
        {
            if (_audioTotalSeconds > 0)
            {
                double progressVal = (_audioCurrentSeconds / _audioTotalSeconds) * 100.0;
                if (sldAudioProgress != null) sldAudioProgress.Value = progressVal;
                if (sldAudioProgressQuiz != null) sldAudioProgressQuiz.Value = progressVal;
            }
            string timeStr = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
            if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = timeStr;
            if (txtAudioCurrentTimeQuiz != null) txtAudioCurrentTimeQuiz.Text = timeStr;
        }

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
                CapNhatAudioPlayState(true);
                _audioTimer?.Start();
            }
            else
            {
                try
                {
                    _mediaPlayer.Pause();
                }
                catch { }
                CapNhatAudioPlayState(false);
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
            if (sldAudioProgressQuiz != null) sldAudioProgressQuiz.Value = 0;
            if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = "00:00";
            if (txtAudioCurrentTimeQuiz != null) txtAudioCurrentTimeQuiz.Text = "00:00";
            if (btnAudioPlayPause != null) btnAudioPlayPause.Content = "▶️ Nghe bài giảng";
            if (btnAudioPlayPauseQuiz != null) btnAudioPlayPauseQuiz.Content = "▶️ Nghe bài tập";
            if (txtAudioStatus != null) txtAudioStatus.Text = "Bộ phát âm thanh bài học (Audio Tutor • MediaPlayer)";
            if (txtAudioStatusQuiz != null) txtAudioStatusQuiz.Text = "🎧 Audio bài nghe — Học viên vừa nghe vừa chọn đáp án bên dưới";
        }

        private void BtnAudioRewind_Click(object sender, RoutedEventArgs e)
        {
            _audioCurrentSeconds = Math.Max(0, _audioCurrentSeconds - 5);
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
            CapNhatTienDoAudioUI();
        }

        private void BtnAudioForward_Click(object sender, RoutedEventArgs e)
        {
            _audioCurrentSeconds = Math.Min(_audioTotalSeconds, _audioCurrentSeconds + 5);
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
            CapNhatTienDoAudioUI();
        }

        private void SldAudioVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldAudioVolume != null)
            {
                try
                {
                    _mediaPlayer.Volume = Math.Clamp(sldAudioVolume.Value, 0.0, 1.0);
                    if (sldAudioVolumeQuiz != null && Math.Abs(sldAudioVolumeQuiz.Value - sldAudioVolume.Value) > 0.02)
                    {
                        sldAudioVolumeQuiz.Value = sldAudioVolume.Value;
                    }
                }
                catch { }
            }
        }

        private void SldAudioVolumeQuiz_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldAudioVolumeQuiz != null)
            {
                try
                {
                    _mediaPlayer.Volume = Math.Clamp(sldAudioVolumeQuiz.Value, 0.0, 1.0);
                    if (sldAudioVolume != null && Math.Abs(sldAudioVolume.Value - sldAudioVolumeQuiz.Value) > 0.02)
                    {
                        sldAudioVolume.Value = sldAudioVolumeQuiz.Value;
                    }
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
            if (btnAudioSpeed != null) btnAudioSpeed.Content = $"{_currentSpeed:0.0}x";
            if (btnAudioSpeedQuiz != null) btnAudioSpeedQuiz.Content = $"{_currentSpeed:0.0}x";
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
                            synth.Volume = (int)((sldAudioVolume?.Value ?? 0.8) * 100);
                            synth.Rate = (int)((_currentSpeed - 1.0) * 3);
                            string textToRead = !string.IsNullOrWhiteSpace(_currentBaiHoc.TomTat) 
                                ? _currentBaiHoc.TomTat 
                                : _currentBaiHoc.TieuDe;
                            synth.Speak(textToRead);
                        }
                    }
                    catch { }
                });

                if (txtAudioStatus != null) txtAudioStatus.Text = "🤖 AI Audio Tutor đang đọc tóm tắt bài học...";
                CapNhatAudioPlayState(true);
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
            var slider = sender as Slider;
            if (slider != null && _audioTotalSeconds > 0)
            {
                _audioCurrentSeconds = (slider.Value / 100.0) * _audioTotalSeconds;
                try
                {
                    _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
                }
                catch { }
                CapNhatTienDoAudioUI();
            }
        }

        private void SldAudioProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraggingSlider && sldAudioProgress != null && _audioTotalSeconds > 0)
            {
                _audioCurrentSeconds = (sldAudioProgress.Value / 100.0) * _audioTotalSeconds;
                string timeStr = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
                if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = timeStr;
                if (txtAudioCurrentTimeQuiz != null) txtAudioCurrentTimeQuiz.Text = timeStr;
            }
        }

        private void SldAudioProgressQuiz_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraggingSlider && sldAudioProgressQuiz != null && _audioTotalSeconds > 0)
            {
                _audioCurrentSeconds = (sldAudioProgressQuiz.Value / 100.0) * _audioTotalSeconds;
                string timeStr = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
                if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = timeStr;
                if (txtAudioCurrentTimeQuiz != null) txtAudioCurrentTimeQuiz.Text = timeStr;
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

            var activeQuiz = _currentQuizMode == "VanDung" ? _currentQuizB : _currentQuizA;
            if (activeQuiz == null || activeQuiz.DanhSachCauHoi == null || activeQuiz.DanhSachCauHoi.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Chưa tải được câu hỏi bài tập. Vui lòng bấm 'Thử lại' hoặc 'AI tạo thêm 10 câu hỏi nữa'.", "Thông báo");
                return;
            }

            int totalQuestions = activeQuiz.DanhSachCauHoi.Count;
            int answeredCount = activeQuiz.DanhSachCauHoi.Count(q => q.LuaChonCuaNguoiDung >= 0);

            // KIỂM TRA ĐIỀU KIỆN: Phải hoàn thành toàn bộ 10 câu hỏi thì mới cho nộp bài
            if (answeredCount < totalQuestions)
            {
                Common.ThongBaoDialog.CanhBao(
                    $"⚠️ BẠN CHƯA HOÀN THÀNH TOÀN BỘ CÂU HỎI!\n\n" +
                    $"• Bạn mới trả lời: {answeredCount}/{totalQuestions} câu hỏi.\n" +
                    $"• Quy định: Bạn bắt buộc phải chọn đáp án cho tất cả {totalQuestions} câu hỏi trước khi nộp bài để hệ thống đánh giá chính xác năng lực.",
                    "Yêu cầu hoàn thành bài tập");
                return;
            }

            int currentUserId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
            int timeSpent = Math.Max(15, _studySeconds);

            bool ok = BaiHocQuizBUS.Instance.NopBaiTapSinhVien(
                currentUserId,
                _currentBaiHoc,
                activeQuiz,
                timeSpent,
                out string diemHienThi,
                out int correctCount,
                out decimal scoreOutOf10
            );

            // Cập nhật giao diện điểm và kết quả
            if (pnlQuizResultSummary != null)
            {
                pnlQuizResultSummary.Visibility = Visibility.Visible;
                txtQuizResultScore.Text = $"{scoreOutOf10:0.0}";
                txtQuizResultDetails.Text = $"{correctCount}/{totalQuestions} câu đúng ({correctCount * 10}%)";
                txtQuizResultFeedback.Text = scoreOutOf10 >= 8.0m 
                    ? "🎉 Xuất sắc! Bạn đã nắm vững toàn bộ kiến thức trọng tâm của bài học."
                    : (scoreOutOf10 >= 5.0m ? "👍 Đạt yêu cầu! Hãy xem lại các câu sai để củng cố thêm." : "⚠️ Bạn cần ôn tập thêm và có thể làm lại bài tập để cải thiện điểm số.");
            }

            if (btnLamLaiFooter != null) btnLamLaiFooter.Visibility = Visibility.Visible;
            if (btnNopBai != null) btnNopBai.Content = "🎯 Cập nhật bài làm";

            txtChiTietTienDo.Text = "100%";
            txtChiTietTrangThai.Text = "Hoàn thành";

            // Vẽ lại danh sách câu hỏi với giao diện Review (hiện đúng/sai và lời giải)
            HienThiGiaoDienQuiz(activeQuiz, isSubmitted: true);

            string tenCheDo = activeQuiz.IsBoVanDung ? "Bộ B (10 câu vận dụng)" : "Bộ A (10 câu củng cố)";
            Common.ThongBaoDialog.ThanhCong(
                $"🎉 CHÚC MỪNG BẠN ĐÃ HOÀN THÀNH BÀI TẬP!\n\n" +
                $"• Chế độ: {tenCheDo}\n" +
                $"• Kết quả đạt được: {correctCount}/{totalQuestions} câu đúng ({scoreOutOf10:0.0}/10 điểm)\n" +
                $"• Thời gian học tập thực tế: {Math.Max(1, timeSpent / 60)} phút (đã cộng vào hồ sơ cá nhân)\n" +
                $"• Trạng thái ghi nhận: 100% (Hoàn thành)\n\n" +
                $"Hệ thống đã lưu kết quả độc lập vào CSDL. Bạn có thể xem lại lời giải chi tiết cho từng câu hỏi bên dưới.",
                "Hoàn thành bài tập");
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
            if (MainWindow.Instance != null)
                MainWindow.Instance.NavigateToCached<TroLyAIView>();
            else
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
