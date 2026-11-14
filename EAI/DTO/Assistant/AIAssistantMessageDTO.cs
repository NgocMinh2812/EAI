using System;

namespace DTO.Assistant
{
    /// <summary>
    /// DTO đại diện cho một tin nhắn trong cuộc hội thoại Trợ lý AI EAI.
    /// Hỗ trợ cả tin nhắn người dùng, phản hồi trợ lý, tool calls, tool results và card payload tương tác.
    /// </summary>
    public class AIAssistantMessageDTO
    {
        public long MaTinNhan { get; set; }
        public int MaHoiThoai { get; set; }
        public string VaiTro { get; set; } = "user"; // user | assistant | system | tool
        public string NoiDung { get; set; } = string.Empty;
        public DateTime ThoiGianGui { get; set; } = DateTime.Now;
        public string? ToolCallsJson { get; set; }
        public string? ToolResultJson { get; set; }
        public string? CardPayloadJson { get; set; }
        public string TrangThai { get; set; } = "Sent"; // Sent | Pending | Error

        // Alias & Convenience Helpers cho Bus / GUI Interoperability
        public long MessageId
        {
            get => MaTinNhan;
            set => MaTinNhan = value;
        }

        public int ConversationId
        {
            get => MaHoiThoai;
            set => MaHoiThoai = value;
        }

        public string Role
        {
            get => VaiTro;
            set => VaiTro = value;
        }

        public string Content
        {
            get => NoiDung;
            set => NoiDung = value;
        }

        public DateTime Timestamp
        {
            get => ThoiGianGui;
            set => ThoiGianGui = value;
        }

        public bool IsUser => string.Equals(VaiTro, "user", StringComparison.OrdinalIgnoreCase);
        public bool IsAssistant => string.Equals(VaiTro, "assistant", StringComparison.OrdinalIgnoreCase);
        public bool IsSystem => string.Equals(VaiTro, "system", StringComparison.OrdinalIgnoreCase);
        public bool IsTool => string.Equals(VaiTro, "tool", StringComparison.OrdinalIgnoreCase);
        public bool HasToolCalls => !string.IsNullOrWhiteSpace(ToolCallsJson);
        public bool HasToolResult => !string.IsNullOrWhiteSpace(ToolResultJson);
        public bool HasCard => !string.IsNullOrWhiteSpace(CardPayloadJson);
        public string FormattedTime => ThoiGianGui.ToString("HH:mm");
    }
}
