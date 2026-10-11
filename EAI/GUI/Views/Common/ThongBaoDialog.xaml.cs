using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.Common
{
    public enum ThongBaoType
    {
        Confirm,
        DangerConfirm,
        Success,
        Warning,
        Error,
        Info
    }

    public partial class ThongBaoDialog : Window
    {
        public bool Result { get; private set; } = false;

        public ThongBaoDialog(string message, string title, ThongBaoType type = ThongBaoType.Info, 
                              string confirmText = "Đồng ý", string cancelText = "Hủy bỏ")
        {
            InitializeComponent();
            txtTitle.Text = title;
            txtMessage.Text = message;
            btnConfirm.Content = confirmText;
            btnCancel.Content = cancelText;

            ApplyTypeTheme(type);
        }

        private void ApplyTypeTheme(ThongBaoType type)
        {
            switch (type)
            {
                case ThongBaoType.DangerConfirm:
                    txtIconSymbol.Text = "🗑️";
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEE2E2")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    btnConfirm.Content = string.IsNullOrEmpty(btnConfirm.Content?.ToString()) ? "Xóa vĩnh viễn" : btnConfirm.Content;
                    btnCancel.Visibility = Visibility.Visible;
                    Grid.SetColumn(btnConfirm, 2);
                    Grid.SetColumnSpan(btnConfirm, 1);
                    break;

                case ThongBaoType.Confirm:
                    txtIconSymbol.Text = "❓";
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEF3C7")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#0F766E")!;
                    btnCancel.Visibility = Visibility.Visible;
                    Grid.SetColumn(btnConfirm, 2);
                    Grid.SetColumnSpan(btnConfirm, 1);
                    break;

                case ThongBaoType.Success:
                    txtIconSymbol.Text = "✓";
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#DCFCE7")!;
                    txtIconSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#15803D")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#10B981")!;
                    btnConfirm.Content = "Đã hiểu";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;

                case ThongBaoType.Warning:
                    txtIconSymbol.Text = "⚠️";
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEF3C7")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#D97706")!;
                    btnConfirm.Content = "Đóng";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;

                case ThongBaoType.Error:
                    txtIconSymbol.Text = "✕";
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEE2E2")!;
                    txtIconSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#EF4444")!;
                    btnConfirm.Content = "Đóng";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;

                case ThongBaoType.Info:
                default:
                    txtIconSymbol.Text = "ℹ️";
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#E0F2FE")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#0F766E")!;
                    btnConfirm.Content = "Đồng ý";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            DialogResult = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnConfirm_Click(sender, e);
            }
            else if (e.Key == Key.Escape)
            {
                BtnCancel_Click(sender, e);
            }
        }

        // ================= STATIC HELPER METHODS =================

        private static Window? GetActiveWindow(Window? owner)
        {
            return owner ?? Application.Current.MainWindow;
        }

        public static bool XacNhanXoa(Window? owner, string message, string title = "Xác nhận xóa tài khoản")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.DangerConfirm, "Xóa vĩnh viễn", "Hủy bỏ");
            return ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner)) == true;
        }

        public static bool XacNhan(Window? owner, string message, string title = "Xác nhận thao tác", 
                                  string confirmText = "Xác nhận", string cancelText = "Hủy bỏ")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Confirm, confirmText, cancelText);
            return ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner)) == true;
        }

        public static void ThanhCong(Window? owner, string message, string title = "Thành công")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Success);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }

        public static void CanhBao(Window? owner, string message, string title = "Cảnh báo")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Warning);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }

        public static void Loi(Window? owner, string message, string title = "Lỗi")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Error);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }

        public static void ThongTin(Window? owner, string message, string title = "Thông báo")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Info);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }
    }
}
