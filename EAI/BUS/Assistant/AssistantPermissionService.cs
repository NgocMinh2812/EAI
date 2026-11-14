using System;

namespace BUS.Assistant
{
    /// <summary>
    /// Service phân quyền tập trung (RBAC) cho Trợ lý AI EAI.
    /// Đảm bảo người dùng chỉ được thực thi các công cụ phù hợp với vai trò của mình:
    /// - QuanTriVien: Toàn quyền truy cập tất cả công cụ quản trị, giảng viên, và học tập.
    /// - GiangVien: Quyền truy cập các công cụ giảng dạy, bài học, đề thi, ngân hàng câu hỏi, và thống kê. Không được thực thi công cụ đặc quyền quản trị viên (như khóa tài khoản, cấu hình hệ thống).
    /// - SinhVien / HocVien: Chỉ được tra cứu bài học, đề thi công khai, tiến độ học tập của chính mình. Tuyệt đối KHÔNG được thực thi công cụ quản trị/giảng viên.
    /// - Khach: Chỉ được tra cứu trợ giúp chung hoặc điều hướng công khai.
    /// </summary>
    public static class AssistantPermissionService
    {
        public const string RoleAdmin = "QuanTriVien";
        public const string RoleLecturer = "GiangVien";
        public const string RoleStudent = "SinhVien";
        public const string RoleGuest = "Khach";
        public const string RoleAny = "Any";

        /// <summary>
        /// Chuẩn hóa chuỗi vai trò từ SessionHelper hoặc CSDL thành một trong 4 nhóm chuẩn.
        /// </summary>
        public static string NormalizeRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role)) return RoleGuest;

            string r = role.Trim().ToLowerInvariant();
            if (r.Contains("admin") || r.Contains("quantri") || r == "1")
            {
                return RoleAdmin;
            }
            if (r.Contains("giangvien") || r.Contains("giang vien") || r.Contains("teacher") || r.Contains("lecturer"))
            {
                return RoleLecturer;
            }
            if (r.Contains("sinhvien") || r.Contains("hocvien") || r.Contains("student") || r == "2")
            {
                return RoleStudent;
            }

            return RoleGuest;
        }

        /// <summary>
        /// Kiểm tra vai trò của người dùng có đủ quyền đáp ứng vai trò yêu cầu hay không.
        /// </summary>
        public static bool IsAuthorized(string userRole, string requiredRole)
        {
            string normalizedUser = NormalizeRole(userRole);
            string normalizedReq = string.IsNullOrWhiteSpace(requiredRole) ? RoleAny : requiredRole.Trim();

            // Nếu công cụ cho phép bất kỳ ai (Any)
            if (string.Equals(normalizedReq, RoleAny, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(normalizedReq, "All", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Quản trị viên có toàn quyền tối cao
            if (normalizedUser == RoleAdmin)
            {
                return true;
            }

            // Khách chưa đăng nhập không có quyền truy cập công cụ nội bộ
            if (normalizedUser == RoleGuest)
            {
                return false;
            }

            // Giảng viên có quyền thực thi công cụ giảng viên và học viên, nhưng bị từ chối công cụ dành riêng cho Quản trị viên
            if (normalizedUser == RoleLecturer)
            {
                if (string.Equals(normalizedReq, RoleAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                return true;
            }

            // Học viên chỉ được thực thi công cụ học viên
            if (normalizedUser == RoleStudent)
            {
                return string.Equals(normalizedReq, RoleStudent, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(normalizedReq, "HocVien", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Thẩm định quyền thực thi công cụ dựa trên ngữ cảnh phiên làm việc.
        /// Trả về false cùng lý do từ chối thân thiện bằng tiếng Việt nếu không đủ thẩm quyền.
        /// </summary>
        public static bool CanExecute(IAssistantTool tool, AssistantContext context, out string reason)
        {
            if (tool == null)
            {
                reason = "Công cụ không tồn tại hoặc chưa được đăng ký.";
                return false;
            }

            if (context == null)
            {
                reason = "Ngữ cảnh người dùng không xác định.";
                return false;
            }

            string userRole = NormalizeRole(context.UserRole);

            // Kiểm tra khách vãng lai
            if (userRole == RoleGuest)
            {
                if (!string.Equals(tool.RequiredRole, RoleAny, StringComparison.OrdinalIgnoreCase))
                {
                    reason = $"Bạn cần đăng nhập vào hệ thống để sử dụng công cụ '{tool.Name}'.";
                    return false;
                }
            }

            // Kiểm tra quyền hạn theo vai trò
            if (!IsAuthorized(userRole, tool.RequiredRole))
            {
                reason = $"Quyền truy cập bị từ chối: Vai trò '{context.UserRole}' không có quyền thực thi công cụ '{tool.Name}' (Yêu cầu vai trò: '{tool.RequiredRole}').";
                return false;
            }

            // Kiểm tra thao tác ghi (Write Operation) đối với khách
            if (tool.IsWriteOperation && userRole == RoleGuest)
            {
                reason = "Khách vãng lai không được phép thực hiện các thao tác ghi dữ liệu vào hệ thống.";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }
}
