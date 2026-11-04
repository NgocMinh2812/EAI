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
        private static IConfiguration? _apiJsonConfig;
        private static string? _lastApiJsonPath;

        public static IConfiguration Configuration => _configuration ??= BuildConfiguration();

        public static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();

            try
            {
                builder.AddEnvironmentVariables();
            }
            catch { }

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
            _apiJsonConfig = null;
            _lastApiJsonPath = null;
            AIConfigurationService.Instance.ReloadConfiguration();
        }

        public static string? FindApiJsonPath()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string p1 = Path.Combine(baseDir, "api.json");
                if (File.Exists(p1)) return p1;

                string currDir = Directory.GetCurrentDirectory();
                string p2 = Path.Combine(currDir, "api.json");
                if (File.Exists(p2)) return p2;

                DirectoryInfo? dir = new DirectoryInfo(baseDir);
                for (int i = 0; i < 6 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir.FullName, "api.json");
                    if (File.Exists(candidate)) return candidate;

                    string candidateSub = Path.Combine(dir.FullName, "EAI", "api.json");
                    if (File.Exists(candidateSub)) return candidateSub;

                    dir = dir.Parent;
                }
            }
            catch { }

            return null;
        }

        public static IConfiguration? ApiJsonConfiguration
        {
            get
            {
                string? path = FindApiJsonPath();
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;

                if (_apiJsonConfig == null || _lastApiJsonPath != path)
                {
                    try
                    {
                        _lastApiJsonPath = path;
                        _apiJsonConfig = new ConfigurationBuilder()
                            .AddJsonFile(path, optional: true, reloadOnChange: true)
                            .Build();
                    }
                    catch
                    {
                        return null;
                    }
                }
                return _apiJsonConfig;
            }
        }

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
            // =========================================================================
            // 0. BẮT BUỘC ƯU TIÊN SỐ 1: Đọc API Key từ tệp apiminh.json qua AIConfigurationService
            // =========================================================================
            string minhKey = AIConfigurationService.Instance.GetApiKey(Models.AIProviderType.Gemini);
            if (!string.IsNullOrWhiteSpace(minhKey)) return minhKey.Trim();

            var config = Configuration;

            string? key = config["Gemini:ApiKey"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            if (!string.IsNullOrWhiteSpace(module))
            {
                key = config[$"Gemini:{module}"];
                if (!string.IsNullOrWhiteSpace(key)) return key.Trim();
            }

            key = config["Gemini:NoiDungHocTap"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            key = config["Gemini:DanhMucCauTruc"] ?? config["Gemini:DanhMuc"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            key = config["GEMINI_API_KEY"] ?? config["GOOGLE_API_KEY"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

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
            // =========================================================================
            // 0. BẮT BUỘC ƯU TIÊN SỐ 1: Đọc API Key từ tệp apiminh.json qua AIConfigurationService
            // =========================================================================
            string minhKey = AIConfigurationService.Instance.GetApiKey(Models.AIProviderType.Groq);
            if (!string.IsNullOrWhiteSpace(minhKey)) return minhKey.Trim();

            var config = Configuration;

            string? key = config["Groq:ApiKey"] ?? config["GROQ_API_KEY"];
            if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

            string? envKey = GetEnvVar("GROQ_API_KEY");
            if (!string.IsNullOrWhiteSpace(envKey)) return envKey.Trim();

            return string.Empty;
        }

        public static (string ApiKey, string BaseUrl) GetXkioConfig()
        {
            // =========================================================================
            // 0. BẮT BUỘC ƯU TIÊN SỐ 1: Đọc API Key & BaseUrl từ tệp apiminh.json qua AIConfigurationService
            // =========================================================================
            string minhKey = AIConfigurationService.Instance.GetApiKey(Models.AIProviderType.Xkio);
            string minhBaseUrl = AIConfigurationService.Instance.GetBaseUrl(Models.AIProviderType.Xkio);
            if (!string.IsNullOrWhiteSpace(minhKey))
            {
                return (minhKey.Trim(), minhBaseUrl.Trim());
            }

            var config = Configuration;

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

            if (string.IsNullOrWhiteSpace(url) || url.Contains("api.xkio.ai", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://api.xkiro.com/v1";
            }

            return (key.Trim(), url.Trim());
        }

        public static string BuildEndpoint(string baseUrl, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = "https://api.xkiro.com/v1";
            string b = baseUrl.Trim().TrimEnd('/');
            string p = (relativePath ?? "").Trim().TrimStart('/');

            if (b.EndsWith("/v1", StringComparison.OrdinalIgnoreCase) && p.StartsWith("v1/", StringComparison.OrdinalIgnoreCase))
            {
                p = p.Substring(3);
            }
            else if (!b.EndsWith("/v1", StringComparison.OrdinalIgnoreCase) && !p.StartsWith("v1/", StringComparison.OrdinalIgnoreCase) && !p.Equals("v1", StringComparison.OrdinalIgnoreCase))
            {
                p = "v1/" + p;
            }

            return $"{b}/{p}";
        }

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

        public static string MaskKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return "Chưa cấu hình";
            if (key.Length <= 8) return "••••••••";
            return $"{key.Substring(0, 4)}••••••••{key.Substring(key.Length - 4)}";
        }
    }
}
