using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BUS.Assistant.Tools;

namespace BUS.Assistant
{
    /// <summary>
    /// Sổ đăng ký công cụ (Tool Registry) tập trung cho Trợ lý AI EAI.
    /// Quản lý việc đăng ký, tra cứu, lọc theo quyền hạn và kết xuất schema công cụ vào Prompt AI.
    /// </summary>
    public class AssistantToolRegistry
    {
        private static readonly Lazy<AssistantToolRegistry> _instance = new(() => new AssistantToolRegistry());
        public static AssistantToolRegistry Instance => _instance.Value;

        private readonly Dictionary<string, IAssistantTool> _tools = new(StringComparer.OrdinalIgnoreCase);
        private readonly object _lock = new();

        public AssistantToolRegistry() { }

        /// <summary>
        /// Đăng ký một công cụ nghiệp vụ vào hệ thống.
        /// </summary>
        public void RegisterTool(IAssistantTool tool)
        {
            if (tool == null) throw new ArgumentNullException(nameof(tool));
            if (string.IsNullOrWhiteSpace(tool.Name)) throw new ArgumentException("Tên công cụ không được để trống.", nameof(tool));

            lock (_lock)
            {
                _tools[tool.Name.Trim()] = tool;
            }
        }

        /// <summary>
        /// Lấy công cụ theo tên (không phân biệt hoa thường).
        /// </summary>
        public IAssistantTool? GetTool(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            lock (_lock)
            {
                _tools.TryGetValue(name.Trim(), out var tool);
                return tool;
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách công cụ đã đăng ký.
        /// </summary>
        public IReadOnlyList<IAssistantTool> GetAllTools()
        {
            lock (_lock)
            {
                return _tools.Values.ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách công cụ được phép sử dụng đối với vai trò người dùng cụ thể.
        /// </summary>
        public IReadOnlyList<IAssistantTool> GetToolsForRole(string role)
        {
            lock (_lock)
            {
                return _tools.Values
                    .Where(tool => AssistantPermissionService.IsAuthorized(role, tool.RequiredRole))
                    .ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách công cụ theo nhóm nghiệp vụ (Category).
        /// </summary>
        public IReadOnlyList<IAssistantTool> GetToolsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return new List<IAssistantTool>();

            lock (_lock)
            {
                return _tools.Values
                    .Where(tool => string.Equals(tool.Category, category.Trim(), StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        /// <summary>
        /// Sinh chuỗi mô tả các công cụ được phép theo vai trò để nhúng vào System Prompt cho AI.
        /// </summary>
        public string BuildToolDeclarationsPrompt(string role)
        {
            var allowedTools = GetToolsForRole(role);
            if (allowedTools.Count == 0)
            {
                return "Không có công cụ nào khả dụng cho vai trò hiện tại.";
            }

            var toolList = allowedTools.Select(t => new
            {
                name = t.Name,
                description = t.Description,
                category = t.Category,
                is_write = t.IsWriteOperation,
                requires_confirmation = t.RequiresConfirmation,
                parameters = t.Parameters.Select(p => new
                {
                    name = p.Name,
                    type = p.Type,
                    description = p.Description,
                    required = p.IsRequired,
                    allowed_values = p.AllowedValues,
                    default_value = p.DefaultValue
                }).ToList()
            }).ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(toolList, options);
        }

        /// <summary>
        /// Đăng ký toàn bộ 16 công cụ nghiệp vụ mặc định (Nhóm A đến F) vào sổ đăng ký hệ thống.
        /// Hỗ trợ nạp lại (forceReload) an toàn cho môi trường Unit Test.
        /// </summary>
        public void RegisterDefaultTools(bool forceReload = false)
        {
            lock (_lock)
            {
                if (_tools.Count > 0 && !forceReload)
                {
                    return;
                }

                if (forceReload)
                {
                    _tools.Clear();
                }

                // Nhóm A: Tra cứu & Tìm kiếm (4 công cụ)
                RegisterTool(new SearchUsersTool());
                RegisterTool(new SearchLessonsTool());
                RegisterTool(new SearchExamsTool());
                RegisterTool(new SearchQuestionsTool());

                // Nhóm B: Thống kê & Phân tích (3 công cụ)
                RegisterTool(new GetSystemOverviewStatisticsTool());
                RegisterTool(new GetTeacherStatisticsTool());
                RegisterTool(new GetStudentLearningStatisticsTool());

                // Nhóm C: Tạo nội dung học tập & Xác nhận 2 bước (4 công cụ)
                RegisterTool(new GenerateMultipleChoiceQuestionTool());
                RegisterTool(new GenerateFillBlankQuestionTool());
                RegisterTool(new GenerateReadingPassageTool());
                RegisterTool(new SaveGeneratedQuestionTool());

                // Nhóm D: Quản lý học tập & Lịch sử (2 công cụ)
                RegisterTool(new GetLessonProgressTool());
                RegisterTool(new GetExamHistoryTool());

                // Nhóm E: Điều hướng giao diện (1 công cụ)
                RegisterTool(new NavigateScreenTool());

                // Nhóm F: Giám sát hệ thống & Cẩm nang trợ giúp (2 công cụ)
                RegisterTool(new GetSystemHealthTool());
                RegisterTool(new GetFeatureGuideTool());
            }
        }

        /// <summary>
        /// Xóa sạch các công cụ đã đăng ký (phục vụ reset trong unit test).
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _tools.Clear();
            }
        }
    }
}
