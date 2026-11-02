using System;
using System.Collections.Generic;

namespace DTO
{
    public class LuotLamBaiDTO
    {
        public int MaLuotLamBai { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaBaiTapDeThi { get; set; }
        public int? MaCauTrucDeThi { get; set; }
        public string? TenCauTrucDeThi { get; set; }
        public string TenBaiTapDeThi { get; set; } = string.Empty;
        public string HinhThucLam { get; set; } = "ThiThu";
        public DateTime ThoiDiemBatDau { get; set; } = DateTime.Now;
        public DateTime? ThoiDiemNopBai { get; set; }
        public int ThoiGianLamGiay { get; set; }
        public int TongSoCau { get; set; }
        public int SoCauDung { get; set; }
        public int SoCauSai { get; set; }
        public decimal DiemSo { get; set; }
        public string TrangThai { get; set; } = "DangLam"; // DangLam / DaNop / DaHuy
        public string HoTenNguoiDung { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public bool ChoXemDapAn { get; set; } = true;
        public bool ChoXemLoiGiai { get; set; } = true;
        public decimal DiemDat { get; set; } = 5.0m;
        public decimal TongDiem { get; set; } = 10.0m;

        public List<ChiTietTraLoiDTO> DanhSachChiTiet { get; set; } = new List<ChiTietTraLoiDTO>();

        // Display Helpers
        public string TenBaiTap { get => TenBaiTapDeThi; set => TenBaiTapDeThi = value; }
        public DateTime ThoiGianKetThuc => ThoiDiemNopBai ?? ThoiDiemBatDau;
        public string TyLeChinhXacText => TyLeChinhXac;
        public string DiemHienThi => $"{DiemSo:0.0} / {TongDiem:0.0}";
        public string ThoiGianHienThi => $"{ThoiGianLamGiay / 60:D2}:{ThoiGianLamGiay % 60:D2}";
        public string TyLeChinhXac => TongSoCau > 0 ? $"{(SoCauDung * 100.0 / TongSoCau):0}%" : "0%";
        public bool LaDatYeuCau => DiemSo >= DiemDat;
        public string TrangThaiKetQuaText => LaDatYeuCau ? "ĐẠT (PASS)" : "CHƯA ĐẠT";
        public string TrangThaiKetQuaBg => LaDatYeuCau ? "#DCFCE7" : "#FEE2E2";
        public string TrangThaiKetQuaFg => LaDatYeuCau ? "#166534" : "#991B1B";
        public string NgayNopFormatted => ThoiDiemNopBai.HasValue ? ThoiDiemNopBai.Value.ToString("dd/MM/yyyy HH:mm") : ThoiDiemBatDau.ToString("dd/MM/yyyy HH:mm");

        public string DanhGiaTongQuan => DiemSo >= 8.0m ? "Xuất sắc! Đạt chuẩn band mong muốn."
                                       : DiemSo >= 6.5m ? "Tốt! Đạt ngưỡng điểm mục tiêu."
                                       : DiemSo >= DiemDat ? "Đạt yêu cầu! Tiếp tục duy trì phong độ."
                                       : "Chưa đạt ngưỡng điểm yêu cầu, hãy ôn tập lại các câu sai.";

        public string HinhThucHienThi => HinhThucLam == "ThiThu" ? "Thi thử" : "Luyện tập";
        public double ThoiGianLamPhutHienThi => Math.Round(ThoiGianLamGiay / 60.0, 1);
        public string SoCauDungTrenTong => $"{SoCauDung}/{TongSoCau}";
        public string TrangThaiHienThi => TrangThai == "DaNop" ? "Đã nộp bài" : (TrangThai == "HetGio" ? "Hết giờ" : "Đang làm");
    }
}
