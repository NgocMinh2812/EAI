using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace GUI.Views
{
    public partial class ThiThuView : Page
    {
        public ThiThuView()
        {
            InitializeComponent();
        }

        private void BtnQuickTest_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DanhGiaNangLucView());
        }

        private void BtnStartIeltsTest_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Đề thi: IELTS Academic Reading & Listening Mini Mock Test #05\nThời gian làm bài: 40 phút.\n\nBạn đã chuẩn bị tai nghe và không gian yên tĩnh chưa?", 
                                      "Bắt đầu bài thi thử", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.OK)
            {
                this.NavigationService?.Navigate(new DanhGiaNangLucView());
            }
        }

        private void BtnStartToeicTest_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Đề thi: TOEIC Reading Comprehension Diagnostic Test\nThời gian làm bài: 45 phút.\n\nBắt đầu tính giờ ngay bây giờ?", 
                                      "Bắt đầu bài thi TOEIC", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.OK)
            {
                this.NavigationService?.Navigate(new DanhGiaNangLucView());
            }
        }

        private void BtnStartGrammarTest_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Đề thi: Kiểm tra Đột phá Ngữ pháp & Collocations Band B2\nThời gian làm bài: 25 phút.\n\nBắt đầu làm bài?", 
                                      "Bắt đầu bài thi Ngữ pháp", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.OK)
            {
                this.NavigationService?.Navigate(new DanhGiaNangLucView());
            }
        }
    }
}
