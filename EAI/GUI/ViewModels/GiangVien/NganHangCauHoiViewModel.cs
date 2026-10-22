using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private readonly System.Windows.Threading.DispatcherTimer _audioTimer;
        private bool _isPlayingAudio;
        private bool _isSeekingAudio;
        private string? _loadedAudioPath;
        private double _audioCurrentSeconds;
        private double _audioTotalSeconds = 100;
        private string _audioCurrentTimeText = "00:00";
        private string _audioTotalTimeText = "00:00";
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
                string? oldAudio = _currentCauHoi?.AudioPath?.Trim();
                if (_currentCauHoi != null)
                {
                    _currentCauHoi.PropertyChanged -= OnCurrentCauHoiPropertyChanged;
                }
                if (SetProperty(ref _currentCauHoi, value ?? new()))
                {
                    if (_currentCauHoi != null)
                    {
                        _currentCauHoi.PropertyChanged += OnCurrentCauHoiPropertyChanged;
                    }
                    NapDapAnChoCauHoiHienTai();
                    OnPropertyChanged(nameof(CurrentCauHoi));
                    string? newAudio = _currentCauHoi?.AudioPath?.Trim();
                    if (!string.Equals(oldAudio, newAudio, StringComparison.OrdinalIgnoreCase))
                    {
                        DungAudio();
                        ResetAudioTimeline();
                    }
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

        public double AudioCurrentSeconds
        {
            get => _audioCurrentSeconds;
            set => SetProperty(ref _audioCurrentSeconds, value);
        }

        public double AudioTotalSeconds
        {
            get => _audioTotalSeconds;
            set => SetProperty(ref _audioTotalSeconds, value);
        }

        public string AudioCurrentTimeText
        {
            get => _audioCurrentTimeText;
            set => SetProperty(ref _audioCurrentTimeText, value);
        }

        public string AudioTotalTimeText
        {
            get => _audioTotalTimeText;
            set => SetProperty(ref _audioTotalTimeText, value);
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

            _audioTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            _audioTimer.Tick += (s, e) =>
            {
                if (!_isSeekingAudio && _mediaPlayer.NaturalDuration.HasTimeSpan && _isPlayingAudio)
                {
                    _audioCurrentSeconds = _mediaPlayer.Position.TotalSeconds;
                    OnPropertyChanged(nameof(AudioCurrentSeconds));
                    AudioCurrentTimeText = FormatTimeSpan(_mediaPlayer.Position);
                }
            };

            _mediaPlayer.MediaOpened += (s, e) =>
            {
                System.Windows.Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                    {
                        double total = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        AudioTotalSeconds = total > 0 ? total : 100;
                        AudioTotalTimeText = FormatTimeSpan(_mediaPlayer.NaturalDuration.TimeSpan);
                    }
                }));
            };

            _mediaPlayer.MediaEnded += (s, e) =>
            {
                System.Windows.Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try { _mediaPlayer.Stop(); } catch { }
                    try { _mediaPlayer.Position = TimeSpan.Zero; } catch { }
                    IsPlayingAudio = false;
                    _audioTimer.Stop();
                    _audioCurrentSeconds = 0;
                    OnPropertyChanged(nameof(AudioCurrentSeconds));
                    AudioCurrentTimeText = "00:00";
                }));
            };

            _mediaPlayer.MediaFailed += (s, e) =>
            {
                System.Windows.Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try { _mediaPlayer.Stop(); } catch { }
                    IsPlayingAudio = false;
                    _audioTimer.Stop();
                    ThongBaoLoi = $"Không thể phát tệp âm thanh: {e.ErrorException?.Message ?? "Lỗi định dạng tệp."}";
                }));
            };

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

        public void NapDanhSachBoCauHoi(int? targetCauHoiId = null)
        {
            var currentMaBo = SelectedBoCauHoi?.MaBoCauHoi;
            DanhSachBoCauHoi.Clear();

            // 1. Mục mặc định: Toàn bộ câu hỏi
            int tongSoCau = BoCauHoiBUS.Instance.DemTongSoCauHoi();
            var itemAll = new BoCauHoiDTO
            {
                MaBoCauHoi = 0,
                TenBoCauHoi = "Toàn bộ câu hỏi hệ thống",
                MoTa = "Xem và quản lý tất cả câu hỏi lưu trữ trong ngân hàng",
                NguonTao = "HeThong",
                TenNguon = "Tất cả nguồn",
                SoLuongCauHoi = tongSoCau
            };
            DanhSachBoCauHoi.Add(itemAll);

            // 2. Danh sách các bộ câu hỏi từ CSDL
            var ds = BoCauHoiBUS.Instance.LayDanhSachBoCauHoi(TuKhoaTimKiemBo, FilterNguonTaoBo);
            foreach (var b in ds)
            {
                DanhSachBoCauHoi.Add(b);
            }

            var nextSelectedBo = (currentMaBo.HasValue
                ? DanhSachBoCauHoi.FirstOrDefault(x => x.MaBoCauHoi == currentMaBo.Value)
                : null) ?? itemAll;

            if (_selectedBoCauHoi?.MaBoCauHoi == nextSelectedBo.MaBoCauHoi)
            {
                _selectedBoCauHoi = nextSelectedBo;
                OnPropertyChanged(nameof(SelectedBoCauHoi));
                OnPropertyChanged(nameof(TieuDeBoHienTai));
                OnPropertyChanged(nameof(IsBoCuTheDuocChon));
                NapDanhSachCauHoi(targetCauHoiId);
            }
            else
            {
                SelectedBoCauHoi = nextSelectedBo;
            }
        }

        public void ChonBo(BoCauHoiDTO? bo)
        {
            if (bo == null) return;
            SelectedBoCauHoi = bo;
        }

        public void NapDanhSachCauHoi(int? targetMaCauHoi = null)
        {
            int currentId = targetMaCauHoi ?? CurrentCauHoi?.MaCauHoi ?? -1;
            DanhSachCauHoi.Clear();
            System.Collections.Generic.List<CauHoiKhaoSatDTO> ds;

            if (SelectedBoCauHoi == null || SelectedBoCauHoi.MaBoCauHoi == 0)
            {
                ds = BoCauHoiBUS.Instance.LayTatCaCauHoi(TuKhoaTimKiem, FilterKyNang, FilterDoKho, 500);
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
                var found = currentId >= 0 ? DanhSachCauHoi.FirstOrDefault(x => x.MaCauHoi == currentId) : null;
                ChonCauHoiDeSua(found ?? DanhSachCauHoi[0]);
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

        private void OnCurrentCauHoiPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CauHoiKhaoSatDTO.LoaiCauHoi))
            {
                CapNhatDapAnTheoLoaiCauHoi();
            }
        }

        private void CapNhatDapAnTheoLoaiCauHoi()
        {
            if (_currentCauHoi == null) return;

            if (_currentCauHoi.IsDangDungSai)
            {
                if (DanhSachDapAn.Count != 2 || (DanhSachDapAn[0].NoiDungLuaChon != "True" && DanhSachDapAn[0].NoiDungLuaChon != "Đúng"))
                {
                    DanhSachDapAn.Clear();
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "True", LaDapAnDung = true });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "False", LaDapAnDung = false });
                }
            }
            else if (_currentCauHoi.IsDangTrueFalseNotGiven)
            {
                if (DanhSachDapAn.Count != 3 || (DanhSachDapAn[0].NoiDungLuaChon != "True" && DanhSachDapAn[0].NoiDungLuaChon != "Đúng"))
                {
                    DanhSachDapAn.Clear();
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "True", LaDapAnDung = true });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "False", LaDapAnDung = false });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "Not Given", LaDapAnDung = false });
                }
            }
            else if (_currentCauHoi.IsDangNhapVanBan)
            {
                if (!DanhSachDapAn.Any())
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "KEY", NoiDungLuaChon = "", LaDapAnDung = true });
                }
                else
                {
                    DanhSachDapAn[0].KyHieuLuaChon = "KEY";
                    DanhSachDapAn[0].LaDapAnDung = true;
                    while (DanhSachDapAn.Count > 1) DanhSachDapAn.RemoveAt(DanhSachDapAn.Count - 1);
                }
            }
            else if (_currentCauHoi.IsDangTuLuan)
            {
                if (!DanhSachDapAn.Any())
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "RUB", NoiDungLuaChon = "Tiêu chí chấm: Task Response, Coherence & Cohesion, Lexical Resource, Grammatical Range.", LaDapAnDung = true });
                }
                else
                {
                    DanhSachDapAn[0].KyHieuLuaChon = "RUB";
                    DanhSachDapAn[0].LaDapAnDung = true;
                    while (DanhSachDapAn.Count > 1) DanhSachDapAn.RemoveAt(DanhSachDapAn.Count - 1);
                }
            }
            else if (_currentCauHoi.IsDangTracNghiem)
            {
                if (DanhSachDapAn.Count < 2)
                {
                    DanhSachDapAn.Clear();
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "", LaDapAnDung = true });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "", LaDapAnDung = false });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "", LaDapAnDung = false });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = "", LaDapAnDung = false });
                }
            }

            DongBoDapAnVaoCurrentCauHoi();
            OnPropertyChanged(nameof(DanhSachDapAn));
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
                if (_currentCauHoi != null && _currentCauHoi.IsDangNhapVanBan)
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "KEY", NoiDungLuaChon = "", LaDapAnDung = true });
                }
                else if (_currentCauHoi != null && _currentCauHoi.IsDangTuLuan)
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "RUB", NoiDungLuaChon = "", LaDapAnDung = true });
                }
                else if (_currentCauHoi != null && _currentCauHoi.IsDangDungSai)
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "True", LaDapAnDung = true });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "False", LaDapAnDung = false });
                }
                else if (_currentCauHoi != null && _currentCauHoi.IsDangTrueFalseNotGiven)
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "True", LaDapAnDung = true });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "False", LaDapAnDung = false });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "Not Given", LaDapAnDung = false });
                }
                else
                {
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "", LaDapAnDung = true });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "", LaDapAnDung = false });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "", LaDapAnDung = false });
                    DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = "", LaDapAnDung = false });
                }
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

        public void KhoiTaoCauHoiMoi()
        {
            var newCh = new CauHoiKhaoSatDTO
            {
                MaCauHoi = -1,
                LoaiCauHoi = "TracNghiem",
                KyNang = "Doc",
                DoKho = "TrungBinh",
                MaTrinhDo = "B1",
                MaTrinhDoId = 3,
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
            if (item != null && DanhSachDapAn.Count > (CurrentCauHoi.IsDangDungSai ? 2 : 1))
            {
                DanhSachDapAn.Remove(item);
                for (int i = 0; i < DanhSachDapAn.Count; i++)
                {
                    DanhSachDapAn[i].KyHieuLuaChon = ((char)('A' + i)).ToString();
                }
                if (!DanhSachDapAn.Any(x => x.LaDapAnDung) && DanhSachDapAn.Count > 0)
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

        private string FormatTimeSpan(TimeSpan ts)
        {
            return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        public void BatDauTuaAudio()
        {
            _isSeekingAudio = true;
        }

        public void KetThucTuaAudio(double seconds)
        {
            _isSeekingAudio = false;
            try
            {
                _mediaPlayer.Position = TimeSpan.FromSeconds(seconds);
                _audioCurrentSeconds = seconds;
                AudioCurrentTimeText = FormatTimeSpan(_mediaPlayer.Position);
                OnPropertyChanged(nameof(AudioCurrentSeconds));
            }
            catch { }
        }

        public void ResetAudioTimeline()
        {
            _loadedAudioPath = null;
            _audioCurrentSeconds = 0;
            _audioTotalSeconds = 100;
            _audioCurrentTimeText = "00:00";
            _audioTotalTimeText = "00:00";
            OnPropertyChanged(nameof(AudioCurrentSeconds));
            OnPropertyChanged(nameof(AudioTotalSeconds));
            OnPropertyChanged(nameof(AudioCurrentTimeText));
            OnPropertyChanged(nameof(AudioTotalTimeText));
        }

        public void DungAudio()
        {
            try
            {
                _audioTimer?.Stop();
                _mediaPlayer.Stop();
                _mediaPlayer.Position = TimeSpan.Zero;
            }
            catch { }
            finally
            {
                IsPlayingAudio = false;
            }
        }

        private void ThucHienChonAudio()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Tệp âm thanh (*.mp3;*.wav;*.m4a;*.ogg;*.aac)|*.mp3;*.wav;*.m4a;*.ogg;*.aac|Tất cả tệp (*.*)|*.*",
                Title = "Chọn file Audio câu hỏi nghe"
            };

            if (dlg.ShowDialog() == true)
            {
                DungAudio();
                ResetAudioTimeline();
                CurrentCauHoi.AudioPath = dlg.FileName;
                OnPropertyChanged(nameof(CurrentCauHoi));
            }
        }

        private void ThucHienToggleAudio()
        {
            if (string.IsNullOrWhiteSpace(CurrentCauHoi?.AudioPath)) return;

            try
            {
                if (IsPlayingAudio)
                {
                    _mediaPlayer.Pause();
                    _audioTimer.Stop();
                    IsPlayingAudio = false;
                }
                else
                {
                    string path = CurrentCauHoi.AudioPath.Trim();
                    Uri? uri = null;
                    if (Uri.TryCreate(path, UriKind.Absolute, out var absUri) && (absUri.Scheme == Uri.UriSchemeHttp || absUri.Scheme == Uri.UriSchemeHttps || absUri.Scheme == Uri.UriSchemeFile))
                    {
                        uri = absUri;
                    }
                    else if (System.IO.File.Exists(path))
                    {
                        uri = new Uri(System.IO.Path.GetFullPath(path));
                    }

                    if (uri == null)
                    {
                        ThongBaoLoi = "Không tìm thấy tệp âm thanh này trên hệ thống máy tính.";
                        return;
                    }

                    if (_loadedAudioPath != path)
                    {
                        _mediaPlayer.Open(uri);
                        _loadedAudioPath = path;
                        _audioCurrentSeconds = 0;
                        AudioCurrentTimeText = "00:00";
                    }

                    _mediaPlayer.Play();
                    _audioTimer.Start();
                    IsPlayingAudio = true;
                }
            }
            catch (Exception ex)
            {
                ThongBaoLoi = $"Lỗi phát audio: {ex.Message}";
                IsPlayingAudio = false;
                _audioTimer.Stop();
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

            // Kiểm tra tính hợp lệ và chuẩn hóa theo từng loại câu hỏi
            if (CurrentCauHoi.IsDangNhapVanBan)
            {
                if (!DanhSachDapAn.Any() || string.IsNullOrWhiteSpace(DanhSachDapAn[0].NoiDungLuaChon))
                {
                    ThongBaoLoi = "Vui lòng nhập đáp án chính xác (từ hoặc câu chuẩn) cho dạng câu hỏi này.";
                    ThongBaoThanhCong = string.Empty;
                    return;
                }
                DanhSachDapAn[0].LaDapAnDung = true;
                DanhSachDapAn[0].KyHieuLuaChon = "KEY";
                while (DanhSachDapAn.Count > 1) DanhSachDapAn.RemoveAt(DanhSachDapAn.Count - 1);
            }
            else if (CurrentCauHoi.IsDangTuLuan)
            {
                if (!DanhSachDapAn.Any() || string.IsNullOrWhiteSpace(DanhSachDapAn[0].NoiDungLuaChon))
                {
                    if (!DanhSachDapAn.Any())
                    {
                        DanhSachDapAn.Add(new LuaChonDapAnDTO
                        {
                            KyHieuLuaChon = "RUB",
                            NoiDungLuaChon = "Tiêu chí chấm: Task Response, Coherence & Cohesion, Lexical Resource, Grammatical Range.",
                            LaDapAnDung = true
                        });
                    }
                }
                DanhSachDapAn[0].LaDapAnDung = true;
                DanhSachDapAn[0].KyHieuLuaChon = "RUB";
                while (DanhSachDapAn.Count > 1) DanhSachDapAn.RemoveAt(DanhSachDapAn.Count - 1);
            }
            else if (CurrentCauHoi.IsDangLuaChon)
            {
                if (DanhSachDapAn.Count < 2)
                {
                    ThongBaoLoi = "Câu hỏi trắc nghiệm/lựa chọn phải có ít nhất 2 phương án.";
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
                int savedId = CurrentCauHoi.MaCauHoi;

                // Nếu đang chọn một bộ cụ thể, tự động liên kết câu hỏi này vào bộ
                if (SelectedBoCauHoi != null && SelectedBoCauHoi.MaBoCauHoi > 0)
                {
                    BoCauHoiBUS.Instance.ThemCauHoiVaoBo(SelectedBoCauHoi.MaBoCauHoi, savedId, out _);
                }

                ThongBaoThanhCong = "Đã lưu thay đổi câu hỏi vào ngân hàng thành công!";
                NapDanhSachBoCauHoi(savedId);
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
            ch ??= CurrentCauHoi;
            if (ch == null || SelectedBoCauHoi == null || SelectedBoCauHoi.MaBoCauHoi == 0) return;
            if (ch.MaCauHoi < 0) return;

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
            ch ??= CurrentCauHoi;
            if (ch == null) return;

            if (ch.MaCauHoi < 0)
            {
                KhoiTaoCauHoiMoi();
                return;
            }

            var result = System.Windows.MessageBox.Show(
                $"CẢNH BÁO: Bạn có chắc chắn muốn XÓA câu hỏi #{ch.ThuTuCauHoi} khỏi toàn bộ hệ thống?\n(Thao tác này sẽ gỡ câu hỏi khỏi tất cả bộ câu hỏi và cơ sở dữ liệu)",
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
