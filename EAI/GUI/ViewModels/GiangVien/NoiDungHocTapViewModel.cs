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

        private bool _isAiGeneratingSample;
        private bool _isAiGeneratingQuestions;
        private bool _isQuestionsModalOpen;
        private string _aiQuestionsStatusMessage = string.Empty;

        public bool IsAiGeneratingSample
        {
            get => _isAiGeneratingSample;
            set => SetProperty(ref _isAiGeneratingSample, value);
        }

        public bool IsAiGeneratingQuestions
        {
            get => _isAiGeneratingQuestions;
            set => SetProperty(ref _isAiGeneratingQuestions, value);
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
        public ICommand ChonAnhMinhHoaCommand { get; }
        public ICommand LuuBaiHocCommand { get; }
        public ICommand ToggleXuatBanCommand { get; }
        public ICommand ThemBaiHocMoiCommand { get; }
        public ICommand LamMoiCommand { get; }
        public ICommand SinhNoiDungMauAICommand { get; }
        public ICommand SinhCauHoiAICommand { get; }
        public ICommand DongModalCauHoiCommand { get; }
        public ICommand ChenCauHoiVaoBaiHocCommand { get; }
        public ICommand ThemCauHoiTracNghiemCommand { get; }
        public ICommand ThemCauHoiTuLuanCommand { get; }
        public ICommand ThemCauHoiDienTuCommand { get; }

        public NoiDungHocTapViewModel()
        {
            ChonAudioCommand = new RelayCommand(ThucHienChonAudio);
            ChonAnhMinhHoaCommand = new RelayCommand(ThucHienChonAnh);
            LuuBaiHocCommand = new RelayCommand(ThucHienLuuBaiHoc);
            ToggleXuatBanCommand = new RelayCommand(ThucHienToggleXuatBan);
            ThemBaiHocMoiCommand = new RelayCommand(ThucHienThemBaiHocMoi);
            LamMoiCommand = new RelayCommand(NapDuLieu);

            SinhNoiDungMauAICommand = new RelayCommand(ThucHienSinhNoiDungMauAI);
            SinhCauHoiAICommand = new RelayCommand(ThucHienSinhCauHoiAI);
            DongModalCauHoiCommand = new RelayCommand(() => IsQuestionsModalOpen = false);
            ChenCauHoiVaoBaiHocCommand = new RelayCommand(ThucHienChenCauHoiVaoBaiHoc);
            ThemCauHoiTracNghiemCommand = new RelayCommand(ThucHienThemCauHoiTracNghiem);
            ThemCauHoiTuLuanCommand = new RelayCommand(ThucHienThemCauHoiTuLuan);
            ThemCauHoiDienTuCommand = new RelayCommand(ThucHienThemCauHoiDienTu);

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
                TieuDe = string.Empty,
                LoaiBaiHoc = "NguPhap",
                MaTrinhDo = "B1",
                ThoiGianUocTinhPhut = 20,
                DaXuatBan = false,
                NoiDung = string.Empty
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

        private async void ThucHienSinhNoiDungMauAI()
        {
            if (SelectedBaiHoc == null) return;
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe))
            {
                ThongBaoLoi = "Vui lòng nhập tiêu đề bài học trước khi tạo nội dung mẫu bằng AI.";
                return;
            }

            IsAiGeneratingSample = true;
            ThongBaoLoi = string.Empty;
            try
            {
                string tieuDe = SelectedBaiHoc.TieuDe.Trim();
                string trinhDo = string.IsNullOrWhiteSpace(SelectedBaiHoc.MaTrinhDo) ? "B1" : SelectedBaiHoc.MaTrinhDo;
                string loai = string.IsNullOrWhiteSpace(SelectedBaiHoc.LoaiBaiHoc) ? "Doc" : SelectedBaiHoc.LoaiBaiHoc;

                string content = await GeminiAIService.SinhNoiDungMauAIAsync(tieuDe, trinhDo, loai, "NoiDungHocTap");
                SelectedBaiHoc.NoiDung = content;
                OnPropertyChanged(nameof(SelectedBaiHoc));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SinhNoiDungMauAI] {ex.Message}");
                ThongBaoLoi = "Không thể sinh nội dung: " + ex.Message;
            }
            finally
            {
                IsAiGeneratingSample = false;
            }
        }

        private async void ThucHienSinhCauHoiAI()
        {
            if (SelectedBaiHoc == null) return;
            if (string.IsNullOrWhiteSpace(SelectedBaiHoc.TieuDe) && string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung))
            {
                ThongBaoLoi = "Vui lòng nhập tiêu đề hoặc nội dung bài học trước khi tạo bộ câu hỏi bằng AI.";
                return;
            }

            string content = SelectedBaiHoc.NoiDung;
            if (string.IsNullOrWhiteSpace(content))
            {
                content = SelectedBaiHoc.TieuDe;
            }

            IsAiGeneratingQuestions = true;
            AiQuestionsStatusMessage = "Gemini AI đang phân tích bài giảng và tạo bộ câu hỏi...";
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
                AiQuestionsStatusMessage = $"Gemini AI đã tạo thành công {AiGeneratedQuestions.Count} câu hỏi chuẩn hóa!";
            }
            catch (Exception ex)
            {
                AiQuestionsStatusMessage = "Lỗi khi tạo câu hỏi: " + ex.Message;
            }
            finally
            {
                IsAiGeneratingQuestions = false;
            }
        }

        private void ThucHienChenCauHoiVaoBaiHoc()
        {
            if (SelectedBaiHoc == null || AiGeneratedQuestions.Count == 0) return;

            var sb = new System.Text.StringBuilder();
            if (!string.IsNullOrWhiteSpace(SelectedBaiHoc.NoiDung))
            {
                sb.AppendLine(SelectedBaiHoc.NoiDung.TrimEnd());
                sb.AppendLine();
            }

            sb.AppendLine("■ BỘ CÂU HỎI TRẮC NGHIỆM ÔN TẬP (AI GENERATED):");
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
