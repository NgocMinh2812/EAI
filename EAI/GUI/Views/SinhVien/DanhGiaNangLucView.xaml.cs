using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using BUS;
using DAL;
using DTO;

namespace GUI.Views
{
    public partial class DanhGiaNangLucView : Page
    {
        private string _selectedLevel = "B1";
        private List<CauHoiKhaoSatDTO> _questions = new List<CauHoiKhaoSatDTO>();
        private int _currentIndex = 0;
        private List<int> _userAnswers = new List<int>(); // 0: A, 1: B, 2: C, 3: D, -1: chưa chọn
        private DispatcherTimer? _timer;
        private int _secondsRemaining = 900; // 15 phút
        private KetQuaKhaoSatDTO? _currentResult;

        public bool IsFirstTime { get; set; } = false;

        public DanhGiaNangLucView(bool isFirstTime = false, bool forceNewSurvey = false)
        {
            InitializeComponent();
            IsFirstTime = isFirstTime;

            if (isFirstTime)
            {
                MainWindow.Instance?.HideSidebar();
            }

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;

            if (isFirstTime || forceNewSurvey)
            {
                // Người dùng mới đăng ký hoặc chủ động bấm làm bài mới: Hỏi mức độ trước
                HienThiGiaiDoanChonMucDo();
            }
            else
            {
                // Thành viên đã đăng ký: Kiểm tra kết quả khảo sát gần nhất
                KetQuaKhaoSatDTO? ketQuaCu = null;
                try
                {
                    ketQuaCu = KhaoSatBUS.Instance.LayKetQuaKhaoSatGanNhat(userId);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi lấy kết quả khảo sát cũ: {ex.Message}");
                }

                if (ketQuaCu != null)
                {
                    // Đã có kết quả: Hiển thị ngay kết quả và bài học đề xuất cho họ, không ép buộc làm lại
                    HienThiKetQuaVaDeXuat(ketQuaCu);
                }
                else
                {
                    // Chưa từng làm: Mở màn hình hỏi mức độ
                    HienThiGiaiDoanChonMucDo();
                }
            }
        }

        // =========================================================================
        // GIAI ĐOẠN 1: HỎI MỨC ĐỘ HIỆN TẠI (LEVEL SELECTION)
        // =========================================================================
        private void HienThiGiaiDoanChonMucDo()
        {
            _timer?.Stop();

            pnlChonMucDo.Visibility = Visibility.Visible;
            pnlLamKhaoSat.Visibility = Visibility.Collapsed;
            pnlKetQuaDeXuat.Visibility = Visibility.Collapsed;
        }

        private void BtnBatDauKhaoSat_Click(object sender, RoutedEventArgs e)
        {
            if (rbLevelA1.IsChecked == true) _selectedLevel = "A1";
            else if (rbLevelA2.IsChecked == true) _selectedLevel = "A2";
            else if (rbLevelB1.IsChecked == true) _selectedLevel = "B1";
            else if (rbLevelB2.IsChecked == true) _selectedLevel = "B2";
            else if (rbLevelC1.IsChecked == true) _selectedLevel = "C1";
            else _selectedLevel = "B1";

            BatDauKhaoSat(_selectedLevel);
        }

        private void BtnChonLaiMucDo_Click(object sender, RoutedEventArgs e)
        {
            HienThiGiaiDoanChonMucDo();
        }

        // =========================================================================
        // GIAI ĐOẠN 2: LÀM BÀI KHẢO SÁT NĂNG LỰC (QUIZ MODE)
        // =========================================================================
        private void BatDauKhaoSat(string level)
        {
            _selectedLevel = level;

            try
            {
                _questions = KhaoSatBUS.Instance.LayDanhSachCauHoiTheoTrinhDo(level);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi lấy câu hỏi theo trình độ: {ex.Message}");
                _questions = new List<CauHoiKhaoSatDTO>();
            }

            if (_questions == null || _questions.Count == 0)
            {
                _questions = KhaoSatBUS.TaoBoCauHoiDuPhongTheoCapDo(level);
            }

            _userAnswers = new List<int>(new int[_questions.Count]);
            for (int i = 0; i < _questions.Count; i++)
            {
                _userAnswers[i] = -1;
            }

            txtLevelTestingBadge.Text = $"Khảo sát năng lực • Trình độ {level}";

            // Chuyển view
            pnlChonMucDo.Visibility = Visibility.Collapsed;
            pnlLamKhaoSat.Visibility = Visibility.Visible;
            pnlKetQuaDeXuat.Visibility = Visibility.Collapsed;

            // Khởi động đồng hồ đếm ngược
            StartTimer();

            // Hiển thị câu hỏi đầu tiên
            LoadQuestion(0);
        }

