using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using BUS;
using DAL;
using DTO;

namespace GUI.Views
{
    public partial class HoSoCaNhanView : Page
    {
        private List<DiemThiTheoThoiGianDTO> _currentLichSuThi = new();

        public HoSoCaNhanView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TaiThongTinHoSo();
            TaiLichSuThi();
        }

        // =========================================================================
        // 1. TẢI VÀ ĐỒNG BỘ THÔNG TIN HỒ SƠ & MỤC TIÊU
        // =========================================================================
        private void TaiThongTinHoSo()
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            // 1.1 Header Hero Banner
            txtStudentId.Text = $"ID Học Viên: EAI-2026-{user.MaNguoiDung:D4}";
            txtDisplayName.Text = user.HoTen;
            txtSubtitleInfo.Text = $"{user.Email} • Ngày tham gia: {user.NgayTao:dd/MM/yyyy} • Vai trò: {user.RoleText}";
            txtBannerJoinDate.Text = $"Tham gia: {user.NgayTao:dd/MM/yyyy}";

            // Hiển thị Avatar (hình ảnh thực tế hoặc chữ viết tắt)
            CapNhatHienThiAvatar(user.DuongDanAnhDaiDien, user.AvatarInitials);

            // 1.2 Điền thông tin vào Tab 1 (Thông tin cá nhân)
            txtUsername.Text = user.TenDangNhap;
            txtName.Text = user.HoTen;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.SoDienThoai ?? "";
            dpNgaySinh.SelectedDate = user.NgaySinh;
            txtJoinDateFull.Text = user.NgayTao.ToString("dd/MM/yyyy HH:mm");

            // 1.3 Tải Hồ sơ học tập và dữ liệu khảo sát/điểm thi
            var hoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(user.MaNguoiDung);
            var tongKet = TienDoHocBUS.Instance.LayTongKetTienDo(user.MaNguoiDung, "TatCa");
            var ketQuaGanNhat = KhaoSatBUS.Instance.LayKetQuaKhaoSatGanNhat(user.MaNguoiDung);

            if (hoSo != null)
            {
                SessionHelper.CurrentHoSo = hoSo;
            }

            // Điểm hiện tại (Ưu tiên điểm thi thực tế nếu có, sau đó là điểm khảo sát đầu vào)
            decimal diemHienTai = 5.0m;
            if (tongKet != null && tongKet.TongSoBaiThi > 0)
            {
                diemHienTai = (decimal)tongKet.DiemTrungBinh;
            }
            else if (hoSo?.DiemDanhGiaDauVao.HasValue == true)
            {
                diemHienTai = hoSo.DiemDanhGiaDauVao.Value;
            }
            else if (ketQuaGanNhat != null)
            {
                diemHienTai = ketQuaGanNhat.DiemSo;
            }

            string trinhDoHienTai = hoSo?.TrinhDoDauVao ?? ketQuaGanNhat?.TrinhDoPhuHop ?? "B1";
            string khungMucTieu = hoSo?.KhungNangLucMucTieu ?? "IELTS";
            decimal diemMucTieu = hoSo?.DiemMucTieu ?? 6.5m;

            // Cập nhật Header Badges
            txtBannerLevel.Text = $"Trình độ: CEFR {trinhDoHienTai}";
            txtBannerTarget.Text = $"{khungMucTieu} Target {diemMucTieu:F1}";

            // Cập nhật Tab 2 (Mục tiêu học tập): 3 Thẻ Metric
            txtScoreCurrent.Text = $"{diemHienTai:F1} / 10";
            txtSubScoreCurrent.Text = $"Tương đương IELTS ~{diemHienTai:F1}";

            txtLevelCurrent.Text = $"CEFR {trinhDoHienTai}";
            var thongTinLevel = KhaoSatBUS.LayThongTinTrinhDo(trinhDoHienTai);
            txtLevelNameCurrent.Text = thongTinLevel.Ten;

            txtTargetScoreDisplay.Text = $"{khungMucTieu} {diemMucTieu:F1}";
            txtTargetScaleText.Text = "Chuẩn đầu ra mong muốn";

            // Tính toán tiến độ và khoảng cách tiệm cận mục tiêu
            double targetVal = (double)diemMucTieu;
            double currentVal = (double)diemHienTai;
            if (targetVal <= 0) targetVal = 6.5;

            double phanTram = Math.Min(100.0, Math.Max(0.0, Math.Round((currentVal / targetVal) * 100.0, 0)));
            pbGoalProgress.Value = phanTram;
            txtGoalPercentage.Text = $"{phanTram:0}% Hoàn thành";

