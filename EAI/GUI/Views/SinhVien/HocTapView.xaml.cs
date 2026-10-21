using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using DTO;
using GUI.Services;

namespace GUI.Views
{
    public partial class HocTapView : Page
    {
        private const string SearchPlaceholder = "Tìm bài học, chủ đề, từ vựng...";
        private List<BaiHocDTO> _danhSachBaiHoc = new List<BaiHocDTO>();
        private string _selectedSkill = "All";
        private TreeFilterNodeTag? _currentTreeFilter;
        private BaiHocDTO? _currentBaiHoc;
        private bool _isInitialized = false;
        private readonly DAL.DanhMucDAL _danhMucDAL = new DAL.DanhMucDAL();

        // TASK-202: Cấu trúc dữ liệu thẻ lọc đại diện cho mỗi nút trên Cây Danh Mục Lộ Trình
        public class TreeFilterNodeTag
        {
            public string FilterType { get; set; } = string.Empty; // "ALL", "SKILL", "TOPIC_PARENT", "TOPIC_CHILD", "LEVEL", "FOLDER_SKILLS", "FOLDER_TOPICS", "FOLDER_LEVELS"
            public string? Code { get; set; }
            public int Id { get; set; }
            public List<int> ChildTopicIds { get; set; } = new List<int>();
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public int LessonCount { get; set; }
        }

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
                    if (txtAudioTotalTime != null)
                    {
                        txtAudioTotalTime.Text = $"{(int)_audioTotalSeconds / 60:D2}:{(int)_audioTotalSeconds % 60:D2}";
                    }
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

                    if (sldAudioProgress != null && _audioTotalSeconds > 0)
                    {
                        sldAudioProgress.Value = (_audioCurrentSeconds / _audioTotalSeconds) * 100.0;
                    }
                    if (txtAudioCurrentTime != null)
                    {
                        txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
                    }
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

