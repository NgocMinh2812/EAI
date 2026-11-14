using System;
using System.Linq;
using System.Threading.Tasks;
using BUS;
using BUS.AI;
using BUS.AI.Configuration;
using BUS.AI.Models;
using DTO;
using Xunit;

namespace EAI.Tests
{
    public class GiamSatHeThongTests
    {
        [Fact]
        public void KiemTraKetNoiDatabase_ShouldReturnHealthyStatusAndValidLatency()
        {
            var dashboardBus = new DashboardBUS();
            var (isConnected, latencyMs, message) = dashboardBus.KiemTraKetNoiDatabase();

            Assert.True(isConnected, "Database SQL Server TiengAnhAI phải kết nối thành công.");
            Assert.True(latencyMs >= 0, "Độ trễ truy vấn database phải là số không âm.");
            Assert.False(string.IsNullOrWhiteSpace(message));
        }

        [Theory]
        [InlineData("AIzaSyD-ABC123456789XYZ_1234567890ABCDE", "AIza••••••••BCDE")]
        [InlineData("gsk_1234567890abcdef1234567890abcdef12", "gsk_••••••••ef12")]
        [InlineData("xkiro_secret_token_12345678901234567890", "xkir••••••••7890")]
        public void MaskApiKey_LongKey_MustMaskMiddleAndPreserveSecurity(string originalKey, string expectedMasked)
        {
            var masked = AIConfigHelper.MaskKey(originalKey);
            Assert.Equal(expectedMasked, masked);
            Assert.DoesNotContain("ABC123456789XYZ", masked);
            Assert.DoesNotContain("abcdef1234567890", masked);
        }

        [Theory]
        [InlineData(null, "Chưa cấu hình")]
        [InlineData("", "Chưa cấu hình")]
        [InlineData("   ", "Chưa cấu hình")]
        [InlineData("short", "••••••••")]
        public void MaskApiKey_InvalidOrShortKey_MustReturnSafeFallback(string? key, string expected)
        {
            var masked = AIConfigHelper.MaskKey(key!);
            Assert.Equal(expected, masked);
        }

        [Fact]
        public void CheckAdminPermission_NonAdminUser_MustBeDenied()
        {
            // Setup session as student
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 999,
                TenDangNhap = "hocvien_test",
                TenVaiTro = "HocVien",
                MaVaiTro = 2
            };

            var canAccess = AIConfigurationService.Instance.CheckAdminPermission();
            Assert.False(canAccess, "Học viên thông thường không được phép thay đổi cấu hình AI hệ thống.");

            // Reset session
            SessionHelper.CurrentUser = null;
        }

        [Fact]
        public void CheckAdminPermission_AdminOrGiangVien_MustBeAllowed()
        {
            // Setup session as GiangVien
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 1,
                TenDangNhap = "giangvien_admin",
                TenVaiTro = "GiangVien",
                MaVaiTro = 3
            };

            var canAccess = AIConfigurationService.Instance.CheckAdminPermission();
            Assert.True(canAccess, "Giảng viên / Quản trị viên phải có quyền quản lý giám sát hệ thống.");

