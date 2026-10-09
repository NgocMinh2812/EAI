using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class DanhMucView : Page
    {
        private readonly DanhMucBUS _danhMucBUS = new DanhMucBUS();
        private List<BaiHocDTO> _allLessons = new List<BaiHocDTO>();
        private TreeNodeDTO? _selectedNode;
        private const string SearchPlaceholder = "🔍 Tìm bài học / đề thi...";

        public DanhMucView()
        {
            InitializeComponent();
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
                MessageBox.Show($"Lỗi tải cây danh mục từ cơ sở dữ liệu: {ex.Message}", "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadLessonsForSelectedTopic()
        {
            try
            {
                int? topicId = (_selectedNode != null && _selectedNode.Id > 0) ? _selectedNode.Id : null;
                string? search = (txtSearchFile != null && txtSearchFile.Text != SearchPlaceholder) ? txtSearchFile.Text.Trim() : null;

                _allLessons = BaiHocBUS.LayDanhSachTuDatabase(topicId, search);

                icLessonList.ItemsSource = _allLessons;
                txtLessonCounter.Text = $"{_allLessons.Count} bài học";

                txtEmptyNotice.Visibility = _allLessons.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

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
                MessageBox.Show($"Lỗi tải danh sách bài học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TvDanhMuc_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
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
            txtPreviewDocTitle.Text = lesson.TieuDe;
            txtPreviewDocMeta.Text = $"Chủ đề: {lesson.TenChuDe} • Trình độ: {lesson.MaTrinhDo} • Ước tính: {lesson.ThoiGianUocTinhPhut} phút • Trạng thái: {lesson.TrangThai}";
            txtSimulationModule.Text = $"{lesson.TenLoaiHienThi.ToUpper()} — TRÌNH ĐỘ {lesson.MaTrinhDo}";
            txtSimulationPassage.Text = $"Tóm tắt: {lesson.TomTat}";

            txtPassageTitle.Text = lesson.TieuDe;
            txtPassageBody.Text = string.IsNullOrWhiteSpace(lesson.NoiDung) 
                ? "Nội dung bài học chưa được cập nhật chi tiết." 
                : lesson.NoiDung.Replace("<p>", "").Replace("</p>", "\n\n").Replace("<h3>", "■ ").Replace("</h3>", "\n").Replace("<ul>", "").Replace("</ul>", "").Replace("<li>", "• ").Replace("</li>", "\n");
        }

        private void ClearPreview()
        {
            txtPreviewDocTitle.Text = "Chưa chọn bài học";
            txtPreviewDocMeta.Text = "Vui lòng chọn bài học từ danh sách để xem trước";
            txtSimulationModule.Text = "HỌC LIỆU";
            txtSimulationPassage.Text = "";
            txtPassageTitle.Text = "Không có dữ liệu bài học";
            txtPassageBody.Text = "Chủ đề này hiện chưa có bài học nào được tạo.";
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
            LoadLessonsForSelectedTopic();
        }

        // ================= CRUD CHỦ ĐỀ HỌC LIỆU =================

        private void BtnThemChuDe_Click(object sender, RoutedEventArgs e)
        {
            var danhSach = _danhMucBUS.LayDanhSachChuDe();
            int? defaultParent = (_selectedNode != null && _selectedNode.Id > 0) ? _selectedNode.Id : null;

            var dlg = new ChuDeDialog(danhSach, null, defaultParent);
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true)
            {
                var (success, msg) = _danhMucBUS.ThemChuDe(dlg.ChuDeData);
                if (success)
                {
                    LoadTreeAndLessons();
                    MessageBox.Show(msg, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnSuaChuDe_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNode == null || _selectedNode.Id <= 0)
            {
                MessageBox.Show("Vui lòng chọn một chủ đề cụ thể để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var danhSach = _danhMucBUS.LayDanhSachChuDe();
            var current = danhSach.FirstOrDefault(c => c.MaChuDe == _selectedNode.Id);
            if (current == null)
            {
                MessageBox.Show("Không tìm thấy chủ đề.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dlg = new ChuDeDialog(danhSach, current);
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true)
            {
                var (success, msg) = _danhMucBUS.CapNhatChuDe(dlg.ChuDeData);
                if (success)
                {
                    LoadTreeAndLessons();
                    MessageBox.Show(msg, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnXoaChuDe_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNode == null || _selectedNode.Id <= 0)
            {
                MessageBox.Show("Vui lòng chọn một chủ đề cụ thể để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa chủ đề: '{_selectedNode.Title}'?",
                                          "Xác nhận xóa danh mục", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                // Kiểm tra ràng buộc và xóa qua BUS
                var (success, msg) = _danhMucBUS.XoaChuDe(_selectedNode.Id);
                if (success)
                {
                    _selectedNode = null;
                    LoadTreeAndLessons();
                    MessageBox.Show(msg, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(msg, "Ràng buộc dữ liệu không cho phép xóa", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // ================= TOP HEADER BUTTONS =================

        private void BtnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở hộp thoại tải lên tệp đề thi mới (.pdf, .docx, .xlsx, .mp3) để bóc tách câu hỏi.", 
                            "Tải lên tệp", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnNewFolder_Click(object sender, RoutedEventArgs e)
        {
            BtnThemChuDe_Click(sender, e);
        }

        private void BtnDownloadCurrent_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đang tải tài liệu về máy tính thành công.", "Tải về", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPrintCurrent_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đang kết nối máy in để in đề thi / học liệu...", "In ấn", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAddToExam_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã thêm toàn bộ nội dung học liệu vào bộ đề thi thử thành công!", 
                            "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
