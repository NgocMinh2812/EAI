using BUS;
using BUS.AI;
using DTO;

namespace EAI.Tests;

public class ListeningAndPracticeTests
{
    [Fact]
    public void SharedGeneratedScriptIsPreservedForEveryQuestion()
    {
        var questions = new List<CauHoiDTO> { new() { DoanVanDoc = "Alice: The train leaves at eight.\nBob: Let's arrive early." }, new() };
        ListeningQuestionContent.PrepareSet(questions);
        Assert.All(questions, q => { Assert.Equal("Nghe", q.KyNang); Assert.Equal(questions[0].DoanVanDoc, q.AudioScript); });
    }

    [Fact]
    public void MissingListeningScriptIsRejectedInsteadOfUsingSample()
    {
        Assert.Throws<InvalidOperationException>(() => ListeningQuestionContent.PrepareSet(new[] { new CauHoiDTO() }));
    }

    [Fact]
    public void DistinctScriptsAreNotOverwritten()
    {
        var questions = new[] { new CauHoiDTO { AudioScript = "First conversation" }, new CauHoiDTO { AudioScript = "Second conversation" } };
        ListeningQuestionContent.PrepareSet(questions);
        Assert.NotEqual(questions[0].DoanVanDoc, questions[1].DoanVanDoc);
    }

    [Theory]
    [InlineData("  IT’S\t SUNNY. ", "it's sunny", true)]
    [InlineData("train station", "TRAIN\u00a0STATION!", true)]
    [InlineData("station", "airport|station", true)]
    [InlineData("airports", "airport", false)]
    [InlineData("...", "airport", false)]
    public void WrittenAnswerUsesQuestionTypeAndNormalizesWhitespace(string answer, string key, bool expected)
    {
        var q = new CauHoiKhaoSatDTO
        {
            LoaiCauHoi = "DienKhuyet",
            DanhSachLuaChon = new()
            {
                new() { MaLuaChon = 1, NoiDungLuaChon = key, LaDapAnDung = true },
                new() { MaLuaChon = 2, NoiDungLuaChon = "distractor" }
            }
        };
        Assert.Equal(expected, LuyenTapBUS.Instance.KiemTraDapAn(q, null, answer).IsCorrect);
    }

    [Fact]
    public void MultipleChoiceStillRequiresSelectedOption()
    {
        var q = new CauHoiKhaoSatDTO { LoaiCauHoi = "TracNghiem", DanhSachLuaChon = new() { new() { MaLuaChon = 42, NoiDungLuaChon = "airport", LaDapAnDung = true } } };
        Assert.False(LuyenTapBUS.Instance.KiemTraDapAn(q, null, "airport").IsCorrect);
        Assert.True(LuyenTapBUS.Instance.KiemTraDapAn(q, 42, null).IsCorrect);
    }
}
