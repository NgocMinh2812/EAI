using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BUS;
using DTO;
using GUI.ViewModels.Common;

namespace GUI.ViewModels.GiangVien
{
    public class DuyetBoCauHoiAIViewModel : ViewModelBase
    {
        private string _tenBoCauHoi = string.Empty;
        private string _tenNguon = string.Empty;
        private string _nguonTao = "AI";
        private int? _maBaiHoc;
        private int? _maChuDe;
        private int? _maTrinhDo;
        private string _maCapDo = "B1";
        private string _kyNang = "Doc";
        private CauHoiKhaoSatDTO? _selectedCauHoi;
        private string _thongBao = string.Empty;
        private bool _isSuccess;
        private bool _isSaving;
        private bool _daLuuThanhCong;

        public string TenBoCauHoi
        {
            get => _tenBoCauHoi;
            set => SetProperty(ref _tenBoCauHoi, value);
        }

        public string TenNguon
        {
            get => _tenNguon;
            set => SetProperty(ref _tenNguon, value);
        }

        public string NguonTao
        {
            get => _nguonTao;
            set => SetProperty(ref _nguonTao, value);
        }

        public int? MaBaiHoc
        {
            get => _maBaiHoc;
            set => SetProperty(ref _maBaiHoc, value);
        }

        public int? MaChuDe
        {
            get => _maChuDe;
            set => SetProperty(ref _maChuDe, value);
        }

        public int? MaTrinhDo
        {
            get => _maTrinhDo;
            set => SetProperty(ref _maTrinhDo, value);
        }

        public string MaCapDo
        {
            get => _maCapDo;
            set => SetProperty(ref _maCapDo, value);
        }

        public string KyNang
        {
            get => _kyNang;
            set => SetProperty(ref _kyNang, value);
        }

        public ObservableCollection<CauHoiKhaoSatDTO> DanhSachCauHoi { get; } = new();

        public CauHoiKhaoSatDTO? SelectedCauHoi
        {
            get => _selectedCauHoi;
            set
            {
                if (SetProperty(ref _selectedCauHoi, value))
                {
                    OnPropertyChanged(nameof(HasSelectedCauHoi));
                }
            }
        }

        public bool HasSelectedCauHoi => SelectedCauHoi != null;

        public string ThongBao
        {
            get => _thongBao;
            set => SetProperty(ref _thongBao, value);
        }

        public bool IsSuccess
        {
            get => _isSuccess;
            set => SetProperty(ref _isSuccess, value);
        }

        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }

        public bool DaLuuThanhCong
        {
            get => _daLuuThanhCong;
            set => SetProperty(ref _daLuuThanhCong, value);
        }

        public int SoLuongDaChon => DanhSachCauHoi.Count(x => x.IsSelected);
        public int TongSoCau => DanhSachCauHoi.Count;

        public string ThongKeChonText => $"Đã chọn {SoLuongDaChon}/{TongSoCau} câu hỏi";

        // Audio support
        private readonly System.Windows.Media.MediaPlayer _mediaPlayer = new();
        private bool _isPlayingAudio;

        public bool IsPlayingAudio
        {
            get => _isPlayingAudio;
            set => SetProperty(ref _isPlayingAudio, value);
        }

        public ICommand ChonTatCaCommand { get; }
        public ICommand BoChonTatCaCommand { get; }
        public ICommand ThemCauHoiCommand { get; }
        public ICommand XoaCauHoiCommand { get; }
        public ICommand ThemDapAnCommand { get; }
        public ICommand XoaDapAnCommand { get; }
        public ICommand ChonDapAnDungCommand { get; }
        public ICommand LuuVaoNganHangCommand { get; }
        public ICommand ChonAudioCommand { get; }
        public ICommand TogglePlayAudioCommand { get; }

        public event Action? OnLuuThanhCong;

        public DuyetBoCauHoiAIViewModel()
        {
            ChonTatCaCommand = new RelayCommand(_ => SetChonTatCa(true));
            BoChonTatCaCommand = new RelayCommand(_ => SetChonTatCa(false));
            ThemCauHoiCommand = new RelayCommand(_ => ThucHienThemCauHoi());
            XoaCauHoiCommand = new RelayCommand(p => ThucHienXoaCauHoi(p as CauHoiKhaoSatDTO));
            ThemDapAnCommand = new RelayCommand(_ => ThucHienThemDapAn());
            XoaDapAnCommand = new RelayCommand(p => ThucHienXoaDapAn(p as LuaChonDapAnDTO));
            ChonDapAnDungCommand = new RelayCommand(p => ThucHienChonDapAnDung(p as LuaChonDapAnDTO));
            LuuVaoNganHangCommand = new RelayCommand(_ => ThucHienLuuVaoNganHang());
            ChonAudioCommand = new RelayCommand(_ => ThucHienChonAudio());
            TogglePlayAudioCommand = new RelayCommand(_ => ThucHienToggleAudio());
        }

