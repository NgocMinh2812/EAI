using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO
{
    public class CauHoiDTO
    {
        public int MaCauHoi { get; set; }
        public string NoiDungCauHoi { get; set; } = string.Empty;
        public List<string> DanhSachLuaChon { get; set; } = new();
        public int? DapAnDungIndex { get; set; } // 0: A, 1: B, 2: C, 3: D (null nếu là câu hỏi điền từ/trả lời ngắn/tự luận)
        public string? GiaiThichChiTiet { get; set; } = string.Empty;
        public int LuaChonCuaNguoiDung { get; set; } = -1; // -1: Chưa chọn
        public bool? DaTraLoiDung { get; set; } = null;
        public bool IsSelected { get; set; } = true;
        public string? KyNang { get; set; }
        public string? DoKho { get; set; }
        public string? LoaiCauHoi { get; set; }
        public string? DoanVanDoc { get; set; }
        public string? DapAnVanBan { get; set; }
        public string? TieuChiCham { get; set; }
        public string? AudioScript { get; set; }

        /// <summary>
        /// Chuẩn hóa nội dung lựa chọn trắc nghiệm, loại bỏ tiền tố A., B., C., D., A), (A), [A], A - ...
        /// tránh tình trạng bị hiển thị lặp "A. A. option" hoặc "B. B. option".
        /// </summary>
        public static string StripOptionPrefix(string? rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText)) return string.Empty;
            string trimmed = rawText.Trim();
            string stripped = System.Text.RegularExpressions.Regex.Replace(
                trimmed,
                @"^(?:(?:\([A-Da-d]\)|\[[A-Da-d]\]|[A-Da-d][\.\)\:\-])\s*)+",
                ""
            ).Trim();
            return string.IsNullOrEmpty(stripped) ? trimmed : stripped;
        }
    }

    public class BaiHocDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private int _maBaiHoc;
        public int MaBaiHoc
        {
            get => _maBaiHoc;
            set => SetProperty(ref _maBaiHoc, value);
        }

        private int _maChuDe;
        public int MaChuDe
        {
            get => _maChuDe;
            set => SetProperty(ref _maChuDe, value);
        }

        private int _maTrinhDoId = 0;
        public int MaTrinhDoId
        {
            get => _maTrinhDoId;
            set => SetProperty(ref _maTrinhDoId, value);
        }

        private int? _nguoiTao;
        public int? NguoiTao
        {
            get => _nguoiTao;
            set => SetProperty(ref _nguoiTao, value);
        }

        private string _tieuDe = string.Empty;
        public string TieuDe
        {
            get => _tieuDe;
            set => SetProperty(ref _tieuDe, value);
        }

        private string _loaiBaiHoc = string.Empty; // TuVung, NguPhap, Doc, Nghe, Viet
        public string LoaiBaiHoc
        {
            get => _loaiBaiHoc;
            set
            {
                if (SetProperty(ref _loaiBaiHoc, value))
                {
                    OnPropertyChanged(nameof(IsKyNangNghe));
                }
            }
        }

        private string _tenLoaiHienThi = string.Empty;
        public string TenLoaiHienThi
        {
            get => _tenLoaiHienThi;
            set => SetProperty(ref _tenLoaiHienThi, value);
        }

        private string _maTrinhDo = string.Empty; // A1, A2, B1, B2, C1
        public string MaTrinhDo
        {
            get => _maTrinhDo;
            set => SetProperty(ref _maTrinhDo, value);
        }

        private string _tenChuDe = string.Empty;
        public string TenChuDe
        {
            get => _tenChuDe;
            set => SetProperty(ref _tenChuDe, value);
        }

        private string _tomTat = string.Empty;
        public string TomTat
        {
            get => _tomTat;
            set => SetProperty(ref _tomTat, value);
        }

        private string _noiDung = string.Empty;
        public string NoiDung
        {
            get => _noiDung;
            set => SetProperty(ref _noiDung, value);
        }

        private int _thoiGianUocTinhPhut = 15;
        public int ThoiGianUocTinhPhut
        {
            get => _thoiGianUocTinhPhut;
            set
            {
                if (SetProperty(ref _thoiGianUocTinhPhut, value))
                {
                    OnPropertyChanged(nameof(ThoiLuongUocTinh));
                }
            }
        }

        public int ThoiLuongUocTinh
        {
            get => ThoiGianUocTinhPhut;
            set => ThoiGianUocTinhPhut = value;
        }

        private bool _daXuatBan = false;
        public bool DaXuatBan
        {
            get => _daXuatBan;
            set
            {
                if (SetProperty(ref _daXuatBan, value))
                {
                    DongBoTrangThaiXuatBan();
                }
            }
        }

        public void DongBoTrangThaiXuatBan()
        {
            TrangThai = _daXuatBan ? "Đã xuất bản" : "Bản nháp";
            StatusBackground = _daXuatBan ? "#DCFCE7" : "#F1F5F9";
            StatusColor = _daXuatBan ? "#15803D" : "#64748B";
            OnPropertyChanged(nameof(TrangThai));
            OnPropertyChanged(nameof(StatusBackground));
            OnPropertyChanged(nameof(StatusColor));
        }

        private string? _duongDanAudio;
        public string? DuongDanAudio
        {
            get => _duongDanAudio;
            set
            {
                if (SetProperty(ref _duongDanAudio, value))
                {
                    OnPropertyChanged(nameof(AudioPath));
                    OnPropertyChanged(nameof(HasAudio));
                }
            }
        }

        private string? _duongDanHinhAnh;
        public string? DuongDanHinhAnh
        {
            get => _duongDanHinhAnh;
            set
            {
                if (SetProperty(ref _duongDanHinhAnh, value))
                {
                    OnPropertyChanged(nameof(AnhMinhHoaPath));
                }
            }
        }

        public string? AudioPath
        {
            get => DuongDanAudio;
            set => DuongDanAudio = value;
        }

        public string? AnhMinhHoaPath
        {
            get => DuongDanHinhAnh;
            set => DuongDanHinhAnh = value;
        }

        public bool HasAudio => !string.IsNullOrWhiteSpace(DuongDanAudio);

        public bool IsKyNangNghe => KiemTraKyNangNghe(LoaiBaiHoc);

        public static bool KiemTraKyNangNghe(string? loai)
        {
            if (string.IsNullOrWhiteSpace(loai)) return false;
            string l = loai.Trim().ToLowerInvariant();
            return l == "nghe" || l == "nghehieu" || l.Contains("nghe") || l.Contains("listening");
        }

        public static string XacDinhKyNangTuTenHoacChuDe(string? tieuDe, string? tenChuDe = null, string? duongDanTep = null)
        {
            string combined = $"{tieuDe} {tenChuDe} {duongDanTep}".ToLowerInvariant();
            if (combined.Contains("nghe") || combined.Contains("listen") || combined.Contains(".mp3") || combined.Contains(".wav") || combined.Contains(".m4a"))
                return "Nghe";
            if (combined.Contains("ngữ pháp") || combined.Contains("ngu phap") || combined.Contains("grammar") || combined.Contains("tenses") || combined.Contains("mệnh đề") || combined.Contains("bị động"))
                return "NguPhap";
            if (combined.Contains("từ vựng") || combined.Contains("tu vung") || combined.Contains("vocab") || combined.Contains("flashcard"))
                return "TuVung";
            if (combined.Contains("viết") || combined.Contains("viet") || combined.Contains("writing") || combined.Contains("essay"))
                return "Viet";
            return "Doc";
        }

        private double _phanTramTienDo = 0;
        public double PhanTramTienDo
        {
            get => _phanTramTienDo;
            set => SetProperty(ref _phanTramTienDo, value);
        }

        public string TrangThai { get; set; } = "Chưa học"; // Chưa học, Đang học, Hoàn thành
        public string DiemSo { get; set; } = "--";
        public string NgayHocGanNhat { get; set; } = "Chưa học";
        public DateTime? NgayTao { get; set; }

        // Helpers for UI display
        public string BadgeBackground { get; set; } = "#E6F7F3";
        public string BadgeForeground { get; set; } = "#0F766E";
        public string IconSkill { get; set; } = "📖";
        public string StatusColor { get; set; } = "#64748B";
        public string StatusBackground { get; set; } = "#F1F5F9";
        public string ButtonActionText { get; set; } = "Vào học ngay ▶";

        // Thông tin tệp vật lý & Badge định dạng (DOCX, PDF, AUDIO)
        public string DinhDangTep { get; set; } = "DOCX";
        public string TenTepVatLy { get; set; } = string.Empty;
        public string DuongDanTepVatLy { get; set; } = string.Empty;
        public string DungLuongHienThi { get; set; } = "15 KB";
        public string BadgeDinhDangBg { get; set; } = "#EFF6FF";
        public string BadgeDinhDangFg { get; set; } = "#1D4ED8";
        public string BadgeDinhDangBorder { get; set; } = "#BFDBFE";

        public List<CauHoiDTO> DanhSachCauHoi { get; set; } = new List<CauHoiDTO>();
        public bool IsSelected { get; set; } = false;

        public BaiHocDTO Clone()
        {
            return new BaiHocDTO
            {
                MaBaiHoc = this.MaBaiHoc,
                MaChuDe = this.MaChuDe,
                MaTrinhDoId = this.MaTrinhDoId,
                NguoiTao = this.NguoiTao,
                TieuDe = this.TieuDe,
                LoaiBaiHoc = this.LoaiBaiHoc,
                TenLoaiHienThi = this.TenLoaiHienThi,
                MaTrinhDo = this.MaTrinhDo,
                TenChuDe = this.TenChuDe,
                TomTat = this.TomTat,
                NoiDung = this.NoiDung,
                DuongDanAudio = this.DuongDanAudio,
                DuongDanHinhAnh = this.DuongDanHinhAnh,
                AudioPath = this.AudioPath,
                AnhMinhHoaPath = this.AnhMinhHoaPath,
                ThoiGianUocTinhPhut = this.ThoiGianUocTinhPhut,
                DaXuatBan = this.DaXuatBan,
                PhanTramTienDo = this.PhanTramTienDo,
                TrangThai = this.TrangThai,
                DiemSo = this.DiemSo,
                NgayHocGanNhat = this.NgayHocGanNhat,
                NgayTao = this.NgayTao,
                BadgeBackground = this.BadgeBackground,
                BadgeForeground = this.BadgeForeground,
                IconSkill = this.IconSkill,
                StatusColor = this.StatusColor,
                StatusBackground = this.StatusBackground,
                ButtonActionText = this.ButtonActionText,
                DinhDangTep = this.DinhDangTep,
                TenTepVatLy = this.TenTepVatLy,
                DuongDanTepVatLy = this.DuongDanTepVatLy,
                DungLuongHienThi = this.DungLuongHienThi,
                BadgeDinhDangBg = this.BadgeDinhDangBg,
                BadgeDinhDangFg = this.BadgeDinhDangFg,
                BadgeDinhDangBorder = this.BadgeDinhDangBorder,
                DanhSachCauHoi = new List<CauHoiDTO>(this.DanhSachCauHoi ?? new List<CauHoiDTO>()),
                IsSelected = this.IsSelected
            };
        }
    }
}
