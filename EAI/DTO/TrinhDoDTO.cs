using System;

namespace DTO
{
    public class TrinhDoDTO
    {
        public int MaTrinhDo { get; set; }
        public string MaCapDo { get; set; } = string.Empty; // A1, A2, B1, B2, C1, C2
        public string TenTrinhDo { get; set; } = string.Empty;
        public string? MoTa { get; set; }

        public string TenHienThi => !string.IsNullOrWhiteSpace(TenTrinhDo) ? $"{MaCapDo} — {TenTrinhDo}" : MaCapDo;

        // Bổ sung thống kê phục vụ hiển thị US-GV04
        public int SoLuongBaiHoc { get; set; } = 0;
        public int SoLuongCauHoi { get; set; } = 0;

        private string CleanCapDo => MaCapDo?.Trim().ToUpper() ?? "";

        public string IeltsTuongDuong => CleanCapDo switch
        {
            "A0" or "PRE-A1" => "< 1.0",
            "A1" => "1.0 – 2.5",
            "A2" => "3.0 – 3.5",
            "B1" => "4.0 – 5.0",
            "B1+" => "5.0 – 5.5",
            "B2" => "5.5 – 6.5",
            "B2+" => "6.5 – 7.0",
            "C1" => "7.0 – 8.0",
            "C2" => "8.5 – 9.0",
            _ => "—"
        };

        public string ToeicTuongDuong => CleanCapDo switch
        {
            "A0" or "PRE-A1" => "< 120",
            "A1" => "120 – 220",
            "A2" => "225 – 545",
            "B1" => "550 – 780",
            "B1+" => "650 – 780",
            "B2" => "785 – 940",
            "B2+" => "850 – 940",
            "C1" => "945 – 990",
            "C2" => "990 Max",
            _ => "—"
        };

        public string BadgeBg => CleanCapDo switch
        {
            "A0" or "PRE-A1" => "#F1F5F9",
            "A1" => "#DCFCE7",
            "A2" => "#DBEAFE",
            "B1" => "#FEF3C7",
            "B1+" => "#FEF9C3",
            "B2" => "#F3E8FF",
            "B2+" => "#EDE9FE",
            "C1" => "#FFE4E6",
            "C2" => "#E0E7FF",
            _ => "#F1F5F9"
        };

        public string BadgeFg => CleanCapDo switch
        {
            "A0" or "PRE-A1" => "#475569",
            "A1" => "#15803D",
            "A2" => "#1E40AF",
            "B1" => "#B45309",
            "B1+" => "#A16207",
            "B2" => "#7E22CE",
            "B2+" => "#6D28D9",
            "C1" => "#BE123C",
            "C2" => "#3730A3",
            _ => "#475569"
        };

        public string BorderAccent => CleanCapDo switch
        {
            "A0" or "PRE-A1" => "#CBD5E1",
            "A1" => "#86EFAC",
            "A2" => "#93C5FD",
            "B1" => "#FCD34D",
            "B1+" => "#FDE047",
            "B2" => "#D8B4FE",
            "B2+" => "#C4B5FD",
            "C1" => "#FDA4AF",
            "C2" => "#A5B4FC",
            _ => "#CBD5E1"
        };
    }
}