        public void DungAudio()
        {
            try
            {
                if (IsPlayingAudio)
                {
                    _mediaPlayer.Stop();
                    IsPlayingAudio = false;
                }
            }
            catch { }
        }

        private void ThucHienChonAudio()
        {
            if (SelectedCauHoi == null) return;
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Tệp âm thanh (*.mp3;*.wav;*.m4a;*.ogg;*.aac)|*.mp3;*.wav;*.m4a;*.ogg;*.aac|Tất cả tệp (*.*)|*.*",
                Title = "Chọn file Audio câu hỏi nghe"
            };

            if (dlg.ShowDialog() == true)
            {
                SelectedCauHoi.AudioPath = dlg.FileName;
                OnPropertyChanged(nameof(SelectedCauHoi));
            }
        }

        private void ThucHienToggleAudio()
        {
            if (SelectedCauHoi == null || string.IsNullOrWhiteSpace(SelectedCauHoi.AudioPath)) return;

            try
            {
                if (IsPlayingAudio)
                {
                    _mediaPlayer.Pause();
                    IsPlayingAudio = false;
                }
                else
                {
                    _mediaPlayer.Open(new Uri(SelectedCauHoi.AudioPath, UriKind.RelativeOrAbsolute));
                    _mediaPlayer.Play();
                    IsPlayingAudio = true;
                }
            }
            catch (Exception ex)
            {
                ThongBao = $"Lỗi phát audio: {ex.Message}";
                IsSuccess = false;
            }
        }

        public void NapDuLieu(
            string tenNguon, 
            string nguonTao, 
            List<CauHoiKhaoSatDTO> cauHois, 
            int? maBaiHoc = null, 
            int? maChuDe = null, 
            int? maTrinhDo = null, 
            string capDo = "B1", 
            string kyNang = "Doc",
            string? audioPath = null)
        {
            TenNguon = tenNguon;
            NguonTao = nguonTao;
            MaBaiHoc = maBaiHoc;
            MaChuDe = maChuDe;
            MaTrinhDo = maTrinhDo;
            MaCapDo = capDo;
            KyNang = kyNang;

            TenBoCauHoi = $"Bộ câu hỏi ôn tập: {tenNguon}";
            ThongBao = string.Empty;
            IsSuccess = false;
            DaLuuThanhCong = false;

            DanhSachCauHoi.Clear();
            int idx = 1;
            foreach (var ch in cauHois)
            {
                ch.ThuTuCauHoi = idx;
                ch.ThuTuTrongBo = idx;
                if (string.IsNullOrWhiteSpace(ch.KyNang)) ch.KyNang = kyNang;
                if (string.IsNullOrWhiteSpace(ch.MaTrinhDo)) ch.MaTrinhDo = capDo;
                if (ch.MaChuDe == null) ch.MaChuDe = maChuDe;
                if (ch.MaTrinhDoId == null) ch.MaTrinhDoId = maTrinhDo;
                if (string.IsNullOrWhiteSpace(ch.AudioPath) && !string.IsNullOrWhiteSpace(audioPath))
                {
                    ch.AudioPath = audioPath;
                }

                DanhSachCauHoi.Add(ch);
                idx++;
            }

            CapNhatThongKe();
            SelectedCauHoi = DanhSachCauHoi.FirstOrDefault();
        }

        public void CapNhatThongKe()
        {
            OnPropertyChanged(nameof(SoLuongDaChon));
            OnPropertyChanged(nameof(TongSoCau));
            OnPropertyChanged(nameof(ThongKeChonText));
        }

        private void SetChonTatCa(bool select)
        {
            foreach (var ch in DanhSachCauHoi)
            {
                ch.IsSelected = select;
            }
            CapNhatThongKe();
            OnPropertyChanged(nameof(DanhSachCauHoi));
        }

        private void ThucHienThemCauHoi()
        {
            int newIdx = DanhSachCauHoi.Count + 1;
            var ch = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 0,
                ThuTuCauHoi = newIdx,
                ThuTuTrongBo = newIdx,
                KyNang = KyNang,
                MaTrinhDo = MaCapDo,
                MaChuDe = MaChuDe,
                MaTrinhDoId = MaTrinhDo,
                NoiDungCauHoi = $"Câu hỏi mới #{newIdx}",
                LoaiCauHoi = "TracNghiem",
                DoKho = "TrungBinh",
                IsSelected = true,
                DaKiemDuyet = true
            };

            ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "", LaDapAnDung = true });
            ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "", LaDapAnDung = false });
            ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "", LaDapAnDung = false });
            ch.DanhSachLuaChon.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = "", LaDapAnDung = false });

            DanhSachCauHoi.Add(ch);
            SelectedCauHoi = ch;
            CapNhatThongKe();
        }

        private void ThucHienXoaCauHoi(CauHoiKhaoSatDTO? ch)
        {
            if (ch == null) return;
            DanhSachCauHoi.Remove(ch);

            // Đánh lại số thứ tự
            for (int i = 0; i < DanhSachCauHoi.Count; i++)
            {
                DanhSachCauHoi[i].ThuTuCauHoi = i + 1;
                DanhSachCauHoi[i].ThuTuTrongBo = i + 1;
            }

            if (SelectedCauHoi == ch)
            {
                SelectedCauHoi = DanhSachCauHoi.FirstOrDefault();
            }

            CapNhatThongKe();
        }

        private void ThucHienThemDapAn()
        {
            if (SelectedCauHoi == null) return;
            char nextChar = (char)('A' + SelectedCauHoi.DanhSachLuaChon.Count);
            if (nextChar <= 'Z')
            {
                SelectedCauHoi.DanhSachLuaChon.Add(new LuaChonDapAnDTO
                {
                    KyHieuLuaChon = nextChar.ToString(),
                    NoiDungLuaChon = "",
                    LaDapAnDung = false
                });
                OnPropertyChanged(nameof(SelectedCauHoi));
            }
        }

        private void ThucHienXoaDapAn(LuaChonDapAnDTO? da)
        {
            if (SelectedCauHoi == null || da == null) return;
            if (SelectedCauHoi.DanhSachLuaChon.Count > 2)
            {
                SelectedCauHoi.DanhSachLuaChon.Remove(da);
                for (int i = 0; i < SelectedCauHoi.DanhSachLuaChon.Count; i++)
                {
                    SelectedCauHoi.DanhSachLuaChon[i].KyHieuLuaChon = ((char)('A' + i)).ToString();
                }
                if (!SelectedCauHoi.DanhSachLuaChon.Any(x => x.LaDapAnDung))
                {
                    SelectedCauHoi.DanhSachLuaChon[0].LaDapAnDung = true;
                }
                OnPropertyChanged(nameof(SelectedCauHoi));
            }
        }

        private void ThucHienChonDapAnDung(LuaChonDapAnDTO? da)
        {
            if (SelectedCauHoi == null || da == null) return;
            foreach (var opt in SelectedCauHoi.DanhSachLuaChon)
            {
                opt.LaDapAnDung = (opt == da);
            }
            OnPropertyChanged(nameof(SelectedCauHoi));
        }

        public void ThucHienLuuVaoNganHang()
        {
            if (string.IsNullOrWhiteSpace(TenBoCauHoi))
            {
                ThongBao = "Vui lòng nhập tên cho bộ câu hỏi.";
                IsSuccess = false;
                return;
            }

            var dsDuocChon = DanhSachCauHoi.Where(x => x.IsSelected).ToList();
            if (dsDuocChon.Count == 0)
            {
                ThongBao = "Vui lòng chọn ít nhất một câu hỏi để lưu.";
                IsSuccess = false;
                return;
            }

            // Kiểm tra nội dung câu hỏi
            foreach (var q in dsDuocChon)
            {
                if (string.IsNullOrWhiteSpace(q.NoiDungCauHoi))
                {
                    ThongBao = $"Câu hỏi #{q.ThuTuCauHoi} đang để trống nội dung.";
                    IsSuccess = false;
                    return;
                }

                if (q.DanhSachLuaChon.Count < 2)
                {
                    ThongBao = $"Câu hỏi #{q.ThuTuCauHoi} phải có ít nhất 2 phương án.";
                    IsSuccess = false;
                    return;
                }

                if (!q.DanhSachLuaChon.Any(x => x.LaDapAnDung))
                {
                    ThongBao = $"Câu hỏi #{q.ThuTuCauHoi} chưa chọn đáp án đúng.";
                    IsSuccess = false;
                    return;
                }
            }

            IsSaving = true;
            ThongBao = "Đang lưu bộ câu hỏi vào hệ thống cơ sở dữ liệu...";

            try
            {
                var bo = new BoCauHoiDTO
                {
                    MaBoCauHoi = 0,
                    TenBoCauHoi = TenBoCauHoi.Trim(),
                    MoTa = $"Được sinh bởi AI từ {TenNguon} ({NguonTao})",
                    NguonTao = NguonTao,
                    TenNguon = TenNguon,
                    MaBaiHoc = MaBaiHoc,
                    MaChuDe = MaChuDe,
                    MaTrinhDo = MaTrinhDo,
                    NguoiTao = SessionHelper.CurrentUser?.MaNguoiDung ?? 1
                };

                int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
                bool ok = BoCauHoiBUS.Instance.LuuBoCauHoi(bo, dsDuocChon, userId, out string err);

                if (ok)
                {
                    IsSuccess = true;
                    DaLuuThanhCong = true;
                    ThongBao = $"Đã lưu thành công bộ câu hỏi \"{bo.TenBoCauHoi}\" gồm {dsDuocChon.Count} câu vào Ngân hàng câu hỏi!";
                    OnLuuThanhCong?.Invoke();
                }
                else
                {
                    IsSuccess = false;
                    ThongBao = "Lỗi khi lưu bộ câu hỏi: " + err;
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                ThongBao = "Lỗi hệ thống khi lưu: " + ex.Message;
            }
            finally
            {
                IsSaving = false;
            }
        }
    }
}
