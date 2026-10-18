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
        private CauHoiKhaoSatDTO _currentCauHoi = new();
        private MediaPlayer _mediaPlayer = new();
        private bool _isPlayingAudio;
        private string _thongBaoLoi = string.Empty;
        private string _tuKhoaTimKiem = string.Empty;
        private string _filterKyNang = "All";
        private string _filterDoKho = "All";

        public CauHoiKhaoSatDTO CurrentCauHoi
        {
            get => _currentCauHoi;
            set => SetProperty(ref _currentCauHoi, value);
        }

        public ObservableCollection<LuaChonDapAnDTO> DanhSachDapAn { get; set; } = new();
        public ObservableCollection<CauHoiKhaoSatDTO> DanhSachCauHoi { get; set; } = new();
        public ObservableCollection<TrinhDoDTO> DanhSachTrinhDo { get; set; } = new();
        public ObservableCollection<ChuDeDTO> DanhSachChuDe { get; set; } = new();

        public string ThongBaoLoi
        {
            get => _thongBaoLoi;
            set => SetProperty(ref _thongBaoLoi, value);
        }

        public bool IsPlayingAudio
        {
            get => _isPlayingAudio;
            set => SetProperty(ref _isPlayingAudio, value);
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

        public ICommand ThemDapAnCommand { get; }
        public ICommand XoaDapAnCommand { get; }
        public ICommand ChonDapAnDungCommand { get; }
        public ICommand ChonAudioCommand { get; }
        public ICommand TogglePlayAudioCommand { get; }
        public ICommand LuuCauHoiCommand { get; }
        public ICommand ThemCauHoiMoiCommand { get; }

        public NganHangCauHoiViewModel()
        {
            ThemDapAnCommand = new RelayCommand(ThucHienThemDapAn);
            XoaDapAnCommand = new RelayCommand(param => ThucHienXoaDapAn(param as LuaChonDapAnDTO));
            ChonDapAnDungCommand = new RelayCommand(param => ThucHienChonDapAnDung(param as LuaChonDapAnDTO));
            ChonAudioCommand = new RelayCommand(ThucHienChonAudio);
            TogglePlayAudioCommand = new RelayCommand(ThucHienToggleAudio);
            LuuCauHoiCommand = new RelayCommand(ThucHienLuuCauHoi);
            ThemCauHoiMoiCommand = new RelayCommand(KhoiTaoCauHoiMoi);

            KhoiTaoCauHoiMoi();
            NapDanhMuc();
            NapDanhSachCauHoi();
        }

        private void NapDanhMuc()
        {
            var dmBUS = new DanhMucBUS();
            DanhSachTrinhDo.Clear();
            foreach (var td in dmBUS.LayDanhSachTrinhDo()) DanhSachTrinhDo.Add(td);

            DanhSachChuDe.Clear();
            foreach (var cd in dmBUS.LayDanhSachChuDe()) DanhSachChuDe.Add(cd);
        }

        public void NapDanhSachCauHoi()
        {
            var ds = CauHoiDAL.Instance.LayDanhSachCauHoiTheoTrinhDo("B1", 50);

            if (!string.IsNullOrEmpty(FilterKyNang) && FilterKyNang != "All")
            {
                ds = ds.Where(x => string.Equals(x.KyNang, FilterKyNang, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(TuKhoaTimKiem))
            {
                ds = ds.Where(x => x.NoiDungCauHoi.Contains(TuKhoaTimKiem, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            DanhSachCauHoi.Clear();
            foreach (var ch in ds)
                DanhSachCauHoi.Add(ch);
        }

        public void ChonCauHoiDeSua(CauHoiKhaoSatDTO ch)
        {
            if (ch == null) return;
            CurrentCauHoi = ch;
            DanhSachDapAn.Clear();
            foreach (var opt in ch.DanhSachLuaChon)
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

            if (DanhSachDapAn.Count == 0)
            {
                DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "", LaDapAnDung = true });
                DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "", LaDapAnDung = false });
            }
            ThongBaoLoi = string.Empty;
        }

        private void KhoiTaoCauHoiMoi()
        {
            CurrentCauHoi = new CauHoiKhaoSatDTO
            {
                LoaiCauHoi = "TracNghiem",
                KyNang = "Doc",
                DoKho = "TrungBinh",
                MaTrinhDo = "B1",
                DaKiemDuyet = true
            };

            DanhSachDapAn.Clear();
            DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = "", LaDapAnDung = true });
            DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = "", LaDapAnDung = false });
            DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = "", LaDapAnDung = false });
            DanhSachDapAn.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = "", LaDapAnDung = false });
            ThongBaoLoi = string.Empty;
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
            }
        }

        private void ThucHienChonDapAnDung(LuaChonDapAnDTO? selected)
        {
            if (selected == null) return;
            foreach (var opt in DanhSachDapAn)
            {
                opt.LaDapAnDung = (opt == selected);
            }
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
                return;
            }

            if (CurrentCauHoi.LoaiCauHoi == "TracNghiem")
            {
                if (DanhSachDapAn.Count < 2)
                {
                    ThongBaoLoi = "Câu hỏi trắc nghiệm phải có ít nhất 2 lựa chọn.";
                    return;
                }

                int soDapAnDung = DanhSachDapAn.Count(x => x.LaDapAnDung);
                if (soDapAnDung != 1)
                {
                    ThongBaoLoi = "Bắt buộc phải chọn đúng duy nhất 1 phương án là đáp án đúng.";
                    return;
                }

                if (DanhSachDapAn.Any(x => string.IsNullOrWhiteSpace(x.NoiDungLuaChon)))
                {
                    ThongBaoLoi = "Các phương án trả lời không được để trống nội dung.";
                    return;
                }
            }

            ThongBaoLoi = string.Empty;
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;

            bool thanhCong = CauHoiDAL.Instance.LuuCauHoiVaDapAn(
                CurrentCauHoi, 
                DanhSachDapAn.ToList(), 
                userId, 
                out string err);

            if (thanhCong)
            {
                GUI.Views.Common.ThongBaoDialog.ThanhCong("Lưu câu hỏi vào ngân hàng thành công bằng SqlTransaction!", "Thành công");
                NapDanhSachCauHoi();
                KhoiTaoCauHoiMoi();
            }
            else
            {
                ThongBaoLoi = "Lỗi khi lưu câu hỏi: " + err;
            }
        }
    }
}
