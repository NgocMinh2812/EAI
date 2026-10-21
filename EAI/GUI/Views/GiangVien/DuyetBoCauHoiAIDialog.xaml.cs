using System.Windows;
using System.Windows.Input;
using GUI.ViewModels.GiangVien;

namespace GUI.Views.GiangVien
{
    public partial class DuyetBoCauHoiAIDialog : Window
    {
        public DuyetBoCauHoiAIViewModel ViewModel { get; }

        public DuyetBoCauHoiAIDialog(DuyetBoCauHoiAIViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = vm;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DungAudio();
            DialogResult = ViewModel.DaLuuThanhCong;
            Close();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            ViewModel.DungAudio();
        }

        private void ChkBaiHoc_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CapNhatThongKe();
        }
    }
}
