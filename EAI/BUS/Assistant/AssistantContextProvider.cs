using System;
using System.Collections.Generic;
using BUS;

namespace BUS.Assistant
{
    /// <summary>
    /// Nhà cung cấp ngữ cảnh thời gian thực (Context Provider) cho Trợ lý AI EAI.
    /// Tự động trích xuất thông tin người dùng từ SessionHelper và phát hiện màn hình làm việc hiện tại.
    /// Thiết kế an toàn: Hoạt động trơn tru cả trong môi trường Headless Unit Test lẫn GUI WPF.
    /// </summary>
    public class AssistantContextProvider
    {
        private static readonly Lazy<AssistantContextProvider> _instance = new(() => new AssistantContextProvider());
        public static AssistantContextProvider Instance => _instance.Value;

        /// <summary>
        /// Hook tùy chọn để tầng GUI đăng ký bộ dò màn hình WPF chuyên sâu mà không làm phụ thuộc tầng BUS vào WPF.
        /// </summary>
        public static Func<(string PageCode, string PageTitle, int? TargetId, string? TargetType)>? CustomPageDetector { get; set; }

        public AssistantContextProvider() { }

        /// <summary>
        /// Trích xuất ngữ cảnh hiện tại của phiên làm việc.
        /// </summary>
        public AssistantContext GetCurrentContext()
        {
            var user = SessionHelper.CurrentUser;
            int userId = SessionHelper.CurrentMaNguoiDung;
            int guestOrUserId = userId > 0 ? userId : 0;
            string username = user?.TenDangNhap ?? (userId > 0 ? $"user_{userId}" : "khach");
            string fullName = user?.HoTen ?? (userId > 0 ? username : "Khách vãng lai");
            string userRole = SessionHelper.CurrentRole;

            var (pageCode, pageTitle, targetId, targetType) = DetectActivePage();

            return new AssistantContext
            {
                UserId = guestOrUserId,
                Username = username,
                FullName = fullName,
                UserRole = userRole,
                CurrentPage = pageCode,
                CurrentPageTitle = pageTitle,
                TargetId = targetId,
                TargetType = targetType
            };
        }

        /// <summary>
        /// Dò tìm màn hình đang hiển thị. Ưu tiên CustomPageDetector nếu được GUI cấu hình.
        /// </summary>
        private (string PageCode, string PageTitle, int? TargetId, string? TargetType) DetectActivePage()
        {
            try
            {
                if (CustomPageDetector != null)
                {
                    return CustomPageDetector();
                }
            }
            catch
            {
                // Bỏ qua lỗi truy cập UI nếu chạy ngoài luồng STA
            }

            // Fallback dựa trên vai trò của người dùng
            string role = AssistantPermissionService.NormalizeRole(SessionHelper.CurrentRole);
            if (role == AssistantPermissionService.RoleAdmin || role == AssistantPermissionService.RoleLecturer)
            {
                return ("TongQuan", "Tổng quan giảng viên & Quản trị", null, null);
            }
            else if (role == AssistantPermissionService.RoleStudent)
            {
                return ("TrangChu", "Trang chủ học tập", null, null);
            }

            return ("General", "Màn hình chung", null, null);
        }

        /// <summary>
        /// Tạo đoạn tóm tắt ngữ cảnh bằng tiếng Việt chuẩn để nhúng vào Prompt hệ thống của AI.
        /// </summary>
        public string BuildSystemPromptContext(AssistantContext context)
        {
            if (context == null) context = GetCurrentContext();

            var lines = new List<string>
            {
                $"[NGỮ CẢNH PHIÊN LÀM VIỆC HIỆN TẠI]",
                $"- Người dùng: {context.FullName} (@{context.Username}, ID: {context.UserId})",
                $"- Vai trò hệ thống: {context.UserRole}",
                $"- Màn hình/Trang đang mở: {context.CurrentPageTitle} (Mã: {context.CurrentPage})"
            };

            if (context.TargetId.HasValue && !string.IsNullOrWhiteSpace(context.TargetType))
            {
                lines.Add($"- Mục tiêu đang chọn: {context.TargetType} #{context.TargetId.Value}");
            }

            if (context.AdditionalContext != null && context.AdditionalContext.Count > 0)
            {
                lines.Add("- Dữ liệu ngữ cảnh bổ sung:");
                foreach (var kvp in context.AdditionalContext)
                {
                    lines.Add($"  + {kvp.Key}: {kvp.Value}");
                }
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}
