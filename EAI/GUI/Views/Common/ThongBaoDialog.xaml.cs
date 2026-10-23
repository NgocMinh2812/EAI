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
        Info,
        Prompt
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
                    icoSymbol.Kind = LucideIconKind.Trash2;
                    icoSymbol.Icon = null;
                    icoSymbol.Size = 30;
                    icoSymbol.StrokeThickness = 2.2;
                    icoSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEE2E2")!;
                    borderMain.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FCA5A5")!;
                    txtTitle.Foreground = (Brush)new BrushConverter().ConvertFrom("#7F1D1D")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    btnConfirm.Content = string.IsNullOrEmpty(btnConfirm.Content?.ToString()) ? "Xóa vĩnh viễn" : btnConfirm.Content;
                    btnCancel.Visibility = Visibility.Visible;
                    Grid.SetColumn(btnConfirm, 2);
                    Grid.SetColumnSpan(btnConfirm, 1);
                    break;

                case ThongBaoType.Confirm:
                    icoSymbol.Kind = LucideIconKind.None;
                    icoSymbol.Icon = "question-mark";
                    icoSymbol.Size = 32;
                    icoSymbol.StrokeThickness = 2.8;
                    icoSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#0F766E")!;
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#CCFBF1")!;
                    borderMain.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#99F6E4")!;
                    txtTitle.Foreground = (Brush)new BrushConverter().ConvertFrom("#134E4A")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#0F766E")!;
                    btnCancel.Visibility = Visibility.Visible;
                    Grid.SetColumn(btnConfirm, 2);
                    Grid.SetColumnSpan(btnConfirm, 1);
                    break;

                case ThongBaoType.Success:
                    icoSymbol.Kind = LucideIconKind.Check;
                    icoSymbol.Icon = null;
                    icoSymbol.Size = 34;
                    icoSymbol.StrokeThickness = 3.2;
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#DCFCE7")!;
                    icoSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#16A34A")!;
                    borderMain.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#86EFAC")!;
                    txtTitle.Foreground = (Brush)new BrushConverter().ConvertFrom("#14532D")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#16A34A")!;
                    btnConfirm.Content = "Đã hiểu";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;

                case ThongBaoType.Warning:
                    icoSymbol.Kind = LucideIconKind.AlertTriangle;
                    icoSymbol.Icon = null;
                    icoSymbol.Size = 34;
                    icoSymbol.StrokeThickness = 2.4;
                    icoSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#D97706")!;
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEF3C7")!;
                    borderMain.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FDE68A")!;
                    txtTitle.Foreground = (Brush)new BrushConverter().ConvertFrom("#78350F")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#D97706")!;
                    btnConfirm.Content = "Đóng";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;

                case ThongBaoType.Error:
                    icoSymbol.Kind = LucideIconKind.X;
                    icoSymbol.Icon = null;
                    icoSymbol.Size = 30;
                    icoSymbol.StrokeThickness = 3.2;
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEE2E2")!;
                    icoSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    borderMain.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FCA5A5")!;
                    txtTitle.Foreground = (Brush)new BrushConverter().ConvertFrom("#7F1D1D")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    btnConfirm.Content = "Đóng";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;

                case ThongBaoType.Info:
                default:
                    icoSymbol.Kind = LucideIconKind.None;
                    icoSymbol.Icon = "info-mark";
                    icoSymbol.Size = 32;
                    icoSymbol.StrokeThickness = 2.8;
                    icoSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#0284C7")!;
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#E0F2FE")!;
                    borderMain.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#BAE6FD")!;
                    txtTitle.Foreground = (Brush)new BrushConverter().ConvertFrom("#0C4A6E")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#0284C7")!;
                    btnConfirm.Content = "Đồng ý";
                    btnCancel.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(btnConfirm, 0);
                    Grid.SetColumnSpan(btnConfirm, 3);
                    break;

                case ThongBaoType.Prompt:
                    icoSymbol.Kind = LucideIconKind.Pencil;
                    icoSymbol.Icon = null;
                    icoSymbol.Size = 30;
                    icoSymbol.StrokeThickness = 2.4;
                    icoSymbol.Foreground = (Brush)new BrushConverter().ConvertFrom("#2563EB")!;
                    borderIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#EFF6FF")!;
                    borderMain.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#93C5FD")!;
                    txtTitle.Foreground = (Brush)new BrushConverter().ConvertFrom("#1E3A8A")!;
                    btnConfirm.Background = (Brush)new BrushConverter().ConvertFrom("#2563EB")!;
                    btnCancel.Visibility = Visibility.Visible;
                    txtInput.Visibility = Visibility.Visible;
                    Grid.SetColumn(btnConfirm, 2);
                    Grid.SetColumnSpan(btnConfirm, 1);
                    Loaded += (s, e) => { txtInput.Focus(); txtInput.SelectAll(); };
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

        private static Window? GetActiveWindow(Window? owner = null)
        {
            if (owner != null && owner.IsLoaded && owner.IsVisible)
                return owner;

            try
            {
                var active = System.Linq.Enumerable.FirstOrDefault(
                    System.Linq.Enumerable.OfType<Window>(Application.Current?.Windows ?? new WindowCollection()),
                    w => w.IsActive && w.IsVisible);

                if (active != null) return active;

                if (Application.Current?.MainWindow != null && Application.Current.MainWindow.IsVisible)
                    return Application.Current.MainWindow;

                return System.Linq.Enumerable.LastOrDefault(
                    System.Linq.Enumerable.OfType<Window>(Application.Current?.Windows ?? new WindowCollection()),
                    w => w.IsVisible);
            }
            catch
            {
                return Application.Current?.MainWindow;
            }
        }

        public static bool XacNhanXoa(Window? owner, string message, string title = "Xác nhận xóa")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.DangerConfirm, "Xóa vĩnh viễn", "Hủy bỏ");
            return ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner)) == true;
        }

        public static bool XacNhanXoa(string message, string title = "Xác nhận xóa") =>
            XacNhanXoa(null, message, title);

        public static bool XacNhan(Window? owner, string message, string title = "Xác nhận thao tác", 
                                  string confirmText = "Xác nhận", string cancelText = "Hủy bỏ")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Confirm, confirmText, cancelText);
            return ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner)) == true;
        }

        public static bool XacNhan(string message, string title = "Xác nhận thao tác",
                                  string confirmText = "Xác nhận", string cancelText = "Hủy bỏ") =>
            XacNhan(null, message, title, confirmText, cancelText);

        public static void ThanhCong(Window? owner, string message, string title = "Thành công")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Success);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }

        public static void ThanhCong(string message, string title = "Thành công") =>
            ThanhCong(null, message, title);

        public static void CanhBao(Window? owner, string message, string title = "Cảnh báo")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Warning);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }

        public static void CanhBao(string message, string title = "Cảnh báo") =>
            CanhBao(null, message, title);

        public static void Loi(Window? owner, string message, string title = "Lỗi")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Error);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }

        public static void Loi(string message, string title = "Lỗi") =>
            Loi(null, message, title);

        public static void ThongTin(Window? owner, string message, string title = "Thông báo")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Info);
            ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
        }

        public static void ThongTin(string message, string title = "Thông báo") =>
            ThongTin(null, message, title);

        public static string? NhapLieu(Window? owner, string message, string title, string defaultText = "",
                                       string confirmText = "Lưu", string cancelText = "Hủy bỏ")
        {
            var dlg = new ThongBaoDialog(message, title, ThongBaoType.Prompt, confirmText, cancelText);
            dlg.txtInput.Text = defaultText;
            bool? ok = ModalHelper.ShowDialogWithDim(dlg, GetActiveWindow(owner));
            return ok == true ? dlg.txtInput.Text : null;
        }

        public static string? NhapLieu(string message, string title, string defaultText = "",
                                       string confirmText = "Lưu", string cancelText = "Hủy bỏ") =>
            NhapLieu(null, message, title, defaultText, confirmText, cancelText);
    }
}
