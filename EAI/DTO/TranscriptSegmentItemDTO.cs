using System;
using System.Text.Json.Serialization;

namespace DTO
{
    public class TranscriptSegmentItemDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("start")]
        public double Start { get; set; }

        [JsonPropertyName("end")]
        public double End { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonIgnore]
        public string StartFormatted => FormatTime(Start);

        [JsonIgnore]
        public string EndFormatted => FormatTime(End);

        [JsonIgnore]
        public string TimeRangeDisplay => $"[{StartFormatted} - {EndFormatted}]";

        private static string FormatTime(double seconds)
        {
            if (seconds < 0) seconds = 0;
            var ts = TimeSpan.FromSeconds(seconds);
            return ts.Hours > 0 
                ? $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}" 
                : $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }
}