        private void StartTimer()
        {
            _timer?.Stop();
            _secondsRemaining = Math.Max(300, _questions.Count * 90); // 1.5 phút / câu
            txtTimer.Text = $"{_secondsRemaining / 60:D2}:{_secondsRemaining % 60:D2}";

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) =>
            {
                if (_secondsRemaining > 0)
                {
                    _secondsRemaining--;
                    int minutes = _secondsRemaining / 60;
                    int seconds = _secondsRemaining % 60;
                    txtTimer.Text = $"{minutes:D2}:{seconds:D2}";
                }
                else
                {
                    _timer.Stop();
                    MessageBox.Show("Đã hết thời gian làm bài! Hệ thống sẽ tự động nộp bài và đánh giá năng lực của bạn.", 
                                    "Hết giờ", MessageBoxButton.OK, MessageBoxImage.Information);
                    BtnSubmit_Click(null, null);
                }
            };
            _timer.Start();
        }

        private void LoadQuestion(int index)
        {
            if (index < 0 || index >= _questions.Count) return;

            _currentIndex = index;
            var q = _questions[index];

            txtProgressCount.Text = $"Câu {index + 1} / {_questions.Count}";
            testProgressBar.Value = ((index + 1) * 100.0) / _questions.Count;

            txtCategoryBadge.Text = q.ChuyenMucHienThi;
            txtQuestionPrompt.Text = $"Câu {index + 1}: {q.NoiDungCauHoi}";

            if (!string.IsNullOrWhiteSpace(q.DoanVanDoc))
            {
                passageBox.Visibility = Visibility.Visible;
                txtPassage.Text = q.DoanVanDoc;
            }
            else
            {
                passageBox.Visibility = Visibility.Collapsed;
            }

            // Gán nội dung các đáp án
            txtOptionA.Text = q.DanhSachLuaChon.Count > 0 ? q.DanhSachLuaChon[0].NoiDungLuaChon : "A";
            txtOptionB.Text = q.DanhSachLuaChon.Count > 1 ? q.DanhSachLuaChon[1].NoiDungLuaChon : "B";
            txtOptionC.Text = q.DanhSachLuaChon.Count > 2 ? q.DanhSachLuaChon[2].NoiDungLuaChon : "C";
            txtOptionD.Text = q.DanhSachLuaChon.Count > 3 ? q.DanhSachLuaChon[3].NoiDungLuaChon : "D";

            // Khôi phục lựa chọn đã chọn
            int ans = _userAnswers[index];
            rbOptionA.IsChecked = (ans == 0);
            rbOptionB.IsChecked = (ans == 1);
            rbOptionC.IsChecked = (ans == 2);
            rbOptionD.IsChecked = (ans == 3);

            btnPrev.IsEnabled = (index > 0);

            if (index == _questions.Count - 1)
            {
                btnNext.Visibility = Visibility.Collapsed;
                btnSubmit.Visibility = Visibility.Visible;
            }
            else
            {
                btnNext.Visibility = Visibility.Visible;
                btnSubmit.Visibility = Visibility.Collapsed;
            }

            UpdateNextButtonState();
        }

        private void SaveCurrentSelection()
        {
            int selectedIdx = -1;
            if (rbOptionA.IsChecked == true) selectedIdx = 0;
            else if (rbOptionB.IsChecked == true) selectedIdx = 1;
            else if (rbOptionC.IsChecked == true) selectedIdx = 2;
            else if (rbOptionD.IsChecked == true) selectedIdx = 3;

            if (_currentIndex >= 0 && _currentIndex < _userAnswers.Count)
            {
                _userAnswers[_currentIndex] = selectedIdx;
            }

            if (_currentIndex >= 0 && _currentIndex < _questions.Count)
            {
                var q = _questions[_currentIndex];
                q.ChiMucDaChon = selectedIdx;
                if (selectedIdx >= 0 && selectedIdx < q.DanhSachLuaChon.Count)
                {
                    q.MaLuaChonDaChon = q.DanhSachLuaChon[selectedIdx].MaLuaChon;
                }
                else
                {
                    q.MaLuaChonDaChon = null;
                }
            }
        }

        private void UpdateNextButtonState()
        {
            bool hasAnswer = (_currentIndex >= 0 && _currentIndex < _userAnswers.Count && _userAnswers[_currentIndex] >= 0);
            btnNext.IsEnabled = hasAnswer;
            btnSubmit.IsEnabled = hasAnswer;
        }

