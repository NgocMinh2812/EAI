using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BUS.AI
{
    public enum SpokenWordStatus
    {
        Correct,    // Khớp chính xác
        Missing,    // Từ bị bỏ sót, chưa đọc
        Different,  // Đọc nhầm sang từ khác
        Extra       // Từ đọc dư thừa
    }

    public class SpokenWordDetail
    {
        public string TargetWord { get; set; } = string.Empty;
        public string? RecognizedWord { get; set; }
        public SpokenWordStatus Status { get; set; } = SpokenWordStatus.Correct;

        public string DisplayColor => Status switch
        {
            SpokenWordStatus.Correct => "#15803D",   // Xanh lá
            SpokenWordStatus.Missing => "#DC2626",   // Đỏ
            SpokenWordStatus.Different => "#D97706", // Cam
            SpokenWordStatus.Extra => "#6B7280",     // Xám
            _ => "#1E293B"
        };

        public string StatusBadge => Status switch
        {
            SpokenWordStatus.Correct => "✓ Đúng",
            SpokenWordStatus.Missing => "✗ Thiếu",
            SpokenWordStatus.Different => "⚠ Khác",
            SpokenWordStatus.Extra => "+ Thừa",
            _ => ""
        };
    }

    public class SpeakingEvaluationResult
    {
        public string TargetText { get; set; } = string.Empty;
        public string RecognizedText { get; set; } = string.Empty;
        public double MatchPercentage { get; set; }
        public int CorrectWordsCount { get; set; }
        public int TotalTargetWords { get; set; }
        public List<SpokenWordDetail> WordDetails { get; set; } = new();
        public string FeedbackComment { get; set; } = string.Empty;
        public string ScopeDisclaimer { get; set; } = "Lưu ý: Đánh giá dựa trên nhận dạng văn bản Speech-to-Text (STT) để phát hiện từ vựng bị thiếu hoặc đọc nhầm, không phải chứng chỉ chấm âm vị bản ngữ.";
    }

    public static class SpeechComparisonHelper
    {
        public static SpeakingEvaluationResult EvaluateSpokenSentence(string targetText, string recognizedText)
        {
            var result = new SpeakingEvaluationResult
            {
                TargetText = targetText ?? string.Empty,
                RecognizedText = recognizedText ?? string.Empty
            };

            var targetTokens = Tokenize(targetText);
            var recTokens = Tokenize(recognizedText);

            result.TotalTargetWords = targetTokens.Count;
            if (targetTokens.Count == 0)
            {
                result.MatchPercentage = 0;
                result.FeedbackComment = "Chưa có nội dung câu mẫu để đối chiếu.";
                return result;
            }

            int recIndex = 0;
            int correctCount = 0;

            for (int i = 0; i < targetTokens.Count; i++)
            {
                string targetWord = targetTokens[i];

                if (recIndex < recTokens.Count)
                {
                    string spokenWord = recTokens[recIndex];
                    if (string.Equals(targetWord, spokenWord, StringComparison.OrdinalIgnoreCase))
                    {
                        result.WordDetails.Add(new SpokenWordDetail
                        {
                            TargetWord = targetWord,
                            RecognizedWord = spokenWord,
                            Status = SpokenWordStatus.Correct
                        });
                        correctCount++;
                        recIndex++;
                    }
                    else
                    {
                        // Kiểm tra xem từ tiếp theo có khớp không (phát hiện đọc nhầm vs bỏ sót)
                        int lookAhead = -1;
                        for (int k = recIndex + 1; k < Math.Min(recIndex + 3, recTokens.Count); k++)
                        {
                            if (string.Equals(targetWord, recTokens[k], StringComparison.OrdinalIgnoreCase))
                            {
                                lookAhead = k;
                                break;
                            }
                        }

                        if (lookAhead != -1)
                        {
                            // Có từ thừa ở giữa
                            result.WordDetails.Add(new SpokenWordDetail
                            {
                                TargetWord = targetWord,
                                RecognizedWord = recTokens[lookAhead],
                                Status = SpokenWordStatus.Correct
                            });
                            correctCount++;
                            recIndex = lookAhead + 1;
                        }
                        else
                        {
                            result.WordDetails.Add(new SpokenWordDetail
                            {
                                TargetWord = targetWord,
                                RecognizedWord = spokenWord,
                                Status = SpokenWordStatus.Different
                            });
                            recIndex++;
                        }
                    }
                }
                else
                {
                    // Hết từ nhận dạng -> các từ còn lại là Missing
                    result.WordDetails.Add(new SpokenWordDetail
                    {
                        TargetWord = targetWord,
                        RecognizedWord = null,
                        Status = SpokenWordStatus.Missing
                    });
                }
            }

            result.CorrectWordsCount = correctCount;
            result.MatchPercentage = Math.Round((double)correctCount / targetTokens.Count * 100.0, 1);

            result.FeedbackComment = result.MatchPercentage switch
            {
                >= 90 => "Xuất sắc! Bạn đã phát âm rất rõ ràng và đầy đủ toàn bộ các từ trong câu.",
                >= 75 => "Rất tốt! Nhận dạng chính xác phần lớn các từ, hãy chú ý đọc chậm rãi ở các từ bị sót.",
                >= 50 => "Khá tốt! Bạn đã nói được ý chính, nên luyện tập thêm để đọc rõ từng âm tiết.",
                _ => "Cần cố gắng thêm! Hãy nghe lại mẫu phát âm của giáo viên và thử đọc lại từng cụm ngắn."
            };

            return result;
        }

        private static List<string> Tokenize(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            string cleaned = Regex.Replace(text, @"[^\w\s\']", " ");
            var words = cleaned.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Select(w => w.Trim().ToLowerInvariant()).Where(w => !string.IsNullOrEmpty(w)).ToList();
        }
    }
}

