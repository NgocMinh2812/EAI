using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using BUS.AI;
using BUS.AI.Configuration;
using BUS.AI.Factory;
using BUS.AI.Models;
using BUS.AI.Providers;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EAI.Tests;

[CollectionDefinition("Backend configuration", DisableParallelization = true)]
public class BackendConfigurationCollection { }

[Collection("Backend configuration")]
public class CentralizedBackendTests
{
    [Fact]
    public async Task Prompt_GoesStraightToBackend_WithoutHealthPreflight()
    {
        using var server = new BackendStub(200, """{"data":{"message":"OK"}}""");
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var result = await AIServiceManager.Instance.ExecutePromptAsync("Hello", ct: timeout.Token);

        Assert.True(result.Success);
        Assert.Equal("OK", result.Text);
        Assert.Equal("EAI.AIBackend", result.ProviderName);
        Assert.StartsWith("POST /api/ai/chat ", await server.RequestLine);
    }

    [Fact]
    public async Task BackendFailure_IsReported_WithoutInvokingLocalProviders()
    {
        using var server = new BackendStub(503, """{"message":"Service unavailable"}""");
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var manager = AIServiceManager.Instance;
        var originalProviders = manager.GetAllProviders().ToArray();
        var spies = originalProviders.Select(p => new LocalProviderSpy(p.ProviderType)).ToArray();
        foreach (var spy in spies) manager.RegisterProvider(spy);
        try
        {
            var result = await manager.ExecutePromptAsync("Hello", ct: timeout.Token);
            Assert.False(result.Success);
            Assert.Equal("EAI.AIBackend", result.ProviderName);
            Assert.Equal(503, result.StatusCode);
            Assert.DoesNotContain("API Key", result.ErrorMessage ?? "");
            Assert.All(spies, p => Assert.Equal(0, p.Calls));
            Assert.StartsWith("POST /api/ai/chat ", await server.RequestLine);
        }
        finally
        {
            foreach (var provider in originalProviders) manager.RegisterProvider(provider);
        }
    }

    [Theory]
    [InlineData(AIProviderType.Gemini, "Gemini")]
    [InlineData(AIProviderType.Groq, "Groq")]
    [InlineData(AIProviderType.Xkio, "XKiro")]
    public async Task ProviderHealth_UsesServerConfiguration(AIProviderType type, string backendName)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(new
        {
            data = new { providers = new[] { new { providerName = backendName, totalKeys = 2, activeKeys = 1, isAvailable = true } } }
        });
        using var server = new BackendStub(200, json);
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var status = await AIProviderFactory.Instance.CreateProvider(type).CheckHealthAsync(timeout.Token);

        Assert.True(status.IsConfigured);
        Assert.Equal("Healthy", status.Health);
        Assert.Contains("Backend", status.Message);
        Assert.StartsWith("GET /api/system/status ", await server.RequestLine);
    }

    // Máy chủ HTTP cục bộ, không dùng Azure, key thật, quota hay database.
    private sealed class BackendStub : IDisposable
    {
        private readonly TcpListener _listener = new(IPAddress.Loopback, 0);
        private readonly CancellationTokenSource _timeout = new(TimeSpan.FromSeconds(8));
        private readonly FieldInfo _configuration = typeof(AIConfigHelper)
            .GetField("_configuration", BindingFlags.NonPublic | BindingFlags.Static)!;
        private readonly object? _previousConfiguration;
        public Task<string> RequestLine { get; }

        public BackendStub(int status, string body)
        {
            _previousConfiguration = _configuration.GetValue(null);
            _listener.Start();
            var port = ((IPEndPoint)_listener.LocalEndpoint).Port;
            _configuration.SetValue(null, new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["AIBackend:Enabled"] = "true",
                    ["AIBackend:BaseUrl"] = $"http://127.0.0.1:{port}"
                }).Build());
            RequestLine = ServeAsync(status, body);
        }

        private async Task<string> ServeAsync(int status, string body)
        {
            using var client = await _listener.AcceptTcpClientAsync(_timeout.Token);
            await using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, leaveOpen: true);
            var requestLine = await reader.ReadLineAsync(_timeout.Token) ?? "";
            var contentLength = 0;
            string? header;
            while (!string.IsNullOrEmpty(header = await reader.ReadLineAsync(_timeout.Token)))
                if (header.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
                    int.TryParse(header[15..].Trim(), out contentLength);
            if (contentLength > 0)
            {
                var content = new char[contentLength];
                await reader.ReadBlockAsync(content.AsMemory(), _timeout.Token);
            }
            var payload = Encoding.UTF8.GetBytes(body);
            var headers = Encoding.ASCII.GetBytes($"HTTP/1.1 {status} Test\r\nContent-Type: application/json\r\nContent-Length: {payload.Length}\r\nConnection: close\r\n\r\n");
            await stream.WriteAsync(headers, _timeout.Token);
            await stream.WriteAsync(payload, _timeout.Token);
            await stream.FlushAsync(_timeout.Token);
            return requestLine;
        }

        public void Dispose()
        {
            _configuration.SetValue(null, _previousConfiguration);
            _timeout.Cancel();
            _listener.Stop();
            _timeout.Dispose();
        }
    }

    private sealed class LocalProviderSpy(AIProviderType type) : IAIProvider
    {
        public int Calls { get; private set; }
        public AIProviderType ProviderType => type;
        public string ProviderName => "Local spy";
        public bool IsConfigured => true;
        public IReadOnlyList<string> SupportedModels => Array.Empty<string>();
        public string DefaultModel => "test";
        public bool IsCatalogDynamic => false;
        public string CatalogStatusMessage => "test";
        public Task<AIResponse> GenerateContentAsync(string prompt, string? model = null, CancellationToken ct = default)
        {
            Calls++;
            throw new InvalidOperationException("Local provider must not be called in Backend mode.");
        }
        public Task<AIHealthStatus> CheckHealthAsync(CancellationToken ct = default) => throw new NotSupportedException();
        public Task<IReadOnlyList<AIModelInfo>> GetModelsAsync(bool forceRefresh = false, CancellationToken ct = default) => throw new NotSupportedException();
    }
}
