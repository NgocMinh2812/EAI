using System.Collections.Generic;

namespace QuestionBuilder
{
    public class QuestionItem
    {
        public string SeedId { get; set; } = "";
        public int TopicId { get; set; } // MaChuDe
        public int LevelId { get; set; } // 1: A1, 2: A2, 3: B1, 4: B2, 5: C1
        public string QuestionType { get; set; } = "TracNghiem";
        public string Skill { get; set; } = "Doc";
        public string Difficulty { get; set; } = "TrungBinh";
        public string QuestionText { get; set; } = "";
        public string? ReadingPassage { get; set; }
        public string? AudioPath { get; set; }
        public string Explanation { get; set; } = "";
        public string Citation { get; set; } = ""; // DoanVanDanChung
        public string SourceType { get; set; } = "Extracted"; // Extracted / Applied
        public string SourceFile { get; set; } = ""; // TepTinNguon
        public int? LessonId { get; set; } // MaBaiHoc
        public List<(string key, string text, bool isCorrect)> Options { get; set; } = new();
    }
}

