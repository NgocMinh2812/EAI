using System;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace BUS.AI.Configuration
{
    public static class AIConfigHelper
    {
        public const string UserSecretsId = "eai-desktop-app-secrets";
        private static IConfiguration? _configuration;

        /// <summary>
        /// Configuration pipeline hợp nhất của ứng dụng:
        /// 1. Nạp .NET User Secrets từ %APPDATA%\Microsoft\UserSecrets\eai-desktop-app-secrets\secrets.json
        /// 2. Nạp Biến môi trường hệ thống (Environment Variables)
        /// Tuyệt đối KHÔNG đọc hay fallback tới file secrets.json trong repository!
        /// </summary>
        public static IConfiguration Configuration => _configuration ??= BuildConfiguration();

        public static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();

            // 1. Biến môi trường hệ thống (Process scope)
            try
            {
                builder.AddEnvironmentVariables();
            }
            catch { }

            // 2. .NET User Secrets
            // Đặt UserSecrets sau để các cấu hình trong Manage User Secrets ghi đè lên các biến Process cũ
            try
            {
                builder.AddUserSecrets(UserSecretsId);
            }
            catch { }

            return builder.Build();
        }

        public static void ReloadConfiguration()
        {
            _configuration = BuildConfiguration();
        }

        /// <summary>
        /// Đọc biến môi trường an toàn từ Windows: Ưu tiên User scope (cập nhật tức thì từ Registry khi đổi trong Windows Settings)
        /// rồi đến Machine scope, cuối cùng mới tới Process scope (tránh giữ key cũ khi chưa restart IDE/Visual Studio).
        /// </summary>
        public static string? GetEnvVar(string name)
        {
            try
            {
                string? userVal = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
                if (!string.IsNullOrWhiteSpace(userVal)) return userVal.Trim();
            }
            catch { }

            try
            {
                string? machineVal = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
                if (!string.IsNullOrWhiteSpace(machineVal)) return machineVal.Trim();
            }
            catch { }

            try
            {
                string? procVal = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
                if (!string.IsNullOrWhiteSpace(procVal)) return procVal.Trim();
            }
            catch { }

            return null;
        }

        public static string GetGeminiKey(string? module = null)
        {
            var config = Configuration;

            // 1. Ưu tiên cấu hình User Secrets (Manage User Secrets)
            // 1a. Cấu trúc lồng chuẩn: Gemini:ApiKey
            string? key = config["Gemini:ApiKey"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            // 1b. Module key nếu có (Gemini:NoiDungHocTap, Gemini:DanhMuc...)
            if (!string.IsNullOrWhiteSpace(module))
            {
                key = config[$"Gemini:{module}"];
                if (!string.IsNullOrWhiteSpace(key)) return key.Trim();
            }

            key = config["Gemini:NoiDungHocTap"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            key = config["Gemini:DanhMucCauTruc"] ?? config["Gemini:DanhMuc"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            // 1c. Flat key: GEMINI_API_KEY hoặc GOOGLE_API_KEY
            key = config["GEMINI_API_KEY"] ?? config["GOOGLE_API_KEY"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            // 2. Biến môi trường hệ thống (User target -> Machine target -> Process target)
            if (!string.IsNullOrWhiteSpace(module))
            {
                string modEnvVar = $"GEMINI_API_KEY_{module.ToUpperInvariant()}";
                string? modEnvVal = GetEnvVar(modEnvVar);
                if (!string.IsNullOrWhiteSpace(modEnvVal)) return modEnvVal.Trim();
            }

            string? envKey = GetEnvVar("GEMINI_API_KEY") ?? GetEnvVar("GOOGLE_API_KEY");
            if (!string.IsNullOrWhiteSpace(envKey)) return envKey.Trim();

            return string.Empty;
        }

        public static string GetGroqKey()
        {
            var config = Configuration;

            // 1. Ưu tiên .NET User Secrets (chuẩn lồng Groq:ApiKey hoặc flat GROQ_API_KEY)
            string? key = config["Groq:ApiKey"] ?? config["GROQ_API_KEY"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            // 2. Biến môi trường hệ thống (User -> Machine -> Process)
            string? envKey = GetEnvVar("GROQ_API_KEY");
            if (!string.IsNullOrWhiteSpace(envKey)) return envKey.Trim();

            return string.Empty;
        }

        public static (string ApiKey, string BaseUrl) GetXkioConfig()
        {
            var config = Configuration;

            // 1. Ưu tiên .NET User Secrets
            string? key = config["Xkio:ApiKey"] 
                       ?? config["xKiro:ApiKey"] 
                       ?? config["XKIO_API_KEY"] 
                       ?? config["XKIRO_API_KEY"];

            string? url = config["Xkio:BaseUrl"] 
                       ?? config["xKiro:BaseUrl"] 
                       ?? config["XKIO_API_ENDPOINT"] 
                       ?? config["XKIRO_API_ENDPOINT"]
                       ?? config["XKIO_BASE_URL"] 
                       ?? config["XKIRO_BASE_URL"];

            // 2. Biến môi trường hệ thống (User -> Machine -> Process)
            if (string.IsNullOrWhiteSpace(key))
            {
                key = GetEnvVar("XKIO_API_KEY") ?? GetEnvVar("XKIRO_API_KEY");
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                url = GetEnvVar("XKIO_API_ENDPOINT") 
                   ?? GetEnvVar("XKIRO_API_ENDPOINT") 
                   ?? GetEnvVar("XKIO_BASE_URL") 
                   ?? GetEnvVar("XKIRO_BASE_URL");
            }

            key ??= "";
            url ??= "";

            // Mặc định tên miền chính thức của dịch vụ xKiro AI Gateway
            if (string.IsNullOrWhiteSpace(url) || url.Contains("api.xkio.ai", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://api.xkiro.com/v1";
            }

            return (key.Trim(), url.Trim());
        }

        /// <summary>
        /// Ghép Base URL với đường dẫn con, xử lý chuẩn hóa để tránh thiếu hoặc lặp '/v1'
        /// </summary>
        public static string BuildEndpoint(string baseUrl, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = "https://api.xkiro.com/v1";
            string b = baseUrl.Trim().TrimEnd('/');
            string p = (relativePath ?? "").Trim().TrimStart('/');

            // Nếu base đã có /v1 và path bắt đầu bằng v1/, loại bỏ 1 cái để tránh /v1/v1
            if (b.EndsWith("/v1", StringComparison.OrdinalIgnoreCase) && p.StartsWith("v1/", StringComparison.OrdinalIgnoreCase))
            {
                p = p.Substring(3);
            }
            // Nếu cả base và path đều chưa có v1, tự động thêm /v1
            else if (!b.EndsWith("/v1", StringComparison.OrdinalIgnoreCase) && !p.StartsWith("v1/", StringComparison.OrdinalIgnoreCase) && !p.Equals("v1", StringComparison.OrdinalIgnoreCase))
            {
                p = "v1/" + p;
            }

            return $"{b}/{p}";
        }

        /// <summary>
        /// Đường dẫn lưu tệp cấu hình mô hình AI đang hoạt động
        /// </summary>
        private static string GetActiveConfigFilePath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string localPath = Path.Combine(baseDir, "ai_active_config.json");
            try
            {
                string testFile = Path.Combine(baseDir, ".test_perm");
                File.WriteAllText(testFile, "ok");
                File.Delete(testFile);
                return localPath;
            }
            catch
            {
                string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TiengAnhAI");
                if (!Directory.Exists(appData)) Directory.CreateDirectory(appData);
                return Path.Combine(appData, "ai_active_config.json");
            }
        }

        /// <summary>
        /// Lưu cấu hình nhà cung cấp và mô hình AI được chọn
        /// </summary>
        public static void SaveActiveModelConfig(string provider, string model, bool enableFallback)
        {
            try
            {
                var data = new
                {
                    ActiveProvider = provider,
                    ActiveModel = model,
                    EnableFallback = enableFallback,
                    LastUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(GetActiveConfigFilePath(), json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AIConfigHelper] Không thể lưu ai_active_config.json: {ex.Message}");
            }
        }

        /// <summary>
        /// Nạp cấu hình nhà cung cấp và mô hình AI đã lưu trước đó
        /// </summary>
        public static (string? Provider, string? Model, bool? EnableFallback) LoadActiveModelConfig()
        {
            try
            {
                string path = GetActiveConfigFilePath();
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;
                    string? provider = root.TryGetProperty("ActiveProvider", out var p) ? p.GetString() : null;
                    string? model = root.TryGetProperty("ActiveModel", out var m) ? m.GetString() : null;
                    bool? fallback = root.TryGetProperty("EnableFallback", out var f) ? f.GetBoolean() : null;
                    return (provider, model, fallback);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AIConfigHelper] Không thể nạp ai_active_config.json: {ex.Message}");
            }
            return (null, null, null);
        }

        /// <summary>
        /// Làm sạch và che giấu chuỗi API Key khi hiển thị trên giao diện hoặc lưu trữ log
        /// </summary>
        public static string MaskKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return "Chưa cấu hình";
            if (key.Length <= 8) return "••••••••";
            return $"{key.Substring(0, 4)}••••••••{key.Substring(key.Length - 4)}";
        }
    }
}
