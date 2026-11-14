using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Input;
using BUS;
using DAL;
using DTO;
using GUI.Services;

namespace GUI.Views
{
    public partial class LuyenTapView : Page
    {
        // =========================================================================
        // VIEW MODELS FOR PRACTICE HUB & PACKAGES
        // =========================================================================
        public class PracticePackageViewModel
        {
            public int MaBaiTapDeThi { get; set; }
            public string TenBaiTapDeThi { get; set; } = string.Empty;
            public string LoaiHinh { get; set; } = "LuyenTap";
            public string? MoTa { get; set; }
            public int ThoiGianLamBaiPhut { get; set; } = 20;
            public int SoLuongCauHoi { get; set; } = 10;
            public string MaTrinhDo { get; set; } = "B2";
            public decimal DiemDatYeuCau { get; set; } = 5;
            public decimal TongDiem { get; set; } = 10;
            public int? MaCauTrucDeThi { get; set; }
            public string? TenCauTrucDeThi { get; set; }
            public bool HasBlueprint => MaCauTrucDeThi.HasValue && MaCauTrucDeThi.Value > 0;
            public Visibility HasBlueprintVisibility => HasBlueprint ? Visibility.Visible : Visibility.Collapsed;
            public string BlueprintBadgeText => !string.IsNullOrEmpty(TenCauTrucDeThi) ? TenCauTrucDeThi : "Đề chuẩn hóa";

            public string DurationPill => $"{ThoiGianLamBaiPhut} phút";
            public string QuestionCountPill => $"{SoLuongCauHoi} câu hỏi";
            public string CleanMoTa => !string.IsNullOrWhiteSpace(MoTa) ? MoTa : "Rèn luyện kiến thức ngôn ngữ học thuật chuyên sâu và phản xạ nhanh.";

            public Brush AccentBrush
            {
                get
                {
                    if (TenBaiTapDeThi.Contains("Điều Kiện") || TenBaiTapDeThi.Contains("Ngữ Pháp") || TenBaiTapDeThi.Contains("Đảo Ngữ")) return new SolidColorBrush(Color.FromRgb(22, 163, 74));
                    if (TenBaiTapDeThi.Contains("Từ Vựng") || TenBaiTapDeThi.Contains("Oxford") || TenBaiTapDeThi.Contains("Idioms") || TenBaiTapDeThi.Contains("Thành Ngữ")) return new SolidColorBrush(Color.FromRgb(217, 119, 6));
                    if (TenBaiTapDeThi.Contains("Đọc Hiểu") || TenBaiTapDeThi.Contains("Reading")) return new SolidColorBrush(Color.FromRgb(37, 99, 235));
                    if (TenBaiTapDeThi.Contains("Viết") || TenBaiTapDeThi.Contains("Lỗi Sai") || TenBaiTapDeThi.Contains("Writing")) return new SolidColorBrush(Color.FromRgb(147, 51, 234));
                    if (TenBaiTapDeThi.Contains("Phản Xạ")) return new SolidColorBrush(Color.FromRgb(220, 38, 38));
                    return new SolidColorBrush(Color.FromRgb(15, 118, 110));
                }
            }

            public LucideIconKind FormatIconKind
            {
                get
                {
                    string target = TenBaiTapDeThi.ToUpper();
                    if (target.Contains("ĐIỀU KIỆN") || target.Contains("NGỮ PHÁP") || target.Contains("ĐẢO NGỮ")) return LucideIconKind.Sparkles;
                    if (target.Contains("TỪ VỰNG") || target.Contains("OXFORD") || target.Contains("IDIOMS") || target.Contains("THÀNH NGỮ")) return LucideIconKind.BookOpen;
                    if (target.Contains("ĐỌC HIỂU") || target.Contains("READING")) return LucideIconKind.FileText;
                    if (target.Contains("VIẾT") || target.Contains("LỖI SAI") || target.Contains("WRITING")) return LucideIconKind.Edit3;
                    if (target.Contains("NÓI") || target.Contains("SPEAKING")) return LucideIconKind.Mic;
                    if (target.Contains("PHẢN XẠ")) return LucideIconKind.Zap;
                    return LucideIconKind.Target;
                }
            }

            public string FormatIcon => "";

            public Brush FormatBadgeBg
            {
                get
                {
                    if (TenBaiTapDeThi.Contains("Điều Kiện") || TenBaiTapDeThi.Contains("Ngữ Pháp") || TenBaiTapDeThi.Contains("Đảo Ngữ")) return new SolidColorBrush(Color.FromRgb(240, 253, 244));
                    if (TenBaiTapDeThi.Contains("Từ Vựng") || TenBaiTapDeThi.Contains("Oxford") || TenBaiTapDeThi.Contains("Idioms") || TenBaiTapDeThi.Contains("Thành Ngữ")) return new SolidColorBrush(Color.FromRgb(254, 243, 199));
                    if (TenBaiTapDeThi.Contains("Đọc Hiểu") || TenBaiTapDeThi.Contains("Reading")) return new SolidColorBrush(Color.FromRgb(239, 246, 255));
                    if (TenBaiTapDeThi.Contains("Viết") || TenBaiTapDeThi.Contains("Lỗi Sai") || TenBaiTapDeThi.Contains("Writing")) return new SolidColorBrush(Color.FromRgb(250, 245, 255));
                    if (TenBaiTapDeThi.Contains("Phản Xạ")) return new SolidColorBrush(Color.FromRgb(254, 242, 242));
                    return new SolidColorBrush(Color.FromRgb(204, 251, 241));
                }
            }

            public string FormatTag
            {
                get
                {
                    if (TenBaiTapDeThi.Contains("Điều Kiện") || TenBaiTapDeThi.Contains("Đảo Ngữ")) return "CHUYÊN ĐỀ ĐIỀU KIỆN & ĐẢO NGỮ";
                    if (TenBaiTapDeThi.Contains("Từ Vựng") || TenBaiTapDeThi.Contains("Oxford")) return "TỪ VỰNG & COLLOCATIONS";
                    if (TenBaiTapDeThi.Contains("Idioms") || TenBaiTapDeThi.Contains("Thành Ngữ")) return "THÀNH NGỮ NÂNG CAO C1";
                    if (TenBaiTapDeThi.Contains("Đọc Hiểu") || TenBaiTapDeThi.Contains("Reading")) return "ĐỌC HIỂU CHUYÊN SÂU";
                    if (TenBaiTapDeThi.Contains("Viết") || TenBaiTapDeThi.Contains("Lỗi Sai")) return "CẤU TRÚC VIẾT & SỬA LỖI";
                    if (TenBaiTapDeThi.Contains("Phản Xạ")) return "PHẢN XẠ NHANH 60S";
                    if (TenBaiTapDeThi.Contains("Căn Bản") || TenBaiTapDeThi.Contains("Giao Tiếp")) return "TỔNG HỢP CĂN BẢN A2";
                    return "LUYỆN TẬP THEO CHỦ ĐỀ";
                }
            }

            public Brush FormatTagBg
            {
                get
                {
                    if (TenBaiTapDeThi.Contains("Điều Kiện") || TenBaiTapDeThi.Contains("Đảo Ngữ")) return new SolidColorBrush(Color.FromRgb(220, 252, 231));
                    if (TenBaiTapDeThi.Contains("Từ Vựng") || TenBaiTapDeThi.Contains("Oxford")) return new SolidColorBrush(Color.FromRgb(254, 240, 138));
                    if (TenBaiTapDeThi.Contains("Idioms") || TenBaiTapDeThi.Contains("Thành Ngữ")) return new SolidColorBrush(Color.FromRgb(254, 215, 170));
                    if (TenBaiTapDeThi.Contains("Đọc Hiểu") || TenBaiTapDeThi.Contains("Reading")) return new SolidColorBrush(Color.FromRgb(219, 234, 254));
                    if (TenBaiTapDeThi.Contains("Viết") || TenBaiTapDeThi.Contains("Lỗi Sai")) return new SolidColorBrush(Color.FromRgb(243, 232, 255));
                    if (TenBaiTapDeThi.Contains("Phản Xạ")) return new SolidColorBrush(Color.FromRgb(254, 226, 226));
                    return new SolidColorBrush(Color.FromRgb(204, 251, 241));
                }
            }

