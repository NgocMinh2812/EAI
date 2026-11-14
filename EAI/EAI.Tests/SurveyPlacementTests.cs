using System;
using System.Collections.Generic;
using BUS;
using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using Xunit;

namespace EAI.Tests
{
    public class SurveyPlacementTests : IDisposable
    {
        private const int TestUserId = 87777;

        public SurveyPlacementTests()
        {
            CleanupTestData();
            // Seed a clean user record in NguoiDung and HoSoHocTap
            string sql = @"
                IF NOT EXISTS (SELECT 1 FROM NguoiDung WHERE MaNguoiDung = @MaND)
                BEGIN
                    DECLARE @MaVT INT = (SELECT TOP 1 MaVaiTro FROM VaiTro WHERE TenVaiTro = 'HocVien');
                    SET IDENTITY_INSERT NguoiDung ON;
                    INSERT INTO NguoiDung (MaNguoiDung, TenDangNhap, MatKhauMaHoa, HoTen, Email, MaVaiTro, NgayTao)
                    VALUES (@MaND, 'test_survey_student', 'HASH123', N'Test Survey Student', 'survey@test.com', ISNULL(@MaVT, 1), SYSDATETIME());
                    SET IDENTITY_INSERT NguoiDung OFF;
                END";

            DatabaseHelper.ExecuteNonQuery(sql, new[] { new SqlParameter("@MaND", TestUserId) });
        }

        public void Dispose()
        {
            CleanupTestData();
            SessionHelper.Logout();
        }

        private static void CleanupTestData()
        {
            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM CauTraLoiKhaoSat WHERE MaLuotKhaoSat IN (SELECT MaLuotKhaoSat FROM LuotKhaoSat WHERE MaNguoiDung = @MaND)", new[] { new SqlParameter("@MaND", TestUserId) });
                DatabaseHelper.ExecuteNonQuery("DELETE FROM LuotKhaoSat WHERE MaNguoiDung = @MaND", new[] { new SqlParameter("@MaND", TestUserId) });
                DatabaseHelper.ExecuteNonQuery("DELETE FROM HoSoHocTap WHERE MaNguoiDung = @MaND", new[] { new SqlParameter("@MaND", TestUserId) });
                DatabaseHelper.ExecuteNonQuery("DELETE FROM NguoiDung WHERE MaNguoiDung = @MaND", new[] { new SqlParameter("@MaND", TestUserId) });
            }
            catch { }
        }

        [Fact]
        public void LuuLuotKhaoSat_RapidSuccession_ReturnsSameId_Idempotent()
        {
            int firstId = CauHoiDAL.Instance.LuuLuotKhaoSat(TestUserId, 1, 15, 12, 8.0m, "B2");
            Assert.True(firstId > 0);

            // Immediate second submission for same user
            int secondId = CauHoiDAL.Instance.LuuLuotKhaoSat(TestUserId, 1, 15, 12, 8.0m, "B2");

            // Idempotency: both return the same survey attempt ID
            Assert.Equal(firstId, secondId);

            // Assert exactly 1 record was inserted into LuotKhaoSat
            var dt = DatabaseHelper.ExecuteQuery("SELECT COUNT(*) FROM LuotKhaoSat WHERE MaNguoiDung = @MaND", new[] { new SqlParameter("@MaND", TestUserId) });
            int count = Convert.ToInt32(dt.Rows[0][0]);
            Assert.Equal(1, count);
        }

        [Fact]
        public void DanhGiaThichUng_SurveyRetake_PreservesPersonalizedTargets()
        {
            // Seed student profile with custom IELTS target
            HoSoHocTapDAL.Instance.CapNhatHoSoHocTap(TestUserId, "A2", "IELTS", 7.5m, "Mục tiêu du học cá nhân");

            var hoSoBefore = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(TestUserId);
            Assert.NotNull(hoSoBefore);
            Assert.Equal("IELTS", hoSoBefore!.KhungNangLucMucTieu);
            Assert.Equal(7.5m, hoSoBefore.DiemMucTieu);

            // Re-take survey evaluating to B1
            var dummyQuestions = new List<CauHoiKhaoSatDTO>
            {
                new CauHoiKhaoSatDTO
                {
                    MaCauHoi = 1,
                    MaLuaChonDaChon = 101,
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new LuaChonDapAnDTO { MaLuaChon = 101, LaDapAnDung = true },
                        new LuaChonDapAnDTO { MaLuaChon = 102, LaDapAnDung = false }
                    }
                },
                new CauHoiKhaoSatDTO
                {
                    MaCauHoi = 2,
                    MaLuaChonDaChon = 201,
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new LuaChonDapAnDTO { MaLuaChon = 201, LaDapAnDung = true },
                        new LuaChonDapAnDTO { MaLuaChon = 202, LaDapAnDung = false }
                    }
                },
                new CauHoiKhaoSatDTO
                {
                    MaCauHoi = 3,
                    MaLuaChonDaChon = 301,
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new LuaChonDapAnDTO { MaLuaChon = 301, LaDapAnDung = true },
                        new LuaChonDapAnDTO { MaLuaChon = 302, LaDapAnDung = false }
                    }
                }
            };

            var ketQua = KhaoSatBUS.Instance.DanhGiaThichUng("B1", dummyQuestions, TestUserId);
            Assert.NotNull(ketQua);

            var hoSoAfter = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(TestUserId);
            Assert.NotNull(hoSoAfter);

            // Personalized target MUST BE PRESERVED (Not overwritten to CEFR target)
            Assert.Equal("IELTS", hoSoAfter!.KhungNangLucMucTieu);
            Assert.Equal(7.5m, hoSoAfter.DiemMucTieu);

            // Diagnostic input placement is updated
            Assert.False(string.IsNullOrEmpty(hoSoAfter.TrinhDoDauVao));
            Assert.NotNull(hoSoAfter.DiemDanhGiaDauVao);
        }

        [Fact]
        public void DanhGiaThichUng_NewUser_InitializesDefaultTarget()
        {
            // Clean profile (no target)
            DatabaseHelper.ExecuteNonQuery("DELETE FROM HoSoHocTap WHERE MaNguoiDung = @MaND", new[] { new SqlParameter("@MaND", TestUserId) });

            var dummyQuestions = new List<CauHoiKhaoSatDTO>
            {
                new CauHoiKhaoSatDTO
                {
                    MaCauHoi = 1,
                    MaLuaChonDaChon = 101,
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new LuaChonDapAnDTO { MaLuaChon = 101, LaDapAnDung = true },
                        new LuaChonDapAnDTO { MaLuaChon = 102, LaDapAnDung = false }
                    }
                },
                new CauHoiKhaoSatDTO
                {
                    MaCauHoi = 2,
                    MaLuaChonDaChon = 201,
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new LuaChonDapAnDTO { MaLuaChon = 201, LaDapAnDung = true },
                        new LuaChonDapAnDTO { MaLuaChon = 202, LaDapAnDung = false }
                    }
                }
            };

            var ketQua = KhaoSatBUS.Instance.DanhGiaThichUng("B1", dummyQuestions, TestUserId);
            Assert.NotNull(ketQua);

            var hoSo = HoSoHocTapDAL.Instance.LayTheoMaNguoiDung(TestUserId);
            Assert.NotNull(hoSo);
            Assert.Equal("CEFR", hoSo!.KhungNangLucMucTieu);
            Assert.NotNull(hoSo.DiemMucTieu);
        }
    }
}
