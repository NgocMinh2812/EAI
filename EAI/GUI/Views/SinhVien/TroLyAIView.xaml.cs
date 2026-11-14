using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using BUS;
using BUS.AI;
using BUS.AI.Configuration;
using BUS.AI.Models;
using GUI.Views.Common;

namespace GUI.Views
{
    public partial class TroLyAIView : Page
    {
        #region Nested Data Models
        public class ChatMessage
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public bool IsUser { get; set; }
            public string Content { get; set; } = string.Empty;
            public DateTime Timestamp { get; set; } = DateTime.Now;
            public bool IsError { get; set; }
        }

        public class ChatSession
        {
            public long DbId { get; set; } = 0;
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Title { get; set; } = "Cuộc hội thoại mới";
            public string Category { get; set; } = "General"; // Grammar, Vocab, Explain, Speaking, General
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public DateTime UpdatedAt { get; set; } = DateTime.Now;
            public List<ChatMessage> Messages { get; set; } = new();
        }
        #endregion

        #region Fields & State
        private readonly List<ChatSession> _sessions = new();
        private ChatSession? _currentSession;
        private string _currentFilterCategory = "All";
        private string _currentLearningMode = "All";
        private string _lastFailedQuery = string.Empty;
        private SpeechSynthesizer? _speechSynthesizer;
        private DispatcherTimer? _dotsTimer;
        private int _dotCount = 0;
        private int CurrentUserId => SessionHelper.CurrentMaNguoiDung;
        #endregion

        public TroLyAIView()
        {
            InitializeComponent();
            InitSpeechSynthesizer();
            InitLoadingDotsAnimation();
            UpdateActiveModelBadge();
            LoadHistoryFromDatabase();
            RefreshHistoryList();

            // Chọn cuộc trò chuyện đầu tiên hoặc chuẩn bị phiên mới
            if (_sessions.Count > 0)
            {
                SelectSession(_sessions[0]);
            }
            else
            {
                BtnNewChat_Click(this, new RoutedEventArgs());
            }
        }

        private void UpdateActiveModelBadge()
        {
            try
            {
                string prov = AIConfigurationService.Instance.ActiveProvider.ToString();
                string mod = AIConfigurationService.Instance.ActiveModel;
                txtActiveModelBadge.Text = $"🤖 {prov}: {mod}";
            }
            catch
            {
                txtActiveModelBadge.Text = "🤖 AI Service";
            }
        }

        private void LoadHistoryFromDatabase()
        {
            try
            {
                if (CurrentUserId <= 0)
                {
                    _sessions.Clear();
                    return;
                }

                var dbList = HoiThoaiAIBUS.Instance.LayDanhSachPhienHoiThoai(CurrentUserId);
                _sessions.Clear();

                if (dbList != null && dbList.Count > 0)
                {
                    foreach (var p in dbList)
                    {
                        var s = new ChatSession
                        {
                            DbId = p.MaPhienHoiThoai,
                            Title = p.TieuDe,
                            Category = string.IsNullOrWhiteSpace(p.NguCanhHoiThoai) ? "General" : p.NguCanhHoiThoai,
                            CreatedAt = p.NgayTao,
                            UpdatedAt = p.TinNhanCuoiVao ?? p.NgayTao
                        };

                        var msgs = HoiThoaiAIBUS.Instance.LayDanhSachTinNhan(p.MaPhienHoiThoai);
                        if (msgs != null)
                        {
                            foreach (var m in msgs)
                            {
                                s.Messages.Add(new ChatMessage
                                {
                                    Id = m.MaTinNhan.ToString(),
                                    IsUser = m.LoaiNguoiGui == "NguoiDung",
                                    Content = m.NoiDungTinNhan,
                                    Timestamp = m.NgayGui
                                });
                            }
                        }

                        _sessions.Add(s);
                    }
                }
                else
                {
                    SeedInitialConversationsToDb();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TroLyAIView] Lỗi load DB: {ex.Message}");
                InitSeedConversations();
            }
        }

        private void SeedInitialConversationsToDb()
        {
            if (CurrentUserId <= 0)
            {
                InitSeedConversations();
                return;
            }

            try
            {
                long p1 = HoiThoaiAIBUS.Instance.TaoPhienHoiThoai(CurrentUserId, "Phân biệt Thì HTHT & QKĐ", "Grammar");
                if (p1 > 0)
                {
                    HoiThoaiAIBUS.Instance.LuuTinNhan(p1, "NguoiDung", "Phân biệt thì Present Perfect và Past Simple khi nào dùng?");
                    HoiThoaiAIBUS.Instance.LuuTinNhan(p1, "AI",
                        "💡 Phân biệt nhanh Hiện tại hoàn thành (Present Perfect) & Quá khứ đơn (Past Simple):\n\n" +
                        "1. Quá khứ đơn (S + V2/ed):\n" +
                        "• Dùng cho hành động ĐÃ CHẤM DỨT hoàn toàn trong quá khứ, có mốc thời gian xác định (yesterday, last year, in 2020, 2 days ago).\n" +
                        "• Ví dụ: \"I visited Tokyo in 2019.\" (Chuyến đi đã kết thúc và năm 2019 đã qua).\n\n" +
                        "2. Hiện tại hoàn thành (S + have/has + V3/ed):\n" +
                        "• Dùng cho trải nghiệm tính đến hiện tại, hoặc hành động xảy ra trong quá khứ nhưng KẾT QUẢ/HẬU QUẢ vẫn ảnh hưởng tới hiện tại (since, for, already, yet, ever).\n" +
                        "• Ví dụ: \"I have lost my passport.\" (Tôi đã làm mất hộ chiếu và hiện tại vẫn chưa tìm thấy để làm thủ tục).\n\n" +
                        "🎯 Mẹo làm bài: Nếu trong câu có mốc thời gian quá khứ rõ ràng ➜ Chọn Quá khứ đơn. Nếu có since/for hoặc nhấn mạnh trải nghiệm ➜ Chọn Hiện tại hoàn thành.");
                }

                long p2 = HoiThoaiAIBUS.Instance.TaoPhienHoiThoai(CurrentUserId, "5 Collocations Environment", "Vocab");
                if (p2 > 0)
                {
                    HoiThoaiAIBUS.Instance.LuuTinNhan(p2, "NguoiDung", "5 Collocations học thuật chủ đề Môi trường và Biến đổi khí hậu");
                    HoiThoaiAIBUS.Instance.LuuTinNhan(p2, "AI",
                        "🌿 5 Collocations học thuật cực kỳ ăn điểm về chủ đề Environment (IELTS Band 7.0+):\n\n" +
                        "1. Combat climate change: Đấu tranh chống lại biến đổi khí hậu.\n" +
                        "   Ví dụ: \"Governments must implement urgent policies to combat climate change.\"\n\n" +
                        "2. Renewable energy sources: Các nguồn năng lượng tái tạo (điện mặt trời, gió).\n" +
                        "   Ví dụ: \"Transitioning to renewable energy sources is essential to reduce carbon emissions.\"\n\n" +
                        "3. Ecological degradation: Sự suy thoái môi trường sinh thái.\n" +
                        "4. Carbon footprint: Dấu chân carbon / lượng khí thải carbon cá nhân.\n" +
                        "5. Sustainable development: Phát triển bền vững.");
                }

                var dbList = HoiThoaiAIBUS.Instance.LayDanhSachPhienHoiThoai(CurrentUserId);
                _sessions.Clear();
                foreach (var p in dbList)
                {
                    var s = new ChatSession
                    {
                        DbId = p.MaPhienHoiThoai,
                        Title = p.TieuDe,
                        Category = string.IsNullOrWhiteSpace(p.NguCanhHoiThoai) ? "General" : p.NguCanhHoiThoai,
                        CreatedAt = p.NgayTao,
                        UpdatedAt = p.TinNhanCuoiVao ?? p.NgayTao
                    };
                    var msgs = HoiThoaiAIBUS.Instance.LayDanhSachTinNhan(p.MaPhienHoiThoai);
                    foreach (var m in msgs)
                    {
                        s.Messages.Add(new ChatMessage
                        {
                            Id = m.MaTinNhan.ToString(),
                            IsUser = m.LoaiNguoiGui == "NguoiDung",
                            Content = m.NoiDungTinNhan,
                            Timestamp = m.NgayGui
                        });
                    }
                    _sessions.Add(s);
                }
            }
            catch
            {
                InitSeedConversations();
            }
        }

