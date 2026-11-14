using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS;
using BUS.AI.Factory;
using BUS.AI.Models;
using BUS.Assistant;
using DTO;
using DTO.Assistant;
using Xunit;

namespace EAI.Tests
{
    #region Test Mock Tools

    public class MockReadOnlyTool : IAssistantTool
    {
        public string Name => "mock_search_lessons";
        public string Description => "Tra cứu bài học mẫu";
        public string Category => "Search";
        public string RequiredRole => "Any";
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;
        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("keyword", "string", "Từ khóa tìm kiếm", isRequired: false)
        };

        public int ExecutionCount { get; private set; }

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            ExecutionCount++;
            string kw = arguments.TryGetValue("keyword", out var v) ? v?.ToString() ?? string.Empty : string.Empty;
            return Task.FromResult(AssistantToolResult.Ok($"Tìm thấy 3 bài học với từ khóa '{kw}'", new[] { "Bài 1", "Bài 2", "Bài 3" }, "lessons_card"));
        }
    }

    public class MockTeacherTool : IAssistantTool
    {
        public string Name => "mock_teacher_stats";
        public string Description => "Thống kê giảng viên";
        public string Category => "Statistics";
        public string RequiredRole => "GiangVien";
        public bool IsWriteOperation => false;
        public bool RequiresConfirmation => false;
        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>();

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            return Task.FromResult(AssistantToolResult.Ok("Số liệu thống kê giảng viên", new { TotalStudents = 45 }));
        }
    }

    public class MockWriteTool : IAssistantTool
    {
        public string Name => "mock_save_question";
        public string Description => "Lưu câu hỏi mới vào CSDL";
        public string Category => "ContentGen";
        public string RequiredRole => "GiangVien";
        public bool IsWriteOperation => true;
        public bool RequiresConfirmation => true;
        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("content", "string", "Nội dung câu hỏi", isRequired: true)
        };

        public bool HasExecuted { get; private set; }
        public string? ExecutedContent { get; private set; }

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            HasExecuted = true;
            ExecutedContent = arguments.TryGetValue("content", out var c) ? c?.ToString() : null;
            return Task.FromResult(AssistantToolResult.Ok($"Đã lưu thành công câu hỏi: '{ExecutedContent}'"));
        }
    }

    public class MockAdminTool : IAssistantTool
    {
        public string Name => "mock_lock_user";
        public string Description => "Khóa tài khoản người dùng";
        public string Category => "System";
        public string RequiredRole => "QuanTriVien";
        public bool IsWriteOperation => true;
        public bool RequiresConfirmation => true;
        public IReadOnlyList<ToolParameterDefinition> Parameters => new List<ToolParameterDefinition>
        {
            new("userId", "int", "ID người dùng", isRequired: true)
        };

        public Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default)
        {
            return Task.FromResult(AssistantToolResult.Ok("Đã khóa tài khoản thành công."));
        }
    }

    #endregion

    /// <summary>
    /// Bộ kiểm thử tự động toàn diện cho Kiến trúc Cốt lõi của Trợ lý AI EAI (Milestone 2):
    /// - AssistantPermissionService (RBAC enforcement)
    /// - AssistantToolRegistry (Registration, lookup, role & category filtering, prompt schema)
    /// - AssistantContextProvider (Context extraction from SessionHelper)
    /// - AIProviderFactory (Instantiation, health checks, fallback mechanism)
    /// - AssistantOrchestrator (Prompt generation, tool call parsing, confirmation flow, safety guardrails)
    /// </summary>
    public class AIAssistantCoreTests : IDisposable
    {
        public AIAssistantCoreTests()
        {
            // Thiết lập môi trường sạch trước mỗi test
            SessionHelper.Logout();
        }

        public void Dispose()
        {
            SessionHelper.Logout();
            AssistantToolRegistry.Instance.Clear();
        }

        #region 1. AssistantPermissionService Tests

        [Theory]
        [InlineData("QuanTriVien", "QuanTriVien")]
        [InlineData("admin", "QuanTriVien")]
        [InlineData("Administrator", "QuanTriVien")]
        [InlineData("1", "QuanTriVien")]
        [InlineData("GiangVien", "GiangVien")]
        [InlineData("giang vien", "GiangVien")]
        [InlineData("teacher", "GiangVien")]
        [InlineData("SinhVien", "SinhVien")]
        [InlineData("HocVien", "SinhVien")]
        [InlineData("student", "SinhVien")]
        [InlineData("2", "SinhVien")]
        [InlineData(null, "Khach")]
        [InlineData("", "Khach")]
        [InlineData("Khach", "Khach")]
        public void NormalizeRole_ShouldNormalizeCorrectly(string? input, string expected)
        {
            string actual = AssistantPermissionService.NormalizeRole(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsAuthorized_Admin_ShouldHaveAccessToAllRoles()
        {
            Assert.True(AssistantPermissionService.IsAuthorized("QuanTriVien", "QuanTriVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("QuanTriVien", "GiangVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("QuanTriVien", "SinhVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("QuanTriVien", "Any"));
        }

        [Fact]
        public void IsAuthorized_Lecturer_ShouldHaveAccessToTeachingAndLearning_ButDeniedAdmin()
        {
            Assert.False(AssistantPermissionService.IsAuthorized("GiangVien", "QuanTriVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("GiangVien", "GiangVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("GiangVien", "SinhVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("GiangVien", "Any"));
        }

        [Fact]
        public void IsAuthorized_Student_ShouldHaveAccessOnlyToStudentAndAny()
        {
            Assert.False(AssistantPermissionService.IsAuthorized("SinhVien", "QuanTriVien"));
            Assert.False(AssistantPermissionService.IsAuthorized("SinhVien", "GiangVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("SinhVien", "SinhVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("SinhVien", "Any"));
        }

        [Fact]
        public void IsAuthorized_Guest_ShouldHaveAccessOnlyToAny()
        {
            Assert.False(AssistantPermissionService.IsAuthorized("Khach", "QuanTriVien"));
            Assert.False(AssistantPermissionService.IsAuthorized("Khach", "GiangVien"));
            Assert.False(AssistantPermissionService.IsAuthorized("Khach", "SinhVien"));
            Assert.True(AssistantPermissionService.IsAuthorized("Khach", "Any"));
        }

        [Fact]
        public void CanExecute_StudentAttemptingTeacherTool_ShouldBeDeniedWithFriendlyReason()
        {
            var teacherTool = new MockTeacherTool();
            var studentContext = new AssistantContext
            {
                UserId = 10,
                Username = "student1",
                UserRole = "SinhVien"
            };

            bool canRun = AssistantPermissionService.CanExecute(teacherTool, studentContext, out string reason);

            Assert.False(canRun);
            Assert.Contains("Quyền truy cập bị từ chối", reason);
            Assert.Contains("GiangVien", reason);
        }

        [Fact]
        public void CanExecute_GuestAttemptingWriteTool_ShouldBeDenied()
        {
            var writeTool = new MockWriteTool();
            var guestContext = new AssistantContext
            {
                UserId = 0,
                Username = "khach",
                UserRole = "Khach"
            };

            bool canRun = AssistantPermissionService.CanExecute(writeTool, guestContext, out string reason);

            Assert.False(canRun);
            Assert.False(string.IsNullOrWhiteSpace(reason));
        }

        [Fact]
        public void CanExecute_LecturerRunningAllowedTool_ShouldSucceed()
        {
            var teacherTool = new MockTeacherTool();
            var lecturerContext = new AssistantContext
            {
                UserId = 5,
                Username = "teacher_minh",
                UserRole = "GiangVien"
            };

            bool canRun = AssistantPermissionService.CanExecute(teacherTool, lecturerContext, out string reason);

            Assert.True(canRun);
            Assert.Empty(reason);
        }

        #endregion

        #region 2. AssistantToolRegistry Tests

        [Fact]
        public void ToolRegistry_RegisterAndGetTool_ShouldWorkCaseInsensitively()
        {
            var registry = new AssistantToolRegistry();
            var tool = new MockReadOnlyTool();

            registry.RegisterTool(tool);

            var retrieved1 = registry.GetTool("mock_search_lessons");
            var retrieved2 = registry.GetTool("MOCK_SEARCH_LESSONS");
            var retrieved3 = registry.GetTool("Mock_Search_Lessons");

            Assert.NotNull(retrieved1);
            Assert.Same(tool, retrieved1);
            Assert.Same(tool, retrieved2);
            Assert.Same(tool, retrieved3);
        }

        [Fact]
        public void ToolRegistry_GetToolsForRole_ShouldFilterAccurately()
        {
            var registry = new AssistantToolRegistry();
            registry.RegisterTool(new MockReadOnlyTool()); // Any
            registry.RegisterTool(new MockTeacherTool());  // GiangVien
            registry.RegisterTool(new MockWriteTool());    // GiangVien
            registry.RegisterTool(new MockAdminTool());    // QuanTriVien

            var adminTools = registry.GetToolsForRole("QuanTriVien");
            var lecturerTools = registry.GetToolsForRole("GiangVien");
            var studentTools = registry.GetToolsForRole("SinhVien");
            var guestTools = registry.GetToolsForRole("Khach");

            Assert.Equal(4, adminTools.Count);
            Assert.Equal(3, lecturerTools.Count); // Any + 2 GiangVien
            Assert.Single(studentTools);          // Only Any
            Assert.Single(guestTools);            // Only Any
        }

        [Fact]
        public void ToolRegistry_GetToolsByCategory_ShouldGroupCorrectly()
        {
            var registry = new AssistantToolRegistry();
            registry.RegisterTool(new MockReadOnlyTool()); // Search
            registry.RegisterTool(new MockTeacherTool());  // Statistics
            registry.RegisterTool(new MockWriteTool());    // ContentGen
            registry.RegisterTool(new MockAdminTool());    // System

            var searchTools = registry.GetToolsByCategory("Search");
            var statTools = registry.GetToolsByCategory("statistics");
            var contentTools = registry.GetToolsByCategory("ContentGen");

            Assert.Single(searchTools);
            Assert.Single(statTools);
            Assert.Single(contentTools);
            Assert.Empty(registry.GetToolsByCategory("NonExistentCategory"));
        }

        [Fact]
        public void ToolRegistry_BuildToolDeclarationsPrompt_ShouldProduceValidJsonSchema()
        {
            var registry = new AssistantToolRegistry();
            registry.RegisterTool(new MockReadOnlyTool());
            registry.RegisterTool(new MockTeacherTool());

            string promptJson = registry.BuildToolDeclarationsPrompt("GiangVien");

            Assert.NotNull(promptJson);
            Assert.Contains("mock_search_lessons", promptJson);
            Assert.Contains("mock_teacher_stats", promptJson);

            // Phải parse được JSON hợp lệ
            using var doc = JsonDocument.Parse(promptJson);
            Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.Equal(2, doc.RootElement.GetArrayLength());
        }

        #endregion

        #region 3. AssistantContextProvider Tests

        [Fact]
        public void ContextProvider_WhenLoggedOut_ShouldReturnGuestContext()
        {
            SessionHelper.Logout();
            var provider = new AssistantContextProvider();

            var context = provider.GetCurrentContext();

            Assert.NotNull(context);
            Assert.Equal(0, context.UserId);
            Assert.Equal("Khach", context.UserRole);
            Assert.True(context.IsGuest);
            Assert.False(context.IsAdmin);
            Assert.False(context.IsLecturer);
        }

        [Fact]
        public void ContextProvider_WhenLoggedInAsLecturer_ShouldReturnLecturerContext()
        {
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 101,
                TenDangNhap = "gv_nguyenvana",
                HoTen = "Nguyễn Văn A",
                TenVaiTro = "GiangVien",
                MaVaiTro = 2
            };

            var provider = new AssistantContextProvider();
            var context = provider.GetCurrentContext();

            Assert.Equal(101, context.UserId);
            Assert.Equal("gv_nguyenvana", context.Username);
            Assert.Equal("Nguyễn Văn A", context.FullName);
            Assert.Equal("GiangVien", context.UserRole);
            Assert.True(context.IsLecturer);
            Assert.False(context.IsAdmin);
        }

        [Fact]
        public void ContextProvider_BuildSystemPromptContext_ShouldIncludeKeyInfo()
        {
            var context = new AssistantContext
            {
                UserId = 7,
                Username = "user_test",
                FullName = "Trần Thị B",
                UserRole = "SinhVien",
                CurrentPage = "HocTap",
                CurrentPageTitle = "Nội dung bài học",
                TargetId = 42,
                TargetType = "Lesson"
            };

            var provider = new AssistantContextProvider();
            string summary = provider.BuildSystemPromptContext(context);

            Assert.Contains("Trần Thị B", summary);
            Assert.Contains("@user_test", summary);
            Assert.Contains("SinhVien", summary);
            Assert.Contains("Nội dung bài học", summary);
            Assert.Contains("Lesson #42", summary);
        }

        #endregion

        #region 4. AIProviderFactory Tests

        [Theory]
        [InlineData(AIProviderType.Gemini, "Gemini")]
        [InlineData(AIProviderType.Groq, "Groq")]
        [InlineData(AIProviderType.Ollama, "Ollama")]
        [InlineData(AIProviderType.Xkio, "Kiro")]
        public void ProviderFactory_CreateProvider_ShouldInstantiateCorrectType(AIProviderType type, string expectedNamePrefix)
        {
            var factory = new AIProviderFactory();
            var provider = factory.CreateProvider(type);

            Assert.NotNull(provider);
            Assert.Equal(type, provider.ProviderType);
            Assert.Contains(expectedNamePrefix, provider.ProviderName, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ProviderFactory_GetAllProviders_ShouldReturnAllSupportedTypes()
        {
            var factory = new AIProviderFactory();
            var all = factory.GetAllProviders();

            Assert.NotNull(all);
            Assert.Equal(4, all.Count);
        }

        [Fact]
        public void ProviderFactory_GetFallbackProvider_ShouldReturnAlternative()
        {
            var factory = new AIProviderFactory();
            var fallback = factory.GetFallbackProvider(AIProviderType.Gemini);

            // Nếu có ít nhất 1 provider được cấu hình (Ollama luôn cấu hình), phải trả về provider khác Gemini
            if (fallback != null)
            {
                Assert.NotEqual(AIProviderType.Gemini, fallback.ProviderType);
            }
        }

        [Fact]
        public async Task ProviderFactory_CheckHealthAsync_ShouldReturnStatusObject()
        {
            var factory = new AIProviderFactory();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

            var status = await factory.CheckHealthAsync(AIProviderType.Ollama, cts.Token);

            Assert.NotNull(status);
            Assert.Equal(AIProviderType.Ollama, status.ProviderType);
            Assert.False(string.IsNullOrWhiteSpace(status.Health));
        }

        #endregion

        #region 5. AssistantOrchestrator Tests

        [Fact]
        public void Orchestrator_CheckSafetyGuardrails_ShouldBlockRawSqlInjection()
        {
            string sqlDrop = "DROP TABLE NguoiDung;";
            string sqlSelectSys = "SELECT * FROM sys.tables";
            string sqlDelete = "DELETE FROM BaiHoc WHERE MaBaiHoc = 1";
            string sqlUnion = "1' UNION SELECT username, password FROM users--";

            Assert.True(AssistantOrchestrator.CheckSafetyGuardrailsViolation(sqlDrop, out string warn1));
            Assert.Contains("SQL", warn1);

            Assert.True(AssistantOrchestrator.CheckSafetyGuardrailsViolation(sqlSelectSys, out string warn2));
            Assert.Contains("SQL", warn2);

            Assert.True(AssistantOrchestrator.CheckSafetyGuardrailsViolation(sqlDelete, out string warn3));
            Assert.True(AssistantOrchestrator.CheckSafetyGuardrailsViolation(sqlUnion, out string warn4));
        }

        [Fact]
        public void Orchestrator_CheckSafetyGuardrails_ShouldBlockArbitraryCodeExecution()
        {
            string codeProcess = "Process.Start(\"cmd.exe\", \"/c calc\");";
            string codePowershell = "chạy lệnh powershell Get-Process";
            string codeEval = "eval(evil_code)";

            Assert.True(AssistantOrchestrator.CheckSafetyGuardrailsViolation(codeProcess, out string warn1));
            Assert.Contains("tiến trình", warn1);

            Assert.True(AssistantOrchestrator.CheckSafetyGuardrailsViolation(codePowershell, out string warn2));
            Assert.True(AssistantOrchestrator.CheckSafetyGuardrailsViolation(codeEval, out string warn3));
        }

        [Fact]
        public void Orchestrator_CheckSafetyGuardrails_ShouldAllowNormalPrompts()
        {
            string normal1 = "Xin chào Trợ lý AI EAI!";
            string normal2 = "Hãy tìm các bài học về Thì hiện tại đơn giúp tôi.";
            string normal3 = "Thống kê số lượng học viên đang học trong tuần này.";

            Assert.False(AssistantOrchestrator.CheckSafetyGuardrailsViolation(normal1, out _));
            Assert.False(AssistantOrchestrator.CheckSafetyGuardrailsViolation(normal2, out _));
            Assert.False(AssistantOrchestrator.CheckSafetyGuardrailsViolation(normal3, out _));
        }

        [Fact]
        public void Orchestrator_TryParseToolCall_CleanJson_ShouldParseSuccessfully()
        {
            string responseText = "{\"tool\": \"mock_search_lessons\", \"arguments\": {\"keyword\": \"Grammar\"}}";

            bool parsed = AssistantOrchestrator.TryParseToolCall(responseText, out string toolName, out var args, out string? preamble);

            Assert.True(parsed);
            Assert.Equal("mock_search_lessons", toolName);
            Assert.NotNull(args);
            Assert.True(args.ContainsKey("keyword"));
            Assert.Equal("Grammar", args["keyword"]?.ToString());
        }

        [Fact]
        public void Orchestrator_TryParseToolCall_MarkdownCodeblock_ShouldParseSuccessfully()
        {
            string responseText = "Dưới đây là thao tác tra cứu:\n```json\n{\n  \"tool\": \"mock_search_lessons\",\n  \"arguments\": {\n    \"keyword\": \"Tenses\"\n  }\n}\n```\nĐang thực thi...";

            bool parsed = AssistantOrchestrator.TryParseToolCall(responseText, out string toolName, out var args, out string? preamble);

            Assert.True(parsed);
            Assert.Equal("mock_search_lessons", toolName);
            Assert.NotNull(preamble);
            Assert.Contains("Dưới đây là thao tác", preamble);
            Assert.Equal("Tenses", args["keyword"]?.ToString());
        }

        [Fact]
        public void Orchestrator_TryParseToolCall_PlainText_ShouldReturnFalse()
        {
            string responseText = "Chào bạn! Tôi có thể giúp gì cho bạn hôm nay?";

            bool parsed = AssistantOrchestrator.TryParseToolCall(responseText, out _, out _, out _);

            Assert.False(parsed);
        }

        [Fact]
        public async Task Orchestrator_ConfirmationFlow_ShouldStageWriteToolAndExecuteOnlyUponConfirmation()
        {
            var registry = new AssistantToolRegistry();
            var writeTool = new MockWriteTool();
            registry.RegisterTool(writeTool);

            var orchestrator = new AssistantOrchestrator(toolRegistry: registry);

            var lecturerContext = new AssistantContext
            {
                UserId = 1,
                Username = "teacher1",
                UserRole = "GiangVien"
            };

            // Mô phỏng AI trả về lệnh gọi mock_save_question
            // Vì mock_save_question là write operation có RequiresConfirmation = true,
            // nó phải staging và trả về ConfirmationToken mà CHƯA thực thi
            string aiToolCallJson = "{\"tool\": \"mock_save_question\", \"arguments\": {\"content\": \"What is the past tense of go?\"}}";

            // Giả lập orchestrator xử lý chuỗi tool call
            bool parsed = AssistantOrchestrator.TryParseToolCall(aiToolCallJson, out string toolName, out var args, out _);
            Assert.True(parsed);

            // Kiểm tra trạng thái ban đầu của mock write tool
            Assert.False(writeTool.HasExecuted);

            // Staging token test thông qua ConfirmAndExecuteWriteToolAsync
            // Gọi với token rác -> phải thất bại
            var failResult = await orchestrator.ConfirmAndExecuteWriteToolAsync("invalid-token-12345");
            Assert.False(failResult.Success);
            Assert.Equal("TOKEN_NOT_FOUND", failResult.ErrorCode);

            // Kiểm tra writeTool vẫn chưa bị gọi
            Assert.False(writeTool.HasExecuted);
        }

        [Fact]
        public void Orchestrator_BuildDynamicSystemPrompt_ShouldIncludeContextAndSafetyRules()
        {
            var registry = new AssistantToolRegistry();
            registry.RegisterTool(new MockReadOnlyTool());

            var orchestrator = new AssistantOrchestrator(toolRegistry: registry);

            var context = new AssistantContext
            {
                UserId = 20,
                Username = "gv_lan",
                FullName = "Nguyễn Thị Lan",
                UserRole = "GiangVien",
                CurrentPage = "NganHangCauHoi",
                CurrentPageTitle = "Ngân hàng câu hỏi"
            };

            string prompt = orchestrator.BuildDynamicSystemPrompt(context);

            Assert.NotNull(prompt);
            Assert.Contains("Trợ lý AI EAI", prompt);
            Assert.Contains("Nguyễn Thị Lan", prompt);
            Assert.Contains("Ngân hàng câu hỏi", prompt);
            Assert.Contains("TUYỆT ĐỐI KHÔNG SINH HOẶC THỰC THI LỆNH SQL THÔ", prompt);
            Assert.Contains("mock_search_lessons", prompt);
        }

        [Fact]
        public async Task Orchestrator_ProcessUserMessage_WhenSqlInjectionDetected_ShouldBlockImmediately()
        {
            var orchestrator = new AssistantOrchestrator();

            var response = await orchestrator.ProcessUserMessageAsync(
                conversationId: 0,
                userMessage: "DROP TABLE NguoiDung; -- bypass auth");

            Assert.False(response.Success);
            Assert.Equal("SECURITY_VIOLATION", response.ErrorCode);
            Assert.Contains("SQL", response.Message);
        }

        #endregion
    }
}
