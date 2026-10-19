using System;
using System.IO;
using System.Text.Json;

namespace BUS.AI.Configuration
{
    public static class AIConfigHelper
    {
        private static List<string> GetCandidateSecretsPaths()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var paths = new List<string>
            {
                Path.Combine(baseDir, "secrets.json"),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\secrets.json")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\GUI\secrets.json")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\GUI\secrets.json")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\secrets.json")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\..\secrets.json"))
            };

            // 1. Ưu tiên ASP.NET Core User Secrets: %APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json
            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string userSecretsRoot = Path.Combine(appData, "Microsoft", "UserSecrets");
                if (Directory.Exists(userSecretsRoot))
                {
                    string eaiSpecificSecret = Path.Combine(userSecretsRoot, "eai-desktop-app-secrets", "secrets.json");
                    if (File.Exists(eaiSpecificSecret))
                    {
                        paths.Insert(0, eaiSpecificSecret);
                    }

                    foreach (var dir in Directory.GetDirectories(userSecretsRoot))
                    {
                        string secretFile = Path.Combine(dir, "secrets.json");
                        if (File.Exists(secretFile) && !paths.Contains(secretFile))
                        {
                            paths.Insert(0, secretFile);
                        }
                    }
                }
            }
            catch { }

            return paths;
        }

        private static JsonDocument? TryLoadSecretsDoc()
        {
            foreach (var path in GetCandidateSecretsPaths())
            {
                try
                {
                    if (File.Exists(path))
                    {
                        string json = File.ReadAllText(path);
                        return JsonDocument.Parse(json);
                    }
                }
                catch { }
            }
            return null;
        }

        public static string GetGeminiKey()
        {
            // 1. Biến môi trường chuẩn hóa: GEMINI_API_KEY
            string envKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") 
                         ?? Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ?? "";
            if (!string.IsNullOrWhiteSpace(envKey))
                return envKey.Trim();

            // 2. File secrets.json hoặc ASP.NET Core User Secrets
            using var doc = TryLoadSecretsDoc();
            if (doc != null)
            {
                var root = doc.RootElement;
                // Flat key: "GEMINI_API_KEY" (theo dotnet user-secrets set)
                if (root.TryGetProperty("GEMINI_API_KEY", out var flatKey))
                {
                    string k = flatKey.GetString() ?? "";
                    if (!string.IsNullOrWhiteSpace(k)) return k.Trim();
                }

                // Nested key: "Gemini": { "ApiKey": "..." }
                if (root.TryGetProperty("Gemini", out var geminiProp))
                {
                    if (geminiProp.TryGetProperty("ApiKey", out var keyProp))
                    {
                        string k = keyProp.GetString() ?? "";
                        if (!string.IsNullOrWhiteSpace(k)) return k.Trim();
                    }
                    if (geminiProp.TryGetProperty("NoiDungHocTap", out var modProp))
                    {
                        string k = modProp.GetString() ?? "";
                        if (!string.IsNullOrWhiteSpace(k)) return k.Trim();
                    }
                    if (geminiProp.TryGetProperty("DanhMucCauTruc", out var dmProp))
                    {
                        string k = dmProp.GetString() ?? "";
                        if (!string.IsNullOrWhiteSpace(k)) return k.Trim();
                    }
                }
            }

            return string.Empty;
        }

        public static string GetGroqKey()
        {
            // 1. Biến môi trường: GROQ_API_KEY
            string envKey = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? "";
            if (!string.IsNullOrWhiteSpace(envKey))
                return envKey.Trim();

            // 2. File secrets.json hoặc ASP.NET Core User Secrets
            using var doc = TryLoadSecretsDoc();
            if (doc != null)
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("GROQ_API_KEY", out var flatKey))
                {
                    string k = flatKey.GetString() ?? "";
                    if (!string.IsNullOrWhiteSpace(k)) return k.Trim();
                }

                if (root.TryGetProperty("Groq", out var groqProp))
                {
                    if (groqProp.TryGetProperty("ApiKey", out var keyProp))
                    {
                        string k = keyProp.GetString() ?? "";
                        if (!string.IsNullOrWhiteSpace(k)) return k.Trim();
                    }
                }
            }

            return string.Empty;
        }

        public static (string ApiKey, string BaseUrl) GetXkioConfig()
        {
            // 1. Biến môi trường: Hỗ trợ cả XKIO_API_KEY và XKIRO_API_KEY
            string key = Environment.GetEnvironmentVariable("XKIO_API_KEY")
                      ?? Environment.GetEnvironmentVariable("XKIRO_API_KEY") ?? "";
            string url = Environment.GetEnvironmentVariable("XKIO_API_ENDPOINT") 
                      ?? Environment.GetEnvironmentVariable("XKIRO_API_ENDPOINT")
                      ?? Environment.GetEnvironmentVariable("XKIO_BASE_URL")
                      ?? Environment.GetEnvironmentVariable("XKIRO_BASE_URL") ?? "";

            // 2. User Secrets hoặc secrets.json
            using var doc = TryLoadSecretsDoc();
            if (doc != null)
            {
                var root = doc.RootElement;
                if (string.IsNullOrWhiteSpace(key))
                {
                    if (root.TryGetProperty("XKIO_API_KEY", out var flatKey) || root.TryGetProperty("XKIRO_API_KEY", out flatKey))
                        key = flatKey.GetString() ?? "";
                }
                if (string.IsNullOrWhiteSpace(url))
                {
                    if (root.TryGetProperty("XKIO_API_ENDPOINT", out var flatUrl) || root.TryGetProperty("XKIRO_API_ENDPOINT", out flatUrl))
                        url = flatUrl.GetString() ?? "";
                }

                // Nested property: "Xkio" hoặc "xKiro"
                JsonElement xkioProp = default;
                if (root.TryGetProperty("Xkio", out var p1)) xkioProp = p1;
                else if (root.TryGetProperty("xKiro", out var p2)) xkioProp = p2;

                if (xkioProp.ValueKind == JsonValueKind.Object)
                {
                    if (string.IsNullOrWhiteSpace(key) && xkioProp.TryGetProperty("ApiKey", out var keyProp))
                    {
                        key = keyProp.GetString() ?? "";
                    }
                    if (string.IsNullOrWhiteSpace(url) && xkioProp.TryGetProperty("BaseUrl", out var urlProp))
                    {
                        url = urlProp.GetString() ?? "";
                    }
                }
            }

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
