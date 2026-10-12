using System;

namespace DTO
{
    public class HoSoHocTapDTO
    {
        public int MaHoSo { get; set; }
        public int MaNguoiDung { get; set; }
        public decimal? DiemMucTieu { get; set; }
        public string? KhungNangLucMucTieu { get; set; } // IELTS / TOEIC / CEFR...
        public string? TrinhDoMucTieu { get; set; }     // A1-C1...
        public string? TrinhDoDauVao { get; set; }      // A1, A2, B1, B2, C1... (Trình độ đánh giá / khảo sát đầu vào)
        public decimal? DiemDanhGiaDauVao { get; set; }
        public int SoPhutHocTichLuy { get; set; }
        public string? GhiChuCaNhan { get; set; }
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public string HienThiTrinhDoDauVao => !string.IsNullOrEmpty(TrinhDoDauVao) ? $"CEFR {TrinhDoDauVao}" : "Chưa đánh giá";

        public string HienThiMucTieu
        {
            get
            {
                if (!string.IsNullOrEmpty(KhungNangLucMucTieu) && DiemMucTieu.HasValue)
                {
                    return $"{KhungNangLucMucTieu} Target {DiemMucTieu:F1}";
                }
                if (!string.IsNullOrEmpty(TrinhDoMucTieu))
                {
                    return $"CEFR {TrinhDoMucTieu}";
                }
                return "Chưa đặt mục tiêu";
            }
        }
    }
}
