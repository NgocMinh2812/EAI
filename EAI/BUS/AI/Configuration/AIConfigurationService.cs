using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Models;

namespace BUS.AI.Configuration
{
    /// <summary>
    /// Dịch vụ quản lý cấu hình AI dùng chung toàn hệ thống (Gia sư AI, AI Writing, Admin LLM Monitor).
    /// Đọc API Key duy nhất từ apiminh.json. Tuyệt đối không đọc từ api.json của Quys.
    /// Quản lý lựa chọn Provider, Model LLM đang hoạt động và kiểm tra kết nối thực tế.
    /// </summary>
    public class AIConfigurationService
    {
        private static readonly Lazy<AIConfigurationService> _instance = new(() => new AIConfigurationService());
        public static AIConfigurationService Instance => _instance.Value;

        private const string ConfigFileName = "apiminh.json";
        private readonly Dictionary<AIProviderType, string> _apiKeys = new();
        private readonly Dictionary<AIProviderType, string> _baseUrls = new();

        public AIProviderType ActiveProvider { get; set; } = AIProviderType.Gemini;
        public string ActiveModel { get; set; } = "gemini-2.0-flash";
        public bool EnableFallback { get; set; } = true;

        public string? ResolvedConfigFilePath { get; private set; }
        public bool IsLoaded { get; private set; }

        private AIConfigurationService()
        {
            // Thiết lập BaseUrl mặc định
            _baseUrls[AIProviderType.Xkio] = "https://api.xkiro.com/v1";
            _baseUrls[AIProviderType.Groq] = "https://api.groq.com/openai/v1";
            _baseUrls[AIProviderType.Gemini] = "https://generativelanguage.googleapis.com/v1beta";
            _baseUrls[AIProviderType.Ollama] = "http://localhost:11434";

            ReloadConfiguration();
            LoadActiveModelConfig();
        }

