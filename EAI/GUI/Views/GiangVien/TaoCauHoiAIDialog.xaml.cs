using System.Windows;
using System.Windows.Input;
using GUI.ViewModels.GiangVien;

namespace GUI.Views.GiangVien
{
    public partial class TaoCauHoiAIDialog : Window
    {
        public NoiDungHocTapViewModel ViewModel { get; }

        public TaoCauHoiAIDialog(NoiDungHocTapViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = vm;
            Loaded += TaoCauHoiAIDialog_Loaded;
        }

        private async void TaoCauHoiAIDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.SinhCauHoiAIAsync();
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
            DialogResult = false;
            Close();
        }

        private void BtnChenCauHoi_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ThucHienChenCauHoiVaoBaiHoc();
            DialogResult = true;
            Close();
        }
    }
}
