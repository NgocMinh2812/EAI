using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using DTO;
using DAL;
using BUS;
using GUI.ViewModels.Common;

namespace GUI.ViewModels.GiangVien
{
    public class NganHangCauHoiViewModel : ViewModelBase
    {
        private BoCauHoiDTO? _selectedBoCauHoi;
        private CauHoiKhaoSatDTO _currentCauHoi = new();
        private MediaPlayer _mediaPlayer = new();
        private bool _isPlayingAudio;
        private string _thongBaoLoi = string.Empty;
        private string _thongBaoThanhCong = string.Empty;

        // Bộ lọc bộ câu hỏi
        private string _tuKhoaTimKiemBo = string.Empty;
        private string _filterNguonTaoBo = "All";

        // Bộ lọc câu hỏi bên trong bộ
        private string _tuKhoaTimKiem = string.Empty;
        private string _filterKyNang = "All";
        private string _filterDoKho = "All";

        public ObservableCollection<BoCauHoiDTO> DanhSachBoCauHoi { get; } = new();
        public ObservableCollection<CauHoiKhaoSatDTO> DanhSachCauHoi { get; } = new();
        public ObservableCollection<LuaChonDapAnDTO> DanhSachDapAn { get; } = new();
        public ObservableCollection<TrinhDoDTO> DanhSachTrinhDo { get; } = new();
        public ObservableCollection<ChuDeDTO> DanhSachChuDe { get; } = new();

        public BoCauHoiDTO? SelectedBoCauHoi
        {
            get => _selectedBoCauHoi;
            set
            {
                if (SetProperty(ref _selectedBoCauHoi, value))
                {
                    OnPropertyChanged(nameof(TieuDeBoHienTai));
                    OnPropertyChanged(nameof(IsBoCuTheDuocChon));
                    NapDanhSachCauHoi();
                }
            }
        }

        public bool IsBoCuTheDuocChon => SelectedBoCauHoi != null && SelectedBoCauHoi.MaBoCauHoi > 0;

        public string TieuDeBoHienTai => SelectedBoCauHoi != null 
            ? SelectedBoCauHoi.TenBoCauHoi 
            : "Toàn bộ câu hỏi trong ngân hàng";

        public CauHoiKhaoSatDTO CurrentCauHoi
        {
            get => _currentCauHoi;
            set
            {
                if (SetProperty(ref _currentCauHoi, value))
                {
                    NapDapAnChoCauHoiHienTai();
                    OnPropertyChanged(nameof(CurrentCauHoi));
                }
            }
        }

        public string ThongBaoLoi
        {
            get => _thongBaoLoi;
            set => SetProperty(ref _thongBaoLoi, value);
        }

        public string ThongBaoThanhCong
        {
            get => _thongBaoThanhCong;
            set => SetProperty(ref _thongBaoThanhCong, value);
        }

        public bool IsPlayingAudio
        {
            get => _isPlayingAudio;
            set => SetProperty(ref _isPlayingAudio, value);
        }

        public string TuKhoaTimKiemBo
        {
            get => _tuKhoaTimKiemBo;
            set
            {
                if (SetProperty(ref _tuKhoaTimKiemBo, value))
                    NapDanhSachBoCauHoi();
            }
        }

        public string FilterNguonTaoBo
        {
            get => _filterNguonTaoBo;
            set
            {
                if (SetProperty(ref _filterNguonTaoBo, value))
                    NapDanhSachBoCauHoi();
            }
        }

        public string TuKhoaTimKiem
        {
            get => _tuKhoaTimKiem;
            set
            {
                if (SetProperty(ref _tuKhoaTimKiem, value))
                    NapDanhSachCauHoi();
            }
        }

        public string FilterKyNang
        {
            get => _filterKyNang;
            set
            {
                if (SetProperty(ref _filterKyNang, value))
                    NapDanhSachCauHoi();
            }
        }

        public string FilterDoKho
        {
            get => _filterDoKho;
            set
            {
                if (SetProperty(ref _filterDoKho, value))
                    NapDanhSachCauHoi();
            }
        }

        public ICommand ChonBoCauHoiCommand { get; }
        public ICommand TaoBoCauHoiMoiCommand { get; }
        public ICommand DoiTenBoCauHoiCommand { get; }
        public ICommand XoaBoCauHoiCommand { get; }
        public ICommand GoCauHoiKhoiBoCommand { get; }
        public ICommand XoaCauHoiVinhVienCommand { get; }
        public ICommand ThemDapAnCommand { get; }
        public ICommand XoaDapAnCommand { get; }
        public ICommand ChonDapAnDungCommand { get; }
        public ICommand ChonAudioCommand { get; }
        public ICommand TogglePlayAudioCommand { get; }
        public ICommand LuuCauHoiCommand { get; }
        public ICommand ThemCauHoiMoiCommand { get; }

