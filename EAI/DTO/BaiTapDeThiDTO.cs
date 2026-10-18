using System;

namespace DTO
{
    public class BaiTapDeThiDTO
    {
        public int MaBaiTapDeThi { get; set; }
        public int? MaBaiHoc { get; set; }
        public int? MaChuDe { get; set; }
        public int? MaTrinhDo { get; set; }
        public string MaCapDo { get; set; } = "B2";
        public string TenTrinhDo { get; set; } = "Trung cấp";
        public string TenChuDe { get; set; } = "Tổng hợp";
        public string TenBaiTapDeThi { get; set; } = string.Empty;
        public string LoaiHinh { get; set; } = "ThiThu"; // ThiThu / LuyenTap
        public string MoTa { get; set; } = string.Empty;
        public int ThoiGianLamBaiPhut { get; set; } = 40;
        public int ThoiGianLamBai { get => ThoiGianLamBaiPhut; set => ThoiGianLamBaiPhut = value; }
        public bool DaPhatHanh { get; set; } = true;
        public int SoLuongCauHoi { get; set; } = 10;
        public int TongSoCau { get => SoLuongCauHoi; set => SoLuongCauHoi = value; }
        public double DiemDat { get; set; } = 5.0;
        public double TongDiem { get; set; } = 10.0;
        public bool XaoTronCauHoi { get; set; } = true;
        public int MaDeThi { get => MaBaiTapDeThi; set => MaBaiTapDeThi = value; }
        public string TieuDeDe { get => TenBaiTapDeThi; set => TenBaiTapDeThi = value; }
        public string LoaiDe { get => LoaiHinh; set => LoaiHinh = value; }
        public int? NguoiTaoId { get; set; }
        public DateTime? NgayTao { get; set; }

        // UI Helpers
        public string ThoiGianHienThi => $"{ThoiGianLamBaiPhut} phút";
        public string SoCauHienThi => $"{SoLuongCauHoi} câu hỏi";
        public string LoaiHinhHienThi => LoaiHinh == "ThiThu" ? "Thi thử chuẩn hóa" : "Luyện tập theo chủ đề";
        public string BadgeBackground => LoaiHinh == "ThiThu" ? "#FEF3C7" : "#E0F2FE";
        public string BadgeForeground => LoaiHinh == "ThiThu" ? "#B45309" : "#0369A1";
        public string IconLoaiHinh => LoaiHinh == "ThiThu" ? "⏱️" : "🎯";
    }
}
