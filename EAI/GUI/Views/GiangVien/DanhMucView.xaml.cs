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
        private List<TrinhDoDTO> _allTrinhDo = new List<TrinhDoDTO>();
        private TreeNodeDTO? _selectedNode;
        private BaiHocDTO? _currentPreviewLesson;
        private const string SearchPlaceholder = "Tìm bài học / đề thi...";
        private bool _isLoaded = false;
        private readonly System.Windows.Threading.DispatcherTimer _searchDebounceTimer;

        public DanhMucView()
        {
            InitializeComponent();
            _searchDebounceTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _searchDebounceTimer.Tick += (s, e) =>
            {
                _searchDebounceTimer.Stop();
                LoadLessonsForSelectedTopic();
            };

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
                string? search = null;
                if (txtSearchFile != null && !string.IsNullOrWhiteSpace(txtSearchFile.Text))
                {
                    string raw = txtSearchFile.Text.Trim();
                    if (!raw.Equals(SearchPlaceholder, StringComparison.OrdinalIgnoreCase) &&
                        !raw.Equals("🔍 " + SearchPlaceholder, StringComparison.OrdinalIgnoreCase) &&
                        !raw.Equals("🔍 Tìm bài học / đề thi...", StringComparison.OrdinalIgnoreCase) &&
                        !raw.Equals("Tìm bài học / đề thi...", StringComparison.OrdinalIgnoreCase))
                    {
                        search = raw;
                    }
                }

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
            txtSimulationPassage.Text = string.IsNullOrWhiteSpace(lesson.TomTat) ? "Xem trước nội dung tệp" : $"Tóm tắt: {lesson.TomTat}";

            txtPassageTitle.Text = lesson.TieuDe ?? "";

            // Đọc nội dung tệp thực tế từ ổ đĩa (DOCX, PDF, TXT) chuẩn như File Explorer
            string actualContent = KhoHocLieuSyncHelper.DocNoiDungTep(lesson.DuongDanTepVatLy, lesson.NoiDung);
            txtPassageBody.Text = string.IsNullOrWhiteSpace(actualContent) 
                ? "Nội dung bài học chưa được cập nhật chi tiết." 
                : actualContent;

            // KIỂM SOÁT HIỂN THỊ CÂU HỎI AI: Chưa bấm yêu cầu thì KHÔNG hiển thị câu hỏi, chỉ hiển thị nút tạo
            bool hasQuestions = lesson.DanhSachCauHoi != null && lesson.DanhSachCauHoi.Count > 0;
            if (pnlAIGeneratePrompt != null)
            {
                pnlAIGeneratePrompt.Visibility = hasQuestions ? Visibility.Collapsed : Visibility.Visible;
            }
            if (pnlExtractedQuestionsBox != null)
            {
                pnlExtractedQuestionsBox.Visibility = hasQuestions ? Visibility.Visible : Visibility.Collapsed;
            }
            if (icExtractedQuestions != null)
            {
                icExtractedQuestions.ItemsSource = hasQuestions ? lesson.DanhSachCauHoi : null;
            }

            // Cập nhật trạng thái động ở Footer Preview
            bool isPublished = string.Equals(lesson.TrangThai, "Đã xuất bản", StringComparison.OrdinalIgnoreCase);
            if (icoPreviewStatus != null)
            {
                icoPreviewStatus.Kind = isPublished ? LucideIconKind.Check : LucideIconKind.Pencil;
                icoPreviewStatus.Foreground = isPublished 
                    ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x15, 0x80, 0x3D)) 
                    : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x64, 0x74, 0x8B));
            }
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
            // Đánh dấu bài học đang chọn trong Cột 2 có viền màu nổi bật
            if (_allLessons != null)
            {
                foreach (var b in _allLessons)
                {
                    b.IsSelected = (b.MaBaiHoc == lesson.MaBaiHoc);
                }
                icLessonList?.Items.Refresh();
            }
        }

        private async void BtnReExtractAI_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null) return;

            if (pbAILoading != null) pbAILoading.Visibility = Visibility.Visible;
            if (btnReExtractAI != null) btnReExtractAI.IsEnabled = false;
            if (btnGenerateAIQuestions != null) btnGenerateAIQuestions.IsEnabled = false;

            try
            {
                // Sử dụng nội dung thực tế vừa được đọc từ tệp vật lý để AI phân tích
                string contentToAnalyze = KhoHocLieuSyncHelper.DocNoiDungTep(_currentPreviewLesson.DuongDanTepVatLy, _currentPreviewLesson.NoiDung);

                var aiQuestions = await GeminiAIService.BocTachCauHoiAIAsync(
                    _currentPreviewLesson.TieuDe ?? "", 
                    contentToAnalyze, 
                    _currentPreviewLesson.MaTrinhDo ?? "B1", 
                    10,
                    module: "DanhMucCauTruc",
                    loaiBaiHoc: _currentPreviewLesson.LoaiBaiHoc ?? "");

                if (aiQuestions != null && aiQuestions.Count > 0)
                {
                    _currentPreviewLesson.DanhSachCauHoi = aiQuestions;

                    // Hiển thị khung câu hỏi và ẩn khung mời tạo
                    if (pnlAIGeneratePrompt != null) pnlAIGeneratePrompt.Visibility = Visibility.Collapsed;
                    if (pnlExtractedQuestionsBox != null) pnlExtractedQuestionsBox.Visibility = Visibility.Visible;

                    if (icExtractedQuestions != null)
                    {
                        icExtractedQuestions.ItemsSource = null;
                        icExtractedQuestions.ItemsSource = _currentPreviewLesson.DanhSachCauHoi;
                    }

                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), $"AI đã phân tích nội dung tài liệu và tạo thành công {aiQuestions.Count} câu hỏi trắc nghiệm kèm giải thích chi tiết!", "Tạo câu hỏi bằng AI thành công");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi tạo câu hỏi bằng AI:\n{ex.Message}", "Lỗi tạo câu hỏi bằng AI");
            }
            finally
            {
                if (pbAILoading != null) pbAILoading.Visibility = Visibility.Collapsed;
                if (btnReExtractAI != null) btnReExtractAI.IsEnabled = true;
                if (btnGenerateAIQuestions != null) btnGenerateAIQuestions.IsEnabled = true;
            }
        }

        private void ClearPreview()
        {
            _currentPreviewLesson = null;
            if (txtPreviewDocTitle == null) return;

            txtPreviewDocTitle.Text = "Chưa chọn bài học";
            txtPreviewDocMeta.Text = "Vui lòng chọn bài học từ danh sách để xem trước";
            txtSimulationModule.Text = "HỌC LIỆU";
            if (pnlAIGeneratePrompt != null) pnlAIGeneratePrompt.Visibility = Visibility.Collapsed;
            if (pnlExtractedQuestionsBox != null) pnlExtractedQuestionsBox.Visibility = Visibility.Collapsed;
            if (icExtractedQuestions != null) icExtractedQuestions.ItemsSource = null;
            txtSimulationPassage.Text = "";
            txtPassageTitle.Text = "Không có dữ liệu bài học";
            txtPassageBody.Text = "Chủ đề này hiện chưa có bài học nào được tạo.";

            if (txtPreviewStatusPill != null) txtPreviewStatusPill.Text = "Chưa chọn";
        }

        private void TxtSearchFile_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchFile.Text == SearchPlaceholder ||
                txtSearchFile.Text == "🔍 " + SearchPlaceholder ||
                txtSearchFile.Text == "🔍 Tìm bài học / đề thi..." ||
                txtSearchFile.Text == "Tìm bài học / đề thi...")
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

            string text = txtSearchFile.Text;
            if (text == SearchPlaceholder || 
                text == "🔍 " + SearchPlaceholder || 
                text == "🔍 Tìm bài học / đề thi..." || 
                text == "Tìm bài học / đề thi...")
            {
                return;
            }

            // Tối ưu hiệu năng: thiết lập delay 2 giây sau khi người dùng ngừng gõ mới thực hiện truy vấn
            _searchDebounceTimer.Stop();
            if (txtLessonCounter != null && !string.IsNullOrWhiteSpace(text))
            {
                txtLessonCounter.Text = "Đang chờ...";
            }
            _searchDebounceTimer.Start();
        }

        private void TxtSearchFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Khi người dùng bấm phím Enter, lập tức tìm kiếm ngay mà không cần chờ hết 2 giây
                _searchDebounceTimer.Stop();
                LoadLessonsForSelectedTopic();
            }
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

            try
            {
                string? srcFile = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(_currentPreviewLesson);

                string ext = !string.IsNullOrEmpty(srcFile) 
                    ? System.IO.Path.GetExtension(srcFile)
                    : ((_currentPreviewLesson.DinhDangTep == "PDF") ? ".pdf" : ".docx");
                if (string.IsNullOrEmpty(ext)) ext = ".docx";

                string defaultName = !string.IsNullOrEmpty(_currentPreviewLesson.TenTepVatLy)
                    ? _currentPreviewLesson.TenTepVatLy
                    : $"{KhoHocLieuSyncHelper.SanitizeFolderName(_currentPreviewLesson.TieuDe)}{ext}";

                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = defaultName,
                    Filter = $"Tệp tài liệu (*{ext})|*{ext}|Tất cả tệp (*.*)|*.*"
                };

                if (sfd.ShowDialog() == true)
                {
                    if (!string.IsNullOrEmpty(srcFile) && System.IO.File.Exists(srcFile))
                    {
                        System.IO.File.Copy(srcFile, sfd.FileName, true);
                    }
                    else
                    {
                        if (ext.Equals(".docx", StringComparison.OrdinalIgnoreCase))
                        {
                            KhoHocLieuSyncHelper.TaoTepDocxTuBaiHoc(sfd.FileName, _currentPreviewLesson);
                        }
                        else
                        {
                            KhoHocLieuSyncHelper.TaoTepHtmlInAn(sfd.FileName, _currentPreviewLesson);
                        }
                    }

                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), $"Đã lưu tệp về máy tính thành công:\n{sfd.FileName}", "Tải về hoàn tất");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi tải tệp:\n{ex.Message}", "Lỗi tải về");
            }
        }

        private void BtnOpenFileExternal_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn bài học cần mở tệp.", "Chưa chọn tài liệu");
                return;
            }

            try
            {
                // 1. Thử tìm tệp vật lý thực tế trên ổ đĩa (trong TaiLieu_TestUpload hoặc KhoHocLieu)
                string? targetFilePath = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(_currentPreviewLesson);

                // 2. Nếu file vật lý thực tế tồn tại, khởi chạy trực tiếp với phần mềm mặc định của Windows (Word cho .docx, Edge/Reader cho .pdf)
                if (!string.IsNullOrEmpty(targetFilePath) && System.IO.File.Exists(targetFilePath))
                {
                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = targetFilePath,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(psi);
                    return;
                }

                // 3. Nếu bài học trong DB chưa có tệp vật lý, trích xuất tệp theo ĐÚNG định dạng được gán (DOCX hoặc PDF/HTML)
                string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EAI_HocLieu");
                if (!System.IO.Directory.Exists(tempDir))
                {
                    System.IO.Directory.CreateDirectory(tempDir);
                }

                string safeTitle = KhoHocLieuSyncHelper.SanitizeFolderName(_currentPreviewLesson.TieuDe ?? "TaiLieu");
                string format = (_currentPreviewLesson.DinhDangTep ?? "").ToUpperInvariant();

                if (format == "PDF")
                {
                    // Tạo tệp HTML giao diện in ấn / tài liệu chuẩn PDF và mở bằng trình duyệt mặc định (Edge/Chrome)
                    string tempHtml = System.IO.Path.Combine(tempDir, $"{safeTitle}.html");
                    KhoHocLieuSyncHelper.TaoTepHtmlInAn(tempHtml, _currentPreviewLesson);

                    var tempPsi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = tempHtml,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(tempPsi);
                }
                else
                {
                    // Tạo tệp DOCX thực thụ (Microsoft Word Document chuẩn OpenXML)
                    string tempDocx = System.IO.Path.Combine(tempDir, $"{safeTitle}.docx");
                    KhoHocLieuSyncHelper.TaoTepDocxTuBaiHoc(tempDocx, _currentPreviewLesson);

                    var tempPsi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = tempDocx,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(tempPsi);
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Không thể mở tệp bên ngoài:\n{ex.Message}", "Lỗi mở tệp");
            }
        }

        private void BtnPrintCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn bài học cần in ấn.", "Chưa chọn bài học");
                return;
            }

            try
            {
                var printDlg = new System.Windows.Controls.PrintDialog();
                if (printDlg.ShowDialog() == true)
                {
                    var doc = new System.Windows.Documents.FlowDocument
                    {
                        FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                        FontSize = 13,
                        PagePadding = new Thickness(40),
                        ColumnWidth = double.PositiveInfinity
                    };

                    // Tiêu đề bài học
                    doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                        new System.Windows.Documents.Run(_currentPreviewLesson.TieuDe))
                    {
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x0F, 0x4C, 0x3A)),
                        Margin = new Thickness(0, 0, 0, 10)
                    });

                    // Thông tin Meta
                    doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                        new System.Windows.Documents.Run($"Chủ đề: {_currentPreviewLesson.TenChuDe}  •  Trình độ: {_currentPreviewLesson.MaTrinhDo}  •  Thời gian: {_currentPreviewLesson.ThoiGianUocTinhPhut} phút"))
                    {
                        FontSize = 11,
                        Foreground = System.Windows.Media.Brushes.Gray,
                        Margin = new Thickness(0, 0, 0, 16)
                    });

                    // Nội dung đọc được từ file thực tế
                    string contentToPrint = KhoHocLieuSyncHelper.DocNoiDungTep(_currentPreviewLesson.DuongDanTepVatLy, _currentPreviewLesson.NoiDung);
                    var paragraphs = contentToPrint.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in paragraphs)
                    {
                        doc.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(p.Trim()))
                        {
                            LineHeight = 22,
                            Margin = new Thickness(0, 0, 0, 8)
                        });
                    }

                    // Nếu có câu hỏi trắc nghiệm kèm theo, in luôn
                    if (_currentPreviewLesson.DanhSachCauHoi != null && _currentPreviewLesson.DanhSachCauHoi.Count > 0)
                    {
                        doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                            new System.Windows.Documents.Run("CÂU HỎI TRẮC NGHIỆM ÔN TẬP"))
                        {
                            FontSize = 14,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 16, 0, 8)
                        });

                        int qIndex = 1;
                        foreach (var q in _currentPreviewLesson.DanhSachCauHoi)
                        {
                            doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                                new System.Windows.Documents.Run($"Câu {qIndex}: {q.NoiDungCauHoi}"))
                            {
                                FontWeight = FontWeights.SemiBold,
                                Margin = new Thickness(0, 4, 0, 4)
                            });

                            foreach (var opt in q.DanhSachLuaChon)
                            {
                                doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                                    new System.Windows.Documents.Run($"   {opt}"))
                                {
                                    Margin = new Thickness(12, 2, 0, 2)
                                });
                            }
                            qIndex++;
                        }
                    }

                    var idpSource = ((System.Windows.Documents.IDocumentPaginatorSource)doc).DocumentPaginator;
                    printDlg.PrintDocument(idpSource, $"In tài liệu - {_currentPreviewLesson.TieuDe}");
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), "Đã gửi tài liệu tới máy in thành công!", "In ấn hoàn tất");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi in ấn tài liệu:\n{ex.Message}", "Lỗi in ấn");
            }
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

        // ================= CỘT 2: DI CHUYỂN & XÓA BÀI HỌC / ĐỀ THI =================

        private void BtnMoveSelectedLesson_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null || _currentPreviewLesson.MaBaiHoc <= 0)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn một bài học / đề thi trong danh sách để di chuyển.", "Chưa chọn tệp");
                return;
            }
            PerformMoveLesson(_currentPreviewLesson);
        }

        private void BtnDeleteSelectedLesson_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null || _currentPreviewLesson.MaBaiHoc <= 0)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn một bài học / đề thi trong danh sách để xóa.", "Chưa chọn tệp");
                return;
            }
            PerformDeleteLesson(_currentPreviewLesson);
        }

        private void BtnItemMove_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true; // Tránh kích hoạt LessonItem_Click
            if (sender is FrameworkElement elem && elem.Tag is int id)
            {
                var lesson = _allLessons.FirstOrDefault(b => b.MaBaiHoc == id) ?? _currentPreviewLesson;
                if (lesson != null)
                {
                    PerformMoveLesson(lesson);
                }
            }
        }

        private void BtnItemDelete_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true; // Tránh kích hoạt LessonItem_Click
            if (sender is FrameworkElement elem && elem.Tag is int id)
            {
                var lesson = _allLessons.FirstOrDefault(b => b.MaBaiHoc == id) ?? _currentPreviewLesson;
                if (lesson != null)
                {
                    PerformDeleteLesson(lesson);
                }
            }
        }

        private void PerformMoveLesson(BaiHocDTO lesson)
        {
            var allChuDe = _danhMucBUS.LayDanhSachChuDe();
            var dlg = new DiChuyenThuMucDialog(lesson, allChuDe);

            if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                var (success, msg) = BaiHocBUS.DiChuyenBaiHoc(lesson.MaBaiHoc, dlg.SelectedTargetMaChuDe);
                if (success)
                {
                    LoadTreeAndLessons();
                    ThongBaoDialog.ThanhCong(
                        Window.GetWindow(this), 
                        $"Đã chuyển '{lesson.TieuDe}' sang thư mục '{dlg.SelectedTargetTenChuDe}' thành công!", 
                        "Di chuyển hoàn tất");
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi di chuyển");
                }
            }
        }

        private void PerformDeleteLesson(BaiHocDTO lesson)
        {
            bool confirm = ThongBaoDialog.XacNhanXoa(
                Window.GetWindow(this),
                $"Bạn có chắc chắn muốn xóa vĩnh viễn bài học / tệp học liệu:\n'{lesson.TieuDe}'?\nThao tác này sẽ gỡ bỏ học liệu khỏi thư mục và cơ sở dữ liệu.",
                "Xác nhận xóa tệp học liệu");

            if (confirm)
            {
                var (success, msg) = BaiHocBUS.XoaBaiHoc(lesson.MaBaiHoc);
                if (success)
                {
                    if (_currentPreviewLesson?.MaBaiHoc == lesson.MaBaiHoc)
                    {
                        _currentPreviewLesson = null;
                    }
                    LoadTreeAndLessons();
                    if (_allLessons != null && _allLessons.Count > 0)
                    {
                        ShowLessonPreview(_allLessons[0]);
                    }
                    else
                    {
                        ClearPreview();
                    }
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Đã xóa tệp thành công");
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi khi xóa tệp");
                }
            }
        }

        #region Xử lý Tab Danh mục Trình độ (A1 - C1) - US-GV04

        private void BtnTabCayChuDe_Click(object sender, RoutedEventArgs e)
        {
            pnlTabCayChuDe.Visibility = Visibility.Visible;
            pnlTabTrinhDo.Visibility = Visibility.Collapsed;

            wrapChuDeActions.Visibility = Visibility.Visible;
            wrapTrinhDoActions.Visibility = Visibility.Collapsed;
        }

        private void BtnTabTrinhDo_Click(object sender, RoutedEventArgs e)
        {
            pnlTabCayChuDe.Visibility = Visibility.Collapsed;
            pnlTabTrinhDo.Visibility = Visibility.Visible;

            wrapChuDeActions.Visibility = Visibility.Collapsed;
            wrapTrinhDoActions.Visibility = Visibility.Visible;

            LoadTrinhDoList();
        }

        private void LoadTrinhDoList()
        {
            try
            {
                _allTrinhDo = _danhMucBUS.LayDanhSachTrinhDo();
                icTrinhDoList.ItemsSource = _allTrinhDo;
                if (txtTongSoTrinhDo != null)
                {
                    txtTongSoTrinhDo.Text = $"{_allTrinhDo.Count} Cấp độ tiêu chuẩn";
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi tải danh mục trình độ:\n{ex.Message}", "Lỗi CSDL");
            }
        }

        private void BtnThemTrinhDo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new TrinhDoDialog();
            if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                var (ok, msg) = _danhMucBUS.ThemTrinhDo(dlg.TrinhDoData);
                if (ok)
                {
                    LoadTrinhDoList();
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Thêm trình độ");
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi thêm trình độ");
                }
            }
        }

        private void BtnSuaTrinhDo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var td = _allTrinhDo.FirstOrDefault(x => x.MaTrinhDo == id);
                if (td != null)
                {
                    var dlg = new TrinhDoDialog(td);
                    if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
                    {
                        var (ok, msg) = _danhMucBUS.CapNhatTrinhDo(dlg.TrinhDoData);
                        if (ok)
                        {
                            LoadTrinhDoList();
                            ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Cập nhật trình độ");
                        }
                        else
                        {
                            ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Lỗi cập nhật");
                        }
                    }
                }
            }
        }

        private void BtnXoaTrinhDo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var td = _allTrinhDo.FirstOrDefault(x => x.MaTrinhDo == id);
                if (td != null)
                {
                    bool confirm = ThongBaoDialog.XacNhanXoa(
                        Window.GetWindow(this),
                        $"Bạn có chắc chắn muốn xóa trình độ '{td.MaCapDo} - {td.TenTrinhDo}'?\nLưu ý: Hệ thống sẽ chặn xóa nếu trình độ đang có bài học hoặc câu hỏi liên kết.",
                        "Xác nhận xóa trình độ");

                    if (confirm)
                    {
                        var (ok, msg) = _danhMucBUS.XoaTrinhDo(id);
                        if (ok)
                        {
                            LoadTrinhDoList();
                            ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Đã xóa trình độ");
                        }
                        else
                        {
                            // Chặn xóa nếu có bài học hoặc ràng buộc khóa ngoại (US-GV04)
                            ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Ràng buộc dữ liệu không thể xóa");
                        }
                    }
                }
            }
        }

        #endregion
    }
}
