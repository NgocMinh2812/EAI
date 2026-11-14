using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BUS;
using DTO;
using GUI.Views.Common;
using GUI.ViewModels.GiangVien;

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

        // Trình phát Audio nghe thử bài học kỹ năng Nghe
        private readonly System.Windows.Media.MediaPlayer _mediaPlayer = new();
        private readonly System.Windows.Threading.DispatcherTimer _audioTimer = new() { Interval = TimeSpan.FromMilliseconds(150) };
        private bool _isPlayingAudio = false;
        private bool _isUserSeekingAudio = false;
        private string? _currentAudioPath;
        private double _audioTotalSeconds = 0;

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

            // Cấu hình sự kiện cho Trình phát Audio
            _audioTimer.Tick += AudioTimer_Tick;

            _mediaPlayer.MediaOpened += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                    {
                        var totalTs = _mediaPlayer.NaturalDuration.TimeSpan;
                        _audioTotalSeconds = totalTs.TotalSeconds;
                        if (sldAudioTimeline != null)
                        {
                            sldAudioTimeline.Maximum = _audioTotalSeconds > 0 ? _audioTotalSeconds : 100;
                        }
                        if (txtAudioTotalTime != null)
                        {
                            txtAudioTotalTime.Text = FormatAudioTime(totalTs);
                        }
                    }
                }));
            };

            _mediaPlayer.MediaEnded += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try { _mediaPlayer.Stop(); } catch { }
                    try { _mediaPlayer.Position = TimeSpan.Zero; } catch { }
                    _isPlayingAudio = false;
                    _audioTimer.Stop();
                    if (sldAudioTimeline != null) sldAudioTimeline.Value = 0;
                    if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = "00:00";
                    CapNhatNutPlay();
                }));
            };

            _mediaPlayer.MediaFailed += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _isPlayingAudio = false;
                    _audioTimer.Stop();
                    CapNhatNutPlay();
                }));
            };

            Unloaded += (s, e) => DungAudio();

            _isLoaded = true;
            Loaded += DanhMucView_Loaded;
            LoadTreeAndLessons();
        }

        private void DanhMucView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isLoaded) return;
            try
            {
                int previousTopicId = _selectedNode?.Id ?? -1;
                int previousLessonId = _currentPreviewLesson?.MaBaiHoc ?? -1;

                var treeNodes = _danhMucBUS.LayCayTreeNode();
                tvDanhMuc.ItemsSource = treeNodes;

                if (previousTopicId > 0)
                {
                    _selectedNode = FindNodeById(treeNodes, previousTopicId) ?? (treeNodes.Count > 0 ? treeNodes[0] : null);
                }
                else if (treeNodes.Count > 0 && _selectedNode == null)
                {
                    _selectedNode = treeNodes[0];
                }

                LoadLessonsForSelectedTopic();

                if (previousLessonId > 0 && _allLessons != null && _allLessons.Count > 0)
                {
                    var match = _allLessons.FirstOrDefault(x => x.MaBaiHoc == previousLessonId);
                    if (match != null)
                    {
                        ShowLessonPreview(match);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DanhMucView.Loaded] {ex.Message}");
            }
        }

        private static TreeNodeDTO? FindNodeById(IEnumerable<TreeNodeDTO> nodes, int id)
        {
            foreach (var node in nodes)
            {
                if (node.Id == id) return node;
                var found = FindNodeById(node.Children, id);
                if (found != null) return found;
            }
            return null;
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
                ApplyLessonFilters();
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi tải danh sách bài học:\n{ex.Message}", "Lỗi dữ liệu");
            }
        }

        private void CboFilterTrinhDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded) return;
            ApplyLessonFilters();
        }

        private void CboFilterKyNang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded) return;
            ApplyLessonFilters();
        }

        private void ApplyLessonFilters()
        {
            if (_allLessons == null) return;

            string selectedTrinhDo = (cboFilterTrinhDo?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "All";
            string selectedKyNang = (cboFilterKyNang?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "All";

            var filtered = _allLessons.AsEnumerable();

            // 1. Lọc theo trình độ (A1, A2, B1, B2, C1)
            if (!string.IsNullOrEmpty(selectedTrinhDo) && !selectedTrinhDo.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(b => 
                    !string.IsNullOrEmpty(b.MaTrinhDo) && 
                    b.MaTrinhDo.Equals(selectedTrinhDo, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Lọc theo kỹ năng (Ngữ pháp, Từ vựng, Đọc hiểu, Nghe hiểu, Viết luận)
            if (!string.IsNullOrEmpty(selectedKyNang) && !selectedKyNang.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(b => MatchKyNang(b.LoaiBaiHoc, selectedKyNang));
            }

            var listFiltered = filtered.ToList();

            if (icLessonList != null)
            {
                icLessonList.ItemsSource = listFiltered;
            }

            if (txtLessonCounter != null)
            {
                txtLessonCounter.Text = $"{listFiltered.Count} bài học";
            }

            if (txtEmptyNotice != null)
            {
                txtEmptyNotice.Visibility = listFiltered.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (listFiltered.Count > 0)
            {
                var current = listFiltered.FirstOrDefault(b => _currentPreviewLesson != null && b.MaBaiHoc == _currentPreviewLesson.MaBaiHoc) ?? listFiltered[0];
                ShowLessonPreview(current);
            }
            else
            {
                ClearPreview();
            }
        }

        private static bool MatchKyNang(string? loaiBaiHoc, string filterTag)
        {
            if (string.IsNullOrEmpty(loaiBaiHoc)) return false;
            string l = loaiBaiHoc.ToLowerInvariant();
            return filterTag switch
            {
                "NguPhap" => l.Contains("nguphap") || l.Contains("ngữ pháp") || l.Contains("grammar"),
                "TuVung" => l.Contains("tuvung") || l.Contains("từ vựng") || l.Contains("vocabulary") || l.Contains("vocab"),
                "Doc" => l.Contains("doc") || l.Contains("đọc") || l.Contains("reading"),
                "Nghe" => l.Contains("nghe") || l.Contains("listening") || l.Contains("audio"),
                "Viet" => l.Contains("viet") || l.Contains("viết") || l.Contains("writing"),
                "Noi" => l.Contains("noi") || l.Contains("nói") || l.Contains("speaking"),
                _ => true
            };
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
            DungAudio();
            _currentPreviewLesson = lesson;

            if (txtSimulationModule != null)
                txtSimulationModule.Text = $"{lesson.TenLoaiHienThi?.ToUpper() ?? "HỌC LIỆU"} — TRÌNH ĐỘ {lesson.MaTrinhDo ?? "B1"}";
            if (txtSimulationPassage != null)
                txtSimulationPassage.Text = "Xem trước nội dung tệp";

            if (txtPassageTitle != null)
                txtPassageTitle.Text = lesson.TieuDe ?? "";

            // KIỂM SOÁT AUDIO BÀI NGHE (LISTENING AUDIO PLAYER)
            bool isListening = lesson.IsKyNangNghe 
                || BaiHocDTO.KiemTraKyNangNghe(lesson.LoaiBaiHoc) 
                || BaiHocDTO.KiemTraKyNangNghe(lesson.TieuDe)
                || BaiHocDTO.KiemTraKyNangNghe(lesson.TenChuDe)
                || lesson.HasAudio
                || BaiHocDTO.XacDinhKyNangTuTenHoacChuDe(lesson.TieuDe, lesson.TenChuDe, lesson.DuongDanTepVatLy) == "Nghe";
            string? audioFilePath = KhoHocLieuSyncHelper.TimTepAudioChoBaiHoc(lesson);

            if (pnlAudioPlayerSection != null)
            {
                if (isListening || !string.IsNullOrEmpty(audioFilePath))
                {
                    pnlAudioPlayerSection.Visibility = Visibility.Visible;
                    _currentAudioPath = audioFilePath;
                    if (!string.IsNullOrEmpty(audioFilePath))
                    {
                        txtAudioFileName.Text = System.IO.Path.GetFileName(audioFilePath);
                        try
                        {
                            _mediaPlayer.Open(new Uri(System.IO.Path.GetFullPath(audioFilePath)));
                        }
                        catch { }
                    }
                    else
                    {
                        txtAudioFileName.Text = !string.IsNullOrEmpty(lesson.AudioPath) 
                            ? System.IO.Path.GetFileName(lesson.AudioPath) 
                            : (!string.IsNullOrEmpty(lesson.DuongDanAudio) ? System.IO.Path.GetFileName(lesson.DuongDanAudio) : "Bài học kỹ năng Nghe (Chưa gắn tệp âm thanh)");
                    }
                }
                else
                {
                    pnlAudioPlayerSection.Visibility = Visibility.Collapsed;
                }
            }

            // Hiển thị tóm tắt nội dung bài học vào khối riêng biệt, tự xuống dòng đầy đủ
            if (pnlPassageSummary != null && txtPassageSummaryFull != null)
            {
                if (!string.IsNullOrWhiteSpace(lesson.TomTat))
                {
                    pnlPassageSummary.Visibility = Visibility.Visible;
                    txtPassageSummaryFull.Text = lesson.TomTat;
                }
                else
                {
                    pnlPassageSummary.Visibility = Visibility.Collapsed;
                }
            }

            // Đọc nội dung tệp thực tế từ ổ đĩa (DOCX, PDF, TXT) chuẩn như File Explorer
            string actualContent = KhoHocLieuSyncHelper.DocNoiDungTep(lesson.DuongDanTepVatLy, lesson.NoiDung);
            txtPassageBody.Text = string.IsNullOrWhiteSpace(actualContent) 
                ? "Nội dung bài học chưa được cập nhật chi tiết." 
                : actualContent;

            // KIỂM SOÁT HIỂN THỊ CÂU HỎI AI: Tách thành quản lý riêng, không chèn vào tài liệu
            bool hasDraft = Views.Common.AICauHoiUIHelper.CoBanNhapTrongPhien(lesson.MaBaiHoc);
            if (pnlAIGeneratePrompt != null)
            {
                pnlAIGeneratePrompt.Visibility = hasDraft ? Visibility.Collapsed : Visibility.Visible;
            }
            if (pnlAIGeneratedStatusBox != null)
            {
                pnlAIGeneratedStatusBox.Visibility = hasDraft ? Visibility.Visible : Visibility.Collapsed;
                if (hasDraft && txtAIGeneratedTitle != null)
                {
                    var draftList = Views.Common.AICauHoiUIHelper.LayBanNhapTrongPhien(lesson.MaBaiHoc);
                    txtAIGeneratedTitle.Text = $"Đã tạo bộ câu hỏi AI ({draftList?.Count ?? 0} câu) cho tài liệu này";
                }
            }

            // Cập nhật trạng thái động ở Footer Preview
            bool isPublished = lesson.DaXuatBan || string.Equals(lesson.TrangThai, "Đã xuất bản", StringComparison.OrdinalIgnoreCase);
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
                txtPreviewStatusPill.Text = isPublished ? "Đã xuất bản" : "Bản nháp";
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

            if (Views.Common.AICauHoiUIHelper.CoBanNhapTrongPhien(_currentPreviewLesson.MaBaiHoc))
            {
                bool confirm = ThongBaoDialog.XacNhan(
                    Window.GetWindow(this),
                    $"Tài liệu '{_currentPreviewLesson.TieuDe}' đã có bộ câu hỏi ôn tập trong phiên làm việc.\n\nBạn có chắc chắn muốn yêu cầu AI bóc tách và tạo lại bộ câu hỏi mới hoàn toàn không?",
                    "Xác nhận tạo lại bộ câu hỏi AI",
                    "Tạo lại mới",
                    "Giữ câu hỏi cũ");
                if (!confirm)
                {
                    var draft = Views.Common.AICauHoiUIHelper.LayBanNhapTrongPhien(_currentPreviewLesson.MaBaiHoc)!;
                    Views.Common.AICauHoiUIHelper.MoModalDuyetBoCauHoi(Window.GetWindow(this), _currentPreviewLesson, draft, "AI_DanhMuc");
                    return;
                }
            }

            if (pbAILoading != null) pbAILoading.Visibility = Visibility.Visible;
            if (btnReExtractAI != null) btnReExtractAI.IsEnabled = false;
            if (btnGenerateAIQuestions != null) btnGenerateAIQuestions.IsEnabled = false;

            try
            {
                bool daLuu = await Views.Common.AICauHoiUIHelper.TaoVaDuyetBoCauHoiAsync(
                    Window.GetWindow(this),
                    _currentPreviewLesson,
                    nguonTao: "AI_DanhMuc",
                    soLuong: 10,
                    forceRecreate: true);

                if (Views.Common.AICauHoiUIHelper.CoBanNhapTrongPhien(_currentPreviewLesson.MaBaiHoc))
                {
                    var draft = Views.Common.AICauHoiUIHelper.LayBanNhapTrongPhien(_currentPreviewLesson.MaBaiHoc);
                    if (pnlAIGeneratePrompt != null) pnlAIGeneratePrompt.Visibility = Visibility.Collapsed;
                    if (pnlAIGeneratedStatusBox != null)
                    {
                        pnlAIGeneratedStatusBox.Visibility = Visibility.Visible;
                        if (txtAIGeneratedTitle != null)
                        {
                            txtAIGeneratedTitle.Text = daLuu
                                ? "✓ Đã lưu thành công bộ câu hỏi vào Ngân hàng câu hỏi!"
                                : $"Đã tạo bộ câu hỏi AI ({draft?.Count ?? 0} câu) cho tài liệu này";
                        }
                    }
                }
            }
            finally
            {
                if (pbAILoading != null) pbAILoading.Visibility = Visibility.Collapsed;
                if (btnReExtractAI != null) btnReExtractAI.IsEnabled = true;
                if (btnGenerateAIQuestions != null) btnGenerateAIQuestions.IsEnabled = true;
            }
        }

        private void BtnOpenAIDialog_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPreviewLesson == null) return;
            if (Views.Common.AICauHoiUIHelper.CoBanNhapTrongPhien(_currentPreviewLesson.MaBaiHoc))
            {
                var draftQuestions = Views.Common.AICauHoiUIHelper.LayBanNhapTrongPhien(_currentPreviewLesson.MaBaiHoc)!;
                bool daLuu = Views.Common.AICauHoiUIHelper.MoModalDuyetBoCauHoi(Window.GetWindow(this), _currentPreviewLesson, draftQuestions, "AI_DanhMuc");
                if (daLuu && txtAIGeneratedTitle != null)
                {
                    txtAIGeneratedTitle.Text = "✓ Đã lưu thành công bộ câu hỏi vào Ngân hàng câu hỏi!";
                }
            }
            else
            {
                BtnReExtractAI_Click(sender, e);
            }
        }

        private void ClearPreview()
        {
            DungAudio();
            _currentPreviewLesson = null;
            if (pnlAudioPlayerSection != null) pnlAudioPlayerSection.Visibility = Visibility.Collapsed;

            if (txtSimulationModule != null) txtSimulationModule.Text = "HỌC LIỆU";
            if (pnlAIGeneratePrompt != null) pnlAIGeneratePrompt.Visibility = Visibility.Collapsed;
            if (pnlAIGeneratedStatusBox != null) pnlAIGeneratedStatusBox.Visibility = Visibility.Collapsed;
            if (txtSimulationPassage != null) txtSimulationPassage.Text = "";
            if (txtPassageTitle != null) txtPassageTitle.Text = "Không có dữ liệu bài học";
            if (txtPassageBody != null) txtPassageBody.Text = "Chủ đề này hiện chưa có bài học nào phù hợp với bộ lọc.";

            if (txtPreviewStatusPill != null) txtPreviewStatusPill.Text = "Chưa chọn";
        }

        #region Xử lý Trình phát Âm thanh (Audio Player) cho bài học Listening

        private void AudioTimer_Tick(object? sender, EventArgs e)
        {
            if (_isUserSeekingAudio) return;
            try
            {
                if (_mediaPlayer.Source != null && _mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    var pos = _mediaPlayer.Position;
                    if (sldAudioTimeline != null)
                    {
                        sldAudioTimeline.Value = pos.TotalSeconds;
                    }
                    if (txtAudioCurrentTime != null)
                    {
                        txtAudioCurrentTime.Text = FormatAudioTime(pos);
                    }
                }
            }
            catch { }
        }

        private static string FormatAudioTime(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        private void DungAudio()
        {
            try
            {
                _audioTimer.Stop();
                if (_isPlayingAudio)
                {
                    _mediaPlayer.Stop();
                    _isPlayingAudio = false;
                }
                _mediaPlayer.Close();
                _currentAudioPath = null;
                _audioTotalSeconds = 0;
                if (sldAudioTimeline != null) sldAudioTimeline.Value = 0;
                if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = "00:00";
                if (txtAudioTotalTime != null) txtAudioTotalTime.Text = "00:00";
                CapNhatNutPlay();
            }
            catch { }
        }

        private void CapNhatNutPlay()
        {
            if (txtPlayAudio == null || icoPlayAudio == null) return;
            if (_isPlayingAudio)
            {
                txtPlayAudio.Text = "Tạm dừng";
                icoPlayAudio.Kind = LucideIconKind.Pause;
            }
            else
            {
                txtPlayAudio.Text = "Phát Audio";
                icoPlayAudio.Kind = LucideIconKind.Play;
            }
        }

        private void BtnTogglePlayAudio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_currentAudioPath) || !System.IO.File.Exists(_currentAudioPath))
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), "Không tìm thấy tệp âm thanh của bài học này trên hệ thống.", "Chưa có file âm thanh");
                    return;
                }

                if (_isPlayingAudio)
                {
                    _mediaPlayer.Pause();
                    _audioTimer.Stop();
                    _isPlayingAudio = false;
                    CapNhatNutPlay();
                }
                else
                {
                    string fullPath = System.IO.Path.GetFullPath(_currentAudioPath);
                    if (_mediaPlayer.Source == null || !_mediaPlayer.Source.LocalPath.Equals(fullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        _mediaPlayer.Open(new Uri(fullPath));
                    }
                    _mediaPlayer.Play();
                    _audioTimer.Start();
                    _isPlayingAudio = true;
                    CapNhatNutPlay();
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi phát tệp âm thanh:\n{ex.Message}", "Lỗi phát Audio");
            }
        }

        private void BtnRewindAudio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _mediaPlayer.Position = TimeSpan.Zero;
                if (sldAudioTimeline != null) sldAudioTimeline.Value = 0;
                if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = "00:00";
                if (!_isPlayingAudio && !string.IsNullOrEmpty(_currentAudioPath) && System.IO.File.Exists(_currentAudioPath))
                {
                    BtnTogglePlayAudio_Click(sender, e);
                }
            }
            catch { }
        }

        private void BtnDownloadAudio_Click(object sender, RoutedEventArgs e)
        {
            string? audioFile = _currentAudioPath;
            if (string.IsNullOrEmpty(audioFile) || !System.IO.File.Exists(audioFile))
            {
                if (_currentPreviewLesson != null)
                {
                    if (!string.IsNullOrEmpty(_currentPreviewLesson.AudioPath) && System.IO.File.Exists(_currentPreviewLesson.AudioPath))
                        audioFile = _currentPreviewLesson.AudioPath;
                    else if (!string.IsNullOrEmpty(_currentPreviewLesson.DuongDanAudio) && System.IO.File.Exists(_currentPreviewLesson.DuongDanAudio))
                        audioFile = _currentPreviewLesson.DuongDanAudio;
                    else
                        audioFile = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(_currentPreviewLesson);
                }
            }

            if (string.IsNullOrEmpty(audioFile) || !System.IO.File.Exists(audioFile))
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Không tìm thấy tệp âm thanh của bài học này trên hệ thống.", "Chưa có file âm thanh");
                return;
            }

            try
            {
                string ext = System.IO.Path.GetExtension(audioFile);
                if (string.IsNullOrEmpty(ext)) ext = ".mp3";

                string safeTitle = _currentPreviewLesson != null 
                    ? KhoHocLieuSyncHelper.SanitizeFolderName(_currentPreviewLesson.TieuDe)
                    : "Audio_BaiHoc";
                string defaultName = $"{safeTitle}_Audio{ext}";

                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = defaultName,
                    Filter = $"Tệp âm thanh (*{ext})|*{ext}|Tất cả tệp (*.*)|*.*",
                    Title = "Lưu tệp âm thanh bài nghe về máy tính"
                };

                if (sfd.ShowDialog() == true)
                {
                    System.IO.File.Copy(audioFile, sfd.FileName, true);
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), $"Đã lưu tệp âm thanh về máy tính thành công:\n{sfd.FileName}", "Tải âm thanh hoàn tất");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi tải tệp âm thanh:\n{ex.Message}", "Lỗi tải về");
            }
        }

        private void SldAudioTimeline_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isUserSeekingAudio = true;
        }

        private void SldAudioTimeline_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                double targetSec = sldAudioTimeline.Value;
                if (targetSec < 0) targetSec = 0;
                if (_audioTotalSeconds > 0 && targetSec > _audioTotalSeconds) targetSec = _audioTotalSeconds;
                _mediaPlayer.Position = TimeSpan.FromSeconds(targetSec);
                if (txtAudioCurrentTime != null)
                {
                    txtAudioCurrentTime.Text = FormatAudioTime(TimeSpan.FromSeconds(targetSec));
                }
            }
            catch { }
            finally
            {
                _isUserSeekingAudio = false;
            }
        }

        private void SldAudioTimeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isUserSeekingAudio && txtAudioCurrentTime != null)
            {
                txtAudioCurrentTime.Text = FormatAudioTime(TimeSpan.FromSeconds(e.NewValue));
            }
        }

        #endregion

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

        private void BtnLamMoiChuDe_Click(object sender, RoutedEventArgs e)
        {
            LoadTreeAndLessons();
        }

        private void BtnXoaChuDe_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNode == null || _selectedNode.Id <= 0)
            {
                ThongBaoDialog.CanhBao(Window.GetWindow(this), "Vui lòng chọn một danh mục cụ thể để xóa.", "Chưa chọn danh mục");
                return;
            }

            // 1. Kiểm tra xem danh mục hoặc các danh mục con trực thuộc có chứa bài học hay không
            int soBaiHoc = _danhMucBUS.DemTongSoBaiHocTrongCayChuDe(_selectedNode.Id);
            if (soBaiHoc > 0)
            {
                ThongBaoDialog.CanhBao(
                    Window.GetWindow(this),
                    $"Không thể xóa danh mục '{_selectedNode.Title}' vì danh mục này (hoặc các danh mục con trực thuộc) đang chứa {soBaiHoc} bài học.\n\nVui lòng di dời hoặc xóa các bài học trước khi xóa danh mục!",
                    "Ràng buộc bài học");
                return;
            }

            // 2. Nếu tất cả đều 0 bài học, hiển thị thông báo xác nhận tương ứng
            int soChuDeCon = _danhMucBUS.DemSoChuDeConTrongCay(_selectedNode.Id);
            string confirmMsg = soChuDeCon > 0
                ? $"Danh mục '{_selectedNode.Title}' và {soChuDeCon} danh mục con trực thuộc đều đang trống (0 bài học).\n\nBạn có chắc chắn muốn xóa vĩnh viễn danh mục cha cùng toàn bộ {soChuDeCon} danh mục con này không?"
                : $"Bạn có chắc chắn muốn xóa vĩnh viễn danh mục '{_selectedNode.Title}' (0 bài học)?";

            bool confirm = ThongBaoDialog.XacNhanXoa(
                Window.GetWindow(this),
                confirmMsg,
                "Xác nhận xóa danh mục");

            if (confirm)
            {
                // Kiểm tra ràng buộc và xóa qua BUS
                var (success, msg) = _danhMucBUS.XoaChuDe(_selectedNode.Id);
                if (success)
                {
                    _selectedNode = null;
                    LoadTreeAndLessons();
                    ThongBaoDialog.ThanhCong(Window.GetWindow(this), msg, "Đã xóa danh mục");
                }
                else
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), msg, "Không thể xóa");
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

                // Nếu là file Audio, bài học kỹ năng Nghe, hoặc định dạng PDF thì xuất tải về dưới dạng PDF
                bool isAudioOrPdf = (_currentPreviewLesson.DinhDangTep == "AUDIO") ||
                                    (_currentPreviewLesson.DinhDangTep == "PDF") ||
                                    (_currentPreviewLesson.LoaiBaiHoc == "Nghe") ||
                                    (_currentPreviewLesson.LoaiBaiHoc == "NgheHieu") ||
                                    (!string.IsNullOrEmpty(srcFile) && (srcFile.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) || srcFile.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase)));

                string ext = isAudioOrPdf ? ".pdf" : (!string.IsNullOrEmpty(srcFile) ? System.IO.Path.GetExtension(srcFile).ToLowerInvariant() : ".docx");
                if (string.IsNullOrEmpty(ext)) ext = isAudioOrPdf ? ".pdf" : ".docx";

                string safeTitle = KhoHocLieuSyncHelper.SanitizeFolderName(_currentPreviewLesson.TieuDe);
                string defaultName = $"{safeTitle}{ext}";

                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = defaultName,
                    Filter = ext.Equals(".pdf", StringComparison.OrdinalIgnoreCase)
                        ? "Tệp PDF Document (*.pdf)|*.pdf|Tất cả tệp (*.*)|*.*"
                        : "Tệp Word Document (*.docx)|*.docx|Tất cả tệp (*.*)|*.*"
                };

                if (sfd.ShowDialog() == true)
                {
                    if (ext.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(srcFile) && System.IO.File.Exists(srcFile) && srcFile.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            System.IO.File.Copy(srcFile, sfd.FileName, true);
                        }
                        else
                        {
                            KhoHocLieuSyncHelper.TaoTepPdfTuBaiHoc(sfd.FileName, _currentPreviewLesson);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(srcFile) && System.IO.File.Exists(srcFile) && srcFile.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                        {
                            System.IO.File.Copy(srcFile, sfd.FileName, true);
                        }
                        else
                        {
                            KhoHocLieuSyncHelper.TaoTepDocxTuBaiHoc(sfd.FileName, _currentPreviewLesson);
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

            bool confirm = ThongBaoDialog.XacNhan(
                Window.GetWindow(this),
                $"Bạn có chắc chắn muốn tạo một Đề thi thử mới từ học liệu:\n'{_currentPreviewLesson.TieuDe}'?\n\nĐề thi mới sẽ được lưu vào danh sách đề thi luyện tập của hệ thống.",
                "Xác nhận tạo Đề thi thử",
                "Tạo đề thi",
                "Hủy bỏ");

            if (!confirm) return;

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
                // Xóa tệp vật lý tương ứng nếu tồn tại trên ổ đĩa
                try
                {
                    string? phyFile = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(lesson);
                    if (!string.IsNullOrEmpty(phyFile) && System.IO.File.Exists(phyFile))
                    {
                        System.IO.File.Delete(phyFile);
                    }
                }
                catch { }

                var (success, msg) = BaiHocBUS.XoaBaiHoc(lesson.MaBaiHoc);
                if (success)
                {
                    Views.Common.AICauHoiUIHelper.XoaBanNhapTrongPhien(lesson.MaBaiHoc);
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
