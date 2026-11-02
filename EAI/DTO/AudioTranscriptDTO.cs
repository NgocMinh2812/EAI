using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DTO
{
    public class AudioTranscriptDTO
    {
        public int MaAudioTranscript { get; set; }
        public int? MaBaiHoc { get; set; }
        public string AudioPath { get; set; } = string.Empty;
        public string LoaiAudio { get; set; } = "Uploaded"; // 'Uploaded', 'AiTts', 'SpeakingPractice'
        public string? TieuDeAudio { get; set; }
        public string? ScriptGoc { get; set; }
        public string? TranscriptText { get; set; }
        public string? TranscriptSegmentsJson { get; set; }
        public string Provider { get; set; } = "Groq";
        public string? MoHinhSTT { get; set; } = "whisper-large-v3-turbo";
        public string? MoHinhTTS { get; set; } = "canopylabs/orpheus-v1-english";
        public string? VoiceName { get; set; }
        public string? AudioHash { get; set; }
        public string TrangThaiDuyet { get; set; } = "ChoDuyet"; // 'ChoDuyet', 'DaDuyet', 'TuChoi'
        public bool ChoPhepSinhVienXem { get; set; } = true;
        public double? ThoiLuongGiay { get; set; }
        public long? DungLuongBytes { get; set; }
        public int? NguoiTao { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        private List<TranscriptSegmentItemDTO>? _segments;
        public List<TranscriptSegmentItemDTO> Segments
        {
            get
            {
                if (_segments == null)
                {
                    if (!string.IsNullOrWhiteSpace(TranscriptSegmentsJson))
                    {
                        try
                        {
                            _segments = JsonSerializer.Deserialize<List<TranscriptSegmentItemDTO>>(TranscriptSegmentsJson) ?? new();
                        }
                        catch
                        {
                            _segments = new();
                        }
                    }
                    else
                    {
                        _segments = new();
                    }
                }
                return _segments;
            }
            set
            {
                _segments = value;
                TranscriptSegmentsJson = _segments != null && _segments.Count > 0 
                    ? JsonSerializer.Serialize(_segments) 
                    : null;
            }
        }

        public bool IsDaDuyet => string.Equals(TrangThaiDuyet, "DaDuyet", StringComparison.OrdinalIgnoreCase);
        public bool HasTranscript => !string.IsNullOrWhiteSpace(TranscriptText);
    }
}

