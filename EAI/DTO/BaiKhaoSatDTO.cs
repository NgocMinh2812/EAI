using System;

namespace DTO
{
    public class BaiKhaoSatDTO
    {
        public int MaBaiKhaoSat { get; set; }
        public string TenBaiKhaoSat { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public int TongSoCauHoi { get; set; }
        public bool DangHoatDong { get; set; }
        public DateTime NgayTao { get; set; }
        public string MaCapDo { get; set; } = "B1";
        public string TenCapDo { get; set; } = string.Empty;
        public int ThoiGianPhut { get; set; } = 30;

        public string BadgeBackground
        {
            get => MaCapDo switch
            {
                "A1" => "#DCFCE7",
                "A2" => "#CCFBF1",
                "B1" => "#DBEAFE",
                "B2" => "#E0E7FF",
                "C1" => "#F3E8FF",
                "C2" => "#FEF3C7",
                "TongHop" => "#F0FDFA",
                _ => "#F1F5F9"
            };
        }

        public string BadgeForeground
        {
            get => MaCapDo switch
            {
                "A1" => "#15803D",
                "A2" => "#0F766E",
                "B1" => "#1D4ED8",
                "B2" => "#4338CA",
                "C1" => "#7E22CE",
                "C2" => "#B45309",
                "TongHop" => "#0F766E",
                _ => "#475569"
            };
        }

        public string StatusBackground => DangHoatDong ? "#ECFDF5" : "#F1F5F9";
        public string StatusColor => DangHoatDong ? "#065F46" : "#64748B";
        public string StatusText => DangHoatDong ? "Đang kích hoạt" : "Sẵn sàng";
        public string ThoiGianText => $"{ThoiGianPhut} phút";
        public string TongSoCauText => $"{TongSoCauHoi} câu hỏi";
    }
}

