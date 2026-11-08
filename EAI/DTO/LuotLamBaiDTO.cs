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
        public string HinhThucBg => HinhThucLam == "ThiThu" ? "#EFF6FF" : "#FAF5FF";
        public string HinhThucFg => HinhThucLam == "ThiThu" ? "#1D4ED8" : "#7E22CE";

        public double ThoiGianLamPhutHienThi => Math.Round(ThoiGianLamGiay / 60.0, 1);
        public string SoCauDungTrenTong => $"{SoCauDung}/{TongSoCau}";
        
        public string TrangThaiHienThi => TrangThai == "DaNop" ? "Đã nộp bài" : (TrangThai == "HetGio" ? "Hết giờ" : (TrangThai == "DaHuy" ? "Đã hủy" : "Đang làm"));
        public string TrangThaiBg => TrangThai == "DaNop" ? "#DCFCE7" : (TrangThai == "HetGio" || TrangThai == "DaHuy" ? "#FEE2E2" : "#FEF3C7");
        public string TrangThaiFg => TrangThai == "DaNop" ? "#15803D" : (TrangThai == "HetGio" || TrangThai == "DaHuy" ? "#B91C1C" : "#B45309");

        public string ScoreColor => DiemSo >= 8.0m ? "#10B981" : (DiemSo >= 6.5m ? "#2563EB" : (DiemSo >= 5.0m ? "#D97706" : "#DC2626"));
        public string ThoiDiemBatDauHienThi => ThoiDiemBatDau.ToString("dd/MM/yyyy HH:mm");

        public string Initials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(HoTenNguoiDung))
                    return string.IsNullOrWhiteSpace(TenDangNhap) ? "HV" : TenDangNhap.Substring(0, Math.Min(2, TenDangNhap.Length)).ToUpper();
                var parts = HoTenNguoiDung.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
                return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
            }
        }
    }
}
