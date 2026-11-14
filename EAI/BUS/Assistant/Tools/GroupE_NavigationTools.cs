using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.Assistant;

namespace BUS.Assistant.Tools
{
    /// <summary>
    /// Công cụ hỗ trợ điều hướng giao diện EAI đến màn hình chức năng phù hợp.
    /// Kiểm soát quyền hạn: Ngăn học viên điều hướng vào màn hình Giảng viên / Quản trị.
    /// </summary>
    public class NavigateScreenTool : IAssistantTool
    {
        public string Name => "navigate_screen";
        public string Description => "Điều hướng giao diện ứng dụng đến màn hình chức năng theo yêu cầu của người dùng.";
        public string Category => "Navigation";
        public string RequiredRole => AssistantPermissionService.RoleAny;
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("screenCode", "string", "Mã màn hình đích (TrangChu, HocTap, TienDoHoc, ThiThu, LuyenNoi, TongQuan, NoiDungHocTap, NganHangCauHoi, AISinhDe, DeThi, ThongKe, GiamSatHeThong, NguoiDung, HoSoCaNhan)", isRequired: true),
            new("targetId", "int", "ID đối tượng cụ thể cần mở chi tiết (mã bài học, mã đề thi...)", isRequired: false),
            new("targetType", "string", "Kiểu đối tượng ('Lesson', 'Exam', 'Question', 'User')", isRequired: false)
        };

        // Danh mục màn hình chuẩn và vai trò yêu cầu
        private static readonly Dictionary<string, (string Title, string RequiredRole)> Screens = new(StringComparer.OrdinalIgnoreCase)
        {
            // Màn hình chung & Học viên
            { "TrangChu", ("Trang chủ học tập", "Any") },
            { "HocTap", ("Danh sách bài học & kho học liệu", "SinhVien") },
            { "TienDoHoc", ("Theo dõi tiến độ học tập", "SinhVien") },
            { "LuyenTap", ("Luyện tập bài tập & từ vựng", "SinhVien") },
            { "ThiThu", ("Phòng thi thử & Khảo sát", "SinhVien") },
            { "LuyenNoi", ("Luyện nói Speaking AI", "SinhVien") },
            { "DanhGiaNangLuc", ("Đánh giá năng lực tiếng Anh", "SinhVien") },
            { "HoSoCaNhan", ("Hồ sơ cá nhân & cài đặt", "Any") },

            // Màn hình Giảng viên & Quản trị
            { "TongQuan", ("Tổng quan đào tạo giảng viên", "GiangVien") },
            { "NoiDungHocTap", ("Quản lý nội dung học tập", "GiangVien") },
            { "NganHangCauHoi", ("Ngân hàng câu hỏi", "GiangVien") },
            { "TaoCauHoiAI", ("Tạo câu hỏi tự động bằng AI", "GiangVien") },
            { "AISinhDe", ("Sinh đề thi & bài tập bằng AI", "GiangVien") },
            { "DeThi", ("Quản lý đề thi & bài tập", "GiangVien") },
            { "DeThiLuyenTap", ("Quản lý đề thi & bài tập", "GiangVien") },
            { "ThongKe", ("Báo cáo thống kê đào tạo", "GiangVien") },
            { "ThongKeHocTapGiangVien", ("Thống kê học tập giảng viên", "GiangVien") },
            { "GiamSatHeThong", ("Giám sát hệ thống & API AI", "GiangVien") },
            { "NguoiDung", ("Quản lý danh sách người dùng", "QuanTriVien") },
            { "DanhMuc", ("Quản lý danh mục & chủ đề", "GiangVien") }
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            string? screenCode = ToolArgumentHelper.GetString(arguments, "screenCode")?.Trim();
            if (string.IsNullOrWhiteSpace(screenCode))
            {
                return Task.FromResult(AssistantToolResult.Fail("Mã màn hình (screenCode) không được để trống.", "MISSING_SCREEN_CODE"));
            }

            if (!Screens.TryGetValue(screenCode, out var screenInfo))
            {
                return Task.FromResult(AssistantToolResult.Fail($"Mã màn hình '{screenCode}' không tồn tại trong hệ thống EAI.", "INVALID_SCREEN"));
            }

            // Kiểm tra phân quyền truy cập màn hình
            if (!AssistantPermissionService.IsAuthorized(context.UserRole, screenInfo.RequiredRole))
            {
                return Task.FromResult(AssistantToolResult.Fail($"Bạn không có quyền truy cập màn hình '{screenInfo.Title}' (Yêu cầu vai trò: {screenInfo.RequiredRole}).", "SCREEN_ACCESS_DENIED"));
            }

            int? targetId = ToolArgumentHelper.GetNullableInt(arguments, "targetId");
            string? targetType = ToolArgumentHelper.GetString(arguments, "targetType");

            var navPayload = new
            {
                action = "navigate",
                screen_code = screenCode,
                screen_title = screenInfo.Title,
                target_id = targetId,
                target_type = targetType,
                can_navigate = true
            };

            string successMessage = targetId.HasValue
                ? $"Đang chuyển hướng đến màn hình '{screenInfo.Title}' (Đối tượng: {targetType} #{targetId.Value})."
                : $"Đang chuyển hướng đến màn hình '{screenInfo.Title}'.";

            return Task.FromResult(AssistantToolResult.OkWithJsonPayload(successMessage, navPayload, "navigation_card"));
        }
    }
}