            public Brush FormatTagFg
            {
                get
                {
                    if (TenBaiTapDeThi.Contains("Điều Kiện") || TenBaiTapDeThi.Contains("Đảo Ngữ")) return new SolidColorBrush(Color.FromRgb(21, 128, 61));
                    if (TenBaiTapDeThi.Contains("Từ Vựng") || TenBaiTapDeThi.Contains("Oxford")) return new SolidColorBrush(Color.FromRgb(161, 98, 7));
                    if (TenBaiTapDeThi.Contains("Idioms") || TenBaiTapDeThi.Contains("Thành Ngữ")) return new SolidColorBrush(Color.FromRgb(194, 65, 12));
                    if (TenBaiTapDeThi.Contains("Đọc Hiểu") || TenBaiTapDeThi.Contains("Reading")) return new SolidColorBrush(Color.FromRgb(29, 78, 216));
                    if (TenBaiTapDeThi.Contains("Viết") || TenBaiTapDeThi.Contains("Lỗi Sai")) return new SolidColorBrush(Color.FromRgb(126, 34, 206));
                    if (TenBaiTapDeThi.Contains("Phản Xạ")) return new SolidColorBrush(Color.FromRgb(185, 28, 28));
                    return new SolidColorBrush(Color.FromRgb(15, 118, 110));
                }
            }

            public string LevelTag => $"Chuẩn {MaTrinhDo}";
            public string PracticeSubtitle => $"⏱️ {ThoiGianLamBaiPhut} phút • {SoLuongCauHoi} câu hỏi • {MoTa ?? "Gói luyện tập chuyên sâu theo chủ đề"}";
        }

        public static string LayTenTiengVietKyNang(string? kyNang)
        {
            if (string.IsNullOrWhiteSpace(kyNang)) return "Kỹ năng tổng hợp";
            string k = kyNang.Trim().ToLowerInvariant();
            if (k.Contains("tuvung") || k.Contains("vocab")) return "Từ vựng";
            if (k.Contains("nghe") || k.Contains("listen")) return "Nghe hiểu";
            if (k.Contains("nguphap") || k.Contains("grammar")) return "Ngữ pháp";
            if (k.Contains("doc") || k.Contains("read")) return "Đọc hiểu";
            if (k.Contains("viet") || k.Contains("writ")) return "Viết";
            if (k.Contains("noi") || k.Contains("speak")) return "Nói";
            return kyNang;
        }

        public class SessionQuestionResultViewModel
        {
            public int QuestionNumber { get; set; }
            public string QuestionNumberText => $"Câu {QuestionNumber}";
            public string? KyNang { get; set; }
            public string KyNangTag => $"Kỹ năng: {LayTenTiengVietKyNang(KyNang)}";
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
            public string KyNangTag => $"Kỹ năng: {LayTenTiengVietKyNang(KyNang)}";
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
        private readonly Dictionary<int, int> _sessionUserAnswers = new Dictionary<int, int>();
        private readonly Dictionary<int, string> _sessionUserTextAnswers = new Dictionary<int, string>();
        private DateTime _sessionStartTime;
        private DispatcherTimer? _sessionDurationTimer;
        private int _sessionElapsedSeconds = 0;
        private int _currentMaLuotLuyenTap = 0;
        private bool _isInstantFeedbackMode = true;
        private string _currentSessionTitle = "Luyện tập tự do";

        // Audio Player for Listening questions
        private readonly MediaPlayer _practiceMediaPlayer = new MediaPlayer();
        private Button? _currentPlayingAudioBtn = null;
        private int _currentPlayingQuestionId = -1;

        // List of wrong questions in current session for targeted re-practice
        private readonly List<CauHoiKhaoSatDTO> _currentSessionWrongQuestions = new List<CauHoiKhaoSatDTO>();

        // Cache for overall wrong questions in DB
        private List<CauHoiKhaoSatDTO> _dbWrongQuestions = new List<CauHoiKhaoSatDTO>();

        // User Score
        private int _score = 150;
        private bool _isInitialized = false;
        // Raw practice packages from DB
        private List<BaiTapDeThiDTO> _rawPracticePackages = new List<BaiTapDeThiDTO>();

        public LuyenTapView()
        {
            InitializeComponent();
            _isInitialized = true;
            this.PreviewKeyDown += LuyenTapView_PreviewKeyDown;
            InitAudioPlayer();

            // Nạp dữ liệu ban đầu
            LoadPracticePackages("All");
            UpdateLiveQuestionCount();
            CapNhatStreakHocVien();
            this.Loaded += (s, e) => CapNhatStreakHocVien();
        }

        public void CapNhatStreakHocVien()
        {
            try
            {
                int userId = SessionHelper.CurrentMaNguoiDung;
                int streak = TienDoHocDAL.Instance.TinhChuoiNgayHoc(userId);
                if (txtStreakText != null)
                {
                    txtStreakText.Text = $"Streak: {streak} ngày";
                }
            }
            catch { }
        }

        private void InitAudioPlayer()
        {
            AudioPlaybackCoordinator.Instance.RegisterActiveMediaPlayer(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    try { _practiceMediaPlayer.Stop(); } catch { }
                    if (_currentPlayingAudioBtn != null)
                    {
                        _currentPlayingAudioBtn.Content = "▶ Nghe bài nói";
                    }
                    _currentPlayingQuestionId = -1;
                });
            });

