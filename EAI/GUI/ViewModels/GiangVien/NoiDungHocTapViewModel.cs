using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<TrinhDoDTO> DanhSachTrinhDoFilter { get; set; } = new();
        public ObservableCollection<ChuDeDTO> DanhSachChuDe { get; set; } = new();

        // Trình phát audio nghe thử bài giảng
        private readonly System.Windows.Media.MediaPlayer _mediaPlayer = new();
        private readonly System.Windows.Threading.DispatcherTimer _audioTimer = new() { Interval = TimeSpan.FromMilliseconds(150) };
        private bool _isPlayingAudio;
        private bool _isTimerUpdating;
        private bool _isUserSeeking;
        private string? _loadedAudioPath;

        public bool IsPlayingAudio
        {
            get => _isPlayingAudio;
            set => SetProperty(ref _isPlayingAudio, value);
        }

        private double _audioCurrentSeconds;
        public double AudioCurrentSeconds
        {
            get => _audioCurrentSeconds;
            set
            {
                if (Math.Abs(_audioCurrentSeconds - value) > 0.05)
                {
                    _audioCurrentSeconds = value;
                    OnPropertyChanged();
                    AudioCurrentTimeText = FormatTimeSpan(TimeSpan.FromSeconds(value));

                    if (!_isTimerUpdating && !_isUserSeeking)
                    {
                        SeekTo(value);
                    }
                }
            }
        }

        private double _audioTotalSeconds = 100;
        public double AudioTotalSeconds
        {
            get => _audioTotalSeconds;
            set => SetProperty(ref _audioTotalSeconds, value);
        }

        private string _audioCurrentTimeText = "00:00";
        public string AudioCurrentTimeText
        {
            get => _audioCurrentTimeText;
            set => SetProperty(ref _audioCurrentTimeText, value);
        }

        private string _audioTotalTimeText = "00:00";
        public string AudioTotalTimeText
        {
            get => _audioTotalTimeText;
            set => SetProperty(ref _audioTotalTimeText, value);
        }

        private BaiHocDTO? _selectedBaiHocInList;
        public BaiHocDTO? SelectedBaiHocInList
        {
            get => _selectedBaiHocInList;
            set
            {
                if (IsAiBusy) return;

                // Cảnh báo nếu đang soạn bài học mới mà có dữ liệu chưa lưu
                if (value != null && IsCreatingNewLesson && CoDuLieuBaiHocMoiChuaLuu() && value != _selectedBaiHoc)
                {
                    bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                        "Bạn đang soạn bài học mới và có dữ liệu chưa lưu.\nNếu chuyển sang bài học khác, toàn bộ nội dung bài học đang tạo sẽ bị mất.\n\nBạn có chắc chắn muốn chuyển không?",
                        "Xác nhận chuyển bài học",
                        confirmText: "Tiếp tục chuyển",
                        cancelText: "Ở lại chỉnh sửa");

                    if (!xacNhan)
                    {
                        // Hủy chuyển: giữ nguyên bài học mới đang soạn, hoàn tác việc chọn trên danh sách
                        System.Windows.Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            OnPropertyChanged(nameof(SelectedBaiHocInList));
                        }));
                        return;
                    }
                }
                else if (value != null && !IsCreatingNewLesson && _selectedBaiHoc != null && _selectedBaiHoc.MaBaiHoc > 0 && value.MaBaiHoc != _selectedBaiHoc.MaBaiHoc)
                {
                    var original = DanhSachBaiHoc.FirstOrDefault(b => b.MaBaiHoc == _selectedBaiHoc.MaBaiHoc);
                    if (original != null && CoThayDoiSoVoiGoc(original, _selectedBaiHoc))
                    {
                        bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                            $"Bài học \"{_selectedBaiHoc.TieuDe}\" có các chỉnh sửa chưa được lưu.\nNếu chuyển sang bài học khác, các thay đổi chưa lưu sẽ bị hủy.\n\nBạn có chắc chắn muốn chuyển không?",
                            "Xác nhận chuyển bài học",
                            confirmText: "Tiếp tục chuyển",
                            cancelText: "Ở lại lưu bài");

                        if (!xacNhan)
                        {
                            System.Windows.Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                OnPropertyChanged(nameof(SelectedBaiHocInList));
                            }));
                            return;
                        }
                    }
                }

                if (SetProperty(ref _selectedBaiHocInList, value))
                {
                    if (value != null)
                    {
                        IsCreatingNewLesson = false;
                        SelectedBaiHoc = value.Clone();
                    }
                }
            }
        }

        private bool _isCreatingNewLesson = false;
        public bool IsCreatingNewLesson
        {
            get => _isCreatingNewLesson;
            set => SetProperty(ref _isCreatingNewLesson, value);
        }

        public BaiHocDTO SelectedBaiHoc
        {
            get => _selectedBaiHoc;
            set
            {
                if (_selectedBaiHoc != null)
                {
                    _selectedBaiHoc.PropertyChanged -= SelectedBaiHoc_PropertyChanged;
                    DungAudio();
                }

                BaiHocDTO item = value ?? new BaiHocDTO();
                if (!string.IsNullOrEmpty(item.NoiDung))
                {
                    item.NoiDung = KhoHocLieuSyncHelper.ChuyenDoiHtmlSangVanBan(item.NoiDung);
                }
                item.PropertyChanged += SelectedBaiHoc_PropertyChanged;
                _selectedBaiHoc = item;
                OnPropertyChanged();

                _noiDungBackupTruocKhiTaoAI = null;
                OnPropertyChanged(nameof(CoNoiDungBackupTruocKhiTaoAI));
                OnPropertyChanged(nameof(IsKyNangNghe));
                OnPropertyChanged(nameof(HasAudioFile));
                OnPropertyChanged(nameof(HasTieuDeBaiHoc));
                OnPropertyChanged(nameof(CanTaoBangAI));
                OnPropertyChanged(nameof(CoTheXoaBaiHoc));
                NapAudioFileHienTai();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void SelectedBaiHoc_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaiHocDTO.LoaiBaiHoc) || e.PropertyName == nameof(BaiHocDTO.IsKyNangNghe))
            {
                _noiDungBackupTruocKhiTaoAI = null;
                OnPropertyChanged(nameof(CoNoiDungBackupTruocKhiTaoAI));
                OnPropertyChanged(nameof(IsKyNangNghe));
                CommandManager.InvalidateRequerySuggested();
            }
            else if (e.PropertyName == nameof(BaiHocDTO.AudioPath) || e.PropertyName == nameof(BaiHocDTO.DuongDanAudio) || e.PropertyName == nameof(BaiHocDTO.HasAudio))
            {
                DungAudio();
                OnPropertyChanged(nameof(HasAudioFile));
                CommandManager.InvalidateRequerySuggested();
            }
            else if (e.PropertyName == nameof(BaiHocDTO.TieuDe))
            {
                OnPropertyChanged(nameof(HasTieuDeBaiHoc));
                OnPropertyChanged(nameof(CanTaoBangAI));
                CommandManager.InvalidateRequerySuggested();
            }
            else if (e.PropertyName == nameof(BaiHocDTO.MaTrinhDo))
            {
                if (SelectedBaiHoc != null && !string.IsNullOrWhiteSpace(SelectedBaiHoc.MaTrinhDo))
                {
                    var matchTd = DanhSachTrinhDo.FirstOrDefault(x => string.Equals(x.MaCapDo, SelectedBaiHoc.MaTrinhDo, StringComparison.OrdinalIgnoreCase));
                    if (matchTd != null && matchTd.MaTrinhDo > 0)
                    {
                        SelectedBaiHoc.MaTrinhDoId = matchTd.MaTrinhDo;
                    }
                }
            }
            else if (e.PropertyName == nameof(BaiHocDTO.MaChuDe))
            {
                if (SelectedBaiHoc != null && SelectedBaiHoc.MaChuDe > 0)
                {
                    var matchingChuDe = DanhSachChuDe.FirstOrDefault(x => x.MaChuDe == SelectedBaiHoc.MaChuDe);
                    if (matchingChuDe != null)
                    {
                        SelectedBaiHoc.TenChuDe = matchingChuDe.TenChuDe;
                    }
                }
            }
        }

        public bool IsKyNangNghe => BaiHocDTO.KiemTraKyNangNghe(SelectedBaiHoc?.LoaiBaiHoc);
        public bool HasAudioFile => !string.IsNullOrWhiteSpace(SelectedBaiHoc?.AudioPath);
        public bool HasTieuDeBaiHoc => !string.IsNullOrWhiteSpace(SelectedBaiHoc?.TieuDe);
        public bool CanTaoBangAI => CanPerformActions && HasTieuDeBaiHoc;

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

        private bool _isAiGeneratingSample;
        private bool _isAiGeneratingQuestions;
        private bool _isQuestionsModalOpen;
        private string _aiQuestionsStatusMessage = string.Empty;

        public bool IsAiBusy => _isAiGeneratingSample || _isAiGeneratingQuestions;
        public bool CanPerformActions => !IsAiBusy;

        public bool IsAiGeneratingSample
        {
            get => _isAiGeneratingSample;
            set
            {
                if (SetProperty(ref _isAiGeneratingSample, value))
                {
                    OnPropertyChanged(nameof(IsAiBusy));
                    OnPropertyChanged(nameof(CanPerformActions));
                    OnPropertyChanged(nameof(CanTaoBangAI));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsAiGeneratingQuestions
        {
            get => _isAiGeneratingQuestions;
            set
            {
                if (SetProperty(ref _isAiGeneratingQuestions, value))
                {
                    OnPropertyChanged(nameof(IsAiBusy));
                    OnPropertyChanged(nameof(CanPerformActions));
                    OnPropertyChanged(nameof(CanTaoBangAI));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public bool IsQuestionsModalOpen
        {
            get => _isQuestionsModalOpen;
            set => SetProperty(ref _isQuestionsModalOpen, value);
        }

        public string AiQuestionsStatusMessage
        {
            get => _aiQuestionsStatusMessage;
            set => SetProperty(ref _aiQuestionsStatusMessage, value);
        }

        public ObservableCollection<CauHoiDTO> AiGeneratedQuestions { get; set; } = new();

        public ICommand ChonAudioCommand { get; }
        public ICommand XoaAudioCommand { get; }
        public ICommand TogglePlayAudioCommand { get; }
        public ICommand LuuBaiHocCommand { get; }
        public ICommand ToggleXuatBanCommand { get; }
        public ICommand ThemBaiHocMoiCommand { get; }
        public ICommand XoaBaiHocHienTaiCommand { get; }
        public bool CoTheXoaBaiHoc => SelectedBaiHoc != null && SelectedBaiHoc.MaBaiHoc > 0;
        private string? _noiDungBackupTruocKhiTaoAI;
        public bool CoNoiDungBackupTruocKhiTaoAI => !string.IsNullOrEmpty(_noiDungBackupTruocKhiTaoAI);

        public ICommand LamMoiCommand { get; }
        public ICommand SinhNoiDungMauAICommand { get; }
        public ICommand HoanTacNoiDungCuCommand { get; }
        public ICommand SinhCauHoiAICommand { get; }
        public ICommand DongModalCauHoiCommand { get; }
        public ICommand ChenCauHoiVaoBaiHocCommand { get; }
        public ICommand ThemCauHoiTracNghiemCommand { get; }
        public ICommand ThemCauHoiTuLuanCommand { get; }
        public ICommand ThemCauHoiDienTuCommand { get; }

        public NoiDungHocTapViewModel()
        {
            _selectedBaiHoc.PropertyChanged += SelectedBaiHoc_PropertyChanged;

            _audioTimer.Tick += AudioTimer_Tick;

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
                    IsPlayingAudio = false;
                    _audioTimer.Stop();
                    ThongBaoLoi = $"Không thể phát tệp âm thanh: {e.ErrorException?.Message ?? "Lỗi định dạng tệp."}";
                }));
            };

            ChonAudioCommand = new RelayCommand(ThucHienChonAudio, () => IsKyNangNghe);
            XoaAudioCommand = new RelayCommand(ThucHienXoaAudio, () => HasAudioFile);
            TogglePlayAudioCommand = new RelayCommand(ThucHienToggleAudio, () => HasAudioFile);
            LuuBaiHocCommand = new RelayCommand(ThucHienLuuBaiHoc);
            ToggleXuatBanCommand = new RelayCommand(ThucHienToggleXuatBan);
            ThemBaiHocMoiCommand = new RelayCommand(ThucHienThemBaiHocMoi);
            XoaBaiHocHienTaiCommand = new RelayCommand(ThucHienXoaBaiHocHienTai);
            LamMoiCommand = new RelayCommand(NapDuLieu);

            SinhNoiDungMauAICommand = new RelayCommand(ThucHienSinhNoiDungMauAI, () => CanTaoBangAI);
            HoanTacNoiDungCuCommand = new RelayCommand(ThucHienHoanTacNoiDungCu, () => CoNoiDungBackupTruocKhiTaoAI);
            SinhCauHoiAICommand = new RelayCommand(ThucHienSinhCauHoiAI);
            DongModalCauHoiCommand = new RelayCommand(() => IsQuestionsModalOpen = false);
            ChenCauHoiVaoBaiHocCommand = new RelayCommand(ThucHienChenCauHoiVaoBaiHoc);
            ThemCauHoiTracNghiemCommand = new RelayCommand(ThucHienThemCauHoiTracNghiem);
            ThemCauHoiTuLuanCommand = new RelayCommand(ThucHienThemCauHoiTuLuan);
            ThemCauHoiDienTuCommand = new RelayCommand(ThucHienThemCauHoiDienTu);

            NapDuLieu();
        }

        public bool CoDuLieuBaiHocMoiChuaLuu()
        {
            if (!IsCreatingNewLesson) return false;
            if (IsAiBusy) return true;
            if (SelectedBaiHoc == null) return false;

            bool coTieuDe = !string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe);
            bool coNoiDung = !string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung);
            bool coTomTat = !string.IsNullOrWhiteSpace(SelectedBaiHoc.TomTat);
            bool coAudio = !string.IsNullOrWhiteSpace(SelectedBaiHoc.AudioPath);

            return coTieuDe || coNoiDung || coTomTat || coAudio;
        }

        public void NapDuLieu()
        {
            if (IsAiBusy) return;

            if (IsCreatingNewLesson && CoDuLieuBaiHocMoiChuaLuu())
            {
                bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    "Bạn đang soạn bài học mới và có dữ liệu chưa lưu.\nNếu làm mới danh sách bài học, dữ liệu đang soạn có thể bị ảnh hưởng.\n\nBạn có chắc chắn muốn làm mới không?",
                    "Xác nhận làm mới",
                    confirmText: "Làm mới",
                    cancelText: "Ở lại chỉnh sửa");

                if (!xacNhan) return;
            }
            try
            {
                CapNhatDanhSachTrinhDo();
                CapNhatDanhSachChuDe();
                LocDanhSach();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NoiDungHocTapViewModel.NapDuLieu] {ex.Message}");
                ThongBaoLoi = "Không thể tải danh mục bài học: " + ex.Message;
            }
        }

        public void CapNhatDanhSachTrinhDo()
        {
            try
            {
                string currentMaTrinhDo = SelectedBaiHoc?.MaTrinhDo ?? "";
                string currentFilterCapDo = SelectedTrinhDoFilter?.MaCapDo ?? "";

                var trinhDos = _danhMucBUS.LayDanhSachTrinhDo();
                if (trinhDos == null) return;

                var newIds = trinhDos.Select(x => x.MaCapDo).ToHashSet(StringComparer.OrdinalIgnoreCase);

                // 1. Cập nhật DanhSachTrinhDoFilter (Có mục "Tất cả cấp độ (A1 - C1)" ở vị trí đầu)
                if (DanhSachTrinhDoFilter.Count == 0 || !string.IsNullOrEmpty(DanhSachTrinhDoFilter[0].MaCapDo))
                {
                    DanhSachTrinhDoFilter.Insert(0, new TrinhDoDTO { MaCapDo = "", TenTrinhDo = "Tất cả cấp độ (A1 - C1)" });
                }

                for (int i = DanhSachTrinhDoFilter.Count - 1; i >= 1; i--)
                {
                    if (!newIds.Contains(DanhSachTrinhDoFilter[i].MaCapDo))
                    {
                        DanhSachTrinhDoFilter.RemoveAt(i);
                    }
                }

                for (int i = 0; i < trinhDos.Count; i++)
                {
                    var item = trinhDos[i];
                    var existing = DanhSachTrinhDoFilter.Skip(1).FirstOrDefault(x => string.Equals(x.MaCapDo, item.MaCapDo, StringComparison.OrdinalIgnoreCase));
                    if (existing != null)
                    {
                        existing.TenTrinhDo = item.TenTrinhDo;
                        existing.MoTa = item.MoTa;
                        existing.MaTrinhDo = item.MaTrinhDo;
                    }
                    else
                    {
                        DanhSachTrinhDoFilter.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(currentFilterCapDo))
                {
                    var matchFilter = DanhSachTrinhDoFilter.FirstOrDefault(x => string.Equals(x.MaCapDo, currentFilterCapDo, StringComparison.OrdinalIgnoreCase));
                    if (matchFilter != null)
                        SelectedTrinhDoFilter = matchFilter;
                }
                else if (SelectedTrinhDoFilter == null && DanhSachTrinhDoFilter.Count > 0)
                {
                    SelectedTrinhDoFilter = DanhSachTrinhDoFilter[0];
                }

                // 2. Cập nhật DanhSachTrinhDo (Dùng cho Form Soạn thảo: CHỈ gồm các cấp độ thực tế A1-C1)
                for (int i = DanhSachTrinhDo.Count - 1; i >= 0; i--)
                {
                    if (!newIds.Contains(DanhSachTrinhDo[i].MaCapDo))
                    {
                        DanhSachTrinhDo.RemoveAt(i);
                    }
                }

                for (int i = 0; i < trinhDos.Count; i++)
                {
                    var item = trinhDos[i];
                    var existing = DanhSachTrinhDo.FirstOrDefault(x => string.Equals(x.MaCapDo, item.MaCapDo, StringComparison.OrdinalIgnoreCase));
                    if (existing != null)
                    {
                        existing.TenTrinhDo = item.TenTrinhDo;
                        existing.MoTa = item.MoTa;
                        existing.MaTrinhDo = item.MaTrinhDo;
                    }
                    else
                    {
                        DanhSachTrinhDo.Add(new TrinhDoDTO
                        {
                            MaTrinhDo = item.MaTrinhDo,
                            MaCapDo = item.MaCapDo,
                            TenTrinhDo = item.TenTrinhDo,
                            MoTa = item.MoTa
                        });
                    }
                }

                if (SelectedBaiHoc != null && !string.IsNullOrEmpty(currentMaTrinhDo))
                {
                    SelectedBaiHoc.MaTrinhDo = currentMaTrinhDo;
                    var matchTd = DanhSachTrinhDo.FirstOrDefault(x => string.Equals(x.MaCapDo, currentMaTrinhDo, StringComparison.OrdinalIgnoreCase));
                    if (matchTd != null && matchTd.MaTrinhDo > 0)
                    {
                        SelectedBaiHoc.MaTrinhDoId = matchTd.MaTrinhDo;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CapNhatDanhSachTrinhDo] {ex.Message}");
            }
        }

        public void CapNhatDanhSachChuDe()
        {
            try
            {
                int currentMaChuDe = SelectedBaiHoc?.MaChuDe ?? 0;
                var chuDes = _danhMucBUS.LayDanhSachChuDe();
                if (chuDes == null) return;

                var sortedList = SapXepChuDePhanCap(chuDes);
                var newIds = sortedList.Select(x => x.MaChuDe).ToHashSet();

                // 1. Xóa các chủ đề không còn tồn tại
                for (int i = DanhSachChuDe.Count - 1; i >= 0; i--)
                {
                    if (!newIds.Contains(DanhSachChuDe[i].MaChuDe))
                    {
                        DanhSachChuDe.RemoveAt(i);
                    }
                }

                // 2. Thêm mới hoặc cập nhật các chủ đề hiện có
                for (int i = 0; i < sortedList.Count; i++)
                {
                    var fresh = sortedList[i];
                    var existing = DanhSachChuDe.FirstOrDefault(x => x.MaChuDe == fresh.MaChuDe);
                    if (existing != null)
                    {
                        existing.TenChuDe = fresh.TenChuDe;
                        existing.TenChuDeCha = fresh.TenChuDeCha;
                        existing.MaChuDeCha = fresh.MaChuDeCha;
                        existing.MoTa = fresh.MoTa;
                        existing.TrangThaiHoatDong = fresh.TrangThaiHoatDong;
                        existing.SoLuongBaiHoc = fresh.SoLuongBaiHoc;

                        int currentIndex = DanhSachChuDe.IndexOf(existing);
                        if (currentIndex != i && currentIndex >= 0 && i < DanhSachChuDe.Count)
                        {
                            DanhSachChuDe.Move(currentIndex, i);
                        }
                    }
                    else
                    {
                        if (i < DanhSachChuDe.Count)
                        {
                            DanhSachChuDe.Insert(i, fresh);
                        }
                        else
                        {
                            DanhSachChuDe.Add(fresh);
                        }
                    }
                }

                // 3. Khôi phục lại MaChuDe của bài học đang chọn (tránh WPF ComboBox two-way binding reset)
                if (SelectedBaiHoc != null)
                {
                    if (newIds.Contains(currentMaChuDe))
                    {
                        if (SelectedBaiHoc.MaChuDe != currentMaChuDe)
                        {
                            SelectedBaiHoc.MaChuDe = currentMaChuDe;
                        }
                    }
                    else if (DanhSachChuDe.Count > 0)
                    {
                        SelectedBaiHoc.MaChuDe = DanhSachChuDe[0].MaChuDe;
                        SelectedBaiHoc.TenChuDe = DanhSachChuDe[0].TenChuDe;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CapNhatDanhSachChuDe] {ex.Message}");
            }
        }

        private static List<ChuDeDTO> SapXepChuDePhanCap(List<ChuDeDTO> rawList)
        {
            var result = new List<ChuDeDTO>();
            var visited = new HashSet<int>();

            void AddSubTree(int parentId)
            {
                var children = rawList.Where(x => x.MaChuDeCha == parentId && !visited.Contains(x.MaChuDe)).OrderBy(x => x.TenChuDe).ToList();
                foreach (var child in children)
                {
                    visited.Add(child.MaChuDe);
                    result.Add(child);
                    AddSubTree(child.MaChuDe);
                }
            }

            var roots = rawList.Where(x => !x.MaChuDeCha.HasValue || x.MaChuDeCha <= 0).OrderBy(x => x.TenChuDe).ToList();
            foreach (var root in roots)
            {
                visited.Add(root.MaChuDe);
                result.Add(root);
                AddSubTree(root.MaChuDe);
            }

            foreach (var item in rawList)
            {
                if (!visited.Contains(item.MaChuDe))
                {
                    visited.Add(item.MaChuDe);
                    result.Add(item);
                }
            }

            return result;
        }

        public void LocDanhSach()
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
                    string target = SelectedKyNangFilter;
                    if (target.Equals("Ngữ pháp", StringComparison.OrdinalIgnoreCase)) target = "NguPhap";
                    else if (target.Equals("Từ vựng", StringComparison.OrdinalIgnoreCase)) target = "TuVung";
                    else if (target.Equals("Đọc hiểu", StringComparison.OrdinalIgnoreCase)) target = "Doc";
                    else if (target.Equals("Nghe hiểu", StringComparison.OrdinalIgnoreCase)) target = "Nghe";
                    else if (target.Equals("Viết luận", StringComparison.OrdinalIgnoreCase)) target = "Viet";
                    else if (target.Equals("Luyện nói", StringComparison.OrdinalIgnoreCase)) target = "Noi";

                    ds = ds.Where(x => string.Equals(x.LoaiBaiHoc, target, StringComparison.OrdinalIgnoreCase)
                                    || string.Equals(x.LoaiBaiHoc, SelectedKyNangFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int currentId = SelectedBaiHoc?.MaBaiHoc ?? -1;
                var currentSelected = SelectedBaiHoc;

                DanhSachBaiHoc.Clear();
                foreach (var item in ds)
                {
                    if (!string.IsNullOrEmpty(item.NoiDung))
                    {
                        item.NoiDung = KhoHocLieuSyncHelper.ChuyenDoiHtmlSangVanBan(item.NoiDung);
                    }
                    DanhSachBaiHoc.Add(item);
                }

                if (currentId > 0)
                {
                    var match = DanhSachBaiHoc.FirstOrDefault(x => x.MaBaiHoc == currentId);
                    if (match != null)
                    {
                        if (IsAiGeneratingSample && currentSelected != null)
                        {
                            match.NoiDung = currentSelected.NoiDung;
                        }
                        SelectedBaiHoc = match.Clone();
                        _selectedBaiHocInList = match;
                        OnPropertyChanged(nameof(SelectedBaiHocInList));
                    }
                    else
                    {
                        if (DanhSachBaiHoc.Count > 0)
                        {
                            SelectedBaiHoc = DanhSachBaiHoc[0].Clone();
                            _selectedBaiHocInList = DanhSachBaiHoc[0];
                            OnPropertyChanged(nameof(SelectedBaiHocInList));
                        }
                        else
                        {
                            SelectedBaiHoc = new BaiHocDTO();
                            _selectedBaiHocInList = null;
                            OnPropertyChanged(nameof(SelectedBaiHocInList));
                        }
                    }
                }
                else if (IsCreatingNewLesson && currentSelected != null)
                {
                    SelectedBaiHoc = currentSelected;
                    _selectedBaiHocInList = null;
                    OnPropertyChanged(nameof(SelectedBaiHocInList));
                }
                else if (!IsCreatingNewLesson)
                {
                    if (DanhSachBaiHoc.Count > 0)
                    {
                        SelectedBaiHoc = DanhSachBaiHoc[0].Clone();
                        _selectedBaiHocInList = DanhSachBaiHoc[0];
                        OnPropertyChanged(nameof(SelectedBaiHocInList));
                    }
                    else
                    {
                        SelectedBaiHoc = new BaiHocDTO();
                        _selectedBaiHocInList = null;
                        OnPropertyChanged(nameof(SelectedBaiHocInList));
                    }
                }

                // Dọn dẹp cache câu hỏi AI nháp đối với các bài học không còn tồn tại trong hệ thống
                Views.Common.AICauHoiUIHelper.DonDepCacBaiHocKhongTonTai(DanhSachBaiHoc.Select(x => x.MaBaiHoc));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NoiDungHocTapViewModel.LocDanhSach] {ex.Message}");
            }
        }

        private static bool CoThayDoiSoVoiGoc(BaiHocDTO original, BaiHocDTO current)
        {
            if (original.TieuDe?.Trim() != current.TieuDe?.Trim()) return true;
            if (original.MaChuDe != current.MaChuDe) return true;
            if (original.MaTrinhDo != current.MaTrinhDo) return true;
            if (original.LoaiBaiHoc != current.LoaiBaiHoc) return true;
            if (original.ThoiGianUocTinhPhut != current.ThoiGianUocTinhPhut) return true;
            if (original.DuongDanAudio != current.DuongDanAudio) return true;
            string c1 = (original.NoiDung ?? "").Trim();
            string c2 = (current.NoiDung ?? "").Trim();
            return c1 != c2;
        }

        private void ThucHienThemBaiHocMoi()
        {
            if (IsAiBusy) return;

            if (IsCreatingNewLesson && CoDuLieuBaiHocMoiChuaLuu())
            {
                bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    "Bạn đang soạn một bài học mới và có nội dung chưa lưu.\nNếu làm mới để tạo lại bài học khác, dữ liệu hiện tại sẽ bị xóa.\n\nBạn có chắc chắn muốn tạo mới không?",
                    "Xác nhận tạo bài học mới",
                    confirmText: "Tạo bài mới",
                    cancelText: "Ở lại chỉnh sửa");

                if (!xacNhan) return;
            }
            else if (!IsCreatingNewLesson && _selectedBaiHoc != null && _selectedBaiHoc.MaBaiHoc > 0)
            {
                var original = DanhSachBaiHoc.FirstOrDefault(b => b.MaBaiHoc == _selectedBaiHoc.MaBaiHoc);
                if (original != null && CoThayDoiSoVoiGoc(original, _selectedBaiHoc))
                {
                    bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                        $"Bài học \"{_selectedBaiHoc.TieuDe}\" có các chỉnh sửa chưa được lưu.\nNếu chuyển sang soạn bài mới, các thay đổi chưa lưu sẽ bị mất.\n\nBạn có chắc chắn muốn chuyển không?",
                        "Xác nhận tạo bài mới",
                        confirmText: "Tiếp tục tạo",
                        cancelText: "Ở lại lưu");

                    if (!xacNhan) return;
                }
            }

            IsCreatingNewLesson = true;
            SelectedBaiHocInList = null;

            int defaultMaChuDe = DanhSachChuDe.FirstOrDefault()?.MaChuDe ?? 1;
            string defaultTenChuDe = DanhSachChuDe.FirstOrDefault()?.TenChuDe ?? string.Empty;

            SelectedBaiHoc = new BaiHocDTO
            {
                TieuDe = string.Empty,
                LoaiBaiHoc = "NguPhap",
                MaTrinhDo = "B1",
                MaChuDe = defaultMaChuDe,
                TenChuDe = defaultTenChuDe,
                ThoiGianUocTinhPhut = 20,
                DaXuatBan = false,
                NoiDung = string.Empty
            };
            ThongBaoLoi = string.Empty;
        }

        private void ThucHienXoaBaiHocHienTai()
        {
            if (IsAiBusy) return;
            if (SelectedBaiHoc == null || SelectedBaiHoc.MaBaiHoc <= 0)
            {
                if (IsCreatingNewLesson && CoDuLieuBaiHocMoiChuaLuu())
                {
                    bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                        "Bạn có chắc chắn muốn hủy bản nháp bài học mới đang soạn thảo không?",
                        "Xác nhận hủy bản nháp",
                        confirmText: "Hủy bản nháp",
                        cancelText: "Tiếp tục soạn");
                    if (xacNhan)
                    {
                        ThucHienThemBaiHocMoi();
                    }
                }
                return;
            }

            bool confirm = GUI.Views.Common.ThongBaoDialog.XacNhanXoa(
                $"Bạn có chắc chắn muốn xóa vĩnh viễn bài học:\n'{SelectedBaiHoc.TieuDe}'?\n\nThao tác này sẽ gỡ bỏ học liệu khỏi hệ thống và cơ sở dữ liệu.",
                "Xác nhận xóa bài học");

            if (confirm)
            {
                try
                {
                    string? phyFile = KhoHocLieuSyncHelper.TimTepVatLyChoBaiHoc(SelectedBaiHoc);
                    if (!string.IsNullOrEmpty(phyFile) && System.IO.File.Exists(phyFile))
                    {
                        System.IO.File.Delete(phyFile);
                    }
                }
                catch { }

                var (success, msg) = BaiHocBUS.XoaBaiHoc(SelectedBaiHoc.MaBaiHoc);
                if (success)
                {
                    Views.Common.AICauHoiUIHelper.XoaBanNhapTrongPhien(SelectedBaiHoc.MaBaiHoc);
                    GUI.Views.Common.ThongBaoDialog.ThanhCong(msg, "Đã xóa bài học");
                    LocDanhSach();
                    if (DanhSachBaiHoc.Count > 0)
                    {
                        SelectedBaiHocInList = DanhSachBaiHoc[0];
                    }
                    else
                    {
                        ThucHienThemBaiHocMoi();
                    }
                }
                else
                {
                    GUI.Views.Common.ThongBaoDialog.CanhBao(msg, "Lỗi khi xóa bài học");
                }
            }
        }

        private void ThucHienChonAudio()
        {
            if (!IsKyNangNghe)
            {
                ThongBaoLoi = "Chức năng đính kèm Audio chỉ khả dụng cho bài học có kỹ năng Nghe.";
                return;
            }

            var dlg = new OpenFileDialog
            {
                Filter = "Tệp âm thanh (*.mp3;*.wav;*.m4a;*.ogg;*.aac)|*.mp3;*.wav;*.m4a;*.ogg;*.aac|Tất cả tệp (*.*)|*.*",
                Title = "Chọn file ghi âm Audio bài giảng nghe"
            };

            // Ưu tiên mở vào thư mục KhoHocLieu hoặc TaiLieu_TestUpload
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string candTest1 = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, @"..\..\..\..\TaiLieu_TestUpload"));
                string candKho1 = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu"));
                if (System.IO.Directory.Exists(candTest1)) dlg.InitialDirectory = candTest1;
                else if (System.IO.Directory.Exists(candKho1)) dlg.InitialDirectory = candKho1;
            }
            catch { }

            if (dlg.ShowDialog() == true)
            {
                DungAudio();

                // Sao chép tệp audio vào thư mục KhoHocLieu theo chủ đề để đồng bộ toàn hệ thống
                var allChuDes = _danhMucBUS.LayDanhSachChuDe();
                string? dest = KhoHocLieuSyncHelper.SaoChepTepKhiUpload(dlg.FileName, SelectedBaiHoc.MaChuDe, SelectedBaiHoc.TieuDe + "_Audio", allChuDes);
                string savedPath = dest ?? dlg.FileName;

                SelectedBaiHoc.AudioPath = savedPath;
                SelectedBaiHoc.DuongDanAudio = savedPath;

                // Nếu bài học đã tồn tại trong CSDL, đồng bộ lưu ngay vào cơ sở dữ liệu
                if (SelectedBaiHoc.MaBaiHoc > 0)
                {
                    BaiHocBUS.LuuBaiHoc(SelectedBaiHoc, out _);
                }

                OnPropertyChanged(nameof(SelectedBaiHoc));
                OnPropertyChanged(nameof(HasAudioFile));
                NapAudioFileHienTai();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void ThucHienXoaAudio()
        {
            if (SelectedBaiHoc == null) return;

            bool confirm = GUI.Views.Common.ThongBaoDialog.XacNhanXoa(
                "Bạn có chắc chắn muốn gỡ tệp âm thanh hiện tại khỏi bài học này không?",
                "Xác nhận gỡ tệp âm thanh");
            if (!confirm) return;

            DungAudio();
            SelectedBaiHoc.AudioPath = null;
            SelectedBaiHoc.DuongDanAudio = null;

            if (SelectedBaiHoc.MaBaiHoc > 0)
            {
                BaiHocBUS.LuuBaiHoc(SelectedBaiHoc, out _);
            }

            OnPropertyChanged(nameof(SelectedBaiHoc));
            OnPropertyChanged(nameof(HasAudioFile));
            CommandManager.InvalidateRequerySuggested();
            GUI.Views.Common.ThongBaoDialog.ThanhCong("Đã gỡ tệp âm thanh khỏi bài học.", "Gỡ Audio");
        }

        public Uri? ResolveAudioUri(string? audioPath)
        {
            if (string.IsNullOrWhiteSpace(audioPath)) return null;

            string path = audioPath.Trim();
            if (System.IO.File.Exists(path))
            {
                return new Uri(System.IO.Path.GetFullPath(path));
            }

            if (Uri.TryCreate(path, UriKind.Absolute, out var absUri) && 
                (absUri.Scheme == Uri.UriSchemeHttp || absUri.Scheme == Uri.UriSchemeHttps || absUri.Scheme == Uri.UriSchemeFile))
            {
                return absUri;
            }

            // Tìm trong KhoHocLieu hoặc các thư mục học liệu của dự án nếu tệp bị đổi đường dẫn tương đối
            string fileName = System.IO.Path.GetFileName(path);
            foreach (var root in KhoHocLieuSyncHelper.GetAllHocLieuRoots())
            {
                if (System.IO.Directory.Exists(root))
                {
                    try
                    {
                        var found = System.IO.Directory.GetFiles(root, fileName, System.IO.SearchOption.AllDirectories).FirstOrDefault();
                        if (found != null && System.IO.File.Exists(found))
                        {
                            return new Uri(System.IO.Path.GetFullPath(found));
                        }
                    }
                    catch { }
                }
            }

            return null;
        }

        private void NapAudioFileHienTai()
        {
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc?.AudioPath))
            {
                AudioTotalSeconds = 100;
                AudioTotalTimeText = "00:00";
                AudioCurrentSeconds = 0;
                AudioCurrentTimeText = "00:00";
                return;
            }

            try
            {
                Uri? uri = ResolveAudioUri(SelectedBaiHoc.AudioPath);
                if (uri != null)
                {
                    _mediaPlayer.Open(uri);
                    _loadedAudioPath = SelectedBaiHoc.AudioPath.Trim();
                }
            }
            catch { }
        }

        private void AudioTimer_Tick(object? sender, EventArgs e)
        {
            if (IsPlayingAudio && !_isUserSeeking)
            {
                if ((_audioTotalSeconds <= 1 || _audioTotalTimeText == "00:00") && _mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    double total = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    AudioTotalSeconds = total > 0 ? total : 100;
                    AudioTotalTimeText = FormatTimeSpan(_mediaPlayer.NaturalDuration.TimeSpan);
                }

                _isTimerUpdating = true;
                try
                {
                    double pos = _mediaPlayer.Position.TotalSeconds;
                    if (_audioTotalSeconds > 0 && pos > _audioTotalSeconds)
                    {
                        pos = _audioTotalSeconds;
                    }
                    _audioCurrentSeconds = pos;
                    OnPropertyChanged(nameof(AudioCurrentSeconds));
                    AudioCurrentTimeText = FormatTimeSpan(TimeSpan.FromSeconds(pos));
                }
                finally
                {
                    _isTimerUpdating = false;
                }
            }
        }

        public void BatDauTuaAudio()
        {
            _isUserSeeking = true;
        }

        public void KetThucTuaAudio(double targetSeconds)
        {
            SeekTo(targetSeconds);
            _isUserSeeking = false;
        }

        public void SeekTo(double seconds)
        {
            try
            {
                if (seconds < 0) seconds = 0;
                if (_audioTotalSeconds > 0 && seconds > _audioTotalSeconds) seconds = _audioTotalSeconds;
                _mediaPlayer.Position = TimeSpan.FromSeconds(seconds);
                _audioCurrentSeconds = seconds;
                OnPropertyChanged(nameof(AudioCurrentSeconds));
                AudioCurrentTimeText = FormatTimeSpan(TimeSpan.FromSeconds(seconds));
            }
            catch { }
        }

        private static string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        private void ThucHienToggleAudio()
        {
            if (SelectedBaiHoc == null || string.IsNullOrWhiteSpace(SelectedBaiHoc.AudioPath)) return;

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
                    string path = SelectedBaiHoc.AudioPath.Trim();
                    Uri? uri = ResolveAudioUri(path);

                    if (uri == null)
                    {
                        ThongBaoLoi = "Không tìm thấy file âm thanh trên hệ thống.";
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

        public void DungAudio()
        {
            try
            {
                _audioTimer.Stop();
                if (IsPlayingAudio)
                {
                    _mediaPlayer.Stop();
                    IsPlayingAudio = false;
                }
                _mediaPlayer.Close();
                _loadedAudioPath = null;
                _audioCurrentSeconds = 0;
                AudioCurrentTimeText = "00:00";
                OnPropertyChanged(nameof(AudioCurrentSeconds));
            }
            catch { }
        }

        private void ThucHienLuuBaiHoc()
        {
            if (IsAiBusy) return;
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe))
            {
                ThongBaoLoi = "Vui lòng nhập tiêu đề bài học.";
                GUI.Views.Common.ThongBaoDialog.CanhBao("Vui lòng nhập tiêu đề bài học trước khi lưu.", "Thiếu thông tin");
                return;
            }

            if (SelectedBaiHoc.TieuDe.Trim().Length > 200)
            {
                ThongBaoLoi = "Tiêu đề bài học không được vượt quá 200 ký tự.";
                GUI.Views.Common.ThongBaoDialog.CanhBao("Tiêu đề bài học không được vượt quá 200 ký tự. Vui lòng rút gọn tiêu đề.", "Tiêu đề quá dài");
                return;
            }

            if (SelectedBaiHoc.ThoiGianUocTinhPhut <= 0 || SelectedBaiHoc.ThoiGianUocTinhPhut > 600)
            {
                ThongBaoLoi = "Thời lượng ước tính phải từ 1 đến 600 phút.";
                GUI.Views.Common.ThongBaoDialog.CanhBao("Thời lượng học ước tính phải từ 1 đến 600 phút (ví dụ: 15, 30, 45 phút).", "Thời lượng không hợp lệ");
                return;
            }

            if (SelectedBaiHoc.MaChuDe <= 0)
            {
                ThongBaoLoi = "Vui lòng chọn chủ đề kiến thức cho bài học.";
                GUI.Views.Common.ThongBaoDialog.CanhBao("Vui lòng chọn một chuyên đề kiến thức để phân loại bài học.", "Chưa chọn chủ đề");
                return;
            }

            if (!string.IsNullOrWhiteSpace(SelectedBaiHoc.TomTat) && SelectedBaiHoc.TomTat.Trim().Length > 500)
            {
                ThongBaoLoi = "Tóm tắt bài học không được vượt quá 500 ký tự.";
                GUI.Views.Common.ThongBaoDialog.CanhBao("Tóm tắt bài học không được vượt quá 500 ký tự. Vui lòng rút ngắn nội dung tóm tắt.", "Tóm tắt quá dài");
                return;
            }

            // Kiểm tra trùng tiêu đề bài học trong cùng một chủ đề
            var duplicate = DanhSachBaiHoc.FirstOrDefault(b =>
                b.MaBaiHoc != SelectedBaiHoc.MaBaiHoc &&
                b.MaChuDe == SelectedBaiHoc.MaChuDe &&
                string.Equals(b.TieuDe?.Trim(), SelectedBaiHoc.TieuDe.Trim(), StringComparison.OrdinalIgnoreCase));
            if (duplicate != null)
            {
                bool confirmLuu = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    $"Trong chủ đề này đã có bài học mang tiêu đề \"{SelectedBaiHoc.TieuDe.Trim()}\".\n\nBạn có chắc chắn muốn lưu bài học này với tiêu đề bị trùng không?",
                    "Cảnh báo trùng tiêu đề bài học",
                    confirmText: "Vẫn lưu",
                    cancelText: "Đổi tiêu đề");
                if (!confirmLuu) return;
            }

            // Kiểm tra ràng buộc âm thanh đối với kỹ năng nghe
            if (BaiHocDTO.KiemTraKyNangNghe(SelectedBaiHoc.LoaiBaiHoc) && !HasAudioFile)
            {
                bool tiepTuc = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    "Bài học này thuộc kỹ năng Luyện nghe (Listening) nhưng hiện tại chưa đính kèm tệp âm thanh (Audio).\n\nBạn có muốn tiếp tục lưu bài học này mà chưa có audio không? (Bạn có thể bổ sung audio sau)",
                    "Yêu cầu tệp âm thanh",
                    confirmText: "Vẫn lưu",
                    cancelText: "Bổ sung Audio");
                if (!tiepTuc) return;
            }

            // Kiểm tra nội dung giáo trình trống
            bool coNoiDung = false;
            if (!string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung))
            {
                string plain = System.Text.RegularExpressions.Regex.Replace(SelectedBaiHoc.NoiDung, "<.*?>", string.Empty)
                                                                   .Replace("&nbsp;", " ")
                                                                   .Trim();
                coNoiDung = !string.IsNullOrWhiteSpace(plain);
            }
            if (!coNoiDung)
            {
                bool xacNhanLuuRong = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    "Nội dung giáo trình lý thuyết của bài học hiện đang để trống.\n\nBạn có chắc chắn muốn lưu bài học này không?",
                    "Nội dung giáo trình trống",
                    confirmText: "Lưu bản nháp trống",
                    cancelText: "Tiếp tục soạn");
                if (!xacNhanLuuRong) return;
            }

            // Nếu cập nhật bài học đã xuất bản, xác nhận với giảng viên
            if (SelectedBaiHoc.MaBaiHoc > 0 && SelectedBaiHoc.DaXuatBan)
            {
                bool confirmCapNhat = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    $"Bài học \"{SelectedBaiHoc.TieuDe}\" đang ở trạng thái đã xuất bản cho học viên.\n\nBạn có chắc chắn muốn lưu các nội dung thay đổi mới lên bài học công khai này không?",
                    "Xác nhận cập nhật bài học đã xuất bản",
                    confirmText: "Lưu thay đổi",
                    cancelText: "Hủy bỏ");
                if (!confirmCapNhat) return;
            }

            ThongBaoLoi = string.Empty;
            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
            SelectedBaiHoc.NguoiTao = userId;

            bool ok = BaiHocBUS.LuuBaiHoc(SelectedBaiHoc, out string err);
            if (ok)
            {
                IsCreatingNewLesson = false;
                GUI.Views.Common.ThongBaoDialog.ThanhCong("Lưu nội dung bài học thành công!", "Thông báo");
                LocDanhSach();
            }
            else
            {
                ThongBaoLoi = "Lỗi khi lưu bài học: " + (string.IsNullOrWhiteSpace(err) ? "Bài học không tồn tại trong hệ thống hoặc đã bị xóa." : err);
                LocDanhSach();
            }
        }

        private void ThucHienToggleXuatBan(object? obj)
        {
            if (obj is BaiHocDTO bh)
            {
                if (bh.DaXuatBan)
                {
                    bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                        $"Bạn có chắc chắn muốn chuyển bài học \"{bh.TieuDe}\" về trạng thái Bản nháp không?\n\nLưu ý: Học viên sẽ tạm thời không thể nhìn thấy và truy cập bài học này trên hệ thống.",
                        "Xác nhận hủy xuất bản bài học",
                        confirmText: "Chuyển về Bản nháp",
                        cancelText: "Hủy bỏ");

                    if (!xacNhan) return;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(bh.TieuDe))
                    {
                        GUI.Views.Common.ThongBaoDialog.CanhBao("Bài học chưa có tiêu đề hợp lệ nên không thể xuất bản.", "Không thể xuất bản");
                        return;
                    }

                    bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                        $"Bạn có chắc chắn muốn xuất bản bài học \"{bh.TieuDe}\" không?\n\nBài học sẽ được công khai ngay lập tức cho toàn bộ học viên.",
                        "Xác nhận xuất bản bài học",
                        confirmText: "Xuất bản ngay",
                        cancelText: "Hủy bỏ");

                    if (!xacNhan) return;
                }

                bh.DaXuatBan = !bh.DaXuatBan;
                bool ok = BaiHocBUS.CapNhatTrangThaiXuatBan(bh.MaBaiHoc, bh.DaXuatBan);
                if (ok)
                {
                    GUI.Views.Common.ThongBaoDialog.ThanhCong(
                        bh.DaXuatBan ? $"Đã xuất bản bài học \"{bh.TieuDe}\" thành công!" : $"Đã chuyển bài học \"{bh.TieuDe}\" về Bản nháp.",
                        "Trạng thái xuất bản");
                }
                else
                {
                    ThongBaoLoi = "Không thể cập nhật trạng thái vì bài học này không còn tồn tại trong hệ thống.";
                }
                LocDanhSach();
            }
        }

        private async void ThucHienSinhNoiDungMauAI()
        {
            if (!CanTaoBangAI) return;
            if (SelectedBaiHoc == null) return;
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe))
            {
                ThongBaoLoi = "Vui lòng nhập tiêu đề bài học trước khi tạo bài học bằng AI.";
                return;
            }

            // RÀNG BUỘC BẢO VỆ DỮ LIỆU: Nếu bài học đã có nội dung, yêu cầu xác nhận trước khi ghi đè
            bool coNoiDung = false;
            if (!string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung))
            {
                string plain = System.Text.RegularExpressions.Regex.Replace(SelectedBaiHoc.NoiDung, "<.*?>", string.Empty)
                                                                   .Replace("&nbsp;", " ")
                                                                   .Trim();
                coNoiDung = !string.IsNullOrWhiteSpace(plain);
            }

            if (coNoiDung)
            {
                bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    "Bài học này hiện đã có nội dung giáo trình lý thuyết.\n\n" +
                    "⚠️ CẢNH BÁO: Nếu tiếp tục tạo bằng AI, toàn bộ nội dung giáo trình hiện tại sẽ bị xóa và thay thế bằng nội dung mới do AI soạn thảo.\n\n" +
                    "Bạn có chắc chắn muốn ghi đè và tạo lại nội dung bằng AI không?",
                    "Xác nhận ghi đè nội dung bài học",
                    "Ghi đè bằng AI",
                    "Giữ lại nội dung");

                if (!xacNhan)
                {
                    return;
                }
            }

            string? noiDungCu = SelectedBaiHoc.NoiDung;

            IsAiGeneratingSample = true;
            ThongBaoLoi = string.Empty;
            try
            {
                string tieuDe = SelectedBaiHoc.TieuDe.Trim();
                string trinhDo = string.IsNullOrWhiteSpace(SelectedBaiHoc.MaTrinhDo) ? "B1" : SelectedBaiHoc.MaTrinhDo;
                string loai = string.IsNullOrWhiteSpace(SelectedBaiHoc.LoaiBaiHoc) ? "Doc" : SelectedBaiHoc.LoaiBaiHoc;

                string content = await GeminiAIService.SinhNoiDungMauAIAsync(tieuDe, trinhDo, loai, "NoiDungHocTap");

                // Lưu lại nội dung cũ để giảng viên có thể hoàn tác khôi phục nếu cần
                if (coNoiDung && !string.IsNullOrWhiteSpace(noiDungCu))
                {
                    _noiDungBackupTruocKhiTaoAI = noiDungCu;
                    OnPropertyChanged(nameof(CoNoiDungBackupTruocKhiTaoAI));
                    CommandManager.InvalidateRequerySuggested();
                }

                SelectedBaiHoc.NoiDung = content;
                OnPropertyChanged(nameof(SelectedBaiHoc));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SinhNoiDungMauAI] {ex.Message}");
                ThongBaoLoi = "Không thể tạo bài học bằng AI: " + ex.Message;
            }
            finally
            {
                IsAiGeneratingSample = false;
            }
        }

        private void ThucHienHoanTacNoiDungCu()
        {
            if (SelectedBaiHoc != null && !string.IsNullOrEmpty(_noiDungBackupTruocKhiTaoAI))
            {
                SelectedBaiHoc.NoiDung = _noiDungBackupTruocKhiTaoAI;
                _noiDungBackupTruocKhiTaoAI = null;
                OnPropertyChanged(nameof(SelectedBaiHoc));
                OnPropertyChanged(nameof(CoNoiDungBackupTruocKhiTaoAI));
                CommandManager.InvalidateRequerySuggested();
                GUI.Views.Common.ThongBaoDialog.ThanhCong("Đã hoàn tác khôi phục lại nội dung giáo trình ban đầu của bạn!", "Hoàn tác thành công");
            }
        }

        public async Task SinhCauHoiAIAsync()
        {
            if (SelectedBaiHoc == null) return;
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe) && string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung))
            {
                ThongBaoLoi = "Vui lòng nhập tiêu đề hoặc nội dung bài học trước khi tạo câu hỏi bằng AI.";
                return;
            }

            string content = SelectedBaiHoc.NoiDung;
            if (string.IsNullOrWhiteSpace(content))
            {
                content = SelectedBaiHoc.TieuDe;
            }

            IsAiGeneratingQuestions = true;
            AiQuestionsStatusMessage = "AI đang phân tích bài học và tạo câu hỏi...";
            IsQuestionsModalOpen = true;

            try
            {
                string tieuDe = string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe) ? "Bài học tiếng Anh" : SelectedBaiHoc.TieuDe.Trim();
                string trinhDo = string.IsNullOrWhiteSpace(SelectedBaiHoc.MaTrinhDo) ? "B1" : SelectedBaiHoc.MaTrinhDo;
                string loai = string.IsNullOrWhiteSpace(SelectedBaiHoc.LoaiBaiHoc) ? "Doc" : SelectedBaiHoc.LoaiBaiHoc;

                var questions = await GeminiAIService.BocTachCauHoiAIAsync(tieuDe, content, trinhDo, 10, "NoiDungHocTap", loai);
                AiGeneratedQuestions.Clear();
                foreach (var q in questions)
                {
                    AiGeneratedQuestions.Add(q);
                }
                AiQuestionsStatusMessage = $"AI đã tạo thành công {AiGeneratedQuestions.Count} câu hỏi chuẩn hóa!";
            }
            catch (Exception ex)
            {
                AiQuestionsStatusMessage = "Lỗi khi tạo câu hỏi bằng AI: " + ex.Message;
            }
            finally
            {
                IsAiGeneratingQuestions = false;
            }
        }

        public async Task<System.Collections.Generic.List<CauHoiKhaoSatDTO>> LayHoacTaoCauHoiAIAsync(bool forceRecreate = false)
        {
            if (SelectedBaiHoc == null) return new System.Collections.Generic.List<CauHoiKhaoSatDTO>();

            if (!forceRecreate && Views.Common.AICauHoiUIHelper.CoBanNhapTrongPhien(SelectedBaiHoc.MaBaiHoc))
            {
                return Views.Common.AICauHoiUIHelper.LayBanNhapTrongPhien(SelectedBaiHoc.MaBaiHoc)!;
            }

            var (converted, kyNang, trinhDo, noiDung) = await BoCauHoiBUS.Instance.SinhCauHoiChoBaiHocAsync(
                SelectedBaiHoc,
                soLuong: 10,
                module: "AI_BaiHoc");

            if (converted != null && converted.Count > 0)
            {
                Views.Common.AICauHoiUIHelper.LuuBanNhapTrongPhien(SelectedBaiHoc.MaBaiHoc, converted);
            }

            return converted ?? new System.Collections.Generic.List<CauHoiKhaoSatDTO>();
        }

        private async void ThucHienSinhCauHoiAI()
        {
            if (SelectedBaiHoc == null) return;
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe) && string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung))
            {
                GUI.Views.Common.ThongBaoDialog.CanhBao("Vui lòng nhập tiêu đề hoặc soạn nội dung bài học trước khi tạo câu hỏi bằng AI.", "Thiếu thông tin");
                return;
            }

            if (AiGeneratedQuestions != null && AiGeneratedQuestions.Count > 0)
            {
                bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                    $"Bài học đã có sẵn danh sách {AiGeneratedQuestions.Count} câu hỏi AI vừa tạo trong phiên.\n\nBạn có muốn yêu cầu AI tạo lại bộ câu hỏi mới hoàn toàn không?",
                    "Xác nhận tạo lại câu hỏi AI",
                    confirmText: "Tạo lại mới",
                    cancelText: "Xem lại câu hỏi cũ");

                if (!xacNhan)
                {
                    IsQuestionsModalOpen = true;
                    return;
                }
            }

            await SinhCauHoiAIAsync();
        }

        public void ThucHienChenCauHoiVaoBaiHoc()
        {
            if (SelectedBaiHoc == null || AiGeneratedQuestions.Count == 0) return;

            var sb = new System.Text.StringBuilder();
            if (!string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung))
            {
                sb.AppendLine(SelectedBaiHoc.NoiDung.TrimEnd());
                sb.AppendLine();
            }

            sb.AppendLine("■ BỘ CÂU HỎI TRẮC NGHIỆM ÔN TẬP (AI):");
            int qIdx = 1;
            foreach (var q in AiGeneratedQuestions)
            {
                sb.AppendLine($"{qIdx}. {q.NoiDungCauHoi}");
                if (q.DanhSachLuaChon != null)
                {
                    for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                    {
                        string mark = (i == q.DapAnDungIndex) ? " (Đúng ✓)" : "";
                        sb.AppendLine($"   {q.DanhSachLuaChon[i]}{mark}");
                    }
                }
                if (!string.IsNullOrWhiteSpace(q.GiaiThichChiTiet))
                {
                    sb.AppendLine($"   ➔ {q.GiaiThichChiTiet}");
                }
                sb.AppendLine();
                qIdx++;
            }

            SelectedBaiHoc.NoiDung = sb.ToString().TrimEnd();
            OnPropertyChanged(nameof(SelectedBaiHoc));
            IsQuestionsModalOpen = false;
        }

        private void ThucHienThemCauHoiTracNghiem()
        {
            if (SelectedBaiHoc == null) return;
            string template = "\n\n■ CÂU HỎI TRẮC NGHIỆM (MULTIPLE CHOICE):\n" +
                              "1. Question: What is the main idea of this lesson?\n" +
                              "   A. Option A\n" +
                              "   B. Option B (Đúng ✓)\n" +
                              "   C. Option C\n" +
                              "   D. Option D\n" +
                              "   ➔ Giải thích: [Nhập giải thích và dẫn chứng từ bài...]";
            SelectedBaiHoc.NoiDung = (SelectedBaiHoc.NoiDung ?? "").TrimEnd() + template;
            OnPropertyChanged(nameof(SelectedBaiHoc));
        }

        private void ThucHienThemCauHoiTuLuan()
        {
            if (SelectedBaiHoc == null) return;
            string template = "\n\n■ CÂU HỎI TỰ LUẬN (FREE TEXT):\n" +
                              "Question: Write a short paragraph (50-80 words) describing your daily work routine using the phrases learned in this lesson.\n" +
                              "➔ Model Answer: In my daily routine, I start by reviewing task priorities and participating in the stand-up meeting...";
            SelectedBaiHoc.NoiDung = (SelectedBaiHoc.NoiDung ?? "").TrimEnd() + template;
            OnPropertyChanged(nameof(SelectedBaiHoc));
        }

        private void ThucHienThemCauHoiDienTu()
        {
            if (SelectedBaiHoc == null) return;
            string template = "\n\n■ CÂU HỎI ĐIỀN TỪ (FILL IN THE BLANK):\n" +
                              "Complete the sentence with the appropriate word or phrase:\n" +
                              "\"Today, I am going to ________ the payment gateway API.\"\n" +
                              "➔ Answer: integrate";
            SelectedBaiHoc.NoiDung = (SelectedBaiHoc.NoiDung ?? "").TrimEnd() + template;
            OnPropertyChanged(nameof(SelectedBaiHoc));
        }
    }
}
