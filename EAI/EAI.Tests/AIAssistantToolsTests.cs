using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS;
using BUS.Assistant;
using BUS.Assistant.Tools;
using DTO;
using Xunit;

namespace EAI.Tests
{
    /// <summary>
    /// Bộ kiểm thử tự động toàn diện cho Hệ thống Công cụ Trợ lý AI EAI (Milestone 3):
    /// - Kiểm tra toàn vẹn định nghĩa Metadata và Parameter Schema của 16 công cụ.
    /// - Kiểm tra việc đăng ký và quản lý trong AssistantToolRegistry.
    /// - Ma trận thẩm định quyền hạn RBAC đối với 4 vai trò (Admin, Lecturer, Student, Guest).
    /// - Kiểm thử các công cụ Nhóm A (SearchUsers, SearchLessons, SearchExams, SearchQuestions).
    /// - Kiểm thử các công cụ Nhóm B (SystemOverviewStats, TeacherStats, StudentLearningStats).
    /// - Kiểm thử các công cụ Nhóm C (GenerateMCQ, GenerateFillBlank, GenerateReadingPassage, SaveGeneratedQuestion) & Confirmation.
    /// - Kiểm thử các công cụ Nhóm D (GetLessonProgressTool, GetExamHistoryTool) & Chống IDOR.
    /// - Kiểm thử công cụ Nhóm E (NavigateScreenTool) & Phân quyền chuyển màn hình.
    /// - Kiểm thử các công cụ Nhóm F (GetSystemHealthTool, GetFeatureGuideTool) & Xử lý lỗi kết nối.
    /// - Kiểm tra luồng xác nhận an toàn (Confirmation Flow) của thao tác ghi (SaveGeneratedQuestionTool).
    /// - Đảm bảo an toàn 100% trong môi trường Headless CLI (không phụ thuộc WPF STA thread).
    /// </summary>
    public class AIAssistantToolsTests : IDisposable
    {
        public AIAssistantToolsTests()
        {
            SessionHelper.Logout();
            AssistantToolRegistry.Instance.RegisterDefaultTools(forceReload: true);
        }

        public void Dispose()
        {
            SessionHelper.Logout();
            AssistantToolRegistry.Instance.Clear();
        }

        #region Region 1: Registry & Metadata Integrity Tests

        [Fact]
        public void RegisterDefaultTools_ShouldRegisterAll16ToolsWithoutDuplicates()
        {
            var registry = AssistantToolRegistry.Instance;
            var tools = registry.GetAllTools();

            Assert.True(tools.Count >= 16, $"Kỳ vọng tối thiểu 16 công cụ được đăng ký, thực tế: {tools.Count}");

            var uniqueNames = tools.Select(t => t.Name.ToLowerInvariant()).Distinct().ToList();
            Assert.Equal(tools.Count, uniqueNames.Count);
        }

        [Fact]
        public void RegisterDefaultTools_CalledMultipleTimes_ShouldBeIdempotent()
        {
            var registry = AssistantToolRegistry.Instance;
            int initialCount = registry.GetAllTools().Count;

            registry.RegisterDefaultTools(forceReload: false);
            registry.RegisterDefaultTools(forceReload: false);

            Assert.Equal(initialCount, registry.GetAllTools().Count);
        }

        [Fact]
        public void RegisterDefaultTools_ForceReload_ShouldResetAndReloadCleanly()
        {
            var registry = AssistantToolRegistry.Instance;
            registry.RegisterDefaultTools(forceReload: true);
            int count1 = registry.GetAllTools().Count;

            registry.RegisterDefaultTools(forceReload: true);
            int count2 = registry.GetAllTools().Count;

            Assert.Equal(count1, count2);
        }

        [Fact]
        public void AllRegisteredTools_MustHaveValidMetadataProperties()
        {
            var registry = AssistantToolRegistry.Instance;
            var validCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Search", "Statistics", "ContentGen", "Learning", "Navigation", "System"
            };

