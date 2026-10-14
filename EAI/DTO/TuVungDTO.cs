using System;

namespace DTO
{
    public class TuVungDTO
    {
        public int MaTuVung { get; set; }
        public int? MaChuDe { get; set; }
        public int? MaTrinhDo { get; set; }
        public string TenTrinhDo { get; set; } = string.Empty;
        public string MaCapDo { get; set; } = "B2";
        public string Tu { get; set; } = string.Empty;
        public string LoaiTu { get; set; } = string.Empty;
        public string PhienAmIPA { get; set; } = string.Empty;
        public string Nghia { get; set; } = string.Empty;
        public string CauViDu { get; set; } = string.Empty;
        public string? DuongDanAudio { get; set; }
        public string CumTuDiKem { get; set; } = string.Empty;

        // Display Helpers
        public string IpaFormatted => string.IsNullOrWhiteSpace(LoaiTu) 
            ? PhienAmIPA 
            : $"{PhienAmIPA} ({LoaiTu})";

        public string TopicBadge => $"VOCABULARY • {MaCapDo}";
    }
}
