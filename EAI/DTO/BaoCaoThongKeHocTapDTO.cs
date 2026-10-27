using System;
using System.Collections.Generic;

namespace DTO
{
    public class ThongKeHocTapFilterDTO
    {
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public int? MaBaiTapDeThi { get; set; }
        public string? HinhThucLam { get; set; } = "TatCa"; // TatCa / LuyenTap / ThiThu
        public string? TuKhoa { get; set; }
        public string? TrinhDo { get; set; }
    }

    public class BaoCaoThongKeHocTapDTO
    {
        // 1. Nhóm chỉ số KPI
        public int TongSoLuotLam { get; set; }
        public int SoLuotDaNop { get; set; }
        public int SoLuotDangLam { get; set; }
        public int SoLuotDaHuy { get; set; }
        public double DiemTrungBinh { get; set; }
        public int ThoiGianTrungBinhGiay { get; set; }
        public double TyLeHoanThanh { get; set; }
        public int SoHocVienThamGia { get; set; }

        public string DiemTrungBinhText => DiemTrungBinh > 0 ? $"{DiemTrungBinh:0.0} / 10" : "0.0";
        public string ThoiGianTrungBinhText => $"{ThoiGianTrungBinhGiay / 60}p {ThoiGianTrungBinhGiay % 60}s";
        public string TyLeHoanThanhText => $"{TyLeHoanThanh:0.0}%";

        // 2. Biểu đồ phân bố điểm số
        public List<PhanBoDiemItem> DanhSachPhanBoDiem { get; set; } = new();

        // 3. Biểu đồ xu hướng nộp bài theo ngày
        public List<XuHuongNgayItem> DanhSachXuHuongNgay { get; set; } = new();

        // 4. Thống kê tỷ lệ theo kỹ năng
        public List<KyNangThongKeItem> DanhSachKyNang { get; set; } = new();

        // 5. Bảng danh sách chi tiết các lượt làm bài
        public List<LuotLamBaiDTO> DanhSachLuotLamBai { get; set; } = new();
    }

    public class PhanBoDiemItem
    {
        public string KhoangDiem { get; set; } = string.Empty;
        public string NhanXet { get; set; } = string.Empty;
        public int SoLuot { get; set; }
        public double TyLePhanTram { get; set; }
        public string MauSac { get; set; } = "#2563EB";
        public double CotHeight { get; set; } = 20;

        public string HienThiText => $"{SoLuot} ({TyLePhanTram:0}%)";
    }

    public class XuHuongNgayItem
    {
        public string Ngay { get; set; } = string.Empty;
        public DateTime FullDate { get; set; }
        public int SoLuot { get; set; }
        public double DiemTrungBinh { get; set; }
        public double CotHeight { get; set; } = 20;
    }

    public class KyNangThongKeItem
    {
        public string KyNang { get; set; } = string.Empty;
        public string TenKyNang { get; set; } = string.Empty;
        public int SoCauDung { get; set; }
        public int TongSoCau { get; set; }
        public double TyLeDung { get; set; }
        public string MauSac { get; set; } = "#10B981";

        public string TiLeText => $"{TyLeDung:0.0}%";
        public string ChiTietText => $"{SoCauDung}/{TongSoCau} câu đúng";
    }
}
