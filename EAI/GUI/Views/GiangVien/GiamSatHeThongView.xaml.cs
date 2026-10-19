using System.Windows.Controls;

namespace GUI.Views.GiangVien
{
    public partial class GiamSatHeThongView : Page
    {
        public GiamSatHeThongView()
        {
            InitializeComponent();
            DataContext = new ViewModels.GiangVien.GiamSatHeThongViewModel();
        }
    }
}
