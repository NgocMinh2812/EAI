using System.Windows;
using System.Windows.Controls;
using GUI.ViewModels.GiangVien;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class DeThiLuyenTapView : Page
    {
        public DeThiLuyenTapView()
        {
            InitializeComponent();
            Loaded += DeThiLuyenTapView_Loaded;
            DataContextChanged += DeThiLuyenTapView_DataContextChanged;
            AttachViewModelEvents();
        }

        private void DeThiLuyenTapView_Loaded(object sender, RoutedEventArgs e)
        {
            AttachViewModelEvents();
            if (DataContext is DeThiLuyenTapViewModel vm)
            {
                vm.TaiDuLieuDeThi();
            }
        }

        private void DeThiLuyenTapView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AttachViewModelEvents();
        }

        private void AttachViewModelEvents()
        {
            if (DataContext is DeThiLuyenTapViewModel vm)
            {
                vm.YeuCauHienThiLichSuBaiNop = (deThi, history) =>
                {
                    var dlg = new LichSuBaiNopDialog(deThi, history);
                    ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));
                };

                vm.YeuCauHienThiXemTruoc = (deThi, questions) =>
                {
                    var dlg = new XemTruocDeThiDialog(deThi, questions);
                    ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));
                };
            }
        }
    }
}
