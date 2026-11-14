using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BUS.Assistant;
using DTO.Assistant;
using GUI.ViewModels.Assistant;

namespace GUI.Views.Common.Assistant
{
    /// <summary>
    /// Interaction logic for AIAssistantSidebarControl.xaml
    /// Thanh Trợ lý AI tích hợp toàn diện EAI (Docked bên phải màn hình).
    /// </summary>
    public partial class AIAssistantSidebarControl : UserControl
    {
        public event Action? CloseRequested;

        private AIAssistantSidebarViewModel ViewModel => AIAssistantSidebarViewModel.Instance;

        private static string _currentPageCode = "TongQuan";
        private static string _currentPageTitle = "Tổng quan giảng viên";

        public AIAssistantSidebarControl()
        {
            InitializeComponent();
            DataContext = ViewModel;

            // Đăng ký hook ngữ cảnh toàn cục cho AssistantContextProvider
            AssistantContextProvider.CustomPageDetector = () => (_currentPageCode, _currentPageTitle, null, null);

            // Tự động cuộn xuống tin nhắn mới nhất
            ViewModel.ScrollToBottomRequested += OnScrollToBottomRequested;
        }

        private void OnScrollToBottomRequested()
        {
            Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    scrChatMessages?.ScrollToBottom();
                }
                catch
                {
                    // Tránh crash nếu UI đang render
                }
            });
        }

        /// <summary>
        /// Cập nhật trang hiện tại mà người dùng đang mở trong EAI
        /// để Trợ lý AI có ngữ cảnh trả lời chính xác nhất.
        /// </summary>
        public void UpdateCurrentPage(string? pageName, string? pageTitle = null)
        {
            if (string.IsNullOrWhiteSpace(pageName)) return;

            _currentPageCode = pageName;
            _currentPageTitle = pageTitle ?? pageName switch
            {
                "NguoiDungPage" => "Quản lý Người dùng & Học viên",
                "DanhMucCauTrucPage" => "Danh mục & Cấu trúc",
                "NoiDungHocTapPage" => "Nội dung & Bài học",
                "NganHangCauHoiPage" => "Ngân hàng câu hỏi",
                "DeThiLuyenTapPage" => "Đề thi & Luyện tập",
                "KhaoSatDauVaoPage" => "Khảo sát đầu vào",
                "ThongKeBaoCaoPage" => "Thống kê & Báo cáo",
                "TaoCauHoiAIPage" => "Tạo câu hỏi bằng AI",
                "TrangChuHocVienPage" => "Trang chủ Học viên",
                "TongQuanPage" => "Tổng quan",
                _ => pageName
            };
        }

        private void BtnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            CloseRequested?.Invoke();
        }

        private async void BtnNewChat_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.TaoCuocTroChuyenMoiAsync();
            txtUserInput?.Focus();
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.GuiTinNhanAsync();
            txtUserInput?.Focus();
        }

        private void BtnStopResponse_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.HuyPhanHoi();
        }

        private async void QuickPromptButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string prompt && !string.IsNullOrWhiteSpace(prompt))
            {
                await ViewModel.GuiTinNhanAsync(prompt);
            }
        }

        private async void BtnConfirmAction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is AssistantChatMessageItem msg)
            {
                await ViewModel.XacNhanHanhDongGhiAsync(msg);
            }
        }

        private void BtnRejectAction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is AssistantChatMessageItem msg)
            {
                ViewModel.TuChoiHanhDongGhi(msg);
            }
        }

        private async void LstConversations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is AIAssistantConversationDTO conv)
            {
                if (ViewModel.CuocTroChuyenHienTai?.MaHoiThoai != conv.MaHoiThoai)
                {
                    await ViewModel.ChonCuocTroChuyenAsync(conv);
                }
            }
        }

        private async void BtnDeleteConversation_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem)
            {
                var conv = elem.Tag as AIAssistantConversationDTO ?? elem.DataContext as AIAssistantConversationDTO;
                if (conv != null)
                {
                    await ViewModel.XoaCuocTroChuyenAsync(conv);
                }
            }
        }

        private async void TxtUserInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                e.Handled = true;
                if (ViewModel.CoTheGui)
                {
                    await ViewModel.GuiTinNhanAsync();
                }
            }
        }
    }
}

