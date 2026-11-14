using System;

namespace DTO.Assistant
{
    /// <summary>
    /// DTO đại diện cho một phiên/cuộc hội thoại giữa người dùng và Trợ lý AI EAI.
    /// </summary>
    public class AIAssistantConversationDTO
    {
        public int MaHoiThoai { get; set; }
        public int MaNguoiDung { get; set; }
        public string TieuDe { get; set; } = "Cuộc hội thoại mới";
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;
        public string TrangThai { get; set; } = "Active"; // Active | Archived | Deleted
        public int TongSoTinNhan { get; set; } = 0;
        public string? MoHinhUuTien { get; set; }
        public string? PromptHeThongTuyChon { get; set; }

        // Alias & Convenience Helpers cho Bus / GUI Interoperability
        public int ConversationId
        {
            get => MaHoiThoai;
            set => MaHoiThoai = value;
        }

        public int UserId
        {
            get => MaNguoiDung;
            set => MaNguoiDung = value;
        }

        public string Title
        {
            get => TieuDe;
            set => TieuDe = value;
        }

        public string FormattedTime => NgayCapNhat.ToString("HH:mm dd/MM");
        public bool IsActive => string.Equals(TrangThai, "Active", StringComparison.OrdinalIgnoreCase);
        public bool IsArchived => string.Equals(TrangThai, "Archived", StringComparison.OrdinalIgnoreCase);
        public bool IsDeleted => string.Equals(TrangThai, "Deleted", StringComparison.OrdinalIgnoreCase);
    }
}
