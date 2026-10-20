using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Models;

namespace BUS.AI.Providers
{
    public interface IAIProvider
    {
        string ProviderName { get; }
        AIProviderType ProviderType { get; }
        bool IsConfigured { get; }
        IReadOnlyList<string> SupportedModels { get; }
        string DefaultModel { get; }

        Task<AIResponse> GenerateContentAsync(string prompt, string? model = null, CancellationToken ct = default);
        Task<AIHealthStatus> CheckHealthAsync(CancellationToken ct = default);
        Task<IReadOnlyList<AIModelInfo>> GetModelsAsync(bool forceRefresh = false, CancellationToken ct = default);
        bool IsCatalogDynamic { get; }
        string CatalogStatusMessage { get; }
    }
}
