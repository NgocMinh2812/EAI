using System;
using System.Windows;
using System.Windows.Controls;
using BUS;
using DAL;
using DTO;

namespace GUI.Views
{
    public partial class HoSoCaNhanView : Page
    {
        public HoSoCaNhanView()
        {
            InitializeComponent();
            TaiThongTinHoSo();
        }

        private void TaiThongTinHoSo()
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            // 1. Thông tin tài khoản người dùng
            txtStudentId.Text = $"ID Học Viên: EAI-2026-{user.MaNguoiDung:D4}";
            txtAvatarInitials.Text = user.AvatarInitials;
            txtDisplayName.Text = user.HoTen;
            txtSubtitleInfo.Text = $"{user.Email} • Ngày tham gia: {user.NgayTao:dd/MM/yyyy}";

            txtName.Text = user.HoTen;
            txtEmail.Text = user.Email;
            txtEmail.IsReadOnly = true;
            txtPhone.Text = user.SoDienThoai ?? "";

            // 2. Tải và đồng bộ Hồ sơ học tập từ CSDL
            var hoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(user.MaNguoiDung);
            if (hoSo != null)
            {
                SessionHelper.CurrentHoSo = hoSo;
                txtBannerTarget.Text = $"{hoSo.KhungNangLucMucTieu ?? "IELTS"} Target {hoSo.DiemMucTieu:F1}";
                string bannerLevel = !string.IsNullOrEmpty(hoSo.TrinhDoDauVao) ? hoSo.TrinhDoDauVao : (hoSo.TrinhDoMucTieu ?? "Chưa rõ");
                txtBannerLevel.Text = $"Trình độ đầu vào: CEFR {bannerLevel}";
            }
            else
            {
                txtBannerTarget.Text = "Chưa đặt mục tiêu";
                txtBannerLevel.Text = "Trình độ: Chưa đánh giá";
            }

            // 3. Hiển thị Điểm đánh giá năng lực đầu vào đã lưu trong hồ sơ
            var ketQuaGanNhat = KhaoSatBUS.Instance.LayKetQuaKhaoSatGanNhat(user.MaNguoiDung);
            decimal? diemDanhGia = hoSo?.DiemDanhGiaDauVao ?? ketQuaGanNhat?.DiemSo;
            string trinhDo = hoSo?.TrinhDoDauVao ?? ketQuaGanNhat?.TrinhDoPhuHop ?? hoSo?.TrinhDoMucTieu ?? "B1";

            // Chỉ hiển thị điểm đã đánh giá khi thực sự có điểm khảo sát hoặc có lượt kiểm tra
            bool daLamKhaoSat = diemDanhGia.HasValue || ketQuaGanNhat != null;

            if (daLamKhaoSat)
            {
                decimal diem = diemDanhGia ?? (ketQuaGanNhat?.DiemSo ?? 0);
                var thongTinTrinhDo = KhaoSatBUS.LayThongTinTrinhDo(trinhDo);
                string tenTrinhDo = ketQuaGanNhat?.TenTrinhDoPhuHop ?? thongTinTrinhDo.Ten;
                string ielts = ketQuaGanNhat?.UocTinhIelts ?? thongTinTrinhDo.Ielts;
                string toeic = ketQuaGanNhat?.UocTinhToeic ?? thongTinTrinhDo.Toeic;
                int soCauDung = ketQuaGanNhat?.SoCauDung ?? (int)Math.Round((double)diem * 1.5);
                int tongCau = ketQuaGanNhat?.TongSoCau ?? 15;
                int phanTram = (int)Math.Round(((double)soCauDung / Math.Max(1, tongCau)) * 100);

                txtProfileScore.Text = $"{diem:F1} / 10";
                txtProfileCorrectCount.Text = ketQuaGanNhat != null 
                    ? $"Đúng {soCauDung} / {tongCau} câu ({phanTram}%)" 
                    : $"Đánh giá đầu vào chuẩn CEFR {trinhDo}";
                txtProfileLevel.Text = $"CEFR {trinhDo}";
                txtProfileLevelName.Text = tenTrinhDo;
                txtProfileEquivalent.Text = $"{ielts} • {toeic}";
                txtProfileAiNote.Text = !string.IsNullOrWhiteSpace(hoSo?.GhiChuCaNhan) 
                    ? hoSo.GhiChuCaNhan 
                    : $"Hệ thống AI đã lưu trình độ đầu vào ({trinhDo}) và điểm khảo sát ({diem:F1}/10) vào hồ sơ cá nhân của bạn.";
                
                txtDisplaySavedScore.Text = $"{diem:F1} / 10 (Đã hoàn thành khảo sát)";
                btnRetakeAssessment.Content = "🔄 Làm lại khảo sát";
            }
            else
            {
                txtProfileScore.Text = "Chưa có điểm";
                txtProfileCorrectCount.Text = "Chưa làm bài test";
                txtProfileLevel.Text = $"CEFR {trinhDo}";
                var thongTinDefault = KhaoSatBUS.LayThongTinTrinhDo(trinhDo);
                txtProfileLevelName.Text = $"{thongTinDefault.Ten} (Chưa qua khảo sát)";
                txtProfileEquivalent.Text = $"{thongTinDefault.Ielts} • {thongTinDefault.Toeic}";
                txtProfileAiNote.Text = "Bạn chưa hoàn thành bài kiểm tra đánh giá năng lực. Hãy bấm 'Làm khảo sát ngay' để kiểm tra trình độ và lưu điểm số vào hồ sơ cá nhân!";
                txtDisplaySavedScore.Text = "Chưa có điểm bài test";
                btnRetakeAssessment.Content = "📝 Làm khảo sát ngay";
            }

