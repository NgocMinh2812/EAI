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
    /// Công cụ lấy các chỉ số KPI tổng quan hệ thống dành cho Giảng viên và Quản trị viên.
    /// </summary>
    public class GetSystemOverviewStatisticsTool : IAssistantTool
    {
        public string Name => "get_system_overview_statistics";
        public string Description => "Lấy các chỉ số KPI tổng quan hệ thống: Tổng số học viên, bài học, ngân hàng câu hỏi, lượt thi và tỷ lệ tăng trưởng.";
        public string Category => "Statistics";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("period", "string", "Chu kỳ thống kê: All (Tất cả), Today (Hôm nay), Week (7 ngày qua), Month (Tháng này)", isRequired: false, allowedValues: new[] { "All", "Today", "Week", "Month" }, defaultValue: "All")
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                string period = ToolArgumentHelper.GetString(arguments, "period", "All")!;
                int filterPeriod = period.ToLowerInvariant() switch
                {
                    "today" => 0,
                    "week" => 1,
                    "month" => 2,
                    _ => 0
                };

                var dashboardBus = new DashboardBUS();
                var kpi = dashboardBus.LaySoLieuTongQuan(filterPeriod);

                var payload = new
                {
                    statType = "SystemOverview",
                    period = period,
                    periodLabel = period.ToLowerInvariant() switch
                    {
                        "today" => "Hôm nay",
                        "week" => "7 ngày qua",
                        "month" => "30 ngày qua",
                        _ => "Toàn thời gian"
                    },
                    kpis = new object[]
                    {
                        new { name = "Tổng học viên", value = kpi.TongHocVien, growth = $"{kpi.TangTruongHocVien:0.0}%", trend = kpi.HocVienTrendPositive ? "up" : "down" },
                        new { name = "Tổng bài học", value = kpi.TongBaiHoc, growth = $"{kpi.TangTruongBaiHoc:0.0}%", trend = kpi.BaiHocTrendPositive ? "up" : "down" },
                        new { name = "Ngân hàng câu hỏi", value = kpi.TongCauHoi, growth = $"+{kpi.CauHoiMoi} mới", trend = "neutral" },
                        new { name = "Lượt thi & bài nộp", value = kpi.TongLuotThi, growth = $"+{kpi.LuotThiMoi} mới", trend = "up" }
                    },
                    secondaryStats = new
                    {
                        totalExams = kpi.TongDeThi,
                        totalPractice = kpi.TongLuyenTap,
                        activeUsers = kpi.DangHoatDong,
                        lockedUsers = kpi.DaKhoa,
                        totalTopics = kpi.TongChuDe
                    }
                };

                string msg = $"Số liệu tổng quan hệ thống ({payload.periodLabel}): {kpi.TongHocVien} học viên, {kpi.TongBaiHoc} bài học, {kpi.TongCauHoi} câu hỏi, {kpi.TongLuotThi} lượt thi.";
                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, payload, "StatisticsCard"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi lấy số liệu thống kê hệ thống: {ex.Message}", "SYSTEM_STATS_ERROR"));
            }
        }
    }

    /// <summary>
    /// Công cụ thống kê chuyên sâu kết quả học tập và khảo thí dành cho Giảng viên.
    /// </summary>
    public class GetTeacherStatisticsTool : IAssistantTool
    {
        public string Name => "get_teacher_statistics";
        public string Description => "Thống kê chuyên sâu kết quả học tập và khảo thí của học viên: Điểm trung bình, tỷ lệ hoàn thành, phân bố phổ điểm và tỷ lệ đúng theo kỹ năng.";
        public string Category => "Statistics";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // GiangVien & QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("examId", "int", "Mã đề thi cụ thể cần phân tích", isRequired: false),
            new("examType", "string", "Loại hình: TatCa, ThiThu, LuyenTap", isRequired: false, defaultValue: "TatCa"),
            new("fromDate", "string", "Ngày bắt đầu lọc (định dạng yyyy-MM-dd)", isRequired: false),
            new("toDate", "string", "Ngày kết thúc lọc (định dạng yyyy-MM-dd)", isRequired: false)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                int? examId = ToolArgumentHelper.GetNullableInt(arguments, "examId");
                string examType = ToolArgumentHelper.GetString(arguments, "examType", "TatCa")!;
                string? fromDateStr = ToolArgumentHelper.GetString(arguments, "fromDate");
                string? toDateStr = ToolArgumentHelper.GetString(arguments, "toDate");

                DateTime? tuNgay = null;
                DateTime? denNgay = null;
                if (!string.IsNullOrWhiteSpace(fromDateStr) && DateTime.TryParse(fromDateStr, out var parsedFrom))
                    tuNgay = parsedFrom;
                if (!string.IsNullOrWhiteSpace(toDateStr) && DateTime.TryParse(toDateStr, out var parsedTo))
                    denNgay = parsedTo;

                var filter = new ThongKeHocTapFilterDTO
                {
                    MaBaiTapDeThi = examId,
                    HinhThucLam = string.Equals(examType, "TatCa", StringComparison.OrdinalIgnoreCase) ? null : examType,
                    TuNgay = tuNgay,
                    DenNgay = denNgay
                };

                var report = BaoCaoThongKeBUS.Instance.LayBaoCaoThongKe(filter);

                var scoreDist = report.DanhSachPhanBoDiem != null
                    ? report.DanhSachPhanBoDiem.Select(p => (object)new
                    {
                        range = p.KhoangDiem,
                        comment = p.NhanXet,
                        count = p.SoLuot,
                        percentage = p.TyLePhanTram
                    }).ToList()
                    : new List<object>();

                var skillPerf = report.DanhSachKyNang != null
                    ? report.DanhSachKyNang.Select(k => (object)new
                    {
                        skill = k.KyNang,
                        skillDisplay = k.TenKyNang,
                        totalQuestions = k.TongSoCau,
                        correctQuestions = k.SoCauDung,
                        accuracyRate = k.TyLeDung
                    }).ToList()
                    : new List<object>();

                var payload = new
                {
                    statType = "TeacherPerformance",
                    examId = examId,
                    summary = new
                    {
                        totalAttempts = report.TongSoLuotLam,
                        submittedAttempts = report.SoLuotDaNop,
                        inProgressAttempts = report.SoLuotDangLam,
                        cancelledAttempts = report.SoLuotDaHuy,
                        completionRate = report.TyLeHoanThanh,
                        averageScore = report.DiemTrungBinh,
                        averageDurationMinutes = Math.Round(report.ThoiGianTrungBinhGiay / 60.0, 1),
                        uniqueStudents = report.SoHocVienThamGia
                    },
                    scoreDistribution = scoreDist,
                    skillPerformance = skillPerf
                };

                string msg = $"Thống kê khảo thí: Tổng số {report.TongSoLuotLam} lượt thi, {report.SoLuotDaNop} bài đã nộp ({report.TyLeHoanThanh:0.0}%), điểm trung bình: {report.DiemTrungBinh:0.0}/10, số học viên tham gia: {report.SoHocVienThamGia}.";
                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, payload, "StatisticsCard"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi lấy báo cáo thống kê khảo thí: {ex.Message}", "TEACHER_STATS_ERROR"));
            }
        }
    }

    /// <summary>
    /// Công cụ lấy thống kê học tập cá nhân của học viên.
    /// Có cơ chế chống IDOR: Học viên chỉ được xem của chính mình.
    /// </summary>
    public class GetStudentLearningStatisticsTool : IAssistantTool
    {
        public string Name => "get_student_learning_statistics";
        public string Description => "Lấy thống kê học tập cá nhân của học viên: Điểm trung bình, số bài học đã hoàn thành, số phút tích lũy, chuỗi ngày học liên tục và mức độ thành thạo kỹ năng.";
        public string Category => "Statistics";
        public string RequiredRole => AssistantPermissionService.RoleStudent; // Cho phép SinhVien, GiangVien, QuanTriVien
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("studentId", "int", "Mã học viên cần xem (Sinh viên chỉ được tra cứu của chính mình, Giảng viên/Quản trị viên được xem học viên bất kỳ)", isRequired: false, defaultValue: 0)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                int studentId = ToolArgumentHelper.GetInt(arguments, "studentId", 0);

                // Chống IDOR: Học viên và Khách chỉ được xem dữ liệu của chính mình
                int targetUserId;
                if (context.IsStudent || context.IsGuest)
                {
                    targetUserId = context.UserId;
                }
                else
                {
                    targetUserId = studentId > 0 ? studentId : context.UserId;
                }

                if (targetUserId <= 0)
                {
                    return Task.FromResult(AssistantToolResult.Fail("Không xác định được danh tính học viên cần tra cứu thống kê.", "STUDENT_NOT_IDENTIFIED"));
                }

                var stats = BaiHocDAL.Instance.LayThongKeSinhVien(targetUserId);
                var studentInfo = NguoiDungBUS.Instance.LayChiTiet(targetUserId);

                string studentName = studentInfo?.HoTen ?? context.FullName;

                double progressPercentage = stats.TongSoBaiHoc > 0
                    ? Math.Round((double)stats.SoBaiDaHoanThanh * 100.0 / stats.TongSoBaiHoc, 1)
                    : 0.0;

                var recentLessons = stats.DanhSachBaiHocGanDay != null
                    ? stats.DanhSachBaiHocGanDay.Select(b => (object)new
                    {
                        lessonId = b.MaBaiHoc,
                        title = b.TieuDe,
                        progress = b.PhanTramTienDo,
                        status = b.TrangThai,
                        lastAccessed = b.NgayHocGanNhat
                    }).ToList()
                    : new List<object>();

                var payload = new
                {
                    statType = "StudentLearning",
                    studentId = targetUserId,
                    studentName = studentName,
                    summary = new
                    {
                        completedLessons = stats.SoBaiDaHoanThanh,
                        totalLessons = stats.TongSoBaiHoc,
                        progressPercentage = progressPercentage,
                        averageScore = stats.DiemTrungBinh,
                        averageScoreDisplay = stats.DiemTrungBinhHienThi,
                        accumulatedMinutes = stats.SoPhutHocTichLuy,
                        streakDays = stats.ChuoiHocLienTiep,
                        totalExamsTaken = stats.TongSoLuotThi
                    },
                    skillMastery = new
                    {
                        vocabulary = stats.TyLeTuVung,
                        grammar = stats.TyLeNguPhap,
                        reading = stats.TyLeDocHieu,
                        listening = stats.TyLeNgheHieu
                    },
                    recentLessons = recentLessons
                };

                string msg = $"Thống kê học tập của {studentName}: Hoàn thành {stats.SoBaiDaHoanThanh}/{stats.TongSoBaiHoc} bài học ({progressPercentage}%), {stats.SoPhutHocTichLuy} phút học, {stats.ChuoiHocLienTiep} ngày liên tiếp, điểm TB: {stats.DiemTrungBinhHienThi}.";
                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(msg, payload, "StatisticsCard"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AssistantToolResult.Fail($"Lỗi khi lấy thống kê học tập học viên: {ex.Message}", "STUDENT_STATS_ERROR"));
            }
        }
    }
}
