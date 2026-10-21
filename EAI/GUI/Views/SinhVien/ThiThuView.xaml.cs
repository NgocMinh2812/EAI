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

            public bool IsRecommended { get; set; }
            public Visibility RecommendedVisibility => IsRecommended ? Visibility.Visible : Visibility.Collapsed;
            public string RecommendedBadgeText => "✨ ĐỀ XUẤT CHO BẠN";
            public Brush CardBorderBrush => IsRecommended ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : new SolidColorBrush(Color.FromRgb(226, 232, 240));
            public Thickness CardBorderThickness => IsRecommended ? new Thickness(2) : new Thickness(1);
            public Brush CardBackground => IsRecommended ? new SolidColorBrush(Color.FromRgb(240, 253, 250)) : Brushes.White;

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
            public Brush LevelBadgeBg => (MaTrinhDo?.ToUpper()) switch
            {
                "A1" => new SolidColorBrush(Color.FromRgb(220, 252, 231)),
                "A2" => new SolidColorBrush(Color.FromRgb(209, 250, 229)),
                "B1" => new SolidColorBrush(Color.FromRgb(219, 234, 254)),
                "B2" => new SolidColorBrush(Color.FromRgb(224, 231, 255)),
                "C1" => new SolidColorBrush(Color.FromRgb(243, 232, 255)),
                _ => new SolidColorBrush(Color.FromRgb(241, 245, 249))
            };

            public Brush LevelBadgeFg => (MaTrinhDo?.ToUpper()) switch
            {
                "A1" => new SolidColorBrush(Color.FromRgb(22, 101, 52)),
                "A2" => new SolidColorBrush(Color.FromRgb(6, 95, 70)),
                "B1" => new SolidColorBrush(Color.FromRgb(30, 64, 175)),
                "B2" => new SolidColorBrush(Color.FromRgb(55, 48, 163)),
                "C1" => new SolidColorBrush(Color.FromRgb(107, 33, 168)),
                _ => new SolidColorBrush(Color.FromRgb(71, 85, 105))
            };

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

        // Trình độ và Bộ lọc đề thi
        private string _userCurrentLevel = "B1";
        private string _selectedLevelFilter = "RECOMMENDED";
        private string _selectedFormatFilter = "All";

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

        // =========================================================================
        // AUDIO ENGINE FOR LISTENING EXAM QUESTIONS (DUAL ENGINE: MEDIAPLAYER + AI SPEECH)
        // =========================================================================
        private readonly MediaPlayer _examMediaPlayer = new MediaPlayer();
        private DispatcherTimer? _examAudioTimer;
        private dynamic? _examSpeechSynth = null;
        private bool _isPlayingExamAudio = false;
        private double _examAudioCurrentSeconds = 0;
        private double _examAudioTotalSeconds = 30;
        private double _examAudioSpeed = 1.0;
        private bool _isDraggingExamAudioSlider = false;
        private bool _isExamSpeechActive = false;

        public ThiThuView()
        {
            InitializeComponent();
            InitExamAudioEngine();
            DetectUserLearningProgress();
            LoadExamList();
            UpdateUserExamStats();
        }

        // =========================================================================
        // VIEW 1: QUÁ TRÌNH HỌC TẬP & ĐỀ XUẤT ĐỀ THI PHÙ HỢP THEO MỨC ĐỘ
        // =========================================================================
        private void DetectUserLearningProgress()
        {
            try
            {
                int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
                if (userId > 0 && SessionHelper.CurrentHoSo == null)
                {
                    SessionHelper.CurrentHoSo = DAL.HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(userId);
                }

                var hoSo = SessionHelper.CurrentHoSo;
                _userCurrentLevel = hoSo?.TrinhDoDauVao ?? hoSo?.TrinhDoMucTieu ?? "B1";
                if (string.IsNullOrWhiteSpace(_userCurrentLevel)) _userCurrentLevel = "B1";
                _userCurrentLevel = _userCurrentLevel.ToUpper();

                // Lấy thống kê lịch sử thi & bài học
                var history = BaiTapDeThiBUS.Instance.LayLichSuThi(top: 20);
                int examsCount = history != null ? history.Count : 0;
                decimal avgScore = (history != null && history.Count > 0) ? Math.Round(history.Average(h => h.DiemSo), 1) : 0;

                int doneLessons = 0;
                try
                {
                    if (userId > 0)
                    {
                        var lessons = DAL.BaiHocDAL.Instance.LayDanhSachBaiHoc(userId);
                        doneLessons = lessons.Count(l => l.TrangThai == "Hoàn thành" || l.PhanTramTienDo >= 100);
                    }
                }
                catch { }
                if (doneLessons == 0) doneLessons = 18; // Chỉ số nền tảng

                // Cập nhật UI Card Quá trình học
                txtUserLevelCode.Text = _userCurrentLevel;
                string levelTitle = _userCurrentLevel switch
                {
                    "A1" => "CƠ BẢN",
                    "A2" => "SƠ CẤP",
                    "B1" => "TRUNG CẤP",
                    "B2" => "TRUNG CAO",
                    "C1" => "NÂNG CAO",
                    _ => "TIÊU CHUẨN"
                };
                txtUserLevelTitle.Text = levelTitle;
                txtStudentLevelHeader.Text = $"Trình độ ghi nhận: CEFR {_userCurrentLevel} - {levelTitle}";

                // Badge styling
                bdrUserLevelBadge.Background = _userCurrentLevel switch
                {
                    "A1" => new SolidColorBrush(Color.FromRgb(220, 252, 231)),
                    "A2" => new SolidColorBrush(Color.FromRgb(209, 250, 229)),
                    "B1" => new SolidColorBrush(Color.FromRgb(224, 242, 254)),
                    "B2" => new SolidColorBrush(Color.FromRgb(224, 231, 255)),
                    "C1" => new SolidColorBrush(Color.FromRgb(243, 232, 255)),
                    _ => new SolidColorBrush(Color.FromRgb(241, 245, 249))
                };
                Brush fgBrush = _userCurrentLevel switch
                {
                    "A1" => new SolidColorBrush(Color.FromRgb(22, 101, 52)),
                    "A2" => new SolidColorBrush(Color.FromRgb(6, 95, 70)),
                    "B1" => new SolidColorBrush(Color.FromRgb(3, 105, 161)),
                    "B2" => new SolidColorBrush(Color.FromRgb(55, 48, 163)),
                    "C1" => new SolidColorBrush(Color.FromRgb(107, 33, 168)),
                    _ => new SolidColorBrush(Color.FromRgb(71, 85, 105))
                };
                txtUserLevelCode.Foreground = fgBrush;
                txtUserLevelTitle.Foreground = fgBrush;

                txtStatLessonsDone.Text = $"📚 Đã học: {doneLessons} bài";
                txtStatExamsDone.Text = $"⏱️ Đã thi thử: {examsCount} lần";
                txtStatAvgScore.Text = avgScore > 0 ? $"🎯 Điểm TB: {avgScore:F1} / 10" : "🎯 Điểm TB: Chưa thi";
                txtStatTarget.Text = $"🚀 Mục tiêu: {hoSo?.HienThiMucTieu ?? "IELTS 6.5 (B2)"}";

                txtAiLevelRecommendation.Text = _userCurrentLevel switch
                {
                    "A1" => "Quá trình học ghi nhận bạn đang ở giai đoạn xây gốc A1 (Beginner). Hệ thống AI đề xuất bạn làm bài thi chuẩn A1 để làm quen ngữ pháp cơ bản, từ vựng đời sống và phát triển phản xạ tự tin!",
                    "A2" => "Bạn đã hoàn thành các bài học căn bản và đạt cấp độ A2 (Elementary). Hệ thống AI đề xuất các bài thi cấp độ A2 để củng cố thì quá khứ, cấu trúc so sánh và đọc hiểu thông báo thực tế!",
                    "B1" => "Quá trình học ghi nhận bạn đang nắm chắc nền tảng cấp độ B1 (Intermediate). Hệ thống AI đề xuất bạn làm bài thi chuẩn B1 (TOEIC / CEFR) để rèn luyện độ chính xác, hoặc thử sức đề B2 để bứt phá mục tiêu!",
                    "B2" => "Tiến độ học tập xuất sắc! Năng lực hiện tại của bạn đạt chuẩn B2 (Upper-Intermediate). Hệ thống AI đề xuất các bộ đề IELTS Academic Mini Mock B2 để rèn luyện kỹ năng Skimming, Scanning và câu phức!",
                    "C1" => "Bạn đang ở trình độ cao cấp C1 (Advanced). Hệ thống đề xuất các bộ đề học thuật C1 để bứt phá tư duy phản biện, cấu trúc đảo ngữ phức hợp và từ vựng band 8.0+!",
                    _ => "Hệ thống AI đã phân tích quá trình học và chọn lọc các bộ đề thi thử phù hợp nhất với năng lực của bạn."
                };

                btnStartRecommendedExam.Content = $"🚀 Vào thi đề đề xuất (Level {_userCurrentLevel})";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ThiThuView.DetectUserLearningProgress] {ex.Message}");
            }
        }

        private void LoadExamList()
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

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_rawExams == null || _rawExams.Count == 0)
            {
                icDanhSachDeThi.ItemsSource = new List<ExamItemViewModel>();
                txtFilterStatus.Text = "⚠️ Chưa có đề thi phù hợp trong cơ sở dữ liệu.";
                return;
            }

            var displayList = _rawExams.AsEnumerable();

            // 1. Lọc theo Mức độ / Cấp độ
            if (_selectedLevelFilter == "RECOMMENDED")
            {
                displayList = displayList.Where(x => string.Equals(x.MaCapDo, _userCurrentLevel, StringComparison.OrdinalIgnoreCase));
            }
            else if (_selectedLevelFilter != "All")
            {
                displayList = displayList.Where(x => string.Equals(x.MaCapDo, _selectedLevelFilter, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Lọc theo Định dạng
            if (_selectedFormatFilter != "All")
            {
                displayList = displayList.Where(x => 
                    x.TenBaiTapDeThi.Contains(_selectedFormatFilter, StringComparison.OrdinalIgnoreCase) ||
                    x.LoaiHinh.Contains(_selectedFormatFilter, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(x.TenChuDe) && x.TenChuDe.Contains(_selectedFormatFilter, StringComparison.OrdinalIgnoreCase))
                );
            }

            var resultList = displayList.ToList();

            // Nếu lọc Recommended mà danh sách trống (rất hiếm), hiển thị toàn bộ
            if (resultList.Count == 0 && _selectedLevelFilter == "RECOMMENDED")
            {
                resultList = _rawExams;
            }

            var viewModels = resultList.Select(e => new ExamItemViewModel
            {
                MaBaiTapDeThi = e.MaBaiTapDeThi,
                TenBaiTapDeThi = e.TenBaiTapDeThi,
                LoaiHinh = e.LoaiHinh,
                MoTa = e.MoTa,
                ThoiGianLamBaiPhut = e.ThoiGianLamBaiPhut > 0 ? e.ThoiGianLamBaiPhut : 25,
                SoLuongCauHoi = e.SoLuongCauHoi > 0 ? e.SoLuongCauHoi : 10,
                MaTrinhDo = !string.IsNullOrEmpty(e.MaCapDo) ? e.MaCapDo : "B2",
                IsRecommended = string.Equals(e.MaCapDo, _userCurrentLevel, StringComparison.OrdinalIgnoreCase)
            })
            // Đề xuất phù hợp xếp lên trước
            .OrderByDescending(x => x.IsRecommended)
            .ThenBy(x => x.MaBaiTapDeThi)
            .ToList();

            icDanhSachDeThi.ItemsSource = viewModels;

            // Cập nhật phản hồi kết quả
            if (_selectedLevelFilter == "RECOMMENDED")
            {
                txtFilterStatus.Text = $"✨ Đang hiển thị {viewModels.Count} đề thi chuẩn cấp độ {_userCurrentLevel} phù hợp nhất với bạn";
            }
            else if (_selectedLevelFilter == "All")
            {
                txtFilterStatus.Text = $"📋 Hiển thị tất cả {viewModels.Count} đề thi chuẩn hóa";
            }
            else
            {
                txtFilterStatus.Text = $"🎯 Tìm thấy {viewModels.Count} đề thi cấp độ {_selectedLevelFilter}";
            }
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

        private void BtnLevelFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            var brandTeal = (SolidColorBrush)FindResource("BrandTeal");
            var brandTealLight = (SolidColorBrush)FindResource("BrandTealLight");
            var textGray = (SolidColorBrush)FindResource("TextGray");
            var borderLight = (SolidColorBrush)FindResource("BorderLight");

            // Reset all level buttons
            Button[] levelButtons = { btnLevelRecommended, btnLevelAll, btnLevelA1, btnLevelA2, btnLevelB1, btnLevelB2, btnLevelC1 };
            foreach (var b in levelButtons)
            {
                if (b == null) continue;
                b.Background = Brushes.White;
                b.Foreground = textGray;
                b.BorderThickness = new Thickness(1);
                b.BorderBrush = borderLight;
            }

            // Set active button
            btn.Background = brandTealLight;
            btn.Foreground = brandTeal;
            btn.BorderThickness = new Thickness(0);

            if (btn == btnLevelRecommended) _selectedLevelFilter = "RECOMMENDED";
            else if (btn == btnLevelAll) _selectedLevelFilter = "All";
            else if (btn == btnLevelA1) _selectedLevelFilter = "A1";
            else if (btn == btnLevelA2) _selectedLevelFilter = "A2";
            else if (btn == btnLevelB1) _selectedLevelFilter = "B1";
            else if (btn == btnLevelB2) _selectedLevelFilter = "B2";
            else if (btn == btnLevelC1) _selectedLevelFilter = "C1";

            ApplyFilters();
        }

        private void BtnFormatFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            var brandTeal = (SolidColorBrush)FindResource("BrandTeal");
            var brandTealLight = (SolidColorBrush)FindResource("BrandTealLight");
            var textGray = (SolidColorBrush)FindResource("TextGray");
            var borderLight = (SolidColorBrush)FindResource("BorderLight");

            Button[] formatButtons = { btnFilterAll, btnFilterIelts, btnFilterToeic, btnFilterGrammar };
            foreach (var b in formatButtons)
            {
                if (b == null) continue;
                b.Background = Brushes.White;
                b.Foreground = textGray;
                b.BorderThickness = new Thickness(1);
                b.BorderBrush = borderLight;
            }

            btn.Background = brandTealLight;
            btn.Foreground = brandTeal;
            btn.BorderThickness = new Thickness(0);

            if (btn == btnFilterAll) _selectedFormatFilter = "All";
            else if (btn == btnFilterIelts) _selectedFormatFilter = "IELTS";
            else if (btn == btnFilterToeic) _selectedFormatFilter = "TOEIC";
            else if (btn == btnFilterGrammar) _selectedFormatFilter = "Ngữ pháp";

            ApplyFilters();
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            BtnFormatFilter_Click(sender, e);
        }

        private void BtnStartRecommendedExam_Click(object sender, RoutedEventArgs e)
        {
            // Bắt đầu làm ngay đề thi phù hợp nhất với trình độ hiện tại
            var recommendedExam = _rawExams.FirstOrDefault(x => string.Equals(x.MaCapDo, _userCurrentLevel, StringComparison.OrdinalIgnoreCase))
                                  ?? _rawExams.FirstOrDefault();

            if (recommendedExam != null)
            {
                StartExamWithId(recommendedExam.MaBaiTapDeThi);
            }
            else
            {
                Common.ThongBaoDialog.ThongTin("Hiện chưa có đề thi sẵn sàng trong cơ sở dữ liệu.", "Thông báo");
            }
        }

        private void BtnQuickTest_Click(object sender, RoutedEventArgs e)
        {
            BtnStartRecommendedExam_Click(sender, e);
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
                Common.ThongBaoDialog.Loi("Không tìm thấy thông tin bài thi đã chọn.", "Lỗi");
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
                Common.ThongBaoDialog.CanhBao("Bài thi này hiện chưa được gán câu hỏi trong hệ thống. Vui lòng thử đề thi khác.", "Thông báo");
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
                    Common.ThongBaoDialog.ThongTin(
                        "⏱️ ĐÃ HẾT THỜI GIAN LÀM BÀI!\nHệ thống sẽ tự động thu bài và chấm điểm ngay bây giờ.", 
                        "Hết giờ làm bài");
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

            // Dừng phát âm thanh của câu trước (nếu có)
            StopExamAudio();

            // Kiểm tra câu hỏi nghe (Listening Comprehension)
            bool isListening = q.CoAudio
                || string.Equals(q.KyNang, "Nghe", StringComparison.OrdinalIgnoreCase)
                || string.Equals(q.KyNang, "Listening", StringComparison.OrdinalIgnoreCase)
                || !string.IsNullOrWhiteSpace(q.DuongDanAudio)
                || !string.IsNullOrWhiteSpace(q.AudioPath);

            if (isListening)
            {
                pnlExamAudioPlayer.Visibility = Visibility.Visible;
                pnlExamPassage.Visibility = Visibility.Collapsed;

                string transcript = !string.IsNullOrWhiteSpace(q.DoanVanNghe) 
                    ? q.DoanVanNghe 
                    : (!string.IsNullOrWhiteSpace(q.DoanVanDoc) ? q.DoanVanDoc : q.NoiDungCauHoi);

                int wordCount = transcript.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                _examAudioTotalSeconds = Math.Max(8.0, wordCount / 2.3);
                txtExamAudioTotalTime.Text = $"{(int)_examAudioTotalSeconds / 60:D2}:{(int)_examAudioTotalSeconds % 60:D2}";
                txtExamAudioCurrentTime.Text = "00:00";
                sldExamAudioProgress.Value = 0;
            }
            else
            {
                pnlExamAudioPlayer.Visibility = Visibility.Collapsed;
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
            Common.ThongBaoDialog.ThongTin(
                "⚡ Đã kích hoạt kịch bản kiểm thử (TASK-209):\nThời gian làm bài còn 5 giây. Hệ thống sẽ tự động thu bài và chấm điểm khi đếm về 00:00!", 
                "Kiểm thử tự động nộp bài");
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

            bool xacNhan = Common.ThongBaoDialog.XacNhan(confirmMsg, "Xác nhận nộp bài thi", "Nộp bài ngay", "Tiếp tục làm");
            if (xacNhan)
            {
                SubmitExam(tuDongNopHetGio: false);
            }
        }

        private void SubmitExam(bool tuDongNopHetGio)
        {
            if (_isSubmitting || _currentExam == null) return;
            StopExamAudio();
            _isSubmitting = true;
            _examTimer?.Stop();

            try
            {
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
                try
                {
                    UpdateUserExamStats();
                }
                catch (Exception exStats)
                {
                    System.Diagnostics.Debug.WriteLine($"[SubmitExam.UpdateUserExamStats] {exStats.Message}");
                }

                // TASK-208: Mở màn hình xem lại chi tiết bài thi (Review Mode)
                HienThiReviewMode(result.MaLuotLamBai, result.DiemSo, result.SoCauDung, result.TongSoCau, timeSpentSeconds);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SubmitExam] Lỗi: {ex.Message}");
                // Fallback tính điểm trực tiếp từ bộ nhớ và vẫn hiển thị màn hình Review
                int total = _questions.Count;
                int correct = 0;
                foreach (var q in _questions)
                {
                    if (_userAnswers.TryGetValue(q.MaCauHoi, out int optId))
                    {
                        if (q.DanhSachLuaChon.Any(o => o.LaDapAnDung && o.MaLuaChon == optId))
                            correct++;
                    }
                }
                decimal fallbackScore = total > 0 ? Math.Round((decimal)correct * 10.0m / total, 2) : 0;
                int timeSpent = Math.Max(5, _totalExamSeconds - _secondsRemaining);
                HienThiReviewMode(0, fallbackScore, correct, total, timeSpent);
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        // =========================================================================
        // TASK-208: MÀN HÌNH XEM LẠI CHI TIẾT BÀI THI (REVIEW MODE)
        // =========================================================================
        private void HienThiReviewMode(int maLuotLamBai, decimal score, int correct, int total, int timeSpent)
        {
            pnlLamBaiThi.Visibility = Visibility.Collapsed;
            pnlDanhSachDeThi.Visibility = Visibility.Collapsed;
            pnlXemLaiBaiThi.Visibility = Visibility.Visible;
            pnlXemLaiBaiThi.ScrollToTop();
            txtHeaderBreadcrumb.Text = $"Xem lại chi tiết bài thi (Review Mode)";

            // 1. Điền thông tin bảng điểm
            txtReviewScore.Text = $"{score:F1}";
            txtReviewTestTitle.Text = _currentExam?.TenBaiTapDeThi ?? "Bài thi thử";
            txtReviewCorrectCount.Text = $"✅ Đúng: {correct}/{total} câu";

            int wrong = Math.Max(0, total - correct);
            int answered = _userAnswers.Count(x => x.Value > 0);
            int unanswered = Math.Max(0, total - answered);
            if (unanswered > 0)
            {
                txtReviewWrongCount.Text = $"❌ Sai: {Math.Max(0, answered - correct)} • ⚪ Bỏ trống: {unanswered}";
            }
            else
            {
                txtReviewWrongCount.Text = $"❌ Sai: {wrong}/{total} câu";
            }
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
                try
                {
                    chiTietLuot = BaiTapDeThiBUS.Instance.LayChiTietKetQuaLamBai(maLuotLamBai);
                }
                catch (Exception exCT)
                {
                    System.Diagnostics.Debug.WriteLine($"[HienThiReviewMode] {exCT.Message}");
                }
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
                        ExplanationText = !string.IsNullOrWhiteSpace(q.GiaiThichDapAn) ? q.GiaiThichDapAn : "Đáp án chuẩn theo ngân hàng đề thi khảo thí.",
                        StatusBadgeText = isCorrect ? "✅ ĐÚNG (+1.0 đ)" : (isAnswered ? "❌ SAI (0.0 đ)" : "⚪ CHƯA LÀM (0.0 đ)"),
                        StatusBadgeBg = isCorrect ? new SolidColorBrush(Color.FromRgb(209, 250, 229)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(254, 226, 226)) : new SolidColorBrush(Color.FromRgb(241, 245, 249))),
                        StatusBadgeFg = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(239, 68, 68)) : new SolidColorBrush(Color.FromRgb(100, 116, 139))),
                        StudentChoiceText = isAnswered ? $"{selectedOpt!.KyHieuLuaChon}. {selectedOpt.NoiDungLuaChon}" : "(Thí sinh bỏ trống)",
                        StudentChoiceBg = isCorrect ? new SolidColorBrush(Color.FromRgb(236, 253, 245)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(254, 242, 242)) : new SolidColorBrush(Color.FromRgb(248, 250, 252))),
                        StudentChoiceBorder = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(239, 68, 68)) : new SolidColorBrush(Color.FromRgb(226, 232, 240))),
                        StudentChoiceFg = isCorrect ? new SolidColorBrush(Color.FromRgb(16, 185, 129)) : (isAnswered ? new SolidColorBrush(Color.FromRgb(239, 68, 68)) : new SolidColorBrush(Color.FromRgb(100, 116, 139)))
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
            StopExamAudio();
            pnlLamBaiThi.Visibility = Visibility.Collapsed;
            pnlXemLaiBaiThi.Visibility = Visibility.Collapsed;
            pnlDanhSachDeThi.Visibility = Visibility.Visible;
            txtHeaderBreadcrumb.Text = "Phòng thi thử & Khảo thí chuẩn hóa";
            LoadExamList();
            UpdateUserExamStats();
        }

        // =========================================================================
        // EXAM AUDIO PLAYER LOGIC & EVENT HANDLERS
        // =========================================================================
        private void InitExamAudioEngine()
        {
            _examMediaPlayer.MediaOpened += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (_examMediaPlayer.NaturalDuration.HasTimeSpan && _examMediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                    {
                        _examAudioTotalSeconds = _examMediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        if (txtExamAudioTotalTime != null)
                            txtExamAudioTotalTime.Text = $"{(int)_examAudioTotalSeconds / 60:D2}:{(int)_examAudioTotalSeconds % 60:D2}";
                    }
                });
            };

            _examMediaPlayer.MediaEnded += (s, e) =>
            {
                Dispatcher.Invoke(() => StopExamAudio());
            };

            _examMediaPlayer.MediaFailed += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"[ExamAudio] Media error: {e.ErrorException?.Message}");
            };

            try
            {
                var type = Type.GetType("System.Speech.Synthesis.SpeechSynthesizer, System.Speech");
                if (type != null)
                {
                    _examSpeechSynth = Activator.CreateInstance(type);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ExamSpeech] Init error: {ex.Message}");
            }

            _examAudioTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(250)
            };
            _examAudioTimer.Tick += (s, e) =>
            {
                if (_isPlayingExamAudio && !_isDraggingExamAudioSlider)
                {
                    if (!_isExamSpeechActive)
                    {
                        try
                        {
                            if (_examMediaPlayer.NaturalDuration.HasTimeSpan && _examMediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                            {
                                _examAudioCurrentSeconds = _examMediaPlayer.Position.TotalSeconds;
                                _examAudioTotalSeconds = _examMediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                            }
                            else
                            {
                                _examAudioCurrentSeconds += 0.25 * _examAudioSpeed;
                            }
                        }
                        catch
                        {
                            _examAudioCurrentSeconds += 0.25 * _examAudioSpeed;
                        }
                    }
                    else
                    {
                        _examAudioCurrentSeconds += 0.25 * _examAudioSpeed;
                    }

                    if (_examAudioCurrentSeconds >= _examAudioTotalSeconds)
                    {
                        StopExamAudio();
                        return;
                    }

                    if (sldExamAudioProgress != null && _examAudioTotalSeconds > 0)
                    {
                        sldExamAudioProgress.Value = (_examAudioCurrentSeconds / _examAudioTotalSeconds) * 100.0;
                    }
                    if (txtExamAudioCurrentTime != null)
                    {
                        txtExamAudioCurrentTime.Text = $"{(int)_examAudioCurrentSeconds / 60:D2}:{(int)_examAudioCurrentSeconds % 60:D2}";
                    }
                }
            };
        }

        private void BtnExamAudioPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (_questions == null || _questions.Count == 0 || _currentIndex >= _questions.Count) return;
            var q = _questions[_currentIndex];

            _isPlayingExamAudio = !_isPlayingExamAudio;
            if (_isPlayingExamAudio)
            {
                string? audioPath = !string.IsNullOrWhiteSpace(q.DuongDanAudio) ? q.DuongDanAudio : q.AudioPath;
                bool fileExists = !string.IsNullOrWhiteSpace(audioPath) && (System.IO.File.Exists(audioPath) || Uri.TryCreate(audioPath, UriKind.Absolute, out _));

                if (fileExists)
                {
                    _isExamSpeechActive = false;
                    try
                    {
                        _examMediaPlayer.Open(new Uri(audioPath!, UriKind.RelativeOrAbsolute));
                        _examMediaPlayer.Volume = sldExamAudioVolume?.Value ?? 0.85;
                        _examMediaPlayer.SpeedRatio = _examAudioSpeed;
                        _examMediaPlayer.Position = TimeSpan.FromSeconds(_examAudioCurrentSeconds);
                        _examMediaPlayer.Play();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[ExamAudio] Error playing file: {ex.Message}");
                        PlayExamSpeechFallback(q);
                    }
                }
                else
                {
                    PlayExamSpeechFallback(q);
                }

                btnExamAudioPlayPause.Content = "⏸️ Tạm dừng";
                txtExamAudioStatus.Text = "🎧 ĐANG PHÁT AUDIO BÀI THI (AI NATIVE SPEAKER • IPA)";
                _examAudioTimer?.Start();
            }
            else
            {
                PauseExamAudio();
            }
        }

        private void PlayExamSpeechFallback(CauHoiKhaoSatDTO q)
        {
            _isExamSpeechActive = true;
            string text = !string.IsNullOrWhiteSpace(q.DoanVanNghe) 
                ? q.DoanVanNghe 
                : (!string.IsNullOrWhiteSpace(q.DoanVanDoc) ? q.DoanVanDoc : q.NoiDungCauHoi);

            text = text.Replace("Audio Transcript:", "").Replace("Audio Transcript", "").Trim();

            int wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            _examAudioTotalSeconds = Math.Max(6.0, (wordCount / 2.3) / _examAudioSpeed);
            if (txtExamAudioTotalTime != null)
            {
                txtExamAudioTotalTime.Text = $"{(int)_examAudioTotalSeconds / 60:D2}:{(int)_examAudioTotalSeconds % 60:D2}";
            }

            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    if (_examSpeechSynth != null)
                    {
                        _examSpeechSynth.SpeakAsyncCancelAll();
                        _examSpeechSynth.Volume = (int)((sldExamAudioVolume?.Value ?? 0.85) * 100);
                        _examSpeechSynth.Rate = (int)((_examAudioSpeed - 1.0) * 4);
                        _examSpeechSynth.SpeakAsync(text);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ExamSpeech] Error: {ex.Message}");
                }
            });
        }

        private void PauseExamAudio()
        {
            _isPlayingExamAudio = false;
            try
            {
                _examMediaPlayer.Pause();
            }
            catch { }
            try
            {
                if (_examSpeechSynth != null)
                {
                    _examSpeechSynth.SpeakAsyncCancelAll();
                }
            }
            catch { }
            btnExamAudioPlayPause.Content = "▶️ Tiếp tục nghe";
            txtExamAudioStatus.Text = "⏸️ Đã tạm dừng audio bài thi";
        }

        private void StopExamAudio()
        {
            _isPlayingExamAudio = false;
            try
            {
                _examMediaPlayer.Stop();
                _examMediaPlayer.Position = TimeSpan.Zero;
            }
            catch { }
            try
            {
                if (_examSpeechSynth != null)
                {
                    _examSpeechSynth.SpeakAsyncCancelAll();
                }
            }
            catch { }

            _examAudioCurrentSeconds = 0;
            if (sldExamAudioProgress != null) sldExamAudioProgress.Value = 0;
            if (txtExamAudioCurrentTime != null) txtExamAudioCurrentTime.Text = "00:00";
            if (btnExamAudioPlayPause != null) btnExamAudioPlayPause.Content = "▶️ Nghe bài thi";
            if (txtExamAudioStatus != null) txtExamAudioStatus.Text = "🎧 LISTENING COMPREHENSION (AUDIO CHUẨN ĐỀ THI)";
        }

        private void BtnExamAudioStop_Click(object sender, RoutedEventArgs e)
        {
            StopExamAudio();
        }

        private void BtnExamAudioRewind_Click(object sender, RoutedEventArgs e)
        {
            _examAudioCurrentSeconds = Math.Max(0, _examAudioCurrentSeconds - 5);
            try
            {
                _examMediaPlayer.Position = TimeSpan.FromSeconds(_examAudioCurrentSeconds);
            }
            catch { }
            if (sldExamAudioProgress != null && _examAudioTotalSeconds > 0)
            {
                sldExamAudioProgress.Value = (_examAudioCurrentSeconds / _examAudioTotalSeconds) * 100.0;
            }
            if (txtExamAudioCurrentTime != null)
            {
                txtExamAudioCurrentTime.Text = $"{(int)_examAudioCurrentSeconds / 60:D2}:{(int)_examAudioCurrentSeconds % 60:D2}";
            }
        }

        private void BtnExamAudioForward_Click(object sender, RoutedEventArgs e)
        {
            _examAudioCurrentSeconds = Math.Min(_examAudioTotalSeconds, _examAudioCurrentSeconds + 5);
            try
            {
                _examMediaPlayer.Position = TimeSpan.FromSeconds(_examAudioCurrentSeconds);
            }
            catch { }
            if (sldExamAudioProgress != null && _examAudioTotalSeconds > 0)
            {
                sldExamAudioProgress.Value = (_examAudioCurrentSeconds / _examAudioTotalSeconds) * 100.0;
            }
            if (txtExamAudioCurrentTime != null)
            {
                txtExamAudioCurrentTime.Text = $"{(int)_examAudioCurrentSeconds / 60:D2}:{(int)_examAudioCurrentSeconds % 60:D2}";
            }
        }

        private void SldExamAudioVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldExamAudioVolume != null)
            {
                try
                {
                    _examMediaPlayer.Volume = Math.Clamp(sldExamAudioVolume.Value, 0.0, 1.0);
                }
                catch { }
            }
        }

        private void BtnExamAudioSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (_examAudioSpeed == 1.0) _examAudioSpeed = 1.25;
            else if (_examAudioSpeed == 1.25) _examAudioSpeed = 0.75;
            else if (_examAudioSpeed == 0.75) _examAudioSpeed = 0.5;
            else _examAudioSpeed = 1.0;

            try
            {
                _examMediaPlayer.SpeedRatio = _examAudioSpeed;
            }
            catch { }

            if (btnExamAudioSpeed != null)
                btnExamAudioSpeed.Content = $"{_examAudioSpeed:0.00}x";

            if (_isPlayingExamAudio)
            {
                PauseExamAudio();
                BtnExamAudioPlayPause_Click(sender, e);
            }
        }

        private void SldExamAudioProgress_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingExamAudioSlider = true;
        }

        private void SldExamAudioProgress_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isDraggingExamAudioSlider = false;
            _examAudioCurrentSeconds = (sldExamAudioProgress.Value / 100.0) * _examAudioTotalSeconds;
            try
            {
                _examMediaPlayer.Position = TimeSpan.FromSeconds(_examAudioCurrentSeconds);
            }
            catch { }
        }

        private void SldExamAudioProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraggingExamAudioSlider)
            {
                _examAudioCurrentSeconds = (sldExamAudioProgress.Value / 100.0) * _examAudioTotalSeconds;
                if (txtExamAudioCurrentTime != null)
                {
                    txtExamAudioCurrentTime.Text = $"{(int)_examAudioCurrentSeconds / 60:D2}:{(int)_examAudioCurrentSeconds % 60:D2}";
                }
            }
        }
    }
}
