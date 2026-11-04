using System;

namespace DTO
{
    public class PhienHoiThoaiDTO
    {
        public long MaPhienHoiThoai { get; set; }
        public int MaNguoiDung { get; set; }
        public string TieuDe { get; set; } = "Cuộc hội thoại mới";
        public string NguCanhHoiThoai { get; set; } = "General"; // Grammar, Vocab, Explain, Speaking, General
        public int? MaCauHoiLienQuan { get; set; }
        public int? MaBaiHocLienQuan { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? TinNhanCuoiVao { get; set; }
        public int SoTinNhan { get; set; }
    }
}

