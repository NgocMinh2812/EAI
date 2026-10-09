using System;

namespace DTO
{
    public class TrinhDoDTO
    {
        public int MaTrinhDo { get; set; }
        public string MaCapDo { get; set; } = string.Empty; // A1, A2, B1, B2, C1, C2
        public string TenTrinhDo { get; set; } = string.Empty;
        public string? MoTa { get; set; }
    }
}
