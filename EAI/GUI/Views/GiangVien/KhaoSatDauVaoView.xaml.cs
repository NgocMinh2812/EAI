using System.Windows;
using System.Windows.Controls;

namespace GUI.Views.GiangVien
{
    public partial class KhaoSatDauVaoView : Page
    {
        public KhaoSatDauVaoView()
        {
            InitializeComponent();
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở biểu mẫu cấu hình bộ câu hỏi bài kiểm tra chẩn đoán năng lực 20 câu hỏi nhanh.", 
                            "Cập nhật đề khảo sát", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
