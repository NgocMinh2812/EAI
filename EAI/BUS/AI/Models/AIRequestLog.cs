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
        public int StatusCode { get; set; } = 200;
        public string PromptPreview { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public bool IsFallback { get; set; }

        public string StatusBadgeText
        {
            get
            {
                if (Success)
                {
                    int code = StatusCode > 0 ? StatusCode : 200;
                    return $"POST {code} OK";
                }
                else
                {
                    int code = StatusCode > 0 ? StatusCode : 500;
                    return $"ERR {code}";
                }
            }
        }
        public string StatusBadgeBg => Success ? "#DCFCE7" : "#FEE2E2";
        public string StatusBadgeFg => Success ? "#15803D" : "#DC2626";
        public string FormattedTime => Timestamp.ToString("HH:mm:ss");
    }
}
