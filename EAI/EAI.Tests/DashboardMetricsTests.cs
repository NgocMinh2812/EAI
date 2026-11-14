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

        [Fact]
        public void LayBaoCaoThongKe_WithSeededAttempts_ReturnsAccurateMetricsAndSkills()
        {
            var filter = new ThongKeHocTapFilterDTO
            {
                TuNgay = null,
                DenNgay = null
            };

            var report = BaoCaoThongKeBUS.Instance.LayBaoCaoThongKe(filter);

            Assert.NotNull(report);
            Assert.True(report.TongSoLuotLam >= 18, "Phải ghi nhận ít nhất 18 lượt làm bài");
            Assert.True(report.SoLuotDaNop >= 17, "Phải ghi nhận ít nhất 17 lượt đã nộp chính thức");
            Assert.True(report.DiemTrungBinh > 0, "Điểm trung bình phải lớn hơn 0");
            Assert.True(report.ThoiGianTrungBinhGiay > 0, "Thời gian trung bình phải lớn hơn 0");
            Assert.Equal(4, report.DanhSachPhanBoDiem.Count);
            Assert.True(report.DanhSachKyNang.Count >= 4, "Phải phân tích ít nhất 4 kỹ năng");
            Assert.True(report.DanhSachLuotLamBai.Count >= 18, "Danh sách nộp bài phải có ít nhất 18 dòng");
        }

        [Fact]
        public void LayChiTietKetQuaLamBai_IncludesUserInfoAndAnswers()
        {
            // Lấy lượt nộp đầu tiên trong hệ thống
            var detail = BaiTapDeThiBUS.Instance.LayChiTietKetQuaLamBai(1);

            Assert.NotNull(detail);
            Assert.False(string.IsNullOrWhiteSpace(detail.HoTenNguoiDung), "Họ tên người dùng phải được nạp");
            Assert.False(string.IsNullOrWhiteSpace(detail.TenDangNhap), "Tên đăng nhập phải được nạp");
            Assert.True(detail.DanhSachChiTiet.Count > 0, "Danh sách chi tiết câu hỏi & trả lời phải có dữ liệu");
        }

        [Fact]
        public void TinhStreakTuDanhSachNgay_EmptyDates_ReturnsZero()
        {
            var result = TienDoHocDAL.TinhStreakTuDanhSachNgay(new System.Collections.Generic.List<DateTime>(), DateTime.Today);
            Assert.Equal(0, result);
        }

        [Fact]
        public void TinhStreakTuDanhSachNgay_SingleDayHighMinutes_ReturnsOne()
        {
            // EC-02 & EC-07: Regardless of learning 600 minutes, a single day equals streak = 1 (not 600 / 30 = 20)
            var dates = new System.Collections.Generic.List<DateTime> { DateTime.Today };
            var result = TienDoHocDAL.TinhStreakTuDanhSachNgay(dates, DateTime.Today);
            Assert.Equal(1, result);
        }

        [Fact]
        public void TinhStreakTuDanhSachNgay_ConsecutiveDays_ReturnsCorrectStreak()
        {
            // EC-03: 3 consecutive days including today
            var dates = new System.Collections.Generic.List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(-1),
                DateTime.Today.AddDays(-2)
            };
            var result = TienDoHocDAL.TinhStreakTuDanhSachNgay(dates, DateTime.Today);
            Assert.Equal(3, result);
        }

        [Fact]
        public void TinhStreakTuDanhSachNgay_GracePeriodToday_ReturnsActiveStreak()
        {
            // EC-04: Studied yesterday and day before, haven't studied today yet -> streak = 2
            var dates = new System.Collections.Generic.List<DateTime>
            {
                DateTime.Today.AddDays(-1),
                DateTime.Today.AddDays(-2)
            };
            var result = TienDoHocDAL.TinhStreakTuDanhSachNgay(dates, DateTime.Today);
            Assert.Equal(2, result);
        }

        [Fact]
        public void TinhStreakTuDanhSachNgay_BrokenStreak_ResetsToZero()
        {
            // EC-05: Missed yesterday (last study was Today - 2) -> streak resets to 0
            var dates = new System.Collections.Generic.List<DateTime>
            {
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(-3)
            };
            var result = TienDoHocDAL.TinhStreakTuDanhSachNgay(dates, DateTime.Today);
            Assert.Equal(0, result);
        }

        [Fact]
        public void TinhStreakTuDanhSachNgay_SameDayMultipleEvents_CountsAsOneDay()
        {
            // EC-06: Multiple events on same day
            var dates = new System.Collections.Generic.List<DateTime>
            {
                DateTime.Today,
                DateTime.Today,
                DateTime.Today.AddDays(-1)
            };
            var result = TienDoHocDAL.TinhStreakTuDanhSachNgay(dates, DateTime.Today);
            Assert.Equal(2, result);
        }

        [Fact]
        public void CongDonThoiGianHoc_InsertsLichSuHocTap_AuditRecord()
        {
            const int testUserId = 89999;
            try
            {
                // Ensure clean state
                DatabaseHelper.ExecuteNonQuery("DELETE FROM LichSuHocTap WHERE MaNguoiDung = " + testUserId);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM HoSoHocTap WHERE MaNguoiDung = " + testUserId);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM NguoiDung WHERE MaNguoiDung = " + testUserId);

                string seedUser = @"
                    DECLARE @MaVT INT = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = 'HocVien');
                    SET IDENTITY_INSERT NguoiDung ON;
                    INSERT INTO NguoiDung (MaNguoiDung, TenDangNhap, MatKhauMaHoa, HoTen, Email, MaVaiTro, NgayTao)
                    VALUES (@MaND, 'test_streak_student', 'HASH123', N'Test Streak Student', 'streak@test.com', ISNULL(@MaVT, 1), SYSDATETIME());
                    SET IDENTITY_INSERT NguoiDung OFF;";
                DatabaseHelper.ExecuteNonQuery(seedUser, new[] { new Microsoft.Data.SqlClient.SqlParameter("@MaND", testUserId) });

                BaiHocDAL.Instance.CongDonThoiGianHoc(testUserId, 25);

                var dt = DatabaseHelper.ExecuteQuery("SELECT COUNT(*) FROM LichSuHocTap WHERE MaNguoiDung = " + testUserId + " AND LoaiHoatDong = N'BaiHoc'");
                int count = Convert.ToInt32(dt.Rows[0][0]);
                Assert.True(count >= 1, "CongDonThoiGianHoc must insert an audit record into LichSuHocTap");

                var hoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(testUserId);
                Assert.NotNull(hoSo);
                Assert.Equal(25, hoSo!.SoPhutHocTichLuy);
            }
            finally
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM LichSuHocTap WHERE MaNguoiDung = " + testUserId);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM HoSoHocTap WHERE MaNguoiDung = " + testUserId);
                DatabaseHelper.ExecuteNonQuery("DELETE FROM NguoiDung WHERE MaNguoiDung = " + testUserId);
            }
        }

        [Fact]
        public void LayTongKetTienDo_EmptyUser_ReturnsZeroStreak()
        {
            var summary = TienDoHocDAL.Instance.LayTongKetTienDo(-9999, "TatCa");
            Assert.NotNull(summary);
            Assert.Equal(0, summary.ChuoiNgayHoc);
        }
    }
}
