using BUS;
using DTO;
using System.Text.Json;

namespace EAI.Tests;

public class LessonQuizTests
{
    private static List<CauHoiDTO> Questions() => Enumerable.Range(1, 10).Select(i => new CauHoiDTO
    {
        NoiDungCauHoi = $"Choose the correct verb for situation {i}: She ___ to school.",
        DanhSachLuaChon = new() { "goes", "go", "going", "gone" },
        DapAnDungIndex = 0,
        GiaiThichChiTiet = "She là chủ ngữ số ít nên động từ thêm s."
    }).ToList();

    [Fact]
    public void CompleteQuestionsSurviveWrappedAndFencedJson()
    {
        var json = JsonSerializer.Serialize(new { questions = Questions() });
        var parsed = BaiHocQuizBUS.ParseAndValidateQuestions("```json\n" + json + "\n```", "Grammar", "A1");
        Assert.True(LessonQuizQuality.IsValidSet(parsed));
    }

    [Fact]
    public void MissingQuestionsAreNeverPadded()
    {
        var parsed = BaiHocQuizBUS.ParseAndValidateQuestions(JsonSerializer.Serialize(Questions().Take(8)), "Grammar", "A1");
        Assert.Equal(8, parsed.Count);
        Assert.False(LessonQuizQuality.IsValidSet(parsed));
    }

    [Theory]
    [InlineData("[Consolidation #1] According to the lesson, which rule is correct?")]
    [InlineData("[Practical Case #1] Which approach demonstrates mastery?")]
    [InlineData("[Practice #1] According to the lesson, which rule is correct?")]
    [InlineData("[Application #1] Which statement is most appropriate?")]
    public void OldFallbackQuestionsAreRejected(string text)
    {
        var q = Questions()[0];
        q.NoiDungCauHoi = text;
        Assert.False(LessonQuizQuality.IsValidQuestion(q));
    }

    [Fact]
    public void DuplicateQuestionsAndOptionsAreRejected()
    {
        var questions = Questions();
        questions[1].NoiDungCauHoi = "2. " + questions[0].NoiDungCauHoi;
        Assert.False(LessonQuizQuality.IsValidSet(questions));
        var q = Questions()[0];
        q.DanhSachLuaChon[1] = "A. goes";
        Assert.False(LessonQuizQuality.IsValidQuestion(q));
    }

    [Fact]
    public void BrokenQuestionDoesNotDiscardOtherQuestions()
    {
        var json = JsonSerializer.Serialize(Questions());
        json = "[{\"NoiDungCauHoi\":42}," + json.Substring(1);
        Assert.Equal(10, BaiHocQuizBUS.ParseAndValidateQuestions(json, "Grammar", "A1").Count);
        Assert.Empty(BaiHocQuizBUS.ParseAndValidateQuestions("[{ truncated", "Grammar", "A1"));
    }

    [Fact]
    public void ResultsFromReplacedQuestionsAreNotRestored()
    {
        var quiz = new BaiHocQuizDTO { DanhSachCauHoi = Questions() };
        for (int i = 0; i < 10; i++) quiz.DanhSachCauHoi[i].MaCauHoi = 100 + i;
        var result = new KetQuaLamBaiBoCauHoiDTO
        {
            ChiTietTraLoiJson = JsonSerializer.Serialize(Enumerable.Range(0, 10).Select(i => new ChiTietLuaChonQuizDTO { MaCauHoi = i }))
        };
        Assert.False(BaiHocQuizBUS.KetQuaThuocBoCauHoi(quiz, result));
        result.ChiTietTraLoiJson = JsonSerializer.Serialize(quiz.DanhSachCauHoi.Select(q => new ChiTietLuaChonQuizDTO { MaCauHoi = q.MaCauHoi }));
        Assert.True(BaiHocQuizBUS.KetQuaThuocBoCauHoi(quiz, result));
    }
}