        public NganHangCauHoiViewModel()
        {
            ChonBoCauHoiCommand = new RelayCommand(p => ChonBo(p as BoCauHoiDTO));
            TaoBoCauHoiMoiCommand = new RelayCommand(_ => ThucHienTaoBoMoi());
            DoiTenBoCauHoiCommand = new RelayCommand(p => ThucHienDoiTenBo(p as BoCauHoiDTO));
            XoaBoCauHoiCommand = new RelayCommand(p => ThucHienXoaBo(p as BoCauHoiDTO));
            GoCauHoiKhoiBoCommand = new RelayCommand(p => ThucHienGoCauHoiKhoiBo(p as CauHoiKhaoSatDTO));
            XoaCauHoiVinhVienCommand = new RelayCommand(p => ThucHienXoaCauHoiVinhVien(p as CauHoiKhaoSatDTO));
            ThemDapAnCommand = new RelayCommand(_ => ThucHienThemDapAn());
            XoaDapAnCommand = new RelayCommand(p => ThucHienXoaDapAn(p as LuaChonDapAnDTO));
            ChonDapAnDungCommand = new RelayCommand(p => ThucHienChonDapAnDung(p as LuaChonDapAnDTO));
            ChonAudioCommand = new RelayCommand(_ => ThucHienChonAudio());
            TogglePlayAudioCommand = new RelayCommand(_ => ThucHienToggleAudio());
            LuuCauHoiCommand = new RelayCommand(_ => ThucHienLuuCauHoi());
            ThemCauHoiMoiCommand = new RelayCommand(_ => KhoiTaoCauHoiMoi());

            KhoiTaoCauHoiMoi();
            NapDanhMuc();
            NapDanhSachBoCauHoi();
        }

        public void NapDanhMuc()
        {
            var dmBUS = new DanhMucBUS();
            DanhSachTrinhDo.Clear();
            foreach (var td in dmBUS.LayDanhSachTrinhDo()) DanhSachTrinhDo.Add(td);

            DanhSachChuDe.Clear();
            foreach (var cd in dmBUS.LayDanhSachChuDe()) DanhSachChuDe.Add(cd);
        }

        public void NapDanhSachBoCauHoi()
        {
            var currentMaBo = SelectedBoCauHoi?.MaBoCauHoi;
            DanhSachBoCauHoi.Clear();

            // 1. Mục mặc định: Toàn bộ câu hỏi
            var itemAll = new BoCauHoiDTO
            {
                MaBoCauHoi = 0,
                TenBoCauHoi = "Toàn bộ câu hỏi hệ thống",
                MoTa = "Xem và quản lý tất cả câu hỏi lưu trữ trong ngân hàng",
                NguonTao = "HeThong",
                TenNguon = "Tất cả nguồn"
            };
            DanhSachBoCauHoi.Add(itemAll);

            // 2. Danh sách các bộ câu hỏi từ CSDL
            var ds = BoCauHoiBUS.Instance.LayDanhSachBoCauHoi(TuKhoaTimKiemBo, FilterNguonTaoBo);
            foreach (var b in ds)
            {
                DanhSachBoCauHoi.Add(b);
            }

            // Giữ lại bộ đang chọn nếu còn tồn tại
            if (currentMaBo.HasValue)
            {
                SelectedBoCauHoi = DanhSachBoCauHoi.FirstOrDefault(x => x.MaBoCauHoi == currentMaBo.Value) ?? itemAll;
            }
            else
            {
                SelectedBoCauHoi = itemAll;
            }
        }

        public void ChonBo(BoCauHoiDTO? bo)
        {
            if (bo == null) return;
            SelectedBoCauHoi = bo;
        }

        public void NapDanhSachCauHoi()
        {
            DanhSachCauHoi.Clear();
            System.Collections.Generic.List<CauHoiKhaoSatDTO> ds;

            if (SelectedBoCauHoi == null || SelectedBoCauHoi.MaBoCauHoi == 0)
            {
                ds = BoCauHoiBUS.Instance.LayTatCaCauHoi(TuKhoaTimKiem, FilterKyNang, FilterDoKho, 100);
            }
            else
            {
                ds = BoCauHoiBUS.Instance.LayDanhSachCauHoiTheoBo(SelectedBoCauHoi.MaBoCauHoi, TuKhoaTimKiem, FilterKyNang, FilterDoKho);
            }

            foreach (var ch in ds)
            {
                DanhSachCauHoi.Add(ch);
            }

            if (DanhSachCauHoi.Count > 0)
            {
                ChonCauHoiDeSua(DanhSachCauHoi[0]);
            }
            else
            {
                KhoiTaoCauHoiMoi();
            }
        }

