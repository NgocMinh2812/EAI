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
        private int _selectedSourceId = 0; // 0 = Toàn bộ ngân hàng đề, >0 = Mã bộ cụ thể
        private string _filterSkill = "All";
        private string _filterQuestionLevel = "All";
        private string _filterDifficulty = "All";
        private string _searchQuestionKeyword = string.Empty;

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
        public int SelectedSourceId
        {
            get => _selectedSourceId;
            set
            {
                if (SetProperty(ref _selectedSourceId, value))
                    NapCauHoiTheoBoLoc();
            }
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

        // Statistics
        public int TongSoDeThi => DanhSachDeThiHienCo.Count;
        public int SoDeDaXuatBan => DanhSachDeThiHienCo.Count(d => d.IsPublished);
        public int SoDeBanNhap => DanhSachDeThiHienCo.Count(d => d.IsDraft);
        public int TongLuotHocVienThi => DanhSachDeThiHienCo.Sum(d => d.SoLuotDaThi);

        public int TongSoCauDaChon => NgânHangCauHoiChon.Count(x => x.IsSelected);
        public double TongDiemDeThi => Math.Round(NgânHangCauHoiChon.Where(x => x.IsSelected).Sum(x => x.Diem), 2);

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

            KhoiTaoDeThiMoi();
            TaiDuLieuDeThi();
            NapCauHoiTheoBoLoc();
        }

        // =========================================================================
        // DATA LOADING & FILTERING
        // =========================================================================
        public void TaiDuLieuDeThi()
        {
            var ds = BaiTapDeThiBUS.Instance.LayDanhSachDeThi();
            DanhSachDeThiHienCo.Clear();
            foreach (var d in ds) DanhSachDeThiHienCo.Add(d);

            var dmBUS = new DanhMucBUS();
            DanhSachTrinhDo.Clear();
            foreach (var td in dmBUS.LayDanhSachTrinhDo()) DanhSachTrinhDo.Add(td);

            var dsBo = BoCauHoiDAL.Instance.LayDanhSachBoCauHoi();
            DanhSachBoCauHoi.Clear();
            DanhSachNguonCauHoi.Clear();
            DanhSachNguonCauHoi.Add(new QuestionSourceOption { Id = 0, DisplayText = "Toàn bộ ngân hàng đề" });

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

        public void NapCauHoiTheoBoLoc()
        {
            // Ghi nhớ các câu hỏi đã chọn trước đó
            var selectedMap = NgânHangCauHoiChon
                .Where(x => x.IsSelected)
                .ToDictionary(x => x.CauHoi.MaCauHoi, x => (x.Diem, x.ThuTu));

            List<CauHoiKhaoSatDTO> dsCauHoi;

            if (SelectedSourceId > 0)
            {
                dsCauHoi = BoCauHoiDAL.Instance.LayDanhSachCauHoiTheoBo(
                    SelectedSourceId, 
                    SearchQuestionKeyword, 
                    FilterSkill, 
                    FilterDifficulty);
            }
            else
            {
                dsCauHoi = BoCauHoiDAL.Instance.LayTatCaCauHoi(
                    SearchQuestionKeyword, 
                    FilterSkill, 
                    FilterDifficulty, 
                    100);
            }

            if (!string.IsNullOrWhiteSpace(FilterQuestionLevel) && FilterQuestionLevel != "All")
            {
                dsCauHoi = dsCauHoi
                    .Where(q => string.Equals(q.MaTrinhDo, FilterQuestionLevel, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            NgânHangCauHoiChon.Clear();
            int stt = 1;

            // Đưa các câu đã chọn lên trước nếu có
            foreach (var ch in dsCauHoi)
            {
                bool isSelected = selectedMap.ContainsKey(ch.MaCauHoi);
                double diem = isSelected ? selectedMap[ch.MaCauHoi].Diem : 1.0;
                int thuTu = isSelected ? selectedMap[ch.MaCauHoi].ThuTu : stt++;

                NgânHangCauHoiChon.Add(new QuestionSelectionItem
                {
                    CauHoi = ch,
                    Diem = diem,
                    ThuTu = thuTu,
                    IsSelected = isSelected,
                    OnChanged = ThucHienCapNhatThongKeCauHoi
                });
            }

            ThucHienCapNhatThongKeCauHoi();
        }

        private void SetAllSelection(bool select)
        {
            foreach (var item in NgânHangCauHoiChon)
            {
                item.IsSelected = select;
            }
            ThucHienCapNhatThongKeCauHoi();
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
        }

        // =========================================================================
        // ACTIONS
        // =========================================================================
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
                ChoXemDapAn = true,
                ChoXemLoiGiai = true,
                SoLuotLamToiDa = 0,
                DaPhatHanh = false,
                TrangThai = "BanNhap",
                MaCapDo = "B2"
            };
            ThongBaoLoi = string.Empty;
            NapCauHoiTheoBoLoc();
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
            ThongBaoLoi = string.Empty;
            IsDanhSachMode = false;

            // Nạp danh sách câu hỏi của đề này
            var dsCauHoiCuaDe = BaiTapDeThiBUS.Instance.LayDanhSachCauHoiCuaDeThi(deThi.MaBaiTapDeThi);
            var selectedMap = dsCauHoiCuaDe.ToDictionary(q => q.MaCauHoi, q => q.Diem);

            // Nạp ngân hàng câu hỏi
            NapCauHoiTheoBoLoc();

            // Đánh dấu các câu đã có trong đề
            foreach (var item in NgânHangCauHoiChon)
            {
                if (selectedMap.TryGetValue(item.CauHoi.MaCauHoi, out double diem))
                {
                    item.IsSelected = true;
                    item.Diem = diem > 0 ? diem : 1.0;
                }
            }

            ThucHienCapNhatThongKeCauHoi();
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
            IsDanhSachMode = true;
            TaiDuLieuDeThi();
        }

        private void ThucHienDiChuyenCauLen(QuestionSelectionItem? item)
        {
            if (item == null) return;
            int idx = NgânHangCauHoiChon.IndexOf(item);
            if (idx > 0)
            {
                NgânHangCauHoiChon.Move(idx, idx - 1);
            }
        }

        private void DiChuyenCauXuong(QuestionSelectionItem? item)
        {
            if (item == null) return;
            int idx = NgânHangCauHoiChon.IndexOf(item);
            if (idx >= 0 && idx < NgânHangCauHoiChon.Count - 1)
            {
                NgânHangCauHoiChon.Move(idx, idx + 1);
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

            ThongBaoLoi = string.Empty;

            CurrentDeThi.DaPhatHanh = xuatBan;
            CurrentDeThi.TrangThai = xuatBan ? "DaXuatBan" : "BanNhap";
            CurrentDeThi.TongDiem = TongDiemDeThi;
            CurrentDeThi.TongSoCau = TongSoCauDaChon;

            var dsChiTiet = NgânHangCauHoiChon
                .Where(x => x.IsSelected)
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
