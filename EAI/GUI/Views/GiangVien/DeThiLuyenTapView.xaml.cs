using System.Windows;
using System.Windows.Controls;
using GUI.ViewModels.GiangVien;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class DeThiLuyenTapView : Page
    {
        public DeThiLuyenTapView()
        {
            InitializeComponent();
            Loaded += DeThiLuyenTapView_Loaded;
            DataContextChanged += DeThiLuyenTapView_DataContextChanged;
            AttachViewModelEvents();
        }

        private void DeThiLuyenTapView_Loaded(object sender, RoutedEventArgs e)
        {
            AttachViewModelEvents();
            if (DataContext is DeThiLuyenTapViewModel vm)
            {
                vm.TaiDuLieuDeThi();
            }
        }

        private void DeThiLuyenTapView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AttachViewModelEvents();
        }

        private void AttachViewModelEvents()
        {
            if (DataContext is DeThiLuyenTapViewModel vm)
            {
                vm.YeuCauHienThiLichSuBaiNop = (deThi, history) =>
                {
                    var dlg = new LichSuBaiNopDialog(deThi, history);
                    ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));
                };

                vm.YeuCauHienThiXemTruoc = (deThi, questions) =>
                {
                    var dlg = new XemTruocDeThiDialog(deThi, questions);
                    ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));
                };
            }
        }

        private void Smooth_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            GUI.Views.Common.ScrollHelper.HandleSmoothPreviewMouseWheel(sender, e);
        }

        private void OnColumnScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0 || e.HorizontalChange != 0)
            {
                GUI.Views.Common.ScrollHelper.CloseAllOpenComboBoxes();
            }
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild) return typedChild;
                var found = FindVisualChild<T>(child);
                if (found != null) return found;
            }
            return null;
        }
    }
}