            // Reset session
            SessionHelper.CurrentUser = null;
        }

        [Fact]
        public void AIConfigurationService_GetSupportedModels_MustReturnNonEmptyListsForAllProviders()
        {
            var configService = AIConfigurationService.Instance;

            var geminiModels = configService.GetSupportedModels(AIProviderType.Gemini);
            Assert.NotEmpty(geminiModels);
            Assert.Contains(geminiModels, m => m.Id.Contains("gemini", StringComparison.OrdinalIgnoreCase));

            var groqModels = configService.GetSupportedModels(AIProviderType.Groq);
            Assert.NotEmpty(groqModels);
            Assert.Contains(groqModels, m => m.Id.Contains("llama", StringComparison.OrdinalIgnoreCase) || m.Id.Contains("qwen", StringComparison.OrdinalIgnoreCase));

            var xkioModels = configService.GetSupportedModels(AIProviderType.Xkio);
            Assert.NotEmpty(xkioModels);
            Assert.Contains(xkioModels, m => m.Id.Contains("qwen", StringComparison.OrdinalIgnoreCase) || m.Id.Contains("cohere", StringComparison.OrdinalIgnoreCase) || m.Id.Contains("nvidia", StringComparison.OrdinalIgnoreCase));

            var ollamaModels = configService.GetSupportedModels(AIProviderType.Ollama);
            Assert.NotEmpty(ollamaModels);
            Assert.Contains(ollamaModels, m => m.Id.Contains("llama", StringComparison.OrdinalIgnoreCase) || m.Id.Contains("qwen", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void OllamaProvider_MustBeRegisteredAndConfiguredForLocalUse()
        {
            var provider = AIServiceManager.Instance.GetProvider(AIProviderType.Ollama);
            Assert.NotNull(provider);
            Assert.Equal("Ollama (Offline/Local LLM)", provider.ProviderName);
            Assert.True(provider.IsConfigured, "Ollama chạy cục bộ nên IsConfigured phải luôn là true.");
            Assert.False(string.IsNullOrWhiteSpace(provider.DefaultModel));
        }

        [Fact]
        public async Task OllamaProvider_WhenServerOffline_ReturnsHelpfulGuidance()
        {
            var provider = AIServiceManager.Instance.GetProvider(AIProviderType.Ollama);
            var health = await provider.CheckHealthAsync();
            Assert.NotNull(health);
            Assert.Equal(AIProviderType.Ollama, health.ProviderType);
            Assert.False(string.IsNullOrWhiteSpace(health.Message));
        }

        [Fact]
        public void AIServiceManager_Metrics_RecordRequest_MustUpdateLogStreamAndStatusColors()
        {
            var metrics = AIServiceManager.Instance.Metrics;
            var initialRequests = metrics.TotalRequests;

            // Record successful request
            metrics.Record(new AIRequestLog
            {
                Provider = "Google Gemini",
                Model = "gemini-2.0-flash",
                LatencyMs = 125,
                Success = true,
                ActionName = "Test Unit Request",
                PromptPreview = "Generate 5 questions for B1",
                IsFallback = false,
                Timestamp = DateTime.Now
            });

            Assert.True(metrics.TotalRequests >= initialRequests + 1);

            var recentLogs = metrics.GetRecentLogs();
            Assert.NotEmpty(recentLogs);

            var latestSuccess = recentLogs.FirstOrDefault(l => l.ActionName == "Test Unit Request");
            Assert.NotNull(latestSuccess);
            Assert.Equal("POST 200 OK", latestSuccess.StatusBadgeText);
            Assert.Equal("#DCFCE7", latestSuccess.StatusBadgeBg);
            Assert.Equal("#15803D", latestSuccess.StatusBadgeFg);
            Assert.False(latestSuccess.IsFallback);

            // Record failed request with fallback
            metrics.Record(new AIRequestLog
            {
                Provider = "Groq Cloud",
                Model = "llama-3.3-70b-versatile",
                LatencyMs = 450,
                Success = false,
                StatusCode = 429,
                ActionName = "Failed Request with Fallback",
                PromptPreview = "Evaluate essay",
                IsFallback = true,
                ErrorMessage = "Quota Exceeded (HTTP 429)",
                Timestamp = DateTime.Now
            });

            var recentLogsAfterFail = metrics.GetRecentLogs();
            var latestFail = recentLogsAfterFail.FirstOrDefault(l => l.ActionName == "Failed Request with Fallback");
            Assert.NotNull(latestFail);
            Assert.Equal("ERR 429", latestFail.StatusBadgeText);
            Assert.Equal("#FEE2E2", latestFail.StatusBadgeBg);
            Assert.Equal("#DC2626", latestFail.StatusBadgeFg);
            Assert.True(latestFail.IsFallback);
            Assert.Equal("Quota Exceeded (HTTP 429)", latestFail.ErrorMessage);
        }

        [Fact]
        public void AISinhDeBUS_DemTongCauHoiDoAISinh_ShouldReturnNonNegativeNumber()
        {
            var totalAiQuestions = AISinhDeBUS.Instance.DemTongCauHoiDoAISinh();
            Assert.True(totalAiQuestions >= 0, "Số lượng câu hỏi do AI tạo phải là số không âm.");
        }

        [Fact]
        public void AIConfigurationService_SaveActiveConfig_ShouldPersistWhenAuthorized()
        {
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 0,
                TenDangNhap = "admin_supervisor",
                TenVaiTro = "QuanTriVien",
                MaVaiTro = 2
            };

            var configService = AIConfigurationService.Instance;
            var originalProvider = configService.ActiveProvider;
            var originalModel = configService.ActiveModel;

            // Save test config
            configService.SaveActiveConfig(AIProviderType.Groq, "llama-3.3-70b-versatile", true);

            Assert.Equal(AIProviderType.Groq, configService.ActiveProvider);
            Assert.Equal("llama-3.3-70b-versatile", configService.ActiveModel);

            // Restore original config
            configService.SaveActiveConfig(originalProvider, originalModel, true);
            SessionHelper.CurrentUser = null;
        }
    }
}
