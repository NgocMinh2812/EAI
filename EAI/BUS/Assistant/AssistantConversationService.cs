using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Assistant;
using DTO.Assistant;

namespace BUS.Assistant
{
    /// <summary>
    /// Service nghiệp vụ quản lý vòng đời cuộc hội thoại và lịch sử tin nhắn của Trợ lý AI EAI.
    /// Đáp ứng hợp đồng chuẩn AssistantConversationService theo PROJECT.md.
    /// </summary>
    public class AssistantConversationService
    {
        private readonly AIAssistantDAL _dal;

        private static AssistantConversationService? _instance;
        public static AssistantConversationService Instance => _instance ??= new AssistantConversationService();

        public AssistantConversationService(AIAssistantDAL? dal = null)
        {
            _dal = dal ?? AIAssistantDAL.Instance;
        }

        #region Asynchronous API (Interface Contract)

        /// <summary>
        /// Tạo mới một cuộc hội thoại cho người dùng.
        /// </summary>
        public async Task<AIAssistantConversationDTO> CreateConversationAsync(int userId, string? title = null)
        {
            return await Task.Run(() => CreateConversation(userId, title));
        }

        /// <summary>
        /// Lấy danh sách cuộc hội thoại của người dùng, hỗ trợ tìm kiếm theo từ khóa.
        /// </summary>
        public async Task<List<AIAssistantConversationDTO>> GetConversationsAsync(int userId, string? keyword = null)
        {
            return await Task.Run(() => GetConversations(userId, keyword));
        }

        /// <summary>
        /// Đổi tên cuộc hội thoại.
        /// </summary>
        public async Task<bool> RenameConversationAsync(int conversationId, int userId, string newTitle)
        {
            return await Task.Run(() => RenameConversation(conversationId, userId, newTitle));
        }

        /// <summary>
        /// Xóa cuộc hội thoại (xóa mềm).
        /// </summary>
        public async Task<bool> DeleteConversationAsync(int conversationId, int userId)
        {
            return await Task.Run(() => DeleteConversation(conversationId, userId));
        }

        /// <summary>
        /// Lấy danh sách tin nhắn của một cuộc hội thoại.
        /// </summary>
        public async Task<List<AIAssistantMessageDTO>> GetMessagesAsync(int conversationId)
        {
            return await Task.Run(() => GetMessages(conversationId, 0));
        }

        /// <summary>
        /// Lấy danh sách tin nhắn của một cuộc hội thoại theo mã người dùng rõ ràng.
        /// </summary>
        public async Task<List<AIAssistantMessageDTO>> GetMessagesAsync(int conversationId, int userId)
        {
            return await Task.Run(() => GetMessages(conversationId, userId));
        }

        /// <summary>
        /// Thêm tin nhắn mới vào cuộc hội thoại.
        /// </summary>
        public async Task<AIAssistantMessageDTO> AddMessageAsync(
            int conversationId, 
            string role, 
            string content, 
            string? toolCallsJson = null, 
            string? toolResultJson = null,
            string? cardPayloadJson = null,
            string status = "Sent")
        {
            return await Task.Run(() => AddMessage(conversationId, role, content, toolCallsJson, toolResultJson, cardPayloadJson, status));
        }

        /// <summary>
        /// Lấy chi tiết cuộc hội thoại theo ID.
        /// </summary>
        public async Task<AIAssistantConversationDTO?> GetConversationByIdAsync(int conversationId, int userId = 0)
        {
            return await Task.Run(() => GetConversationById(conversationId, userId));
        }

        /// <summary>
        /// Xóa toàn bộ tin nhắn trong cuộc hội thoại (clear history).
        /// </summary>
        public async Task<bool> ClearMessagesAsync(int conversationId, int userId = 0)
        {
            return await Task.Run(() => ClearMessages(conversationId, userId));
        }

        #endregion

        #region Synchronous Business Methods

        public AIAssistantConversationDTO CreateConversation(int userId, string? title = null, string? activeModel = null, string? customSystemPrompt = null)
        {
            int resolvedUserId = userId > 0 ? userId : SessionHelper.CurrentMaNguoiDung;
            if (resolvedUserId <= 0)
            {
                throw new ArgumentException("Mã người dùng không hợp lệ hoặc người dùng chưa đăng nhập.", nameof(userId));
            }

            string finalTitle = string.IsNullOrWhiteSpace(title) 
                ? "Cuộc trò chuyện mới" 
                : NormalizeTitle(title);

            var conv = new AIAssistantConversationDTO
            {
                MaNguoiDung = resolvedUserId,
                TieuDe = finalTitle,
                TrangThai = "Active",
                TongSoTinNhan = 0,
                MoHinhUuTien = activeModel,
                PromptHeThongTuyChon = customSystemPrompt,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };

            int newId = _dal.TaoHoiThoai(conv);
            if (newId <= 0)
            {
                throw new InvalidOperationException("Không thể tạo cuộc hội thoại mới trong CSDL.");
            }

            conv.MaHoiThoai = newId;
            return conv;
        }

