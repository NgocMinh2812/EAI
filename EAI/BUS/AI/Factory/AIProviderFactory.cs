using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Configuration;
using BUS.AI.Models;
using BUS.AI.Providers;

namespace BUS.AI.Factory
{
    /// <summary>
    /// Giao diện Factory cho việc khởi tạo, lựa chọn dự phòng và kiểm tra sức khỏe của các AI Providers.
    /// </summary>
    public interface IAIProviderFactory
    {
        IAIProvider CreateProvider(AIProviderType type);
        IAIProvider GetDefaultProvider();
        IReadOnlyList<IAIProvider> GetAllProviders();
        IReadOnlyList<IAIProvider> GetConfiguredProviders();
        IAIProvider? GetFallbackProvider(AIProviderType currentType);
        Task<AIHealthStatus> CheckHealthAsync(AIProviderType type, CancellationToken ct = default);
        Task<Dictionary<AIProviderType, AIHealthStatus>> CheckAllHealthAsync(CancellationToken ct = default);
    }

    /// <summary>
    /// Factory triển khai mẫu hình Factory Pattern để tạo và điều phối các AI Provider trong EAI:
    /// Gemini, Groq, Ollama, Xkio.
    /// Hỗ trợ cơ chế Fallback tự động và kiểm tra sức khỏe (Health Check).
    /// </summary>
    public class AIProviderFactory : IAIProviderFactory
    {
        private static readonly Lazy<AIProviderFactory> _instance = new(() => new AIProviderFactory());
        public static AIProviderFactory Instance => _instance.Value;

        // Thứ tự ưu tiên dự phòng mặc định khi một provider gặp lỗi
        private static readonly AIProviderType[] FallbackOrder = new[]
        {
            AIProviderType.Gemini,
            AIProviderType.Groq,
            AIProviderType.Ollama,
            AIProviderType.Xkio
        };

        public AIProviderFactory() { }

        /// <summary>
        /// Khởi tạo một thể hiện mới của IAIProvider theo loại chỉ định.
        /// </summary>
        public IAIProvider CreateProvider(AIProviderType type)
        {
            return type switch
            {
                AIProviderType.Gemini => new GeminiProvider(),
                AIProviderType.Groq => new GroqProvider(),
                AIProviderType.Xkio => new XkioProvider(),
                AIProviderType.Ollama => new OllamaProvider(),
                _ => throw new NotSupportedException($"Không hỗ trợ loại AI Provider '{type}'.")
            };
        }

        /// <summary>
        /// Lấy Provider mặc định dựa trên cấu hình lưu trữ hoặc ưu tiên Gemini.
        /// </summary>
        public IAIProvider GetDefaultProvider()
        {
            try
            {
                var (savedProvider, _, _) = AIConfigHelper.LoadActiveModelConfig();
                if (!string.IsNullOrWhiteSpace(savedProvider) && Enum.TryParse<AIProviderType>(savedProvider, true, out var pType))
                {
                    var provider = CreateProvider(pType);
                    if (provider.IsConfigured)
                    {
                        return provider;
                    }
                }
            }
            catch
            {
                // Fallback nếu không đọc được config
            }

            // Thử lần lượt các provider đã được cấu hình theo thứ tự ưu tiên
            foreach (var type in FallbackOrder)
            {
                var p = CreateProvider(type);
                if (p.IsConfigured)
                {
                    return p;
                }
            }

            // Mặc định trả về Gemini
            return CreateProvider(AIProviderType.Gemini);
        }

        /// <summary>
        /// Trả về danh sách tất cả các Provider được hỗ trợ.
        /// </summary>
        public IReadOnlyList<IAIProvider> GetAllProviders()
        {
            var list = new List<IAIProvider>();
            foreach (AIProviderType type in Enum.GetValues(typeof(AIProviderType)))
            {
                list.Add(CreateProvider(type));
            }
            return list;
        }

        /// <summary>
        /// Lấy danh sách các Provider đã được cấu hình hợp lệ (có API key hoặc là Ollama local).
        /// </summary>
        public IReadOnlyList<IAIProvider> GetConfiguredProviders()
        {
            return GetAllProviders().Where(p => p.IsConfigured).ToList();
        }

        /// <summary>
        /// Tìm Provider dự phòng tiếp theo khi Provider hiện tại gặp sự cố (quá tải, hết hạn mức 429, lỗi mạng...).
        /// </summary>
        public IAIProvider? GetFallbackProvider(AIProviderType currentType)
        {
            var configured = GetConfiguredProviders();
            if (configured.Count == 0) return null;

            // Tìm provider đầu tiên khác loại hiện tại theo thứ tự FallbackOrder
            foreach (var fallbackType in FallbackOrder)
            {
                if (fallbackType == currentType) continue;

                var candidate = configured.FirstOrDefault(p => p.ProviderType == fallbackType);
                if (candidate != null)
                {
                    return candidate;
                }
            }

            // Nếu không khớp danh sách FallbackOrder, lấy bất kỳ provider nào khác
            return configured.FirstOrDefault(p => p.ProviderType != currentType);
        }

        /// <summary>
        /// Kiểm tra sức khỏe của một Provider cụ thể.
        /// </summary>
        public async Task<AIHealthStatus> CheckHealthAsync(AIProviderType type, CancellationToken ct = default)
        {
            try
            {
                var provider = CreateProvider(type);
                return await provider.CheckHealthAsync(ct);
            }
            catch (Exception ex)
            {
                return new AIHealthStatus
                {
                    ProviderType = type,
                    ProviderName = type.ToString(),
                    Health = "Unavailable",
                    Message = $"Lỗi kiểm tra sức khỏe {type}: {ex.Message}",
                    LastChecked = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Kiểm tra sức khỏe đồng thời toàn bộ các Provider.
        /// </summary>
        public async Task<Dictionary<AIProviderType, AIHealthStatus>> CheckAllHealthAsync(CancellationToken ct = default)
        {
            var results = new Dictionary<AIProviderType, AIHealthStatus>();
            var tasks = new List<(AIProviderType Type, Task<AIHealthStatus> Task)>();

            foreach (AIProviderType type in Enum.GetValues(typeof(AIProviderType)))
            {
                var p = CreateProvider(type);
                tasks.Add((type, p.CheckHealthAsync(ct)));
            }

            foreach (var item in tasks)
            {
                try
                {
                    results[item.Type] = await item.Task;
                }
                catch (Exception ex)
                {
                    results[item.Type] = new AIHealthStatus
                    {
                        ProviderType = item.Type,
                        ProviderName = item.Type.ToString(),
                        Health = "Unavailable",
                        Message = $"Lỗi kiểm tra {item.Type}: {ex.Message}",
                        LastChecked = DateTime.Now
                    };
                }
            }

            return results;
        }
    }
}
