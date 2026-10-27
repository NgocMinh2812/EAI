using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class SuaMaTranQuyDoiDialog : Window
    {
        private ObservableCollection<CauHinhQuyDoiKhaoSatDTO> _matrixList = new();

        public SuaMaTranQuyDoiDialog()
        {
            InitializeComponent();
            Loaded += SuaMaTranQuyDoiDialog_Loaded;
        }

        private void SuaMaTranQuyDoiDialog_Loaded(object sender, RoutedEventArgs e)
        {
            NapDuLieu();
        }

        private void NapDuLieu()
        {
            var list = KhaoSatBUS.Instance.LayMaTranQuyDoi();
            _matrixList = new ObservableCollection<CauHinhQuyDoiKhaoSatDTO>(list);
            dgMaTran.ItemsSource = _matrixList;
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnKhoiPhucMacDinh_Click(object sender, RoutedEventArgs e)
        {
            _matrixList.Clear();
            _matrixList.Add(new CauHinhQuyDoiKhaoSatDTO { MaCapDo = "A1", TenHienThi = "A1 (Mất gốc / Cơ bản)", DiemPhanTramMin = 0, DiemPhanTramMax = 39, QuyDoiIelts = "IELTS 3.0", QuyDoiToeic = "TOEIC 100 - 250", LoTrinhDeXuat = "Ngữ pháp nền tảng + 500 từ vựng cốt lõi", MauSac = "#DC2626", ThuTuSapXep = 1 });
            _matrixList.Add(new CauHinhQuyDoiKhaoSatDTO { MaCapDo = "A2", TenHienThi = "A2 (Sơ cấp)", DiemPhanTramMin = 40, DiemPhanTramMax = 59, QuyDoiIelts = "IELTS 3.5 - 4.5", QuyDoiToeic = "TOEIC 255 - 450", LoTrinhDeXuat = "Luyện phản xạ câu & Nghe hiểu đơn giản", MauSac = "#EA580C", ThuTuSapXep = 2 });
            _matrixList.Add(new CauHinhQuyDoiKhaoSatDTO { MaCapDo = "B1", TenHienThi = "B1 (Trung cấp)", DiemPhanTramMin = 60, DiemPhanTramMax = 74, QuyDoiIelts = "IELTS 5.0 - 5.5", QuyDoiToeic = "TOEIC 455 - 650", LoTrinhDeXuat = "Từ vựng học thuật B1 & Cấu trúc câu ghép", MauSac = "#2563EB", ThuTuSapXep = 3 });
            _matrixList.Add(new CauHinhQuyDoiKhaoSatDTO { MaCapDo = "B2", TenHienThi = "B2 (Trên trung cấp)", DiemPhanTramMin = 75, DiemPhanTramMax = 89, QuyDoiIelts = "IELTS 6.0 - 7.0", QuyDoiToeic = "TOEIC 655 - 850", LoTrinhDeXuat = "Viết luận Task 2 & Skimming/Scanning đọc hiểu", MauSac = "#15803D", ThuTuSapXep = 4 });
            _matrixList.Add(new CauHinhQuyDoiKhaoSatDTO { MaCapDo = "C1", TenHienThi = "C1 (Nâng cao)", DiemPhanTramMin = 90, DiemPhanTramMax = 100, QuyDoiIelts = "IELTS 7.5 - 8.5+", QuyDoiToeic = "TOEIC 855 - 990", LoTrinhDeXuat = "Thành ngữ Collocations cao cấp & Phản biện", MauSac = "#7E22CE", ThuTuSapXep = 5 });
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            dgMaTran.CommitEdit();

            // Validate logic: Min <= Max
            foreach (var item in _matrixList)
            {
                if (item.DiemPhanTramMin < 0 || item.DiemPhanTramMax > 100 || item.DiemPhanTramMin > item.DiemPhanTramMax)
                {
                    Common.ThongBaoDialog.CanhBao($"Khoảng điểm của cấp độ {item.MaCapDo} không hợp lệ (Phải từ 0% đến 100% và Min <= Max).", "Lỗi dữ liệu");
                    return;
                }
            }

            bool thanhCong = KhaoSatBUS.Instance.LuuMaTranQuyDoi(_matrixList.ToList(), out string errorMessage);
            if (thanhCong)
            {
                Common.ThongBaoDialog.ThanhCong("Đã lưu ma trận quy đổi trình độ vào CSDL SQL Server thành công!", "Thành công");
                DialogResult = true;
                Close();
            }
            else
            {
                Common.ThongBaoDialog.Loi($"Không thể lưu ma trận quy đổi: {errorMessage}", "Lỗi lưu dữ liệu");
            }
        }
    }
}