        #region Speech & Animations Init
        private void InitSpeechSynthesizer()
        {
            try
            {
                _speechSynthesizer = new SpeechSynthesizer();
                _speechSynthesizer.SetOutputToDefaultAudioDevice();
            }
            catch
            {
                _speechSynthesizer = null;
            }
        }

        private void InitLoadingDotsAnimation()
        {
            _dotsTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(400)
            };
            _dotsTimer.Tick += (s, e) =>
            {
                _dotCount = (_dotCount + 1) % 4;
                txtLoadingDots.Text = new string('.', _dotCount == 0 ? 1 : _dotCount) + " ";
            };
        }
        #endregion

        #region Seed Conversations
        private void InitSeedConversations()
        {
            // 1. Phân biệt thì Hiện tại hoàn thành vs Quá khứ đơn
            var s1 = new ChatSession
            {
                Title = "Phân biệt Thì HTHT & QKĐ",
                Category = "Grammar",
                UpdatedAt = DateTime.Now.AddHours(-1)
            };
            s1.Messages.Add(new ChatMessage
            {
                IsUser = true,
                Content = "Phân biệt thì Present Perfect và Past Simple khi nào dùng?",
                Timestamp = DateTime.Now.AddHours(-1)
            });
            s1.Messages.Add(new ChatMessage
            {
                IsUser = false,
                Content = "💡 Phân biệt nhanh Hiện tại hoàn thành (Present Perfect) & Quá khứ đơn (Past Simple):\n\n" +
                          "1. Quá khứ đơn (S + V2/ed):\n" +
                          "• Dùng cho hành động ĐÃ CHẤM DỨT hoàn toàn trong quá khứ, có mốc thời gian xác định (yesterday, last year, in 2020, 2 days ago).\n" +
                          "• Ví dụ: \"I visited Tokyo in 2019.\" (Chuyến đi đã kết thúc và năm 2019 đã qua).\n\n" +
                          "2. Hiện tại hoàn thành (S + have/has + V3/ed):\n" +
                          "• Dùng cho trải nghiệm tính đến hiện tại, hoặc hành động xảy ra trong quá khứ nhưng KẾT QUẢ/HẬU QUẢ vẫn ảnh hưởng tới hiện tại (since, for, already, yet, ever).\n" +
                          "• Ví dụ: \"I have lost my passport.\" (Tôi đã làm mất hộ chiếu và hiện tại vẫn chưa tìm thấy để làm thủ tục).\n\n" +
                          "🎯 Mẹo làm bài: Nếu trong câu có mốc thời gian quá khứ rõ ràng ➜ Chọn Quá khứ đơn. Nếu có since/for hoặc nhấn mạnh trải nghiệm ➜ Chọn Hiện tại hoàn thành.",
                Timestamp = DateTime.Now.AddHours(-1).AddMinutes(1)
            });

            // 2. 5 Collocations học thuật chủ đề Môi trường
            var s2 = new ChatSession
            {
                Title = "5 Collocations Environment",
                Category = "Vocab",
                UpdatedAt = DateTime.Now.AddHours(-3)
            };
            s2.Messages.Add(new ChatMessage
            {
                IsUser = true,
                Content = "5 Collocations học thuật chủ đề Môi trường và Biến đổi khí hậu",
                Timestamp = DateTime.Now.AddHours(-3)
            });
            s2.Messages.Add(new ChatMessage
            {
                IsUser = false,
                Content = "🌿 5 Collocations học thuật cực kỳ ăn điểm về chủ đề Environment (IELTS Band 7.0+):\n\n" +
                          "1. Combat climate change: Đấu tranh chống lại biến đổi khí hậu.\n" +
                          "   Ví dụ: \"Governments must implement urgent policies to combat climate change.\"\n\n" +
                          "2. Renewable energy sources: Các nguồn năng lượng tái tạo (điện mặt trời, gió).\n" +
                          "   Ví dụ: \"Investing in renewable energy sources will reduce reliance on fossil fuels.\"\n\n" +
                          "3. Drastic reduction in carbon emissions: Cắt giảm quyết liệt lượng khí thải carbon.\n\n" +
                          "4. Ecological footprint: Dấu chân sinh thái / Mức độ tác động đến hệ sinh thái.\n\n" +
                          "5. Sustainable development: Phát triển bền vững bảo vệ thế hệ tương lai.",
                Timestamp = DateTime.Now.AddHours(-3).AddMinutes(1)
            });

            // 3. Luyện giao tiếp gọi món
            var s3 = new ChatSession
            {
                Title = "Roleplay: Đặt món nhà hàng",
                Category = "Speaking",
                UpdatedAt = DateTime.Now.AddDays(-1)
            };
            s3.Messages.Add(new ChatMessage
            {
                IsUser = true,
                Content = "Let's practice ordering food at a restaurant in English!",
                Timestamp = DateTime.Now.AddDays(-1)
            });
            s3.Messages.Add(new ChatMessage
            {
                IsUser = false,
                Content = "🍽️ Wonderful! I will be your server today at 'The Green Bistro'.\n\n" +
                          "\"Good evening! Welcome to The Green Bistro. Here is your menu. Would you like to start with something to drink while you decide on your main course?\"\n\n" +
                          "👉 Gợi ý câu trả lời cho bạn:\n" +
                          "• \"Yes, please. I'd like a glass of sparkling water, please.\"\n" +
                          "• \"Could you recommend your signature cocktail or drink?\"\n\n" +
                          "Bạn hãy gõ câu trả lời của mình bên dưới để chúng ta cùng tiếp tục nhé!",
                Timestamp = DateTime.Now.AddDays(-1).AddMinutes(1)
            });

            _sessions.Add(s1);
            _sessions.Add(s2);
            _sessions.Add(s3);
        }
        #endregion

        #region Conversation History Sidebar
        private void RefreshHistoryList()
        {
            pnlHistoryList.Children.Clear();
            string search = txtSearchHistory.Text.Trim().ToLower();

            var filtered = _sessions.Where(s =>
            {
                // Bộ lọc category
                bool matchCat = _currentFilterCategory == "All" ||
                                string.Equals(s.Category, _currentFilterCategory, StringComparison.OrdinalIgnoreCase);

                // Tìm kiếm theo tiêu đề hoặc nội dung
                bool matchSearch = string.IsNullOrEmpty(search) ||
                                   s.Title.ToLower().Contains(search) ||
                                   s.Messages.Any(m => m.Content.ToLower().Contains(search));

                return matchCat && matchSearch;
            }).OrderByDescending(s => s.UpdatedAt).ToList();

            txtHistoryCountBadge.Text = $"{filtered.Count} đoạn chat";

            if (filtered.Count == 0)
            {
                var emptyNotice = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                    BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(14, 20, 14, 20),
                    Margin = new Thickness(8, 20, 8, 0)
                };
                var emptyStack = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
                emptyStack.Children.Add(new LucideIcon
                {
                    Kind = LucideIconKind.MessageSquare,
                    Size = 24,
                    Foreground = (SolidColorBrush)FindResource("TextGray"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 8)
                });
                emptyStack.Children.Add(new TextBlock
                {
                    Text = "Chưa có đoạn chat nào",
                    FontSize = 12.5,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = (SolidColorBrush)FindResource("TextDark"),
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                emptyStack.Children.Add(new TextBlock
                {
                    Text = "Bấm '+ Cuộc hội thoại mới' để bắt đầu",
                    FontSize = 11,
                    Foreground = (SolidColorBrush)FindResource("TextGray"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 4, 0, 0)
                });
                emptyNotice.Child = emptyStack;
                pnlHistoryList.Children.Add(emptyNotice);
                return;
            }

            foreach (var session in filtered)
            {
                pnlHistoryList.Children.Add(CreateHistoryItemCard(session));
            }
        }

        private UIElement CreateHistoryItemCard(ChatSession session)
        {
            bool isActive = _currentSession == session;

            var itemBorder = new Border
            {
                Background = isActive ? (SolidColorBrush)FindResource("BrandTealLight") : Brushes.White,
                BorderBrush = isActive ? (SolidColorBrush)FindResource("BrandTeal") : (SolidColorBrush)FindResource("BorderLight"),
                BorderThickness = new Thickness(isActive ? 1.5 : 1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(12, 10, 8, 10),
                Margin = new Thickness(6, 3, 6, 4),
                Cursor = Cursors.Hand,
                Tag = session
            };

            // Grid chia làm 3 phần: Icon phân loại + Thông tin session + Nút xóa
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(28) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(28) });

            // 1. Icon phân loại
            var iconBorder = new Border
            {
                Width = 24,
                Height = 24,
                CornerRadius = new CornerRadius(6),
                Background = GetCategoryBgBrush(session.Category),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            var catIcon = new LucideIcon
            {
                Kind = GetCategoryLucideKind(session.Category),
                Size = 13,
                Foreground = GetCategoryFgBrush(session.Category),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            iconBorder.Child = catIcon;
            Grid.SetColumn(iconBorder, 0);

            // 2. Nội dung Tiêu đề & Thời gian
            var textStack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0, 6, 0)
            };
            var txtTitle = new TextBlock
            {
                Text = session.Title,
                FontWeight = isActive ? FontWeights.Bold : FontWeights.SemiBold,
                FontSize = 12.5,
                Foreground = isActive ? (SolidColorBrush)FindResource("BrandTeal") : (SolidColorBrush)FindResource("TextDark"),
                TextTrimming = TextTrimming.CharacterEllipsis,
                MaxWidth = 160
            };
            var txtTime = new TextBlock
            {
                Text = FormatSessionTime(session.UpdatedAt),
                FontSize = 11,
                Foreground = (SolidColorBrush)FindResource("TextGray"),
                Margin = new Thickness(0, 2, 0, 0)
            };
            textStack.Children.Add(txtTitle);
            textStack.Children.Add(txtTime);
            Grid.SetColumn(textStack, 1);

            // 3. Nút Xóa cuộc trò chuyện (Key requirement: Xóa hội thoại)
            var btnDelete = new Button
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                Width = 26,
                Height = 26,
                Padding = new Thickness(0),
                ToolTip = "Xóa cuộc hội thoại này",
                Tag = session
            };
            var delTemplate = new ControlTemplate(typeof(Button));
            var delFeFactory = new FrameworkElementFactory(typeof(Border));
            delFeFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            delFeFactory.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            var iconFactory = new FrameworkElementFactory(typeof(LucideIcon));
            iconFactory.SetValue(LucideIcon.KindProperty, LucideIconKind.Trash2);
            iconFactory.SetValue(LucideIcon.SizeProperty, 13.0);
            iconFactory.SetValue(LucideIcon.ForegroundProperty, (SolidColorBrush)FindResource("TextGray"));
            delFeFactory.AppendChild(iconFactory);
            delTemplate.VisualTree = delFeFactory;
            btnDelete.Template = delTemplate;

            btnDelete.Click += (s, e) =>
            {
                e.Handled = true;
                DeleteSession(session);
            };
            Grid.SetColumn(btnDelete, 2);

            grid.Children.Add(iconBorder);
            grid.Children.Add(textStack);
            grid.Children.Add(btnDelete);
            itemBorder.Child = grid;

            // Click vào card để chuyển session
            itemBorder.MouseLeftButtonDown += (s, e) =>
            {
                SelectSession(session);
            };

            return itemBorder;
        }

        private void SelectSession(ChatSession session)
        {
            _currentSession = session;
            pnlChatMessages.Children.Clear();

            if (session.Messages.Count == 0)
            {
                pnlWelcomeHero.Visibility = Visibility.Visible;
            }
            else
            {
                pnlWelcomeHero.Visibility = Visibility.Collapsed;
                foreach (var msg in session.Messages)
                {
                    if (msg.IsUser)
                    {
                        AddUserBubbleToUI(msg.Content, msg.Timestamp);
                    }
                    else
                    {
                        AddAIBubbleToUI(msg.Content, msg.Timestamp, msg.IsError);
                    }
                }
            }

            // Cập nhật lại chỉ báo chủ đề
            UpdateModeIndicator(session.Category);
            RefreshHistoryList();
            ScrollChatToEnd();
        }

        private void DeleteSession(ChatSession session)
        {
            if (ThongBaoDialog.XacNhan(
                $"Bạn có chắc chắn muốn xóa cuộc hội thoại:\n\"{session.Title}\" không?\n\nToàn bộ lịch sử tin nhắn trong phiên này sẽ bị xóa khỏi cơ sở dữ liệu.",
                "Xác nhận xóa hội thoại",
                "Xóa ngay",
                "Giữ lại"))
            {
                try
                {
                    if (session.DbId > 0 && CurrentUserId > 0)
                    {
                        HoiThoaiAIBUS.Instance.XoaPhienHoiThoai(session.DbId, CurrentUserId);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[TroLyAIView] Lỗi xóa DB: {ex.Message}");
                }

                _sessions.Remove(session);

                if (_currentSession == session)
                {
                    if (_sessions.Count > 0)
                    {
                        SelectSession(_sessions[0]);
                    }
                    else
                    {
                        BtnNewChat_Click(this, new RoutedEventArgs());
                    }
                }
                else
                {
                    RefreshHistoryList();
                }

                ThongBaoDialog.ThanhCong("Đã xóa cuộc hội thoại thành công.", "Thành công");
            }
        }

        private void BtnNewChat_Click(object sender, RoutedEventArgs e)
        {
            string cat = _currentLearningMode == "All" ? "General" : _currentLearningMode;
            long newDbId = 0;
            if (CurrentUserId > 0)
            {
                try
                {
                    newDbId = HoiThoaiAIBUS.Instance.TaoPhienHoiThoai(CurrentUserId, "Cuộc hội thoại mới", cat);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[TroLyAIView] Lỗi tạo phiên DB: {ex.Message}");
                }
            }

            var newSession = new ChatSession
            {
                DbId = newDbId,
                Title = "Cuộc hội thoại mới",
                Category = cat,
                UpdatedAt = DateTime.Now
            };
            _sessions.Insert(0, newSession);
            SelectSession(newSession);
            txtChatInput.Focus();
        }

        private void TxtSearchHistory_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtSearchPlaceholder.Visibility = string.IsNullOrEmpty(txtSearchHistory.Text) ? Visibility.Visible : Visibility.Collapsed;
            RefreshHistoryList();
        }

        private void BtnFilterCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                _currentFilterCategory = tag;

                btnFilterAll.Style = (Style)FindResource("FilterTabPill");
                btnFilterGrammar.Style = (Style)FindResource("FilterTabPill");
                btnFilterVocab.Style = (Style)FindResource("FilterTabPill");
                btnFilterComm.Style = (Style)FindResource("FilterTabPill");

                btn.Style = (Style)FindResource("ActiveFilterTabPill");
                RefreshHistoryList();
            }
        }
        #endregion

        #region Header Learning Modes & Actions
        private void BtnMode_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string mode)
            {
                _currentLearningMode = mode;

                btnModeAll.Style = (Style)FindResource("FilterTabPill");
                btnModeGrammar.Style = (Style)FindResource("FilterTabPill");
                btnModeVocab.Style = (Style)FindResource("FilterTabPill");
                btnModeExplain.Style = (Style)FindResource("FilterTabPill");
                btnModeSpeaking.Style = (Style)FindResource("FilterTabPill");

                btn.Style = (Style)FindResource("ActiveFilterTabPill");
                UpdateModeIndicator(mode);

                if (_currentSession != null)
                {
                    _currentSession.Category = mode;
                    RefreshHistoryList();
                }
            }
        }

        private void UpdateModeIndicator(string mode)
        {
            txtCurrentModeIndicator.Text = mode switch
            {
                "Grammar" => "Chế độ: Giải đáp Ngữ pháp chuyên sâu",
                "Vocab" => "Chế độ: Trau dồi Từ vựng & Collocation",
                "Explain" => "Chế độ: Giải thích đáp án & Mẹo bài thi",
                "Speaking" => "Chế độ: Luyện phản xạ Giao tiếp 2 chiều",
                _ => "Chế độ: Gia sư AI tổng hợp (Song ngữ)"
            };
        }

        private void BtnClearCurrentChat_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSession == null || _currentSession.Messages.Count == 0)
            {
                ThongBaoDialog.ThongTin("Cuộc trò chuyện hiện tại đang trống.", "Thông báo");
                return;
            }

            if (ThongBaoDialog.XacNhan(
                "Bạn có chắc muốn làm trống cuộc trò chuyện này không?",
                "Xác nhận xóa đoạn chat",
                "Xóa sạch",
                "Hủy"))
            {
                _currentSession.Messages.Clear();
                pnlChatMessages.Children.Clear();
                pnlWelcomeHero.Visibility = Visibility.Visible;
                RefreshHistoryList();
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            ThongBaoDialog.ThongTin(
                "🎓 Chào mừng bạn đến với Gia sư AI TiếngAnhAI!\n\n" +
                "• Bạn có thể hỏi bất kỳ thắc mắc nào bằng tiếng Việt hoặc tiếng Anh.\n" +
                "• AI hỗ trợ: Giải đáp ngữ pháp, tra từ vựng kèm Collocations, phân tích vì sao đáp án đúng/sai, và luyện giao tiếp phản xạ.\n" +
                "• Mỗi câu trả lời của AI đều có nút [Sao chép] và nút [Nghe phát âm] bằng giọng đọc tiếng Anh tự nhiên.\n" +
                "• Nhấn '+ Cuộc hội thoại mới' bên cột trái để mở chủ đề học tập mới bất cứ lúc nào.",
                "Hướng dẫn sử dụng Gia sư AI");
        }
        #endregion

        #region Prompt Suggestions & Inputs
        private void BtnPrompt_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string prompt = btn.Content?.ToString() ?? string.Empty;
                if (prompt.StartsWith("💡 ") || prompt.StartsWith("📝 ") || prompt.StartsWith("📚 ") ||
                    prompt.StartsWith("🎯 ") || prompt.StartsWith("🗣️ "))
                {
                    prompt = prompt.Substring(2).Trim();
                }
                txtChatInput.Text = prompt;
                ExecuteSendMessage(prompt);
            }
        }

        private void BtnSuggestionCard_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is string prompt)
            {
                txtChatInput.Text = prompt;
                ExecuteSendMessage(prompt);
            }
        }

        private void TxtChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Shift + Enter để xuống dòng, Enter đơn để gửi
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    int caret = txtChatInput.CaretIndex;
                    txtChatInput.Text = txtChatInput.Text.Insert(caret, Environment.NewLine);
                    txtChatInput.CaretIndex = caret + Environment.NewLine.Length;
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                    BtnSendMessage_Click(sender, e);
                }
            }
        }

        private void TxtChatInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool hasText = !string.IsNullOrWhiteSpace(txtChatInput.Text);
            txtInputPlaceholder.Visibility = hasText ? Visibility.Collapsed : Visibility.Visible;
            btnClearInput.Visibility = hasText ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnClearInput_Click(object sender, RoutedEventArgs e)
        {
            txtChatInput.Text = string.Empty;
            txtChatInput.Focus();
        }

        private void BtnVoiceInput_Click(object sender, RoutedEventArgs e)
        {
            ThongBaoDialog.ThongTin(
                "🎙️ MẸO LUYỆN GIAO TIẾP VỚI GIA SƯ AI:\n\n" +
                "1. Bạn có thể gõ các câu giao tiếp tiếng Anh (Ví dụ: 'Can I check in early?', 'How much is the fare?').\n" +
                "2. Sau khi AI phản hồi, bấm nút [🔊 Nghe phát âm] ở chân câu trả lời để nghe giọng đọc chuẩn bản xứ.\n" +
                "3. Để luyện nói ghi âm qua Microphone và chấm điểm phát âm tự động, bạn có thể truy cập phân hệ 'Luyện tập' hoặc 'Phòng luyện nói Speaking' trên thanh menu!",
                "Hướng dẫn luyện phát âm");
        }

        private void BtnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = txtChatInput.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            ExecuteSendMessage(message);
        }

        private void BtnRetry_Click(object sender, RoutedEventArgs e)
        {
            pnlErrorMessage.Visibility = Visibility.Collapsed;
            if (!string.IsNullOrWhiteSpace(_lastFailedQuery))
            {
                ExecuteSendMessage(_lastFailedQuery);
            }
        }
        #endregion

        #region Core Chat Execution & AI Engine
        private async void ExecuteSendMessage(string userMsg)
        {
            if (string.IsNullOrWhiteSpace(userMsg)) return;

            // Đảm bảo có current session
            if (_currentSession == null)
            {
                BtnNewChat_Click(this, new RoutedEventArgs());
            }

            pnlWelcomeHero.Visibility = Visibility.Collapsed;
            pnlErrorMessage.Visibility = Visibility.Collapsed;
            _lastFailedQuery = userMsg;

            // 1. Thêm tin nhắn Người dùng vào UI & CSDL Memory
            var userMsgObj = new ChatMessage
            {
                IsUser = true,
                Content = userMsg,
                Timestamp = DateTime.Now
            };
            _currentSession!.Messages.Add(userMsgObj);
            _currentSession.UpdatedAt = DateTime.Now;

            // Đặt tên session nếu còn là "Cuộc hội thoại mới"
            if (_currentSession.Title == "Cuộc hội thoại mới" || _currentSession.Title.StartsWith("Cuộc hội thoại"))
            {
                _currentSession.Title = userMsg.Length > 28 ? userMsg.Substring(0, 28) + "..." : userMsg;
            }

            AddUserBubbleToUI(userMsg, userMsgObj.Timestamp);
            txtChatInput.Text = string.Empty;

            // Lưu tin nhắn người dùng vào CSDL theo UserId
            try
            {
                if (CurrentUserId > 0)
                {
                    if (_currentSession.DbId == 0)
                    {
                        _currentSession.DbId = HoiThoaiAIBUS.Instance.TaoPhienHoiThoai(CurrentUserId, _currentSession.Title, _currentSession.Category);
                    }
                    else
                    {
                        HoiThoaiAIBUS.Instance.CapNhatTieuDePhien(_currentSession.DbId, _currentSession.Title);
                    }

                    if (_currentSession.DbId > 0)
                    {
                        HoiThoaiAIBUS.Instance.LuuTinNhan(_currentSession.DbId, "NguoiDung", userMsg);
                    }
                }
            }
            catch (Exception dbEx)
            {
                System.Diagnostics.Debug.WriteLine($"[TroLyAIView] Lỗi lưu tin nhắn người dùng: {dbEx.Message}");
            }

            // 2. Bật Trạng thái Loading (Key requirement: Loading indicator)
            pnlLoadingIndicator.Visibility = Visibility.Visible;
            btnSendMessage.IsEnabled = false;
            _dotsTimer?.Start();
            ScrollChatToEnd();
            UpdateActiveModelBadge();

            try
            {
                // 3. Xử lý gọi AI Service với Provider và Model do Admin chỉ định
                string aiResponse = await GenerateAIEducationalResponseAsync(userMsg);

                // 4. Thêm tin nhắn AI vào UI & Session & Database
                var aiMsgObj = new ChatMessage
                {
                    IsUser = false,
                    Content = aiResponse,
                    Timestamp = DateTime.Now
                };
                _currentSession.Messages.Add(aiMsgObj);
                _currentSession.UpdatedAt = DateTime.Now;

                try
                {
                    if (_currentSession.DbId > 0)
                    {
                        HoiThoaiAIBUS.Instance.LuuTinNhan(_currentSession.DbId, "AI", aiResponse);
                    }
                }
                catch (Exception dbEx)
                {
                    System.Diagnostics.Debug.WriteLine($"[TroLyAIView] Lỗi lưu tin nhắn AI: {dbEx.Message}");
                }

                AddAIBubbleToUI(aiResponse, aiMsgObj.Timestamp);
            }
            catch (Exception ex)
            {
                // Key requirement: Xử lý lỗi khi AI API không phản hồi
                pnlErrorMessage.Visibility = Visibility.Visible;
                txtErrorDetail.Text = $"Chi tiết lỗi: {ex.Message}";
            }
            finally
            {
                _dotsTimer?.Stop();
                pnlLoadingIndicator.Visibility = Visibility.Collapsed;
                btnSendMessage.IsEnabled = true;
                RefreshHistoryList();
                ScrollChatToEnd();
            }
        }

        /// <summary>
        /// Tạo câu trả lời học tập thông minh, gọi AI Service thực tế với API Key từ apiminh.json và Model Admin cấu hình
        /// </summary>
        private async Task<string> GenerateAIEducationalResponseAsync(string query)
        {
            try
            {
                long sessionId = _currentSession?.DbId ?? 0;
                var aiResp = await HoiThoaiAIBUS.Instance.GuiCauHoiChoAIAsync(query, _currentLearningMode, sessionId);
                if (aiResp != null && aiResp.Success && !string.IsNullOrWhiteSpace(aiResp.Text))
                {
                    return aiResp.Text.Trim();
                }

                if (aiResp != null && !aiResp.Success && !string.IsNullOrWhiteSpace(aiResp.ErrorMessage))
                {
                    string qLowerCheck = query.ToLower();
                    bool hasGrammarKeyword = qLowerCheck.Contains("present perfect") || qLowerCheck.Contains("past simple") || 
                                             qLowerCheck.Contains("câu điều kiện") || qLowerCheck.Contains("environment") || 
                                             qLowerCheck.Contains("were") || qLowerCheck.Contains("collocation");
                    if (!hasGrammarKeyword)
                    {
                        throw new Exception(aiResp.ErrorMessage);
                    }
                }
            }
            catch (Exception ex) when (!ex.Message.Contains("Chi tiết lỗi"))
            {
                string qLowerCheck = query.ToLower();
                bool hasGrammarKeyword = qLowerCheck.Contains("present perfect") || qLowerCheck.Contains("past simple") || 
                                         qLowerCheck.Contains("câu điều kiện") || qLowerCheck.Contains("environment") || 
                                         qLowerCheck.Contains("were") || qLowerCheck.Contains("collocation");
                if (!hasGrammarKeyword)
                {
                    throw;
                }
            }

            // Chờ mô phỏng độ trễ tự nhiên nếu chạy offline
            await Task.Delay(500);

            string qLower = query.ToLower();

            // 1. NGỮ PHÁP: Present Perfect vs Past Simple
            if (qLower.Contains("present perfect") || qLower.Contains("past simple") || qLower.Contains("hiện tại hoàn thành") || qLower.Contains("quá khứ đơn"))
            {
                return "💡 PHÂN BIỆT NHANH HIỆN TẠI HOÀN THÀNH & QUÁ KHỨ ĐƠN:\n\n" +
                       "1. Quá khứ đơn (Past Simple):\n" +
                       "• Cấu trúc: S + V2/ed\n" +
                       "• Bản chất: Hành động xảy ra và ĐÃ KẾT THÚC HOÀN TOÀN trong quá khứ, có mốc thời gian xác định.\n" +
                       "• Dấu hiệu: yesterday, in 2020, 2 days ago, last week, when I was young.\n" +
                       "• Ví dụ: \"She graduated from university in 2022.\"\n\n" +
                       "2. Hiện tại hoàn thành (Present Perfect):\n" +
                       "• Cấu trúc: S + have / has + V3/ed\n" +
                       "• Bản chất: Diễn tả trải nghiệm (tính đến nay), hoặc hành động quá khứ để lại KẾT QUẢ ảnh hưởng đến hiện tại.\n" +
                       "• Dấu hiệu: since, for, already, yet, ever, never, so far, recently.\n" +
                       "• Ví dụ: \"She has graduated, so she is looking for a job now.\"\n\n" +
                       "🎯 Bí kíp bài thi: Hãy tìm mốc thời gian cụ thể trước! Có mốc xác định quá khứ ➜ Quá khứ đơn; Chưa rõ mốc hoặc có 'since/for' ➜ Hiện tại hoàn thành.";
            }

            // 2. NGỮ PHÁP: Câu điều kiện (Conditionals / Were vs Was)
            if (qLower.Contains("câu điều kiện") || qLower.Contains("conditional") || qLower.Contains("were") || qLower.Contains("if i were"))
            {
                return "📝 GIẢI THÍCH QUY TẮC CÂU ĐIỀU KIỆN LOẠI 2 & CẤU TRÚC 'IF I WERE YOU':\n\n" +
                       "• Câu hỏi: Vì sao mệnh đề If loại 2 lại dùng 'WERE' thay vì 'was' cho các ngôi số ít (I, he, she, it)?\n\n" +
                       "• Bản chất ngữ pháp: Đây là thể GIẢ ĐỊNH CÁCH (Subjunctive Mood) trong tiếng Anh chuẩn học thuật. Thể giả định được dùng để diễn tả một điều KHÔNG CÓ THẬT hoặc TRÁI NGƯỢC với thực tế ở hiện tại.\n\n" +
                       "• Ví dụ kinh điển:\n" +
                       "  - \"If I were you, I would take that scholarship.\"\n" +
                       "  (Tôi không phải là bạn, điều này hoàn toàn trái ngược với thực tế).\n\n" +
                       "• Lưu ý bài thi: Trong văn nói thân mật, người bản xứ đôi khi nói 'If I was you', nhưng trong các kỳ thi chuẩn hóa (IELTS, TOEIC, THPT Quốc Gia), bạn BẮT BUỘC phải dùng 'WERE' để đạt điểm tuyệt đối!";
            }

            // 3. TỪ VỰNG: Collocations Environment / Technology
            if (qLower.Contains("environment") || qLower.Contains("môi trường") || qLower.Contains("collocation") || qLower.Contains("từ vựng"))
            {
                return "🌿 5 COLLOCATIONS HỌC THUẬT NÂNG BAND ĐIỂM CHỦ ĐỀ ENVIRONMENT (IELTS 7.0+):\n\n" +
                       "1. Combat climate change (v. phr): Đấu tranh chống biến đổi khí hậu.\n" +
                       "   ➜ Ví dụ: \"Global cooperation is imperative to combat climate change.\"\n\n" +
                       "2. Transition to renewable energy (v. phr): Chuyển đổi sang năng lượng tái tạo.\n" +
                       "   ➜ Ví dụ: \"Countries are accelerating their transition to renewable energy sources.\"\n\n" +
                       "3. Drastic reduction in emissions (n. phr): Sự cắt giảm khí thải quyết liệt.\n\n" +
                       "4. Ecological degradation (n. phr): Sự suy thoái hệ sinh thái.\n\n" +
                       "5. Sustainable development practices (n. phr): Các phương pháp phát triển bền vững.\n\n" +
                       "💡 Cách học: Hãy đặt 1 câu văn của riêng bạn kết hợp ít nhất 2 cụm từ trên và gõ vào đây để mình chấm sửa nhé!";
            }

            // 4. LUYỆN GIAO TIẾP: Roleplay / Speaking
            if (qLower.Contains("roleplay") || qLower.Contains("restaurant") || qLower.Contains("nhà hàng") || qLower.Contains("order") || qLower.Contains("phỏng vấn") || qLower.Contains("interview"))
            {
                return "🗣️ BÀI TẬP LUYỆN GIAO TIẾP PHẢN XẠ 2 CHIỀU:\n\n" +
                       "Tuyệt vời! Chúng ta cùng thực hành tình huống giao tiếp nhé.\n\n" +
                       "\"Hello! Thank you for coming in today. Could you briefly introduce yourself and tell me why you are interested in this position?\"\n\n" +
                       "👉 Khung câu trả lời mẫu gợi ý cho bạn:\n" +
                       "1. \"Thank you for having me today. My name is [Tên], and I have a strong background in...\"\n" +
                       "2. \"I am particularly drawn to this role because your organization values innovation and professional growth.\"\n\n" +
                       "Hãy gõ câu trả lời bằng tiếng Anh của bạn, mình sẽ sửa lỗi ngữ pháp và gợi ý cách diễn đạt tự nhiên hơn ngay!";
            }

            // 5. SỬA LỖI CÂU / CHỮA BÀI
            if (qLower.Contains("sửa") || qLower.Contains("check") || qLower.Contains("câu này đúng không") || qLower.Contains("lỗi"))
            {
                return "✍️ GIA SƯ AI SẴN SÀNG SỬA CÂU CHO BẠN:\n\n" +
                       "Bạn hãy gửi câu văn tiếng Anh bạn muốn kiểm tra vào khung chat (ví dụ: 'She don't like coffee because it make her nervous').\n\n" +
                       "Gia sư AI sẽ phân tích:\n" +
                       "1. Các lỗi ngữ pháp, chia thì và hòa hợp chủ vị (Subject-Verb Agreement).\n" +
                       "2. Cách sửa chuẩn xác (Corrected version).\n" +
                       "3. Đề xuất phiên bản tự nhiên chuẩn bản xứ (Native-like rephrasing).";
            }

            // MẶC ĐỊNH: Phản hồi song ngữ thân thiện
            return $"✨ Cảm ơn bạn đã hỏi về: \"{query}\"!\n\n" +
                   "Gia sư TiếngAnhAI đã ghi nhận câu hỏi của bạn. Để giúp bạn học hiệu quả nhất, mình gợi ý:\n\n" +
                   "• Nếu cần giải thích NGỮ PHÁP: Bạn có thể hỏi thêm công thức, cách dùng và bài tập áp dụng.\n" +
                   "• Nếu cần TỪ VỰNG: Mình có thể cung cấp nghĩa, từ loại, phiên âm IPA, từ đồng nghĩa và câu ví dụ.\n" +
                   "• Nếu muốn LUYỆN NÓI: Bạn hãy gõ câu trả lời bằng tiếng Anh, mình sẽ đóng vai người bản xứ để trò chuyện cùng bạn.\n\n" +
                   "Bạn muốn mình đào sâu hơn về phần nào trước?";
        }
        #endregion

        #region Bubble Rendering (UI Helpers)
        private void AddUserBubbleToUI(string message, DateTime timestamp)
        {
            var userContainer = new Grid
            {
                Margin = new Thickness(80, 4, 0, 14),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            userContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            userContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

            // Bubble Card
            var bubbleBorder = new Border
            {
                CornerRadius = new CornerRadius(16, 16, 4, 16),
                Padding = new Thickness(16, 12, 16, 12),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 660
            };

            var grad = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1)
            };
            grad.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#0F766E"), 0));
            grad.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#0D9488"), 1));
            bubbleBorder.Background = grad;

            var stack = new StackPanel();
            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 13.5,
                LineHeight = 22,
                TextWrapping = TextWrapping.Wrap
            };
            var timeBlock = new TextBlock
            {
                Text = timestamp.ToString("HH:mm") + " • Đã gửi",
                FontSize = 10.5,
                Foreground = new SolidColorBrush(Color.FromRgb(204, 251, 241)), // #CCFBF1
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 4, 0, 0)
            };
            stack.Children.Add(textBlock);
            stack.Children.Add(timeBlock);
            bubbleBorder.Child = stack;
            Grid.SetColumn(bubbleBorder, 0);

            // User Avatar
            var avatarBorder = new Border
            {
                Width = 32,
                Height = 32,
                CornerRadius = new CornerRadius(16),
                Background = (SolidColorBrush)FindResource("BrandTeal"),
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(8, 0, 0, 0)
            };
            var avatarText = new TextBlock
            {
                Text = SessionHelper.CurrentUser?.AvatarInitials ?? "SV",
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                FontSize = 11.5,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            avatarBorder.Child = avatarText;
            Grid.SetColumn(avatarBorder, 1);

            userContainer.Children.Add(bubbleBorder);
            userContainer.Children.Add(avatarBorder);
            pnlChatMessages.Children.Add(userContainer);
        }

        private void AddAIBubbleToUI(string message, DateTime timestamp, bool isError = false)
        {
            var botContainer = new Grid
            {
                Margin = new Thickness(0, 4, 60, 16),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            botContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(42) });
            botContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // AI Avatar
            var avatarBorder = new Border
            {
                Width = 36,
                Height = 36,
                CornerRadius = new CornerRadius(18),
                Background = (SolidColorBrush)FindResource("BrandTealLight"),
                BorderBrush = (SolidColorBrush)FindResource("BrandTealBorder"),
                BorderThickness = new Thickness(1),
                VerticalAlignment = VerticalAlignment.Top
            };
            var botIcon = new LucideIcon
            {
                Kind = LucideIconKind.Bot,
                Size = 19,
                Foreground = (SolidColorBrush)FindResource("BrandTeal"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            avatarBorder.Child = botIcon;
            Grid.SetColumn(avatarBorder, 0);

            // AI Content Card
            var contentBorder = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(4, 16, 16, 16),
                BorderBrush = (SolidColorBrush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(18, 14, 18, 14),
                Margin = new Thickness(10, 0, 0, 0),
                MaxWidth = 720
            };
            contentBorder.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                BlurRadius = 10,
                ShadowDepth = 2,
                Opacity = 0.04,
                Color = Colors.Black
            };

            var mainStack = new StackPanel();

            // Header info row
            var headerGrid = new Grid { Margin = new Thickness(0, 0, 0, 8) };
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var titleStack = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };
            var botName = new TextBlock
            {
                Text = "Gia sư TiếngAnhAI",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = (SolidColorBrush)FindResource("BrandTeal"),
                Margin = new Thickness(0, 0, 8, 0)
            };
            var badge = new Border
            {
                Background = (SolidColorBrush)FindResource("BrandMintSoft"),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2)
            };
            badge.Child = new TextBlock
            {
                Text = "Chính xác • Dễ hiểu",
                FontSize = 10.5,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61))
            };
            titleStack.Children.Add(botName);
            titleStack.Children.Add(badge);
            Grid.SetColumn(titleStack, 0);

            var timeText = new TextBlock
            {
                Text = timestamp.ToString("HH:mm"),
                FontSize = 11,
                Foreground = (SolidColorBrush)FindResource("TextGray"),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(timeText, 1);

            headerGrid.Children.Add(titleStack);
            headerGrid.Children.Add(timeText);
            mainStack.Children.Add(headerGrid);

            // Message text
            var replyText = new TextBlock
            {
                FontSize = 13.5,
                LineHeight = 22,
                TextWrapping = TextWrapping.Wrap,
                Foreground = (SolidColorBrush)FindResource("TextDark")
            };
            FormatMarkdownToTextBlock(replyText, message);
            mainStack.Children.Add(replyText);

            // Divider
            var divider = new Separator
            {
                Background = (SolidColorBrush)FindResource("BorderLight"),
                Margin = new Thickness(0, 10, 0, 8)
            };
            mainStack.Children.Add(divider);

            // Footer Action Toolbar: [Sao chép] & [Nghe phát âm TTS]
            var actionRow = new StackPanel { Orientation = Orientation.Horizontal };

            // Nút Sao chép
            var btnCopy = new Button
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                Padding = new Thickness(4, 2, 8, 2),
                Margin = new Thickness(0, 0, 12, 0)
            };
            var copyStack = new StackPanel { Orientation = Orientation.Horizontal };
            copyStack.Children.Add(new LucideIcon { Kind = LucideIconKind.Copy, Size = 13, Foreground = (SolidColorBrush)FindResource("TextGray"), Margin = new Thickness(0, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center });
            var txtCopyLabel = new TextBlock { Text = "Sao chép", FontSize = 11.5, Foreground = (SolidColorBrush)FindResource("TextGray"), VerticalAlignment = VerticalAlignment.Center };
            copyStack.Children.Add(txtCopyLabel);
            btnCopy.Content = copyStack;

            btnCopy.Click += (s, e) =>
            {
                try
                {
                    Clipboard.SetText(message);
                    txtCopyLabel.Text = "Đã chép! ✓";
                    txtCopyLabel.Foreground = (SolidColorBrush)FindResource("BrandTeal");
                }
                catch { }
            };
            actionRow.Children.Add(btnCopy);

            // Nút Nghe phát âm TTS
            var btnSpeak = new Button
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                Padding = new Thickness(4, 2, 8, 2)
            };
            var speakStack = new StackPanel { Orientation = Orientation.Horizontal };
            speakStack.Children.Add(new LucideIcon { Kind = LucideIconKind.Volume2, Size = 13, Foreground = (SolidColorBrush)FindResource("BrandTeal"), Margin = new Thickness(0, 0, 4, 0), VerticalAlignment = VerticalAlignment.Center });
            var txtSpeakLabel = new TextBlock { Text = "Nghe phát âm", FontSize = 11.5, Foreground = (SolidColorBrush)FindResource("BrandTeal"), VerticalAlignment = VerticalAlignment.Center };
            speakStack.Children.Add(txtSpeakLabel);
            btnSpeak.Content = speakStack;

            btnSpeak.Click += (s, e) =>
            {
                SpeakEnglishText(message);
            };
            actionRow.Children.Add(btnSpeak);

            mainStack.Children.Add(actionRow);
            contentBorder.Child = mainStack;
            Grid.SetColumn(contentBorder, 1);

            botContainer.Children.Add(avatarBorder);
            botContainer.Children.Add(contentBorder);
            pnlChatMessages.Children.Add(botContainer);
        }

        private void SpeakEnglishText(string fullText)
        {
            if (_speechSynthesizer == null)
            {
                ThongBaoDialog.ThongTin("Tính năng Text-to-Speech không khả dụng trên thiết bị này.", "Thông báo");
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    // Trích xuất các câu tiếng Anh trong dấu ngoặc kép hoặc câu dài
                    var englishMatches = Regex.Matches(fullText, @"\""([^\""]+)\""");
                    string toSpeak = "";
                    if (englishMatches.Count > 0)
                    {
                        foreach (Match m in englishMatches)
                        {
                            toSpeak += m.Groups[1].Value + ". ";
                        }
                    }
                    else
                    {
                        toSpeak = fullText.Length > 200 ? fullText.Substring(0, 200) : fullText;
                    }

                    _speechSynthesizer.SpeakAsyncCancelAll();
                    _speechSynthesizer.SpeakAsync(toSpeak);
                }
                catch { }
            });
        }

        private void ScrollChatToEnd()
        {
            chatScrollViewer.Dispatcher.InvokeAsync(() =>
            {
                chatScrollViewer.ScrollToEnd();
            }, DispatcherPriority.Loaded);
        }
        #endregion

        #region Helpers
        private string FormatSessionTime(DateTime time)
        {
            var diff = DateTime.Now - time;
            if (diff.TotalMinutes < 60) return $"{Math.Max(1, (int)diff.TotalMinutes)} phút trước";
            if (diff.TotalHours < 24) return time.ToString("HH:mm");
            if (diff.TotalDays < 2) return "Hôm qua";
            return time.ToString("dd/MM/yyyy");
        }

        private SolidColorBrush GetCategoryBgBrush(string cat) => cat switch
        {
            "Grammar" => new SolidColorBrush(Color.FromRgb(254, 243, 199)), // #FEF3C7
            "Vocab" => new SolidColorBrush(Color.FromRgb(219, 234, 254)),   // #DBEAFE
            "Explain" => new SolidColorBrush(Color.FromRgb(220, 252, 231)), // #DCFCE7
            "Speaking" => new SolidColorBrush(Color.FromRgb(243, 232, 255)),// #F3E8FF
            _ => (SolidColorBrush)FindResource("BrandTealLight")
        };

        private SolidColorBrush GetCategoryFgBrush(string cat) => cat switch
        {
            "Grammar" => new SolidColorBrush(Color.FromRgb(217, 119, 6)),  // #D97706
            "Vocab" => new SolidColorBrush(Color.FromRgb(37, 99, 235)),    // #2563EB
            "Explain" => new SolidColorBrush(Color.FromRgb(21, 128, 61)),  // #15803D
            "Speaking" => new SolidColorBrush(Color.FromRgb(126, 34, 206)),// #7E22CE
            _ => (SolidColorBrush)FindResource("BrandTeal")
        };

        private LucideIconKind GetCategoryLucideKind(string cat) => cat switch
        {
            "Grammar" => LucideIconKind.Lightbulb,
            "Vocab" => LucideIconKind.BookOpen,
            "Explain" => LucideIconKind.Target,
            "Speaking" => LucideIconKind.MessageSquare,
            _ => LucideIconKind.Sparkles
        };

        /// <summary>
        /// Định dạng Markdown thông minh cho câu trả lời của AI: hỗ trợ danh sách gạch đầu dòng, in đậm (**text**), in nghiêng (*text*), code (`text`)
        /// </summary>
        private void FormatMarkdownToTextBlock(TextBlock tb, string rawText)
        {
            tb.Inlines.Clear();
            if (string.IsNullOrEmpty(rawText)) return;

            string[] lines = rawText.Replace("\r\n", "\n").Split('\n');
            SolidColorBrush tealBrush = (SolidColorBrush)FindResource("BrandTeal");
            SolidColorBrush darkBrush = (SolidColorBrush)FindResource("TextDark");

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrWhiteSpace(line))
                {
                    if (i > 0 && i < lines.Length - 1)
                    {
                        tb.Inlines.Add(new LineBreak());
                    }
                    continue;
                }

                string trimmedStart = line.TrimStart();
                int indentLevel = line.Length - trimmedStart.Length;

                if (trimmedStart.StartsWith("* ") || trimmedStart.StartsWith("- ") || trimmedStart.StartsWith("• "))
                {
                    string bullet = indentLevel >= 2 ? "      ◦  " : "  •  ";
                    tb.Inlines.Add(new Run(bullet) { FontWeight = FontWeights.Bold, Foreground = tealBrush });
                    line = trimmedStart.Substring(2).TrimStart();
                }
                else if (Regex.IsMatch(trimmedStart, @"^\d+[\.\)]\s"))
                {
                    var match = Regex.Match(trimmedStart, @"^(\d+[\.\)])\s");
                    string num = "  " + match.Groups[1].Value + "  ";
                    tb.Inlines.Add(new Run(num) { FontWeight = FontWeights.Bold, Foreground = tealBrush });
                    line = trimmedStart.Substring(match.Length);
                }
                else if (trimmedStart.StartsWith("### "))
                {
                    line = trimmedStart.Substring(4);
                    tb.Inlines.Add(new Bold(new Run(line)) { FontSize = 14, Foreground = tealBrush });
                    if (i < lines.Length - 1) tb.Inlines.Add(new LineBreak());
                    continue;
                }
                else if (trimmedStart.StartsWith("## "))
                {
                    line = trimmedStart.Substring(3);
                    tb.Inlines.Add(new Bold(new Run(line)) { FontSize = 15, Foreground = tealBrush });
                    if (i < lines.Length - 1) tb.Inlines.Add(new LineBreak());
                    continue;
                }
                else if (trimmedStart.StartsWith("# "))
                {
                    line = trimmedStart.Substring(2);
                    tb.Inlines.Add(new Bold(new Run(line)) { FontSize = 16, Foreground = tealBrush });
                    if (i < lines.Length - 1) tb.Inlines.Add(new LineBreak());
                    continue;
                }

                ParseInlineMarkdown(tb, line, tealBrush, darkBrush);

                if (i < lines.Length - 1)
                {
                    tb.Inlines.Add(new LineBreak());
                }
            }
        }

        private void ParseInlineMarkdown(TextBlock tb, string text, SolidColorBrush accentBrush, SolidColorBrush normalBrush)
        {
            if (string.IsNullOrEmpty(text)) return;

            var regex = new Regex(@"((\*\*[^\*]+?\*\*)|(\*[^\*]+?\*)|(`[^`]+?`)|(_[^_]+?_))");
            int lastIdx = 0;

            foreach (Match m in regex.Matches(text))
            {
                if (m.Index > lastIdx)
                {
                    string plain = text.Substring(lastIdx, m.Index - lastIdx);
                    tb.Inlines.Add(new Run(plain) { Foreground = normalBrush });
                }

                if (m.Value.StartsWith("**") && m.Value.EndsWith("**") && m.Value.Length >= 4)
                {
                    string boldText = m.Value.Substring(2, m.Value.Length - 4);
                    tb.Inlines.Add(new Bold(new Run(boldText)) { Foreground = normalBrush });
                }
                else if (m.Value.StartsWith("*") && m.Value.EndsWith("*") && m.Value.Length >= 2)
                {
                    string italicText = m.Value.Substring(1, m.Value.Length - 2);
                    tb.Inlines.Add(new Italic(new Run(italicText)) { Foreground = accentBrush });
                }
                else if (m.Value.StartsWith("_") && m.Value.EndsWith("_") && m.Value.Length >= 2)
                {
                    string italicText = m.Value.Substring(1, m.Value.Length - 2);
                    tb.Inlines.Add(new Italic(new Run(italicText)) { Foreground = accentBrush });
                }
                else if (m.Value.StartsWith("`") && m.Value.EndsWith("`") && m.Value.Length >= 2)
                {
                    string codeText = m.Value.Substring(1, m.Value.Length - 2);
                    tb.Inlines.Add(new Run(codeText) { FontWeight = FontWeights.SemiBold, Foreground = accentBrush, FontFamily = new FontFamily("Consolas") });
                }

                lastIdx = m.Index + m.Length;
            }

            if (lastIdx < text.Length)
            {
                string remaining = text.Substring(lastIdx);
                tb.Inlines.Add(new Run(remaining) { Foreground = normalBrush });
            }
        }
        #endregion
    }
}