            _practiceMediaPlayer.MediaEnded += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_currentPlayingAudioBtn != null)
                    {
                        _currentPlayingAudioBtn.Content = "▶ Nghe lại bài nói";
                    }
                    _currentPlayingQuestionId = -1;
                });
            };

            _practiceMediaPlayer.MediaFailed += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_currentPlayingAudioBtn != null)
                    {
                        _currentPlayingAudioBtn.Content = "▶ Nghe bài nói (TTS)";
                    }
                    _currentPlayingQuestionId = -1;
                });
            };
        }

        // =========================================================================
        // SPEECH SYNTHESIS HELPER
        // =========================================================================
        private void SpeakText(string text, double speed = 1.0)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            AudioPlaybackCoordinator.Instance.SpeakAsync(text, speed);
        }

        // =========================================================================
        // TAB NAVIGATION & PRACTICE STATE GUARDS
        // =========================================================================
        public bool IsPracticeInProgress => pnlHubSession != null && pnlHubSession.Visibility == Visibility.Visible;

        public bool RequestExitPractice()
        {
            if (!IsPracticeInProgress) return true;

            bool xacNhan = Common.ThongBaoDialog.XacNhan(
                Window.GetWindow(this),
                "Bạn đang trong phiên luyện tập.\nNếu rời khỏi bây giờ, tiến trình làm bài hiện tại sẽ không được lưu.\n\nBạn có chắc chắn muốn thoát khỏi phiên luyện tập không?",
                "Xác nhận dừng luyện tập",
                "Dừng luyện tập",
                "Tiếp tục làm");

            if (xacNhan)
            {
                ExitPracticeMode();
                return true;
            }
            return false;
        }

        private void ExitPracticeMode()
        {
            _practiceMediaPlayer.Stop();
            _sessionDurationTimer?.Stop();

            // Nếu người dùng hủy phiên luyện tập giữa chừng, đánh dấu DaHuy
            if (_currentMaLuotLuyenTap > 0)
            {
                try
                {
                    LuyenTapBUS.Instance.HuyLuotLuyenTap(_currentMaLuotLuyenTap);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[LuyenTapView] Lỗi hủy phiên: {ex.Message}");
                }
                _currentMaLuotLuyenTap = 0;
            }

            pnlHubSession.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSetup.Visibility = Visibility.Visible;

            rowTopHeader.Height = new GridLength(64);
            pnlTopHeader.Visibility = Visibility.Visible;
            rowModeTabs.Height = GridLength.Auto;
            pnlModeTabs.Visibility = Visibility.Visible;
            MainWindow.Instance?.ShowSidebar();

            txtHeaderTitle.Text = "Trung tâm luyện tập linh hoạt & Kỹ năng";
            LoadPracticePackages("All");
            UpdateLiveQuestionCount();
        }

        private void TabHub_Click(object sender, RoutedEventArgs e)
        {
            if (IsPracticeInProgress && !RequestExitPractice()) return;
            _practiceMediaPlayer.Stop();
            SetActiveTab(tabHub, pnlHub);
            UpdateLiveQuestionCount();
        }

        private void TabWrongQuestions_Click(object sender, RoutedEventArgs e)
        {
            if (IsPracticeInProgress && !RequestExitPractice()) return;
            _practiceMediaPlayer.Stop();
            SetActiveTab(tabWrongQuestions, pnlWrongQuestions);
            LoadWrongQuestions();
        }

        private void TabHistory_Click(object sender, RoutedEventArgs e)
        {
            if (IsPracticeInProgress && !RequestExitPractice()) return;
            _practiceMediaPlayer.Stop();
            SetActiveTab(tabHistory, pnlHistory);
            LoadPracticeHistory();
        }

        private void TabFlashcard_Click(object sender, RoutedEventArgs e)
        {
            if (IsPracticeInProgress && !RequestExitPractice()) return;
            _practiceMediaPlayer.Stop();
            SetActiveTab(tabFlashcard, pnlFlashcard);
            LoadFlashcards();
        }

        private void TabDictation_Click(object sender, RoutedEventArgs e)
        {
            if (IsPracticeInProgress && !RequestExitPractice()) return;
            _practiceMediaPlayer.Stop();
            SetActiveTab(tabDictation, pnlDictation);
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
        // SECTION: DANH SÁCH BÀI TẬP & CHUYÊN ĐỀ RÈN LUYỆN (LoaiHinh == "LuyenTap")
        // =========================================================================
        private string _selectedPkgSkill = "All";

        public void LoadPracticePackages(string? filterKeyword = null)
        {
            try
            {
                // Chỉ lấy các đề thuộc nhóm Luyện tập tự do / Luyện tập theo chủ đề (LoaiHinh == "LuyenTap", DaPhatHanh == true)
                _rawPracticePackages = BaiTapDeThiBUS.Instance.LayDanhSachDeThi(chiLayDaXuatBan: true)
                    .Where(x => string.Equals(x.LoaiHinh, "LuyenTap", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapView] Lỗi nạp danh sách gói luyện tập: {ex.Message}");
                _rawPracticePackages = new List<BaiTapDeThiDTO>();
            }

            if (!string.IsNullOrWhiteSpace(filterKeyword) && filterKeyword != "All")
            {
                _selectedPkgSkill = filterKeyword;
                SyncPkgSkillButtonsVisual(filterKeyword);
            }

            ApplyPracticePackagesFilter();
        }

        private void ApplyPracticePackagesFilter()
        {
            if (_rawPracticePackages == null) return;

            var query = _rawPracticePackages.AsEnumerable();

            // 1. Lọc theo kỹ năng / chuyên đề
            if (!string.IsNullOrEmpty(_selectedPkgSkill) && _selectedPkgSkill != "All")
            {
                query = query.Where(x =>
                    (x.TenBaiTapDeThi != null && x.TenBaiTapDeThi.Contains(_selectedPkgSkill, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(x.MoTa) && x.MoTa.Contains(_selectedPkgSkill, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(x.TenChuDe) && x.TenChuDe.Contains(_selectedPkgSkill, StringComparison.OrdinalIgnoreCase))
                );
            }

            // 2. Lọc theo Trình độ
            if (cboPkgTrinhDo != null && cboPkgTrinhDo.SelectedItem is ComboBoxItem itemTrinhDo)
            {
                string trinhDo = itemTrinhDo.Content?.ToString() ?? "Tất cả";
                if (trinhDo != "Tất cả")
                {
                    query = query.Where(x =>
                        (!string.IsNullOrEmpty(x.MaCapDo) && x.MaCapDo.Equals(trinhDo, StringComparison.OrdinalIgnoreCase)) ||
                        (x.TenBaiTapDeThi != null && x.TenBaiTapDeThi.Contains(trinhDo, StringComparison.OrdinalIgnoreCase))
                    );
                }
            }

            // 3. Lọc theo từ khóa tìm kiếm
            if (txtSearchPkg != null)
            {
                string keyword = txtSearchPkg.Text.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(x =>
                        (x.TenBaiTapDeThi != null && x.TenBaiTapDeThi.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(x.MoTa) && x.MoTa.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(x.TenChuDe) && x.TenChuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(x.MaCapDo) && x.MaCapDo.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    );
                }
            }

            var displayList = query.ToList();

            var viewModels = displayList.Select(p => new PracticePackageViewModel
            {
                MaBaiTapDeThi = p.MaBaiTapDeThi,
                TenBaiTapDeThi = p.TenBaiTapDeThi,
                LoaiHinh = p.LoaiHinh,
                MoTa = p.MoTa,
                ThoiGianLamBaiPhut = p.ThoiGianLamBaiPhut > 0 ? p.ThoiGianLamBaiPhut : 20,
                SoLuongCauHoi = p.SoLuongCauHoi > 0 ? p.SoLuongCauHoi : (p.TongSoCau > 0 ? p.TongSoCau : 10),
                MaTrinhDo = !string.IsNullOrEmpty(p.MaCapDo) ? p.MaCapDo : "B2",
                DiemDatYeuCau = p.DiemDatYeuCau,
                TongDiem = p.TongDiem > 0 ? (decimal)p.TongDiem : 10m,
                MaCauTrucDeThi = p.MaCauTrucDeThi,
                TenCauTrucDeThi = p.TenCauTrucDeThi
            }).ToList();

            if (icDanhSachGoiLuyenTap != null)
            {
                icDanhSachGoiLuyenTap.ItemsSource = viewModels;
                icDanhSachGoiLuyenTap.Visibility = viewModels.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (pnlEmptyPracticePackages != null)
            {
                pnlEmptyPracticePackages.Visibility = viewModels.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void SyncPkgSkillButtonsVisual(string activeTag)
        {
            var brandTeal = (SolidColorBrush)FindResource("BrandTeal");
            var borderLight = (SolidColorBrush)FindResource("BorderLight");

            var buttons = new[] { btnPkgFilterAll, btnPkgFilterGrammar, btnPkgFilterVocab, btnPkgFilterReading, btnPkgFilterWriting, btnPkgFilterSpeaking, btnPkgFilterReflex };
            foreach (var btn in buttons)
            {
                if (btn == null) continue;
                string tag = btn.Tag?.ToString() ?? string.Empty;
                bool isMatch = string.Equals(tag, activeTag, StringComparison.OrdinalIgnoreCase);
                if (isMatch)
                {
                    btn.Background = brandTeal;
                    btn.Foreground = Brushes.White;
                    btn.BorderThickness = new Thickness(0);
                }
                else
                {
                    btn.Background = new SolidColorBrush(Color.FromRgb(248, 250, 252));
                    btn.Foreground = new SolidColorBrush(Color.FromRgb(71, 85, 105));
                    btn.BorderThickness = new Thickness(1);
                    btn.BorderBrush = borderLight;
                }
            }
        }

        private void BtnPkgFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            string tag = btn.Tag?.ToString() ?? "All";
            _selectedPkgSkill = tag;
            SyncPkgSkillButtonsVisual(tag);
            ApplyPracticePackagesFilter();
        }

        private void CboPkgFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApplyPracticePackagesFilter();
        }

        private void TxtSearchPkg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (btnClearSearchPkg != null)
            {
                btnClearSearchPkg.Visibility = string.IsNullOrEmpty(txtSearchPkg?.Text) ? Visibility.Collapsed : Visibility.Visible;
            }
            if (!_isInitialized) return;
            ApplyPracticePackagesFilter();
        }

        private void BtnClearSearchPkg_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearchPkg != null)
            {
                txtSearchPkg.Text = string.Empty;
                txtSearchPkg.Focus();
            }
        }

        private void BtnResetPkgFilter_Click(object sender, RoutedEventArgs e)
        {
            _selectedPkgSkill = "All";
            SyncPkgSkillButtonsVisual("All");
            if (cboPkgTrinhDo != null) cboPkgTrinhDo.SelectedIndex = 0;
            if (txtSearchPkg != null) txtSearchPkg.Text = string.Empty;
            ApplyPracticePackagesFilter();
        }
        private void BtnStartPracticePackage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag == null) return;
            if (!int.TryParse(btn.Tag.ToString(), out int maDe)) return;

            var pkg = _rawPracticePackages.FirstOrDefault(x => x.MaBaiTapDeThi == maDe);
            string packageTitle = pkg?.TenBaiTapDeThi ?? "Gói Luyện Tập Tự Do";

            try
            {
                var questions = BaiTapDeThiBUS.Instance.LayDanhSachCauHoiCuaDeThi(maDe);
                if (questions == null || questions.Count == 0)
                {
                    Common.ThongBaoDialog.CanhBao("Gói luyện tập này hiện chưa có câu hỏi trong cơ sở dữ liệu.", "Thông báo");
                    return;
                }

                StartPracticeWithQuestions(null, null, null, questions.Count, isInstant: true, sessionTitle: packageTitle, preloadedQuestions: questions);
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.Loi($"Không thể khởi chạy gói luyện tập: {ex.Message}", "Lỗi hệ thống");
            }
        }

        // =========================================================================
        // TAB 1: ADAPTIVE PRACTICE ENGINE (YÊU CẦU 4.1 - 4.4)
        // =========================================================================
        private (string? KyNang, string? CapDo, string? DoKho, int SoLuong, bool IsInstant, string SessionTitle) GetPracticeCriteria()
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
                else if (text.Contains("Speaking") || text.Contains("Nói")) kyNang = "Noi";
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

            string typeName = "Luyện tập củng cố";
            if (cboPracticeType?.SelectedItem is ComboBoxItem itemType)
            {
                string t = itemType.Content?.ToString() ?? "";
                if (t.Contains("phản xạ")) typeName = "Thử thách phản xạ nhanh";
                else if (t.Contains("chuyên sâu")) typeName = "Luyện tập chuyên sâu";
                else if (t.Contains("khắc phục")) typeName = "Ôn tập khắc phục điểm yếu";
                else if (t.Contains("củng cố")) typeName = "Luyện tập củng cố";
            }

            string skillName = kyNang != null ? LayTenTiengVietKyNang(kyNang) : "Tổng hợp";
            string sessionTitle = $"{typeName} - {skillName} ({soLuong} câu)";

            return (kyNang, capDo, doKho, soLuong, isInstant, sessionTitle);
        }

        private void PracticeFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            UpdateLiveQuestionCount();
        }

        private void UpdateLiveQuestionCount()
        {
            if (txtLiveAvailableCount == null) return;
            var (kyNang, capDo, doKho, _, _, _) = GetPracticeCriteria();

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

        private void BtnStartPracticeSession_Click(object sender, RoutedEventArgs e)
        {
            var (kyNang, capDo, doKho, soLuong, isInstant, sessionTitle) = GetPracticeCriteria();
            StartPracticeWithQuestions(kyNang, capDo, doKho, soLuong, isInstant, sessionTitle);
        }

        private void StartPracticeWithQuestions(
            string? kyNang, 
            string? capDo, 
            string? doKho, 
            int soLuong, 
            bool isInstant, 
            string sessionTitle = "Luyện tập tự do", 
            List<CauHoiKhaoSatDTO>? preloadedQuestions = null)
        {
            if (preloadedQuestions != null && preloadedQuestions.Count > 0)
            {
                _sessionQuestions = preloadedQuestions;
            }
            else
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
            }

            if (_sessionQuestions == null || _sessionQuestions.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Không thể tải danh sách câu hỏi. Vui lòng kiểm tra lại kết nối cơ sở dữ liệu.", "Thông báo");
                return;
            }

            // Khởi tạo phiên luyện tập
            _currentSessionTitle = sessionTitle;
            _sessionUserAnswers.Clear();
            _sessionUserTextAnswers.Clear();
            _currentSessionWrongQuestions.Clear();
            _isInstantFeedbackMode = isInstant;
            _sessionStartTime = DateTime.Now;
            _sessionElapsedSeconds = 0;

            // Khởi tạo lượt làm bài trạng thái 'DangLam' trong DB để giảng viên giám sát theo thời gian thực
            try
            {
                int currentUserId = SessionHelper.CurrentMaNguoiDung;
                if (currentUserId > 0)
                {
                    _currentMaLuotLuyenTap = LuyenTapBUS.Instance.KhoiTaoLuotLuyenTap(currentUserId, _currentSessionTitle, _sessionQuestions.Count);
                }
                else
                {
                    _currentMaLuotLuyenTap = 0;
                }
            }
            catch (Exception ex)
            {
                _currentMaLuotLuyenTap = 0;
                System.Diagnostics.Debug.WriteLine($"[LuyenTapView] Lỗi khởi tạo phiên luyện tập: {ex.Message}");
            }

            // Khởi động đồng hồ thời gian làm bài (đếm xuôi)
            StartSessionTimer();

            // Chế độ tập trung (Focus Mode): Ẩn Menu Trái, Header và Tabs
            rowTopHeader.Height = new GridLength(0);
            pnlTopHeader.Visibility = Visibility.Collapsed;
            rowModeTabs.Height = new GridLength(0);
            pnlModeTabs.Visibility = Visibility.Collapsed;
            MainWindow.Instance?.HideSidebar();

            // Chuyển view
            pnlHubSetup.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSession.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = _currentSessionTitle;

            UpdateSessionProgressText();
            RenderAllSessionQuestions();
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

        private void UpdateSessionProgressText()
        {
            int answered = _sessionUserAnswers.Count + _sessionUserTextAnswers.Count;
            int total = _sessionQuestions.Count;
            txtSessionProgress.Text = $"ĐÃ TRẢ LỜI: {answered} / {total} CÂU";
            txtBottomProgressText.Text = $"Đã trả lời: {answered} / {total} câu hỏi";
        }

        private void RenderAllSessionQuestions()
        {
            pnlQuestionsContainer.Children.Clear();

            for (int i = 0; i < _sessionQuestions.Count; i++)
            {
                var q = _sessionQuestions[i];

                var cardBorder = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1.5),
                    CornerRadius = new CornerRadius(14),
                    Padding = new Thickness(24, 20, 24, 20),
                    Margin = new Thickness(0, 0, 0, 18),
                    SnapsToDevicePixels = true
                };

                var cardStack = new StackPanel();
                cardBorder.Child = cardStack;

                // 1. Header Bar of Question
                var headerGrid = new Grid { Margin = new Thickness(0, 0, 0, 12) };
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var leftHeader = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };

                // Badge: Câu X
                var qNumBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(224, 242, 254)),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(10, 4, 10, 4),
                    Margin = new Thickness(0, 0, 8, 0)
                };
                qNumBorder.Child = new TextBlock
                {
                    Text = $"CÂU {i + 1}",
                    FontWeight = FontWeights.Bold,
                    FontSize = 12.5,
                    Foreground = new SolidColorBrush(Color.FromRgb(2, 132, 199))
                };
                leftHeader.Children.Add(qNumBorder);

                // Badge: Kỹ năng
                string tenKyNang = LayTenTiengVietKyNang(q.KyNang);
                var skillBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(8, 4, 8, 4),
                    Margin = new Thickness(0, 0, 8, 0)
                };
                skillBorder.Child = new TextBlock
                {
                    Text = tenKyNang,
                    FontWeight = FontWeights.Medium,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(71, 85, 105))
                };
                leftHeader.Children.Add(skillBorder);

                // Badge: Tên bài học (nếu có)
                if (!string.IsNullOrWhiteSpace(q.TenBaiHoc))
                {
                    var lessonBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(254, 243, 199)),
                        CornerRadius = new CornerRadius(6),
                        Padding = new Thickness(8, 4, 8, 4)
                    };
                    lessonBorder.Child = new TextBlock
                    {
                        Text = $"📖 {q.TenBaiHoc}",
                        FontWeight = FontWeights.Medium,
                        FontSize = 12,
                        Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9))
                    };
                    leftHeader.Children.Add(lessonBorder);
                }

                Grid.SetColumn(leftHeader, 0);
                headerGrid.Children.Add(leftHeader);

                // Level indicator
                var levelText = new TextBlock
                {
                    Text = $"Trình độ: {q.MaTrinhDo ?? "B1"}",
                    FontWeight = FontWeights.Normal,
                    FontSize = 12,
                    Foreground = (SolidColorBrush)FindResource("TextGray"),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(levelText, 1);
                headerGrid.Children.Add(levelText);

                cardStack.Children.Add(headerGrid);

                // 2. Reading passage (if any)
                if (!string.IsNullOrWhiteSpace(q.DoanVanDoc) && !q.IsKyNangNghe)
                {
                    var passageBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(203, 213, 225)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(10),
                        Padding = new Thickness(14, 12, 14, 12),
                        Margin = new Thickness(0, 0, 0, 14)
                    };
                    var passageStack = new StackPanel();
                    passageStack.Children.Add(new TextBlock
                    {
                        Text = "📄 Đoạn văn đọc hiểu:",
                        FontWeight = FontWeights.SemiBold,
                        FontSize = 12,
                        Foreground = (SolidColorBrush)FindResource("TextGray"),
                        Margin = new Thickness(0, 0, 0, 6)
                    });
                    passageStack.Children.Add(new TextBlock
                    {
                        Text = q.DoanVanDoc,
                        FontSize = 13.5,
                        LineHeight = 22,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = (SolidColorBrush)FindResource("TextDark")
                    });
                    passageBorder.Child = passageStack;
                    cardStack.Children.Add(passageBorder);
                }

                // 3. Audio listening box (attached from Lesson if listening question)
                if (q.IsKyNangNghe)
                {
                    var audioBorder = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(239, 246, 255)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(147, 197, 253)),
                        BorderThickness = new Thickness(1.5),
                        CornerRadius = new CornerRadius(10),
                        Padding = new Thickness(16, 12, 16, 12),
                        Margin = new Thickness(0, 0, 0, 14)
                    };

                    var audioGrid = new Grid();
                    audioGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    audioGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    var audioInfoStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                    string audioTitle = !string.IsNullOrWhiteSpace(q.TenBaiHoc) 
                        ? $"🎧 File nghe bài học: {q.TenBaiHoc}" 
                        : "🎧 Bài nghe kỹ năng Nghe";
                    audioInfoStack.Children.Add(new TextBlock
                    {
                        Text = audioTitle,
                        FontWeight = FontWeights.SemiBold,
                        FontSize = 13,
                        Foreground = new SolidColorBrush(Color.FromRgb(29, 78, 216))
                    });

                    var audioStatusTxt = new TextBlock
                    {
                        Text = "Bấm nút bên phải để nghe bài nói chuẩn.",
                        FontSize = 11.5,
                        Foreground = new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                        Margin = new Thickness(0, 2, 0, 0)
                    };
                    audioInfoStack.Children.Add(audioStatusTxt);

                    Grid.SetColumn(audioInfoStack, 0);
                    audioGrid.Children.Add(audioInfoStack);

                    var playAudioBtn = new Button
                    {
                        Content = "▶ Nghe bài nói",
                        Style = (Style)FindResource("TealActionButton"),
                        Padding = new Thickness(16, 8, 16, 8),
                        FontSize = 12.5,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    var currentQ = q;
                    playAudioBtn.Click += (s, e) => PlayAudioForQuestion(currentQ, playAudioBtn, audioStatusTxt);

                    Grid.SetColumn(playAudioBtn, 1);
                    audioGrid.Children.Add(playAudioBtn);

                    audioBorder.Child = audioGrid;
                    cardStack.Children.Add(audioBorder);
                }

                // 4. Question Prompt
                var promptText = new TextBlock
                {
                    Text = $"{i + 1}. {q.NoiDungCauHoi}",
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14.5,
                    Foreground = (SolidColorBrush)FindResource("TextDark"),
                    TextWrapping = TextWrapping.Wrap,
                    LineHeight = 22,
                    Margin = new Thickness(0, 0, 0, 14)
                };
                cardStack.Children.Add(promptText);

                // 5. Choices and Feedback Panel
                var feedbackBorder = new Border
                {
                    CornerRadius = new CornerRadius(10),
                    BorderThickness = new Thickness(1.5),
                    Padding = new Thickness(16, 12, 16, 12),
                    Margin = new Thickness(0, 12, 0, 0),
                    Visibility = Visibility.Collapsed
                };
                var feedbackStack = new StackPanel();
                var feedbackTitleTxt = new TextBlock { FontWeight = FontWeights.Bold, FontSize = 13.5, Margin = new Thickness(0, 0, 0, 4) };
                var feedbackCorrectTxt = new TextBlock { FontWeight = FontWeights.SemiBold, FontSize = 13, Foreground = new SolidColorBrush(Color.FromRgb(55, 65, 81)), Margin = new Thickness(0, 0, 0, 4) };
                var feedbackExpTxt = new TextBlock { FontSize = 12.5, Foreground = new SolidColorBrush(Color.FromRgb(75, 85, 99)), TextWrapping = TextWrapping.Wrap };
                feedbackStack.Children.Add(feedbackTitleTxt);
                feedbackStack.Children.Add(feedbackCorrectTxt);
                feedbackStack.Children.Add(feedbackExpTxt);
                feedbackBorder.Child = feedbackStack;

                if (q.IsDangLuaChon)
                {
                    var choicesStack = new StackPanel();
                    var optionButtonsList = new List<Button>();

                    foreach (var opt in q.DanhSachLuaChon)
                    {
                        var optBtn = new Button
                        {
                            Style = (Style)FindResource("OptionAnswerButton"),
                            Content = $"{opt.KyHieuLuaChon}. {CauHoiDTO.StripOptionPrefix(opt.NoiDungLuaChon)}",
                            FontSize = 13.5,
                            Tag = opt
                        };

                        optionButtonsList.Add(optBtn);
                        choicesStack.Children.Add(optBtn);

                        var capturedQ = q;
                        var capturedOpt = opt;
                        optBtn.Click += (s, e) =>
                        {
                            OnQuestionOptionSelected(capturedQ, capturedOpt, optionButtonsList, feedbackBorder, feedbackTitleTxt, feedbackCorrectTxt, feedbackExpTxt);
                        };
                    }

                    cardStack.Children.Add(choicesStack);
                }
                else
                {
                    // Free text input
                    var textInputGrid = new Grid { Margin = new Thickness(0, 0, 0, 8) };
                    textInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    textInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    var inputTxt = new TextBox
                    {
                        Height = 40,
                        FontSize = 13.5,
                        Padding = new Thickness(10, 6, 10, 6),
                        BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(inputTxt, 0);
                    textInputGrid.Children.Add(inputTxt);

                    var checkBtn = new Button
                    {
                        Content = "Ghi nhận",
                        Style = (Style)FindResource("TealActionButton"),
                        Padding = new Thickness(18, 0, 18, 0),
                        Height = 40,
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    Grid.SetColumn(checkBtn, 1);
                    textInputGrid.Children.Add(checkBtn);

                    var capturedQ = q;
                    checkBtn.Click += (s, e) =>
                    {
                        string answer = inputTxt.Text.Trim();
                        if (string.IsNullOrEmpty(answer))
                        {
                            Common.ThongBaoDialog.CanhBao("Vui lòng nhập câu trả lời của bạn.", "Thông báo");
                            return;
                        }

                        _sessionUserTextAnswers[capturedQ.MaCauHoi] = answer;
                        inputTxt.IsEnabled = false;
                        checkBtn.IsEnabled = false;
                        UpdateSessionProgressText();

                        var checkResult = LuyenTapBUS.Instance.KiemTraDapAn(capturedQ, null, answer);
                        bool isCorrect = checkResult.IsCorrect;
                        if (!isCorrect && !_currentSessionWrongQuestions.Any(w => w.MaCauHoi == capturedQ.MaCauHoi))
                            _currentSessionWrongQuestions.Add(capturedQ);

                        if (_isInstantFeedbackMode)
                        {
                            feedbackBorder.Visibility = Visibility.Visible;
                            if (isCorrect)
                            {
                                feedbackBorder.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                                feedbackBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                                feedbackTitleTxt.Text = "✅ CHÍNH XÁC!";
                                feedbackTitleTxt.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                                _score += 15;
                                txtUserScore.Text = $"Điểm tích lũy: {_score} pts";
                            }
                            else
                            {
                                feedbackBorder.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                                feedbackBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                                feedbackTitleTxt.Text = "❌ CHƯA CHÍNH XÁC";
                                feedbackTitleTxt.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                            }
                            feedbackCorrectTxt.Text = $"Đáp án mẫu: {capturedQ.DapAnDungText ?? "(Theo barem chuẩn)"}";
                            feedbackExpTxt.Text = !string.IsNullOrWhiteSpace(capturedQ.GiaiThichDapAn) ? $"💡 {capturedQ.GiaiThichDapAn}" : "💡 Hãy lưu ý chính tả.";
                        }
                    };

                    cardStack.Children.Add(textInputGrid);
                }

                cardStack.Children.Add(feedbackBorder);
                pnlQuestionsContainer.Children.Add(cardBorder);
            }
        }

        private void OnQuestionOptionSelected(
            CauHoiKhaoSatDTO q, 
            LuaChonDapAnDTO selectedOpt, 
            List<Button> optionButtons, 
            Border feedbackBorder, 
            TextBlock feedbackTitle, 
            TextBlock feedbackCorrect, 
            TextBlock feedbackExplanation)
        {
            _sessionUserAnswers[q.MaCauHoi] = selectedOpt.MaLuaChon;
            UpdateSessionProgressText();

            if (_isInstantFeedbackMode)
            {
                foreach (var b in optionButtons)
                {
                    b.IsEnabled = false;
                }

                bool isCorrect = selectedOpt.LaDapAnDung;
                if (!isCorrect)
                {
                    if (!_currentSessionWrongQuestions.Any(w => w.MaCauHoi == q.MaCauHoi))
                        _currentSessionWrongQuestions.Add(q);
                }

                foreach (var b in optionButtons)
                {
                    if (b.Tag is LuaChonDapAnDTO opt)
                    {
                        if (opt.LaDapAnDung)
                        {
                            b.Background = new SolidColorBrush(Color.FromRgb(209, 250, 229));
                            b.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                            b.Foreground = new SolidColorBrush(Color.FromRgb(6, 95, 70));
                            b.FontWeight = FontWeights.SemiBold;
                        }
                        else if (opt.MaLuaChon == selectedOpt.MaLuaChon)
                        {
                            b.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226));
                            b.BorderBrush = new SolidColorBrush(Color.FromRgb(239, 68, 68));
                            b.Foreground = new SolidColorBrush(Color.FromRgb(153, 27, 27));
                        }
                    }
                }

                feedbackBorder.Visibility = Visibility.Visible;
                if (isCorrect)
                {
                    feedbackBorder.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                    feedbackBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(110, 231, 183));
                    feedbackTitle.Text = "✅ CHÍNH XÁC!";
                    feedbackTitle.Foreground = new SolidColorBrush(Color.FromRgb(16, 185, 129));
                    _score += 15;
                    txtUserScore.Text = $"Điểm tích lũy: {_score} pts";
                }
                else
                {
                    feedbackBorder.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                    feedbackBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(252, 165, 165));
                    feedbackTitle.Text = "❌ CHƯA CHÍNH XÁC";
                    feedbackTitle.Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                }

                var correctOpt = q.DanhSachLuaChon.FirstOrDefault(o => o.LaDapAnDung);
                feedbackCorrect.Text = correctOpt != null 
                    ? $"Đáp án chính xác: {correctOpt.KyHieuLuaChon}. {CauHoiDTO.StripOptionPrefix(correctOpt.NoiDungLuaChon)}" 
                    : "";
                feedbackExplanation.Text = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) 
                    ? $"💡 Giải thích: {q.GiaiThichDapAn}" 
                    : "💡 Giải thích: Đáp án chuẩn theo ngân hàng câu hỏi đề thi.";
            }
            else
            {
                // Chế độ Tổng kết cuối phiên: KHÔNG HIỆN THÔNG BÁO "ĐÃ GHI NHẬN LỰA CHỌN"
                // Chỉ highlight nút được chọn như radio-button để học viên nhận biết rõ ràng
                foreach (var b in optionButtons)
                {
                    if (b.Tag is LuaChonDapAnDTO opt && opt.MaLuaChon == selectedOpt.MaLuaChon)
                    {
                        b.Background = (SolidColorBrush)FindResource("BrandMintSoft");
                        b.BorderBrush = (SolidColorBrush)FindResource("BrandTeal");
                        b.Foreground = (SolidColorBrush)FindResource("BrandTealDark");
                        b.FontWeight = FontWeights.SemiBold;
                    }
                    else
                    {
                        b.Background = Brushes.White;
                        b.BorderBrush = (SolidColorBrush)FindResource("BorderLight");
                        b.Foreground = (SolidColorBrush)FindResource("TextDark");
                        b.FontWeight = FontWeights.Normal;
                    }
                }

                feedbackBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void PlayAudioForQuestion(CauHoiKhaoSatDTO q, Button playBtn, TextBlock statusTxt)
        {
            if (_currentPlayingQuestionId == q.MaCauHoi)
            {
                _practiceMediaPlayer.Stop();
                _currentPlayingQuestionId = -1;
                playBtn.Content = "▶ Nghe bài nói";
                statusTxt.Text = "⏹️ Đã dừng phát bài nghe.";
                return;
            }

            if (_currentPlayingAudioBtn != null && _currentPlayingAudioBtn != playBtn)
            {
                _currentPlayingAudioBtn.Content = "▶ Nghe bài nói";
            }

            _currentPlayingAudioBtn = playBtn;
            _currentPlayingQuestionId = q.MaCauHoi;

            Uri? audioUri = HocTapView.ResolveAudioUri(q.AudioPath);
            if (audioUri != null)
            {
                try
                {
                    AudioPlaybackCoordinator.Instance.NotifyMediaPlayerStarting();
                    _practiceMediaPlayer.Stop();
                    _practiceMediaPlayer.Open(audioUri);
                    _practiceMediaPlayer.Play();
                    playBtn.Content = "⏸ Tạm dừng bài nghe";
                    statusTxt.Text = $"🎧 Đang phát âm thanh bài học ({System.IO.Path.GetFileName(audioUri.LocalPath)})...";
                    return;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[LuyenTapAudio] Lỗi phát MediaPlayer: {ex.Message}");
                }
            }

            // Fallback sang TTS AI nếu không có file âm thanh bài học
            string ttsContent = !string.IsNullOrWhiteSpace(q.DoanVanDoc) ? q.DoanVanDoc : q.NoiDungCauHoi;
            playBtn.Content = "🔊 Đang đọc bài...";
            statusTxt.Text = "🔊 Đang phát âm chuẩn bản xứ (TTS AI)...";
            SpeakText(ttsContent, 0.95);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(6) };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                if (_currentPlayingQuestionId == q.MaCauHoi)
                {
                    playBtn.Content = "▶ Nghe lại bài nói";
                    statusTxt.Text = "🎧 Bấm để nghe lại bài nói";
                    _currentPlayingQuestionId = -1;
                }
            };
            timer.Start();
        }

        private void BtnSubmitPracticeSession_Click(object sender, RoutedEventArgs e)
        {
            int answered = _sessionUserAnswers.Count + _sessionUserTextAnswers.Count;
            int total = _sessionQuestions.Count;

            if (answered < total)
            {
                bool xacNhan = Common.ThongBaoDialog.XacNhan(
                    $"Bạn mới trả lời {answered}/{total} câu hỏi.\nBạn có chắc chắn muốn nộp bài và xem kết quả tổng kết ngay không?",
                    "Xác nhận nộp bài",
                    "Nộp bài ngay", "Tiếp tục làm");
                if (!xacNhan) return;
            }

            FinishPracticeSession();
        }

        private void BtnStopPracticeSession_Click(object sender, RoutedEventArgs e)
        {
            _practiceMediaPlayer.Stop();
            bool xacNhan = Common.ThongBaoDialog.XacNhan(
                Window.GetWindow(this),
                "Bạn có chắc chắn muốn dừng phiên luyện tập hiện tại?\nKết quả các câu đã làm sẽ được tổng kết.",
                "Dừng phiên luyện tập",
                "Dừng ngay", "Tiếp tục làm");
            if (xacNhan)
            {
                FinishPracticeSession();
            }
        }

        private void FinishPracticeSession()
        {
            _practiceMediaPlayer.Stop();
            _sessionDurationTimer?.Stop();

            int timeSpent = Math.Max(5, _sessionElapsedSeconds);
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            string title = !string.IsNullOrWhiteSpace(_currentSessionTitle) ? _currentSessionTitle : "Luyện tập tự do";

            // Lưu kết quả phiên luyện tập vào database (HinhThucLam = 'LuyenTap', DangLam -> DaNop)
            try
            {
                LuyenTapBUS.Instance.LuuKetQuaPhienLuyenTap(
                    userId,
                    title,
                    _sessionStartTime,
                    timeSpent,
                    _sessionQuestions,
                    _sessionUserAnswers,
                    _sessionUserTextAnswers,
                    _currentMaLuotLuyenTap);
                _currentMaLuotLuyenTap = 0;
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
                if (isCorrect)
                {
                    correct++;
                    // Loại bỏ câu hỏi đã làm đúng ra khỏi danh sách câu sai
                    _dbWrongQuestions.RemoveAll(w => w.MaCauHoi == q.MaCauHoi);
                }
                else
                {
                    if (!_currentSessionWrongQuestions.Any(w => w.MaCauHoi == q.MaCauHoi))
                        _currentSessionWrongQuestions.Add(q);
                }

                string studentChoiceText = "(Chưa trả lời)";
                if (userOptId.HasValue)
                {
                    var opt = q.DanhSachLuaChon.FirstOrDefault(o => o.MaLuaChon == userOptId.Value);
                    if (opt != null) studentChoiceText = $"{opt.KyHieuLuaChon}. {CauHoiDTO.StripOptionPrefix(opt.NoiDungLuaChon)}";
                }
                else if (!string.IsNullOrWhiteSpace(userText))
                {
                    studentChoiceText = userText;
                }

                string correctText;
                if (q.IsDangLuaChon)
                {
                    var cOpt = q.DanhSachLuaChon.FirstOrDefault(o => o.LaDapAnDung);
                    correctText = cOpt != null ? $"{cOpt.KyHieuLuaChon}. {CauHoiDTO.StripOptionPrefix(cOpt.NoiDungLuaChon)}" : "Đang cập nhật";
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
                    ExplanationText = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) ? $"💡 {q.GiaiThichDapAn}" : "💡 Đáp án chuẩn theo ngân hàng đề thi.",
                    IsCorrect = isCorrect
                });
            }

            int wrong = total - correct;
            int accuracyPct = total > 0 ? (correct * 100) / total : 0;

            // Hiển thị Card tổng kết
            txtResultSummaryTitle.Text = $"TỔNG KẾT: {title.ToUpper()}";
            txtResultAccuracyPct.Text = $"{accuracyPct}%";
            txtResultCorrectText.Text = $"Đúng: {correct} câu";
            txtResultWrongText.Text = $"Sai: {wrong} câu";
            txtResultTimeSpent.Text = $"Thời gian: {timeSpent / 60:D2}:{timeSpent % 60:D2}";

            if (accuracyPct >= 80)
            {
                txtResultAiComment.Text = "AI Nhận xét: Bạn xử lý các câu hỏi rất xuất sắc! Hãy duy trì phong độ và thử thách với các câu hỏi khó hơn.";
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

            // Khôi phục Sidebar, Header và Tabs khi kết thúc phiên
            rowTopHeader.Height = new GridLength(64);
            pnlTopHeader.Visibility = Visibility.Visible;
            rowModeTabs.Height = GridLength.Auto;
            pnlModeTabs.Visibility = Visibility.Visible;
            MainWindow.Instance?.ShowSidebar();

            pnlHubSession.Visibility = Visibility.Collapsed;
            pnlHubSetup.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = $"Tổng kết: {title}";
        }

        private void BtnRePracticeWrongInSession_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSessionWrongQuestions.Count == 0) return;
            var wrongCopy = new List<CauHoiKhaoSatDTO>(_currentSessionWrongQuestions);
            StartPracticeWithQuestions(null, null, null, wrongCopy.Count, isInstant: true, sessionTitle: $"Ôn lại {wrongCopy.Count} câu sai trong phiên vừa rồi", preloadedQuestions: wrongCopy);
        }

        private void BtnNewPracticeSession_Click(object sender, RoutedEventArgs e)
        {
            _practiceMediaPlayer.Stop();
            _sessionDurationTimer?.Stop();

            rowTopHeader.Height = new GridLength(64);
            pnlTopHeader.Visibility = Visibility.Visible;
            rowModeTabs.Height = GridLength.Auto;
            pnlModeTabs.Visibility = Visibility.Visible;
            MainWindow.Instance?.ShowSidebar();

            pnlHubSession.Visibility = Visibility.Collapsed;
            pnlHubResult.Visibility = Visibility.Collapsed;
            pnlHubSetup.Visibility = Visibility.Visible;
            txtHeaderTitle.Text = "Trung tâm luyện tập linh hoạt & Kỹ năng";
            LoadPracticePackages("All");
            UpdateLiveQuestionCount();
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

            SetActiveTab(tabHub, pnlHub);
            var wrongCopy = new List<CauHoiKhaoSatDTO>(_dbWrongQuestions);
            StartPracticeWithQuestions(null, null, null, wrongCopy.Count, isInstant: true, sessionTitle: $"Ôn tập {wrongCopy.Count} câu hỏi bạn đã từng làm sai", preloadedQuestions: wrongCopy);
        }

        private void BtnDrillSingleWrong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag == null || !int.TryParse(btn.Tag.ToString(), out int maCauHoi)) return;

            var targetQ = _dbWrongQuestions.FirstOrDefault(q => q.MaCauHoi == maCauHoi);
            if (targetQ == null) return;

            SetActiveTab(tabHub, pnlHub);
            StartPracticeWithQuestions(null, null, null, 1, isInstant: true, sessionTitle: $"Luyện tập trọng điểm: Câu hỏi #{targetQ.MaCauHoi}", preloadedQuestions: new List<CauHoiKhaoSatDTO> { targetQ });
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

        #region FLASHCARD VOCABULARY & SPACED REPETITION
        private List<FlashcardDTO> _allFlashcards = new List<FlashcardDTO>();
        private List<FlashcardDTO> _filteredFlashcards = new List<FlashcardDTO>();
        private int _fcCurrentIndex = 0;
        private bool _fcIsFlipped = false;

        private void LoadFlashcards()
        {
            try
            {
                _allFlashcards = TuVungBUS.Instance.LayDanhSachFlashcard() ?? new List<FlashcardDTO>();
                UpdateFcStatistics();
                ApDungBoLocFlashcard();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LuyenTapView] Lỗi nạp flashcard: {ex.Message}");
            }
        }

        private void UpdateFcStatistics()
        {
            int tongTu = _allFlashcards.Count;
            int daThuoc = _allFlashcards.Count(x => x.DaThuoc);
            int canOn = Math.Max(0, tongTu - daThuoc);

            if (txtFcStatTongTu != null) txtFcStatTongTu.Text = tongTu.ToString();
            if (txtFcStatDaThuoc != null) txtFcStatDaThuoc.Text = daThuoc.ToString();
            if (txtFcStatCanOn != null) txtFcStatCanOn.Text = canOn.ToString();
        }

        private void ApDungBoLocFlashcard()
        {
            if (!_isInitialized) return;

            string? trinhDo = null;
            if (cboFcTrinhDo?.SelectedItem is ComboBoxItem itemTrinhDo)
            {
                string text = itemTrinhDo.Content?.ToString() ?? "";
                if (text.Contains("—"))
                {
                    trinhDo = text.Substring(0, text.IndexOf("—")).Trim();
                }
                else if (text != "Tất cả cấp độ")
                {
                    trinhDo = text.Trim();
                }
            }

            string trangThai = (cboFcTrangThai?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tất cả từ";

            var list = _allFlashcards.AsEnumerable();

            if (!string.IsNullOrEmpty(trinhDo))
            {
                list = list.Where(x => string.Equals(x.MaCapDo?.Trim(), trinhDo, StringComparison.OrdinalIgnoreCase));
            }

            if (trangThai == "Cần ôn tập")
            {
                list = list.Where(x => !x.DaThuoc);
            }
            else if (trangThai == "Đã thuộc")
            {
                list = list.Where(x => x.DaThuoc);
            }

            string keyword = txtFcSearch?.Text?.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(keyword))
            {
                list = list.Where(x => (x.Tu != null && x.Tu.ToLower().Contains(keyword)) ||
                                       (x.Nghia != null && x.Nghia.ToLower().Contains(keyword)) ||
                                       (x.TenChuDe != null && x.TenChuDe.ToLower().Contains(keyword)));
            }

            _filteredFlashcards = list.ToList();
            _fcCurrentIndex = 0;
            _fcIsFlipped = false;

            HienThiFcHienTai();
            HienThiFcDanhSach();
        }

        private void HienThiFcHienTai()
        {
            if (!_isInitialized || txtFcTienDoOnTap == null || txtFcBadgeDaThuoc == null) return;

            if (_filteredFlashcards.Count == 0)
            {
                txtFcTienDoOnTap.Text = "0 / 0";
                txtFcBadgeDaThuoc.Text = "Đã thuộc: 0/0";
                txtFcCardBadge.Text = "CHƯA CÓ TỪ VỰNG";
                txtFcMatTruocTu.Text = "Không có từ vựng phù hợp";
                txtFcMatTruocIPA.Text = "Thử chọn cấp độ khác";
                txtFcMatSauTu.Text = "";
                txtFcMatSauLoaiTu.Text = "";
                txtFcMatSauNghia.Text = "Hiện không có từ vựng nào thỏa mãn điều kiện lọc hiện tại.";
                txtFcMatSauViDu.Text = "";
                txtFcMatSauThongKe.Text = "";
                pnlFcMatTruoc.Visibility = Visibility.Visible;
                pnlFcMatSau.Visibility = Visibility.Collapsed;
                return;
            }

            if (_fcCurrentIndex < 0) _fcCurrentIndex = _filteredFlashcards.Count - 1;
            if (_fcCurrentIndex >= _filteredFlashcards.Count) _fcCurrentIndex = 0;

            var card = _filteredFlashcards[_fcCurrentIndex];
            txtFcTienDoOnTap.Text = $"Thẻ {_fcCurrentIndex + 1} / {_filteredFlashcards.Count}";

            int daThuocCount = _filteredFlashcards.Count(x => x.DaThuoc);
            txtFcBadgeDaThuoc.Text = $"Đã thuộc: {daThuocCount}/{_filteredFlashcards.Count}";

            // Mặt trước
            txtFcCardBadge.Text = $"{card.TenChuDe.ToUpper()} • {card.MaCapDo}";
            txtFcMatTruocTu.Text = card.Tu;
            txtFcMatTruocIPA.Text = !string.IsNullOrWhiteSpace(card.PhienAmIPA) ? card.PhienAmIPA : "(Chưa có phiên âm IPA)";

            // Mặt sau
            txtFcMatSauTu.Text = card.Tu;
            txtFcMatSauLoaiTu.Text = !string.IsNullOrWhiteSpace(card.LoaiTu) ? card.LoaiTu : "Từ vựng";
            txtFcMatSauNghia.Text = card.Nghia;
            txtFcMatSauViDu.Text = !string.IsNullOrWhiteSpace(card.CauViDu) ? $"\"{card.CauViDu}\"" : "\"No example sentence provided.\"";
            txtFcMatSauThongKe.Text = $"Đã ôn tập: {card.SoLanOnTap} lần • Ghi nhớ đúng: {card.SoLanDung} lần";

            // Trạng thái lật thẻ
            pnlFcMatTruoc.Visibility = _fcIsFlipped ? Visibility.Collapsed : Visibility.Visible;
            pnlFcMatSau.Visibility = _fcIsFlipped ? Visibility.Visible : Visibility.Collapsed;
        }

        private void HienThiFcDanhSach()
        {
            if (icFcVocabularyList != null)
            {
                icFcVocabularyList.ItemsSource = null;
                icFcVocabularyList.ItemsSource = _filteredFlashcards;
            }
            if (txtFcListHeader != null)
            {
                txtFcListHeader.Text = $"Kho từ vựng của bạn ({_filteredFlashcards.Count} từ)";
            }
        }

        private void CboFcTrinhDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApDungBoLocFlashcard();
        }

        private void CboFcTrangThai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApDungBoLocFlashcard();
        }

        private void BtnFcModeCard_Click(object sender, RoutedEventArgs e)
        {
            pnlFcCardView.Visibility = Visibility.Visible;
            pnlFcListView.Visibility = Visibility.Collapsed;
            pnlFcTienDo.Visibility = Visibility.Visible;

            btnFcModeCard.Background = (SolidColorBrush)FindResource("BrandTeal");
            btnFcModeCard.Foreground = Brushes.White;
            btnFcModeCard.BorderBrush = (SolidColorBrush)FindResource("BrandTeal");

            btnFcModeList.Background = new SolidColorBrush(Color.FromRgb(248, 250, 252));
            btnFcModeList.Foreground = new SolidColorBrush(Color.FromRgb(71, 85, 105));
            btnFcModeList.BorderBrush = (SolidColorBrush)FindResource("BorderLight");
        }

        private void BtnFcModeList_Click(object sender, RoutedEventArgs e)
        {
            pnlFcCardView.Visibility = Visibility.Collapsed;
            pnlFcListView.Visibility = Visibility.Visible;
            pnlFcTienDo.Visibility = Visibility.Collapsed;

            btnFcModeList.Background = (SolidColorBrush)FindResource("BrandTeal");
            btnFcModeList.Foreground = Brushes.White;
            btnFcModeList.BorderBrush = (SolidColorBrush)FindResource("BrandTeal");

            btnFcModeCard.Background = new SolidColorBrush(Color.FromRgb(248, 250, 252));
            btnFcModeCard.Foreground = new SolidColorBrush(Color.FromRgb(71, 85, 105));
            btnFcModeCard.BorderBrush = (SolidColorBrush)FindResource("BorderLight");
        }

        private void BdFcCard_Click(object sender, MouseButtonEventArgs e)
        {
            _fcIsFlipped = !_fcIsFlipped;
            pnlFcMatTruoc.Visibility = _fcIsFlipped ? Visibility.Collapsed : Visibility.Visible;
            pnlFcMatSau.Visibility = _fcIsFlipped ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnFcFlip_Click(object sender, RoutedEventArgs e)
        {
            _fcIsFlipped = !_fcIsFlipped;
            pnlFcMatTruoc.Visibility = _fcIsFlipped ? Visibility.Collapsed : Visibility.Visible;
            pnlFcMatSau.Visibility = _fcIsFlipped ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnFcPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            _fcCurrentIndex--;
            _fcIsFlipped = false;
            HienThiFcHienTai();
        }

        private void BtnFcNext_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            _fcCurrentIndex++;
            _fcIsFlipped = false;
            HienThiFcHienTai();
        }

        private void BtnFcChuaNho_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            var card = _filteredFlashcards[_fcCurrentIndex];
            TuVungBUS.Instance.CapNhatKetQuaFlashcard(card.MaTuVung, daNho: false);
            card.SoLanOnTap++;
            card.SoLanDung = 0;

            UpdateFcStatistics();

            _fcCurrentIndex++;
            _fcIsFlipped = false;
            HienThiFcHienTai();
            HienThiFcDanhSach();
        }

        private void BtnFcDaThuoc_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            var card = _filteredFlashcards[_fcCurrentIndex];
            TuVungBUS.Instance.CapNhatKetQuaFlashcard(card.MaTuVung, daNho: true);
            card.SoLanOnTap++;
            card.SoLanDung++;

            UpdateFcStatistics();

            _fcCurrentIndex++;
            _fcIsFlipped = false;
            HienThiFcHienTai();
            HienThiFcDanhSach();
        }

        private void BtnFcPhatAm_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (_filteredFlashcards.Count > 0 && _fcCurrentIndex < _filteredFlashcards.Count)
            {
                string word = _filteredFlashcards[_fcCurrentIndex].Tu;
                _ = AIPronunciationService.Instance.PlayPronunciationAsync(word);
            }
        }

        private void BtnFcItemAudio_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string word && !string.IsNullOrWhiteSpace(word))
            {
                _ = AIPronunciationService.Instance.PlayPronunciationAsync(word);
            }
        }

        private void TxtFcSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApDungBoLocFlashcard();
        }

        private void LuyenTapView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (pnlFlashcard.Visibility != Visibility.Visible || pnlFcCardView.Visibility != Visibility.Visible) return;
            if (txtFcSearch != null && txtFcSearch.IsFocused) return;

            if (e.Key == Key.Space)
            {
                e.Handled = true;
                BtnFcFlip_Click(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.Left)
            {
                e.Handled = true;
                BtnFcPrev_Click(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;
                BtnFcNext_Click(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                e.Handled = true;
                BtnFcChuaNho_Click(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                e.Handled = true;
                BtnFcDaThuoc_Click(this, new RoutedEventArgs());
            }
        }
        #endregion
    }
}
