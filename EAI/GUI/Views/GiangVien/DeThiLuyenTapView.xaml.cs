using System.Windows;
using System.Windows.Controls;

namespace GUI.Views.GiangVien
{
    public partial class DeThiLuyenTapView : Page
    {
        public DeThiLuyenTapView()
        {
            InitializeComponent();
        }

        private void BtnTaoDeThi_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin(
                "Mở trình tạo đề thi: Thiết lập tiêu đề, chọn bài tập từ Ngân hàng câu hỏi, cài đặt thời gian đếm ngược và chế độ thi.", 
                "Tạo đề thi mới");
        }
    }
}
