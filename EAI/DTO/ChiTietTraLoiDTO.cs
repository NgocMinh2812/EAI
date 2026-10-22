using System;
using System.Collections.Generic;

namespace DTO
{
    public class ChiTietTraLoiDTO
    {
        public int MaChiTietTraLoi { get; set; }
        public int MaLuotLamBai { get; set; }
        public int MaCauHoi { get; set; }
        public int ThuTuCauHoi { get; set; }
        public string NoiDungCauHoi { get; set; } = string.Empty;
        public string? DoanVanDoc { get; set; }
        public string KyNang { get; set; } = "Doc";
        public string MaTrinhDo { get; set; } = "B2";

        public int? MaLuaChonDaChon { get; set; }
        public string KyHieuDaChon { get; set; } = string.Empty;
        public string NoiDungDaChon { get; set; } = string.Empty;
        public string? CauTraLoiVanBan { get; set; }

        public int MaLuaChonDung { get; set; }
        public string KyHieuDapAnDung { get; set; } = string.Empty;
        public string NoiDungDapAnDung { get; set; } = string.Empty;

        public bool? KetQuaDungSai { get; set; }
        public decimal DiemSo { get; set; }
        public decimal DiemDat { get => DiemSo; set => DiemSo = value; }
        public decimal DiemToiDa { get; set; } = 1.0m;
        public string? GiaiThichDapAn { get; set; }

        public List<LuaChonDapAnDTO> DanhSachLuaChon { get; set; } = new List<LuaChonDapAnDTO>();

        // Display Helpers
        public bool IsAnswered => (MaLuaChonDaChon.HasValue && MaLuaChonDaChon.Value > 0) || !string.IsNullOrWhiteSpace(CauTraLoiVanBan);
        public bool IsCorrect => KetQuaDungSai.HasValue && KetQuaDungSai.Value;
    }
}
