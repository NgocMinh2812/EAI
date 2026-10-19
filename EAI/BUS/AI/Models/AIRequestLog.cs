using System;

namespace BUS.AI.Models
{
    public class AIRequestLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string ActionName { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public long LatencyMs { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string PromptPreview { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public bool IsFallback { get; set; }

        public string StatusBadgeText => Success ? $"POST {StatusCode} OK" : $"ERR {StatusCode}";
        public string FormattedTime => Timestamp.ToString("HH:mm:ss");
    }
}
