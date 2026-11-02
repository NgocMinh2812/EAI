using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO
{
    /// <summary>
    /// Đại diện cho Cấu trúc đề thi chuẩn hóa (Exam Blueprint)
    /// Độc lập với đề thi/bài tập cụ thể, phản ánh đúng ma trận đề thi chuẩn quốc tế (IELTS, TOEIC, VSTEP, Cambridge CEFR, THPT Quốc Gia)
    /// </summary>
    public class CauTrucDeThiDTO : INotifyPropertyChanged
    {
        private int _maCauTruc;
        private string _tenCauTruc = string.Empty;
        private string _maKyThi = "IELTS";
        private string _maCapDo = "B2";
        private string _loaiDeThi = "ThiThu";
        private int _thoiGianPhut = 60;
        private int _tongSoCau = 40;
        private double _tongDiem = 10.0;
        private string? _moTa;
        private string? _nguonThamChieu;
        private DateTime _ngayXacMinh = DateTime.Now;
        private string _trangThai = "HoatDong";

        public int MaCauTruc
        {
            get => _maCauTruc;
            set { if (_maCauTruc != value) { _maCauTruc = value; OnPropertyChanged(); } }
        }

        public string TenCauTruc
        {
            get => _tenCauTruc;
            set { if (_tenCauTruc != value) { _tenCauTruc = value; OnPropertyChanged(); } }
        }

        public string MaKyThi
        {
            get => _maKyThi;
            set
            {
                if (_maKyThi != value)
                {
                    _maKyThi = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TenKyThiHienThi));
                    OnPropertyChanged(nameof(BadgeKyThiBg));
                    OnPropertyChanged(nameof(BadgeKyThiFg));
                }
            }
        }

        public string TenKyThiHienThi => MaKyThi switch
        {
            "IELTS" => "IELTS Academic",
            "TOEIC" => "TOEIC L&R",
            "VSTEP" => "VSTEP (B1-B2-C1)",
            "CAMBRIDGE_KET" => "Cambridge A2 Key (KET)",
            "CAMBRIDGE_PET" => "Cambridge B1 Preliminary (PET)",
            "CAMBRIDGE_FCE" => "Cambridge B2 First (FCE)",
            "CAMBRIDGE_CAE" => "Cambridge C1 Advanced (CAE)",
            "STARTERS" => "Pre A1 Starters (YLE)",
            "MOVERS" => "A1 Movers (YLE)",
            "FLYERS" => "A2 Flyers (YLE)",
            "THPT_QG" => "THPT Quốc Gia",
            "CEFR_PRACTICE" => "CEFR Practice",
            _ => MaKyThi
        };

        public string TenHienThi => MaCauTruc <= 0 
            ? TenCauTruc 
            : $"[{TenKyThiHienThi}] {TenCauTruc} ({TongSoCau} câu • {ThoiGianPhut} phút • {MaCapDo})";

        public string BadgeKyThiBg => MaKyThi switch
        {
            "IELTS" => "#FEE2E2",
            "TOEIC" => "#FEF3C7",
            "VSTEP" => "#E0E7FF",
            "CAMBRIDGE_CAE" or "CAMBRIDGE_FCE" => "#F3E8FF",
            "THPT_QG" => "#FEF08A",
            _ => "#E0F2FE"
        };

        public string BadgeKyThiFg => MaKyThi switch
        {
            "IELTS" => "#991B1B",
            "TOEIC" => "#92400E",
            "VSTEP" => "#3730A3",
            "CAMBRIDGE_CAE" or "CAMBRIDGE_FCE" => "#6B21A8",
            "THPT_QG" => "#854D0E",
            _ => "#075985"
        };

        public string MaCapDo
        {
            get => _maCapDo;
            set { if (_maCapDo != value) { _maCapDo = value; OnPropertyChanged(); } }
        }

        public string LoaiDeThi
        {
            get => _loaiDeThi;
            set { if (_loaiDeThi != value) { _loaiDeThi = value; OnPropertyChanged(); } }
        }

        public int ThoiGianPhut
        {
            get => _thoiGianPhut;
            set { if (_thoiGianPhut != value) { _thoiGianPhut = value; OnPropertyChanged(); OnPropertyChanged(nameof(ThoiGianHienThi)); } }
        }

        public string ThoiGianHienThi => $"{ThoiGianPhut} phút";

        public int TongSoCau
        {
            get => _tongSoCau;
            set { if (_tongSoCau != value) { _tongSoCau = value; OnPropertyChanged(); OnPropertyChanged(nameof(SoCauHienThi)); } }
        }

        public string SoCauHienThi => $"{TongSoCau} câu";

        public double TongDiem
        {
            get => _tongDiem;
            set { if (_tongDiem != value) { _tongDiem = value; OnPropertyChanged(); } }
        }

        public string? MoTa
        {
            get => _moTa;
            set { if (_moTa != value) { _moTa = value; OnPropertyChanged(); } }
        }

        public string? NguonThamChieu
        {
            get => _nguonThamChieu;
            set { if (_nguonThamChieu != value) { _nguonThamChieu = value; OnPropertyChanged(); } }
        }

        public DateTime NgayXacMinh
        {
            get => _ngayXacMinh;
            set { if (_ngayXacMinh != value) { _ngayXacMinh = value; OnPropertyChanged(); } }
        }

        public string TrangThai
        {
            get => _trangThai;
            set { if (_trangThai != value) { _trangThai = value; OnPropertyChanged(); } }
        }

        public List<ChiTietCauTrucDeThiDTO> DanhSachPhanThi { get; set; } = new List<ChiTietCauTrucDeThiDTO>();

        public int SoLuongPhanThi => DanhSachPhanThi?.Count ?? 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Chi tiết từng phần thi (Section / Part) trong ma trận đề thi chuẩn
    /// </summary>
    public class ChiTietCauTrucDeThiDTO : INotifyPropertyChanged
    {
        private int _maChiTiet;
        private int _maCauTruc;
        private string _tenPhan = string.Empty;
        private int _thuTuPhan = 1;
        private string _kyNang = "Doc";
        private string _loaiCauHoiChoPhep = "TracNghiem";
        private int _soLuongCau = 10;
        private double _diemMoiCau = 1.0;
        private int? _thoiGianPhut;
        private bool _yeuCauDoanVan;
        private bool _yeuCauAudio;
        private string? _moTa;

        public int MaChiTiet
        {
            get => _maChiTiet;
            set { if (_maChiTiet != value) { _maChiTiet = value; OnPropertyChanged(); } }
        }

        public int MaCauTruc
        {
            get => _maCauTruc;
            set { if (_maCauTruc != value) { _maCauTruc = value; OnPropertyChanged(); } }
        }

        public string TenPhan
        {
            get => _tenPhan;
            set { if (_tenPhan != value) { _tenPhan = value; OnPropertyChanged(); } }
        }

        public int ThuTuPhan
        {
            get => _thuTuPhan;
            set { if (_thuTuPhan != value) { _thuTuPhan = value; OnPropertyChanged(); } }
        }

        public string KyNang
        {
            get => _kyNang;
            set { if (_kyNang != value) { _kyNang = value; OnPropertyChanged(); } }
        }

        public string LoaiCauHoiChoPhep
        {
            get => _loaiCauHoiChoPhep;
            set { if (_loaiCauHoiChoPhep != value) { _loaiCauHoiChoPhep = value; OnPropertyChanged(); } }
        }

        public int SoLuongCau
        {
            get => _soLuongCau;
            set { if (_soLuongCau != value) { _soLuongCau = value; OnPropertyChanged(); } }
        }

        public double DiemMoiCau
        {
            get => _diemMoiCau;
            set { if (_diemMoiCau != value) { _diemMoiCau = value; OnPropertyChanged(); } }
        }

        public int? ThoiGianPhut
        {
            get => _thoiGianPhut;
            set { if (_thoiGianPhut != value) { _thoiGianPhut = value; OnPropertyChanged(); } }
        }

        public bool YeuCauDoanVan
        {
            get => _yeuCauDoanVan;
            set { if (_yeuCauDoanVan != value) { _yeuCauDoanVan = value; OnPropertyChanged(); } }
        }

        public bool YeuCauAudio
        {
            get => _yeuCauAudio;
            set { if (_yeuCauAudio != value) { _yeuCauAudio = value; OnPropertyChanged(); } }
        }

        public string? MoTa
        {
            get => _moTa;
            set { if (_moTa != value) { _moTa = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
