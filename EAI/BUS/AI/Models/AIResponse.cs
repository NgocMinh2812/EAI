namespace BUS.AI.Models
{
    public class AIResponse
    {
        public bool Success { get; set; }
        public string Text { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public string ModelUsed { get; set; } = string.Empty;
        public long LatencyMs { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsFallback { get; set; }

        public static AIResponse Fail(string providerName, string errorMessage, int statusCode = 500, long latencyMs = 0)
        {
            return new AIResponse
            {
                Success = false,
                ProviderName = providerName,
                ErrorMessage = errorMessage,
                StatusCode = statusCode,
                LatencyMs = latencyMs
            };
        }

        public static AIResponse Ok(string text, string providerName, string modelUsed, long latencyMs, int statusCode = 200, bool isFallback = false)
        {
            return new AIResponse
            {
                Success = true,
                Text = text,
                ProviderName = providerName,
                ModelUsed = modelUsed,
                LatencyMs = latencyMs,
                StatusCode = statusCode,
                IsFallback = isFallback
            };
        }
    }
}