        /// <summary>
        /// Tìm đường dẫn thực tế của tệp apiminh.json trong Solution
        /// Quét qua: BaseDirectory, CurrentDirectory, và các thư mục cha lên đến thư mục gốc của Solution
        /// </summary>
        public string? FindApiMinhPath()
        {
            try
            {
                // 1. Thư mục chạy ứng dụng (bin\Debug\...)
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string p1 = Path.Combine(baseDir, ConfigFileName);
                if (File.Exists(p1)) return p1;

                // 2. Thư mục làm việc hiện tại
                string currDir = Directory.GetCurrentDirectory();
                string p2 = Path.Combine(currDir, ConfigFileName);
                if (File.Exists(p2)) return p2;

                // 3. Quét ngược lên 6 cấp thư mục cha (tìm EAI\apiminh.json hoặc apiminh.json)
                DirectoryInfo? dir = new DirectoryInfo(baseDir);
                for (int i = 0; i < 6 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir.FullName, ConfigFileName);
                    if (File.Exists(candidate)) return candidate;

                    string candidateSub = Path.Combine(dir.FullName, "EAI", ConfigFileName);
                    if (File.Exists(candidateSub)) return candidateSub;

                    dir = dir.Parent;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Đọc và tải cấu hình API Key từ apiminh.json
        /// Hỗ trợ cả hai định dạng: dạng JSON chuẩn ({ "Gemini": { "ApiKey": "..." } })
        /// và dạng Key-Value từng dòng ("Xkiro: sk-xt-...", "Groq: gsk_...", "Gemini: AQ...")
        /// </summary>
        public void ReloadConfiguration()
        {
            _apiKeys.Clear();
            ResolvedConfigFilePath = FindApiMinhPath();

            if (string.IsNullOrEmpty(ResolvedConfigFilePath) || !File.Exists(ResolvedConfigFilePath))
            {
                IsLoaded = false;
                System.Diagnostics.Debug.WriteLine($"[AIConfigurationService] CẢNH BÁO: Không tìm thấy tệp {ConfigFileName} trong Solution.");
                return;
            }

            try
            {
                string rawText = File.ReadAllText(ResolvedConfigFilePath).Trim();
                if (string.IsNullOrWhiteSpace(rawText))
                {
                    IsLoaded = false;
                    return;
                }

                // Cách 1: Thử phân tích cú pháp JSON
                if (rawText.StartsWith("{") && rawText.EndsWith("}"))
                {
                    using var doc = JsonDocument.Parse(rawText);
                    var root = doc.RootElement;

                    foreach (var prop in root.EnumerateObject())
                    {
                        var providerType = ParseProviderType(prop.Name);
                        if (!providerType.HasValue) continue;

                        if (prop.Value.ValueKind == JsonValueKind.Object)
                        {
                            if (prop.Value.TryGetProperty("ApiKey", out var keyEl))
                            {
                                string? k = keyEl.GetString();
                                if (!string.IsNullOrWhiteSpace(k)) _apiKeys[providerType.Value] = k.Trim();
                            }
                            if (prop.Value.TryGetProperty("BaseUrl", out var urlEl))
                            {
                                string? u = urlEl.GetString();
                                if (!string.IsNullOrWhiteSpace(u)) _baseUrls[providerType.Value] = u.Trim();
                            }
                        }
                        else if (prop.Value.ValueKind == JsonValueKind.String)
                        {
                            string? k = prop.Value.GetString();
                            if (!string.IsNullOrWhiteSpace(k)) _apiKeys[providerType.Value] = k.Trim();
                        }
                    }
                }
                else
                {
                    // Cách 2: Phân tích cú pháp từng dòng Key-Value (ví dụ: "Xkiro: sk-xt-...")
                    var lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        string trimmed = line.Trim();
                        if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#") || trimmed.StartsWith("//"))
                            continue;

                        int colonIdx = trimmed.IndexOf(':');
                        if (colonIdx > 0 && colonIdx < trimmed.Length - 1)
                        {
                            string providerPart = trimmed.Substring(0, colonIdx).Trim();
                            string valuePart = trimmed.Substring(colonIdx + 1).Trim();

                            var providerType = ParseProviderType(providerPart);
                            if (providerType.HasValue && !string.IsNullOrWhiteSpace(valuePart))
                            {
                                _apiKeys[providerType.Value] = valuePart;
                            }
                        }
                    }
                }

                // Dự phòng: nếu apiminh.json chưa cấu hình key nào, nạp từ .NET User Secrets / Biến môi trường
                if (_apiKeys.Count == 0)
                {
                    NapApiKeyDuPhong();
                }

                IsLoaded = _apiKeys.Count > 0;
            }
            catch (Exception ex)
            {
                IsLoaded = false;
                System.Diagnostics.Debug.WriteLine($"[AIConfigurationService] Lỗi khi nạp {ConfigFileName}: {ex.Message}");
                NapApiKeyDuPhong();
            }
        }

        private void NapApiKeyDuPhong()
        {
            try
            {
                var cfg = AIConfigHelper.Configuration;
                string? geminiKey = cfg["Gemini:ApiKey"] ?? cfg["GEMINI_API_KEY"] ?? cfg["GOOGLE_API_KEY"]
                                 ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                                 ?? Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
                if (!string.IsNullOrWhiteSpace(geminiKey)) _apiKeys[AIProviderType.Gemini] = geminiKey.Trim();

                string? groqKey = cfg["Groq:ApiKey"] ?? cfg["GROQ_API_KEY"]
                                 ?? Environment.GetEnvironmentVariable("GROQ_API_KEY");
                if (!string.IsNullOrWhiteSpace(groqKey)) _apiKeys[AIProviderType.Groq] = groqKey.Trim();

                string? xkioKey = cfg["Xkio:ApiKey"] ?? cfg["xKiro:ApiKey"] ?? cfg["XKIO_API_KEY"]
                                 ?? Environment.GetEnvironmentVariable("XKIO_API_KEY")
                                 ?? Environment.GetEnvironmentVariable("XKIRO_API_KEY");
                if (!string.IsNullOrWhiteSpace(xkioKey)) _apiKeys[AIProviderType.Xkio] = xkioKey.Trim();

                if (_apiKeys.Count > 0) IsLoaded = true;
            }
            catch { }
        }

        private AIProviderType? ParseProviderType(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            name = name.Trim();

            if (name.Equals("Gemini", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("Google", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("GoogleGemini", StringComparison.OrdinalIgnoreCase))
                return AIProviderType.Gemini;

            if (name.Equals("Groq", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("GroqCloud", StringComparison.OrdinalIgnoreCase))
                return AIProviderType.Groq;

            if (name.Equals("Xkio", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("Xkiro", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("xKiro", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("xKiroAIGateway", StringComparison.OrdinalIgnoreCase))
                return AIProviderType.Xkio;

            if (name.Equals("Ollama", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("Local", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("LocalAI", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("Offline", StringComparison.OrdinalIgnoreCase))
                return AIProviderType.Ollama;

            return null;
        }

        /// <summary>
        /// Lấy API Key thực tế của Provider từ apiminh.json (hoặc User Secrets dự phòng)
        /// </summary>
        public string GetApiKey(AIProviderType provider)
        {
            if (provider == AIProviderType.Ollama)
            {
                return "localhost"; // Ollama không yêu cầu API Key
            }

            if (_apiKeys.TryGetValue(provider, out var key) && !string.IsNullOrWhiteSpace(key))
            {
                return key.Trim();
            }

            try
            {
                var cfg = AIConfigHelper.Configuration;
                string? envKey = provider switch
                {
                    AIProviderType.Gemini => cfg["Gemini:ApiKey"] ?? cfg["GEMINI_API_KEY"] ?? cfg["GOOGLE_API_KEY"]
                                             ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                                             ?? Environment.GetEnvironmentVariable("GOOGLE_API_KEY"),
                    AIProviderType.Groq => cfg["Groq:ApiKey"] ?? cfg["GROQ_API_KEY"]
                                           ?? Environment.GetEnvironmentVariable("GROQ_API_KEY"),
                    AIProviderType.Xkio => cfg["Xkio:ApiKey"] ?? cfg["xKiro:ApiKey"] ?? cfg["XKIO_API_KEY"]
                                           ?? Environment.GetEnvironmentVariable("XKIO_API_KEY")
                                           ?? Environment.GetEnvironmentVariable("XKIRO_API_KEY"),
                    _ => null
                };

                if (!string.IsNullOrWhiteSpace(envKey))
                {
                    _apiKeys[provider] = envKey.Trim();
                    return envKey.Trim();
                }
            }
            catch { }

            return string.Empty;
        }

        /// <summary>
        /// Lấy API Key đã được che an toàn (ví dụ: sk-****a0be) để hiển thị trên giao diện Admin
        /// Tuyệt đối không để lộ toàn bộ API Key
        /// </summary>
        public string GetMaskedApiKey(AIProviderType provider)
        {
            if (provider == AIProviderType.Ollama)
            {
                return "Không cần API Key (Localhost)";
            }

            string key = GetApiKey(provider);
            if (string.IsNullOrWhiteSpace(key)) return "Chưa cấu hình";
            if (key.Length <= 8) return "••••••••";
            return $"{key.Substring(0, 4)}••••••••{key.Substring(key.Length - 4)}";
        }

        /// <summary>
        /// Kiểm tra trạng thái đã cấu hình của Provider
        /// </summary>
        public bool IsProviderConfigured(AIProviderType provider)
        {
            if (provider == AIProviderType.Ollama) return true;
            return !string.IsNullOrWhiteSpace(GetApiKey(provider));
        }

        /// <summary>
        /// Lấy Base URL của Provider nếu yêu cầu
        /// </summary>
        public string GetBaseUrl(AIProviderType provider)
        {
            if (_baseUrls.TryGetValue(provider, out var url) && !string.IsNullOrWhiteSpace(url))
            {
                return url.Trim();
            }

            return provider switch
            {
                AIProviderType.Xkio => "https://api.xkiro.com/v1",
                AIProviderType.Groq => "https://api.groq.com/openai/v1",
                AIProviderType.Ollama => "http://localhost:11434",
                _ => "https://generativelanguage.googleapis.com/v1beta"
            };
        }

        /// <summary>
        /// Danh sách Model hợp lệ theo từng Provider
        /// </summary>
        public IReadOnlyList<AIModelInfo> GetSupportedModels(AIProviderType provider)
        {
            return provider switch
            {
                AIProviderType.Gemini => new List<AIModelInfo>
                {
                    new() { Id = "gemini-2.0-flash", DisplayName = "Gemini 2.0 Flash (Khuyên dùng - Tốc độ cao)", ProviderType = AIProviderType.Gemini, ProviderName = "Google Gemini", OwnedBy = "Google", AccessTier = "free" },
                    new() { Id = "gemini-1.5-flash", DisplayName = "Gemini 1.5 Flash (Tiêu chuẩn)", ProviderType = AIProviderType.Gemini, ProviderName = "Google Gemini", OwnedBy = "Google", AccessTier = "free" },
                    new() { Id = "gemini-1.5-pro", DisplayName = "Gemini 1.5 Pro (Học thuật chuyên sâu)", ProviderType = AIProviderType.Gemini, ProviderName = "Google Gemini", OwnedBy = "Google", AccessTier = "paid" },
                    new() { Id = "gemini-flash-latest", DisplayName = "Gemini Flash Latest", ProviderType = AIProviderType.Gemini, ProviderName = "Google Gemini", OwnedBy = "Google", AccessTier = "free" }
                },
                AIProviderType.Groq => new List<AIModelInfo>
                {
                    new() { Id = "llama-3.3-70b-versatile", DisplayName = "Llama 3.3 70B Versatile (Mạnh mẽ)", ProviderType = AIProviderType.Groq, ProviderName = "Groq Cloud", OwnedBy = "Meta", AccessTier = "free" },
                    new() { Id = "qwen/qwen3.8-27b", DisplayName = "Qwen 3.8 27B (Đa ngôn ngữ tốt)", ProviderType = AIProviderType.Groq, ProviderName = "Groq Cloud", OwnedBy = "Alibaba", AccessTier = "free" },
                    new() { Id = "openai/gpt-oss-120b", DisplayName = "GPT OSS 120B", ProviderType = AIProviderType.Groq, ProviderName = "Groq Cloud", OwnedBy = "OpenAI", AccessTier = "free" },
                    new() { Id = "openai/gpt-oss-20b", DisplayName = "GPT OSS 20B", ProviderType = AIProviderType.Groq, ProviderName = "Groq Cloud", OwnedBy = "OpenAI", AccessTier = "free" }
                },
                AIProviderType.Xkio => new List<AIModelInfo>
                {
                    new() { Id = "qwen/qwen3.5-flash:free", DisplayName = "Qwen 3.5 Flash (Free Gateway)", ProviderType = AIProviderType.Xkio, ProviderName = "xKiro AI Gateway", OwnedBy = "Alibaba", AccessTier = "free" },
                    new() { Id = "qwen/qwen3.8-max:free", DisplayName = "Qwen 3.8 Max (Free Gateway)", ProviderType = AIProviderType.Xkio, ProviderName = "xKiro AI Gateway", OwnedBy = "Alibaba", AccessTier = "free" },
                    new() { Id = "qwen/qwen3.7-plus:free", DisplayName = "Qwen 3.7 Plus (Free Gateway)", ProviderType = AIProviderType.Xkio, ProviderName = "xKiro AI Gateway", OwnedBy = "Alibaba", AccessTier = "free" },
                    new() { Id = "cohere/command-r7b-12-2024", DisplayName = "Cohere Command R7B", ProviderType = AIProviderType.Xkio, ProviderName = "xKiro AI Gateway", OwnedBy = "Cohere", AccessTier = "free" },
                    new() { Id = "nvidia/nemotron-3-ultra", DisplayName = "NVIDIA Nemotron 3 Ultra", ProviderType = AIProviderType.Xkio, ProviderName = "xKiro AI Gateway", OwnedBy = "NVIDIA", AccessTier = "free" },
                    new() { Id = "mistralai/ministral-8b", DisplayName = "Mistral Ministral 8B", ProviderType = AIProviderType.Xkio, ProviderName = "xKiro AI Gateway", OwnedBy = "Mistral", AccessTier = "free" }
                },
                AIProviderType.Ollama => new List<AIModelInfo>
                {
                    new() { Id = "llama3.2:latest", DisplayName = "Llama 3.2 (Meta - Khuyên dùng Offline)", ProviderType = AIProviderType.Ollama, ProviderName = "Ollama (Offline/Local LLM)", OwnedBy = "Meta", AccessTier = "offline", DefaultModel = true },
                    new() { Id = "qwen2.5:latest", DisplayName = "Qwen 2.5 (Alibaba - Đa ngôn ngữ Offline)", ProviderType = AIProviderType.Ollama, ProviderName = "Ollama (Offline/Local LLM)", OwnedBy = "Alibaba", AccessTier = "offline" },
                    new() { Id = "mistral:latest", DisplayName = "Mistral 7B (Mistral AI - Lập luận Offline)", ProviderType = AIProviderType.Ollama, ProviderName = "Ollama (Offline/Local LLM)", OwnedBy = "Mistral", AccessTier = "offline" },
                    new() { Id = "phi3:latest", DisplayName = "Phi-3 Mini (Microsoft - Siêu nhẹ Offline)", ProviderType = AIProviderType.Ollama, ProviderName = "Ollama (Offline/Local LLM)", OwnedBy = "Microsoft", AccessTier = "offline" },
                    new() { Id = "deepseek-r1:latest", DisplayName = "DeepSeek R1 (DeepSeek - Suy luận Toán/Code Offline)", ProviderType = AIProviderType.Ollama, ProviderName = "Ollama (Offline/Local LLM)", OwnedBy = "DeepSeek", AccessTier = "offline" },
                    new() { Id = "gemma2:latest", DisplayName = "Gemma 2 (Google - Tối ưu CPU Offline)", ProviderType = AIProviderType.Ollama, ProviderName = "Ollama (Offline/Local LLM)", OwnedBy = "Google", AccessTier = "offline" }
                },
                _ => new List<AIModelInfo>()
            };
        }

        /// <summary>
        /// Lấy Model mặc định của từng Provider
        /// </summary>
        public string GetDefaultModel(AIProviderType provider)
        {
            return provider switch
            {
                AIProviderType.Gemini => "gemini-2.0-flash",
                AIProviderType.Groq => "llama-3.3-70b-versatile",
                AIProviderType.Xkio => "qwen/qwen3.5-flash:free",
                AIProviderType.Ollama => "llama3.2:latest",
                _ => "gemini-2.0-flash"
            };
        }

        #region Active Model Persistence & Synchronization
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

        public void SaveActiveConfig(AIProviderType provider, string model, bool enableFallback)
        {
            // Kiểm tra quyền Admin trước khi lưu cấu hình
            if (!CheckAdminPermission())
            {
                throw new UnauthorizedAccessException("Chỉ có tài khoản Quản trị viên (Admin/Giảng viên) mới có quyền lưu cấu hình mô hình AI hệ thống.");
            }

            ActiveProvider = provider;
            ActiveModel = !string.IsNullOrWhiteSpace(model) ? model : GetDefaultModel(provider);
            EnableFallback = enableFallback;

            // Đồng bộ sang AIServiceManager
            AIServiceManager.Instance.ActiveProviderType = ActiveProvider;
            AIServiceManager.Instance.ActiveModel = ActiveModel;
            AIServiceManager.Instance.EnableFallback = EnableFallback;

            try
            {
                var data = new
                {
                    ActiveProvider = provider.ToString(),
                    ActiveModel = ActiveModel,
                    EnableFallback = enableFallback,
                    LastUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ConfigSource = ConfigFileName
                };
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(GetActiveConfigFilePath(), json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AIConfigurationService] Lỗi khi lưu cấu hình AI: {ex.Message}");
            }
        }

        public void LoadActiveModelConfig()
        {
            try
            {
                string path = GetActiveConfigFilePath();
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("ActiveProvider", out var p) && Enum.TryParse<AIProviderType>(p.GetString(), true, out var parsedProvider))
                    {
                        ActiveProvider = parsedProvider;
                    }
                    if (root.TryGetProperty("ActiveModel", out var m) && !string.IsNullOrWhiteSpace(m.GetString()))
                    {
                        ActiveModel = m.GetString()!;
                    }
                    else
                    {
                        ActiveModel = GetDefaultModel(ActiveProvider);
                    }
                    if (root.TryGetProperty("EnableFallback", out var f))
                    {
                        EnableFallback = f.GetBoolean();
                    }

                    // Đồng bộ với AIServiceManager
                    AIServiceManager.Instance.ActiveProviderType = ActiveProvider;
                    AIServiceManager.Instance.ActiveModel = ActiveModel;
                    AIServiceManager.Instance.EnableFallback = EnableFallback;
                }
                else
                {
                    ActiveModel = GetDefaultModel(ActiveProvider);
                }
            }
            catch { }
        }

        /// <summary>
        /// Kiểm tra quyền Admin/Giảng viên của phiên đăng nhập hiện tại
        /// </summary>
        public bool CheckAdminPermission()
        {
            return SessionHelper.IsGiangVien || SessionHelper.IsAdmin;
        }
        #endregion

        #region Real Connection Testing (Nút Test Connection)
        /// <summary>
        /// Gửi request kiểm tra thực tế đến Provider được chọn bằng API Key từ apiminh.json
        /// Không trả về trạng thái giả lập.
        /// </summary>
        public async Task<AIResponse> TestConnectionAsync(AIProviderType providerType, string? model = null, CancellationToken ct = default)
        {
            string apiKey = GetApiKey(providerType);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return AIResponse.Fail(providerType.ToString(), $"Chưa cấu hình API Key cho nhà cung cấp {providerType} trong tệp {ConfigFileName}.", 401);
            }

            string testModel = !string.IsNullOrWhiteSpace(model) ? model : GetDefaultModel(providerType);
            var provider = AIServiceManager.Instance.GetProvider(providerType);

            return await provider.GenerateContentAsync("Hello, respond in one word: OK.", testModel, ct);
        }
        #endregion
    }
}