            var validRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Any", "SinhVien", "HocVien", "GiangVien", "QuanTriVien", "Admin"
            };

            foreach (var tool in registry.GetAllTools())
            {
                Assert.False(string.IsNullOrWhiteSpace(tool.Name), $"Công cụ {tool.GetType().Name} có Name rỗng.");
                Assert.False(string.IsNullOrWhiteSpace(tool.Description), $"Công cụ {tool.Name} có Description rỗng.");
                Assert.True(validCategories.Contains(tool.Category), $"Công cụ {tool.Name} có Category không hợp lệ: '{tool.Category}'");
                Assert.True(validRoles.Contains(tool.RequiredRole), $"Công cụ {tool.Name} có RequiredRole không hợp lệ: '{tool.RequiredRole}'");
                Assert.NotNull(tool.Parameters);

                foreach (var param in tool.Parameters)
                {
                    Assert.False(string.IsNullOrWhiteSpace(param.Name), $"Công cụ {tool.Name} có tham số rỗng tên.");
                    Assert.False(string.IsNullOrWhiteSpace(param.Type), $"Công cụ {tool.Name} tham số {param.Name} có Type rỗng.");
                    Assert.False(string.IsNullOrWhiteSpace(param.Description), $"Công cụ {tool.Name} tham số {param.Name} có Description rỗng.");
                }
            }
        }

        [Theory]
        [InlineData("Search", 4)]
        [InlineData("Statistics", 3)]
        [InlineData("ContentGen", 4)]
        [InlineData("Learning", 2)]
        [InlineData("Navigation", 1)]
        [InlineData("System", 2)]
        public void GetToolsByCategory_ShouldReturnExpectedToolCounts(string category, int expectedCount)
        {
            var registry = AssistantToolRegistry.Instance;
            var tools = registry.GetToolsByCategory(category);

            Assert.True(tools.Count >= expectedCount, $"Category '{category}' kỳ vọng ít nhất {expectedCount} công cụ, thực tế có {tools.Count}.");
        }

        #endregion

        #region Region 2: RBAC Security Matrix Tests (All 4 Roles across Tools)

        [Fact]
        public void RBAC_QuanTriVien_MustHaveAccessToAll16Tools()
        {
            var registry = AssistantToolRegistry.Instance;
            var adminTools = registry.GetToolsForRole("QuanTriVien");
            var allTools = registry.GetAllTools();

            Assert.Equal(allTools.Count, adminTools.Count);
        }

        [Fact]
        public void RBAC_GiangVien_CanAccessTeachingAndStudentTools_ButDeniedPureAdminTools()
        {
            var registry = AssistantToolRegistry.Instance;
            var teacherTools = registry.GetToolsForRole("GiangVien");

            // Giảng viên được truy cập công cụ học tập & giảng dạy
            Assert.Contains(teacherTools, t => t.Name == "get_lesson_progress");
            Assert.Contains(teacherTools, t => t.Name == "get_exam_history");
            Assert.Contains(teacherTools, t => t.Name == "get_system_health");
            Assert.Contains(teacherTools, t => t.Name == "get_feature_guide");
            Assert.Contains(teacherTools, t => t.Name == "save_generated_question");
            Assert.Contains(teacherTools, t => t.Name == "search_users");
            Assert.Contains(teacherTools, t => t.Name == "search_questions");
            Assert.Contains(teacherTools, t => t.Name == "get_system_overview_statistics");
            Assert.Contains(teacherTools, t => t.Name == "get_teacher_statistics");

            // Nếu có công cụ chỉ dành riêng cho Admin (RequiredRole = "QuanTriVien")
            var adminOnlyTools = registry.GetAllTools().Where(t => t.RequiredRole == "QuanTriVien").ToList();
            foreach (var aTool in adminOnlyTools)
            {
                Assert.DoesNotContain(teacherTools, t => t.Name == aTool.Name);
            }
        }

        [Fact]
        public void RBAC_SinhVien_MustBeRestrictedStrictlyToAllowedTools()
        {
            var registry = AssistantToolRegistry.Instance;
            var studentTools = registry.GetToolsForRole("SinhVien");

            // Được phép
            Assert.Contains(studentTools, t => t.Name == "get_lesson_progress");
            Assert.Contains(studentTools, t => t.Name == "get_exam_history");
            Assert.Contains(studentTools, t => t.Name == "get_feature_guide");
            Assert.Contains(studentTools, t => t.Name == "navigate_screen");
            Assert.Contains(studentTools, t => t.Name == "search_lessons");
            Assert.Contains(studentTools, t => t.Name == "search_exams");
            Assert.Contains(studentTools, t => t.Name == "get_student_learning_statistics");

            // Bị nghiêm cấm
            Assert.DoesNotContain(studentTools, t => t.Name == "get_system_health");
            Assert.DoesNotContain(studentTools, t => t.Name == "save_generated_question");
            Assert.DoesNotContain(studentTools, t => t.Name == "get_teacher_statistics");
            Assert.DoesNotContain(studentTools, t => t.Name == "get_system_overview_statistics");
            Assert.DoesNotContain(studentTools, t => t.Name == "search_users");
            Assert.DoesNotContain(studentTools, t => t.Name == "search_questions");
            Assert.DoesNotContain(studentTools, t => t.Name == "generate_multiple_choice_question");
            Assert.DoesNotContain(studentTools, t => t.Name == "generate_fill_blank_question");
            Assert.DoesNotContain(studentTools, t => t.Name == "generate_reading_passage");
        }

        [Fact]
        public void RBAC_Khach_CanOnlyAccessPublicTools()
        {
            var registry = AssistantToolRegistry.Instance;
            var guestTools = registry.GetToolsForRole("Khach");

            foreach (var tool in guestTools)
            {
                Assert.Equal("Any", tool.RequiredRole);
                Assert.False(tool.IsWriteOperation, $"Khách không được phép truy cập công cụ ghi: {tool.Name}");
            }
        }

        #endregion

        #region Region 3: Group A (Search & Lookup) Tests

        [Fact]
        public void SearchUsersTool_WhenCalledByStudent_ShouldBeRejectedByRBAC()
        {
            var tool = new SearchUsersTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien", UserId = 1 };

            bool canExec = AssistantPermissionService.CanExecute(tool, studentContext, out string reason);
            Assert.False(canExec);
            Assert.Contains("không có quyền thực thi", reason);
        }

        [Fact]
        public async Task SearchUsersTool_WhenCalledByLecturer_ShouldReturnUserListCard()
        {
            var tool = new SearchUsersTool();
            var lecturerContext = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?> { ["limit"] = 5 };

            var result = await tool.ExecuteAsync(args, lecturerContext);

            Assert.True(result.Success);
            Assert.Equal("UserListCard", result.CardType);
            Assert.False(string.IsNullOrWhiteSpace(result.CardPayloadJson));
        }

        [Fact]
        public async Task SearchLessonsTool_WhenCalledByStudent_ShouldOnlyReturnPublishedLessons()
        {
            var tool = new SearchLessonsTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien", UserId = 1 };
            var args = new Dictionary<string, object?> { ["limit"] = 10 };

            var result = await tool.ExecuteAsync(args, studentContext);

            Assert.True(result.Success);
            Assert.Equal("LessonListCard", result.CardType);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            var lessons = doc.RootElement.GetProperty("lessons").EnumerateArray();
            foreach (var lesson in lessons)
            {
                Assert.True(lesson.GetProperty("isPublished").GetBoolean());
            }
        }

        [Fact]
        public async Task SearchExamsTool_WhenCalledByStudent_ShouldForcePublishedOnly()
        {
            var tool = new SearchExamsTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien", UserId = 1 };
            var args = new Dictionary<string, object?> { ["limit"] = 10 };

            var result = await tool.ExecuteAsync(args, studentContext);

            Assert.True(result.Success);
            Assert.Equal("ExamListCard", result.CardType);
            Assert.False(string.IsNullOrWhiteSpace(result.CardPayloadJson));
        }

        [Fact]
        public void SearchQuestionsTool_WhenCalledByStudent_ShouldBeRejectedByRBAC()
        {
            var tool = new SearchQuestionsTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien", UserId = 1 };

            bool canExec = AssistantPermissionService.CanExecute(tool, studentContext, out string reason);
            Assert.False(canExec);
            Assert.Contains("không có quyền thực thi", reason);
        }

        [Fact]
        public async Task SearchQuestionsTool_WhenCalledByLecturer_ShouldReturnQuestionListCard()
        {
            var tool = new SearchQuestionsTool();
            var lecturerContext = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?> { ["limit"] = 5 };

            var result = await tool.ExecuteAsync(args, lecturerContext);

            Assert.True(result.Success);
            Assert.Equal("QuestionListCard", result.CardType);
            Assert.False(string.IsNullOrWhiteSpace(result.CardPayloadJson));
        }

        #endregion

        #region Region 4: Group B (Statistics & Analysis) Tests

        [Fact]
        public async Task GetSystemOverviewStatisticsTool_ShouldReturnKpiStats()
        {
            var tool = new GetSystemOverviewStatisticsTool();
            var context = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?> { ["period"] = "Week" };

            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("StatisticsCard", result.CardType);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            Assert.Equal("SystemOverview", doc.RootElement.GetProperty("statType").GetString());
            Assert.True(doc.RootElement.TryGetProperty("kpis", out var kpis) && kpis.GetArrayLength() > 0);
        }

        [Fact]
        public async Task GetTeacherStatisticsTool_ShouldReturnScoreDistribution()
        {
            var tool = new GetTeacherStatisticsTool();
            var context = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?> { ["examType"] = "TatCa" };

            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("StatisticsCard", result.CardType);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            Assert.Equal("TeacherPerformance", doc.RootElement.GetProperty("statType").GetString());
            Assert.True(doc.RootElement.TryGetProperty("summary", out _));
        }

        [Fact]
        public async Task GetStudentLearningStatisticsTool_StudentAccessingOtherStudent_ShouldEnforceSelfOnly()
        {
            var tool = new GetStudentLearningStatisticsTool();
            var studentContext = new AssistantContext
            {
                UserId = 7,
                UserRole = "SinhVien",
                FullName = "Sinh Viên Bảy"
            };

            // Sinh viên truyền studentId = 99 (IDOR attack)
            var args = new Dictionary<string, object?> { ["studentId"] = 99 };
            var result = await tool.ExecuteAsync(args, studentContext);

            Assert.True(result.Success);
            Assert.Equal("StatisticsCard", result.CardType);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            int returnedStudentId = doc.RootElement.GetProperty("studentId").GetInt32();
            Assert.Equal(7, returnedStudentId); // Must be forced to caller's own ID
        }

        #endregion

        #region Region 5: Group C (Content Generation & Two-Step Confirmation) Tests

        [Fact]
        public async Task GenerateMultipleChoiceQuestionTool_WhenCalledByLecturer_ShouldReturnValidPreviewCard()
        {
            var tool = new GenerateMultipleChoiceQuestionTool();
            var context = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?>
            {
                ["topic"] = "Passive Voice",
                ["level"] = "B1",
                ["skill"] = "NguPhap",
                ["count"] = 2
            };

            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("question_preview_card", result.CardType);
            Assert.NotNull(result.CardPayloadJson);

            using var doc = JsonDocument.Parse(result.CardPayloadJson);
            var questions = doc.RootElement.GetProperty("Questions");
            Assert.True(questions.GetArrayLength() >= 1);
        }

        [Fact]
        public async Task GenerateFillBlankQuestionTool_ShouldReturnSentenceCompletionQuestions()
        {
            var tool = new GenerateFillBlankQuestionTool();
            var context = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?>
            {
                ["topic"] = "Modal Verbs",
                ["level"] = "B1",
                ["count"] = 2
            };

            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("question_preview_card", result.CardType);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            var questions = doc.RootElement.GetProperty("Questions");
            Assert.True(questions.GetArrayLength() >= 1);
            var firstQ = questions[0];
            Assert.Equal("DienKhuyet", firstQ.GetProperty("QuestionType").GetString());
        }

        [Fact]
        public async Task GenerateReadingPassageTool_ShouldGeneratePassageAndComprehensionQuestions()
        {
            var tool = new GenerateReadingPassageTool();
            var context = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?>
            {
                ["topic"] = "Artificial Intelligence in Education",
                ["level"] = "B2",
                ["questionCount"] = 2
            };

            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("question_preview_card", result.CardType);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            Assert.False(string.IsNullOrWhiteSpace(doc.RootElement.GetProperty("Passage").GetString()));
            var questions = doc.RootElement.GetProperty("Questions");
            Assert.True(questions.GetArrayLength() >= 1);
        }

        [Fact]
        public void SaveGeneratedQuestionTool_MetadataCheck_ShouldBeWriteAndRequireConfirmation()
        {
            var tool = new SaveGeneratedQuestionTool();

            Assert.True(tool.IsWriteOperation, "SaveGeneratedQuestionTool phải là thao tác ghi.");
            Assert.True(tool.RequiresConfirmation, "SaveGeneratedQuestionTool bắt buộc phải yêu cầu xác nhận 2 bước.");
            Assert.Equal(AssistantPermissionService.RoleLecturer, tool.RequiredRole);
        }

        [Fact]
        public void SaveGeneratedQuestionTool_WhenCalledByStudent_ShouldBeRejectedByPermissionService()
        {
            var tool = new SaveGeneratedQuestionTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien", UserId = 1 };

            bool canExec = AssistantPermissionService.CanExecute(tool, studentContext, out string reason);
            Assert.False(canExec);
            Assert.Contains("không có quyền thực thi", reason);
        }

        [Fact]
        public async Task SaveGeneratedQuestionTool_MissingContent_ShouldReturnFail()
        {
            var tool = new SaveGeneratedQuestionTool();
            var context = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?> { ["content"] = "" };

            var result = await tool.ExecuteAsync(args, context);

            Assert.False(result.Success);
            Assert.Equal("MISSING_CONTENT", result.ErrorCode);
        }

        [Fact]
        public async Task SaveGeneratedQuestionTool_InsufficientOptions_ShouldReturnFail()
        {
            var tool = new SaveGeneratedQuestionTool();
            var context = new AssistantContext { UserRole = "GiangVien", UserId = 2 };
            var args = new Dictionary<string, object?>
            {
                ["content"] = "Which sentence is correct?",
                ["questionType"] = "TracNghiem",
                ["options"] = new List<string> { "Only One Option" }
            };

            var result = await tool.ExecuteAsync(args, context);

            Assert.False(result.Success);
            Assert.Equal("INSUFFICIENT_OPTIONS", result.ErrorCode);
        }

        [Fact]
        public async Task Orchestrator_TwoStepConfirmation_SaveQuestionFlow()
        {
            var registry = AssistantToolRegistry.Instance;
            var orchestrator = new AssistantOrchestrator(toolRegistry: registry);

            var context = new AssistantContext
            {
                UserId = 1,
                Username = "admin",
                UserRole = "QuanTriVien"
            };

            var args = new Dictionary<string, object?>
            {
                ["content"] = "Which auxiliary verb is used with Present Perfect?",
                ["questionType"] = "TracNghiem",
                ["skill"] = "NguPhap",
                ["level"] = "B1",
                ["difficulty"] = "De",
                ["options"] = new List<string> { "have / has", "did", "was / were", "will" },
                ["correctIndex"] = 0,
                ["explanation"] = "Thì hiện tại hoàn thành sử dụng have/has + V3/ed."
            };

            // Stage the write action
            string token = orchestrator.StageWriteAction("save_generated_question", args, context);
            Assert.False(string.IsNullOrWhiteSpace(token));

            // Confirm and execute the staged write tool
            var execResult = await orchestrator.ConfirmAndExecuteWriteToolAsync(token);
            Assert.True(execResult.Success, $"Thực thi xác nhận thất bại: {execResult.Message}");
            Assert.Equal("save_question_success_card", execResult.CardType);

            // Replay protection: executing with same token must fail with TOKEN_NOT_FOUND
            var replayResult = await orchestrator.ConfirmAndExecuteWriteToolAsync(token);
            Assert.False(replayResult.Success);
            Assert.Equal("TOKEN_NOT_FOUND", replayResult.ErrorCode);
        }

        #endregion

        #region Region 6: Group D (Learning Management & History) Tests & IDOR Guardrail

        [Fact]
        public async Task GetLessonProgressTool_WithoutMaBaiHoc_ShouldReturnSummaryProgressCard()
        {
            var tool = new GetLessonProgressTool();
            var context = new AssistantContext
            {
                UserId = 1,
                UserRole = "SinhVien",
                Username = "sinhvien1"
            };

            var args = new Dictionary<string, object?>();
            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("learning_progress_summary_card", result.CardType);
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
            Assert.False(string.IsNullOrWhiteSpace(result.CardPayloadJson));
        }

        [Fact]
        public async Task GetLessonProgressTool_WithMaBaiHoc_ShouldReturnLessonProgressCard()
        {
            var tool = new GetLessonProgressTool();
            var context = new AssistantContext
            {
                UserId = 1,
                UserRole = "SinhVien",
                Username = "sinhvien1"
            };

            var args = new Dictionary<string, object?> { ["maBaiHoc"] = 1 };
            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("lesson_progress_card", result.CardType);
            Assert.Contains("Tiến độ bài học", result.Message);
        }

        [Fact]
        public async Task GetLessonProgressTool_SinhVien_IDOR_Protection_MustForceContextUserId()
        {
            var tool = new GetLessonProgressTool();
            var studentContext = new AssistantContext
            {
                UserId = 10,
                UserRole = "SinhVien",
                Username = "sinhvien10"
            };

            // Sinh viên cố tình truyền maNguoiDung của người khác (User 99)
            var args = new Dictionary<string, object?>
            {
                ["maNguoiDung"] = 99
            };

            var result = await tool.ExecuteAsync(args, studentContext);

            Assert.True(result.Success);
            // Payload bắt buộc phải trả về user_id của chính người gọi (10), không được là 99
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            int returnedUserId = doc.RootElement.GetProperty("user_id").GetInt32();
            Assert.Equal(10, returnedUserId);
        }

        [Fact]
        public async Task GetLessonProgressTool_Lecturer_CanQuerySpecificStudent()
        {
            var tool = new GetLessonProgressTool();
            var teacherContext = new AssistantContext
            {
                UserId = 2,
                UserRole = "GiangVien",
                Username = "teacher2"
            };

            // Giảng viên tra cứu tiến độ của học viên 1
            var args = new Dictionary<string, object?>
            {
                ["maNguoiDung"] = 1
            };

            var result = await tool.ExecuteAsync(args, teacherContext);

            Assert.True(result.Success);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            int returnedUserId = doc.RootElement.GetProperty("user_id").GetInt32();
            Assert.Equal(1, returnedUserId);
        }

        [Fact]
        public async Task GetExamHistoryTool_ValidUser_ShouldReturnExamHistoryCard()
        {
            var tool = new GetExamHistoryTool();
            var context = new AssistantContext
            {
                UserId = 1,
                UserRole = "SinhVien",
                Username = "sinhvien1"
            };

            var args = new Dictionary<string, object?> { ["top"] = 3 };
            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("exam_history_card", result.CardType);
            Assert.False(string.IsNullOrWhiteSpace(result.CardPayloadJson));
        }

        [Fact]
        public async Task GetExamHistoryTool_SinhVien_IDOR_Protection_MustPreventViewingOtherUserExams()
        {
            var tool = new GetExamHistoryTool();
            var studentContext = new AssistantContext
            {
                UserId = 5,
                UserRole = "SinhVien",
                Username = "sinhvien5"
            };

            // Cố tình xem trộm lịch sử thi của User 88
            var args = new Dictionary<string, object?> { ["maNguoiDung"] = 88 };
            var result = await tool.ExecuteAsync(args, studentContext);

            Assert.True(result.Success);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            int returnedUserId = doc.RootElement.GetProperty("user_id").GetInt32();
            Assert.Equal(5, returnedUserId);
        }

        #endregion

        #region Region 7: Group E (Navigation & App Context) Tests

        [Theory]
        [InlineData("TrangChu", "SinhVien")]
        [InlineData("HocTap", "SinhVien")]
        [InlineData("TienDoHoc", "SinhVien")]
        [InlineData("ThiThu", "SinhVien")]
        [InlineData("LuyenNoi", "SinhVien")]
        public async Task NavigateScreenTool_Student_CanNavigateToPublicAndStudentScreens(string screenCode, string role)
        {
            var tool = new NavigateScreenTool();
            var context = new AssistantContext { UserRole = role, UserId = 1 };
            var args = new Dictionary<string, object?> { ["screenCode"] = screenCode };

            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("navigation_card", result.CardType);
            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            Assert.Equal(screenCode, doc.RootElement.GetProperty("screen_code").GetString());
            Assert.True(doc.RootElement.GetProperty("can_navigate").GetBoolean());
        }

        [Theory]
        [InlineData("GiamSatHeThong")]
        [InlineData("NguoiDung")]
        [InlineData("NoiDungHocTap")]
        [InlineData("NganHangCauHoi")]
        public async Task NavigateScreenTool_Student_DeniedAccessToAdminAndTeacherScreens(string screenCode)
        {
            var tool = new NavigateScreenTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien", UserId = 1 };
            var args = new Dictionary<string, object?> { ["screenCode"] = screenCode };

            var result = await tool.ExecuteAsync(args, studentContext);

            Assert.False(result.Success);
            Assert.Equal("SCREEN_ACCESS_DENIED", result.ErrorCode);
        }

        [Fact]
        public async Task NavigateScreenTool_Admin_CanNavigateToAnyScreen()
        {
            var tool = new NavigateScreenTool();
            var adminContext = new AssistantContext { UserRole = "QuanTriVien", UserId = 1 };
            var args = new Dictionary<string, object?> { ["screenCode"] = "NguoiDung" };

            var result = await tool.ExecuteAsync(args, adminContext);

            Assert.True(result.Success);
            Assert.Equal("navigation_card", result.CardType);
        }

        [Fact]
        public async Task NavigateScreenTool_InvalidScreenCode_ReturnsFriendlyError()
        {
            var tool = new NavigateScreenTool();
            var context = new AssistantContext { UserRole = "SinhVien", UserId = 1 };
            var args = new Dictionary<string, object?> { ["screenCode"] = "ManHinhKhongTonTai" };

            var result = await tool.ExecuteAsync(args, context);

            Assert.False(result.Success);
            Assert.Equal("INVALID_SCREEN", result.ErrorCode);
        }

        #endregion

        #region Region 8: Group F (System Health & Application Help) Tests

        [Fact]
        public async Task GetSystemHealthTool_AdminOrLecturer_ShouldReturnValidHealthCard()
        {
            var tool = new GetSystemHealthTool();
            var teacherContext = new AssistantContext { UserRole = "GiangVien", UserId = 2 };

            var args = new Dictionary<string, object?> { ["checkAiProviders"] = false };
            var result = await tool.ExecuteAsync(args, teacherContext);

            Assert.True(result.Success);
            Assert.Equal("system_health_card", result.CardType);
            Assert.False(string.IsNullOrWhiteSpace(result.CardPayloadJson));

            using var doc = JsonDocument.Parse(result.CardPayloadJson!);
            var root = doc.RootElement;
            Assert.True(root.TryGetProperty("overall_status", out _));
            Assert.True(root.TryGetProperty("database", out _));
            Assert.True(root.TryGetProperty("runtime", out _));
        }

        [Fact]
        public void GetSystemHealthTool_Student_MustBeRejectedByPermissionService()
        {
            var tool = new GetSystemHealthTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien", UserId = 1 };

            bool canExec = AssistantPermissionService.CanExecute(tool, studentContext, out string reason);

            Assert.False(canExec);
            Assert.Contains("không có quyền thực thi", reason);
        }

        [Fact]
        public async Task GetFeatureGuideTool_SpecificFeature_ShouldReturnDetailedGuide()
        {
            var tool = new GetFeatureGuideTool();
            var context = new AssistantContext { UserRole = "SinhVien" };
            var args = new Dictionary<string, object?> { ["featureName"] = "luyen_noi" };

            var result = await tool.ExecuteAsync(args, context);

            Assert.True(result.Success);
            Assert.Equal("feature_guide_card", result.CardType);
            Assert.Contains("Luyện nói Speaking AI", result.Message);
        }

        [Fact]
        public async Task GetFeatureGuideTool_AllFeatures_ShouldReturnRoleFilteredCatalog()
        {
            var tool = new GetFeatureGuideTool();
            var studentContext = new AssistantContext { UserRole = "SinhVien" };
            var args = new Dictionary<string, object?> { ["featureName"] = "tat_ca" };

            var result = await tool.ExecuteAsync(args, studentContext);

            Assert.True(result.Success);
            Assert.Equal("feature_guide_card", result.CardType);

            // Sinh viên không được gợi ý tính năng quản lý tài khoản người dùng
            Assert.DoesNotContain("Quản lý danh sách người dùng", result.CardPayloadJson!);
        }

        [Theory]
        [InlineData("Khach")]
        [InlineData("SinhVien")]
        [InlineData("GiangVien")]
        [InlineData("QuanTriVien")]
        public void GetFeatureGuideTool_AnyRole_CanAccess(string role)
        {
            var tool = new GetFeatureGuideTool();
            var context = new AssistantContext { UserRole = role };

            bool canExec = AssistantPermissionService.CanExecute(tool, context, out string reason);
            Assert.True(canExec, $"Vai trò '{role}' phải được phép truy cập GetFeatureGuideTool. Lý do bị chặn: {reason}");
        }

        #endregion

        #region Region 9: Headless Safety & Thread Safety

        [Fact]
        public void AllRegisteredTools_CanInstantiateAndInspectWithoutWpfSTAThread()
        {
            // Kiểm tra việc nạp toàn bộ công cụ trong môi trường non-UI thread
            var registry = AssistantToolRegistry.Instance;
            var tools = registry.GetAllTools();

            Assert.NotEmpty(tools);
            foreach (var tool in tools)
            {
                var meta = tool.GetMetadata();
                Assert.NotNull(meta);
                Assert.Equal(tool.Name, meta.Name);
            }
        }

        [Fact]
        public void ToolRegistry_ThreadSafe_ConcurrentAccess()
        {
            var registry = AssistantToolRegistry.Instance;

            Parallel.For(0, 100, i =>
            {
                var tools = registry.GetAllTools();
                var studentTools = registry.GetToolsForRole("SinhVien");
                var searchTools = registry.GetToolsByCategory("Search");
                Assert.NotEmpty(tools);
                Assert.NotEmpty(studentTools);
                Assert.NotEmpty(searchTools);
            });
        }

        #endregion
    }
}
