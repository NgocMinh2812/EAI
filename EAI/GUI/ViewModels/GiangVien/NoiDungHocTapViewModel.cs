using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Win32;
using DTO;
using BUS;
using GUI.ViewModels.Common;

namespace GUI.ViewModels.GiangVien
{
    public class NoiDungHocTapViewModel : ViewModelBase
    {
        private readonly DanhMucBUS _danhMucBUS = new();
        private BaiHocDTO _selectedBaiHoc = new();
        private string _thongBaoLoi = string.Empty;
        private string _tuKhoaTimKiem = string.Empty;
        private TrinhDoDTO? _selectedTrinhDoFilter;
        private ChuDeDTO? _selectedChuDeFilter;
        private string _selectedKyNangFilter = "All";

        public ObservableCollection<BaiHocDTO> DanhSachBaiHoc { get; set; } = new();
        public ObservableCollection<TrinhDoDTO> DanhSachTrinhDo { get; set; } = new();
        public ObservableCollection<ChuDeDTO> DanhSachChuDe { get; set; } = new();

        public BaiHocDTO SelectedBaiHoc
        {
            get => _selectedBaiHoc;
            set
            {
                if (value != null && !string.IsNullOrEmpty(value.NoiDung))
                {
                    value.NoiDung = KhoHocLieuSyncHelper.ChuyenDoiHtmlSangVanBan(value.NoiDung);
                }
                SetProperty(ref _selectedBaiHoc, value ?? new BaiHocDTO());
            }
        }

        public string ThongBaoLoi
        {
            get => _thongBaoLoi;
            set => SetProperty(ref _thongBaoLoi, value);
        }

        public string TuKhoaTimKiem
        {
            get => _tuKhoaTimKiem;
            set
            {
                if (SetProperty(ref _tuKhoaTimKiem, value))
                    LocDanhSach();
            }
        }

        public TrinhDoDTO? SelectedTrinhDoFilter
        {
            get => _selectedTrinhDoFilter;
            set
            {
                if (SetProperty(ref _selectedTrinhDoFilter, value))
                    LocDanhSach();
            }
        }

        public ChuDeDTO? SelectedChuDeFilter
        {
            get => _selectedChuDeFilter;
            set
            {
                if (SetProperty(ref _selectedChuDeFilter, value))
                    LocDanhSach();
            }
        }

        public string SelectedKyNangFilter
        {
            get => _selectedKyNangFilter;
            set
            {
                if (SetProperty(ref _selectedKyNangFilter, value))
                    LocDanhSach();
            }
        }

        public ICommand ChonAudioCommand { get; }
        public ICommand ChonAnhMinhHoaCommand { get; }
        public ICommand LuuBaiHocCommand { get; }
        public ICommand ToggleXuatBanCommand { get; }
        public ICommand ThemBaiHocMoiCommand { get; }
        public ICommand LamMoiCommand { get; }

        public NoiDungHocTapViewModel()
        {
            ChonAudioCommand = new RelayCommand(ThucHienChonAudio);
            ChonAnhMinhHoaCommand = new RelayCommand(ThucHienChonAnh);
            LuuBaiHocCommand = new RelayCommand(ThucHienLuuBaiHoc);
            ToggleXuatBanCommand = new RelayCommand(ThucHienToggleXuatBan);
            ThemBaiHocMoiCommand = new RelayCommand(ThucHienThemBaiHocMoi);
            LamMoiCommand = new RelayCommand(NapDuLieu);

            NapDuLieu();
        }

        public void NapDuLieu()
        {
            try
            {
                // Nạp Trình Độ kèm mục mặc định
                DanhSachTrinhDo.Clear();
                DanhSachTrinhDo.Add(new TrinhDoDTO { MaCapDo = "", TenTrinhDo = "Tất cả cấp độ (A1 - C1)" });
                var trinhDos = _danhMucBUS.LayDanhSachTrinhDo();
                foreach (var td in trinhDos)
                    DanhSachTrinhDo.Add(td);

                _selectedTrinhDoFilter = DanhSachTrinhDo[0];
                OnPropertyChanged(nameof(SelectedTrinhDoFilter));

                // Nạp Chủ Đề
                DanhSachChuDe.Clear();
                var chuDes = _danhMucBUS.LayDanhSachChuDe();
                foreach (var cd in chuDes)
                    DanhSachChuDe.Add(cd);

                LocDanhSach();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NoiDungHocTapViewModel.NapDuLieu] {ex.Message}");
                ThongBaoLoi = "Không thể tải danh mục bài học: " + ex.Message;
            }
        }

