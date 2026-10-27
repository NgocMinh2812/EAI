using System;
using System.Collections.Generic;

namespace DTO
{
    public class SinhVienKhaoSatResultDTO
    {
        public int MaLuotKhaoSat { get; set; }
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ThoiDiemBatDau { get; set; }
        public DateTime? ThoiDiemNopBai { get; set; }
        public int TongSoCau { get; set; }
        public int SoCauDung { get; set; }
        public decimal DiemDatDuoc { get; set; }
        public string MaCapDo { get; set; } = "B1";
        public string TenTrinhDo { get; set; } = string.Empty;
        public string KhungDanhGiaKetQua { get; set; } = "CEFR";
        public string QuyDoiIelts { get; set; } = string.Empty;
        public string QuyDoiToeic { get; set; } = string.Empty;

        public string TyLeDungText => TongSoCau > 0 ? $"{(SoCauDung * 100.0 / TongSoCau):0}%" : "0%";
        public string KetQuaTiLe => $"{SoCauDung}/{TongSoCau} ({TyLeDungText})";
        public string DiemHienThi => $"{DiemDatDuoc:0.0} / 10";
        public string NgayNopFormatted => ThoiDiemNopBai.HasValue ? ThoiDiemNopBai.Value.ToString("dd/MM/yyyy HH:mm") : ThoiDiemBatDau.ToString("dd/MM/yyyy HH:mm");
        public string ThoiDiemNopBaiHienThi => NgayNopFormatted;
        public string Initials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(HoTen)) return "SV";
                var parts = HoTen.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
                return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
            }
        }

        public List<ChiTietTraLoiKhaoSatItemDTO> DanhSachChiTiet { get; set; } = new();
    }

    public class ChiTietTraLoiKhaoSatItemDTO
    {
        public int MaCauTraLoiKhaoSat { get; set; }
        public int MaCauHoi { get; set; }
        public string NoiDungCauHoi { get; set; } = string.Empty;
        public string? DoanVanDoc { get; set; }
        public string KyNang { get; set; } = "Doc";
        public string MaTrinhDo { get; set; } = "B1";
        public int? MaLuaChonDaChon { get; set; }
        public string KyHieuDaChon { get; set; } = string.Empty;
        public string NoiDungDaChon { get; set; } = string.Empty;
        public bool KetQuaDungSai { get; set; }
        public string GiaiThichDapAn { get; set; } = string.Empty;
        public List<LuaChonDapAnDTO> DanhSachLuaChon { get; set; } = new();
    }
}