        private void Option_Checked(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelection();
            UpdateNextButtonState();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelection();
            if (_currentIndex > 0)
            {
                LoadQuestion(_currentIndex - 1);
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentSelection();
            if (_userAnswers[_currentIndex] < 0)
            {
                MessageBox.Show("Vui lòng chọn một đáp án cho câu hỏi trước khi tiếp tục.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentIndex < _questions.Count - 1)
            {
                LoadQuestion(_currentIndex + 1);
            }
        }

        private void BtnSubmit_Click(object? sender, RoutedEventArgs? e)
        {
            SaveCurrentSelection();

            if (sender != null && _userAnswers[_currentIndex] < 0)
            {
                MessageBox.Show("Vui lòng chọn đáp án cho câu hỏi này trước khi nộp bài.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _timer?.Stop();

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;

            // Đánh giá thích ứng & lưu kết quả qua BUS
            var ketQua = KhaoSatBUS.Instance.DanhGiaThichUng(_selectedLevel, _questions, userId);

            // Đánh dấu người dùng đã hoàn thành khảo sát
            if (SessionHelper.CurrentUser != null)
            {
                SessionHelper.CurrentUser.LaLanDauDangNhap = false;
            }

            HienThiKetQuaVaDeXuat(ketQua);
        }

        // =========================================================================
        // GIAI ĐOẠN 3: HIỂN THỊ KẾT QUẢ VÀ ĐỀ XUẤT BÀI HỌC PHÙ HỢP
        // =========================================================================
        private void HienThiKetQuaVaDeXuat(KetQuaKhaoSatDTO ketQua)
        {
            _currentResult = ketQua;
            _timer?.Stop();

            pnlChonMucDo.Visibility = Visibility.Collapsed;
            pnlLamKhaoSat.Visibility = Visibility.Collapsed;
            pnlKetQuaDeXuat.Visibility = Visibility.Visible;

            // Hiển thị Điểm số & Số câu đúng
            txtScoreDisplay.Text = $"{ketQua.DiemSo:F1} / 10";
            int tongCau = Math.Max(1, ketQua.TongSoCau);
            int phanTram = (int)Math.Round(((double)ketQua.SoCauDung / tongCau) * 100);
            txtCorrectCount.Text = $"Đúng {ketQua.SoCauDung}/{ketQua.TongSoCau} câu ({phanTram}%)";

            // Hiển thị Trình độ đạt được
            txtLevelDisplay.Text = $"CEFR {ketQua.TrinhDoPhuHop}";
            txtLevelName.Text = ketQua.TenTrinhDoPhuHop;

            // Quy đổi tương đương
            txtIeltsDisplay.Text = !string.IsNullOrEmpty(ketQua.UocTinhIelts) ? ketQua.UocTinhIelts : "IELTS 5.0 - 5.5";
            txtToeicDisplay.Text = !string.IsNullOrEmpty(ketQua.UocTinhToeic) ? ketQua.UocTinhToeic : "TOEIC 550 - 650";

            // Phân tích thích ứng từ AI
            txtAiComment.Text = !string.IsNullOrEmpty(ketQua.LyDoDieuChinh) ? ketQua.LyDoDieuChinh :
                $"Chúc mừng bạn đã hoàn thành khảo sát năng lực! Hệ thống xác thực bạn đạt chuẩn trình độ {ketQua.TrinhDoPhuHop}.";
            txtAiRoadmap.Text = $"Lộ trình học tập đề xuất: {ketQua.NhanXetLộTrinh}";

            // Tiêu đề khối bài học đề xuất
            txtDeXuatHeader.Text = $"🎯 BÀI HỌC AI ĐỀ XUẤT CHO BẠN (TRÌNH ĐỘ {ketQua.TrinhDoPhuHop})";

            // Danh sách bài học đề xuất
            var dsBaiHoc = ketQua.DanhSachBaiHocDeXuat;
            if (dsBaiHoc == null || dsBaiHoc.Count == 0)
            {
                dsBaiHoc = BaiHocDAL.TaoDanhSachBaiHocDuPhongTheoTrinhDo(ketQua.TrinhDoPhuHop);
            }
            icDeXuatBaiHoc.ItemsSource = dsBaiHoc;

            // Cập nhật thông tin học viên trên Sidebar MainWindow
            MainWindow.Instance?.CapNhatThongTinHocVien();
        }

        private void BtnHocBaiDeXuat_Click(object sender, RoutedEventArgs e)
        {
            IsFirstTime = false;
            MainWindow.Instance?.ShowSidebar();

            if (sender is Button btn && btn.Tag != null)
            {
                if (int.TryParse(btn.Tag.ToString(), out int maBaiHoc))
                {
                    MainWindow.Instance?.SetActiveMenu("HocTap");
                    this.NavigationService?.Navigate(new HocTapView(maBaiHoc));
                    return;
                }
            }

            // Fallback mở trang học tập
            MainWindow.Instance?.SetActiveMenu("HocTap");
            this.NavigationService?.Navigate(new HocTapView());
        }

        private void BtnSkip_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();

            if (SessionHelper.CurrentUser != null)
            {
                NguoiDungBUS.Instance.CapNhatLaLanDauDangNhap(SessionHelper.CurrentUser.MaNguoiDung, false);
            }

            ChuyenVaoTrangChu();
        }

        private void BtnGoHome_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            ChuyenVaoTrangChu();
        }

        private void ChuyenVaoTrangChu()
        {
            IsFirstTime = false;
            MainWindow.Instance?.ShowSidebar();
            MainWindow.Instance?.CapNhatThongTinHocVien();
            MainWindow.Instance?.SetActiveMenu("TrangChu");
            this.NavigationService?.Navigate(new TrangChuView());
        }
    }
}
