using System;
using System.Collections.Generic;
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
        // =========================================================================
        // NESTED VIEW MODELS
        // =========================================================================
        public class QuestionSourceOption
        {
            public int Id { get; set; } // 0: Toàn bộ ngân hàng đề, >0: Mã bộ câu hỏi
            public string DisplayText { get; set; } = string.Empty;
        }

        public class QuestionSelectionItem : ViewModelBase
        {
            private bool _isSelected;
            private double _diem = 1.0;
            private int _thuTu = 1;
            public Action? OnChanged { get; set; }

            public CauHoiKhaoSatDTO CauHoi { get; set; } = new();

            public double Diem
            {
                get => _diem;
                set
                {
                    if (SetProperty(ref _diem, value))
                    {
                        OnChanged?.Invoke();
                    }
                }
            }

            public int ThuTu
            {
                get => _thuTu;
                set => SetProperty(ref _thuTu, value);
            }

            private bool _isAnswersExpanded;
            public bool IsAnswersExpanded
            {
                get => _isAnswersExpanded;
                set => SetProperty(ref _isAnswersExpanded, value);
            }

            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    if (SetProperty(ref _isSelected, value))
                    {
                        OnChanged?.Invoke();
                    }
                }
            }
        }

        // =========================================================================
        // STATE FIELDS
        // =========================================================================
        private bool _isDanhSachMode = true;
        private bool _isPreviewModalOpen = false;
        private bool _isHistoryModalOpen = false;
        private BaiTapDeThiDTO _currentDeThi = new();
        private BaiTapDeThiDTO? _selectedExam;
        private string _thongBaoLoi = string.Empty;

        // Bộ lọc danh sách đề thi
        private string _searchExamKeyword = string.Empty;
        private string _filterExamLevel = "All";
        private string _filterExamStatus = "All";
        private string _filterExamType = "All";

        // Bộ lọc chọn câu hỏi từ ngân hàng
        public const int ALL_SOURCE_ID = -1; // -1 = Toàn bộ ngân hàng đề, >=0 = Mã bộ cụ thể
        private int _selectedSourceId = ALL_SOURCE_ID;
        private string _filterSkill = "All";
        private string _filterQuestionLevel = "All";
        private string _filterDifficulty = "All";
        private string _searchQuestionKeyword = string.Empty;
        private bool _isShowOnlySelected = false;
        private bool _isLoadingQuestions = false;

        public bool IsLoadingQuestions
        {
            get => _isLoadingQuestions;
            set => SetProperty(ref _isLoadingQuestions, value);
        }

        // Khóa đồng bộ cho ObservableCollection cập nhật đa luồng an toàn
        private readonly object _collectionLock = new();

        // Bộ nhớ đệm danh sách item câu hỏi để tối ưu hóa hiệu năng, chống giật lag
        private readonly Dictionary<int, QuestionSelectionItem> _allQuestionItemsCache = new();

        // Bộ lưu trữ master tất cả câu hỏi đã chọn trong đề thi (bảo toàn khi lọc)
        private readonly Dictionary<int, QuestionSelectionItem> _selectedQuestionsMaster = new();

        // Delegates mở Dialog hiển thị ngoài toàn màn hình
        public Action<BaiTapDeThiDTO, List<LuotLamBaiDTO>>? YeuCauHienThiLichSuBaiNop { get; set; }
        public Action<BaiTapDeThiDTO, List<CauHoiKhaoSatDTO>>? YeuCauHienThiXemTruoc { get; set; }

        // =========================================================================
        // COLLECTIONS
        // =========================================================================
        public ObservableCollection<BaiTapDeThiDTO> DanhSachDeThiHienCo { get; set; } = new();
        public ObservableCollection<BaiTapDeThiDTO> DanhSachDeThiLoc { get; set; } = new();
        public ObservableCollection<TrinhDoDTO> DanhSachTrinhDo { get; set; } = new();
        public ObservableCollection<BoCauHoiDTO> DanhSachBoCauHoi { get; set; } = new();
        public ObservableCollection<QuestionSourceOption> DanhSachNguonCauHoi { get; set; } = new();
        public ObservableCollection<QuestionSelectionItem> NgânHangCauHoiChon { get; set; } = new();
        public ObservableCollection<CauTrucDeThiDTO> DanhSachCauTrucDeThi { get; set; } = new();
        public ObservableCollection<ChiTietCauTrucDeThiDTO> DanhSachPhanThiHienTai { get; set; } = new();

        // Modals
        public ObservableCollection<CauHoiKhaoSatDTO> PreviewQuestions { get; set; } = new();
        public ObservableCollection<LuotLamBaiDTO> ExamAttemptHistory { get; set; } = new();

        // =========================================================================
        // PROPERTIES
        // =========================================================================
        public bool IsDanhSachMode
        {
            get => _isDanhSachMode;
            set
            {
                if (SetProperty(ref _isDanhSachMode, value))
                {
                    OnPropertyChanged(nameof(IsChinhSuaMode));
                    if (!_isDanhSachMode && NgânHangCauHoiChon.Count == 0)
                    {
                        NapCauHoiTheoBoLoc();
                    }
                }
            }
        }

        public bool IsChinhSuaMode => !_isDanhSachMode;

        public bool IsPreviewModalOpen
        {
            get => _isPreviewModalOpen;
            set => SetProperty(ref _isPreviewModalOpen, value);
        }

        public bool IsHistoryModalOpen
        {
            get => _isHistoryModalOpen;
            set => SetProperty(ref _isHistoryModalOpen, value);
        }

        public BaiTapDeThiDTO CurrentDeThi
        {
            get => _currentDeThi;
            set => SetProperty(ref _currentDeThi, value);
        }

        private CauTrucDeThiDTO? _selectedCauTrucDeThi;
        public CauTrucDeThiDTO? SelectedCauTrucDeThi
        {
            get => _selectedCauTrucDeThi;
            set
            {
                if (SetProperty(ref _selectedCauTrucDeThi, value))
                {
                    OnCauTrucDeThiChanged();
                }
            }
        }

        public bool CoCacPhanThi => DanhSachPhanThiHienTai.Count > 0;

        public BaiTapDeThiDTO? SelectedExam
        {
            get => _selectedExam;
            set => SetProperty(ref _selectedExam, value);
        }

        public string ThongBaoLoi
        {
            get => _thongBaoLoi;
            set => SetProperty(ref _thongBaoLoi, value);
        }

        // Filters - Exam List
        public string SearchExamKeyword
        {
            get => _searchExamKeyword;
            set
            {
                if (SetProperty(ref _searchExamKeyword, value))
                    ApDungBoLocDeThi();
            }
        }

        public string FilterExamLevel
        {
            get => _filterExamLevel;
            set
            {
                if (SetProperty(ref _filterExamLevel, value))
                    ApDungBoLocDeThi();
            }
        }

        public string FilterExamStatus
        {
            get => _filterExamStatus;
            set
            {
                if (SetProperty(ref _filterExamStatus, value))
                    ApDungBoLocDeThi();
            }
        }

        public string FilterExamType
        {
            get => _filterExamType;
            set
            {
                if (SetProperty(ref _filterExamType, value))
                    ApDungBoLocDeThi();
            }
        }

        // Filters - Question Bank Selection
        private QuestionSourceOption? _selectedNguonCauHoi;
        public QuestionSourceOption? SelectedNguonCauHoi
        {
            get => _selectedNguonCauHoi;
            set
            {
                if (SetProperty(ref _selectedNguonCauHoi, value))
                {
                    _selectedSourceId = value?.Id ?? ALL_SOURCE_ID;
                    OnPropertyChanged(nameof(SelectedSourceId));
                    NapCauHoiTheoBoLoc();
                }
            }
        }

        public int SelectedSourceId
        {
            get => _selectedSourceId;
            set
            {
                if (SetProperty(ref _selectedSourceId, value))
                {
                    _selectedNguonCauHoi = DanhSachNguonCauHoi.FirstOrDefault(x => x.Id == value) ?? DanhSachNguonCauHoi.FirstOrDefault();
                    OnPropertyChanged(nameof(SelectedNguonCauHoi));
                    NapCauHoiTheoBoLoc();
                }
            }
        }

        // Audio state
        private System.Windows.Media.MediaPlayer? _mediaPlayer;
        private bool _isPlayingAudio = false;
        public bool IsPlayingAudio
        {
            get => _isPlayingAudio;
            set => SetProperty(ref _isPlayingAudio, value);
        }

        public string FilterSkill
        {
            get => _filterSkill;
            set
            {
                if (SetProperty(ref _filterSkill, value))
                    NapCauHoiTheoBoLoc();
            }
        }

        public string FilterQuestionLevel
        {
            get => _filterQuestionLevel;
            set
            {
                if (SetProperty(ref _filterQuestionLevel, value))
                    NapCauHoiTheoBoLoc();
            }
        }

        public string FilterDifficulty
        {
            get => _filterDifficulty;
            set
            {
                if (SetProperty(ref _filterDifficulty, value))
                    NapCauHoiTheoBoLoc();
            }
        }

        public string SearchQuestionKeyword
        {
            get => _searchQuestionKeyword;
            set
            {
                if (SetProperty(ref _searchQuestionKeyword, value))
                    NapCauHoiTheoBoLoc();
            }
        }

        public bool IsShowOnlySelected
        {
            get => _isShowOnlySelected;
            set
            {
                if (SetProperty(ref _isShowOnlySelected, value))
                    NapCauHoiTheoBoLoc();
            }
        }

        // Statistics
        public int TongSoDeThi => DanhSachDeThiHienCo.Count;
        public int SoDeDaXuatBan => DanhSachDeThiHienCo.Count(d => d.IsPublished);
        public int SoDeBanNhap => DanhSachDeThiHienCo.Count(d => d.IsDraft);
        public int TongLuotHocVienThi => DanhSachDeThiHienCo.Sum(d => d.SoLuotDaThi);

        public int TongSoCauDaChon => _selectedQuestionsMaster.Count;
        public double TongDiemDeThi => Math.Round(_selectedQuestionsMaster.Values.Sum(x => x.Diem), 2);

        // THUỘC TÍNH RÀNG BUỘC VÀ TIẾN ĐỘ THEO CẤU TRÚC ĐỀ THI (BLUEPRINT MATRIX)
        public bool HasSelectedBlueprint => _selectedCauTrucDeThi != null && _selectedCauTrucDeThi.MaCauTruc > 0;
        public int SoCauYeuCauCauTruc => _selectedCauTrucDeThi?.TongSoCau ?? 0;
        public bool IsKhopCauTruc => HasSelectedBlueprint && TongSoCauDaChon == SoCauYeuCauCauTruc;
        public int SoCauConThieu => HasSelectedBlueprint && TongSoCauDaChon < SoCauYeuCauCauTruc ? (SoCauYeuCauCauTruc - TongSoCauDaChon) : 0;
        public int SoCauDu => HasSelectedBlueprint && TongSoCauDaChon > SoCauYeuCauCauTruc ? (TongSoCauDaChon - SoCauYeuCauCauTruc) : 0;

        public string ThongBaoTrangThaiCauTruc
        {
            get
            {
                if (!HasSelectedBlueprint)
                    return "Đề thi tự do (không áp dụng ma trận chuẩn hóa)";
                if (IsKhopCauTruc)
                    return $"Khớp 100% cấu trúc: Đã chọn đúng {TongSoCauDaChon}/{SoCauYeuCauCauTruc} câu hỏi chuẩn!";
                if (SoCauConThieu > 0)
                    return $"Chưa đủ câu: Đang chọn {TongSoCauDaChon}/{SoCauYeuCauCauTruc} câu (Thiếu {SoCauConThieu} câu). Bấm 'Tự động chọn câu' để hoàn tất!";
                return $"Vượt quá số câu: Đang chọn {TongSoCauDaChon}/{SoCauYeuCauCauTruc} câu (Dư {SoCauDu} câu). Cần bỏ bớt {SoCauDu} câu!";
            }
        }

        public LucideIconKind IconTrangThaiCauTruc => IsKhopCauTruc ? LucideIconKind.CheckCircle : (HasSelectedBlueprint ? LucideIconKind.AlertCircle : LucideIconKind.Info);

        public string MauNenTrangThaiCauTruc => IsKhopCauTruc ? "#DCFCE7" : (HasSelectedBlueprint ? "#FEF3C7" : "#F1F5F9");
        public string MauVienTrangThaiCauTruc => IsKhopCauTruc ? "#86EFAC" : (HasSelectedBlueprint ? "#FCD34D" : "#CBD5E1");
        public string MauChuTrangThaiCauTruc => IsKhopCauTruc ? "#15803D" : (HasSelectedBlueprint ? "#B45309" : "#475569");
        public string ChuoiTienDoCauChon => HasSelectedBlueprint ? $"{TongSoCauDaChon}/{SoCauYeuCauCauTruc} câu" : $"{TongSoCauDaChon} câu";

        // =========================================================================
        // COMMANDS
        // =========================================================================
        public ICommand ThemDeThiMoiCommand { get; }
        public ICommand ChonSuaDeThiCommand { get; }
        public ICommand XemTruocDeThiCommand { get; }
        public ICommand DongPreviewModalCommand { get; }
        public ICommand XuatBanDeThiCommand { get; }
        public ICommand DongDeThiCommand { get; }
        public ICommand NhanBanDeThiCommand { get; }
        public ICommand XoaDeThiCommand { get; }
        public ICommand XemLichSuDeThiCommand { get; }
        public ICommand DongHistoryModalCommand { get; }
        public ICommand QuayLaiDanhSachCommand { get; }

        public ICommand LuuBanNhapCommand { get; }
        public ICommand LuuVaXuatBanCommand { get; }
        public ICommand CapNhatLuaChonCauHoiCommand { get; }
        public ICommand ChonTatCaCauHoiCommand { get; }
        public ICommand BoChonTatCaCommand { get; }
        public ICommand DiChuyenCauLenCommand { get; }
        public ICommand DiChuyenCauXuongCommand { get; }
        public ICommand LamMoiDanhSachCommand { get; }

        public ICommand ToggleChonCauHoiCommand { get; }
        public ICommand ToggleXemDapAnCommand { get; }

        public ICommand ChonAudioDeThiCommand { get; }
        public ICommand XoaAudioDeThiCommand { get; }
        public ICommand TogglePlayAudioDeThiCommand { get; }
        public ICommand ApDungThongSoCauTrucCommand { get; }
        public ICommand TuDongChonCauHoiTheoCauTrucCommand { get; }

        // =========================================================================
        // CONSTRUCTOR
        // =========================================================================
        public DeThiLuyenTapViewModel()
        {
            ThemDeThiMoiCommand = new RelayCommand(ThucHienThemDeThiMoi);
            ChonSuaDeThiCommand = new RelayCommand<BaiTapDeThiDTO>(ThucHienChonSuaDeThi);
            XemTruocDeThiCommand = new RelayCommand<BaiTapDeThiDTO>(ThucHienXemTruocDeThi);
            DongPreviewModalCommand = new RelayCommand(() => IsPreviewModalOpen = false);
            XuatBanDeThiCommand = new RelayCommand<BaiTapDeThiDTO>(ThucHienXuatBanDeThi);
            DongDeThiCommand = new RelayCommand<BaiTapDeThiDTO>(ThucHienDongDeThi);
            NhanBanDeThiCommand = new RelayCommand<BaiTapDeThiDTO>(ThucHienNhanBanDeThi);
            XoaDeThiCommand = new RelayCommand<BaiTapDeThiDTO>(ThucHienXoaDeThi);
            XemLichSuDeThiCommand = new RelayCommand<BaiTapDeThiDTO>(ThucHienXemLichSuDeThi);
            DongHistoryModalCommand = new RelayCommand(() => IsHistoryModalOpen = false);
            QuayLaiDanhSachCommand = new RelayCommand(ThucHienQuayLaiDanhSach);

            LuuBanNhapCommand = new RelayCommand(() => ThucHienLuuDeThi(xuatBan: false));
            LuuVaXuatBanCommand = new RelayCommand(() => ThucHienLuuDeThi(xuatBan: true));
            CapNhatLuaChonCauHoiCommand = new RelayCommand(ThucHienCapNhatThongKeCauHoi);
            ChonTatCaCauHoiCommand = new RelayCommand(() => SetAllSelection(true));
            BoChonTatCaCommand = new RelayCommand(() => SetAllSelection(false));
            DiChuyenCauLenCommand = new RelayCommand<QuestionSelectionItem>(ThucHienDiChuyenCauLen);
            DiChuyenCauXuongCommand = new RelayCommand<QuestionSelectionItem>(ThucHienDiChuyenCauXuong);
            LamMoiDanhSachCommand = new RelayCommand(TaiDuLieuDeThi);

            ToggleChonCauHoiCommand = new RelayCommand<QuestionSelectionItem>(ThucHienToggleChonCauHoi);
            ToggleXemDapAnCommand = new RelayCommand<QuestionSelectionItem>(ThucHienToggleXemDapAn);

            ChonAudioDeThiCommand = new RelayCommand(ThucHienChonAudioDeThi);
            XoaAudioDeThiCommand = new RelayCommand(ThucHienXoaAudioDeThi);
            TogglePlayAudioDeThiCommand = new RelayCommand(ThucHienTogglePlayAudioDeThi);
            ApDungThongSoCauTrucCommand = new RelayCommand(ThucHienApDungThongSoCauTruc);
            TuDongChonCauHoiTheoCauTrucCommand = new RelayCommand(ThucHienTuDongChonCauHoiTheoCauTruc);

            // Cho phép cập nhật ObservableCollection từ luồng nền an toàn
            System.Windows.Data.BindingOperations.EnableCollectionSynchronization(NgânHangCauHoiChon, _collectionLock);

            TaiDuLieuDeThi();
            KhoiTaoDeThiMoi();
        }

        // =========================================================================
        // DATA LOADING & FILTERING
        // =========================================================================
        public void TaiDuLieuDeThi()
        {
            NapDanhSachCauTrucDeThi();
            var ds = BaiTapDeThiBUS.Instance.LayDanhSachDeThi();
            DanhSachDeThiHienCo.Clear();
            foreach (var d in ds) DanhSachDeThiHienCo.Add(d);

            var dmBUS = new DanhMucBUS();
            DanhSachTrinhDo.Clear();
            foreach (var td in dmBUS.LayDanhSachTrinhDo()) DanhSachTrinhDo.Add(td);

            var dsBo = BoCauHoiDAL.Instance.LayDanhSachBoCauHoi();
            DanhSachBoCauHoi.Clear();
            DanhSachNguonCauHoi.Clear();
            var defaultOption = new QuestionSourceOption { Id = ALL_SOURCE_ID, DisplayText = "Toàn bộ ngân hàng đề" };
            DanhSachNguonCauHoi.Add(defaultOption);

            foreach (var bo in dsBo)
            {
                DanhSachBoCauHoi.Add(bo);
                string nguon = string.IsNullOrWhiteSpace(bo.NguonTao) ? "" : $" [{bo.NguonTao}]";
                DanhSachNguonCauHoi.Add(new QuestionSourceOption
                {
                    Id = bo.MaBoCauHoi,
                    DisplayText = $"📁 {bo.TenBoCauHoi}{nguon}"
                });
            }

            // Luôn đặt mặc định là Toàn bộ ngân hàng đề
            _selectedSourceId = ALL_SOURCE_ID;
            _selectedNguonCauHoi = defaultOption;
            OnPropertyChanged(nameof(SelectedNguonCauHoi));
            OnPropertyChanged(nameof(SelectedSourceId));

            ApDungBoLocDeThi();
            CapNhatThongKeKpi();
        }

        private void ApDungBoLocDeThi()
        {
            var query = DanhSachDeThiHienCo.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchExamKeyword))
            {
                string kw = SearchExamKeyword.Trim().ToLowerInvariant();
                query = query.Where(d => 
                    (d.TenBaiTapDeThi != null && d.TenBaiTapDeThi.ToLowerInvariant().Contains(kw)) ||
                    (d.MoTa != null && d.MoTa.ToLowerInvariant().Contains(kw))
                );
            }

            if (!string.IsNullOrWhiteSpace(FilterExamLevel) && FilterExamLevel != "All")
            {
                if (FilterExamLevel == "Basic")
                    query = query.Where(d => string.Equals(d.MaCapDo, "A1", StringComparison.OrdinalIgnoreCase) || string.Equals(d.MaCapDo, "A2", StringComparison.OrdinalIgnoreCase));
                else if (FilterExamLevel == "Intermediate")
                    query = query.Where(d => string.Equals(d.MaCapDo, "B1", StringComparison.OrdinalIgnoreCase) || string.Equals(d.MaCapDo, "B2", StringComparison.OrdinalIgnoreCase));
                else if (FilterExamLevel == "Advanced")
                    query = query.Where(d => string.Equals(d.MaCapDo, "C1", StringComparison.OrdinalIgnoreCase) || string.Equals(d.MaCapDo, "C2", StringComparison.OrdinalIgnoreCase));
                else
                    query = query.Where(d => string.Equals(d.MaCapDo, FilterExamLevel, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(FilterExamStatus) && FilterExamStatus != "All")
            {
                query = query.Where(d => string.Equals(d.TrangThai, FilterExamStatus, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(FilterExamType) && FilterExamType != "All")
            {
                query = query.Where(d => string.Equals(d.LoaiHinh, FilterExamType, StringComparison.OrdinalIgnoreCase));
            }

            DanhSachDeThiLoc.Clear();
            foreach (var item in query)
            {
                DanhSachDeThiLoc.Add(item);
            }
        }

        private void CapNhatThongKeKpi()
        {
            OnPropertyChanged(nameof(TongSoDeThi));
            OnPropertyChanged(nameof(SoDeDaXuatBan));
            OnPropertyChanged(nameof(SoDeBanNhap));
            OnPropertyChanged(nameof(TongLuotHocVienThi));
        }

        public async void NapCauHoiTheoBoLoc()
        {
            if (IsDanhSachMode) return;

            if (IsShowOnlySelected)
            {
                var listSelected = _selectedQuestionsMaster.Values.OrderBy(x => x.ThuTu).ToList();

                // 1. Lọc theo từ khóa tìm kiếm
                if (!string.IsNullOrWhiteSpace(SearchQuestionKeyword))
                {
                    string kw = SearchQuestionKeyword.Trim().ToLowerInvariant();
                    listSelected = listSelected.Where(x => 
                        (x.CauHoi.NoiDungCauHoi != null && x.CauHoi.NoiDungCauHoi.ToLowerInvariant().Contains(kw)) ||
                        (x.CauHoi.DoanVanDoc != null && x.CauHoi.DoanVanDoc.ToLowerInvariant().Contains(kw))
                    ).ToList();
                }

                // 2. Lọc theo Nguồn câu hỏi
                if (SelectedSourceId >= 0)
                {
                    try
                    {
                        var boQIds = DAL.BoCauHoiDAL.Instance.LayDanhSachCauHoiTheoBo(SelectedSourceId).Select(q => q.MaCauHoi).ToHashSet();
                        listSelected = listSelected.Where(x => boQIds.Contains(x.CauHoi.MaCauHoi)).ToList();
                    }
                    catch { }
                }

                // 3. Lọc theo Kỹ năng
                if (!string.IsNullOrWhiteSpace(FilterSkill) && 
                    !string.Equals(FilterSkill, "All", StringComparison.OrdinalIgnoreCase) &&
                    !FilterSkill.Contains("All", StringComparison.OrdinalIgnoreCase) &&
                    !FilterSkill.Contains("Tất cả", StringComparison.OrdinalIgnoreCase))
                {
                    listSelected = listSelected.Where(x => 
                        string.Equals(x.CauHoi.KyNang, FilterSkill, StringComparison.OrdinalIgnoreCase) ||
                        (FilterSkill == "Doc" && (x.CauHoi.KyNang == "Doc" || x.CauHoi.KyNang == "Reading")) ||
                        (FilterSkill == "Nghe" && (x.CauHoi.KyNang == "Nghe" || x.CauHoi.KyNang == "Listening")) ||
                        (FilterSkill == "NguPhap" && (x.CauHoi.KyNang == "NguPhap" || x.CauHoi.KyNang == "Grammar")) ||
                        (FilterSkill == "TuVung" && (x.CauHoi.KyNang == "TuVung" || x.CauHoi.KyNang == "Vocabulary")) ||
                        (FilterSkill == "Viet" && (x.CauHoi.KyNang == "Viet" || x.CauHoi.KyNang == "Writing")) ||
                        (FilterSkill == "Noi" && (x.CauHoi.KyNang == "Noi" || x.CauHoi.KyNang == "Speaking"))
                    ).ToList();
                }

                // 4. Lọc theo Cấp độ (A1, A2, B1, B2, C1)
                if (!string.IsNullOrWhiteSpace(FilterQuestionLevel) && 
                    !string.Equals(FilterQuestionLevel, "All", StringComparison.OrdinalIgnoreCase) &&
                    !FilterQuestionLevel.Contains("All", StringComparison.OrdinalIgnoreCase) &&
                    !FilterQuestionLevel.Contains("Tất cả", StringComparison.OrdinalIgnoreCase))
                {
                    listSelected = listSelected.Where(x => 
                        string.Equals(x.CauHoi.MaTrinhDo, FilterQuestionLevel, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(x.CauHoi.TenTrinhDo, FilterQuestionLevel, StringComparison.OrdinalIgnoreCase) ||
                        (x.CauHoi.TenTrinhDo != null && x.CauHoi.TenTrinhDo.IndexOf(FilterQuestionLevel, StringComparison.OrdinalIgnoreCase) >= 0)
                    ).ToList();
                }

                // 5. Lọc theo Độ khó (De, TrungBinh, Kho)
                if (!string.IsNullOrWhiteSpace(FilterDifficulty) && 
                    !string.Equals(FilterDifficulty, "All", StringComparison.OrdinalIgnoreCase) &&
                    !FilterDifficulty.Contains("All", StringComparison.OrdinalIgnoreCase) &&
                    !FilterDifficulty.Contains("Tất cả", StringComparison.OrdinalIgnoreCase))
                {
                    listSelected = listSelected.Where(x => 
                        string.Equals(x.CauHoi.DoKho, FilterDifficulty, StringComparison.OrdinalIgnoreCase) ||
                        (FilterDifficulty == "De" && (x.CauHoi.DoKho == "De" || x.CauHoi.DoKho == "Easy")) ||
                        (FilterDifficulty == "TrungBinh" && (x.CauHoi.DoKho == "TrungBinh" || x.CauHoi.DoKho == "Medium")) ||
                        (FilterDifficulty == "Kho" && (x.CauHoi.DoKho == "Kho" || x.CauHoi.DoKho == "Hard"))
                    ).ToList();
                }

                lock (_collectionLock)
                {
                    NgânHangCauHoiChon.Clear();
                    foreach (var item in listSelected)
                    {
                        NgânHangCauHoiChon.Add(item);
                    }
                }
                OnPropertyChanged(nameof(NgânHangCauHoiChon));
                ThucHienCapNhatThongKeCauHoi();
                return;
            }

            int targetSourceId = SelectedSourceId;
            string tuKhoa = SearchQuestionKeyword;
            string kyNang = FilterSkill;
            string doKho = FilterDifficulty;
            string trinhDo = FilterQuestionLevel;

            IsLoadingQuestions = true;
            try
            {
                var dsCauHoi = await System.Threading.Tasks.Task.Run(() =>
                {
                    List<CauHoiKhaoSatDTO> list;
                    if (targetSourceId >= 0)
                    {
                        list = BoCauHoiDAL.Instance.LayDanhSachCauHoiTheoBo(
                            targetSourceId, 
                            tuKhoa, 
                            kyNang, 
                            doKho,
                            daKiemDuyet: true);
                    }
                    else
                    {
                        list = BoCauHoiDAL.Instance.LayTatCaCauHoi(
                            tuKhoa, 
                            kyNang, 
                            doKho, 
                            1000,
                            daKiemDuyet: true);
                    }

                    if (!string.IsNullOrWhiteSpace(trinhDo) && 
                        !string.Equals(trinhDo, "All", StringComparison.OrdinalIgnoreCase) &&
                        !trinhDo.Contains("All", StringComparison.OrdinalIgnoreCase) &&
                        !trinhDo.Contains("Tất cả", StringComparison.OrdinalIgnoreCase))
                    {
                        list = list.Where(q => string.Equals(q.MaTrinhDo, trinhDo, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    return list.Where(q => q.DaKiemDuyet).ToList();
                });

                var itemsToAdd = new List<QuestionSelectionItem>();
                int stt = _selectedQuestionsMaster.Count + 1;

                foreach (var ch in dsCauHoi)
                {
                    if (_selectedQuestionsMaster.TryGetValue(ch.MaCauHoi, out var existing))
                    {
                        itemsToAdd.Add(existing);
                    }
                    else
                    {
                        if (!_allQuestionItemsCache.TryGetValue(ch.MaCauHoi, out var cachedItem))
                        {
                            cachedItem = new QuestionSelectionItem
                            {
                                CauHoi = ch,
                                Diem = 1.0,
                                ThuTu = stt++,
                                IsSelected = false
                            };
                            cachedItem.OnChanged = () =>
                            {
                                if (cachedItem.IsSelected)
                                {
                                    _selectedQuestionsMaster[cachedItem.CauHoi.MaCauHoi] = cachedItem;
                                }
                                else
                                {
                                    _selectedQuestionsMaster.Remove(cachedItem.CauHoi.MaCauHoi);
                                }
                                ThucHienCapNhatThongKeCauHoi();
                            };
                            _allQuestionItemsCache[ch.MaCauHoi] = cachedItem;
                        }
                        else
                        {
                            cachedItem.IsSelected = false;
                        }
                        itemsToAdd.Add(cachedItem);
                    }
                }

                void UpdateList()
                {
                    lock (_collectionLock)
                    {
                        NgânHangCauHoiChon.Clear();
                        foreach (var it in itemsToAdd)
                        {
                            NgânHangCauHoiChon.Add(it);
                        }
                    }
                    OnPropertyChanged(nameof(NgânHangCauHoiChon));
                    ThucHienCapNhatThongKeCauHoi();
                }

                var dispatcher = System.Windows.Application.Current?.Dispatcher;
                if (dispatcher != null && !dispatcher.CheckAccess())
                {
                    dispatcher.Invoke(UpdateList);
                }
                else
                {
                    UpdateList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NapCauHoiTheoBoLoc] {ex.Message}");
            }
            finally
            {
                IsLoadingQuestions = false;
            }
        }

        private void SetAllSelection(bool select)
        {
            foreach (var item in NgânHangCauHoiChon)
            {
                item.IsSelected = select;
                if (select)
                {
                    _selectedQuestionsMaster[item.CauHoi.MaCauHoi] = item;
                }
                else
                {
                    _selectedQuestionsMaster.Remove(item.CauHoi.MaCauHoi);
                }
            }
            if (!select && IsShowOnlySelected)
            {
                _selectedQuestionsMaster.Clear();
                NgânHangCauHoiChon.Clear();
            }
            ThucHienCapNhatThongKeCauHoi();
        }

        private void ThucHienToggleChonCauHoi(QuestionSelectionItem? item)
        {
            if (item == null) return;
            item.IsSelected = !item.IsSelected;
            if (item.IsSelected)
            {
                item.IsAnswersExpanded = true;
                _selectedQuestionsMaster[item.CauHoi.MaCauHoi] = item;
            }
            else
            {
                _selectedQuestionsMaster.Remove(item.CauHoi.MaCauHoi);
                if (IsShowOnlySelected)
                {
                    NgânHangCauHoiChon.Remove(item);
                }
            }
            ThucHienCapNhatThongKeCauHoi();
        }

        private void ThucHienToggleXemDapAn(QuestionSelectionItem? item)
        {
            if (item != null)
            {
                item.IsAnswersExpanded = !item.IsAnswersExpanded;
            }
        }

        private void ThucHienCapNhatThongKeCauHoi()
        {
            OnPropertyChanged(nameof(TongSoCauDaChon));
            OnPropertyChanged(nameof(TongDiemDeThi));
            if (CurrentDeThi != null)
            {
                CurrentDeThi.TongDiem = TongDiemDeThi;
                CurrentDeThi.SoLuongCauHoi = TongSoCauDaChon;

                // Tự động tính điểm đạt đề xuất = 50% tổng điểm (làm tròn 1 chữ số thập phân)
                if (TongDiemDeThi > 0)
                {
                    CurrentDeThi.DiemDat = Math.Round(TongDiemDeThi * 0.5, 1);
                }
                else
                {
                    CurrentDeThi.DiemDat = 0;
                }
                CurrentDeThi.NotifyPropertyChanged(nameof(CurrentDeThi.DiemDat));
                CurrentDeThi.NotifyPropertyChanged(nameof(CurrentDeThi.TongDiem));
                OnPropertyChanged(nameof(CurrentDeThi));
            }
            CapNhatTrangThaiCauTruc();
        }

        public void CapNhatTrangThaiCauTruc()
        {
            OnPropertyChanged(nameof(HasSelectedBlueprint));
            OnPropertyChanged(nameof(SoCauYeuCauCauTruc));
            OnPropertyChanged(nameof(IsKhopCauTruc));
            OnPropertyChanged(nameof(SoCauConThieu));
            OnPropertyChanged(nameof(SoCauDu));
            OnPropertyChanged(nameof(ThongBaoTrangThaiCauTruc));
            OnPropertyChanged(nameof(IconTrangThaiCauTruc));
            OnPropertyChanged(nameof(MauNenTrangThaiCauTruc));
            OnPropertyChanged(nameof(MauVienTrangThaiCauTruc));
            OnPropertyChanged(nameof(MauChuTrangThaiCauTruc));
            OnPropertyChanged(nameof(ChuoiTienDoCauChon));
        }

        // =========================================================================
        // ACTIONS
        // =========================================================================
        private void KhoiTaoDeThiMoi()
        {
            _selectedQuestionsMaster.Clear();
            foreach (var item in _allQuestionItemsCache.Values)
            {
                item.IsSelected = false;
            }
            _isShowOnlySelected = false;
            OnPropertyChanged(nameof(IsShowOnlySelected));

            CurrentDeThi = new BaiTapDeThiDTO
            {
                TenBaiTapDeThi = "Đề thi thử mới",
                LoaiHinh = "ThiThu",
                ThoiGianLamBaiPhut = 45,
                DiemDat = 5.0,
                TongDiem = 10.0,
                XaoTronCauHoi = true,
                ChoXemDapAn = true,
                ChoXemLoiGiai = true,
                SoLuotLamToiDa = 0,
                DaPhatHanh = false,
                TrangThai = "BanNhap",
                MaCapDo = "B2"
            };

            var tdDefault = DanhSachTrinhDo.FirstOrDefault(x => string.Equals(x.MaCapDo, "B2", StringComparison.OrdinalIgnoreCase));
            if (tdDefault != null)
            {
                CurrentDeThi.MaTrinhDo = tdDefault.MaTrinhDo;
                CurrentDeThi.TenTrinhDo = tdDefault.TenTrinhDo;
            }

            SelectedCauTrucDeThi = DanhSachCauTrucDeThi.FirstOrDefault(x => x.MaCauTruc == 0);
            ThongBaoLoi = string.Empty;
        }

        private void NapDanhSachCauTrucDeThi()
        {
            DanhSachCauTrucDeThi.Clear();
            DanhSachCauTrucDeThi.Add(new CauTrucDeThiDTO
            {
                MaCauTruc = 0,
                TenCauTruc = "-- Đề thi tự do (Không theo cấu trúc chuẩn) --",
                MaKyThi = "FREE",
                MaCapDo = "MULTI",
                ThoiGianPhut = 30,
                TongSoCau = 10,
                TongDiem = 10.0
            });

            try
            {
                var list = CauTrucDeThiBUS.Instance.LayDanhSachCauTruc();
                foreach (var bp in list)
                {
                    DanhSachCauTrucDeThi.Add(bp);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NapDanhSachCauTrucDeThi] {ex.Message}");
            }
        }

        private void OnCauTrucDeThiChanged()
        {
            if (_selectedCauTrucDeThi == null || _selectedCauTrucDeThi.MaCauTruc <= 0)
            {
                CurrentDeThi.MaCauTrucDeThi = null;
                CurrentDeThi.TenCauTrucDeThi = null;
                DanhSachPhanThiHienTai.Clear();
                OnPropertyChanged(nameof(CoCacPhanThi));
                CapNhatTrangThaiCauTruc();
                return;
            }

            CurrentDeThi.MaCauTrucDeThi = _selectedCauTrucDeThi.MaCauTruc;
            CurrentDeThi.TenCauTrucDeThi = _selectedCauTrucDeThi.TenCauTruc;

            DanhSachPhanThiHienTai.Clear();
            if (_selectedCauTrucDeThi.DanhSachPhanThi != null)
            {
                foreach (var p in _selectedCauTrucDeThi.DanhSachPhanThi)
                {
                    DanhSachPhanThiHienTai.Add(p);
                }
            }
            OnPropertyChanged(nameof(CoCacPhanThi));
            CapNhatTrangThaiCauTruc();

            // Tự động điền các thông số nếu tạo mới hoặc tên trống
            if (string.IsNullOrWhiteSpace(CurrentDeThi.TenBaiTapDeThi) || CurrentDeThi.MaBaiTapDeThi <= 0 || CurrentDeThi.TenBaiTapDeThi == "Đề thi thử mới")
            {
                ThucHienApDungThongSoCauTruc();
            }
        }

        public void ThucHienApDungThongSoCauTruc()
        {
            if (_selectedCauTrucDeThi == null || _selectedCauTrucDeThi.MaCauTruc <= 0) return;

            CurrentDeThi.TenBaiTapDeThi = _selectedCauTrucDeThi.TenCauTruc;
            CurrentDeThi.ThoiGianLamBaiPhut = _selectedCauTrucDeThi.ThoiGianPhut;
            CurrentDeThi.SoLuongCauHoi = _selectedCauTrucDeThi.TongSoCau;
            CurrentDeThi.TongDiem = _selectedCauTrucDeThi.TongDiem;
            CurrentDeThi.DiemDat = Math.Round(_selectedCauTrucDeThi.TongDiem * 0.5, 1);
            CurrentDeThi.LoaiHinh = _selectedCauTrucDeThi.LoaiDeThi ?? "ThiThu";
            CurrentDeThi.MoTa = _selectedCauTrucDeThi.MoTa ?? "";

            if (!string.IsNullOrWhiteSpace(_selectedCauTrucDeThi.MaCapDo) && _selectedCauTrucDeThi.MaCapDo != "MULTI")
            {
                CurrentDeThi.MaCapDo = _selectedCauTrucDeThi.MaCapDo;
                var td = DanhSachTrinhDo.FirstOrDefault(x => string.Equals(x.MaCapDo, _selectedCauTrucDeThi.MaCapDo, StringComparison.OrdinalIgnoreCase));
                if (td != null)
                {
                    CurrentDeThi.MaTrinhDo = td.MaTrinhDo;
                    CurrentDeThi.TenTrinhDo = td.TenTrinhDo;
                }
            }

            OnPropertyChanged(nameof(CurrentDeThi));
            CapNhatTrangThaiCauTruc();
        }

        public async void ThucHienTuDongChonCauHoiTheoCauTruc()
        {
            if (_selectedCauTrucDeThi == null || _selectedCauTrucDeThi.MaCauTruc <= 0)
            {
                GUI.Views.Common.ThongBaoDialog.CanhBao("Vui lòng chọn một Cấu trúc đề thi chuẩn hóa (Exam Blueprint) trước khi sử dụng tính năng tạo đề nhanh!", "Chưa chọn cấu trúc");
                return;
            }

            IsLoadingQuestions = true;
            try
            {
                // Nạp đầy đủ ngân hàng câu hỏi vào cache nếu chưa có
                if (_allQuestionItemsCache.Count == 0)
                {
                    var allDb = await System.Threading.Tasks.Task.Run(() => DAL.BoCauHoiDAL.Instance.LayTatCaCauHoi(null, null, null, 1000, daKiemDuyet: true));
                    foreach (var ch in allDb)
                    {
                        if (!_allQuestionItemsCache.ContainsKey(ch.MaCauHoi))
                        {
                            var item = new QuestionSelectionItem
                            {
                                CauHoi = ch,
                                IsSelected = false,
                                Diem = 0.25,
                                ThuTu = 1,
                                OnChanged = ThucHienCapNhatThongKeCauHoi
                            };
                            _allQuestionItemsCache[ch.MaCauHoi] = item;
                        }
                    }
                }

                // Xóa chọn cũ
                _selectedQuestionsMaster.Clear();
                foreach (var item in _allQuestionItemsCache.Values)
                {
                    item.IsSelected = false;
                }

                int currentThuTu = 1;
                var pickedIds = new HashSet<int>();
                int targetTotal = _selectedCauTrucDeThi.TongSoCau;
                double targetTongDiem = _selectedCauTrucDeThi.TongDiem > 0 ? _selectedCauTrucDeThi.TongDiem : 10.0;

                // 1. Nếu cấu trúc có chia theo các phần thi (Sections/Passages)
                if (_selectedCauTrucDeThi.DanhSachPhanThi != null && _selectedCauTrucDeThi.DanhSachPhanThi.Count > 0)
                {
                    foreach (var phan in _selectedCauTrucDeThi.DanhSachPhanThi.OrderBy(p => p.ThuTuPhan))
                    {
                        int remainingSlots = targetTotal - _selectedQuestionsMaster.Count;
                        if (remainingSlots <= 0) break;

                        int canChon = Math.Min(phan.SoLuongCau, remainingSlots);
                        if (canChon <= 0) continue;

                        string kyNang = phan.KyNang ?? "Doc";
                        string loaiChoPhep = phan.LoaiCauHoiChoPhep ?? "";
                        var loaiList = loaiChoPhep.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        var allowedSkills = kyNang.Split(new[] { ',', ';', '/' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        // CHỌN CHUẨN XÁC THEO KỸ NĂNG CỦA PHẦN THI (KHÔNG TRỘN LẪN TỪ VỰNG VÀO NGỮ PHÁP / ĐỌC HIỂU)
                        var candidates = _allQuestionItemsCache.Values
                            .Where(x => !pickedIds.Contains(x.CauHoi.MaCauHoi))
                            .Where(x => allowedSkills.Any(s => string.Equals(x.CauHoi.KyNang, s, StringComparison.OrdinalIgnoreCase)))
                            .OrderBy(x =>
                            {
                                bool matchLevel = string.Equals(x.CauHoi.MaTrinhDo, _selectedCauTrucDeThi.MaCapDo, StringComparison.OrdinalIgnoreCase);
                                bool matchPassage = phan.YeuCauDoanVan ? !string.IsNullOrWhiteSpace(x.CauHoi.DoanVanDoc) : true;
                                bool matchLoai = loaiList.Length == 0 || loaiList.Any(l => string.Equals(l, x.CauHoi.LoaiCauHoi, StringComparison.OrdinalIgnoreCase));

                                int score = 0;
                                if (matchLevel) score += 4;
                                if (matchLoai) score += 2;
                                if (matchPassage) score += 1;
                                return -score;
                            })
                            .Take(canChon)
                            .ToList();

                        // Nếu ngân hàng chưa đủ câu đúng level, tiếp tục lấy câu cùng KỸ NĂNG ở level gần nhất (tuyệt đối không lấy kỹ năng khác)
                        if (candidates.Count < canChon)
                        {
                            int stillNeed = canChon - candidates.Count;
                            var moreSameSkill = _allQuestionItemsCache.Values
                                .Where(x => !pickedIds.Contains(x.CauHoi.MaCauHoi) && !candidates.Contains(x))
                                .Where(x => allowedSkills.Any(s => string.Equals(x.CauHoi.KyNang, s, StringComparison.OrdinalIgnoreCase)))
                                .Take(stillNeed)
                                .ToList();
                            candidates.AddRange(moreSameSkill);
                        }

                        foreach (var item in candidates)
                        {
                            item.IsSelected = true;
                            item.ThuTu = currentThuTu++;
                            item.Diem = phan.DiemMoiCau > 0 ? phan.DiemMoiCau : Math.Round(targetTongDiem / targetTotal, 2);
                            pickedIds.Add(item.CauHoi.MaCauHoi);
                            _selectedQuestionsMaster[item.CauHoi.MaCauHoi] = item;
                        }
                    }
                }

                // 2. Nếu còn thiếu để đạt đủ TongSoCau của cấu trúc đề thi, bổ sung từ ngân hàng
                if (_selectedQuestionsMaster.Count < targetTotal)
                {
                    int thieu = targetTotal - _selectedQuestionsMaster.Count;
                    var boSung = _allQuestionItemsCache.Values
                        .Where(x => !pickedIds.Contains(x.CauHoi.MaCauHoi))
                        .OrderBy(x => string.Equals(x.CauHoi.MaTrinhDo, _selectedCauTrucDeThi.MaCapDo, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                        .Take(thieu)
                        .ToList();

                    foreach (var item in boSung)
                    {
                        item.IsSelected = true;
                        item.ThuTu = currentThuTu++;
                        item.Diem = Math.Round(targetTongDiem / targetTotal, 2);
                        pickedIds.Add(item.CauHoi.MaCauHoi);
                        _selectedQuestionsMaster[item.CauHoi.MaCauHoi] = item;
                    }
                }

                // 3. Nếu vượt quá số lượng câu yêu cầu (dư câu), cắt bớt chuẩn xác về targetTotal
                if (_selectedQuestionsMaster.Count > targetTotal)
                {
                    var excessKeys = _selectedQuestionsMaster.Keys.Skip(targetTotal).ToList();
                    foreach (var key in excessKeys)
                    {
                        if (_selectedQuestionsMaster.TryGetValue(key, out var item))
                        {
                            item.IsSelected = false;
                            _selectedQuestionsMaster.Remove(key);
                        }
                    }
                }

                // 4. Đánh lại số thứ tự liên tục 1..targetTotal
                int sttReset = 1;
                foreach (var item in _selectedQuestionsMaster.Values)
                {
                    item.ThuTu = sttReset++;
                }

                // 5. Cân bằng lại điểm từng câu để tổng điểm bằng đúng TongDiem của cấu trúc
                if (_selectedQuestionsMaster.Count > 0)
                {
                    double diemMoiCau = Math.Round(targetTongDiem / _selectedQuestionsMaster.Count, 3);
                    foreach (var item in _selectedQuestionsMaster.Values)
                    {
                        item.Diem = diemMoiCau;
                    }
                }

                ThucHienApDungThongSoCauTruc();
                ThucHienCapNhatThongKeCauHoi();
                CapNhatTrangThaiCauTruc();

                // Chuyển sang hiển thị các câu đã chọn để xem lại ngay
                IsShowOnlySelected = true;
                NapCauHoiTheoBoLoc();

                GUI.Views.Common.ThongBaoDialog.ThanhCong(
                    $"⚡ Đã tự động chọn đủ {_selectedQuestionsMaster.Count}/{targetTotal} câu hỏi khớp 100% với cấu trúc '{_selectedCauTrucDeThi.TenCauTruc}'!\n\nBạn có thể kiểm tra danh sách câu hỏi đã chọn bên phải và bấm 'Lưu & Xuất bản ngay'.",
                    "Tạo đề nhanh thành công");
            }
            catch (Exception ex)
            {
                GUI.Views.Common.ThongBaoDialog.Loi($"Có lỗi khi tự động chọn câu hỏi: {ex.Message}", "Lỗi");
            }
            finally
            {
                IsLoadingQuestions = false;
            }
        }

        private void ThucHienThemDeThiMoi()
        {
            KhoiTaoDeThiMoi();
            IsDanhSachMode = false;
            NapCauHoiTheoBoLoc();
        }

        private void ThucHienChonSuaDeThi(BaiTapDeThiDTO? deThi)
        {
            if (deThi == null) return;
            CurrentDeThi = deThi;
            SelectedCauTrucDeThi = DanhSachCauTrucDeThi.FirstOrDefault(x => x.MaCauTruc == deThi.MaCauTrucDeThi)
                ?? DanhSachCauTrucDeThi.FirstOrDefault(x => x.MaCauTruc == 0);
            ThongBaoLoi = string.Empty;
            IsDanhSachMode = false;
            _isShowOnlySelected = false;
            OnPropertyChanged(nameof(IsShowOnlySelected));
            _selectedQuestionsMaster.Clear();

            // Nạp danh sách câu hỏi của đề này
            var dsCauHoiCuaDe = BaiTapDeThiBUS.Instance.LayDanhSachCauHoiCuaDeThi(deThi.MaBaiTapDeThi);
            int stt = 1;
            foreach (var q in dsCauHoiCuaDe)
            {
                var item = new QuestionSelectionItem
                {
                    CauHoi = q,
                    Diem = q.Diem > 0 ? q.Diem : 1.0,
                    ThuTu = q.ThuTuTrongBo > 0 ? q.ThuTuTrongBo : stt++,
                    IsSelected = true
                };
                item.OnChanged = () =>
                {
                    if (item.IsSelected)
                        _selectedQuestionsMaster[item.CauHoi.MaCauHoi] = item;
                    else
                        _selectedQuestionsMaster.Remove(item.CauHoi.MaCauHoi);
                    ThucHienCapNhatThongKeCauHoi();
                };
                _selectedQuestionsMaster[q.MaCauHoi] = item;
                _allQuestionItemsCache[q.MaCauHoi] = item;
            }

            NapCauHoiTheoBoLoc();
        }

        private void ThucHienXemTruocDeThi(BaiTapDeThiDTO? deThi)
        {
            if (deThi == null) return;
            SelectedExam = deThi;
            var ds = BaiTapDeThiBUS.Instance.LayDanhSachCauHoiCuaDeThi(deThi.MaBaiTapDeThi);
            PreviewQuestions.Clear();
            foreach (var q in ds) PreviewQuestions.Add(q);
            if (YeuCauHienThiXemTruoc != null)
            {
                YeuCauHienThiXemTruoc.Invoke(deThi, ds);
            }
            else
            {
                IsPreviewModalOpen = true;
            }
        }

        private void ThucHienXuatBanDeThi(BaiTapDeThiDTO? deThi)
        {
            if (deThi == null) return;
            if (BaiTapDeThiBUS.Instance.XuatBanDeThi(deThi.MaBaiTapDeThi, out string err))
            {
                deThi.DaPhatHanh = true;
                deThi.TrangThai = "DaXuatBan";
                TaiDuLieuDeThi();
                GUI.Views.Common.ThongBaoDialog.ThanhCong($"Đã xuất bản đề thi '{deThi.TenBaiTapDeThi}' cho học viên làm bài!", "Xuất bản thành công");
            }
            else
            {
                GUI.Views.Common.ThongBaoDialog.Loi($"Lỗi xuất bản: {err}", "Thông báo");
            }
        }

        private void ThucHienDongDeThi(BaiTapDeThiDTO? deThi)
        {
            if (deThi == null) return;
            if (BaiTapDeThiBUS.Instance.DongDeThi(deThi.MaBaiTapDeThi, out string err))
            {
                deThi.DaPhatHanh = false;
                deThi.TrangThai = "DaDong";
                TaiDuLieuDeThi();
                GUI.Views.Common.ThongBaoDialog.ThongTin($"Đã đóng đề thi '{deThi.TenBaiTapDeThi}'. Học viên sẽ không thể vào làm đề này.", "Đã đóng đề thi");
            }
            else
            {
                GUI.Views.Common.ThongBaoDialog.Loi($"Lỗi đóng đề thi: {err}", "Thông báo");
            }
        }

        private void ThucHienNhanBanDeThi(BaiTapDeThiDTO? deThi)
        {
            if (deThi == null) return;
            int newId = BaiTapDeThiBUS.Instance.NhanBanDeThi(deThi.MaBaiTapDeThi, out string err);
            if (newId > 0)
            {
                TaiDuLieuDeThi();
                GUI.Views.Common.ThongBaoDialog.ThanhCong($"Đã nhân bản đề thi thành công! Đề mới được tạo ở trạng thái Bản nháp (Mã #{newId}).", "Nhân bản thành công");
            }
            else
            {
                GUI.Views.Common.ThongBaoDialog.Loi($"Lỗi nhân bản: {err}", "Thông báo");
            }
        }

        private void ThucHienXoaDeThi(BaiTapDeThiDTO? deThi)
        {
            if (deThi == null) return;

            bool xacNhan = GUI.Views.Common.ThongBaoDialog.XacNhan(
                $"Bạn có chắc chắn muốn xóa đề thi '{deThi.TenBaiTapDeThi}'?\n\nLưu ý: Nếu đề thi đã có lượt làm bài của học viên, hệ thống sẽ tự động chuyển sang trạng thái 'Đã đóng' để bảo toàn lịch sử thi.",
                "Xác nhận xóa đề thi");

            if (!xacNhan) return;

            if (BaiTapDeThiBUS.Instance.XoaDeThi(deThi.MaBaiTapDeThi, out string err))
            {
                TaiDuLieuDeThi();
                GUI.Views.Common.ThongBaoDialog.ThanhCong("Đã xóa đề thi an toàn khỏi hệ thống!", "Thành công");
            }
            else
            {
                TaiDuLieuDeThi();
                GUI.Views.Common.ThongBaoDialog.CanhBao(err, "Thông báo");
            }
        }

        private void ThucHienXemLichSuDeThi(BaiTapDeThiDTO? deThi)
        {
            if (deThi == null) return;
            SelectedExam = deThi;
            var history = BaiTapDeThiBUS.Instance.LayLichSuLamBaiCuaDeThi(deThi.MaBaiTapDeThi);
            ExamAttemptHistory.Clear();
            foreach (var h in history) ExamAttemptHistory.Add(h);
            if (YeuCauHienThiLichSuBaiNop != null)
            {
                YeuCauHienThiLichSuBaiNop.Invoke(deThi, history);
            }
            else
            {
                IsHistoryModalOpen = true;
            }
        }

        private void ThucHienQuayLaiDanhSach()
        {
            StopAudio();
            IsDanhSachMode = true;
            TaiDuLieuDeThi();
        }

        private void ThucHienChonAudioDeThi()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Chọn tệp âm thanh nghe cho đề thi",
                Filter = "Tệp âm thanh (*.mp3;*.wav;*.m4a;*.aac)|*.mp3;*.wav;*.m4a;*.aac|Tất cả các tệp (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                CurrentDeThi.DuongDanAudio = dlg.FileName;
            }
        }

        private void ThucHienXoaAudioDeThi()
        {
            StopAudio();
            CurrentDeThi.DuongDanAudio = null;
        }

        private void ThucHienTogglePlayAudioDeThi()
        {
            if (string.IsNullOrWhiteSpace(CurrentDeThi.DuongDanAudio)) return;

            if (IsPlayingAudio)
            {
                StopAudio();
            }
            else
            {
                try
                {
                    _mediaPlayer ??= new System.Windows.Media.MediaPlayer();
                    _mediaPlayer.MediaEnded += (s, e) => { IsPlayingAudio = false; };
                    _mediaPlayer.MediaFailed += (s, e) => { IsPlayingAudio = false; };
                    _mediaPlayer.Open(new Uri(CurrentDeThi.DuongDanAudio, UriKind.RelativeOrAbsolute));
                    _mediaPlayer.Play();
                    IsPlayingAudio = true;
                }
                catch
                {
                    IsPlayingAudio = false;
                }
            }
        }

        private void StopAudio()
        {
            try
            {
                _mediaPlayer?.Stop();
                _mediaPlayer?.Close();
            }
            catch { }
            IsPlayingAudio = false;
        }

        private void ThucHienDiChuyenCauLen(QuestionSelectionItem? item)
        {
            if (item == null) return;
            int idx = NgânHangCauHoiChon.IndexOf(item);
            if (idx > 0)
            {
                NgânHangCauHoiChon.Move(idx, idx - 1);
                for (int i = 0; i < NgânHangCauHoiChon.Count; i++)
                {
                    NgânHangCauHoiChon[i].ThuTu = i + 1;
                }
            }
        }

        private void DiChuyenCauXuong(QuestionSelectionItem? item)
        {
            if (item == null) return;
            int idx = NgânHangCauHoiChon.IndexOf(item);
            if (idx >= 0 && idx < NgânHangCauHoiChon.Count - 1)
            {
                NgânHangCauHoiChon.Move(idx, idx + 1);
                for (int i = 0; i < NgânHangCauHoiChon.Count; i++)
                {
                    NgânHangCauHoiChon[i].ThuTu = i + 1;
                }
            }
        }

        private void ThucHienDiChuyenCauXuong(QuestionSelectionItem? item)
        {
            DiChuyenCauXuong(item);
        }

        private void ThucHienLuuDeThi(bool xuatBan)
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

            // RÀNG BUỘC CHẶT CHẼ THEO CẤU TRÚC ĐỀ THI (BLUEPRINT MATRIX)
            if (SelectedCauTrucDeThi != null && SelectedCauTrucDeThi.MaCauTruc > 0)
            {
                int soCauYeuCau = SelectedCauTrucDeThi.TongSoCau;
                if (TongSoCauDaChon != soCauYeuCau)
                {
                    if (xuatBan)
                    {
                        string chenhLechMsg = TongSoCauDaChon < soCauYeuCau 
                            ? $"còn thiếu {soCauYeuCau - TongSoCauDaChon} câu" 
                            : $"đang dư {TongSoCauDaChon - soCauYeuCau} câu";

                        GUI.Views.Common.ThongBaoDialog.CanhBao(
                            $"Không thể xuất bản đề thi!\n\nCấu trúc chuẩn '{SelectedCauTrucDeThi.TenCauTruc}' yêu cầu chính xác {soCauYeuCau} câu hỏi, nhưng bạn đang chọn {TongSoCauDaChon} câu ({chenhLechMsg}).\n\n👉 Vui lòng nhấn nút '⚡ Tự động chọn câu' để hệ thống chọn chuẩn theo ma trận đề, hoặc lưu dưới dạng 'Bản nháp' để bổ sung sau.",
                            "Ràng buộc cấu trúc đề thi");
                        return;
                    }
                    else
                    {
                        // Lưu bản nháp: Cho phép nhưng nhắc nhở người dùng
                        bool tiepTuc = GUI.Views.Common.ThongBaoDialog.XacNhan(
                            $"Đề thi này gắn với cấu trúc chuẩn '{SelectedCauTrucDeThi.TenCauTruc}' (yêu cầu {soCauYeuCau} câu) nhưng hiện mới chọn {TongSoCauDaChon} câu.\n\nBạn có chắc chắn muốn lưu tạm dưới dạng BẢN NHÁP để tiếp tục hoàn thiện sau không?",
                            "Lưu bản nháp chưa đủ cấu trúc");
                        if (!tiepTuc) return;
                    }
                }
            }

            ThongBaoLoi = string.Empty;

            CurrentDeThi.DaPhatHanh = xuatBan;
            CurrentDeThi.TrangThai = xuatBan ? "DaXuatBan" : "BanNhap";
            CurrentDeThi.TongDiem = TongDiemDeThi;
            CurrentDeThi.TongSoCau = TongSoCauDaChon;

            // Đồng bộ MaTrinhDo và TenTrinhDo từ MaCapDo được chọn
            if (!string.IsNullOrWhiteSpace(CurrentDeThi.MaCapDo))
            {
                var td = DanhSachTrinhDo.FirstOrDefault(x => string.Equals(x.MaCapDo, CurrentDeThi.MaCapDo, StringComparison.OrdinalIgnoreCase));
                if (td != null)
                {
                    CurrentDeThi.MaTrinhDo = td.MaTrinhDo;
                    CurrentDeThi.TenTrinhDo = td.TenTrinhDo;
                }
            }

            var dsChiTiet = _selectedQuestionsMaster.Values
                .OrderBy(x => x.ThuTu)
                .Select((item, idx) => (item.CauHoi.MaCauHoi, ThuTu: idx + 1, item.Diem))
                .ToList();

            bool thanhCong = BaiTapDeThiBUS.Instance.LuuDeThiVaCauHoi(
                CurrentDeThi, 
                dsChiTiet, 
                out string err);

            if (thanhCong)
            {
                string msg = xuatBan 
                    ? "Đã lưu và xuất bản đề thi thành công! Nội dung đề và đáp án đã được chụp Snapshot bảo toàn nguyên vẹn." 
                    : "Đã lưu đề thi dưới dạng Bản nháp thành công!";

                GUI.Views.Common.ThongBaoDialog.ThanhCong(msg, "Thành công");
                IsDanhSachMode = true;
                TaiDuLieuDeThi();
            }
            else
            {
                ThongBaoLoi = "Lỗi khi lưu đề thi: " + err;
            }
        }
    }
}
