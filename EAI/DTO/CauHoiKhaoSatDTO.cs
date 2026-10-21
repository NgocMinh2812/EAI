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

        public string KyNang
        {
            get => _kyNang;
            set
            {
                if (_kyNang != value)
                {
                    _kyNang = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ChuyenMucHienThi));
                    OnPropertyChanged(nameof(IsKyNangNghe));
                    OnPropertyChanged(nameof(IsKyNangDoc));
                }
            }
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
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ChuyenMucHienThi));
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

        public string LoaiCauHoi
        {
            get => _loaiCauHoi;
            set { if (_loaiCauHoi != value) { _loaiCauHoi = value; OnPropertyChanged(); } }
        }

        public string DoKho
        {
            get => _doKho;
            set { if (_doKho != value) { _doKho = value; OnPropertyChanged(); } }
        }

        public string? AudioPath
        {
            get => _audioPath;
            set { if (_audioPath != value) { _audioPath = value; OnPropertyChanged(); } }
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
                    _ => KyNang?.ToUpper() ?? "KIẾN THỨC"
                };
                return $"{kyNangStr} • TRÌNH ĐỘ {MaTrinhDo}";
            }
        }

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
