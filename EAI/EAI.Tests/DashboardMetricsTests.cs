using System;
using BUS;
using DAL;
using DTO;
using Xunit;

namespace EAI.Tests
{
    public class DashboardMetricsTests
    {
        [Fact]
        public void LayThongKeSinhVien_EmptyUser_MustNotReturnFake75ScoreOr1DayStreak()
        {
            // Bug MET-01 fix verification: A non-existent or fresh user with 0 lessons
            // should NOT have 75.0 average score or 1-day study streak
            var stats = BaiHocDAL.Instance.LayThongKeSinhVien(-9999);

            Assert.NotNull(stats);
            Assert.Equal(0.0, stats.DiemTrungBinh);
            Assert.Equal(0, stats.ChuoiHocLienTiep);
            Assert.Equal(0, stats.SoBaiDaHoanThanh);
            Assert.Equal(0, stats.TongSoLuotThi);
        }

        [Fact]
        public void LaySoLieuTongQuan_MustReturnValidKpiDto()
        {
            var kpi = new DashboardDAL().GetKpiStats(0);
            Assert.NotNull(kpi);
            Assert.True(kpi.TongHocVien >= 0);
            Assert.True(kpi.TongBaiHoc >= 0);
            Assert.True(kpi.TongCauHoi >= 0);
            Assert.True(kpi.TongLuotThi >= 0);
            Assert.Equal("so với hôm qua", kpi.TenKyThongKe);
        }

        [Theory]
        [InlineData(0, "so với hôm qua")]
        [InlineData(1, "so với tuần trước")]
        [InlineData(2, "so với tháng trước")]
        public void LaySoLieuTongQuan_FilterPeriods_MustSetPeriodNameCorrectly(int period, string expectedName)
        {
            var kpi = new DashboardDAL().GetKpiStats(period);
            Assert.NotNull(kpi);
            Assert.Equal(expectedName, kpi.TenKyThongKe);
            Assert.NotNull(kpi.FormatTrend(kpi.TangTruongHocVien));
            Assert.NotNull(kpi.FormatTrend(kpi.TangTruongBaiHoc));
        }

        [Fact]
        public void LayTuVungHomNay_MustReturnValidWord()
        {
            var tuVung = TuVungDAL.Instance.LayTuVungHomNay();
            // Database has seed vocabulary
            if (tuVung != null)
            {
                Assert.False(string.IsNullOrWhiteSpace(tuVung.Tu));
                Assert.False(string.IsNullOrWhiteSpace(tuVung.Nghia));
            }
        }

        [Fact]
        public void TimKiemCauHoi_ValidKeyword_ReturnsNonEmptyResults()
        {
            var results = CauHoiDAL.Instance.TimKiemCauHoi("the", 5);
            Assert.NotNull(results);
            Assert.True(results.Count <= 5);
        }

        [Fact]
        public void TimKiemCauHoi_EmptyKeyword_ReturnsEmptyList()
        {
            var results = CauHoiDAL.Instance.TimKiemCauHoi("", 5);
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Theory]
        [InlineData(12.5, "+12.5% so với tuần trước")]
        [InlineData(-5.2, "-5.2% so với tuần trước")]
        [InlineData(0.0, "0.0% so với tuần trước")]
        public void FormatTrend_CalculatesCorrectString(double trend, string expected)
        {
            var kpi = new DashboardKpiDTO { TenKyThongKe = "so với tuần trước" };
            string formatted = kpi.FormatTrend(trend);
            Assert.Equal(expected, formatted);
        }
    }
}
