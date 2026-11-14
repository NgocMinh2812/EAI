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
        private string _snapshotMatrix = "";

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
            _snapshotMatrix = TaoChuoiSnapshot();
        }

        private string TaoChuoiSnapshot()
        {
            if (_matrixList == null) return "";
            return string.Join("|", _matrixList.Select(m => $"{m.MaCapDo}_{m.TenHienThi}_{m.DiemPhanTramMin}_{m.DiemPhanTramMax}_{m.QuyDoiIelts}_{m.QuyDoiToeic}_{m.LoTrinhDeXuat}"));
        }

        private bool CoThayDoiChuaLuu()
        {
            dgMaTran?.CommitEdit();
            return string.Compare(_snapshotMatrix, TaoChuoiSnapshot(), StringComparison.Ordinal) != 0;
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            if (CoThayDoiChuaLuu())
            {
                bool confirm = Common.ThongBaoDialog.XacNhan(
                    "Ma trận quy đổi có các thay đổi chưa được lưu.\n\nBạn có chắc chắn muốn thoát mà không lưu không?",
                    "Xác nhận thoát",
                    confirmText: "Thoát không lưu",
                    cancelText: "Ở lại");
                if (!confirm) return;
            }

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
            bool confirm = Common.ThongBaoDialog.XacNhan(
                "Bạn có chắc chắn muốn khôi phục toàn bộ ma trận quy đổi về giá trị chuẩn CEFR mặc định không?\n\nCác giá trị điều chỉnh hiện tại trên bảng sẽ bị thay thế.",
                "Xác nhận khôi phục mặc định",
                confirmText: "Khôi phục",
                cancelText: "Hủy bỏ");
            if (!confirm) return;

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

            // Validate logic: Min <= Max, Min >= 0, Max <= 100, TextNotEmpty
            foreach (var item in _matrixList)
            {
                if (string.IsNullOrWhiteSpace(item.TenHienThi))
                {
                    Common.ThongBaoDialog.CanhBao($"Tên hiển thị của cấp độ {item.MaCapDo} không được để trống.", "Thiếu thông tin");
                    return;
                }

                if (string.IsNullOrWhiteSpace(item.QuyDoiIelts) || string.IsNullOrWhiteSpace(item.QuyDoiToeic))
                {
                    Common.ThongBaoDialog.CanhBao($"Thông tin quy đổi IELTS / TOEIC của cấp độ {item.MaCapDo} không được để trống.", "Thiếu thông tin");
                    return;
                }

                if (item.DiemPhanTramMin < 0 || item.DiemPhanTramMax > 100 || item.DiemPhanTramMin > item.DiemPhanTramMax)
                {
                    Common.ThongBaoDialog.CanhBao($"Khoảng điểm của cấp độ {item.MaCapDo} không hợp lệ (Phải từ 0% đến 100% và Từ % <= Đến %).", "Lỗi dữ liệu");
                    return;
                }
            }

            // Kiểm tra tính liên tục / chồng lấn
            var sortedList = _matrixList.OrderBy(m => m.ThuTuSapXep).ToList();
            for (int i = 0; i < sortedList.Count - 1; i++)
            {
                if (sortedList[i].DiemPhanTramMax >= sortedList[i + 1].DiemPhanTramMax)
                {
                    Common.ThongBaoDialog.CanhBao($"Ngưỡng điểm tối đa của {sortedList[i].MaCapDo} ({sortedList[i].DiemPhanTramMax}%) không thể lớn hơn hoặc bằng {sortedList[i + 1].MaCapDo} ({sortedList[i + 1].DiemPhanTramMax}%).", "Xung đột khoảng điểm");
                    return;
                }
            }

            bool confirm = Common.ThongBaoDialog.XacNhan(
                "Bạn có chắc chắn muốn lưu ma trận quy đổi trình độ mới vào cơ sở dữ liệu không?\n\nCác bài khảo sát sau này của sinh viên sẽ tự động đối chiếu theo thang điểm này.",
                "Xác nhận lưu ma trận",
                confirmText: "Lưu ma trận",
                cancelText: "Hủy bỏ");
            if (!confirm) return;

            bool thanhCong = KhaoSatBUS.Instance.LuuMaTranQuyDoi(_matrixList.ToList(), out string errorMessage);
            if (thanhCong)
            {
                _snapshotMatrix = TaoChuoiSnapshot();
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
