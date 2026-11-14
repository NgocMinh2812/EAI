using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS;
using DAL;
using DTO;
using DTO.Assistant;

namespace BUS.Assistant.Tools
{
    /// <summary>
    /// Helper tiện ích hỗ trợ bóc tách an toàn các tham số truyền vào công cụ từ Dictionary/JSON.
    /// </summary>
    public static class ToolArgumentHelper
    {
        public static string? GetString(Dictionary<string, object?> args, string key, string? defaultValue = null)
        {
            if (args.TryGetValue(key, out var val) && val != null)
            {
                if (val is JsonElement je) return je.ValueKind == JsonValueKind.String ? je.GetString() : je.ToString();
                return val.ToString();
            }
            return defaultValue;
        }

        public static int GetInt(Dictionary<string, object?> args, string key, int defaultValue = 0)
        {
            if (args.TryGetValue(key, out var val) && val != null)
            {
                if (val is int iVal) return iVal;
                if (val is JsonElement je && je.TryGetInt32(out int i)) return i;
                if (int.TryParse(val.ToString(), out int parsed)) return parsed;
            }
            return defaultValue;
        }

        public static int? GetNullableInt(Dictionary<string, object?> args, string key)
        {
            if (args.TryGetValue(key, out var val) && val != null)
            {
                if (val is int iVal) return iVal;
                if (val is JsonElement je && je.TryGetInt32(out int i)) return i;
                if (int.TryParse(val.ToString(), out int parsed)) return parsed;
            }
            return null;
        }

        public static bool? GetNullableBool(Dictionary<string, object?> args, string key)
        {
            if (args.TryGetValue(key, out var val) && val != null)
            {
                if (val is bool bVal) return bVal;
                if (val is JsonElement je)
                {
                    if (je.ValueKind == JsonValueKind.True) return true;
                    if (je.ValueKind == JsonValueKind.False) return false;
                }
                if (bool.TryParse(val.ToString(), out bool b)) return b;
            }
            return null;
        }

        public static bool GetBool(Dictionary<string, object?> args, string key, bool defaultValue = false)
        {
            return GetNullableBool(args, key) ?? defaultValue;
        }
    }

