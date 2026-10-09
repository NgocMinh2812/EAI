using System;
using System.Collections.Generic;

namespace DTO
{
    public class ChuDeDTO
    {
        public int MaChuDe { get; set; }
        public int? MaChuDeCha { get; set; }
        public string TenChuDe { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public bool TrangThaiHoatDong { get; set; } = true;

        // Bổ sung thống kê
        public string TenChuDeCha { get; set; } = string.Empty;
        public int SoLuongBaiHoc { get; set; } = 0;
        public List<ChuDeDTO> DanhSachCon { get; set; } = new List<ChuDeDTO>();
    }
}
