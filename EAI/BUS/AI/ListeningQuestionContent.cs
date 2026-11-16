using DTO;

namespace BUS.AI;

public static class ListeningQuestionContent
{
    public static void PrepareSet(IReadOnlyCollection<CauHoiDTO> questions)
    {
        var scripts = questions.Select(q => !string.IsNullOrWhiteSpace(q.AudioScript) ? q.AudioScript : q.DoanVanDoc)
            .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!.Trim()).Distinct().ToList();
        foreach (var question in questions)
        {
            if (string.IsNullOrWhiteSpace(question.AudioScript) && string.IsNullOrWhiteSpace(question.DoanVanDoc) && scripts.Count == 1)
                question.DoanVanDoc = scripts[0];
            Prepare(question);
        }
    }

    public static void Prepare(CauHoiDTO question)
    {
        var script = !string.IsNullOrWhiteSpace(question.AudioScript) ? question.AudioScript : question.DoanVanDoc;
        if (string.IsNullOrWhiteSpace(script))
            throw new InvalidOperationException("AI chưa tạo lời thoại cho câu hỏi nghe. Vui lòng tạo lại.");
        question.KyNang = "Nghe";
        question.AudioScript = script.Trim();
        question.DoanVanDoc = script.Trim();
    }
}
