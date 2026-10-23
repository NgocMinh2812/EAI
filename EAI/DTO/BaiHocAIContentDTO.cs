using System;
using System.Collections.Generic;

namespace DTO
{
    /// <summary>
    /// Thẻ học tập linh hoạt theo từng kỹ năng (Listening, Reading, Writing, Speaking, Vocabulary, Grammar)
    /// </summary>
    public class LearningCardDTO
    {
        public string CardType { get; set; } = string.Empty; 
        // CardType: "Objective", "AudioSection", "Passage", "CoreKnowledge", "Vocabulary", 
        //           "Strategy", "Pitfall", "Sample", "Dialogue", "PracticePrompt"

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Icon { get; set; } = "📖";
        public string HeaderColor { get; set; } = "#0F766E";
        public string BackgroundColor { get; set; } = "#F0FDF4";
        public string BorderColor { get; set; } = "#BBF7D0";
        public int OrderIndex { get; set; } = 0;
        public bool IsAiSupplemented { get; set; } = false;

        public List<VocabularyItemDTO>? VocabularyItems { get; set; }
        public List<DialogueLineDTO>? DialogueLines { get; set; }
        public List<string>? KeyPoints { get; set; }
    }

    /// <summary>
    /// Chi tiết từ vựng chuyên sâu trong bài học
    /// </summary>
    public class VocabularyItemDTO
    {
        public string Word { get; set; } = string.Empty;
        public string Ipa { get; set; } = string.Empty;
        public string PartOfSpeech { get; set; } = string.Empty;
        public string Meaning { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
        public string Collocation { get; set; } = string.Empty;
    }

    /// <summary>
    /// Lời thoại trong đoạn hội thoại mẫu
    /// </summary>
    public class DialogueLineDTO
    {
        public string Speaker { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
    }

    /// <summary>
    /// Kết quả phân tích bài học thông minh bằng AI được lưu trữ trong Database
    /// </summary>
    public class BaiHocAIContentDTO
    {
        public int MaNoiDungAI { get; set; }
        public int MaBaiHoc { get; set; }
        public string ContentHash { get; set; } = string.Empty;
        
        // TrangThai: "ChuaTao", "DangXuLy", "HoanThanh", "ThatBai"
        public string TrangThai { get; set; } = "ChuaTao";
        public string? KyNang { get; set; }
        public string? MucTieuBaiHoc { get; set; }
        public List<LearningCardDTO> DanhSachTheHoc { get; set; } = new();
        public string? AudioPathGoc { get; set; }
        public string? TranscriptGoc { get; set; }
        public string? MoHinhAI { get; set; }
        public string? ThongBaoLoi { get; set; }
        public DateTime? ThoiDiemBatDauXuLy { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }
}
