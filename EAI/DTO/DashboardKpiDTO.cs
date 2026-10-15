using System;
using System.Collections.Generic;

namespace DTO
{
    public class DashboardKpiDTO
    {
        // 4 thẻ KPI động yêu cầu US-GV02
        public int TongHocVien { get; set; } = 0;
        public double TangTruongHocVien { get; set; } = 0.0;
        public string HocVienTrendText => FormatTrend(TangTruongHocVien);
        public bool HocVienTrendPositive => TangTruongHocVien >= 0;

        public int TongBaiHoc { get; set; } = 0;
        public double TangTruongBaiHoc { get; set; } = 0.0;
        public string BaiHocTrendText => FormatTrend(TangTruongBaiHoc);
        public bool BaiHocTrendPositive => TangTruongBaiHoc >= 0;

        public int TongCauHoi { get; set; } = 0;
        public double TangTruongCauHoi { get; set; } = 0.0;
        public string CauHoiTrendText => FormatTrend(TangTruongCauHoi);
        public bool CauHoiTrendPositive => TangTruongCauHoi >= 0;

        public int TongLuotThi { get; set; } = 0;
        public double TangTruongLuotThi { get; set; } = 0.0;
        public string LuotThiTrendText => FormatTrend(TangTruongLuotThi);
        public bool LuotThiTrendPositive => TangTruongLuotThi >= 0;

        // Các chỉ số phụ trợ
        public int ChoDuyet { get; set; } = 0;
        public int TongNguoiDung { get; set; } = 0;
        public int DangHoatDong { get; set; } = 0;
        public int DaKhoa { get; set; } = 0;
        public int TongChuDe { get; set; } = 0;
        public int TongDeThi { get; set; } = 0;

        // Thống kê nhóm học theo trình độ
        public int NhomA1Count { get; set; } = 12;
        public int NhomB1Count { get; set; } = 31;
        public int NhomB2Count { get; set; } = 28;
        public int NhomC1Count { get; set; } = 23;

        // Dữ liệu bài học & đề thi nổi bật thực tế từ CSDL
        public DashboardItemDTO? BaiHoc1 { get; set; }
        public DashboardItemDTO? DeThi1 { get; set; }
        public DashboardItemDTO? BaiHoc2 { get; set; }

        // Biểu đồ xu hướng học tập
        public List<TrendPointDTO> XuHuongHocTap { get; set; } = new List<TrendPointDTO>();

        // Danh sách hoạt động gần đây
        public List<HoatDongGanDayDTO> DanhSachHoatDong { get; set; } = new List<HoatDongGanDayDTO>();

        private static string FormatTrend(double pct)
        {
            if (pct > 0) return $"+{pct:F1}% so với tuần trước";
            if (pct < 0) return $"{pct:F1}% so với tuần trước";
            return "0.0% so với tuần trước";
        }
    }

    public class TrendPointDTO
    {
        public string NgayTrongTuan { get; set; } = ""; // T2, T3, T4, T5, T6, T7, CN
        public string NgayThang { get; set; } = "";     // 10/10
        public int SoLuotLamBai { get; set; } = 0;
        public double CotHeight { get; set; } = 20.0;   // Chiều cao cột đồ thị (tối đa ~120px)
        public string CotColor { get; set; } = "#10B981";
        public string ToolTipText => $"{NgayTrongTuan} ({NgayThang}): {SoLuotLamBai} lượt học & thi";
    }

    public class HoatDongGanDayDTO
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; } = "";
        public string AvatarInitials { get; set; } = "HV";
        public string AvatarBg { get; set; } = "#DCFCE7";
        public string AvatarFg { get; set; } = "#15803D";
        public string TieuDe { get; set; } = "";
        public string NoiDung { get; set; } = "";
        public string ThoiGian { get; set; } = "";
        public string DiemSo { get; set; } = "";
        public string TrangThai { get; set; } = "Đã nộp";
        public string TrangThaiBg { get; set; } = "#DCFCE7";
        public string TrangThaiFg { get; set; } = "#15803D";
    }

    public class DashboardItemDTO
    {
        public int MaId { get; set; }
        public string MaCode { get; set; } = "";
        public string TieuDe { get; set; } = "";
        public string TomTat { get; set; } = "";
        public string LoaiHinh { get; set; } = "Ngữ pháp";
        public string TrinhDo { get; set; } = "B1";
        public string TrangThai { get; set; } = "Đã xuất bản";
        public string LoaiBadgeColor { get; set; } = "#10B981";
    }
}
