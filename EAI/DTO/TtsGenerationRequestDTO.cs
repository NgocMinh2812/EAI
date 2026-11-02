using System;
using System.Collections.Generic;

namespace DTO
{
    public class DialogueVoiceLineDTO
    {
        public string Speaker { get; set; } = "Speaker 1";
        public string Voice { get; set; } = "diana"; // 'autumn', 'diana', 'hannah', 'austin', 'daniel', 'troy'
        public string Text { get; set; } = string.Empty;
        public double PauseAfterSeconds { get; set; } = 0.4;
    }

    public class TtsGenerationRequestDTO
    {
        public int? MaBaiHoc { get; set; }
        public string Script { get; set; } = string.Empty;
        public string Voice { get; set; } = "diana";
        public string Model { get; set; } = "canopylabs/orpheus-v1-english";
        public bool IsDialogue { get; set; } = false;
        public List<DialogueVoiceLineDTO> DialogueLines { get; set; } = new();
    }
}