    /// <summary>
    /// Công cụ tìm kiếm người dùng trong hệ thống (Học viên, Giảng viên, Quản trị viên).
    /// Yêu cầu vai trò: GiangVien (Giảng viên & Quản trị viên).
    /// </summary>
    public class SearchUsersTool : IAssistantTool
    {
        public string Name => "search_users";
        public string Description => "Tìm kiếm người dùng, học viên, giảng viên theo họ tên, email, tên đăng nhập hoặc vai trò trong hệ thống.";
        public string Category => "Search";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("keyword", "string", "Từ khóa tìm kiếm theo họ tên, email hoặc tên đăng nhập", isRequired: false),
            new("role", "string", "Lọc theo vai trò (TatCa, HocVien, GiangVien, QuanTriVien)", isRequired: false, allowedValues: new[] { "TatCa", "HocVien", "GiangVien", "QuanTriVien" }, defaultValue: "TatCa"),
            new("isActive", "bool", "Lọc theo trạng thái hoạt động (true: hoạt động, false: khóa, null: tất cả)", isRequired: false),
            new("limit", "int", "Số lượng bản ghi tối đa (1 - 50)", isRequired: false, defaultValue: 10)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                string? keyword = ToolArgumentHelper.GetString(arguments, "keyword");
                string role = ToolArgumentHelper.GetString(arguments, "role", "TatCa")!;
                bool? isActive = ToolArgumentHelper.GetNullableBool(arguments, "isActive");
                int limit = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "limit", 10), 1, 50);

                string? normalizedRole = (string.Equals(role, "TatCa", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(role)) ? null : role;

                var (items, total) = NguoiDungBUS.Instance.LayDanhSachPhanTrang(1, limit, keyword, normalizedRole, isActive);

                var userSummaries = items.Select(u => new
                {
                    userId = u.MaNguoiDung,
                    username = u.TenDangNhap,
                    fullName = u.HoTen,
                    email = u.Email,
                    role = u.TenVaiTro,
                    roleDisplay = u.TenVaiTro switch { "QuanTriVien" => "Quản trị viên", "GiangVien" => "Giảng viên", _ => "Học viên" },
                    isActive = u.TrangThaiHoatDong,
                    targetLevel = u.TrinhDoMucTieu,
                    lastLogin = u.LanDangNhapCuoi?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa đăng nhập"
                }).ToList();

                var payload = new
                {
                    totalCount = total,
                    displayedCount = userSummaries.Count,
                    keyword = keyword ?? string.Empty,
                    role = role,
                    users = userSummaries
                };

                string msg = total > 0 
                    ? $"Tìm thấy {total} người dùng trong hệ thống (đang hiển thị {userSummaries.Count} người dùng đầu tiên)."
                    : "Không tìm thấy người dùng nào phù hợp với điều kiện tìm kiếm.";

                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, payload, "UserListCard"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi tra cứu người dùng: {ex.Message}", "SEARCH_USERS_ERROR"));
            }
        }
    }

    /// <summary>
    /// Công cụ tra cứu danh sách bài học và học liệu trong hệ thống.
    /// Học viên và Khách chỉ nhìn thấy các bài học đã xuất bản (DaXuatBan = true).
    /// </summary>
    public class SearchLessonsTool : IAssistantTool
    {
        public string Name => "search_lessons";
        public string Description => "Tra cứu bài học và tài liệu học tập trong hệ thống theo từ khóa, kỹ năng, chủ đề hoặc trình độ CEFR.";
        public string Category => "Search";
        public string RequiredRole => AssistantPermissionService.RoleAny;
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("keyword", "string", "Từ khóa trong tiêu đề hoặc tóm tắt bài học", isRequired: false),
            new("topicId", "int", "Mã chủ đề khóa học (MaChuDe)", isRequired: false),
            new("skill", "string", "Kỹ năng (TatCa, TuVung, NguPhap, Doc, Nghe, Viet)", isRequired: false, defaultValue: "TatCa"),
            new("level", "string", "Trình độ (TatCa, A1, A2, B1, B2, C1, C2)", isRequired: false, defaultValue: "TatCa"),
            new("limit", "int", "Số lượng bài học tối đa (1 - 50)", isRequired: false, defaultValue: 10)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                string? keyword = ToolArgumentHelper.GetString(arguments, "keyword");
                int? topicId = ToolArgumentHelper.GetNullableInt(arguments, "topicId");
                string skill = ToolArgumentHelper.GetString(arguments, "skill", "TatCa")!;
                string level = ToolArgumentHelper.GetString(arguments, "level", "TatCa")!;
                int limit = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "limit", 10), 1, 50);

                var list = BaiHocBUS.LayDanhSachTuDatabase(maChuDe: topicId, search: keyword);

                // Bảo vệ nội dung: Học viên và Khách chỉ xem bài học đã phát hành
                if (context.IsStudent || context.IsGuest)
                {
                    list = list.Where(b => b.DaXuatBan).ToList();
                }

                if (!string.Equals(skill, "TatCa", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(skill))
                {
                    list = list.Where(b => string.Equals(b.LoaiBaiHoc, skill, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.Equals(level, "TatCa", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(level))
                {
                    list = list.Where(b => string.Equals(b.MaTrinhDo, level, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int totalCount = list.Count;
                var displayItems = list.Take(limit).Select(b => new
                {
                    lessonId = b.MaBaiHoc,
                    title = b.TieuDe,
                    skill = b.LoaiBaiHoc,
                    skillDisplay = b.TenLoaiHienThi,
                    level = b.MaTrinhDo,
                    topicName = b.TenChuDe,
                    durationMinutes = b.ThoiLuongUocTinh,
                    summary = b.TomTat,
                    isPublished = b.DaXuatBan
                }).ToList();

                var payload = new
                {
                    totalCount = totalCount,
                    displayedCount = displayItems.Count,
                    keyword = keyword ?? string.Empty,
                    skill = skill,
                    level = level,
                    lessons = displayItems
                };

                string msg = totalCount > 0
                    ? $"Tìm thấy {totalCount} bài học phù hợp (hiển thị {displayItems.Count} bài học đầu tiên)."
                    : "Không tìm thấy bài học nào phù hợp với tiêu chí tìm kiếm.";

                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, payload, "LessonListCard"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi tra cứu bài học: {ex.Message}", "SEARCH_LESSONS_ERROR"));
            }
        }
    }

    /// <summary>
    /// Công cụ tìm kiếm bài tập trắc nghiệm, bài thi thử và phòng luyện tập.
    /// Học viên và Khách chỉ xem đề thi đã xuất bản (DaXuatBan = true).
    /// </summary>
    public class SearchExamsTool : IAssistantTool
    {
        public string Name => "search_exams";
        public string Description => "Tìm kiếm các bài tập trắc nghiệm, bài thi thử và phòng luyện tập theo từ khóa, loại hình hoặc trình độ.";
        public string Category => "Search";
        public string RequiredRole => AssistantPermissionService.RoleAny;
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("keyword", "string", "Từ khóa trong tên hoặc mô tả đề thi", isRequired: false),
            new("examType", "string", "Loại hình: TatCa, ThiThu, LuyenTap", isRequired: false, defaultValue: "TatCa"),
            new("level", "string", "Trình độ: TatCa, A1, A2, B1, B2, C1, C2", isRequired: false, defaultValue: "TatCa"),
            new("status", "string", "Trạng thái: TatCa, DaXuatBan, BanNhap, DaDong", isRequired: false, defaultValue: "TatCa"),
            new("limit", "int", "Số lượng đề thi tối đa (1 - 50)", isRequired: false, defaultValue: 10)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                string? keyword = ToolArgumentHelper.GetString(arguments, "keyword");
                string examType = ToolArgumentHelper.GetString(arguments, "examType", "TatCa")!;
                string level = ToolArgumentHelper.GetString(arguments, "level", "TatCa")!;
                string status = ToolArgumentHelper.GetString(arguments, "status", "TatCa")!;
                int limit = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "limit", 10), 1, 50);

                bool chiLayDaXuatBan = context.IsStudent || context.IsGuest;

                string? loaiHinhParam = string.Equals(examType, "TatCa", StringComparison.OrdinalIgnoreCase) ? null : examType;
                string? trinhDoParam = string.Equals(level, "TatCa", StringComparison.OrdinalIgnoreCase) ? null : level;
                string? trangThaiParam = string.Equals(status, "TatCa", StringComparison.OrdinalIgnoreCase) ? null : status;

                var list = BaiTapDeThiBUS.Instance.LayDanhSachDeThi(
                    loaiHinh: loaiHinhParam,
                    trinhDo: trinhDoParam,
                    trangThai: trangThaiParam,
                    tuKhoa: keyword,
                    chiLayDaXuatBan: chiLayDaXuatBan);

                int totalCount = list.Count;
                var displayItems = list.Take(limit).Select(e => new
                {
                    examId = e.MaBaiTapDeThi,
                    title = e.TenBaiTapDeThi,
                    examType = e.LoaiHinh,
                    examTypeDisplay = e.LoaiHinhHienThi,
                    level = e.MaTrinhDo?.ToString() ?? e.MaCapDo,
                    durationMinutes = e.ThoiGianLamBaiPhut,
                    totalQuestions = e.SoLuongCauHoi,
                    passScore = e.DiemDat,
                    totalScore = e.TongDiem,
                    status = e.TrangThai,
                    statusDisplay = e.TrangThaiHienThi,
                    authorName = e.TenNguoiTao,
                    attemptCount = e.SoLuotDaThi
                }).ToList();

                var payload = new
                {
                    totalCount = totalCount,
                    displayedCount = displayItems.Count,
                    keyword = keyword ?? string.Empty,
                    examType = examType,
                    level = level,
                    exams = displayItems
                };

                string msg = totalCount > 0
                    ? $"Tìm thấy {totalCount} đề thi / bài tập phù hợp (hiển thị {displayItems.Count} đề thi đầu tiên)."
                    : "Không tìm thấy đề thi nào phù hợp với điều kiện tìm kiếm.";

                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, payload, "ExamListCard"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi tra cứu đề thi: {ex.Message}", "SEARCH_EXAMS_ERROR"));
            }
        }
    }

    /// <summary>
    /// Công cụ tra cứu câu hỏi trong ngân hàng câu hỏi.
    /// Yêu cầu vai trò: GiangVien (bảo vệ bí mật đề thi, cấm Sinh viên).
    /// </summary>
    public class SearchQuestionsTool : IAssistantTool
    {
        public string Name => "search_questions";
        public string Description => "Tra cứu câu hỏi trong ngân hàng câu hỏi theo nội dung, kỹ năng, độ khó hoặc theo bộ câu hỏi.";
        public string Category => "Search";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("keyword", "string", "Từ khóa tìm trong nội dung câu hỏi", isRequired: false),
            new("skill", "string", "Kỹ năng: TatCa, TuVung, NguPhap, Doc, Nghe, Viet", isRequired: false, defaultValue: "TatCa"),
            new("difficulty", "string", "Độ khó: TatCa, De, TrungBinh, Kho", isRequired: false, defaultValue: "TatCa"),
            new("questionSetId", "int", "Mã bộ câu hỏi (MaBoCauHoi)", isRequired: false),
            new("limit", "int", "Số lượng câu hỏi tối đa (1 - 50)", isRequired: false, defaultValue: 10)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                string? keyword = ToolArgumentHelper.GetString(arguments, "keyword");
                string skill = ToolArgumentHelper.GetString(arguments, "skill", "TatCa")!;
                string difficulty = ToolArgumentHelper.GetString(arguments, "difficulty", "TatCa")!;
                int? questionSetId = ToolArgumentHelper.GetNullableInt(arguments, "questionSetId");
                int limit = Math.Clamp(ToolArgumentHelper.GetInt(arguments, "limit", 10), 1, 50);

                string? skillParam = string.Equals(skill, "TatCa", StringComparison.OrdinalIgnoreCase) ? null : skill;
                string? diffParam = string.Equals(difficulty, "TatCa", StringComparison.OrdinalIgnoreCase) ? null : difficulty;

                List<CauHoiKhaoSatDTO> questions;
                if (questionSetId.HasValue && questionSetId.Value > 0)
                {
                    questions = BoCauHoiBUS.Instance.LayDanhSachCauHoiTheoBo(questionSetId.Value, keyword, skillParam, diffParam);
                }
                else
                {
                    questions = BoCauHoiBUS.Instance.LayTatCaCauHoi(keyword, skillParam, diffParam, top: limit);
                }

                int totalCount = questions.Count;
                var displayItems = questions.Take(limit).Select(q => new
                {
                    questionId = q.MaCauHoi,
                    content = q.NoiDungCauHoi,
                    skill = q.KyNang,
                    skillDisplay = q.KyNangHienThi,
                    difficulty = q.DoKho,
                    difficultyDisplay = q.DoKhoHienThi,
                    level = q.MaTrinhDo,
                    questionType = q.LoaiCauHoi,
                    optionsCount = q.DanhSachLuaChon?.Count ?? 0,
                    isApproved = q.DaKiemDuyet,
                    explanation = q.GiaiThichDapAn
                }).ToList();

                var payload = new
                {
                    totalCount = totalCount,
                    displayedCount = displayItems.Count,
                    keyword = keyword ?? string.Empty,
                    skill = skill,
                    difficulty = difficulty,
                    questions = displayItems
                };

                string msg = totalCount > 0
                    ? $"Tìm thấy {totalCount} câu hỏi trong ngân hàng câu hỏi (hiển thị {displayItems.Count} câu hỏi đầu tiên)."
                    : "Không tìm thấy câu hỏi nào phù hợp với điều kiện tìm kiếm.";

                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, payload, "QuestionListCard"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi tra cứu ngân hàng câu hỏi: {ex.Message}", "SEARCH_QUESTIONS_ERROR"));
            }
        }
    }
}
