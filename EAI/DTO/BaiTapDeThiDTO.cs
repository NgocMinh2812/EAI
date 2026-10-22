using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DTO
{
    public class BaiTapDeThiDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _tenBaiTapDeThi = string.Empty;
        private string _moTa = string.Empty;
        private double _diemDat = 5.0;
        private double _tongDiem = 10.0;
        private int _soLuongCauHoi = 10;
        private int _thoiGianLamBaiPhut = 40;
        private int _soLuotLamToiDa = 0;
        private string _loaiHinh = "ThiThu";
        private string _trangThai = "BanNhap";
        private string _maCapDo = "B2";

        public int MaBaiTapDeThi { get; set; }
        public int? MaBaiHoc { get; set; }
        public int? MaChuDe { get; set; }
        public int? MaTrinhDo { get; set; }
        public string MaCapDo { get => _maCapDo; set { _maCapDo = value; NotifyPropertyChanged(); } }
        public string TenTrinhDo { get; set; } = "Trung cấp";
        public string TenChuDe { get; set; } = "Tổng hợp";
        public string TenBaiTapDeThi { get => _tenBaiTapDeThi; set { _tenBaiTapDeThi = value; NotifyPropertyChanged(); } }
        public string LoaiHinh { get => _loaiHinh; set { _loaiHinh = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(LoaiHinhHienThi)); } }
        public string MoTa { get => _moTa; set { _moTa = value; NotifyPropertyChanged(); } }
        public int ThoiGianLamBaiPhut { get => _thoiGianLamBaiPhut; set { _thoiGianLamBaiPhut = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(ThoiGianHienThi)); } }
        public int ThoiGianLamBai { get => ThoiGianLamBaiPhut; set => ThoiGianLamBaiPhut = value; }
        public bool DaPhatHanh { get; set; } = false;
        public bool ChoXemDapAn { get; set; } = true;
        public bool ChoXemLoiGiai { get; set; } = true;
        public int SoLuotLamToiDa { get => _soLuotLamToiDa; set { _soLuotLamToiDa = value; NotifyPropertyChanged(); } }
        public string TrangThai { get => _trangThai; set { _trangThai = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(TrangThaiHienThi)); } }
        public int SoLuongCauHoi { get => _soLuongCauHoi; set { _soLuongCauHoi = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(SoCauHienThi)); } }
        public int TongSoCau { get => SoLuongCauHoi; set => SoLuongCauHoi = value; }
        public double DiemDat { get => _diemDat; set { _diemDat = value; NotifyPropertyChanged(); } }
        public decimal DiemDatYeuCau { get => (decimal)DiemDat; set => DiemDat = (double)value; }
        public double TongDiem { get => _tongDiem; set { _tongDiem = value; NotifyPropertyChanged(); } }
        public bool XaoTronCauHoi { get; set; } = true;
        public int MaDeThi { get => MaBaiTapDeThi; set => MaBaiTapDeThi = value; }
        public string TieuDeDe { get => TenBaiTapDeThi; set => TenBaiTapDeThi = value; }
        public string LoaiDe { get => LoaiHinh; set => LoaiHinh = value; }
        public int? NguoiTaoId { get; set; }
        public string? TenNguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int SoLuotDaThi { get; set; } = 0;

        // UI Helpers
        public string ThoiGianHienThi => ThoiGianLamBaiPhut > 0 ? $"{ThoiGianLamBaiPhut} phút" : "Không giới hạn";
        public string SoCauHienThi => $"{SoLuongCauHoi} câu hỏi";
        public string LoaiHinhHienThi => LoaiHinh == "ThiThu" ? "Thi thử chuẩn hóa" : "Luyện tập theo chủ đề";
        public string BadgeBackground => LoaiHinh == "ThiThu" ? "#FEF3C7" : "#E0F2FE";
        public string BadgeForeground => LoaiHinh == "ThiThu" ? "#B45309" : "#0369A1";
        public string IconLoaiHinh => LoaiHinh == "ThiThu" ? "⏱️" : "🎯";

        public bool IsDraft => TrangThai == "BanNhap" || !DaPhatHanh;
        public bool IsPublished => TrangThai == "DaXuatBan" || DaPhatHanh;
        public bool IsClosed => TrangThai == "DaDong";

        public string TrangThaiHienThi => TrangThai switch
        {
            "DaXuatBan" => "Đã xuất bản",
            "DaDong" => "Đã đóng",
            _ => "Bản nháp"
        };

        public string TrangThaiBadgeBg => TrangThai switch
        {
            "DaXuatBan" => "#DCFCE7",
            "DaDong" => "#F1F5F9",
            _ => "#FEF3C7"
        };

        public string TrangThaiBadgeFg => TrangThai switch
        {
            "DaXuatBan" => "#166534",
            "DaDong" => "#475569",
            _ => "#B45309"
        };

        public string NgayTaoFormatted => NgayTao.HasValue ? NgayTao.Value.ToString("dd/MM/yyyy HH:mm") : "—";
        public string NgayCapNhatFormatted => NgayCapNhat.HasValue ? NgayCapNhat.Value.ToString("dd/MM/yyyy HH:mm") : (NgayTaoFormatted);
        public string SoLuotLamHienThi => SoLuotLamToiDa > 0 ? $"{SoLuotLamToiDa} lượt" : "Không giới hạn";
    }
}