        public void ChonCauHoiDeSua(CauHoiKhaoSatDTO? ch)
        {
            if (ch == null) return;
            CurrentCauHoi = ch;
            NapDapAnChoCauHoiHienTai();
        }

        private void NapDapAnChoCauHoiHienTai()
        {
            DanhSachDapAn.Clear();
            if (_currentCauHoi != null && _currentCauHoi.DanhSachLuaChon != null && _currentCauHoi.DanhSachLuaChon.Count > 0)
            {
                foreach (var opt in _currentCauHoi.DanhSachLuaChon)
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO
                    {
                        MaLuaChon = opt.MaLuaChon,
                        MaCauHoi = opt.MaCauHoi,
                        KyHieuLuaChon = opt.KyHieuLuaChon,
                        NoiDungLuaChon = opt.NoiDungLuaChon,
                        LaDapAnDung = opt.LaDapAnDung
                    });
                }
            }
            else
            {
                DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "", LaDapAnDung = true });
                DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "", LaDapAnDung = false });
                DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "", LaDapAnDung = false });
                DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = "", LaDapAnDung = false });
            }
            ThongBaoLoi = string.Empty;
            ThongBaoThanhCong = string.Empty;
            OnPropertyChanged(nameof(DanhSachDapAn));
        }

        private void DongBoDapAnVaoCurrentCauHoi()
        {
            if (_currentCauHoi == null) return;
            _currentCauHoi.DanhSachLuaChon = DanhSachDapAn.Select(opt => new LuaChonDapAnDTO
            {
                MaLuaChon = opt.MaLuaChon,
                MaCauHoi = opt.MaCauHoi,
                KyHieuLuaChon = opt.KyHieuLuaChon,
                NoiDungLuaChon = opt.NoiDungLuaChon,
                LaDapAnDung = opt.LaDapAnDung
            }).ToList();
        }

        private void KhoiTaoCauHoiMoi()
        {
            var newCh = new CauHoiKhaoSatDTO
            {
                LoaiCauHoi = "TracNghiem",
                KyNang = "Doc",
                DoKho = "TrungBinh",
                MaTrinhDo = "B1",
                DaKiemDuyet = true
            };
            newCh.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "", LaDapAnDung = true });
            newCh.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "", LaDapAnDung = false });
            newCh.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "", LaDapAnDung = false });
            newCh.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = "", LaDapAnDung = false });

            CurrentCauHoi = newCh;
            NapDapAnChoCauHoiHienTai();
        }

        private void ThucHienThemDapAn()
        {
            char nextChar = (char)('A' + DanhSachDapAn.Count);
            if (nextChar <= 'Z')
            {
                DanhSachDapAn.Add(new LuaChonDapAnDTO
                {
                    KyHieuLuaChon = nextChar.ToString(),
                    NoiDungLuaChon = "",
                    LaDapAnDung = false
                });
                DongBoDapAnVaoCurrentCauHoi();
            }
        }

        private void ThucHienXoaDapAn(LuaChonDapAnDTO? item)
        {
            if (item != null && DanhSachDapAn.Count > 2)
            {
                DanhSachDapAn.Remove(item);
                for (int i = 0; i < DanhSachDapAn.Count; i++)
                {
                    DanhSachDapAn[i].KyHieuLuaChon = ((char)('A' + i)).ToString();
                }
                if (!DanhSachDapAn.Any(x => x.LaDapAnDung))
                {
                    DanhSachDapAn[0].LaDapAnDung = true;
                }
                DongBoDapAnVaoCurrentCauHoi();
            }
        }

        private void ThucHienChonDapAnDung(LuaChonDapAnDTO? selected)
        {
            if (selected == null) return;
            foreach (var opt in DanhSachDapAn)
            {
                opt.LaDapAnDung = (opt == selected);
            }
            DongBoDapAnVaoCurrentCauHoi();
            OnPropertyChanged(nameof(DanhSachDapAn));
        }

        private void ThucHienChonAudio()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Tệp âm thanh (*.mp3;*.wav;*.m4a)|*.mp3;*.wav;*.m4a",
                Title = "Chọn file Audio câu hỏi nghe"
            };

            if (dlg.ShowDialog() == true)
            {
                CurrentCauHoi.AudioPath = dlg.FileName;
                OnPropertyChanged(nameof(CurrentCauHoi));
            }
        }

        private void ThucHienToggleAudio()
        {
            if (string.IsNullOrWhiteSpace(CurrentCauHoi.AudioPath)) return;

            try
            {
                if (IsPlayingAudio)
                {
                    _mediaPlayer.Pause();
                    IsPlayingAudio = false;
                }
                else
                {
                    _mediaPlayer.Open(new Uri(CurrentCauHoi.AudioPath, UriKind.RelativeOrAbsolute));
                    _mediaPlayer.Play();
                    IsPlayingAudio = true;
                }
            }
            catch (Exception ex)
            {
                ThongBaoLoi = $"Lỗi phát audio: {ex.Message}";
            }
        }

        private void ThucHienLuuCauHoi()
        {
            if (string.IsNullOrWhiteSpace(CurrentCauHoi.NoiDungCauHoi))
            {
                ThongBaoLoi = "Vui lòng nhập nội dung câu hỏi.";
                ThongBaoThanhCong = string.Empty;
                return;
            }

            if (CurrentCauHoi.LoaiCauHoi == "TracNghiem")
            {
                if (DanhSachDapAn.Count < 2)
                {
                    ThongBaoLoi = "Câu hỏi trắc nghiệm phải có ít nhất 2 lựa chọn.";
                    ThongBaoThanhCong = string.Empty;
                    return;
                }

                int soDapAnDung = DanhSachDapAn.Count(x => x.LaDapAnDung);
                if (soDapAnDung != 1)
                {
                    ThongBaoLoi = "Bắt buộc phải chọn đúng duy nhất 1 phương án là đáp án đúng.";
                    ThongBaoThanhCong = string.Empty;
                    return;
                }

                if (DanhSachDapAn.Any(x => string.IsNullOrWhiteSpace(x.NoiDungLuaChon)))
                {
                    ThongBaoLoi = "Các phương án trả lời không được để trống nội dung.";
                    ThongBaoThanhCong = string.Empty;
                    return;
                }
            }

            ThongBaoLoi = string.Empty;
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;

            DongBoDapAnVaoCurrentCauHoi();

            bool thanhCong = CauHoiDAL.Instance.LuuCauHoiVaDapAn(
                CurrentCauHoi, 
                DanhSachDapAn.ToList(), 
                userId, 
                out string err);

            if (thanhCong)
            {
                // Nếu đang chọn một bộ cụ thể, tự động liên kết câu hỏi này vào bộ
                if (SelectedBoCauHoi != null && SelectedBoCauHoi.MaBoCauHoi > 0)
                {
                    BoCauHoiBUS.Instance.ThemCauHoiVaoBo(SelectedBoCauHoi.MaBoCauHoi, CurrentCauHoi.MaCauHoi, out _);
                }

                ThongBaoThanhCong = "Đã lưu câu hỏi vào ngân hàng thành công!";
                NapDanhSachCauHoi();
                NapDanhSachBoCauHoi();
            }
            else
            {
                ThongBaoLoi = "Lỗi khi lưu câu hỏi: " + err;
                ThongBaoThanhCong = string.Empty;
            }
        }

        private void ThucHienTaoBoMoi()
        {
            var tenBo = $"Bộ câu hỏi mới {DateTime.Now:dd/MM/yyyy HH:mm}";
            var bo = new BoCauHoiDTO
            {
                TenBoCauHoi = tenBo,
                NguonTao = "ThuCong",
                TenNguon = "Tạo thủ công",
                NguoiTao = SessionHelper.CurrentUser?.MaNguoiDung ?? 1
            };

            // Tạo bộ mới với 1 câu hỏi mẫu mặc định
            var dsMau = new System.Collections.Generic.List<CauHoiKhaoSatDTO>
            {
                new CauHoiKhaoSatDTO
                {
                    NoiDungCauHoi = "Câu hỏi đầu tiên trong bộ câu hỏi",
                    LoaiCauHoi = "TracNghiem",
                    KyNang = "Doc",
                    DoKho = "TrungBinh",
                    MaTrinhDo = "B1",
                    DanhSachLuaChon = new System.Collections.Generic.List<LuaChonDapAnDTO>
                    {
                        new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "Phương án A", LaDapAnDung = true },
                        new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "Phương án B", LaDapAnDung = false }
                    }
                }
            };

            bool ok = BoCauHoiBUS.Instance.LuuBoCauHoi(bo, dsMau, bo.NguoiTao ?? 1, out string err);
            if (ok)
            {
                ThongBaoThanhCong = $"Đã tạo bộ câu hỏi \"{bo.TenBoCauHoi}\" thành công!";
                NapDanhSachBoCauHoi();
                SelectedBoCauHoi = DanhSachBoCauHoi.FirstOrDefault(x => x.MaBoCauHoi == bo.MaBoCauHoi);
            }
            else
            {
                ThongBaoLoi = "Lỗi khi tạo bộ câu hỏi: " + err;
            }
        }

        private void ThucHienDoiTenBo(BoCauHoiDTO? bo)
        {
            if (bo == null || bo.MaBoCauHoi == 0) return;

            // Mở prompt đơn giản đổi tên
            string tenMoi = Microsoft.VisualBasic.Interaction.InputBox("Nhập tên mới cho bộ câu hỏi:", "Đổi tên bộ câu hỏi", bo.TenBoCauHoi);
            if (!string.IsNullOrWhiteSpace(tenMoi) && tenMoi != bo.TenBoCauHoi)
            {
                bool ok = BoCauHoiBUS.Instance.DoiTenBoCauHoi(bo.MaBoCauHoi, tenMoi.Trim(), out string err);
                if (ok)
                {
                    bo.TenBoCauHoi = tenMoi.Trim();
                    OnPropertyChanged(nameof(SelectedBoCauHoi));
                    OnPropertyChanged(nameof(TieuDeBoHienTai));
                    NapDanhSachBoCauHoi();
                    ThongBaoThanhCong = "Đổi tên bộ câu hỏi thành công!";
                }
                else
                {
                    ThongBaoLoi = "Lỗi khi đổi tên: " + err;
                }
            }
        }

        private void ThucHienXoaBo(BoCauHoiDTO? bo)
        {
            if (bo == null || bo.MaBoCauHoi == 0) return;

            var result = System.Windows.MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa bộ câu hỏi \"{bo.TenBoCauHoi}\"?\n\nLưu ý: Các câu hỏi bên trong vẫn được bảo toàn an toàn trong Ngân hàng câu hỏi để sử dụng cho các bài tập / đề thi khác.",
                "Xác nhận xóa bộ câu hỏi",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                bool ok = BoCauHoiBUS.Instance.XoaBoCauHoi(bo.MaBoCauHoi, out string err);
                if (ok)
                {
                    ThongBaoThanhCong = $"Đã xóa bộ câu hỏi \"{bo.TenBoCauHoi}\" thành công!";
                    NapDanhSachBoCauHoi();
                }
                else
                {
                    ThongBaoLoi = "Lỗi khi xóa bộ câu hỏi: " + err;
                }
            }
        }

        private void ThucHienGoCauHoiKhoiBo(CauHoiKhaoSatDTO? ch)
        {
            if (ch == null || SelectedBoCauHoi == null || SelectedBoCauHoi.MaBoCauHoi == 0) return;

            var result = System.Windows.MessageBox.Show(
                $"Bạn có muốn gỡ câu hỏi #{ch.ThuTuCauHoi} khỏi bộ \"{SelectedBoCauHoi.TenBoCauHoi}\"?\n(Câu hỏi vẫn được lưu trữ đầy đủ trong ngân hàng)",
                "Xác nhận gỡ câu hỏi khỏi bộ",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                bool ok = BoCauHoiBUS.Instance.XoaCauHoiKhoiBo(SelectedBoCauHoi.MaBoCauHoi, ch.MaCauHoi, out string err);
                if (ok)
                {
                    ThongBaoThanhCong = "Đã gỡ câu hỏi khỏi bộ thành công!";
                    NapDanhSachCauHoi();
                    NapDanhSachBoCauHoi();
                }
                else
                {
                    ThongBaoLoi = "Lỗi khi gỡ câu hỏi: " + err;
                }
            }
        }

        private void ThucHienXoaCauHoiVinhVien(CauHoiKhaoSatDTO? ch)
        {
            if (ch == null || ch.MaCauHoi <= 0) return;

            var result = System.Windows.MessageBox.Show(
                $"CẢNH BÁO: Bạn có chắc chắn muốn XÓA câu hỏi #{ch.ThuTuCauHoi} khỏi toàn bộ hệ thống?",
                "Xác nhận xóa câu hỏi",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                bool ok = BoCauHoiBUS.Instance.XoaCauHoiVinhVien(ch.MaCauHoi, out string err);
                if (ok)
                {
                    ThongBaoThanhCong = "Đã xóa câu hỏi khỏi hệ thống!";
                    NapDanhSachCauHoi();
                    NapDanhSachBoCauHoi();
                    KhoiTaoCauHoiMoi();
                }
                else
                {
                    ThongBaoLoi = err;
                    System.Windows.MessageBox.Show(err, "Không thể xóa", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
            }
        }
    }
}
