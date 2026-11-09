using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using BUS;
using DTO;

namespace GUI.Views
{
    public partial class TrangChuView : Page
    {
        public TrangChuView()
        {
            InitializeComponent();
            LoadUserData();
            this.Loaded += async (s, e) => await LoadStatsAsync();
        }

        private void LoadUserData()
        {
            if (SessionHelper.CurrentUser != null)
            {
                txtWelcomeUser.Text = SessionHelper.CurrentUser.HoTen;
            }

            if (SessionHelper.CurrentHoSo != null)
            {
                txtCurrentLevel.Text = !string.IsNullOrEmpty(SessionHelper.CurrentHoSo.HienThiTrinhDoDauVao) 
                    ? SessionHelper.CurrentHoSo.HienThiTrinhDoDauVao 
                    : "B1 Intermediate";
                txtTargetLevel.Text = SessionHelper.CurrentHoSo.HienThiMucTieu;
            }
            else
            {
                txtCurrentLevel.Text = "B1 Intermediate";
                txtTargetLevel.Text = "IELTS 6.5 (CEFR B2)";
            }
            bool daHoanThanhKhaoSat = (SessionHelper.CurrentUser != null && !SessionHelper.CurrentUser.LaLanDauDangNhap);

            if (daHoanThanhKhaoSat)
            {
                // Thành viên đã đăng ký / đã làm khảo sát: Không ép buộc làm khảo sát nữa
                txtBannerEmoji.Text = "✅";
                iconBannerBadge.Background = (Brush)new BrushConverter().ConvertFromString("#E0F2FE")!;
                bannerAssessment.Background = (Brush)new BrushConverter().ConvertFromString("#F0F9FF")!;
                bannerAssessment.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#BAE6FD")!;
                txtBannerTitle.Text = "Lộ trình năng lực cá nhân hóa của bạn";
                txtBannerTitle.Foreground = (Brush)new BrushConverter().ConvertFromString("#0369A1")!;
                string currentLevelDisplay = SessionHelper.CurrentHoSo?.HienThiTrinhDoDauVao ?? SessionHelper.CurrentHoSo?.HienThiMucTieu ?? "CEFR B1";
                txtBannerDesc.Text = $"Bạn đã hoàn thành đánh giá năng lực ({currentLevelDisplay}). Các bài học và đề xuất đã được cá nhân hóa sẵn sàng cho bạn!";
                txtBannerDesc.Foreground = (Brush)new BrushConverter().ConvertFromString("#0284C7")!;
                btnAssessmentAction.Background = (Brush)new BrushConverter().ConvertFromString("#0284C7")!;
                txtBannerBtnText.Text = "Kiểm tra lại trình độ 🔄";
            }
            else
            {
                // Người dùng chưa làm khảo sát
                txtBannerEmoji.Text = "🎯";
                iconBannerBadge.Background = (Brush)new BrushConverter().ConvertFromString("#DCFCE7")!;
                bannerAssessment.Background = (Brush)new BrushConverter().ConvertFromString("#F0FDF4")!;
                bannerAssessment.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#BBF7D0")!;
                txtBannerTitle.Text = "Khảo sát Đánh giá Năng lực Đầu vào (Placement Test)";
                txtBannerTitle.Foreground = (Brush)new BrushConverter().ConvertFromString("#166534")!;
                txtBannerDesc.Text = "Hãy chọn mức độ hiện tại và làm bài khảo sát ngắn để AI xác định cấp độ và đề xuất bài học tối ưu cho bạn.";
                txtBannerDesc.Foreground = (Brush)new BrushConverter().ConvertFromString("#15803D")!;
                btnAssessmentAction.Background = (Brush)new BrushConverter().ConvertFromString("#16A34A")!;
                txtBannerBtnText.Text = "Làm bài kiểm tra ngay ➔";
            }
        }

