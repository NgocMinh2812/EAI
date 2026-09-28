using System.Windows;

namespace GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow(bool isFirstLogin = false)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            if (isFirstLogin)
            {
                MainFrame.Navigate(new Views.DanhGiaNangLucView());
            }
            else
            {
                MainFrame.Navigate(new Views.TrangChuView());
            }
        }
    }
}