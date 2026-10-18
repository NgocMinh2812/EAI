using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DTO;
using DAL;
using BUS;
using GUI.ViewModels.Common;

namespace GUI.ViewModels.GiangVien
{
    public class DeThiLuyenTapViewModel : ViewModelBase
    {
        private BaiTapDeThiDTO _currentDeThi = new();
        private string _thongBaoLoi = string.Empty;
        private string _filterTrinhDo = "All";

        public class QuestionSelectionItem : ViewModelBase
        {
            private bool _isSelected;
            private double _diem = 0.25;

            public CauHoiKhaoSatDTO CauHoi { get; set; } = new();

            public double Diem
            {
                get => _diem;
                set => SetProperty(ref _diem, value);
            }

            public bool IsSelected
            {
                get => _isSelected;
                set => SetProperty(ref _isSelected, value);
            }
        }

        public BaiTapDeThiDTO CurrentDeThi
        {
            get => _currentDeThi;
            set => SetProperty(ref _currentDeThi, value);
        }

        public ObservableCollection<QuestionSelectionItem> NgânHangCauHoiChon { get; set; } = new();
        public ObservableCollection<BaiTapDeThiDTO> DanhSachDeThiHienCo { get; set; } = new();
        public ObservableCollection<TrinhDoDTO> DanhSachTrinhDo { get; set; } = new();

        public int TongSoCauDaChon => NgânHangCauHoiChon.Count(x => x.IsSelected);
        public double TongDiemDeThi => Math.Round(NgânHangCauHoiChon.Where(x => x.IsSelected).Sum(x => x.Diem), 2);

        public string ThongBaoLoi
        {
            get => _thongBaoLoi;
            set => SetProperty(ref _thongBaoLoi, value);
        }

        public string FilterTrinhDo
        {
            get => _filterTrinhDo;
            set
            {
                if (SetProperty(ref _filterTrinhDo, value))
                    NapCauHoiTheoTrinhDo();
            }
        }

        public ICommand CapNhatLuaChonCauHoiCommand { get; }
        public ICommand LuuDeThiCommand { get; }
        public ICommand ThemDeThiMoiCommand { get; }
        public ICommand ChonTatCaCauHoiCommand { get; }
        public ICommand BoChonTatCaCommand { get; }

        public DeThiLuyenTapViewModel()
        {
            CapNhatLuaChonCauHoiCommand = new RelayCommand(ThucHienCapNhatThongKeCauHoi);
            LuuDeThiCommand = new RelayCommand(ThucHienLuuDeThi);
            ThemDeThiMoiCommand = new RelayCommand(KhoiTaoDeThiMoi);
            ChonTatCaCauHoiCommand = new RelayCommand(() => SetAllSelection(true));
            BoChonTatCaCommand = new RelayCommand(() => SetAllSelection(false));

            KhoiTaoDeThiMoi();
            TaiDuLieuDeThi();
        }

        private void SetAllSelection(bool select)
        {
            foreach (var item in NgânHangCauHoiChon)
                item.IsSelected = select;
            ThucHienCapNhatThongKeCauHoi();
        }

        private void KhoiTaoDeThiMoi()
        {
            CurrentDeThi = new BaiTapDeThiDTO
            {
                TenBaiTapDeThi = "Đề thi thử mới",
                LoaiHinh = "ThiThu",
                ThoiGianLamBaiPhut = 45,
                DiemDat = 5.0,
                TongDiem = 10.0,
                XaoTronCauHoi = true,
                DaPhatHanh = true
            };
            ThongBaoLoi = string.Empty;
        }

        private void TaiDuLieuDeThi()
        {
            var ds = BaiTapDeThiDAL.Instance.LayDanhSachDeThi();
            DanhSachDeThiHienCo.Clear();
            foreach (var d in ds) DanhSachDeThiHienCo.Add(d);

            var dmBUS = new DanhMucBUS();
            DanhSachTrinhDo.Clear();
            foreach (var td in dmBUS.LayDanhSachTrinhDo()) DanhSachTrinhDo.Add(td);

            NapCauHoiTheoTrinhDo();
        }

        private void NapCauHoiTheoTrinhDo()
        {
            string trinhDo = FilterTrinhDo == "All" ? "B1" : FilterTrinhDo;
            var dsCauHoi = CauHoiDAL.Instance.LayDanhSachCauHoiTheoTrinhDo(trinhDo, 50);

            NgânHangCauHoiChon.Clear();
            foreach (var ch in dsCauHoi)
            {
                NgânHangCauHoiChon.Add(new QuestionSelectionItem { CauHoi = ch, Diem = 0.25, IsSelected = false });
            }

            ThucHienCapNhatThongKeCauHoi();
        }

        private void ThucHienCapNhatThongKeCauHoi()
        {
            OnPropertyChanged(nameof(TongSoCauDaChon));
            OnPropertyChanged(nameof(TongDiemDeThi));
            CurrentDeThi.TongDiem = TongDiemDeThi;
            CurrentDeThi.SoLuongCauHoi = TongSoCauDaChon;
        }

        private void ThucHienLuuDeThi()
        {
            if (string.IsNullOrWhiteSpace(CurrentDeThi.TenBaiTapDeThi))
            {
                ThongBaoLoi = "Vui lòng nhập tên đề thi.";
                return;
            }

            if (CurrentDeThi.LoaiHinh == "ThiThu" && CurrentDeThi.ThoiGianLamBaiPhut <= 0)
            {
                ThongBaoLoi = "Đề thi thử (Mock Test) bắt buộc thời gian làm bài phải > 0 phút.";
                return;
            }

            if (TongSoCauDaChon == 0)
            {
                ThongBaoLoi = "Vui lòng chọn ít nhất 1 câu hỏi từ ngân hàng đưa vào đề thi.";
                return;
            }

            if (CurrentDeThi.DiemDat > TongDiemDeThi)
            {
                ThongBaoLoi = "Điểm đạt yêu cầu không được vượt quá tổng điểm bài thi.";
                return;
            }

            ThongBaoLoi = string.Empty;

            var dsChiTiet = NgânHangCauHoiChon
                .Where(x => x.IsSelected)
                .Select((item, idx) => (item.CauHoi.MaCauHoi, ThuTu: idx + 1, item.Diem))
                .ToList();

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;

            bool thanhCong = BaiTapDeThiDAL.Instance.LuuDeThiVaCauHoi(
                CurrentDeThi, 
                dsChiTiet, 
                userId, 
                out string err);

            if (thanhCong)
            {
                TaiDuLieuDeThi();
                GUI.Views.Common.ThongBaoDialog.ThanhCong("Cấu hình & xuất bản đề thi thành công với kiểm soát Transaction!", "Thành công");
            }
            else
            {
                ThongBaoLoi = "Lỗi khi lưu đề thi: " + err;
            }
        }
    }
}
