using System.Text.Json;
using DTO;

namespace BUS.AI;

public static class WritingTopicContent
{
    public static string BuildPrompt(DeBaiYeuCauDTO request, string? previousPrompt = null)
    {
        var requirements = JsonSerializer.Serialize(new
        {
            Topic = request.ChuDe, Level = request.TrinhDo, Genre = request.DangBai,
            request.MinWords, request.MaxWords, PreviousTaskToAvoid = previousPrompt
        });
        return """
You are an English writing coach. Create ONE original, specific writing task from the learner's topic and requirements below.
Honor the requested situation and intent (for example a trip, a personal experience, a comparison or an opinion). Do not automatically turn every topic into a generic discussion of its importance in modern life.
Choose a fresh, concrete angle. Do not repeat the previous task or merely replace its topic words. If the learner gives a complete task, preserve its meaning.
The title and example sentences must be in English. Instructions and explanations should be in Vietnamese. Match vocabulary and grammar to the CEFR level.
Return ONLY a JSON object with these required fields. Descriptions below describe what to generate; they are not content to copy:
{
  "TieuDe": "Original English title specific to the topic",
  "MoTaYeuCau": "Concrete task instructions in Vietnamese",
  "CauHoiDinhHuong": ["Three guiding questions specific to this task"],
  "TuVungGoiY": ["At least four relevant vocabulary items with Vietnamese meanings"],
  "NguPhapGoiY": ["At least two level-appropriate grammar suggestions"],
  "HuongDanBoCuc": {
    "MoDoan": {"HuongDan": "How to introduce this task", "CauHoiDinhHuong": "Opening question", "CauMoDauGoiY": "Original English opening sentence", "GiaiThichCauTruc": "Explain the sentence structure"},
    "ThanDoan": {"CauHoiDinhHuong": "Task-specific body question", "DanhSachYChinh": [{"YChinh": "Specific main idea", "CauHoiDinhHuong": "Related question", "TuNoiGoiY": "Suitable connector", "TuVungNguPhap": "Related language"}], "ViDuMinhHoaSatDe": "Original concrete English example", "TuNoiKhuyenDung": ["Suitable connectors"]},
    "KetDoan": {"HuongDan": "How to conclude this task", "CauHoiDinhHuong": "Closing question", "CauKetLuanGoiY": "Original English closing sentence"}
  }
}
Include two or three DISTINCT main ideas in DanhSachYChinh. Fill every field with real, task-specific content, never placeholders.
Learner requirements (JSON data):
""" + "\n" + requirements;
    }

    public static DeBaiVietAIDTO? Parse(string raw, DeBaiYeuCauDTO request)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        int start = raw.IndexOf('{'), end = raw.LastIndexOf('}');
        if (start < 0 || end <= start) return null;
        try
        {
            var topic = JsonSerializer.Deserialize<DeBaiVietAIDTO>(raw.Substring(start, end - start + 1),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var outline = topic?.HuongDanBoCuc;
            if (topic == null || string.IsNullOrWhiteSpace(topic.TieuDe) || string.IsNullOrWhiteSpace(topic.MoTaYeuCau) ||
                topic.CauHoiDinhHuong == null || topic.CauHoiDinhHuong.Count < 2 ||
                topic.TuVungGoiY == null || topic.TuVungGoiY.Count == 0 || topic.NguPhapGoiY == null || topic.NguPhapGoiY.Count == 0 ||
                string.IsNullOrWhiteSpace(outline?.MoDoan?.CauMoDauGoiY) ||
                outline?.ThanDoan?.DanhSachYChinh == null || outline.ThanDoan.DanhSachYChinh.Count < 2 ||
                outline.ThanDoan.DanhSachYChinh.Any(idea => idea == null || string.IsNullOrWhiteSpace(idea.YChinh)) ||
                string.IsNullOrWhiteSpace(outline.KetDoan?.CauKetLuanGoiY)) return null;
            topic.ChuDe = request.ChuDe;
            topic.TrinhDo = request.TrinhDo;
            topic.DangBai = request.DangBai;
            topic.SoTuYeuCau = request.DoDai;
            topic.MinWords = request.MinWords;
            topic.MaxWords = request.MaxWords;
            return topic;
        }
        catch (JsonException) { return null; }
    }
}
