using System;
using System.Collections.Generic;

namespace DTO
{
    /// <summary>
    /// Bộ lọc thời gian cho Bảng Tiến độ học tập
    /// </summary>
    public class TienDoHocFilterDTO
    {
        public string ThoiGian { get; set; } = "TatCa"; // "TuanNay", "ThangNay", "TatCa"
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }

    /// <summary>
    /// Tổng kết tiến độ học tập và các chỉ số KPI
    /// </summary>
    public class TongKetTienDoDTO
    {
        // 1. KPI cơ bản
        public int TongSoPhutHoc { get; set; }          // Tổng số phút học tích lũy
        public int TongSoBaiThi { get; set; }           // Tổng số bài thi đã hoàn thành
        public double DiemTrungBinh { get; set; }       // Điểm trung bình các lần thi
        public int SoBaiDat { get; set; }               // Số bài đạt (DiemSo >= DiemDat)
        public int SoBaiChuaDat { get; set; }           // Số bài chưa đạt
        public double TyLeDat { get; set; }             // Tỷ lệ đạt (%)
        public double TyLeChuaDat { get; set; }         // Tỷ lệ chưa đạt (%)

        // 2. Mục tiêu học tập
        public decimal? DiemMucTieu { get; set; }
        public string? KhungNangLucMucTieu { get; set; } // IELTS / TOEIC / CEFR
        public string? TrinhDoMucTieu { get; set; }
        public string? TrinhDoDauVao { get; set; }
        public double PhanTramMucTieu { get; set; }      // % Tiến độ hoàn thành mục tiêu học tập (0 - 100%)
        public string MoTaTienDoMucTieu { get; set; } = string.Empty;

        // 3. Chuỗi học tập
        public int ChuoiNgayHoc { get; set; } = 1;

        // Display Helpers
        public string TongGioHocText => TongSoPhutHoc >= 60 
            ? $"{(TongSoPhutHoc / 60)}h {(TongSoPhutHoc % 60):D2}m" 
            : $"{TongSoPhutHoc} phút";

        public string DiemTrungBinhText => TongSoBaiThi > 0 ? $"{DiemTrungBinh:0.0} / 10" : "-- / 10";
        public string TyLeDatText => TongSoBaiThi > 0 ? $"{TyLeDat:0.0}%" : "--%";
        
        public string MucTieuText
        {
            get
            {
                if (!string.IsNullOrEmpty(KhungNangLucMucTieu) && DiemMucTieu.HasValue)
                    return $"{KhungNangLucMucTieu} {DiemMucTieu:F1}";
                if (!string.IsNullOrEmpty(TrinhDoMucTieu))
                    return $"CEFR {TrinhDoMucTieu}";
                return "Chưa đặt";
            }
        }

        public string PhanTramMucTieuText => $"{PhanTramMucTieu:0}%";
        public bool CoDuLieu => TongSoBaiThi > 0 || TongSoPhutHoc > 0;
    }

    /// <summary>
    /// Bản ghi điểm số theo thời gian từng lần thi
    /// </summary>
    public class DiemThiTheoThoiGianDTO
    {
        public int MaLuotLamBai { get; set; }
        public int MaBaiTapDeThi { get; set; }
        public string TenDeThi { get; set; } = string.Empty;
        public decimal DiemSo { get; set; }
        public decimal DiemDat { get; set; } = 5.0m;
        public decimal TongDiem { get; set; } = 10.0m;
        public DateTime ThoiDiemNopBai { get; set; }
        public string HinhThucLam { get; set; } = "ThiThu";
        public int ThoiGianLamGiay { get; set; }
        public int SoCauDung { get; set; }
        public int TongSoCau { get; set; }
        public bool LaDat => DiemSo >= DiemDat;

        public int ThuTuLanThi { get; set; } // 1, 2, 3...
        public string LanThiLabel => $"Lần #{ThuTuLanThi}";
        public string NgayNopText => ThoiDiemNopBai.ToString("dd/MM/yyyy HH:mm");
        public string NgayNopNgan => ThoiDiemNopBai.ToString("dd/MM");
        public string DiemHienThi => $"{DiemSo:0.0} / {TongDiem:0.0}";
        public string ThoiGianHienThi => $"{ThoiGianLamGiay / 60:D2}:{ThoiGianLamGiay % 60:D2}";
        public string SoCauDungText => $"{SoCauDung}/{TongSoCau}";
        public string TrangThaiText => LaDat ? "ĐẠT" : "CHƯA ĐẠT";
        public string TrangThaiBg => LaDat ? "#DCFCE7" : "#FEE2E2";
        public string TrangThaiFg => LaDat ? "#166534" : "#991B1B";
    }

    /// <summary>
    /// Thống kê giờ học phân bổ theo ngày
    /// </summary>
    public class GioHocTheoNgayDTO
    {
        public DateTime Ngay { get; set; }
        public string NhanNgay { get; set; } = string.Empty; // "T2 (04/11)", "T3"...
        public int SoPhutHoc { get; set; }
        public double SoGioHoc => Math.Round(SoPhutHoc / 60.0, 1);
        public string GioHocText => SoPhutHoc >= 60 ? $"{SoPhutHoc / 60}h {SoPhutHoc % 60}m" : $"{SoPhutHoc}m";
    }
}

