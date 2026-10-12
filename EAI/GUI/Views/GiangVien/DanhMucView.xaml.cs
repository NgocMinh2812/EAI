using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BUS;
using DTO;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class DanhMucView : Page
    {
        private readonly DanhMucBUS _danhMucBUS = new DanhMucBUS();
        private List<BaiHocDTO> _allLessons = new List<BaiHocDTO>();
        private TreeNodeDTO? _selectedNode;
        private BaiHocDTO? _currentPreviewLesson;
        private const string SearchPlaceholder = "🔍 Tìm bài học / đề thi...";
        private bool _isLoaded = false;

        public DanhMucView()
        {
            InitializeComponent();
            _isLoaded = true;
            LoadTreeAndLessons();
        }

        private void LoadTreeAndLessons()
        {
            try
            {
                // 1. Tải cây chủ đề
                var treeNodes = _danhMucBUS.LayCayTreeNode();
                tvDanhMuc.ItemsSource = treeNodes;

                // Tự động chọn nút gốc
                if (treeNodes.Count > 0)
                {
                    _selectedNode = treeNodes[0];
                }

                // 2. Tải bài học tương ứng
                LoadLessonsForSelectedTopic();
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi tải cây danh mục từ cơ sở dữ liệu:\n{ex.Message}", "Lỗi CSDL");
            }
        }

        private void LoadLessonsForSelectedTopic()
        {
            if (!_isLoaded) return;

            try
            {
                int? topicId = (_selectedNode != null && _selectedNode.Id > 0) ? _selectedNode.Id : null;
                string? search = (txtSearchFile != null && txtSearchFile.Text != SearchPlaceholder) ? txtSearchFile.Text.Trim() : null;

                _allLessons = BaiHocBUS.LayDanhSachTuDatabase(topicId, search) ?? new List<BaiHocDTO>();

                if (icLessonList != null)
                {
                        icLessonList.ItemsSource = _allLessons;
                }

                if (txtLessonCounter != null)
                {
                    txtLessonCounter.Text = $"{_allLessons.Count} bài học";
                }

                if (txtEmptyNotice != null)
                {
                    txtEmptyNotice.Visibility = _allLessons.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }

                if (_allLessons.Count > 0)
                {
                    ShowLessonPreview(_allLessons[0]);
                }
                else
                {
                    ClearPreview();
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi tải danh sách bài học:\n{ex.Message}", "Lỗi dữ liệu");
            }
        }

        private void TvDanhMuc_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!_isLoaded) return;

            if (e.NewValue is TreeNodeDTO node)
            {
                _selectedNode = node;
                LoadLessonsForSelectedTopic();
            }
        }

        private void LessonItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.Tag is int id)
            {
                var lesson = _allLessons.FirstOrDefault(b => b.MaBaiHoc == id);
                if (lesson != null)
                {
                    ShowLessonPreview(lesson);
                }
            }
        }

        private void ShowLessonPreview(BaiHocDTO lesson)
        {
            _currentPreviewLesson = lesson;
            if (txtPreviewDocTitle == null) return;

            string fileName = !string.IsNullOrEmpty(lesson.TenTepVatLy) ? lesson.TenTepVatLy : (lesson.TieuDe ?? "Học liệu");
            txtPreviewDocTitle.Text = fileName;
            txtPreviewDocMeta.Text = $"Định dạng: {lesson.DinhDangTep} • Dung lượng: {lesson.DungLuongHienThi} • Chủ đề: {lesson.TenChuDe} • Trình độ: {lesson.MaTrinhDo}";
            txtSimulationModule.Text = $"{lesson.TenLoaiHienThi?.ToUpper() ?? "HỌC LIỆU"} — TRÌNH ĐỘ {lesson.MaTrinhDo ?? "B1"}";
            txtSimulationPassage.Text = $"Tóm tắt: {lesson.TomTat ?? ""}";

            txtPassageTitle.Text = lesson.TieuDe ?? "";
            txtPassageBody.Text = string.IsNullOrWhiteSpace(lesson.NoiDung) 
                ? "Nội dung bài học chưa được cập nhật chi tiết." 
                : lesson.NoiDung.Replace("<p>", "").Replace("</p>", "\n\n").Replace("<h3>", "■ ").Replace("</h3>", "\n").Replace("<ul>", "").Replace("</ul>", "").Replace("<li>", "• ").Replace("</li>", "\n");

            // Hiển thị câu hỏi bóc tách tương ứng với bài học hiện tại (bám sát nội dung bài, không bao giờ lệch sang chủ đề khác)
            if (lesson.DanhSachCauHoi == null || lesson.DanhSachCauHoi.Count == 0)
            {
                lesson.DanhSachCauHoi = GeminiAIService.TaoCauHoiThongMinhTheoNoiDung(lesson.TieuDe ?? "", lesson.NoiDung ?? "");
            }
            if (icExtractedQuestions != null)
            {
                icExtractedQuestions.ItemsSource = null;
                icExtractedQuestions.ItemsSource = lesson.DanhSachCauHoi;
            }

            // Cập nhật trạng thái động ở Footer Preview
            bool isPublished = string.Equals(lesson.TrangThai, "Đã xuất bản", StringComparison.OrdinalIgnoreCase);
            if (txtPreviewStatusIcon != null) txtPreviewStatusIcon.Text = isPublished ? "✓" : "✎";
            if (bdPreviewStatusIcon != null) bdPreviewStatusIcon.Background = isPublished 
                ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xDC, 0xFC, 0xE7)) 
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xF1, 0xF5, 0xF9));
            if (txtPreviewStatusPill != null)
            {
                txtPreviewStatusPill.Text = lesson.TrangThai;
                txtPreviewStatusPill.Foreground = isPublished 
                    ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x15, 0x80, 0x3D)) 
                    : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x64, 0x74, 0x8B));
            }
            if (bdPreviewStatusPill != null)
            {
                bdPreviewStatusPill.Background = isPublished 
                    ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xDC, 0xFC, 0xE7)) 
                    : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xF1, 0xF5, 0xF9));
            }
            if (txtPreviewStatusSub != null)
            {
                txtPreviewStatusSub.Text = isPublished 
                    ? "Sẵn sàng đưa vào giảng dạy & đề thi" 
                    : "Đang ở chế độ bản nháp nội bộ";
            }
        }

        private async void BtnReExtractAI_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null) return;

            if (pbAILoading != null) pbAILoading.Visibility = Visibility.Visible;
            if (btnReExtractAI != null) btnReExtractAI.IsEnabled = false;

            try
            {
                var aiQuestions = await GeminiAIService.BocTachCauHoiAIAsync(
                    _currentPreviewLesson.TieuDe ?? "", 
                    _currentPreviewLesson.NoiDung ?? "", 
                    _currentPreviewLesson.MaTrinhDo ?? "B1", 
                    2);

                if (aiQuestions != null && aiQuestions.Count > 0)
                {
                    _currentPreviewLesson.DanhSachCauHoi = aiQuestions;
                    if (icExtractedQuestions != null)
                    {
                        icExtractedQuestions.ItemsSource = null;
                        icExtractedQuestions.ItemsSource = _currentPreviewLesson.DanhSachCauHoi;
                    }
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), "Gemini AI đã bóc tách thành công 2 câu hỏi trắc nghiệm mới từ nội dung bài học!", "AI bóc tách thành công");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi bóc tách câu hỏi bằng AI:\n{ex.Message}", "Lỗi Gemini AI");
            }
            finally
            {
                if (pbAILoading != null) pbAILoading.Visibility = Visibility.Collapsed;
                if (btnReExtractAI != null) btnReExtractAI.IsEnabled = true;
            }
        }

        private void ClearPreview()
        {
            _currentPreviewLesson = null;
            if (txtPreviewDocTitle == null) return;

            txtPreviewDocTitle.Text = "Chưa chọn bài học";
            txtPreviewDocMeta.Text = "Vui lòng chọn bài học từ danh sách để xem trước";
            txtSimulationModule.Text = "HỌC LIỆU";
            if (icExtractedQuestions != null) icExtractedQuestions.ItemsSource = null;
            txtSimulationPassage.Text = "";
            txtPassageTitle.Text = "Không có dữ liệu bài học";
            txtPassageBody.Text = "Chủ đề này hiện chưa có bài học nào được tạo.";

            if (txtPreviewStatusPill != null) txtPreviewStatusPill.Text = "Chưa chọn";
            if (txtPreviewStatusSub != null) txtPreviewStatusSub.Text = "Chọn bài học để xem chi tiết";
        }

        private void TxtSearchFile_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchFile.Text == SearchPlaceholder)
            {
                txtSearchFile.Text = "";
                txtSearchFile.Foreground = (System.Windows.Media.Brush)FindResource("TextDark");
            }
        }

        private void TxtSearchFile_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchFile.Text))
            {
                txtSearchFile.Text = SearchPlaceholder;
                txtSearchFile.Foreground = (System.Windows.Media.Brush)FindResource("TextGray");
            }
        }

        private void TxtSearchFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isLoaded) return;
            LoadLessonsForSelectedTopic();
        }

        // ================= CRUD CHỦ ĐỀ HỌC LIỆU =================

        private void BtnThemChuDe_Click(object sender, RoutedEventArgs e)
        {
            var danhSach = _danhMucBUS.LayDanhSachChuDe();
            int? defaultParent = (_selectedNode != null && _selectedNode.Id > 0) ? _selectedNode.Id : null;

            var dlg = new ChuDeDialog(danhSach, null, defaultParent);
            if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                var (success, msg) = _danhMucBUS.ThemChuDe(dlg.ChuDeData);
                if (success)
                {
                    LoadTreeAndLessons();
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Thành công");
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi thêm chủ đề");
                }
            }
        }

        private void BtnSuaChuDe_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNode == null || _selectedNode.Id <= 0)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn một chủ đề cụ thể từ cây danh mục để sửa.", "Chưa chọn chủ đề");
                return;
            }

            var danhSach = _danhMucBUS.LayDanhSachChuDe();
            var current = danhSach.FirstOrDefault(c => c.MaChuDe == _selectedNode.Id);
            if (current == null)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), "Không tìm thấy dữ liệu chủ đề tương ứng.", "Lỗi");
                return;
            }

            var dlg = new ChuDeDialog(danhSach, current);
            if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                var (success, msg) = _danhMucBUS.CapNhatChuDe(dlg.ChuDeData);
                if (success)
                {
                    LoadTreeAndLessons();
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Cập nhật thành công");
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi cập nhật");
                }
            }
        }

        private void BtnXoaChuDe_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNode == null || _selectedNode.Id <= 0)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn một chủ đề cụ thể để xóa.", "Chưa chọn chủ đề");
                return;
            }

            bool confirm = ThongBaoDialog.XacNhanXoa(
                Window.GetWindow(this),
                $"Bạn có chắc chắn muốn xóa chủ đề: '{_selectedNode.Title}'?\nHệ thống sẽ kiểm tra ràng buộc bài học trước khi xóa.",
                "Xác nhận xóa danh mục");

            if (confirm)
            {
                // Kiểm tra ràng buộc và xóa qua BUS
                var (success, msg) = _danhMucBUS.XoaChuDe(_selectedNode.Id);
                if (success)
                {
                    _selectedNode = null;
                    LoadTreeAndLessons();
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Đã xóa chủ đề");
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Ràng buộc dữ liệu");
                }
            }
        }

        // ================= TOP HEADER BUTTONS =================

        private void BtnOpenFolderExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string candidate1 = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu"));
                string candidate2 = @"d:\CDPTUD1\EAI\KhoHocLieu";
                string targetPath = System.IO.Directory.Exists(candidate1) ? candidate1 : candidate2;

                if (System.IO.Directory.Exists(targetPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = targetPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), $"Chưa tìm thấy thư mục KhoHocLieu tại:\n{targetPath}", "Kho học liệu");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Không thể mở thư mục:\n{ex.Message}", "Lỗi");
            }
        }

        private void BtnSyncKhoHocLieu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var (created, total) = _danhMucBUS.DongBoKhoHocLieu();
                string root = BUS.KhoHocLieuSyncHelper.GetKhoHocLieuRoot();
                ThongBaoDialog.ThanhCong(
                    Window.GetWindow(this),
                    $"Đã kiểm tra và đồng bộ {total} danh mục chủ đề vào Kho học liệu!\n(Tạo mới {created} thư mục còn thiếu).\n\nĐường dẫn thư mục:\n{root}",
                    "Đồng bộ hoàn tất");
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi đồng bộ danh mục: {ex.Message}", "Lỗi");
            }
        }

        private void BtnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            var danhSach = _danhMucBUS.LayDanhSachChuDe();
            int? defaultChuDeId = (_selectedNode != null && _selectedNode.Id > 0) ? _selectedNode.Id : null;

            var dlg = new UploadTaiLieuDialog(danhSach, defaultChuDeId);
            if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                // Tải lại toàn bộ cây danh mục và danh sách bài học
                LoadTreeAndLessons();

                // Tự động tìm và xem trước bài học vừa tải lên
                if (dlg.BaiHocData.MaBaiHoc > 0)
                {
                    var newLesson = _allLessons.FirstOrDefault(b => b.MaBaiHoc == dlg.BaiHocData.MaBaiHoc) ?? dlg.BaiHocData;
                    ShowLessonPreview(newLesson);
                }

                ThongBaoDialog.ThanhCong(
                    Window.GetWindow(this), 
                    $"Đã bóc tách nội dung và lưu học liệu '{dlg.BaiHocData.TieuDe}' thành công vào Cơ sở dữ liệu!", 
                    "Tải lên tệp thành công");
            }
        }

        private void BtnNewFolder_Click(object sender, RoutedEventArgs e)
        {
            BtnThemChuDe_Click(sender, e);
        }

        private void BtnDownloadCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn bài học cần tải về.", "Chưa chọn tài liệu");
                return;
            }

            if (!string.IsNullOrEmpty(_currentPreviewLesson.DuongDanTepVatLy) && System.IO.File.Exists(_currentPreviewLesson.DuongDanTepVatLy))
            {
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = _currentPreviewLesson.TenTepVatLy,
                    Filter = "Tất cả tệp (*.*)|*.*"
                };
                if (sfd.ShowDialog() == true)
                {
                    System.IO.File.Copy(_currentPreviewLesson.DuongDanTepVatLy, sfd.FileName, true);
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), $"Đã tải tệp về máy tính thành công:\n{sfd.FileName}", "Tải về hoàn tất");
                }
            }
            else
            {
                ThongBaoDialog.ThongTin(Window.GetWindow(this), $"Đang chuẩn bị đóng gói bài học '{_currentPreviewLesson.TieuDe}' để tải về...", "Tải về");
            }
        }

        private void BtnPrintCurrent_Click(object sender, RoutedEventArgs e)
        {
            ThongBaoDialog.ThongTin(Window.GetWindow(this), "Đang kết nối dịch vụ in ấn để xuất bản tài liệu / đề thi...", "In ấn");
        }

        private void BtnAddToExam_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null || _currentPreviewLesson.MaBaiHoc <= 0)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn một bài học cụ thể từ danh sách để đưa vào đề thi thử.", "Chưa chọn học liệu");
                return;
            }

            string tenDeThi = $"Đề thi thử: {_currentPreviewLesson.TieuDe}";
            int? userId = SessionHelper.CurrentUser?.MaNguoiDung;

            var (success, msg) = BaiHocBUS.TaoDeThiThuTuBaiHoc(_currentPreviewLesson.MaBaiHoc, tenDeThi, userId);
            if (success)
            {
                ThongBaoDialog.ThanhCong(
                    Window.GetWindow(this), 
                    $"{msg}\nĐề thi thử mới đã được tạo và lưu vào bảng BaiTapDeThi thành công!", 
                    "Tạo Đề thi thử thành công");
            }
            else
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), msg, "Lỗi tạo đề thi");
            }
        }
    }
}
