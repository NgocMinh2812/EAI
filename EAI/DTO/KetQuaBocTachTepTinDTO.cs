using System;
using System.Collections.Generic;

namespace DTO
{
    public class KetQuaBocTachTepTinDTO
    {
        public string TenTep { get; set; } = string.Empty;
        public string DinhDang { get; set; } = string.Empty;
        public string DinhDangTep
        {
            get => DinhDang;
            set => DinhDang = value;
        }
        public long KichThuocByte { get; set; }
        public int TongSoCauDocDuoc { get; set; }
        public int SoCauHopLe { get; set; }
        public List<CauHoiDTO> DanhSachCauHoi { get; set; } = new();
        public List<string> DanhSachLoi { get; set; } = new();
        public bool CoLoi => DanhSachLoi.Count > 0;
    }
}