        private void LocDanhSach()
        {
            try
            {
                int? maChuDe = SelectedChuDeFilter?.MaChuDe > 0 ? SelectedChuDeFilter.MaChuDe : null;
                var ds = BaiHocBUS.LayDanhSachTuDatabase(maChuDe, TuKhoaTimKiem);

                if (SelectedTrinhDoFilter != null && !string.IsNullOrWhiteSpace(SelectedTrinhDoFilter.MaCapDo))
                {
                    ds = ds.Where(x => string.Equals(x.MaTrinhDo, SelectedTrinhDoFilter.MaCapDo, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(SelectedKyNangFilter) && SelectedKyNangFilter != "All")
                {
                    ds = ds.Where(x => string.Equals(x.LoaiBaiHoc, SelectedKyNangFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                DanhSachBaiHoc.Clear();
                foreach (var item in ds)
                {
                    if (!string.IsNullOrEmpty(item.NoiDung))
                    {
                        item.NoiDung = KhoHocLieuSyncHelper.ChuyenDoiHtmlSangVanBan(item.NoiDung);
                    }
                    DanhSachBaiHoc.Add(item);
                }

                if ((SelectedBaiHoc == null || SelectedBaiHoc.MaBaiHoc <= 0) && DanhSachBaiHoc.Count > 0)
                {
                    SelectedBaiHoc = DanhSachBaiHoc[0];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NoiDungHocTapViewModel.LocDanhSach] {ex.Message}");
            }
        }

        private void ThucHienThemBaiHocMoi()
        {
            SelectedBaiHoc = new BaiHocDTO
            {
                TieuDe = "Bài giảng mới",
                LoaiBaiHoc = "NguPhap",
                MaTrinhDo = "B1",
                ThoiGianUocTinhPhut = 20,
                DaXuatBan = false
            };
            ThongBaoLoi = string.Empty;
        }

        private void ThucHienChonAudio()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Tệp âm thanh (*.mp3;*.wav;*.m4a)|*.mp3;*.wav;*.m4a|Tất cả tệp (*.*)|*.*",
                Title = "Chọn file ghi âm Audio bài giảng"
            };

            if (dlg.ShowDialog() == true)
            {
                SelectedBaiHoc.AudioPath = dlg.FileName;
                OnPropertyChanged(nameof(SelectedBaiHoc));
            }
        }

        private void ThucHienChonAnh()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Tệp hình ảnh (*.png;*.jpg;*.jpeg;*.webp)|*.png;*.jpg;*.jpeg;*.webp",
                Title = "Chọn ảnh minh họa bài giảng"
            };

            if (dlg.ShowDialog() == true)
            {
                SelectedBaiHoc.AnhMinhHoaPath = dlg.FileName;
                OnPropertyChanged(nameof(SelectedBaiHoc));
            }
        }

        private void ThucHienLuuBaiHoc()
        {
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe))
            {
                ThongBaoLoi = "Vui lòng nhập tiêu đề bài học.";
                return;
            }

            if (SelectedBaiHoc.ThoiGianUocTinhPhut <= 0)
            {
                ThongBaoLoi = "Thời lượng ước tính phải lớn hơn 0 phút.";
                return;
            }

            ThongBaoLoi = string.Empty;
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
            SelectedBaiHoc.NguoiTao = userId;

            bool ok = BaiHocBUS.LuuBaiHoc(SelectedBaiHoc, out string err);
            if (ok)
            {
                GUI.Views.Common.ThongBaoDialog.ThanhCong("Lưu nội dung bài học thành công!", "Thông báo");
                LocDanhSach();
            }
            else
            {
                ThongBaoLoi = "Lỗi khi lưu bài học: " + err;
            }
        }

        private void ThucHienToggleXuatBan(object? obj)
        {
            if (obj is BaiHocDTO bh)
            {
                bh.DaXuatBan = !bh.DaXuatBan;
                BaiHocBUS.CapNhatTrangThaiXuatBan(bh.MaBaiHoc, bh.DaXuatBan);
                LocDanhSach();
            }
        }
    }
}