        public List<AIAssistantConversationDTO> GetConversations(int userId, string? keyword = null, int top = 50)
        {
            int resolvedUserId = userId > 0 ? userId : SessionHelper.CurrentMaNguoiDung;
            if (resolvedUserId <= 0)
            {
                return new List<AIAssistantConversationDTO>();
            }

            string? safeKeyword = string.IsNullOrWhiteSpace(keyword) 
                ? null 
                : (keyword.Trim().Length > 200 ? keyword.Trim().Substring(0, 200) : keyword.Trim());

            return _dal.LayDanhSachHoiThoai(resolvedUserId, safeKeyword, top);
        }

        public AIAssistantConversationDTO? GetConversationById(int conversationId, int userId = 0)
        {
            if (conversationId <= 0) return null;
            int resolvedUserId = userId > 0 ? userId : SessionHelper.CurrentMaNguoiDung;
            if (resolvedUserId <= 0) return null;
            return _dal.LayHoiThoaiTheoId(conversationId, resolvedUserId);
        }

        public bool RenameConversation(int conversationId, int userId, string newTitle)
        {
            if (conversationId <= 0) return false;
            if (string.IsNullOrWhiteSpace(newTitle)) return false;

            int resolvedUserId = userId > 0 ? userId : SessionHelper.CurrentMaNguoiDung;
            if (resolvedUserId <= 0) return false;
            string normalized = NormalizeTitle(newTitle);
            return _dal.CapNhatTieuDeHoiThoai(conversationId, resolvedUserId, normalized);
        }

        public bool DeleteConversation(int conversationId, int userId, bool hardDelete = false)
        {
            if (conversationId <= 0) return false;
            int resolvedUserId = userId > 0 ? userId : SessionHelper.CurrentMaNguoiDung;
            if (resolvedUserId <= 0) return false;
            return _dal.XoaHoiThoai(conversationId, resolvedUserId, hardDelete);
        }

        public List<AIAssistantMessageDTO> GetMessages(int conversationId, int userId = 0)
        {
            if (conversationId <= 0) return new List<AIAssistantMessageDTO>();
            int resolvedUserId = userId > 0 ? userId : SessionHelper.CurrentMaNguoiDung;
            if (resolvedUserId <= 0) return new List<AIAssistantMessageDTO>();
            return _dal.LayDanhSachTinNhan(conversationId, resolvedUserId);
        }

        public AIAssistantMessageDTO AddMessage(
            int conversationId,
            string role,
            string content,
            string? toolCallsJson = null,
            string? toolResultJson = null,
            string? cardPayloadJson = null,
            string status = "Sent")
        {
            if (conversationId <= 0)
            {
                throw new ArgumentException("Mã cuộc hội thoại không hợp lệ.", nameof(conversationId));
            }

            string validRole = NormalizeRole(role);

            var msg = new AIAssistantMessageDTO
            {
                MaHoiThoai = conversationId,
                VaiTro = validRole,
                NoiDung = content ?? string.Empty,
                ToolCallsJson = toolCallsJson,
                ToolResultJson = toolResultJson,
                CardPayloadJson = cardPayloadJson,
                TrangThai = string.IsNullOrWhiteSpace(status) ? "Sent" : status.Trim(),
                ThoiGianGui = DateTime.Now
            };

            long newId = _dal.ThemTinNhan(msg);
            if (newId <= 0)
            {
                throw new InvalidOperationException($"Không thể lưu tin nhắn vào cuộc hội thoại {conversationId}.");
            }

            msg.MaTinNhan = newId;

            // Nếu đây là tin nhắn đầu tiên của user và tiêu đề đang là tiêu đề mặc định, tự động cập nhật tiêu đề theo nội dung tin nhắn
            if (string.Equals(validRole, "user", StringComparison.OrdinalIgnoreCase))
            {
                TryAutoUpdateTitleFromMessage(conversationId, content);
            }

            return msg;
        }

        public bool ClearMessages(int conversationId, int userId = 0)
        {
            if (conversationId <= 0) return false;
            int resolvedUserId = userId > 0 ? userId : SessionHelper.CurrentMaNguoiDung;
            if (resolvedUserId <= 0) return false;
            return _dal.XoaTatCaTinNhan(conversationId, resolvedUserId);
        }

        #endregion

        #region Title Auto-generation & Helpers

        private void TryAutoUpdateTitleFromMessage(int conversationId, string? userMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userMessage)) return;

                string newTitle = GenerateTitleFromPrompt(userMessage);
                _dal.CapNhatTieuDeTuDongLanDau(conversationId, newTitle);
            }
            catch
            {
                // Không chặn luồng gửi tin nếu auto-update tiêu đề gặp trục trặc
            }
        }

        public static string GenerateTitleFromPrompt(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt)) return "Cuộc trò chuyện mới";

            string cleaned = prompt.Trim().Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
            while (cleaned.Contains("  "))
            {
                cleaned = cleaned.Replace("  ", " ");
            }

            if (cleaned.Length > 45)
            {
                cleaned = cleaned.Substring(0, 42).Trim() + "...";
            }

            return cleaned;
        }

        private static string NormalizeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "Cuộc trò chuyện mới";
            string t = title.Trim();
            if (t.Length > 250)
            {
                t = t.Substring(0, 247).Trim() + "...";
            }
            return t;
        }

        private static string NormalizeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) return "user";
            string r = role.Trim().ToLowerInvariant();
            return r switch
            {
                "user" => "user",
                "assistant" => "assistant",
                "system" => "system",
                "tool" => "tool",
                _ => "user"
            };
        }

        #endregion
    }
}
