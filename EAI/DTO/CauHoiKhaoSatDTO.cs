using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO
{
    public class LuaChonDapAnDTO : INotifyPropertyChanged
    {
        private int _maLuaChon;
        private int _maCauHoi;
        private string _kyHieuLuaChon = string.Empty;
        private string _noiDungLuaChon = string.Empty;
        private bool _laDapAnDung;

        public int MaLuaChon
        {
            get => _maLuaChon;
            set { if (_maLuaChon != value) { _maLuaChon = value; OnPropertyChanged(); } }
        }

        public int MaCauHoi
        {
            get => _maCauHoi;
            set { if (_maCauHoi != value) { _maCauHoi = value; OnPropertyChanged(); } }
        }

        public string KyHieuLuaChon
        {
            get => _kyHieuLuaChon;
            set { if (_kyHieuLuaChon != value) { _kyHieuLuaChon = value; OnPropertyChanged(); } }
        }

        public string NoiDungLuaChon
        {
            get => _noiDungLuaChon;
            set { if (_noiDungLuaChon != value) { _noiDungLuaChon = value; OnPropertyChanged(); } }
        }

        public bool LaDapAnDung
        {
            get => _laDapAnDung;
            set { if (_laDapAnDung != value) { _laDapAnDung = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CauHoiKhaoSatDTO : INotifyPropertyChanged
    {
        private int _maCauHoi;
        private int _thuTuCauHoi;
        private string _kyNang = string.Empty;
        private string _maTrinhDo = string.Empty;
        private string _tenTrinhDo = string.Empty;
        private string _noiDungCauHoi = string.Empty;
        private string? _doanVanDoc;
        private string? _giaiThichDapAn;
        private int? _maChuDe;
        private int? _maTrinhDoId;
        private string _loaiCauHoi = "TracNghiem";
        private string _doKho = "TrungBinh";
        private string? _audioPath;
        private int? _maBaiHoc;
        private string? _tenBaiHoc;
        private bool _daKiemDuyet = true;
        private int? _nguoiTaoId;
        private DateTime? _ngayTao;
        private bool _isSelected = true;
        private int _thuTuTrongBo = 1;

        public int MaCauHoi
        {
            get => _maCauHoi;
            set { if (_maCauHoi != value) { _maCauHoi = value; OnPropertyChanged(); } }
        }

        public int ThuTuCauHoi
        {
            get => _thuTuCauHoi;
            set { if (_thuTuCauHoi != value) { _thuTuCauHoi = value; OnPropertyChanged(); } }
        }

        public static string ChuanHoaKyNang(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "Doc";
            string lower = raw.Trim().ToLowerInvariant();
            if (lower.Contains("nghe") || lower.Contains("listen")) return "Nghe";
            if (lower.Contains("nguphap") || lower.Contains("grammar")) return "NguPhap";
            if (lower.Contains("tuvung") || lower.Contains("vocab")) return "TuVung";
            if (lower.Contains("viet") || lower.Contains("writ") || lower.Contains("essay")) return "Viet";
            if (lower.Contains("noi") || lower.Contains("speak")) return "Noi";
            if (lower.Contains("doc") || lower.Contains("read")) return "Doc";
            return raw.Trim();
        }

        public static string ChuanHoaDoKho(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "TrungBinh";
            string lower = raw.Trim().ToLowerInvariant();
            if (lower.Contains("de") || lower.Contains("easy")) return "De";
            if (lower.Contains("kho") || lower.Contains("hard") || lower.Contains("diff")) return "Kho";
            return "TrungBinh";
        }

        public string KyNang
        {
            get => _kyNang;
            set
            {
                string norm = ChuanHoaKyNang(value);
                if (_kyNang != norm)
                {
                    _kyNang = norm;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ChuyenMucHienThi));
                    OnPropertyChanged(nameof(KyNangHienThi));
                    OnPropertyChanged(nameof(IsKyNangNghe));
                    OnPropertyChanged(nameof(IsKyNangDoc));
                }
            }
        }

        public static string LayTenKyNang(string? kyNang) => (kyNang?.Trim()) switch
        {
            "Doc" or "DocHieu" or "Reading" => "Đọc hiểu",
            "Nghe" or "NgheHieu" or "Listening" => "Nghe hiểu",
            "NguPhap" or "Grammar" => "Ngữ pháp",
            "TuVung" or "Vocabulary" => "Từ vựng",
            "Viet" or "Writing" => "Viết",
            "Noi" or "Speaking" => "Nói",
            _ => string.IsNullOrWhiteSpace(kyNang) ? "Đọc hiểu" : kyNang
        };

        public static string LayTenLoaiCauHoi(string? loai)
        {
            string norm = ChuanHoaLoaiCauHoi(loai);
            return norm switch
            {
                "TracNghiem" => "Trắc nghiệm",
                "DungSai" => "Đúng / Sai",
                "TrueFalseNotGiven" => "True / False",
                "DienKhuyet" => "Điền khuyết",
                "TraLoiNgan" => "Trả lời ngắn",
                "NoiCap" => "Nối cặp",
                "SapXep" => "Sắp xếp",
                "SuaLoiSai" => "Sửa lỗi sai",
                "VietLaiCau" => "Viết lại câu",
                "TuLuan" => "Tự luận",
                _ => "Trắc nghiệm"
            };
        }

        public string KyNangHienThi
        {
            get => LayTenKyNang(KyNang);
        }

        public bool IsKyNangNghe =>
            string.Equals(KyNang, "Nghe", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(KyNang, "NgheHieu", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(KyNang, "Listening", StringComparison.OrdinalIgnoreCase);

        public bool IsKyNangDoc =>
            string.Equals(KyNang, "Doc", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(KyNang, "DocHieu", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(KyNang, "Reading", StringComparison.OrdinalIgnoreCase);

        public string MaTrinhDo
        {
            get => _maTrinhDo;
            set
            {
                if (_maTrinhDo != value)
                {
                    _maTrinhDo = value;
                    _maTrinhDoId = value switch
                    {
                        "A1" => 0,
                        "A2" => 1,
                        "B1" => 2,
                        "B2" => 3,
                        "C1" => 4,
                        "C2" => 5,
                        _ => _maTrinhDoId
                    };
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ChuyenMucHienThi));
                    OnPropertyChanged(nameof(MaTrinhDoId));
                }
            }
        }

        public string TenTrinhDo
        {
            get => _tenTrinhDo;
            set { if (_tenTrinhDo != value) { _tenTrinhDo = value; OnPropertyChanged(); } }
        }

        public string NoiDungCauHoi
        {
            get => _noiDungCauHoi;
            set { if (_noiDungCauHoi != value) { _noiDungCauHoi = value; OnPropertyChanged(); } }
        }

        public string? DoanVanDoc
        {
            get => _doanVanDoc;
            set { if (_doanVanDoc != value) { _doanVanDoc = value; OnPropertyChanged(); } }
        }

        public string? GiaiThichDapAn
        {
            get => _giaiThichDapAn;
            set { if (_giaiThichDapAn != value) { _giaiThichDapAn = value; OnPropertyChanged(); } }
        }

        public int? MaChuDe
        {
            get => _maChuDe;
            set { if (_maChuDe != value) { _maChuDe = value; OnPropertyChanged(); } }
        }

        public int? MaTrinhDoId
        {
            get => _maTrinhDoId;
            set { if (_maTrinhDoId != value) { _maTrinhDoId = value; OnPropertyChanged(); } }
        }

        public static string ChuanHoaLoaiCauHoi(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "TracNghiem";
            string lower = raw.Trim().ToLowerInvariant();
            if (lower.Contains("tfng") || lower.Contains("notgiven") || lower.Contains("not given")) return "TrueFalseNotGiven";
            if (lower.Contains("truefalse") || lower.Contains("true/false") || lower.Contains("dungsai") || lower == "tf") return "DungSai";
            if (lower.Contains("shortanswer") || lower.Contains("short_answer") || lower.Contains("traloingan")) return "TraLoiNgan";
            if (lower.Contains("matching") || lower.Contains("noicap") || lower.Contains("gheptieude")) return "NoiCap";
            if (lower.Contains("sequence") || lower.Contains("ordering") || lower.Contains("sapxep")) return "SapXep";
            if (lower.Contains("error") || lower.Contains("loi") || lower.Contains("timloi") || lower.Contains("sualoi")) return "SuaLoiSai";
            if (lower.Contains("transform") || lower.Contains("vietlai") || lower.Contains("rewrite")) return "VietLaiCau";
            if (lower.Contains("essay") || lower.Contains("paragraph") || lower.Contains("tuluan") || lower.Contains("writing") || lower.Contains("bai_viet")) return "TuLuan";
            if (lower.Contains("fill") || lower.Contains("dientu") || lower.Contains("dienkhuyet") || lower.Contains("blank") || lower.Contains("completion")) return "DienKhuyet";
            return "TracNghiem";
        }

        public string LoaiCauHoi
        {
            get => _loaiCauHoi;
            set
            {
                string norm = ChuanHoaLoaiCauHoi(value);
                if (_loaiCauHoi != norm)
                {
                    _loaiCauHoi = norm;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LoaiCauHoiHienThi));
                    OnPropertyChanged(nameof(IsDangTracNghiem));
                    OnPropertyChanged(nameof(IsDangDungSai));
                    OnPropertyChanged(nameof(IsDangTrueFalseNotGiven));
                    OnPropertyChanged(nameof(IsDangLuaChon));
                    OnPropertyChanged(nameof(IsDangNhapVanBan));
                    OnPropertyChanged(nameof(IsDangTuLuan));
                    OnPropertyChanged(nameof(IsDangNoiCap));
                    OnPropertyChanged(nameof(IsDangSapXep));
                }
            }
        }

        public string LoaiCauHoiHienThi
        {
            get
            {
                return LoaiCauHoi switch
                {
                    "TracNghiem" => "Trắc nghiệm",
                    "DungSai" => "Đúng / Sai",
                    "TrueFalseNotGiven" => "True / False / Not Given",
                    "DienKhuyet" => "Điền khuyết",
                    "TraLoiNgan" => "Trả lời ngắn",
                    "NoiCap" => "Nối cặp (Matching)",
                    "SapXep" => "Sắp xếp thứ tự",
                    "SuaLoiSai" => "Sửa lỗi sai",
                    "VietLaiCau" => "Viết lại câu",
                    "TuLuan" => "Tự luận / Viết bài",
                    _ => string.IsNullOrWhiteSpace(LoaiCauHoi) ? "Trắc nghiệm" : LoaiCauHoi
                };
            }
        }

        public bool IsDangTracNghiem => LoaiCauHoi == "TracNghiem";
        public bool IsDangDungSai => LoaiCauHoi == "DungSai";
        public bool IsDangTrueFalseNotGiven => LoaiCauHoi == "TrueFalseNotGiven";
        public bool IsDangLuaChon => IsDangTracNghiem || IsDangDungSai || IsDangTrueFalseNotGiven;
        public bool IsDangNhapVanBan => LoaiCauHoi == "DienKhuyet" || LoaiCauHoi == "TraLoiNgan" || LoaiCauHoi == "VietLaiCau" || LoaiCauHoi == "SuaLoiSai";
        public bool IsDangTuLuan => LoaiCauHoi == "TuLuan";
        public bool IsDangNoiCap => LoaiCauHoi == "NoiCap";
        public bool IsDangSapXep => LoaiCauHoi == "SapXep";

        private string? _cauTraLoiVanBan;
        public string? CauTraLoiVanBan
        {
            get => _cauTraLoiVanBan;
            set { if (_cauTraLoiVanBan != value) { _cauTraLoiVanBan = value; OnPropertyChanged(); } }
        }

        public string DapAnDungText
        {
            get
            {
                var dung = DanhSachLuaChon?.FirstOrDefault(x => x.LaDapAnDung);
                if (dung != null && !string.IsNullOrWhiteSpace(dung.NoiDungLuaChon))
                    return dung.NoiDungLuaChon;
                return DanhSachLuaChon?.FirstOrDefault()?.NoiDungLuaChon ?? "";
            }
        }

        public string TieuChiChamRubric
        {
            get
            {
                var rubricOpt = DanhSachLuaChon?.FirstOrDefault(x => x.KyHieuLuaChon == "RUB" || x.KyHieuLuaChon == "RUBRIC" || x.KyHieuLuaChon == "TC");
                if (rubricOpt != null && !string.IsNullOrWhiteSpace(rubricOpt.NoiDungLuaChon))
                    return rubricOpt.NoiDungLuaChon;
                return GiaiThichDapAn ?? "Chấm theo 4 tiêu chí: Task Response, Coherence & Cohesion, Lexical Resource, Grammatical Range.";
            }
        }

        public string DoKho
        {
            get => _doKho;
            set
            {
                string norm = ChuanHoaDoKho(value);
                if (_doKho != norm)
                {
                    _doKho = norm;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DoKhoHienThi));
                }
            }
        }

        public string DoKhoHienThi
        {
            get
            {
                return DoKho switch
                {
                    "De" or "Easy" => "Dễ",
                    "TrungBinh" or "Medium" => "Trung Bình",
                    "Kho" or "Hard" => "Khó",
                    _ => string.IsNullOrWhiteSpace(DoKho) ? "Trung Bình" : DoKho
                };
            }
        }

        public string? AudioPath
        {
            get => _audioPath;
            set { if (_audioPath != value) { _audioPath = value; OnPropertyChanged(); } }
        }

        public int? MaBaiHoc
        {
            get => _maBaiHoc;
            set { if (_maBaiHoc != value) { _maBaiHoc = value; OnPropertyChanged(); } }
        }

        public string? TenBaiHoc
        {
            get => _tenBaiHoc;
            set { if (_tenBaiHoc != value) { _tenBaiHoc = value; OnPropertyChanged(); } }
        }

        public bool DaKiemDuyet
        {
            get => _daKiemDuyet;
            set { if (_daKiemDuyet != value) { _daKiemDuyet = value; OnPropertyChanged(); } }
        }

        public int? NguoiTaoId
        {
            get => _nguoiTaoId;
            set { if (_nguoiTaoId != value) { _nguoiTaoId = value; OnPropertyChanged(); } }
        }

        public DateTime? NgayTao
        {
            get => _ngayTao;
            set { if (_ngayTao != value) { _ngayTao = value; OnPropertyChanged(); } }
        }

        private string? _loaiNguon = "Extracted";
        private string? _tepTinNguon;
        private string? _doanVanDanChung;
        private string? _maDinhDanhSeed;

        public string? LoaiNguon
        {
            get => _loaiNguon;
            set
            {
                if (_loaiNguon != value)
                {
                    _loaiNguon = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LoaiNguonHienThi));
                    OnPropertyChanged(nameof(IsExtracted));
                }
            }
        }

        public string LoaiNguonHienThi => LoaiNguon switch
        {
            "Applied" => "Vận dụng kiến thức",
            _ => "Trích xuất bài học"
        };

        public bool IsExtracted => LoaiNguon != "Applied";

        public string? TepTinNguon
        {
            get => _tepTinNguon;
            set { if (_tepTinNguon != value) { _tepTinNguon = value; OnPropertyChanged(); } }
        }

        public string? DoanVanDanChung
        {
            get => _doanVanDanChung;
            set { if (_doanVanDanChung != value) { _doanVanDanChung = value; OnPropertyChanged(); } }
        }

        public string? MaDinhDanhSeed
        {
            get => _maDinhDanhSeed;
            set { if (_maDinhDanhSeed != value) { _maDinhDanhSeed = value; OnPropertyChanged(); } }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { if (_isSelected != value) { _isSelected = value; OnPropertyChanged(); } }
        }

        public int ThuTuTrongBo
        {
            get => _thuTuTrongBo;
            set { if (_thuTuTrongBo != value) { _thuTuTrongBo = value; OnPropertyChanged(); } }
        }

        public List<LuaChonDapAnDTO> DanhSachLuaChon { get; set; } = new List<LuaChonDapAnDTO>();

        // Lựa chọn hiện tại của học viên
        public int? MaLuaChonDaChon { get; set; }
        public int ChiMucDaChon { get; set; } = -1; // 0: A, 1: B, 2: C, 3: D

        public string ChuyenMucHienThi
        {
            get
            {
                string kyNangStr = KyNang switch
                {
                    "NguPhap" => "NGỮ PHÁP",
                    "TuVung" => "TỪ VỰNG",
                    "Doc" => "ĐỌC HIỂU",
                    "Nghe" => "NGHE HIỂU",
                    "NgheHieu" => "NGHE HIỂU",
                    "Viet" => "VIẾT",
                    "Noi" => "NÓI",
                    _ => KyNang?.ToUpper() ?? "KIẾN THỨC"
                };
                return $"{kyNangStr} • TRÌNH ĐỘ {MaTrinhDo}";
            }
        }

        private double _diem = 1.0;
        public double Diem
        {
            get => _diem;
            set { if (_diem != value) { _diem = value; OnPropertyChanged(); } }
        }

        private bool _isAnswersExpanded = false;
        public bool IsAnswersExpanded
        {
            get => _isAnswersExpanded;
            set { if (_isAnswersExpanded != value) { _isAnswersExpanded = value; OnPropertyChanged(); } }
        }

        private bool _isInCurrentSurvey = false;
        public bool IsInCurrentSurvey
        {
            get => _isInCurrentSurvey;
            set { if (_isInCurrentSurvey != value) { _isInCurrentSurvey = value; OnPropertyChanged(); } }
        }

        public bool HasAudio => !string.IsNullOrEmpty(AudioPath);

        public string SkillBadgeBg => KyNang switch
        {
            "NguPhap" => "#EEF2FF",
            "TuVung" => "#F3E8FF",
            "Doc" or "DocHieu" => "#ECFDF5",
            "Nghe" or "NgheHieu" => "#FEF3C7",
            "Viet" => "#FCE7F3",
            "Noi" => "#E0F2FE",
            _ => "#F1F5F9"
        };

        public string SkillBadgeFg => KyNang switch
        {
            "NguPhap" => "#4338CA",
            "TuVung" => "#7E22CE",
            "Doc" or "DocHieu" => "#065F46",
            "Nghe" or "NgheHieu" => "#B45309",
            "Viet" => "#BE185D",
            "Noi" => "#0369A1",
            _ => "#475569"
        };

        public string DifficultyBadgeBg => DoKho switch
        {
            "De" => "#DCFCE7",
            "Kho" => "#FEE2E2",
            _ => "#FEF3C7"
        };

        public string DifficultyBadgeFg => DoKho switch
        {
            "De" => "#15803D",
            "Kho" => "#DC2626",
            _ => "#B45309"
        };

        public string LevelBadgeBg => MaTrinhDo switch
        {
            "A1" => "#DCFCE7",
            "A2" => "#CCFBF1",
            "B1" => "#DBEAFE",
            "B2" => "#E0E7FF",
            "C1" => "#F3E8FF",
            "C2" => "#FEF3C7",
            _ => "#F1F5F9"
        };

        public string LevelBadgeFg => MaTrinhDo switch
        {
            "A1" => "#15803D",
            "A2" => "#0F766E",
            "B1" => "#1D4ED8",
            "B2" => "#4338CA",
            "C1" => "#7E22CE",
            "C2" => "#B45309",
            _ => "#475569"
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class KetQuaKhaoSatDTO
    {
        public int TongSoCau { get; set; }
        public int SoCauDung { get; set; }
        public decimal DiemSo { get; set; } // Thang 10

        // Thông tin tự chọn & đánh giá thích ứng
        public string TrinhDoDaChon { get; set; } = "B1"; // Cấp độ học viên tự chọn ban đầu
        public bool KetQuaDat { get; set; } = true; // true: Đạt chuẩn cấp độ đã chọn, false: Chưa đạt (hạ cấp)
        public string TrinhDoPhuHop { get; set; } = "B1"; // Cấp độ AI xác định phù hợp
        public string TenTrinhDoPhuHop { get; set; } = "Intermediate (Trung cấp)";
        public string LyDoDieuChinh { get; set; } = string.Empty;

        // Tương thích
        public string TrinhDoCEFR => TrinhDoPhuHop;
        public string TenTrinhDo => TenTrinhDoPhuHop;
        public string UocTinhIelts { get; set; } = "5.0 - 5.5";
        public string UocTinhToeic { get; set; } = "550 - 650";
        public string NhanXetLộTrinh { get; set; } = string.Empty;

        // Danh sách bài học AI đề xuất phù hợp năng lực
        public List<BaiHocDTO> DanhSachBaiHocDeXuat { get; set; } = new List<BaiHocDTO>();
    }
}
