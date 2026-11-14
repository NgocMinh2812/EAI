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
using GUI.ViewModels.GiangVien;

namespace GUI.Views.GiangVien
{
    public partial class GiangVienWindow : Window
    {
        private bool _isSidebarCollapsed = false;
        private FrameworkElement[] _navIcons = Array.Empty<FrameworkElement>();
        private TextBlock[] _navLabels = Array.Empty<TextBlock>();
        private StackPanel[] _navPanels = Array.Empty<StackPanel>();

        public static GiangVienWindow? Instance { get; private set; }
        private readonly System.Collections.Generic.Dictionary<Type, Page> _pageCache = new();

        public Page GetOrCreatePage<T>() where T : Page, new()
        {
            var type = typeof(T);
            if (!_pageCache.TryGetValue(type, out var page))
            {
                page = new T();
                _pageCache[type] = page;
            }
            return page;
        }

        public void NavigateToCached<T>() where T : Page, new()
        {
            var page = GetOrCreatePage<T>();
            if (AdminMainFrame.Content != page)
            {
                AdminMainFrame.Navigate(page);
            }
        }

        public GiangVienWindow()
        {
            InitializeComponent();
            Instance = this;
            InitSidebarElements();
            ApplyRolePermissions();
            AdminMainFrame.Navigating += AdminMainFrame_Navigating;
            AdminMainFrame.Navigated += AdminMainFrame_Navigated;
            Closing += GiangVienWindow_Closing;
            PreviewKeyDown += GiangVienWindow_PreviewKeyDown;

            popNotifications.PlacementTarget = btnNotificationBell;
            popNotifications.Placement = System.Windows.Controls.Primitives.PlacementMode.Custom;
            popNotifications.CustomPopupPlacementCallback = (popupSize, targetSize, offset) =>
            {
                // Căn lề phải popup khớp chính xác với lề phải nút Chuông thông báo (bù trừ margin đổ bóng 12px)
                double x = targetSize.Width - popupSize.Width + 12;
                double y = targetSize.Height - 8;
                return new[] { new System.Windows.Controls.Primitives.CustomPopupPlacement(new Point(x, y), System.Windows.Controls.Primitives.PopupPrimaryAxis.Horizontal) };
            };

            popAdminSuggestions.PlacementTarget = borderAdminSearch;
            popAdminSuggestions.Placement = System.Windows.Controls.Primitives.PlacementMode.Custom;
            popAdminSuggestions.CustomPopupPlacementCallback = (popupSize, targetSize, offset) =>
            {
                // Căn lề trái popup khớp chính xác với lề trái thanh tìm kiếm (bù trừ margin đổ bóng 12px)
                double x = -12;
                double y = targetSize.Height - 8;
                return new[] { new System.Windows.Controls.Primitives.CustomPopupPlacement(new Point(x, y), System.Windows.Controls.Primitives.PopupPrimaryAxis.Horizontal) };
            };

            // Đăng ký sự kiện đóng sidebar Trợ lý AI
            aiAssistantSidebar.CloseRequested += DongAIAssistantSidebar;

            // Mở mặc định trang Tổng quan sử dụng page cache để bảo toàn tiến trình
            AdminMainFrame.Navigate(GetOrCreatePage<TongQuanView>());
            Closed += (s, e) => _pageCache.Clear();
        }

        private void InitSidebarElements()
        {
            _navIcons = new FrameworkElement[] {
                icoNavTongQuan, icoNavNguoiDung, icoNavDanhMuc, icoNavNoiDung,
                icoNavNganHang, icoNavDeThi, icoNavKhaoSat, icoNavThongKeHocTap, icoNavAISinhDe,
                icoNavGiamSat, icoNavToStudentHome
            };
            _navLabels = new TextBlock[] {
                lblNavTongQuan, lblNavNguoiDung, lblNavDanhMuc, lblNavNoiDung,
                lblNavNganHang, lblNavDeThi, lblNavKhaoSat, lblNavThongKeHocTap, lblNavAISinhDe,
                lblNavGiamSat, lblNavToStudentHome
            };
            _navPanels = new StackPanel[] {
                spNavTongQuan, spNavNguoiDung, spNavDanhMuc, spNavNoiDung,
                spNavNganHang, spNavDeThi, spNavKhaoSat, spNavThongKeHocTap, spNavAISinhDe,
                spNavGiamSat, spNavToStudentHome
            };
        }