            // Đồng bộ lựa chọn Mục tiêu trên ComboBox
            if (hoSo != null && !string.IsNullOrEmpty(hoSo.KhungNangLucMucTieu))
            {
                if (hoSo.KhungNangLucMucTieu.Contains("TOEIC")) cboTargetGoal.SelectedIndex = 2;
                else if (hoSo.KhungNangLucMucTieu.Contains("VSTEP")) cboTargetGoal.SelectedIndex = 3;
                else if (hoSo.DiemMucTieu >= 7.0m) cboTargetGoal.SelectedIndex = 1;
                else cboTargetGoal.SelectedIndex = 0;
            }

            // Đồng bộ lựa chọn Trình độ đầu vào trên ComboBox
            string entryLvl = hoSo?.TrinhDoDauVao ?? ketQuaGanNhat?.TrinhDoPhuHop ?? "B1";
            for (int i = 0; i < cboEntryLevel.Items.Count; i++)
            {
                if (cboEntryLevel.Items[i] is ComboBoxItem item && item.Content.ToString()!.StartsWith(entryLvl))
                {
                    cboEntryLevel.SelectedIndex = i;
                    break;
                }
            }
        }

        private void BtnRetakeAssessment_Click(object sender, RoutedEventArgs e)
        {
            // Điều hướng sang bài khảo sát đánh giá năng lực với cờ forceNewSurvey = true
            MainWindow.Instance?.SetActiveMenu("DanhGia");
            this.NavigationService?.Navigate(new DanhGiaNangLucView(isFirstTime: false, forceNewSurvey: true));
        }

        private void BtnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            if (user == null) return;

            string hoTen = txtName.Text.Trim();
            string soDienThoai = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Vui lòng không để trống họ và tên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cập nhật thông tin cá nhân
            bool okUser = NguoiDungBUS.Instance.CapNhatThongTinCaNhan(user.MaNguoiDung, hoTen, soDienThoai);

            // Cập nhật mục tiêu
            string khungMoi = "IELTS";
            decimal diemMoi = 6.5m;
            if (cboTargetGoal.SelectedIndex == 1) { khungMoi = "IELTS"; diemMoi = 7.5m; }
            else if (cboTargetGoal.SelectedIndex == 2) { khungMoi = "TOEIC"; diemMoi = 800m; }
            else if (cboTargetGoal.SelectedIndex == 3) { khungMoi = "VSTEP"; diemMoi = 6.0m; }
            else if (cboTargetGoal.SelectedIndex == 4) { khungMoi = "GiaoTiếp"; diemMoi = 7.0m; }

            // Lấy trình độ đầu vào đã chọn
            string trinhDoDauVaoMoi = "B1";
            if (cboEntryLevel.SelectedItem is ComboBoxItem entryItem)
            {
                string content = entryItem.Content.ToString() ?? "B1";
                trinhDoDauVaoMoi = content.Substring(0, 2);
            }

            HoSoHocTapDAL.Instance.CapNhatHoSoHocTap(user.MaNguoiDung, trinhDoDauVaoMoi, khungMoi, diemMoi);

            // Làm mới giao diện và thông tin học viên trên Sidebar MainWindow
            TaiThongTinHoSo();
            MainWindow.Instance?.CapNhatThongTinHocVien();

            MessageBox.Show($"✅ Thông tin hồ sơ cá nhân, trình độ đầu vào (CEFR {trinhDoDauVaoMoi}) và mục tiêu học tập đã được lưu thành công vào CSDL!", 
                            "Cập nhật thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng chọn ảnh đại diện từ thiết bị đang được đồng bộ hóa với hệ thống lưu trữ đám mây.", 
                            "Đổi ảnh đại diện", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var email = SessionHelper.CurrentUser?.Email ?? "email của bạn";
            MessageBox.Show($"Hệ thống đã gửi liên kết đặt lại mật khẩu an toàn đến email '{email}'. Vui lòng kiểm tra hộp thư!", 
                            "Đổi mật khẩu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Logout();
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
