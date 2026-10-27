using System.Windows;
using System.Windows.Controls;
using GUI.ViewModels.GiangVien;

namespace GUI.Views.GiangVien
{
    public partial class NganHangCauHoiView : Page
    {
        public NganHangCauHoiView()
        {
            InitializeComponent();
            Loaded += NganHangCauHoiView_Loaded;
            Unloaded += NganHangCauHoiView_Unloaded;
        }

        private void NganHangCauHoiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NganHangCauHoiViewModel vm)
            {
                if (vm.DanhSachBoCauHoi.Count == 0)
                {
                    vm.NapDanhMuc();
                    vm.NapDanhSachBoCauHoi();
                }
            }
        }

        private void NganHangCauHoiView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NganHangCauHoiViewModel vm)
            {
                vm.DungAudio();
            }
        }

        private void SldAudioTimeline_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is NganHangCauHoiViewModel vm)
            {
                vm.BatDauTuaAudio();
            }
        }

        private void SldAudioTimeline_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Slider slider && DataContext is NganHangCauHoiViewModel vm)
            {
                vm.KetThucTuaAudio(slider.Value);
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
