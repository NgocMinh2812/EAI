using System;

namespace BUS.AI.Models
{
    public class AIHealthStatus
    {
        public string ProviderName { get; set; } = string.Empty;
        public AIProviderType ProviderType { get; set; }
        public bool IsConfigured { get; set; }
        public string ConfiguredStatusText => IsConfigured ? "Đã cấu hình" : "Chưa cấu hình";
        
        /// <summary>
        /// Trạng thái: "Healthy" (Hoạt động tốt), "Degraded" (Chập chờn), "Unavailable" (Không khả dụng), "Unverified" (Chưa xác minh)
        /// </summary>
        public string Health { get; set; } = "Unavailable";
        public bool IsHealthy => string.Equals(Health, "Healthy", StringComparison.OrdinalIgnoreCase);
        
        public long LatencyMs { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? LastChecked { get; set; }
        public string ActiveModel { get; set; } = string.Empty;
    }
}