        private async System.Threading.Tasks.Task LoadStatsAsync()
        {
            try
            {
                var stats = await System.Threading.Tasks.Task.Run(() => BaiHocBUS.LayThongKeSinhVien());
                txtDiemTrungBinh.Text = $"{stats.DiemTrungBinh:0.0}%";
                txtSoBaiHoanThanh.Text = stats.SoBaiDaHoanThanh.ToString();
                txtTongSoBai.Text = $" / {Math.Max(stats.TongSoBaiHoc, 29)} bài";
                txtThoiGianTichLuy.Text = stats.SoPhutHocTichLuy.ToString();
                txtChuoiHoc.Text = $"Chuỗi {stats.ChuoiHocLienTiep} ngày chuyên cần";

                txtAccTuVung.Text = $"{stats.TyLeTuVung}% độ chính xác";
                txtAccNguPhap.Text = $"{stats.TyLeNguPhap}% độ chính xác";
                txtAccDocHieu.Text = $"{stats.TyLeDocHieu}% độ chính xác";
                txtAccNgheHieu.Text = $"{stats.TyLeNgheHieu}% độ chính xác";

                if (stats.DanhSachBaiHocGanDay != null && stats.DanhSachBaiHocGanDay.Count > 0)
                {
                    icRecentLessons.ItemsSource = stats.DanhSachBaiHocGanDay;
                    txtNoRecentLessons.Visibility = Visibility.Collapsed;
                }
                else
                {
                    txtNoRecentLessons.Visibility = Visibility.Visible;
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi nạp thống kê TrangChu: {ex.Message}");
            }
        }

        private void AssessmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DanhGiaNangLucView(forceNewSurvey: true));
        }

