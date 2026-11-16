using BUS.AI;
using DTO;
using System.Text.Json;

namespace EAI.Tests;

public class WritingTopicTests
{
    private static DeBaiYeuCauDTO Request() => new() { ChuDe = "a trip to Da Lat", TrinhDo = "B1", MinWords = 120, MaxWords = 180, DoDai = "120-180 words" };

    [Fact]
    public void PromptIncludesCustomTopicAndPreviousTask()
    {
        var prompt = WritingTopicContent.BuildPrompt(Request(), "My first holiday");
        Assert.Contains("a trip to Da Lat", prompt);
        Assert.Contains("My first holiday", prompt);
        Assert.DoesNotContain("My Favorite Learning Experience", prompt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("{ truncated")]
    [InlineData("{\"TieuDe\":\"The Importance of Travel in Modern Life\"}")]
    public void InvalidOrIncompleteResultIsRejectedWithoutManufacturedOutline(string json)
    {
        Assert.Null(WritingTopicContent.Parse(json, Request()));
    }

    [Fact]
    public void GeneratedTopicAndOutlineArePreservedWithRequestedLevelAndLength()
    {
        var original = new DeBaiVietAIDTO
        {
            TieuDe = "A Rainy Afternoon in Da Lat",
            MoTaYeuCau = "Describe a memorable afternoon during your trip.",
            CauHoiDinhHuong = new() { "Where did you go?", "What happened?" },
            TuVungGoiY = new() { "shelter" }, NguPhapGoiY = new() { "Past simple" },
            HuongDanBoCuc = new()
            {
                MoDoan = new() { CauMoDauGoiY = "Last summer I visited Da Lat." },
                ThanDoan = new() { DanhSachYChinh = new() { new() { YChinh = "The unexpected rain" }, new() { YChinh = "A warm cafe" } } },
                KetDoan = new() { CauKetLuanGoiY = "That afternoon became my favorite memory." }
            }
        };
        var result = WritingTopicContent.Parse("```json\n" + JsonSerializer.Serialize(original) + "\n```", Request());
        Assert.NotNull(result);
        Assert.Equal(original.TieuDe, result.TieuDe);
        Assert.Equal(original.HuongDanBoCuc.MoDoan.CauMoDauGoiY, result.HuongDanBoCuc.MoDoan.CauMoDauGoiY);
        Assert.Equal(120, result.MinWords);
        Assert.Equal(180, result.MaxWords);
        Assert.Equal(Request().ChuDe, result.ChuDe);
    }
}
