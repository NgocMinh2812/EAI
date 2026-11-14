using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS;
using BUS.AI.Factory;
using BUS.AI.Models;
using BUS.Assistant;

namespace BUS.Assistant.Tools
{
    /// <summary>
    /// Công cụ kiểm tra sức khỏe hệ thống: Kết nối CSDL SQL Server, trạng thái các AI Provider, tài nguyên bộ nhớ và môi trường runtime.
    /// Yêu cầu vai trò tối thiểu: GiangVien (QuanTriVien & GiangVien có quyền; SinhVien và Khach bị từ chối).
    /// </summary>
    public class GetSystemHealthTool : IAssistantTool
    {
        public string Name => "get_system_health";
        public string Description => "Kiểm tra trạng thái sức khỏe hệ thống: kết nối CSDL SQL Server, tình trạng các AI Provider, tài nguyên bộ nhớ và môi trường runtime.";
        public string Category => "System";
        public string RequiredRole => AssistantPermissionService.RoleLecturer; // Quản trị & Giảng viên
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("checkAiProviders", "bool", "Có kiểm tra sức khỏe các nhà cung cấp AI hay không (mặc định: true)", isRequired: false, defaultValue: true)
        };

        public async Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            try
            {
                bool checkAi = ToolArgumentHelper.GetBool(arguments, "checkAiProviders", true);

                // 1. Kiểm tra kết nối CSDL TiengAnhAI
                var dashboardBus = new DashboardBUS();
                var (isDbOk, dbLatencyMs, dbMessage) = dashboardBus.KiemTraKetNoiDatabase();

                // 2. Kiểm tra sức khỏe các AI Provider
                var aiProvidersStatus = new List<object>();
                bool anyAiHealthy = false;

                if (checkAi)
                {
                    try
                    {
                        var healthMap = await AIProviderFactory.Instance.CheckAllHealthAsync(ct);
                        foreach (var kvp in healthMap)
                        {
                            bool isHealthy = string.Equals(kvp.Value.Health, "Operational", StringComparison.OrdinalIgnoreCase);
                            if (isHealthy) anyAiHealthy = true;

                            aiProvidersStatus.Add(new
                            {
                                provider = kvp.Key.ToString(),
                                health = kvp.Value.Health,
                                message = kvp.Value.Message,
                                latency_ms = kvp.Value.LatencyMs,
                                last_checked = kvp.Value.LastChecked?.ToString("HH:mm:ss") ?? "Chưa kiểm tra"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        aiProvidersStatus.Add(new
                        {
                            provider = "All",
                            health = "Error",
                            message = $"Lỗi kiểm tra AI providers: {ex.Message}",
                            latency_ms = 0,
                            last_checked = DateTime.Now.ToString("HH:mm:ss")
                        });
                    }
                }
                else
                {
                    var configured = AIProviderFactory.Instance.GetConfiguredProviders();
                    anyAiHealthy = configured.Count > 0;
                    foreach (var p in configured)
                    {
                        aiProvidersStatus.Add(new
                        {
                            provider = p.ProviderType.ToString(),
                            health = "Configured",
                            message = "Đã cấu hình hợp lệ",
                            latency_ms = 0,
                            last_checked = DateTime.Now.ToString("HH:mm:ss")
                        });
                    }
                }

                // 3. Thu thập thông số runtime ứng dụng (Headless safe)
                long workingSetMb = 0;
                TimeSpan uptime = TimeSpan.Zero;
                try
                {
                    using var proc = Process.GetCurrentProcess();
                    workingSetMb = proc.WorkingSet64 / (1024 * 1024);
                    uptime = DateTime.Now - proc.StartTime;
                }
                catch
                {
                    workingSetMb = GC.GetTotalMemory(false) / (1024 * 1024);
                }

                // 4. Đánh giá trạng thái chung
                string overallStatus = "Healthy";
                if (!isDbOk)
                {
                    overallStatus = "Unhealthy";
                }
                else if (checkAi && !anyAiHealthy && aiProvidersStatus.Count > 0)
                {
                    overallStatus = "Degraded";
                }

                var healthPayload = new
                {
                    overall_status = overallStatus,
                    database = new
                    {
                        connected = isDbOk,
                        latency_ms = dbLatencyMs,
                        message = dbMessage
                    },
                    ai_providers = aiProvidersStatus,
                    runtime = new
                    {
                        memory_working_set_mb = workingSetMb,
                        uptime_minutes = Math.Round(uptime.TotalMinutes, 1),
                        dotnet_version = Environment.Version.ToString(),
                        os_platform = RuntimeInformation.OSDescription
                    },
                    checked_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                string summary = $"[Sức khỏe hệ thống: {overallStatus}] CSDL: {(isDbOk ? "Kết nối tốt" : "Mất kết nối")} ({dbLatencyMs}ms) | RAM tiêu thụ: {workingSetMb} MB | AI Providers: {aiProvidersStatus.Count} nhà cung cấp.";
                return AssistantToolResult.OkWithJsonPayload(summary, healthPayload, "system_health_card");
            }
            catch (Exception ex)
            {
                return AssistantToolResult.Fail($"Lỗi khi kiểm tra sức khỏe hệ thống: {ex.Message}", "SYSTEM_HEALTH_CHECK_FAILED");
            }
        }
    }

    /// <summary>
    /// Công cụ cẩm nang hướng dẫn sử dụng tính năng EAI theo từng vai trò (Học viên, Giảng viên, Quản trị).
    /// Khả dụng cho toàn bộ người dùng (Role = Any).
    /// </summary>
    public class GetFeatureGuideTool : IAssistantTool
    {
        public string Name => "get_feature_guide";
        public string Description => "Tra cứu hướng dẫn sử dụng các tính năng trong phần mềm EAI theo từng vai trò (Học viên, Giảng viên, Quản trị viên).";
        public string Category => "System";
        public string RequiredRole => AssistantPermissionService.RoleAny;
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;

        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("featureName", "string", "Tên tính năng hoặc từ khóa cần tra cứu (ví dụ: 'luyen_noi', 'thi_thu', 'tao_de_ai', 'ngan_hang_cau_hoi', 'thong_ke', 'giam_sat_he_thong', 'tat_ca')", isRequired: false),
            new("role", "string", "Lọc tính năng theo vai trò người dùng (mặc định theo phiên làm việc)", isRequired: false)
        };

        private class FeatureEntry
        {
            public string Key { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string TargetRole { get; set; } = "Any";
            public string ScreenCode { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public List<string> Steps { get; set; } = new();
            public string Tips { get; set; } = string.Empty;
        }

        private static readonly List<FeatureEntry> KnowledgeBase = new()
        {
            new FeatureEntry
            {
                Key = "luyen_tap_hoc_tap",
                Title = "Học bài & Luyện tập ngữ pháp, từ vựng",
                TargetRole = "SinhVien",
                ScreenCode = "HocTap",
                Description = "Học theo lộ trình chia theo chủ đề và trình độ từ A1 đến C1.",
                Steps = new List<string>
                {
                    "1. Vào mục 'Học tập' trên thanh điều hướng bên trái.",
                    "2. Chọn trình độ phù hợp (A1, A2, B1, B2, C1) và chủ đề muốn học.",
                    "3. Đọc kỹ phần lý thuyết và ví dụ minh họa.",
                    "4. Làm bài tập thực hành trực tiếp và bấm 'Kiểm tra' để xem giải thích chi tiết."
                },
                Tips = "Bạn có thể xem lại tỷ lệ hoàn thành của từng bài tại mục 'Tiến độ học'."
            },
            new FeatureEntry
            {
                Key = "luyen_noi_ai",
                Title = "Luyện nói Speaking AI & Chấm phát âm",
                TargetRole = "SinhVien",
                ScreenCode = "LuyenNoi",
                Description = "Luyện phát âm chuẩn IPA và đối thoại trực tiếp với AI thông minh.",
                Steps = new List<string>
                {
                    "1. Mở tính năng 'Luyện nói AI' trên giao diện học viên.",
                    "2. Chọn mẫu câu hoặc đoạn hội thoại theo chủ đề giao tiếp.",
                    "3. Bấm nút Mic và đọc rõ ràng vào micro.",
                    "4. Hệ thống AI phân tích độ chính xác, ngữ điệu và chỉ ra các từ phát âm chưa chuẩn."
                },
                Tips = "Đảm bảo microphone hoạt động tốt và môi trường xung quanh không có tạp âm."
            },
            new FeatureEntry
            {
                Key = "thi_thu_khao_sat",
                Title = "Làm bài thi thử & Khảo sát năng lực",
                TargetRole = "SinhVien",
                ScreenCode = "ThiThu",
                Description = "Tham gia các kỳ thi thử trực tuyến với đồng hồ bấm giờ và chấm điểm tự động chuẩn hóa.",
                Steps = new List<string>
                {
                    "1. Mở màn hình 'Thi thử' để xem danh sách các đề thi đang mở.",
                    "2. Chọn đề thi mong muốn và bấm 'Bắt đầu làm bài'.",
                    "3. Hoàn thành các câu hỏi trắc nghiệm trước khi hết thời gian.",
                    "4. Bấm 'Nộp bài' để hệ thống chấm điểm và xuất bảng phân tích kết quả."
                },
                Tips = "Kết quả bài thi sẽ được lưu tự động vào lịch sử thi để theo dõi sự tiến bộ."
            },
            new FeatureEntry
            {
                Key = "quan_ly_kho_hoc_lieu",
                Title = "Quản lý bài giảng & Kho học liệu",
                TargetRole = "GiangVien",
                ScreenCode = "NoiDungHocTap",
                Description = "Soạn thảo, cập nhật và xuất bản tài liệu học tập, bài giảng cho sinh viên.",
                Steps = new List<string>
                {
                    "1. Chọn 'Nội dung học tập' trong giao diện Giảng viên.",
                    "2. Bấm 'Thêm bài học mới' hoặc chọn bài học hiện có để chỉnh sửa.",
                    "3. Nhập tiêu đề, tóm tắt, nội dung lý thuyết và thời lượng ước tính.",
                    "4. Bật tùy chọn 'Đã xuất bản' để học viên có thể tiếp cận tài liệu."
                },
                Tips = "Có thể phân loại bài học theo cây danh mục chủ đề để học viên dễ tra cứu."
            },
            new FeatureEntry
            {
                Key = "tao_cau_hoi_de_thi_ai",
                Title = "Sinh câu hỏi & Đề thi tự động bằng AI",
                TargetRole = "GiangVien",
                ScreenCode = "AISinhDe",
                Description = "Tận dụng mô hình AI (Gemini, Groq...) để sinh câu hỏi trắc nghiệm, điền từ và đọc hiểu tự động.",
                Steps = new List<string>
                {
                    "1. Mở màn hình 'Sinh đề AI' hoặc yêu cầu trực tiếp qua Trợ lý AI Sidebar.",
                    "2. Chọn chủ đề, trình độ mục tiêu và số lượng câu hỏi cần sinh.",
                    "3. Bấm 'Tạo nội dung' và xem trước câu hỏi trên Thẻ xem trước (Preview Card).",
                    "4. Bấm 'Xác nhận lưu' để lưu chính thức vào Ngân hàng câu hỏi."
                },
                Tips = "Mọi thao tác lưu câu hỏi từ AI bắt buộc qua bước duyệt xác nhận để đảm bảo chất lượng học liệu."
            },
            new FeatureEntry
            {
                Key = "giam_sat_he_thong",
                Title = "Giám sát hệ thống & Cấu hình API AI",
                TargetRole = "GiangVien",
                ScreenCode = "GiamSatHeThong",
                Description = "Kiểm tra kết nối CSDL, theo dõi sức khỏe và cấu hình API key cho các AI Providers.",
                Steps = new List<string>
                {
                    "1. Mở màn hình 'Giám sát hệ thống' trong menu quản trị/giảng viên.",
                    "2. Xem trạng thái kết nối SQL Server và độ trễ phản hồi (ms).",
                    "3. Kiểm tra danh sách API Provider: Gemini, Groq, Ollama, Xkio.",
                    "4. Cập nhật API Key hoặc đổi mô hình dự phòng khi cần thiết."
                },
                Tips = "Khóa API luôn được mã hóa hiển thị an toàn (dạng AIza••••••••BCDE)."
            },
            new FeatureEntry
            {
                Key = "quan_ly_nguoi_dung",
                Title = "Quản lý danh sách người dùng",
                TargetRole = "QuanTriVien",
                ScreenCode = "NguoiDung",
                Description = "Quản lý tài khoản học viên, giảng viên, phân quyền và khóa/mở khóa tài khoản.",
                Steps = new List<string>
                {
                    "1. Mở menu 'Người dùng' trong giao diện quản trị viên.",
                    "2. Tìm kiếm theo tên đăng nhập, họ tên hoặc lọc theo vai trò.",
                    "3. Chỉnh sửa thông tin tài khoản, đặt lại mật khẩu hoặc đổi vai trò.",
                    "4. Khóa hoặc kích hoạt lại tài khoản khi cần thiết."
                },
                Tips = "Chỉ Quản trị viên tối cao mới có quyền thay đổi thông tin và trạng thái người dùng."
            },
            new FeatureEntry
            {
                Key = "tro_ly_ai",
                Title = "Sử dụng Trợ lý AI EAI Sidebar",
                TargetRole = "Any",
                ScreenCode = "General",
                Description = "Trợ lý ảo hỗ trợ học tập, tra cứu tiến độ, tạo đề thi và điều hướng màn hình.",
                Steps = new List<string>
                {
                    "1. Bấm nút 'Trợ lý AI' trên thanh tiêu đề góc phải màn hình.",
                    "2. Nhập câu hỏi hoặc yêu cầu vào khung chat (Enter để gửi, Shift+Enter xuống dòng).",
                    "3. Sử dụng các gợi ý nhanh ở cột mép phải hoặc danh sách lịch sử hội thoại.",
                    "4. Với các thao tác ghi dữ liệu, kiểm tra thẻ xem trước rồi bấm 'Xác nhận'."
                },
                Tips = "Bạn có thể bấm nút 'Dừng tạo' bất kỳ lúc nào nếu muốn hủy yêu cầu đang xử lý."
            }
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            string currentRole = context.UserRole;
            string? roleArg = ToolArgumentHelper.GetString(arguments, "role");
            if (!string.IsNullOrWhiteSpace(roleArg))
            {
                currentRole = roleArg;
            }

            string featureQuery = ToolArgumentHelper.GetString(arguments, "featureName")?.Trim() ?? string.Empty;

            // Lọc danh mục theo vai trò người dùng
            var availableFeatures = KnowledgeBase
                .Where(f => AssistantPermissionService.IsAuthorized(currentRole, f.TargetRole))
                .ToList();

            if (string.IsNullOrWhiteSpace(featureQuery) || featureQuery.Equals("tat_ca", StringComparison.OrdinalIgnoreCase) || featureQuery.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                // Trả về danh sách tóm tắt tất cả tính năng khả dụng
                var listPayload = availableFeatures.Select(f => new
                {
                    key = f.Key,
                    title = f.Title,
                    screen = f.ScreenCode,
                    description = f.Description
                }).ToList();

                string listSummary = $"Hệ thống EAI có {availableFeatures.Count} nhóm tính năng chính dành cho vai trò '{currentRole}'. Bạn có thể yêu cầu xem chi tiết từng tính năng (ví dụ: 'Hướng dẫn luyện nói', 'Cách tạo câu hỏi bằng AI').";
                return Task.FromResult(AssistantToolResult.OkWithJsonPayload(listSummary, listPayload, "feature_guide_card"));
            }

            // Tìm kiếm tính năng theo từ khóa
            var match = availableFeatures.FirstOrDefault(f => 
                f.Key.IndexOf(featureQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                f.Title.IndexOf(featureQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                f.Description.IndexOf(featureQuery, StringComparison.OrdinalIgnoreCase) >= 0);

            if (match == null)
            {
                string suggestMsg = $"Không tìm thấy tài liệu hướng dẫn cụ thể cho từ khóa '{featureQuery}'. Các tính năng hiện có: " +
                    string.Join(", ", availableFeatures.Select(f => f.Title));
                return Task.FromResult(AssistantToolResult.Fail(suggestMsg, "FEATURE_NOT_FOUND"));
            }

            var detailPayload = new
            {
                key = match.Key,
                title = match.Title,
                target_role = match.TargetRole,
                screen_code = match.ScreenCode,
                description = match.Description,
                steps = match.Steps,
                tips = match.Tips
            };

            string detailMsg = $"### Hướng dẫn: {match.Title}\n{match.Description}\n\n**Các bước thực hiện:**\n" +
                string.Join("\n", match.Steps) + $"\n\n*Lưu ý:* {match.Tips}";

            return Task.FromResult(AssistantToolResult.OkWithJsonPayload(detailMsg, detailPayload, "feature_guide_card"));
        }
    }
}