            double gap = Math.Round(targetVal - currentVal, 1);
            if (gap > 0)
            {
                txtGoalGapDescription.Text = $"Khoảng cách còn lại: Cần cải thiện thêm khoảng {gap:0.0} band để đạt ngưỡng mục tiêu {khungMucTieu} {diemMucTieu:F1}!";
            }
            else
            {
                txtGoalGapDescription.Text = $"🎉 Chúc mừng bạn đã chạm mốc hoặc vượt điểm mục tiêu {khungMucTieu} {diemMucTieu:F1}! Tiếp tục duy trì phong độ.";
            }

            // Đồng bộ ComboBox Mục tiêu
            DongBoComboBoxMucTieu(khungMucTieu, diemMucTieu, trinhDoHienTai);
            txtGoalNotes.Text = hoSo?.GhiChuCaNhan ?? "";
        }

        private void CapNhatHienThiAvatar(string? duongDanAnh, string initials)
        {
            if (!string.IsNullOrWhiteSpace(duongDanAnh) && File.Exists(duongDanAnh))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(duongDanAnh, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    imgAvatar.Source = bitmap;
                    imgAvatar.Visibility = Visibility.Visible;
                    txtAvatarInitials.Visibility = Visibility.Collapsed;
                    return;
                }
                catch
                {
                    // Fallback to initials if load failed
                }
            }

            imgAvatar.Visibility = Visibility.Collapsed;
            txtAvatarInitials.Visibility = Visibility.Visible;
            txtAvatarInitials.Text = initials;
        }

        private void DongBoComboBoxMucTieu(string khung, decimal diem, string trinhDo)
        {
            // Sync Khung chứng chỉ
            for (int i = 0; i < cboTargetCert.Items.Count; i++)
            {
                if (cboTargetCert.Items[i] is ComboBoxItem item && item.Content.ToString()!.Contains(khung))
                {
                    cboTargetCert.SelectedIndex = i;
                    break;
                }
            }

            // Sync Điểm mục tiêu
            string diemStr = diem.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
            bool foundScore = false;
            for (int i = 0; i < cboTargetScore.Items.Count; i++)
            {
                if (cboTargetScore.Items[i] is ComboBoxItem item && item.Content.ToString()!.Contains(diemStr))
                {
                    cboTargetScore.SelectedIndex = i;
                    foundScore = true;
                    break;
                }
            }
            if (!foundScore && cboTargetScore.Items.Count > 0)
            {
                cboTargetScore.SelectedIndex = 3; // Default 6.5
            }

            // Sync Trình độ hiện tại
            for (int i = 0; i < cboCurrentLevel.Items.Count; i++)
            {
                if (cboCurrentLevel.Items[i] is ComboBoxItem item && item.Content.ToString()!.StartsWith(trinhDo))
                {
                    cboCurrentLevel.SelectedIndex = i;
                    break;
                }
            }
        }

        // =========================================================================
        // 2. CHUYỂN ĐỔI TAB GIAO DIỆN
        // =========================================================================
        private void Tab_Checked(object sender, RoutedEventArgs e)
        {
            if (pnlTabThongTin == null || pnlTabMucTieu == null || pnlTabBaoMat == null || pnlTabLichSu == null)
                return;

            pnlTabThongTin.Visibility = rbTabThongTin.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            pnlTabMucTieu.Visibility = rbTabMucTieu.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            pnlTabBaoMat.Visibility = rbTabBaoMat.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            pnlTabLichSu.Visibility = rbTabLichSu.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;

            if (rbTabLichSu.IsChecked == true)
            {
                TaiLichSuThi();
            }
        }

        // =========================================================================
        // 3. CẬP NHẬT THÔNG TIN CÁ NHÂN & ẢNH ĐẠI DIỆN
        // =========================================================================
        private void BtnChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            var dialog = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện của bạn",
                Filter = "Tệp hình ảnh (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedFile = dialog.FileName;
                string targetFile = selectedFile;
                try
                {
                    string avatarDir = System.IO.Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                        "EnglishAI", "Avatars");
                    if (!System.IO.Directory.Exists(avatarDir)) 
                        System.IO.Directory.CreateDirectory(avatarDir);

                    string ext = System.IO.Path.GetExtension(selectedFile);
                    string targetName = $"avatar_{user.MaNguoiDung}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                    targetFile = System.IO.Path.Combine(avatarDir, targetName);
                    System.IO.File.Copy(selectedFile, targetFile, true);
                }
                catch
                {
                    targetFile = selectedFile;
                }

                bool ok = NguoiDungBUS.Instance.CapNhatAnhDaiDien(user.MaNguoiDung, targetFile);
                if (ok)
                {
                    user.DuongDanAnhDaiDien = targetFile;
                    CapNhatHienThiAvatar(targetFile, user.AvatarInitials);
                    MainWindow.Instance?.CapNhatThongTinHocVien();
                    Common.ThongBaoDialog.ThanhCong("✅ Cập nhật ảnh đại diện thành công!", "Đổi ảnh đại diện");
                }
                else
                {
                    Common.ThongBaoDialog.CanhBao("Không thể cập nhật ảnh đại diện vào cơ sở dữ liệu.", "Lỗi");
                }
            }
        }

        private void BtnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            string hoTen = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string soDienThoai = txtPhone.Text.Trim();
            DateTime? ngaySinh = dpNgaySinh.SelectedDate;

            var (success, message) = NguoiDungBUS.Instance.CapNhatHoSoSinhVien(
                user.MaNguoiDung, hoTen, email, soDienThoai, ngaySinh, user.DuongDanAnhDaiDien);

            if (success)
            {
                TaiThongTinHoSo();
                MainWindow.Instance?.CapNhatThongTinHocVien();
                Common.ThongBaoDialog.ThanhCong("✅ " + message, "Cập nhật thành công");
            }
            else
            {
                Common.ThongBaoDialog.CanhBao(message, "Thông tin chưa hợp lệ");
            }
        }

        // =========================================================================
        // 4. CẬP NHẬT MỤC TIÊU HỌC TẬP
        // =========================================================================
        private void CboTargetCert_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTargetCert == null || cboTargetScore == null) return;

            // Tự động điều chỉnh thang điểm phù hợp với chứng chỉ đã chọn
            string cert = (cboTargetCert.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            cboTargetScore.Items.Clear();

            if (cert.Contains("TOEIC"))
            {
                string[] toeicScores = { "550", "650", "700", "750", "800", "850", "900+" };
                foreach (var s in toeicScores) cboTargetScore.Items.Add(new ComboBoxItem { Content = s });
                cboTargetScore.SelectedIndex = 4; // 800
            }
            else if (cert.Contains("VSTEP"))
            {
                string[] vstepScores = { "B1 (Chuẩn tốt nghiệp)", "B2 (Chuẩn đầu ra)", "C1 (Giảng dạy)" };
                foreach (var s in vstepScores) cboTargetScore.Items.Add(new ComboBoxItem { Content = s });
                cboTargetScore.SelectedIndex = 1; // B2
            }
            else
            {
                // IELTS hoặc CEFR: thang điểm 4.5 - 9.0
                string[] ieltsScores = { "4.5", "5.0", "5.5", "6.0", "6.5", "7.0", "7.5", "8.0", "8.5", "9.0" };
                foreach (var s in ieltsScores) cboTargetScore.Items.Add(new ComboBoxItem { Content = s });
                cboTargetScore.SelectedIndex = 4; // 6.5
            }
        }

        private void BtnSaveGoals_Click(object sender, RoutedEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            // 1. Khung chứng chỉ
            string certText = (cboTargetCert.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "IELTS Academic";
            string khung = certText.Contains("TOEIC") ? "TOEIC" : (certText.Contains("VSTEP") ? "VSTEP" : "IELTS");

            // 2. Điểm mục tiêu
            string scoreText = (cboTargetScore.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "6.5";
            decimal diemMucTieu = 6.5m;
            if (decimal.TryParse(scoreText.Replace("+", "").Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal parsed))
            {
                diemMucTieu = parsed;
            }

            // 3. Trình độ hiện tại
            string currentLvlText = (cboCurrentLevel.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "B1";
            string trinhDoHienTai = currentLvlText.Length >= 2 ? currentLvlText.Substring(0, 2) : "B1";

            // 4. Ghi chú cá nhân
            string ghiChu = txtGoalNotes.Text.Trim();

            bool ok = HoSoHocTapDAL.Instance.CapNhatHoSoHocTap(user.MaNguoiDung, trinhDoHienTai, khung, diemMucTieu, ghiChu);
            if (ok)
            {
                TaiThongTinHoSo();
                MainWindow.Instance?.CapNhatThongTinHocVien();
                Common.ThongBaoDialog.ThanhCong("✅ Đã cập nhật mục tiêu học tập và lộ trình cá nhân vào hệ thống!", "Lưu mục tiêu thành công");
            }
            else
            {
                Common.ThongBaoDialog.CanhBao("Lỗi khi lưu mục tiêu vào CSDL.", "Thất bại");
            }
        }

        private void BtnRetakeSurvey_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.SetActiveMenu("DanhGia");
            this.NavigationService?.Navigate(new DanhGiaNangLucView(isFirstTime: false, forceNewSurvey: true));
        }

        // =========================================================================
        // 5. QUẢN LÝ BẢO MẬT & ĐỔI MẬT KHẨU
        // =========================================================================
        private void PbNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string pass = pbNewPassword.Password;
            if (string.IsNullOrEmpty(pass))
            {
                pbPasswordStrength.Value = 0;
                pbPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444"));
                txtPasswordStrength.Text = "Chưa nhập";
                txtPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
                return;
            }

            int score = 0;
            if (pass.Length >= 6) score += 30;
            if (pass.Length >= 8) score += 20;
            if (System.Text.RegularExpressions.Regex.IsMatch(pass, @"[0-9]")) score += 20;
            if (System.Text.RegularExpressions.Regex.IsMatch(pass, @"[A-Z]")) score += 15;
            if (System.Text.RegularExpressions.Regex.IsMatch(pass, @"[^a-zA-Z0-9]")) score += 15;

            pbPasswordStrength.Value = score;
            if (score < 50)
            {
                pbPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444")); // Red
                txtPasswordStrength.Text = "Yếu";
                txtPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444"));
            }
            else if (score < 80)
            {
                pbPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B")); // Amber
                txtPasswordStrength.Text = "Trung bình";
                txtPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D97706"));
            }
            else
            {
                pbPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981")); // Green
                txtPasswordStrength.Text = "Rất mạnh";
                txtPasswordStrength.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
            }
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            string currentPass = pbCurrentPassword.Password;
            string newPass = pbNewPassword.Password;
            string confirmPass = pbConfirmPassword.Password;

            var (success, message) = NguoiDungBUS.Instance.DoiMatKhauHocVien(user.MaNguoiDung, currentPass, newPass, confirmPass);
            if (success)
            {
                pbCurrentPassword.Clear();
                pbNewPassword.Clear();
                pbConfirmPassword.Clear();
                PbNewPassword_PasswordChanged(this, new RoutedEventArgs());

                Common.ThongBaoDialog.ThanhCong("✅ " + message, "Đổi mật khẩu thành công");
            }
            else
            {
                Common.ThongBaoDialog.CanhBao(message, "Đổi mật khẩu thất bại");
            }
        }

        // =========================================================================
        // 6. LỊCH SỬ THI VÀ XEM LẠI KẾT QUẢ
        // =========================================================================
        private void TaiLichSuThi()
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            try
            {
                _currentLichSuThi = TienDoHocBUS.Instance.LayLichSuDiemThi(user.MaNguoiDung, "TatCa");
                icExamHistory.ItemsSource = _currentLichSuThi;
                txtHistoryCount.Text = $"Tổng cộng: {_currentLichSuThi.Count} bài thi";

                if (_currentLichSuThi.Count == 0)
                {
                    pnlEmptyHistory.Visibility = Visibility.Visible;
                    icExamHistory.Visibility = Visibility.Collapsed;
                }
                else
                {
                    pnlEmptyHistory.Visibility = Visibility.Collapsed;
                    icExamHistory.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HoSoCaNhanView.TaiLichSuThi] Lỗi: {ex.Message}");
            }
        }

        private void BtnViewExamDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DiemThiTheoThoiGianDTO exam)
            {
                string statusDesc = exam.LaDat 
                    ? "🎉 Xin chúc mừng! Bạn đã đạt yêu cầu bài thi chuẩn hóa này." 
                    : "⚠️ Điểm số chưa đạt ngưỡng yêu cầu. Hãy ôn tập lại các chủ đề còn yếu.";

                Common.ThongBaoDialog.ThanhCong(
                    $"📊 KẾT QUẢ CHI TIẾT BÀI THI\n\n" +
                    $"• Đề thi: {exam.TenDeThi}\n" +
                    $"• Thời điểm nộp bài: {exam.NgayNopText}\n" +
                    $"• Thời gian làm bài: {exam.ThoiGianHienThi}\n" +
                    $"• Số câu đúng: {exam.SoCauDungText} câu\n" +
                    $"• Điểm số đạt được: {exam.DiemHienThi}\n" +
                    $"• Đánh giá: {exam.TrangThaiText} (Ngưỡng đạt: {exam.DiemDat:F1} điểm)\n\n" +
                    $"{statusDesc}",
                    "Chi tiết kết quả thi");
            }
        }

        private void BtnGoToThiThu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.SetActiveMenu("ThiThu");
            this.NavigationService?.Navigate(new ThiThuView());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            bool confirm = Common.ThongBaoDialog.XacNhan(
                Window.GetWindow(this), 
                "Bạn có chắc chắn muốn đăng xuất khỏi tài khoản học viên hiện tại?", 
                "Xác nhận đăng xuất", 
                confirmText: "Đăng xuất", 
                cancelText: "Ở lại");

            if (!confirm) return;

            SessionHelper.Logout();
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