        private void BtnHocTap_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new HocTapView());
        }

        private void BtnThiThu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new ThiThuView());
        }

        private void BtnLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance != null)
                MainWindow.Instance.NavigateToCached<LuyenTapView>();
            else
                this.NavigationService?.Navigate(new LuyenTapView());
        }

        private void BtnLuyenTap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Instance != null)
                MainWindow.Instance.NavigateToCached<LuyenTapView>();
            else
                this.NavigationService?.Navigate(new LuyenTapView());
        }

        private void BtnThiThu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Instance != null)
                MainWindow.Instance.NavigateToCached<ThiThuView>();
            else
                this.NavigationService?.Navigate(new ThiThuView());
        }

        private void BtnBaiVietAI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Instance != null)
                MainWindow.Instance.NavigateToCached<BaiVietAIView>();
            else
                this.NavigationService?.Navigate(new BaiVietAIView());
        }

        private void BtnAIChat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Instance != null)
                MainWindow.Instance.NavigateToCached<TroLyAIView>();
            else
                this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void ItemRecentLesson_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.Tag is int maBaiHoc)
            {
                if (!BaiHocBUS.KiemTraQuyenTruyCap(maBaiHoc))
                {
                    GUI.Views.Common.ThongBaoDialog.CanhBao(
                        Window.GetWindow(this),
                        "Bài học này chưa được giảng viên cho phép truy cập hoặc đã bị thu hồi quyền.",
                        "Không có quyền truy cập"
                    );
                    return;
                }
                this.NavigationService?.Navigate(new HocTapView(maBaiHoc));
            }
        }

        private void AIChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance != null)
                MainWindow.Instance.NavigateToCached<TroLyAIView>();
            else
                this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
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

        #region Live Search Autocomplete & Suggestions
        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC).ToLowerInvariant();
        }

        private static bool MatchSearch(string source, string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return false;
            if (string.IsNullOrWhiteSpace(source)) return false;
            if (source.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return RemoveDiacritics(source).Contains(RemoveDiacritics(query));
        }

        private void BtnClearStudentSearch_Click(object sender, RoutedEventArgs e)
        {
            txtStudentSearch.Text = string.Empty;
            popStudentSuggestions.IsOpen = false;
        }

        private void TxtStudentSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                popStudentSuggestions.IsOpen = false;
                e.Handled = true;
            }
        }

        private async void TxtStudentSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = txtStudentSearch.Text.Trim();

            txtStudentSearchPlaceholder.Visibility = string.IsNullOrEmpty(query) ? Visibility.Visible : Visibility.Collapsed;
            btnClearStudentSearch.Visibility = string.IsNullOrEmpty(query) ? Visibility.Collapsed : Visibility.Visible;

            if (string.IsNullOrEmpty(query))
            {
                popStudentSuggestions.IsOpen = false;
                pnlStudentSuggestions.Children.Clear();
                return;
            }

            pnlStudentSuggestions.Children.Clear();

            // 1. Chức năng hệ thống
            var features = new (string Icon, string Title, string Subtitle, string Category, string BadgeBg, string BadgeFg, Action Action)[]
            {
                ("📚", "Kho Bài học & Lý thuyết", "Học ngữ pháp, từ vựng theo chủ đề", "Chức năng", "#DCFCE7", "#15803D", () => NavigateToHocTap()),
                ("🎯", "Trung tâm Luyện tập Kỹ năng", "Luyện Nghe, Nói, Đọc, Viết, Flashcard", "Chức năng", "#FEF3C7", "#B45309", () => NavigateToLuyenTap()),
                ("⏱️", "Phòng Thi thử Chuẩn hóa", "Luyện thi thử đề TOEIC & IELTS", "Chức năng", "#E0F2FE", "#0369A1", () => NavigateToThiThu()),
                ("✍️", "Luyện viết & Chấm bài AI", "Chấm chữa bài viết luận tiếng Anh tự động", "Chức năng", "#F3E8FF", "#7E22CE", () => NavigateToBaiVietAI()),
                ("🤖", "Gia sư Trợ lý AI", "Hỏi đáp ngữ pháp, tra từ vựng, giao tiếp song ngữ 24/7", "Chức năng", "#CCFBF1", "#0F766E", () => NavigateToTroLyAI()),
                ("📊", "Bảng Tiến độ & Phân tích Năng lực", "Xem biểu đồ học tập, streak, báo cáo", "Chức năng", "#F1F5F9", "#334155", () => NavigateToTienDo()),
                ("👤", "Hồ sơ Cá nhân & Mục tiêu", "Cài đặt thông tin tài khoản và mục tiêu", "Chức năng", "#FEE2E2", "#B91C1C", () => NavigateToHoSo()),
                ("🎯", "Đánh giá Năng lực Đầu vào", "Khảo sát và xếp lớp trình độ tiếng Anh", "Chức năng", "#F0FDFA", "#0D9488", () => NavigateToDanhGia())
            };

            int matchCount = 0;
            foreach (var f in features)
            {
                if (MatchSearch(f.Title, query) || MatchSearch(f.Subtitle, query) || MatchSearch(f.Category, query))
                {
                    pnlStudentSuggestions.Children.Add(CreateSuggestionItem(f.Icon, f.Title, f.Subtitle, f.Category, f.BadgeBg, f.BadgeFg, () =>
                    {
                        popStudentSuggestions.IsOpen = false;
                        txtStudentSearch.Text = string.Empty;
                        f.Action();
                    }));
                    matchCount++;
                }
            }

            // 2. Tra cứu Bài học thực tế trong CSDL
            try
            {
                var lessons = await System.Threading.Tasks.Task.Run(() => BaiHocBUS.LayDanhSachTuDatabase(search: query));
                if (lessons != null && lessons.Count > 0)
                {
                    foreach (var bai in lessons.Take(6))
                    {
                        string desc = !string.IsNullOrEmpty(bai.TomTat) ? bai.TomTat : $"Trình độ: {bai.MaTrinhDo} • {bai.TenChuDe}";
                        pnlStudentSuggestions.Children.Add(CreateSuggestionItem("📖", bai.TieuDe, desc, "Bài học", "#F0FDF4", "#15803D", () =>
                        {
                            popStudentSuggestions.IsOpen = false;
                            txtStudentSearch.Text = string.Empty;
                            if (!BaiHocBUS.KiemTraQuyenTruyCap(bai.MaBaiHoc))
                            {
                                Common.ThongBaoDialog.CanhBao(Window.GetWindow(this), "Bài học này chưa được mở quyền truy cập.", "Thông báo");
                                return;
                            }
                            this.NavigationService?.Navigate(new HocTapView(bai.MaBaiHoc));
                        }));
                        matchCount++;
                    }
                }
            }
            catch { }

            // 3. Nếu không tìm thấy
            if (matchCount == 0)
            {
                var emptyBorder = new Border
                {
                    Padding = new Thickness(14, 16, 14, 16),
                    Background = Brushes.Transparent
                };
                var spEmpty = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
                spEmpty.Children.Add(new TextBlock { Text = "🔍", FontSize = 22, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 0, 0, 6) });
                spEmpty.Children.Add(new TextBlock
                {
                    Text = $"Không tìm thấy kết quả nào cho \"{query}\"",
                    FontSize = 12.5,
                    FontWeight = FontWeights.Medium,
                    Foreground = (Brush)new BrushConverter().ConvertFromString("#64748B")!,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                spEmpty.Children.Add(new TextBlock
                {
                    Text = "Thử tìm kiếm với từ khóa khác (ví dụ: ngữ pháp, từ vựng, thi thử...)",
                    FontSize = 11,
                    Foreground = (Brush)new BrushConverter().ConvertFromString("#94A3B8")!,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 4, 0, 0)
                });
                emptyBorder.Child = spEmpty;
                pnlStudentSuggestions.Children.Add(emptyBorder);
            }

            popStudentSuggestions.IsOpen = true;
        }

        private UIElement CreateSuggestionItem(string iconText, string title, string subtitle, string category, string badgeBg, string badgeFg, Action onClick)
        {
            var border = new Border
            {
                Background = Brushes.Transparent,
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10, 8, 10, 8),
                Margin = new Thickness(0, 0, 0, 3),
                Cursor = Cursors.Hand
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var tbIcon = new TextBlock
            {
                Text = iconText,
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Grid.SetColumn(tbIcon, 0);

            var spText = new StackPanel { VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4, 0, 8, 0) };
            var tbTitle = new TextBlock
            {
                Text = title,
                FontWeight = FontWeights.SemiBold,
                FontSize = 12.5,
                Foreground = (Brush)new BrushConverter().ConvertFromString("#0F172A")!
            };
            var tbSub = new TextBlock
            {
                Text = subtitle,
                FontSize = 11,
                Foreground = (Brush)new BrushConverter().ConvertFromString("#64748B")!,
                Margin = new Thickness(0, 2, 0, 0),
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            spText.Children.Add(tbTitle);
            spText.Children.Add(tbSub);
            Grid.SetColumn(spText, 1);

            var badgeBorder = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFromString(badgeBg)!,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(7, 2, 7, 2),
                VerticalAlignment = VerticalAlignment.Center
            };
            var tbBadge = new TextBlock
            {
                Text = category,
                FontSize = 10.5,
                FontWeight = FontWeights.Bold,
                Foreground = (Brush)new BrushConverter().ConvertFromString(badgeFg)!
            };
            badgeBorder.Child = tbBadge;
            Grid.SetColumn(badgeBorder, 2);

            grid.Children.Add(tbIcon);
            grid.Children.Add(spText);
            grid.Children.Add(badgeBorder);
            border.Child = grid;

            border.MouseEnter += (s, e) => border.Background = (Brush)new BrushConverter().ConvertFromString("#F1F5F9")!;
            border.MouseLeave += (s, e) => border.Background = Brushes.Transparent;
            border.MouseLeftButtonUp += (s, e) =>
            {
                e.Handled = true;
                onClick();
            };

            return border;
        }

        private void NavigateToHocTap()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<HocTapView>();
            else this.NavigationService?.Navigate(new HocTapView());
        }

        private void NavigateToLuyenTap()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<LuyenTapView>();
            else this.NavigationService?.Navigate(new LuyenTapView());
        }

        private void NavigateToThiThu()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<ThiThuView>();
            else this.NavigationService?.Navigate(new ThiThuView());
        }

        private void NavigateToBaiVietAI()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<BaiVietAIView>();
            else this.NavigationService?.Navigate(new BaiVietAIView());
        }

        private void NavigateToTroLyAI()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<TroLyAIView>();
            else this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void NavigateToTienDo()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<TienDoHocView>();
            else this.NavigationService?.Navigate(new TienDoHocView());
        }

        private void NavigateToHoSo()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<HoSoCaNhanView>();
            else this.NavigationService?.Navigate(new HoSoCaNhanView());
        }

        private void NavigateToDanhGia()
        {
            if (MainWindow.Instance != null) MainWindow.Instance.NavigateToCached<DanhGiaNangLucView>();
            else this.NavigationService?.Navigate(new DanhGiaNangLucView());
        }
        #endregion
    }
}