                // Xây dựng cây danh mục lộ trình trực tiếp từ CSDL
                BuildRoadmapTree();
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.CanhBao("Lỗi nạp dữ liệu bài học: " + ex.Message, "Thông báo");
            }
        }

        /// <summary>
        /// Nạp và xây dựng CÂY DANH MỤC LỘ TRÌNH tự động từ CSDL (Bảng ChuDe, TrinhDo & BaiHoc)
        /// Kèm badge đếm số lượng bài học theo thời gian thực
        /// </summary>
        private void BuildRoadmapTree()
        {
            if (trvDanhMuc == null) return;
            trvDanhMuc.Items.Clear();

            int totalLessons = _danhSachBaiHoc.Count;

            // =========================================================================
            // NHÁNH 1: PHÂN THEO KỸ NĂNG (SKILLS)
            // =========================================================================
            var skillDefinitions = new (string Code, string Name, string Icon)[]
            {
                ("TuVung", "Từ vựng (Vocabulary)", "📖"),
                ("NguPhap", "Ngữ pháp (Grammar)", "📝"),
                ("Doc", "Đọc hiểu (Reading)", "📚"),
                ("Nghe", "Nghe hiểu (Listening)", "🎧"),
                ("Viet", "Viết luận (Writing)", "✍️")
            };

            int totalSkillLessons = 0;
            var skillItemTags = new List<(TreeViewItem Item, TreeFilterNodeTag Tag)>();

            foreach (var sk in skillDefinitions)
            {
                int count = _danhSachBaiHoc.Count(b => b.LoaiBaiHoc.Equals(sk.Code, StringComparison.OrdinalIgnoreCase));
                totalSkillLessons += count;

                var tag = new TreeFilterNodeTag
                {
                    FilterType = "SKILL",
                    Code = sk.Code,
                    Title = sk.Name,
                    Description = $"Rèn luyện chuyên sâu kỹ năng {sk.Name} với học liệu chuẩn hóa và AI chấm điểm chi tiết.",
                    LessonCount = count
                };

                var item = CreateTreeItem(sk.Icon, sk.Name, count, tag, tag.Description);
                skillItemTags.Add((item, tag));
            }

            var folderSkillsTag = new TreeFilterNodeTag
            {
                FilterType = "FOLDER_SKILLS",
                Title = "Phân theo Kỹ năng",
                Description = "Lộ trình học tập đa kỹ năng: Từ vựng, Ngữ pháp, Đọc hiểu, Nghe hiểu và Viết luận.",
                LessonCount = totalSkillLessons
            };
            var branchSkills = CreateTreeItem("📂", "Phân theo Kỹ năng", totalSkillLessons, folderSkillsTag, folderSkillsTag.Description, isBold: true, isExpanded: true);
            foreach (var s in skillItemTags)
            {
                branchSkills.Items.Add(s.Item);
            }
            trvDanhMuc.Items.Add(branchSkills);

            // =========================================================================
            // NHÁNH 2: CÂY CHỦ ĐỀ HỌC TẬP (TOPICS - Nạp động từ bảng ChuDe)
            // =========================================================================
            List<ChuDeDTO> allChuDe;
            try
            {
                allChuDe = _danhMucDAL.GetAllChuDe();
            }
            catch
            {
                allChuDe = new List<ChuDeDTO>();
            }

            var rootTopics = allChuDe.Where(x => !x.MaChuDeCha.HasValue || x.MaChuDeCha <= 0).ToList();
            int totalTopicLessons = 0;
            var branchTopicItems = new List<TreeViewItem>();

            foreach (var parent in rootTopics)
            {
                // Tìm toàn bộ chủ đề con trực tiếp
                var children = allChuDe.Where(x => x.MaChuDeCha == parent.MaChuDe).ToList();
                var childIds = children.Select(c => c.MaChuDe).ToList();

                // Tính tổng số bài học của chủ đề cha + tất cả con
                int parentLessonCount = _danhSachBaiHoc.Count(b => b.MaChuDe == parent.MaChuDe || childIds.Contains(b.MaChuDe));
                totalTopicLessons += parentLessonCount;

                string parentIcon = GetTopicIcon(parent.TenChuDe, isParent: true);

                var parentTag = new TreeFilterNodeTag
                {
                    FilterType = "TOPIC_PARENT",
                    Id = parent.MaChuDe,
                    ChildTopicIds = childIds,
                    Title = parent.TenChuDe,
                    Description = !string.IsNullOrWhiteSpace(parent.MoTa) ? parent.MoTa : $"Kho bài học chủ đề {parent.TenChuDe}",
                    LessonCount = parentLessonCount
                };

                var parentItem = CreateTreeItem(parentIcon, parent.TenChuDe, parentLessonCount, parentTag, parentTag.Description, isBold: true, isExpanded: true);

                foreach (var child in children)
                {
                    int childCount = _danhSachBaiHoc.Count(b => b.MaChuDe == child.MaChuDe);
                    string childIcon = GetTopicIcon(child.TenChuDe, isParent: false);

                    var childTag = new TreeFilterNodeTag
                    {
                        FilterType = "TOPIC_CHILD",
                        Id = child.MaChuDe,
                        Title = child.TenChuDe,
                        Description = !string.IsNullOrWhiteSpace(child.MoTa) ? child.MoTa : $"Chủ đề chi tiết: {child.TenChuDe}",
                        LessonCount = childCount
                    };

                    var childItem = CreateTreeItem(childIcon, child.TenChuDe, childCount, childTag, childTag.Description);
                    parentItem.Items.Add(childItem);
                }

                branchTopicItems.Add(parentItem);
            }

            var folderTopicsTag = new TreeFilterNodeTag
            {
                FilterType = "FOLDER_TOPICS",
                Title = "Cây Chủ đề học tập",
                Description = "Cây danh mục chủ đề phân cấp từ thực tế đời sống, học thuật IELTS đến CNTT và Thương mại.",
                LessonCount = totalTopicLessons
            };
            var branchTopics = CreateTreeItem("📂", "Cây Chủ đề học tập", totalTopicLessons, folderTopicsTag, folderTopicsTag.Description, isBold: true, isExpanded: true);
            foreach (var t in branchTopicItems)
            {
                branchTopics.Items.Add(t);
            }
            trvDanhMuc.Items.Add(branchTopics);

            // =========================================================================
            // NHÁNH 3: TRÌNH ĐỘ CHUẨN CEFR (LEVELS - Nạp động từ bảng TrinhDo)
            // =========================================================================
            List<TrinhDoDTO> allTrinhDo;
            try
            {
                allTrinhDo = _danhMucDAL.GetAllTrinhDo();
            }
            catch
            {
                allTrinhDo = new List<TrinhDoDTO>();
            }

            int totalLevelLessons = 0;
            var branchLevelItems = new List<TreeViewItem>();

            foreach (var td in allTrinhDo)
            {
                int count = _danhSachBaiHoc.Count(b => b.MaTrinhDo.Equals(td.MaCapDo, StringComparison.OrdinalIgnoreCase));
                totalLevelLessons += count;

                string bullet = td.MaCapDo.ToUpper() switch
                {
                    "A1" => "🟢",
                    "A2" => "🔵",
                    "B1" => "🟣",
                    "B2" => "🟠",
                    "C1" => "🔴",
                    _ => "⭐"
                };

                string title = $"{bullet} {td.MaCapDo} — {td.TenTrinhDo}";
                var tag = new TreeFilterNodeTag
                {
                    FilterType = "LEVEL",
                    Code = td.MaCapDo,
                    Id = td.MaTrinhDo,
                    Title = title,
                    Description = !string.IsNullOrWhiteSpace(td.MoTa) ? td.MoTa : $"Khung năng lực tiếng Anh trình độ {td.MaCapDo}",
                    LessonCount = count
                };

                var item = CreateTreeItem(string.Empty, title, count, tag, tag.Description);
                branchLevelItems.Add(item);
            }

            var folderLevelsTag = new TreeFilterNodeTag
            {
                FilterType = "FOLDER_LEVELS",
                Title = "Trình độ chuẩn CEFR",
                Description = "Khung tham chiếu ngôn ngữ chung Châu Âu từ A1 (Căn bản) đến C1 (Nâng cao).",
                LessonCount = totalLevelLessons
            };
            var branchLevels = CreateTreeItem("📂", "Trình độ chuẩn CEFR", totalLevelLessons, folderLevelsTag, folderLevelsTag.Description, isBold: true, isExpanded: true);
            foreach (var l in branchLevelItems)
            {
                branchLevels.Items.Add(l);
            }
            trvDanhMuc.Items.Add(branchLevels);

            // Cập nhật nhãn nút Reset trên cùng
            if (btnTreeReset != null)
            {
                btnTreeReset.Content = $"🌐 Xem toàn bộ bài học ({totalLessons})";
            }
        }

        /// <summary>
        /// Tạo TreeViewItem hiện đại có biểu tượng, tên danh mục và huy hiệu (pill badge) số lượng bài học
        /// </summary>
        private TreeViewItem CreateTreeItem(string icon, string title, int count, TreeFilterNodeTag tag, string? tooltip = null, bool isBold = false, bool isExpanded = true)
        {
            var item = new TreeViewItem
            {
                Tag = tag,
                IsExpanded = isExpanded,
                ToolTip = !string.IsNullOrWhiteSpace(tooltip) ? tooltip : title
            };

            var stack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 1, 0, 1)
            };

            if (!string.IsNullOrEmpty(icon))
            {
                stack.Children.Add(new TextBlock
                {
                    Text = icon,
                    FontSize = 13,
                    Margin = new Thickness(0, 0, 6, 0),
                    VerticalAlignment = VerticalAlignment.Center
                });
            }

            stack.Children.Add(new TextBlock
            {
                Text = title,
                FontSize = 12.5,
                FontWeight = isBold ? FontWeights.SemiBold : FontWeights.Normal,
                VerticalAlignment = VerticalAlignment.Center
            });

            // Huy hiệu số bài học
            var badgeBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(6, 1, 6, 1),
                Margin = new Thickness(6, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            badgeBorder.Child = new TextBlock
            {
                Text = count.ToString(),
                FontSize = 10.5,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139))
            };

            stack.Children.Add(badgeBorder);
            item.Header = stack;
            return item;
        }

        private string GetTopicIcon(string topicName, bool isParent)
        {
            string n = topicName.ToLower();
            if (isParent)
            {
                if (n.Contains("giao tiếp")) return "☕";
                if (n.Contains("ielts") || n.Contains("học thuật")) return "🎓";
                if (n.Contains("ngữ pháp")) return "📐";
                if (n.Contains("thương mại") || n.Contains("công sở")) return "💼";
                if (n.Contains("công nghệ") || n.Contains("it") || n.Contains("phần mềm")) return "💻";
                return "📁";
            }
            else
            {
                if (n.Contains("chào hỏi") || n.Contains("giới thiệu")) return "👋";
                if (n.Contains("mua sắm") || n.Contains("nhà hàng") || n.Contains("ẩm thực")) return "🛍️";
                if (n.Contains("du lịch") || n.Contains("giao thông") || n.Contains("sân bay")) return "✈️";
                if (n.Contains("reading") || n.Contains("skimming")) return "📑";
                if (n.Contains("writing") || n.Contains("luận")) return "✍️";
                if (n.Contains("listening")) return "🎧";
                if (n.Contains("thì") || n.Contains("tenses")) return "⏳";
                if (n.Contains("mệnh đề") || n.Contains("bị động")) return "🔗";
                if (n.Contains("điều kiện") || n.Contains("đảo ngữ")) return "⚖️";
                if (n.Contains("email") || n.Contains("thư")) return "✉️";
                if (n.Contains("đàm phán") || n.Contains("thuyết trình")) return "🤝";
                if (n.Contains("lập trình") || n.Contains("thuật ngữ")) return "💻";
                if (n.Contains("agile") || n.Contains("scrum")) return "🗣️";
                return "📄";
            }
        }

        private void ApplyFilter()
        {
            if (!_isInitialized || icDanhSachBaiHoc == null || txtTongSoBai == null || txtSoBaiHoanThanh == null)
                return;

            try
            {
                var query = _danhSachBaiHoc.AsEnumerable();

                // 1. Lọc theo Cây danh mục lộ trình (TreeView - TASK-202)
                if (_currentTreeFilter != null)
                {
                    switch (_currentTreeFilter.FilterType)
                    {
                        case "SKILL":
                            if (!string.IsNullOrEmpty(_currentTreeFilter.Code))
                            {
                                query = query.Where(b => b.LoaiBaiHoc.Equals(_currentTreeFilter.Code, StringComparison.OrdinalIgnoreCase));
                            }
                            break;

                        case "TOPIC_CHILD":
                            query = query.Where(b => b.MaChuDe == _currentTreeFilter.Id);
                            break;

                        case "TOPIC_PARENT":
                            var allIds = new HashSet<int>(_currentTreeFilter.ChildTopicIds) { _currentTreeFilter.Id };
                            query = query.Where(b => allIds.Contains(b.MaChuDe));
                            break;

                        case "LEVEL":
                            if (!string.IsNullOrEmpty(_currentTreeFilter.Code))
                            {
                                query = query.Where(b => b.MaTrinhDo.Equals(_currentTreeFilter.Code, StringComparison.OrdinalIgnoreCase));
                            }
                            break;

                        case "FOLDER_TOPICS":
                            query = query.Where(b => b.MaChuDe > 0);
                            break;

                        case "FOLDER_LEVELS":
                            query = query.Where(b => !string.IsNullOrEmpty(b.MaTrinhDo));
                            break;

                        case "FOLDER_SKILLS":
                        default:
                            break;
                    }
                }

                // 2. Lọc theo Skill Chip Bar (nếu khác All)
                if (!string.IsNullOrEmpty(_selectedSkill) && _selectedSkill != "All")
                {
                    query = query.Where(b => b.LoaiBaiHoc.Equals(_selectedSkill, StringComparison.OrdinalIgnoreCase));
                }

                // 3. Lọc theo Trình độ (ComboBox)
                if (cboTrinhDo != null && cboTrinhDo.SelectedItem is ComboBoxItem itemTrinhDo)
                {
                    string trinhDo = itemTrinhDo.Content?.ToString() ?? "Tất cả";
                    if (trinhDo != "Tất cả")
                    {
                        query = query.Where(b => b.MaTrinhDo.Equals(trinhDo, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // 4. Lọc theo Trạng thái (ComboBox)
                if (cboTrangThai != null && cboTrangThai.SelectedItem is ComboBoxItem itemTrangThai)
                {
                    string trangThai = itemTrangThai.Content?.ToString() ?? "Tất cả";
                    if (trangThai != "Tất cả")
                    {
                        query = query.Where(b => b.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // 5. Lọc theo từ khóa tìm kiếm
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

                    if (txtEmptyReason != null && _currentTreeFilter != null)
                    {
                        txtEmptyReason.Text = $"Chưa có bài học nào thuộc '{_currentTreeFilter.Title}' phù hợp với bộ lọc hiện tại.";
                    }
                    else if (txtEmptyReason != null)
                    {
                        txtEmptyReason.Text = "Không có bài học nào trong danh mục hoặc bộ lọc hiện tại.";
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

        private void TrvDanhMuc_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item && item.Tag is TreeFilterNodeTag nodeTag)
            {
                _currentTreeFilter = nodeTag;

                // Cập nhật giao diện Banner Tiêu đề & Mô tả
                UpdateBannerForTreeSelection(nodeTag);

                // Đồng bộ bộ lọc ngoài nếu chọn Kỹ năng hoặc Trình độ
                if (nodeTag.FilterType == "SKILL" && !string.IsNullOrEmpty(nodeTag.Code))
                {
                    SyncSkillButtonsVisual(nodeTag.Code);
                }
                else if (nodeTag.FilterType == "LEVEL" && !string.IsNullOrEmpty(nodeTag.Code))
                {
                    SyncTrinhDoComboBoxVisual(nodeTag.Code);
                }

                ApplyFilter();
            }
        }

        private void UpdateBannerForTreeSelection(TreeFilterNodeTag tag)
        {
            if (txtCurrentNavCategory != null)
            {
                txtCurrentNavCategory.Text = tag.FilterType switch
                {
                    "SKILL" => $"KỸ NĂNG: {tag.Title.ToUpper()}",
                    "TOPIC_PARENT" => $"LỘ TRÌNH: {tag.Title.ToUpper()}",
                    "TOPIC_CHILD" => $"CHỦ ĐỀ: {tag.Title.ToUpper()}",
                    "LEVEL" => $"TRÌNH ĐỘ: {tag.Title.ToUpper()}",
                    "FOLDER_SKILLS" => "TẤT CẢ KỸ NĂNG HỌC TẬP",
                    "FOLDER_TOPICS" => "CÂY CHỦ ĐỀ HỌC TẬP TOÀN DIỆN",
                    "FOLDER_LEVELS" => "KHUNG NĂNG LỰC CHUẨN CEFR",
                    _ => tag.Title.ToUpper()
                };
            }

            if (txtBannerTitle != null)
            {
                txtBannerTitle.Text = tag.FilterType switch
                {
                    "SKILL" => $"Học liệu Kỹ năng: {tag.Title}",
                    "TOPIC_PARENT" => tag.Title,
                    "TOPIC_CHILD" => tag.Title,
                    "LEVEL" => $"Lộ trình trình độ: {tag.Title}",
                    _ => tag.Title
                };
            }

            if (txtBannerSubtitle != null && !string.IsNullOrWhiteSpace(tag.Description))
            {
                txtBannerSubtitle.Text = tag.Description;
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

        private void SyncTrinhDoComboBoxVisual(string levelCode)
        {
            if (cboTrinhDo == null) return;
            for (int i = 0; i < cboTrinhDo.Items.Count; i++)
            {
                if (cboTrinhDo.Items[i] is ComboBoxItem item && 
                    string.Equals(item.Content?.ToString(), levelCode, StringComparison.OrdinalIgnoreCase))
                {
                    cboTrinhDo.SelectedIndex = i;
                    break;
                }
            }
        }

        private void BtnRefreshTree_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ApplyFilter();
        }

        private void BtnTreeReset_Click(object sender, RoutedEventArgs e)
        {
            _currentTreeFilter = null;
            _selectedSkill = "All";

            // Bỏ chọn nút trên TreeView nếu có
            if (trvDanhMuc != null && trvDanhMuc.SelectedItem is TreeViewItem selItem)
            {
                selItem.IsSelected = false;
            }

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
                txtBannerSubtitle.Text = "Luyện tập đa kỹ năng: Từ vựng, Ngữ pháp, Đọc hiểu, Nghe và Viết luận có AI hướng dẫn chi tiết từng câu.";
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
                    "VietLuan" => "Viet",
                    _ => tag
                };

                SyncSkillButtonsVisual(mappedSkill);
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
                HienThiChiTietBaiHoc(maBaiHoc);
            }
        }

        private void HienThiChiTietBaiHoc(int maBaiHoc)
        {
            _currentBaiHoc = BaiHocBUS.LayChiTietBaiHoc(maBaiHoc);
            if (_currentBaiHoc == null) return;

            // TASK-204: Khởi động bộ đếm thời gian học tập thực tế
            _studySeconds = 0;
            _studyTimer?.Start();
            if (txtStudyTimeElapsed != null)
            {
                txtStudyTimeElapsed.Text = "⏱️ Đã học: 00:00";
            }

            // TASK-203: Kiểm tra loại bài học: CHỈ HIỂN THỊ BỘ PHÁT AUDIO KHI HỌC NGHE HIỂU
            bool isListening = string.Equals(_currentBaiHoc.LoaiBaiHoc, "Nghe", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(_currentBaiHoc.LoaiBaiHoc, "NgheHieu", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(_currentBaiHoc.TenLoaiHienThi, "Nghe hiểu", StringComparison.OrdinalIgnoreCase)
                            || (_currentBaiHoc.LoaiBaiHoc != null && _currentBaiHoc.LoaiBaiHoc.IndexOf("Nghe", StringComparison.OrdinalIgnoreCase) >= 0);

            if (pnlAudioPlayerController != null)
            {
                pnlAudioPlayerController.Visibility = isListening ? Visibility.Visible : Visibility.Collapsed;
            }

            // Dừng phát âm thanh khi chuyển bài học hoặc khi bài học không phải là Nghe
            BtnAudioStop_Click(this, new RoutedEventArgs());

            if (isListening)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(_currentBaiHoc.DuongDanAudio))
                    {
                        Uri audioUri;
                        if (Uri.IsWellFormedUriString(_currentBaiHoc.DuongDanAudio, UriKind.Absolute))
                        {
                            audioUri = new Uri(_currentBaiHoc.DuongDanAudio, UriKind.Absolute);
                        }
                        else
                        {
                            string fullPath = System.IO.Path.IsPathRooted(_currentBaiHoc.DuongDanAudio)
                                ? _currentBaiHoc.DuongDanAudio
                                : System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _currentBaiHoc.DuongDanAudio);
                            audioUri = new Uri(fullPath);
                        }
                        _mediaPlayer.Open(audioUri);
                    }
                }
                catch { }

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

            // TASK-202: Nạp nội dung chi tiết bài học đầy đủ, cụ thể cho từng ngữ pháp, từ vựng
            var detailed = BaiHocDetailProvider.GetDetailedContent(_currentBaiHoc);
            txtLectureObjective.Text = detailed.Objective;

            if (txtLectureFormulaTitle != null && !string.IsNullOrEmpty(detailed.FormulaTitle))
                txtLectureFormulaTitle.Text = detailed.FormulaTitle;
            txtLectureFormula.Text = detailed.Formula;

            if (txtLectureExampleTitle != null && !string.IsNullOrEmpty(detailed.ExampleTitle))
                txtLectureExampleTitle.Text = detailed.ExampleTitle;
            txtLectureExample.Text = detailed.Example;

            if (txtLectureTipsTitle != null && !string.IsNullOrEmpty(detailed.TipsTitle))
                txtLectureTipsTitle.Text = detailed.TipsTitle;
            txtLectureTips.Text = detailed.Tips;

            // Bổ sung đoạn văn tiếng Anh cho phần Đọc hiểu (Reading Passage)
            if (bdrReadingPassage != null)
            {
                bool isReading = string.Equals(_currentBaiHoc.LoaiBaiHoc, "Doc", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(_currentBaiHoc.LoaiBaiHoc, "DocHieu", StringComparison.OrdinalIgnoreCase)
                              || string.Equals(_currentBaiHoc.TenLoaiHienThi, "Đọc hiểu", StringComparison.OrdinalIgnoreCase)
                              || !string.IsNullOrWhiteSpace(detailed.ReadingPassageContent);

                if (isReading && !string.IsNullOrWhiteSpace(detailed.ReadingPassageContent))
                {
                    bdrReadingPassage.Visibility = Visibility.Visible;
                    if (txtReadingPassageTitle != null)
                        txtReadingPassageTitle.Text = detailed.ReadingPassageTitle ?? "Reading Comprehension Passage";
                    if (txtReadingPassageContent != null)
                        txtReadingPassageContent.Text = detailed.ReadingPassageContent;
                }
                else
                {
                    bdrReadingPassage.Visibility = Visibility.Collapsed;
                }
            }

            txtChiTietNoiDung.Text = !string.IsNullOrWhiteSpace(detailed.FullContent) 
                ? detailed.FullContent 
                : BaiHocDetailProvider.CleanHtml(_currentBaiHoc.NoiDung);

            // Build Questions UI
            TaoGiaoDienCauHoi(_currentBaiHoc);

            // Default to Theory Tab
            SwitchToTab(isLyThuyet: true);

            // Toggle views
            pnlDanhSachBaiHoc.Visibility = Visibility.Collapsed;
            pnlChiTietBaiHoc.Visibility = Visibility.Visible;
        }

        private void TaoGiaoDienCauHoi(BaiHocDTO baiHoc)
        {
            pnlDanhSachCauHoi.Children.Clear();

            // Đảm bảo bài tập củng cố được tạo và lấy trực tiếp từ nội dung bài học (quy mô 5-10 câu)
            if (baiHoc.DanhSachCauHoi == null || baiHoc.DanhSachCauHoi.Count < 5)
            {
                baiHoc.DanhSachCauHoi = BaiHocDetailProvider.GetPracticeQuestions(baiHoc);
            }

            if (baiHoc.DanhSachCauHoi == null || baiHoc.DanhSachCauHoi.Count == 0)
            {
                var emptyNotice = new TextBlock
                {
                    Text = "Chưa có câu hỏi trắc nghiệm cho bài học này.",
                    Foreground = Brushes.Gray,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 10, 0, 10)
                };
                pnlDanhSachCauHoi.Children.Add(emptyNotice);
                return;
            }

            for (int i = 0; i < baiHoc.DanhSachCauHoi.Count; i++)
            {
                var ch = baiHoc.DanhSachCauHoi[i];

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

                // Question Title
                var txtQ = new TextBlock
                {
                    Text = $"Câu {i + 1}: {ch.NoiDungCauHoi}",
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 12)
                };
                spCard.Children.Add(txtQ);

                // Explanation Box (Hidden by default)
                var bdrGiaiThich = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(240, 253, 250)),
                    BorderBrush = GetBrush("BrandMintSoft", new SolidColorBrush(Color.FromRgb(204, 251, 241))),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(12, 10, 12, 10),
                    Margin = new Thickness(0, 10, 0, 0),
                    Visibility = Visibility.Collapsed
                };
                var txtGiaiThich = new TextBlock
                {
                    Text = $"💡 Giải thích: {ch.GiaiThichChiTiet}",
                    Foreground = GetBrush("BrandTealDark", new SolidColorBrush(Color.FromRgb(4, 47, 46))),
                    FontSize = 12.5,
                    TextWrapping = TextWrapping.Wrap
                };
                bdrGiaiThich.Child = txtGiaiThich;

                // Options list
                for (int optIdx = 0; optIdx < ch.DanhSachLuaChon.Count; optIdx++)
                {
                    string optionText = ch.DanhSachLuaChon[optIdx];
                    char optionLetter = (char)('A' + optIdx);
                    int currentOpt = optIdx;

                    var rb = new RadioButton
                    {
                        GroupName = $"Q_{ch.MaCauHoi}",
                        Content = $"{optionLetter}. {optionText}",
                        FontSize = 13,
                        Margin = new Thickness(4, 4, 4, 8),
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    rb.Checked += (s, ev) =>
                    {
                        ch.LuaChonCuaNguoiDung = currentOpt;
                        bool isCorrect = (currentOpt == ch.DapAnDungIndex);
                        ch.DaTraLoiDung = isCorrect;

                        bdrGiaiThich.Visibility = Visibility.Visible;
                        if (isCorrect)
                        {
                            txtGiaiThich.Text = $"✅ Chính xác! {ch.GiaiThichChiTiet}";
                            bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                        }
                        else
                        {
                            char correctLetter = (char)('A' + ch.DapAnDungIndex);
                            txtGiaiThich.Text = $"❌ Chưa đúng (Đáp án đúng là {correctLetter}). {ch.GiaiThichChiTiet}";
                            bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                        }
                    };

                    spCard.Children.Add(rb);
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
        }

        private void BtnChuyenSangBaiTap_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: false);
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
                btnAudioPlayPause.Content = "⏸️ Tạm dừng";
                txtAudioStatus.Text = "Đang phát âm thanh bài giảng (Audio Tutor • MediaPlayer)...";
                _audioTimer?.Start();
            }
            else
            {
                try
                {
                    _mediaPlayer.Pause();
                }
                catch { }
                btnAudioPlayPause.Content = "▶️ Tiếp tục nghe";
                txtAudioStatus.Text = "Đã tạm dừng phát âm thanh.";
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
            if (txtAudioCurrentTime != null) txtAudioCurrentTime.Text = "00:00";
            if (btnAudioPlayPause != null) btnAudioPlayPause.Content = "▶️ Nghe bài giảng";
            if (txtAudioStatus != null) txtAudioStatus.Text = "Bộ phát âm thanh bài học (Audio Tutor • MediaPlayer)";
        }

        private void BtnAudioRewind_Click(object sender, RoutedEventArgs e)
        {
            _audioCurrentSeconds = Math.Max(0, _audioCurrentSeconds - 5);
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
            if (sldAudioProgress != null && _audioTotalSeconds > 0)
            {
                sldAudioProgress.Value = (_audioCurrentSeconds / _audioTotalSeconds) * 100.0;
            }
            if (txtAudioCurrentTime != null)
            {
                txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
            }
        }

        private void BtnAudioForward_Click(object sender, RoutedEventArgs e)
        {
            _audioCurrentSeconds = Math.Min(_audioTotalSeconds, _audioCurrentSeconds + 5);
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
            if (sldAudioProgress != null && _audioTotalSeconds > 0)
            {
                sldAudioProgress.Value = (_audioCurrentSeconds / _audioTotalSeconds) * 100.0;
            }
            if (txtAudioCurrentTime != null)
            {
                txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
            }
        }

        private void SldAudioVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldAudioVolume != null)
            {
                try
                {
                    _mediaPlayer.Volume = Math.Clamp(sldAudioVolume.Value, 0.0, 1.0);
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
            btnAudioSpeed.Content = $"{_currentSpeed:0.0}x";
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
                            synth.Volume = (int)(sldAudioVolume.Value * 100);
                            synth.Rate = (int)((_currentSpeed - 1.0) * 3);
                            string textToRead = !string.IsNullOrWhiteSpace(_currentBaiHoc.TomTat) 
                                ? _currentBaiHoc.TomTat 
                                : _currentBaiHoc.TieuDe;
                            synth.Speak(textToRead);
                        }
                    }
                    catch { }
                });

                txtAudioStatus.Text = "🤖 AI Audio Tutor đang đọc tóm tắt bài học...";
                btnAudioPlayPause.Content = "⏸️ Đang đọc...";
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
            _audioCurrentSeconds = (sldAudioProgress.Value / 100.0) * _audioTotalSeconds;
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(_audioCurrentSeconds);
            }
            catch { }
        }

        private void SldAudioProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isDraggingSlider)
            {
                _audioCurrentSeconds = (sldAudioProgress.Value / 100.0) * _audioTotalSeconds;
                if (txtAudioCurrentTime != null)
                {
                    txtAudioCurrentTime.Text = $"{(int)_audioCurrentSeconds / 60:D2}:{(int)_audioCurrentSeconds % 60:D2}";
                }
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

            int totalQuestions = _currentBaiHoc.DanhSachCauHoi?.Count ?? 0;
            int answeredCount = _currentBaiHoc.DanhSachCauHoi?.Count(q => q.LuaChonCuaNguoiDung >= 0) ?? 0;

            // KIỂM TRA ĐIỀU KIỆN: Phải hoàn thành toàn bộ bài tập củng cố thì mới cho hoàn thành 100%
            if (totalQuestions > 0 && answeredCount < totalQuestions)
            {
                Common.ThongBaoDialog.CanhBao(
                    $"⚠️ BẠN CHƯA HOÀN THÀNH BÀI TẬP CỦNG CỐ!\n\n" +
                    $"• Bạn mới trả lời: {answeredCount}/{totalQuestions} câu hỏi.\n" +
                    $"• Quy định: Bạn bắt buộc phải trả lời đầy đủ tất cả câu hỏi bài tập củng cố để kiểm tra kiến thức trước khi được hệ thống ghi nhận hoàn thành 100% bài học.\n\n" +
                    $"Hệ thống sẽ chuyển bạn sang tab 'Bài tập thực hành' ngay bây giờ.",
                    "Yêu cầu làm bài tập củng cố");

                SwitchToTab(isLyThuyet: false);
                return;
            }

            // Đã hoàn thành toàn bộ câu hỏi: Tính điểm thực tế
            int correctCount = _currentBaiHoc.DanhSachCauHoi?.Count(q => q.DaTraLoiDung == true) ?? 0;
            double scoreOutOf10 = totalQuestions > 0 ? Math.Round((double)correctCount / totalQuestions * 10, 1) : 10.0;
            string diemSo = $"{scoreOutOf10:0.0}/10 ({correctCount}/{totalQuestions} đúng)";

            int minutesSpent = Math.Max(1, _studySeconds / 60);
            BaiHocBUS.CongDonThoiGianHoc(minutesSpent);
            BaiHocBUS.CapNhatTienDo(_currentBaiHoc.MaBaiHoc, 100, "Hoàn thành", diemSo);

            _currentBaiHoc.PhanTramTienDo = 100;
            _currentBaiHoc.TrangThai = "Hoàn thành";
            txtChiTietTienDo.Text = "100%";
            txtChiTietTrangThai.Text = "Hoàn thành";

            _studyTimer?.Stop();
            BtnAudioStop_Click(this, new RoutedEventArgs());

            Common.ThongBaoDialog.ThanhCong(
                $"🎉 CHÚC MỪNG BẠN ĐÃ HOÀN THÀNH BÀI HỌC 100%!\n\n" +
                $"• Bài học: {_currentBaiHoc.TieuDe}\n" +
                $"• Kết quả bài tập củng cố: {correctCount}/{totalQuestions} câu đúng ({scoreOutOf10:0.0}/10 điểm)\n" +
                $"• Thời gian học tập thực tế: {minutesSpent} phút (đã cộng vào hồ sơ cá nhân)\n" +
                $"• Trạng thái ghi nhận: 100% (Hoàn thành)", 
                "Hoàn thành bài học");

            BtnQuayLai_Click(sender, e);
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
