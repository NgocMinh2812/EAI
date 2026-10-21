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
        }

        private void NganHangCauHoiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NganHangCauHoiViewModel vm)
            {
                vm.NapDanhMuc();
                vm.NapDanhSachBoCauHoi();
            }
        }
    }
}
