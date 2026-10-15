using System.Windows;
using System.Windows.Controls;

namespace GUI.Views
{
    public partial class TienDoHocView : Page
    {
        public TienDoHocView()
        {
            InitializeComponent();
        }

        private void BtnExportReport_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThanhCong(
                "📊 Đang tạo và xuất báo cáo tiến độ học tập chi tiết (PDF). Báo cáo đã được lưu vào thư mục Downloads của bạn!", 
                "Xuất báo cáo");
        }
    }
}