        private void BtnToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            SetSidebarCollapsed(!_isSidebarCollapsed);
        }

        private void SetSidebarCollapsed(bool collapsed)
        {
            _isSidebarCollapsed = collapsed;
            var user = SessionHelper.CurrentUser;
            bool isAdmin = user != null && string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);

            if (collapsed)
            {
                // Thu gọn về 68px
                colSidebar.Width = new GridLength(68);
                colSidebar.MinWidth = 68;
                colSidebar.MaxWidth = 68;

                // Header
                pnlBrandFull.Visibility = Visibility.Collapsed;
                btnToggleSidebarMini.Visibility = Visibility.Visible;

                // Section Headers
                headerChinh.Visibility = Visibility.Collapsed;
                headerQuanLy.Visibility = Visibility.Collapsed;
                headerTriTueAI.Visibility = Visibility.Collapsed;
                headerHeThong.Visibility = Visibility.Collapsed;
                headerChuyenDoi.Visibility = Visibility.Collapsed;

                // Separators phân cách giữa các nhóm
                sep1.Visibility = Visibility.Visible;
                sep2.Visibility = Visibility.Visible;
                sep3.Visibility = Visibility.Visible;
                sep4.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                sep5.Visibility = Visibility.Visible;

                // Ẩn chữ nhãn, căn giữa icon
                if (_navLabels != null)
                {
                    foreach (var lbl in _navLabels)
                        lbl.Visibility = Visibility.Collapsed;
                }
                if (_navIcons != null)
                {
                    foreach (var ico in _navIcons)
                        ico.Margin = new Thickness(0);
                }
                if (_navPanels != null)
                {
                    foreach (var pnl in _navPanels)
                        pnl.HorizontalAlignment = HorizontalAlignment.Center;
                }
                badgeNavAISinhDe.Visibility = Visibility.Collapsed;

                // Footer User Profile
                pnlUserFull.Visibility = Visibility.Collapsed;
                pnlUserCollapsed.Visibility = Visibility.Visible;
            }
            else
            {
                // Mở rộng về 240px
                colSidebar.Width = new GridLength(240);
                colSidebar.MinWidth = 200;
                colSidebar.MaxWidth = 280;

                // Header
                pnlBrandFull.Visibility = Visibility.Visible;
                btnToggleSidebarMini.Visibility = Visibility.Collapsed;

                // Section Headers
                headerChinh.Visibility = Visibility.Visible;
                headerQuanLy.Visibility = Visibility.Visible;
                headerTriTueAI.Visibility = Visibility.Visible;
                headerHeThong.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                headerChuyenDoi.Visibility = Visibility.Visible;

                // Separators
                sep1.Visibility = Visibility.Collapsed;
                sep2.Visibility = Visibility.Collapsed;
                sep3.Visibility = Visibility.Collapsed;
                sep4.Visibility = Visibility.Collapsed;
                sep5.Visibility = Visibility.Collapsed;

                // Hiện lại chữ nhãn, căn trái icon
                if (_navLabels != null)
                {
                    foreach (var lbl in _navLabels)
                        lbl.Visibility = Visibility.Visible;
                }
                if (_navIcons != null)
                {
                    foreach (var ico in _navIcons)
                        ico.Margin = new Thickness(0, 0, 10, 0);
                }
                if (_navPanels != null)
                {
                    foreach (var pnl in _navPanels)
                        pnl.HorizontalAlignment = HorizontalAlignment.Left;
                }
                badgeNavAISinhDe.Visibility = Visibility.Visible;

                // Footer User Profile
                pnlUserFull.Visibility = Visibility.Visible;
                pnlUserCollapsed.Visibility = Visibility.Collapsed;
            }
        }

        private void ApplyRolePermissions()
        {
            var user = SessionHelper.CurrentUser;
            bool isAdmin = user != null && string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);

            if (!isAdmin)
            {
                // Giảng viên -> Ẩn mục Giám sát hệ thống
                headerHeThong.Visibility = Visibility.Collapsed;
                btnNavGiamSat.Visibility = Visibility.Collapsed;
            }

            if (user != null)
            {
                txtCurrentUserName.Text = user.HoTen;
                txtCurrentUserRole.Text = user.RoleText;
                txtAvatarInitials.Text = user.Initials;
                txtAvatarInitialsMini.Text = user.Initials;
                pnlUserCollapsedAvatar.ToolTip = $"{user.HoTen} ({user.RoleText})";
                txtTopAvatar.Text = user.Initials;
                txtBrandSubtitle.Text = isAdmin ? "Admin Dashboard" : "Cổng Giảng Viên";
            }
        }

        private int _modalOverlayCount = 0;

        public void ShowModalOverlay()
        {
            _modalOverlayCount++;
            OverlayDimLayer.Visibility = Visibility.Visible;
            MainContentContainer.Effect = new System.Windows.Media.Effects.BlurEffect 
            { 
                Radius = 8, 
                RenderingBias = System.Windows.Media.Effects.RenderingBias.Performance 
            };
        }

        public void HideModalOverlay()
        {
            _modalOverlayCount--;
            if (_modalOverlayCount <= 0)
            {
                _modalOverlayCount = 0;
                OverlayDimLayer.Visibility = Visibility.Collapsed;
                MainContentContainer.Effect = null;
            }
        }

        private void AdminMainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            ResetNavButtons();

            if (e.Content is TongQuanView)
            {
                SetActiveButton(btnNavTongQuan);
                txtBreadcrumb.Text = "Tổng quan";
            }
            else if (e.Content is NguoiDungView)
            {
                SetActiveButton(btnNavNguoiDung);
                txtBreadcrumb.Text = "Người dùng";
            }
            else if (e.Content is DanhMucView)
            {
                SetActiveButton(btnNavDanhMuc);
                txtBreadcrumb.Text = "Danh mục & Cấu trúc";
            }
            else if (e.Content is NoiDungHocTapView)
            {
                SetActiveButton(btnNavNoiDung);
                txtBreadcrumb.Text = "Nội dung học tập";
            }
            else if (e.Content is NganHangCauHoiView)
            {
                SetActiveButton(btnNavNganHang);
                txtBreadcrumb.Text = "Ngân hàng câu hỏi";
            }
            else if (e.Content is DeThiLuyenTapView)
            {
                SetActiveButton(btnNavDeThi);
                txtBreadcrumb.Text = "Đề thi & Luyện tập";
            }
            else if (e.Content is KhaoSatDauVaoView)
            {
                SetActiveButton(btnNavKhaoSat);
                txtBreadcrumb.Text = "Khảo sát đầu vào";
            }
            else if (e.Content is ThongKeHocTapGiangVienView)
            {
                SetActiveButton(btnNavThongKeHocTap);
                txtBreadcrumb.Text = "Thống kê học tập";
            }
            else if (e.Content is AISinhDeView)
            {
                SetActiveButton(btnNavAISinhDe);
                txtBreadcrumb.Text = "Tạo câu hỏi bằng AI";
            }
            else if (e.Content is GiamSatHeThongView)
            {
                SetActiveButton(btnNavGiamSat);
                txtBreadcrumb.Text = "Giám sát hệ thống";
            }
            else if (e.Content is TroLyAIView)
            {
                txtBreadcrumb.Text = "Trợ lý AI";
            }

            // Đồng bộ ngữ cảnh trang đang xem cho Trợ lý AI
            aiAssistantSidebar?.UpdateCurrentPage(e.Content?.GetType().Name);
        }

        private void SetActiveButton(Button btn)
        {
            if (btn != null && TryFindResource("AdminActiveNavItemStyle") is Style activeStyle)
            {
                btn.Style = activeStyle;
            }
        }

        private void ResetNavButtons()
        {
            if (TryFindResource("AdminNavItemStyle") is Style normalStyle)
            {
                btnNavTongQuan.Style = normalStyle;
                btnNavNguoiDung.Style = normalStyle;
                btnNavDanhMuc.Style = normalStyle;
                btnNavNoiDung.Style = normalStyle;
                btnNavNganHang.Style = normalStyle;
                btnNavDeThi.Style = normalStyle;
                btnNavKhaoSat.Style = normalStyle;
                btnNavThongKeHocTap.Style = normalStyle;
                btnNavAISinhDe.Style = normalStyle;
                btnNavGiamSat.Style = normalStyle;
            }
        }

        private void NavTongQuan_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not TongQuanView)
                AdminMainFrame.Navigate(GetOrCreatePage<TongQuanView>());
        }

        private void NavNguoiDung_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not NguoiDungView)
                AdminMainFrame.Navigate(GetOrCreatePage<NguoiDungView>());
        }

        private void NavDanhMuc_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not DanhMucView)
                AdminMainFrame.Navigate(GetOrCreatePage<DanhMucView>());
        }

        private void NavNoiDung_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not NoiDungHocTapView)
                AdminMainFrame.Navigate(GetOrCreatePage<NoiDungHocTapView>());
        }

        private void NavNganHang_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not NganHangCauHoiView)
                AdminMainFrame.Navigate(GetOrCreatePage<NganHangCauHoiView>());
        }

        private void NavDeThi_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not DeThiLuyenTapView)
                AdminMainFrame.Navigate(GetOrCreatePage<DeThiLuyenTapView>());
        }

        private void NavKhaoSat_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not KhaoSatDauVaoView)
                AdminMainFrame.Navigate(GetOrCreatePage<KhaoSatDauVaoView>());
        }

        private void NavThongKeHocTap_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not ThongKeHocTapGiangVienView)
                AdminMainFrame.Navigate(GetOrCreatePage<ThongKeHocTapGiangVienView>());
        }

        private void NavAISinhDe_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is not AISinhDeView)
                AdminMainFrame.Navigate(GetOrCreatePage<AISinhDeView>());
        }

        private void NavGiamSat_Click(object sender, RoutedEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            bool hasAccess = user == null || 
                             string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(user.TenVaiTro, "GiangVien", StringComparison.OrdinalIgnoreCase);
            if (!hasAccess)
            {
                Common.ThongBaoDialog.CanhBao(this, "Chức năng 'Giám sát hệ thống' chỉ dành cho Quản trị viên và Giảng viên!", "Không có quyền truy cập");
                return;
            }

            if (AdminMainFrame.Content is not GiamSatHeThongView)
                AdminMainFrame.Navigate(GetOrCreatePage<GiamSatHeThongView>());
        }

        private bool _isAssistantOpen = false;

        public void ToggleAIAssistantSidebar()
        {
            if (_isAssistantOpen)
            {
                DongAIAssistantSidebar();
            }
            else
            {
                MoAIAssistantSidebar();
            }
        }

        public void MoAIAssistantSidebar()
        {
            _isAssistantOpen = true;
            aiAssistantSidebar.Visibility = Visibility.Visible;
            colRightAssistant.Width = new GridLength(680);
            aiAssistantSidebar.UpdateCurrentPage(AdminMainFrame.Content?.GetType().Name);
        }

        public void DongAIAssistantSidebar()
        {
            _isAssistantOpen = false;
            colRightAssistant.Width = new GridLength(0);
            aiAssistantSidebar.Visibility = Visibility.Collapsed;
        }

        private void TopNavTroLyAI_Click(object sender, RoutedEventArgs e)
        {
            ToggleAIAssistantSidebar();
        }

        private void AdminMainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (AdminMainFrame.Content is NoiDungHocTapView page &&
                page.DataContext is NoiDungHocTapViewModel vm &&
                vm.IsCreatingNewLesson &&
                vm.CoDuLieuBaiHocMoiChuaLuu())
            {
                bool xacNhan = Common.ThongBaoDialog.XacNhan(
                    this,
                    "Bạn đang soạn bài học mới và có nội dung chưa lưu.\nNếu chuyển sang chức năng khác, nội dung bài học đang tạo sẽ bị mất.\n\nBạn có chắc chắn muốn rời khỏi không?",
                    "Xác nhận rời khỏi trang",
                    confirmText: "Rời khỏi",
                    cancelText: "Ở lại chỉnh sửa");

                if (!xacNhan)
                {
                    e.Cancel = true;
                    return;
                }

                // Người dùng đồng ý rời đi: reset cờ tạo bài mới để không kẹt trạng thái khi quay lại
                vm.IsCreatingNewLesson = false;
            }
        }

        private void NavToStudentHome_Click(object sender, RoutedEventArgs e)
        {
            if (AdminMainFrame.Content is NoiDungHocTapView page &&
                page.DataContext is NoiDungHocTapViewModel vm &&
                vm.IsCreatingNewLesson &&
                vm.CoDuLieuBaiHocMoiChuaLuu())
            {
                bool xacNhan = Common.ThongBaoDialog.XacNhan(
                    this,
                    "Bạn đang soạn bài học mới và có nội dung chưa lưu.\nNếu chuyển về Trang chủ Học viên, nội dung bài học đang tạo sẽ bị mất.\n\nBạn có chắc chắn muốn rời khỏi không?",
                    "Xác nhận chuyển trang",
                    confirmText: "Rời khỏi",
                    cancelText: "Ở lại chỉnh sửa");

                if (!xacNhan) return;
                vm.IsCreatingNewLesson = false;
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void PnlUserProfile_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var user = SessionHelper.CurrentUser;
            bool isAdmin = user != null && string.Equals(user.TenVaiTro, "QuanTriVien", StringComparison.OrdinalIgnoreCase);

            var dlg = new ProfileGiangVienDialog();
            if (Common.ModalHelper.ShowDialogWithDim(dlg, this) == true)
            {
                // Đồng bộ cập nhật thông tin hiển thị trên thanh sidebar và header
                ApplyRolePermissions();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            bool confirm = Common.ThongBaoDialog.XacNhan(
                this, 
                "Bạn có chắc chắn muốn đăng xuất khỏi phiên làm việc hiện tại?", 
                "Xác nhận đăng xuất", 
                confirmText: "Đăng xuất", 
                cancelText: "Ở lại");

            if (confirm)
            {
                SessionHelper.Logout();
                var dangNhapWindow = new DangNhapWindow();
                dangNhapWindow.Show();
                this.Close();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private bool _notif1Read = false;
        private bool _notif2Read = false;
        private bool _notif3Read = false;

        private void UpdateNotificationStatus()
        {
            int unread = (_notif1Read ? 0 : 1) + (_notif2Read ? 0 : 1) + (_notif3Read ? 0 : 1);
            lblNotificationCount.Text = $"Thông báo hệ thống ({unread})";
            bdNotificationBadge.Visibility = unread > 0 ? Visibility.Visible : Visibility.Collapsed;
            pnlNotifItem1.Opacity = _notif1Read ? 0.55 : 1.0;
            pnlNotifItem2.Opacity = _notif2Read ? 0.55 : 1.0;
            pnlNotifItem3.Opacity = _notif3Read ? 0.55 : 1.0;
        }

        private void BtnNotification_Click(object sender, RoutedEventArgs e)
        {
            popNotifications.PlacementTarget = btnNotificationBell;
            popNotifications.IsOpen = !popNotifications.IsOpen;
        }

        private void BtnMarkAllNotificationsRead_Click(object sender, RoutedEventArgs e)
        {
            _notif1Read = true;
            _notif2Read = true;
            _notif3Read = true;
            UpdateNotificationStatus();
        }

        private void NotifItem1_Click(object sender, MouseButtonEventArgs e)
        {
            _notif1Read = true;
            UpdateNotificationStatus();
            popNotifications.IsOpen = false;
            AdminMainFrame.Navigate(GetOrCreatePage<DeThiLuyenTapView>());
        }

        private void NotifItem2_Click(object sender, MouseButtonEventArgs e)
        {
            _notif2Read = true;
            UpdateNotificationStatus();
            popNotifications.IsOpen = false;
            AdminMainFrame.Navigate(GetOrCreatePage<NganHangCauHoiView>());
        }

        private void NotifItem3_Click(object sender, MouseButtonEventArgs e)
        {
            _notif3Read = true;
            UpdateNotificationStatus();
            popNotifications.IsOpen = false;
            AdminMainFrame.Navigate(GetOrCreatePage<GiamSatHeThongView>());
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GiangVienWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AdminMainFrame.Content is NoiDungHocTapView page &&
                page.DataContext is NoiDungHocTapViewModel vm &&
                vm.IsCreatingNewLesson &&
                vm.CoDuLieuBaiHocMoiChuaLuu())
            {
                bool xacNhan = Common.ThongBaoDialog.XacNhan(
                    this,
                    "Bạn đang soạn bài học mới và có nội dung chưa lưu.\nNếu đóng ứng dụng, nội dung bài học đang tạo sẽ bị mất.\n\nBạn có chắc chắn muốn thoát không?",
                    "Xác nhận thoát",
                    confirmText: "Thoát và bỏ thay đổi",
                    cancelText: "Ở lại chỉnh sửa");

                if (!xacNhan)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        #region Live Search Autocomplete & Suggestions
        private void GiangVienWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                txtGlobalSearch.Focus();
                txtGlobalSearch.SelectAll();
                e.Handled = true;
            }
        }

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

        private System.Windows.Threading.DispatcherTimer? _searchDebounceTimer;

        private void BtnClearAdminSearch_Click(object sender, RoutedEventArgs e)
        {
            _searchDebounceTimer?.Stop();
            txtGlobalSearch.Text = string.Empty;
            popAdminSuggestions.IsOpen = false;
        }

        private void TxtGlobalSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                popAdminSuggestions.IsOpen = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                string query = txtGlobalSearch.Text.Trim();
                if (!string.IsNullOrEmpty(query))
                {
                    _searchDebounceTimer?.Stop();
                    popAdminSuggestions.IsOpen = false;
                    AdminMainFrame.Navigate(new NoiDungHocTapView(searchKeyword: query));
                    e.Handled = true;
                }
            }
        }

        private void TxtGlobalSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = txtGlobalSearch.Text.Trim();

            txtGlobalSearchPlaceholder.Visibility = string.IsNullOrEmpty(query) ? Visibility.Visible : Visibility.Collapsed;
            btnClearAdminSearch.Visibility = string.IsNullOrEmpty(query) ? Visibility.Collapsed : Visibility.Visible;

            if (string.IsNullOrEmpty(query))
            {
                _searchDebounceTimer?.Stop();
                popAdminSuggestions.IsOpen = false;
                pnlAdminSuggestions.Children.Clear();
                return;
            }

            if (_searchDebounceTimer == null)
            {
                _searchDebounceTimer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(250)
                };
                _searchDebounceTimer.Tick += async (s, ev) =>
                {
                    _searchDebounceTimer.Stop();
                    await ExecuteAdminGlobalSearchAsync(txtGlobalSearch.Text.Trim());
                };
            }
            else
            {
                _searchDebounceTimer.Stop();
            }

            _searchDebounceTimer.Start();
        }

        private async System.Threading.Tasks.Task ExecuteAdminGlobalSearchAsync(string query)
        {
            if (string.IsNullOrEmpty(query)) return;

            pnlAdminSuggestions.Children.Clear();

            // 1. Chức năng quản trị và điều hướng
            var modules = new (string Icon, string Title, string Subtitle, string Category, string BadgeBg, string BadgeFg, Action Action)[]
            {
                ("📊", "Tổng quan Dashboard", "Bảng điều khiển và các chỉ số hoạt động", "Chính", "#DCFCE7", "#15803D", () => AdminMainFrame.Navigate(GetOrCreatePage<TongQuanView>())),
                ("👥", "Quản lý Người dùng", "Danh sách học viên, giảng viên và quản trị viên", "Quản lý", "#E0F2FE", "#0369A1", () => AdminMainFrame.Navigate(GetOrCreatePage<NguoiDungView>())),
                ("📁", "Danh mục & Cấu trúc khóa học", "Cây thư mục, chủ đề và phân loại kiến thức", "Quản lý", "#FEF3C7", "#B45309", () => AdminMainFrame.Navigate(GetOrCreatePage<DanhMucView>())),
                ("📖", "Nội dung học tập & Bài giảng", "Kho bài học, bài tập trắc nghiệm và Flashcard", "Quản lý", "#F0FDF4", "#166534", () => AdminMainFrame.Navigate(GetOrCreatePage<NoiDungHocTapView>())),
                ("🗄️", "Ngân hàng Câu hỏi", "Kho lưu trữ và phân loại câu hỏi luyện thi", "Quản lý", "#F3E8FF", "#7E22CE", () => AdminMainFrame.Navigate(GetOrCreatePage<NganHangCauHoiView>())),
                ("⏱️", "Đề thi & Luyện tập", "Quản lý các đề thi khảo thí và phòng luyện tập", "Quản lý", "#FFE4E6", "#BE123C", () => AdminMainFrame.Navigate(GetOrCreatePage<DeThiLuyenTapView>())),
                ("📋", "Khảo sát Đầu vào", "Bài kiểm tra phân loại trình độ CEFR / IELTS", "Quản lý", "#CCFBF1", "#0F766E", () => AdminMainFrame.Navigate(GetOrCreatePage<KhaoSatDauVaoView>())),
                ("📈", "Thống kê Học tập Giảng viên", "Báo cáo điểm số, tỷ lệ hoàn thành và chuyên cần", "Báo cáo", "#F1F5F9", "#334155", () => AdminMainFrame.Navigate(GetOrCreatePage<ThongKeHocTapGiangVienView>())),
                ("✨", "Tạo Câu hỏi bằng AI", "Trí tuệ nhân tạo sinh đề từ tài liệu hoặc prompt", "AI", "#FFEDD5", "#C2410C", () => AdminMainFrame.Navigate(GetOrCreatePage<AISinhDeView>())),
                ("💻", "Giám sát Hệ thống", "Theo dõi sức khỏe API, database và tài nguyên", "Hệ thống", "#E2E8F0", "#475569", () => AdminMainFrame.Navigate(GetOrCreatePage<GiamSatHeThongView>())),
                ("🏠", "Trang chủ Học viên", "Chuyển nhanh sang giao diện trải nghiệm học viên", "Điều hướng", "#DCFCE7", "#15803D", () => NavToStudentHome_Click(this, new RoutedEventArgs()))
            };

            int matchCount = 0;
            foreach (var m in modules)
            {
                if (MatchSearch(m.Title, query) || MatchSearch(m.Subtitle, query) || MatchSearch(m.Category, query))
                {
                    pnlAdminSuggestions.Children.Add(CreateSuggestionItem(m.Icon, m.Title, m.Subtitle, m.Category, m.BadgeBg, m.BadgeFg, () =>
                    {
                        popAdminSuggestions.IsOpen = false;
                        txtGlobalSearch.Text = string.Empty;
                        m.Action();
                    }));
                    matchCount++;
                }
            }

            // 2. Tra cứu dữ liệu Người dùng thực tế
            try
            {
                var users = await System.Threading.Tasks.Task.Run(() => NguoiDungBUS.Instance.LayDanhSach(keyword: query));
                if (users != null && users.Count > 0)
                {
                    foreach (var u in users.Take(4))
                    {
                        string role = u.TenVaiTro == "GiangVien" ? "Giảng viên" : (u.TenVaiTro == "Admin" ? "Quản trị viên" : "Học viên");
                        string info = $"{u.Email} • {role} • Trạng thái: {(u.TrangThaiHoatDong ? "Đang hoạt động" : "Bị khóa")}";
                        pnlAdminSuggestions.Children.Add(CreateSuggestionItem("👤", u.HoTen, info, "Người dùng", "#E0F2FE", "#0369A1", () =>
                        {
                            popAdminSuggestions.IsOpen = false;
                            txtGlobalSearch.Text = string.Empty;
                            AdminMainFrame.Navigate(new NguoiDungView(searchKeyword: u.HoTen));
                        }));
                        matchCount++;
                    }
                }
            }
            catch { }

            // 3. Tra cứu Bài học thực tế
            try
            {
                var lessons = await System.Threading.Tasks.Task.Run(() => BaiHocBUS.LayDanhSachTuDatabase(search: query));
                if (lessons != null && lessons.Count > 0)
                {
                    foreach (var l in lessons.Take(4))
                    {
                        string info = $"Cấp độ: {l.MaTrinhDo} • {l.TenChuDe} • Trạng thái: {(l.DaXuatBan ? "Đã xuất bản" : "Bản nháp")}";
                        pnlAdminSuggestions.Children.Add(CreateSuggestionItem("📖", l.TieuDe, info, "Bài học", "#DCFCE7", "#15803D", () =>
                        {
                            popAdminSuggestions.IsOpen = false;
                            txtGlobalSearch.Text = string.Empty;
                            AdminMainFrame.Navigate(new NoiDungHocTapView(targetLessonId: l.MaBaiHoc));
                        }));
                        matchCount++;
                    }
                }
            }
            catch { }

            // 4. Tra cứu Ngân hàng câu hỏi thực tế
            try
            {
                var questions = await System.Threading.Tasks.Task.Run(() => DAL.CauHoiDAL.Instance.TimKiemCauHoi(query, 4));
                if (questions != null && questions.Count > 0)
                {
                    foreach (var q in questions)
                    {
                        string shortContent = q.NoiDungCauHoi.Length > 55 ? q.NoiDungCauHoi.Substring(0, 52) + "..." : q.NoiDungCauHoi;
                        string info = $"Mã #{q.MaCauHoi} • Độ khó: {q.DoKho} • Kỹ năng: {q.KyNang}";
                        pnlAdminSuggestions.Children.Add(CreateSuggestionItem("❓", shortContent, info, "Câu hỏi", "#F3E8FF", "#7E22CE", () =>
                        {
                            popAdminSuggestions.IsOpen = false;
                            txtGlobalSearch.Text = string.Empty;
                            AdminMainFrame.Navigate(new NganHangCauHoiView(q.MaCauHoi));
                        }));
                        matchCount++;
                    }
                }
            }
            catch { }

            // 5. Nếu không có kết quả
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
                    Text = "Thử tìm kiếm với tên người dùng, email, tiêu đề bài học, câu hỏi hoặc chức năng...",
                    FontSize = 11,
                    Foreground = (Brush)new BrushConverter().ConvertFromString("#94A3B8")!,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 4, 0, 0)
                });
                emptyBorder.Child = spEmpty;
                pnlAdminSuggestions.Children.Add(emptyBorder);
            }

            popAdminSuggestions.IsOpen = true;
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
        #endregion
    }
}
