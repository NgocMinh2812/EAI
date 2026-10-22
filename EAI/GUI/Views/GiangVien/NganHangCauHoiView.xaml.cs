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
                vm.NapDanhMuc();
                vm.NapDanhSachBoCauHoi();
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
    }
}
