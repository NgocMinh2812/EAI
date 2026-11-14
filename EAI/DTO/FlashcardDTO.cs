using System;

namespace DTO
{
    public class FlashcardDTO
    {
        public int MaFlashcard { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaTuVung { get; set; }
        public string Tu { get; set; } = string.Empty;
        public string LoaiTu { get; set; } = string.Empty;
        public string PhienAmIPA { get; set; } = string.Empty;
        public string Nghia { get; set; } = string.Empty;
        public string CauViDu { get; set; } = string.Empty;
        public string? DuongDanAudio { get; set; }
        public string CumTuDiKem { get; set; } = string.Empty;
        public string MaCapDo { get; set; } = "B2";
        public string TenChuDe { get; set; } = "Học thuật";

        public int SoLanOnTap { get; set; }
        public int SoLanDung { get; set; }
        public DateTime? LanOnTapCuoi { get; set; }
        public DateTime? NgayOnTapTiepTheo { get; set; }

        public string IpaFormatted => string.IsNullOrWhiteSpace(LoaiTu) ? PhienAmIPA : $"{PhienAmIPA} ({LoaiTu})";
        public string TopicBadge => $"{TenChuDe.ToUpper()} • {MaCapDo}";
        public bool DaThuoc => SoLanDung >= 3;
        public int LeitnerBox => Math.Clamp(SoLanDung + 1, 1, 5);
        public bool IsDueForReview => NgayOnTapTiepTheo == null || NgayOnTapTiepTheo <= DateTime.Now;

        public string DaThuocBadgeText => DaThuoc ? "✓ Đã thuộc" : "Cần ôn tập";
        public string DaThuocBadgeBg => DaThuoc ? "#DCFCE7" : "#FEF3C7";
        public string DaThuocBadgeFg => DaThuoc ? "#15803D" : "#B45309";
    }
}
