using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Services;
using Xunit;

namespace EAI.Tests
{
    public class OllamaProcessServiceTests
    {
        private readonly IOllamaProcessService _service = OllamaProcessService.Instance;

        [Fact]
        public void FindExecutablePath_WhenOllamaInstalled_ReturnsValidAndExistingPath()
        {
            // Act
            string? exePath = _service.FindExecutablePath();

            // Assert
            Assert.NotNull(exePath);
            Assert.NotEmpty(exePath);
            Assert.True(File.Exists(exePath), $"Tệp thực thi {exePath} phải tồn tại trên ổ cứng.");
            Assert.EndsWith("ollama.exe", exePath, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void IsInstalled_OnDevelopmentHost_ReturnsTrue()
        {
            // Act
            bool isInstalled = _service.IsInstalled();

            // Assert
            Assert.True(isInstalled, "Ollama phải được phát hiện đã cài đặt trên môi trường phát triển.");
        }

        [Theory]
        [InlineData(0, "0 MB")]
        [InlineData(-100, "0 MB")]
        [InlineData(52428800, "50.0 MB")]               // 50 MB
        [InlineData(85458944, "81.5 MB")]               // ~81.5 MB
        [InlineData(1073741824, "1.00 GB")]             // 1 GB
        [InlineData(2748779069, "2.56 GB")]             // ~2.56 GB
        public void FormatBytes_VariousInputs_FormatsCorrectly(long inputBytes, string expectedFormatted)
        {
            // Act
            string actual = _service.FormatBytes(inputBytes);

            // Assert
            Assert.Equal(expectedFormatted, actual);
        }

        [Fact]
        public void GetTotalWorkingSetBytes_ReturnsNonNegative()
        {
            // Act
            long ramBytes = _service.GetTotalWorkingSetBytes();

            // Assert
            Assert.True(ramBytes >= 0, "Dung lượng RAM trả về không được là số âm.");
        }

        [Fact]
        public async Task GetDetailedStatusAsync_ReturnsPopulatedStatusInfo()
        {
            // Act
            var status = await _service.GetDetailedStatusAsync();

            // Assert
            Assert.NotNull(status);
            Assert.NotNull(status.EndpointUrl);
            Assert.StartsWith("http://localhost", status.EndpointUrl);
            Assert.NotNull(status.RamUsageFormatted);
            Assert.NotEmpty(status.RamUsageFormatted);
            Assert.True(status.RamBytes >= 0);

            if (status.IsRunning)
            {
                Assert.True(status.ProcessCount > 0 || status.RamBytes > 0);
            }
            else
            {
                Assert.Contains("Đã dừng", status.RamUsageFormatted);
            }
        }

        [Fact]
        public void OllamaStatusInfo_IsInstalledProperty_EvaluatesFileExistenceCorrectly()
        {
            // Arrange 1: Path exists
            string tempValidFile = Path.GetTempFileName();
            try
            {
                var infoValid = new OllamaStatusInfo { ExecutablePath = tempValidFile };
                Assert.True(infoValid.IsInstalled);

                // Arrange 2: Path null or not exists
                var infoNull = new OllamaStatusInfo { ExecutablePath = null };
                Assert.False(infoNull.IsInstalled);

                var infoFake = new OllamaStatusInfo { ExecutablePath = @"C:\NonExistentFolder_12345\ollama.exe" };
                Assert.False(infoFake.IsInstalled);
            }
            finally
            {
                if (File.Exists(tempValidFile))
                {
                    File.Delete(tempValidFile);
                }
            }
        }

        [Fact]
        public async Task StartServerAsync_WhenAlreadyRunning_ReturnsSuccessQuickly()
        {
            // Nếu Ollama đang chạy, gọi StartServerAsync phải trả về Success mà không tạo tiến trình thừa
            if (_service.IsProcessRunning())
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var (success, message) = await _service.StartServerAsync(cts.Token);

                Assert.True(success);
                Assert.Contains("đang hoạt động", message, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // Nếu đang tắt, kiểm tra đường dẫn cài đặt
                Assert.True(_service.IsInstalled());
            }
        }

        [Fact]
        public async Task CustomHttpClient_WhenServerOffline_HandlesGracefullyWithoutThrowing()
        {
            // Tạo một HttpClient trỏ vào cổng không tồn tại để kiểm tra độ bền ngoại lệ
            var mockHandler = new MockOfflineHttpMessageHandler();
            var customHttp = new HttpClient(mockHandler) { Timeout = TimeSpan.FromMilliseconds(500) };
            var customService = new OllamaProcessService(customHttp);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var status = await customService.GetDetailedStatusAsync(cts.Token);

            Assert.NotNull(status);
            Assert.False(status.LoadedModelCount > 0);
        }

        private class MockOfflineHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                throw new HttpRequestException("Cổng kết nối bị từ chối (Mô phỏng máy chủ ngoại tuyến)");
            }
        }
    }
}
