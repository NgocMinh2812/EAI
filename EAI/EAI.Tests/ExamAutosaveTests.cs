using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using BUS;
using DAL;
using DTO;
using Xunit;

namespace EAI.Tests
{
    public class ExamAutosaveTests
    {
        [Fact]
        public void LuuCauTraLoiTamThoi_InvalidIds_ReturnsFalseSafely()
        {
            bool result = BaiTapDeThiBUS.Instance.LuuCauTraLoiTamThoi(-1, -1, null, null);
            Assert.False(result);
        }

        [Fact]
        public void LayChiTietTraLoiCuaLuot_InvalidAttemptId_ReturnsEmptyList()
        {
            var list = BaiTapDeThiBUS.Instance.LayChiTietTraLoiCuaLuot(-1);
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public void LayLuotLamBaiDangLam_InvalidIds_ReturnsNull()
        {
            var attempt = BaiTapDeThiBUS.Instance.LayLuotLamBaiDangLam(-1, -1);
            Assert.Null(attempt);
        }

        [Fact]
        public void Autosave_FullLifecycle_UpsertAndResume()
        {
            if (!SqlDataProvider.Instance.TestConnection()) return;

            // 1. Get an existing user, exam, and question from DB
            DataTable dtUser = SqlDataProvider.Instance.ExecuteQuery("SELECT TOP 1 MaNguoiDung FROM NguoiDung WHERE MaVaiTro = 1 AND MaNguoiDung > 0");
            DataTable dtExam = SqlDataProvider.Instance.ExecuteQuery("SELECT TOP 1 MaBaiTapDeThi FROM BaiTapDeThi WHERE MaBaiTapDeThi > 0 ORDER BY MaBaiTapDeThi ASC");
            DataTable dtChoice = SqlDataProvider.Instance.ExecuteQuery("SELECT TOP 2 MaCauHoi, MaLuaChon FROM LuaChonDapAn ORDER BY MaLuaChon ASC");

            if (dtUser.Rows.Count == 0 || dtExam.Rows.Count == 0 || dtChoice.Rows.Count < 2) return;

            int maNguoiDung = Convert.ToInt32(dtUser.Rows[0]["MaNguoiDung"]);
            int maDeThi = Convert.ToInt32(dtExam.Rows[0]["MaBaiTapDeThi"]);
            int maCauHoi = Convert.ToInt32(dtChoice.Rows[0]["MaCauHoi"]);
            int maLuaChon1 = Convert.ToInt32(dtChoice.Rows[0]["MaLuaChon"]);
            int maLuaChon2 = Convert.ToInt32(dtChoice.Rows[1]["MaLuaChon"]);

            // Ensure test user session
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = maNguoiDung,
                TenDangNhap = "test_student",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            // 2. Initialize in-progress attempt
            int maLuot = BaiTapDeThiBUS.Instance.KhoiTaoLuotLamBai(maNguoiDung, maDeThi, "ThiThu", 10);
            Assert.True(maLuot > 0, "KhoiTaoLuotLamBai must generate a valid attempt ID");

            try
            {
                // 3. Verify in-progress attempt detection
                var activeAttempt = BaiTapDeThiBUS.Instance.LayLuotLamBaiDangLam(maNguoiDung, maDeThi);
                Assert.NotNull(activeAttempt);
                Assert.Equal(maLuot, activeAttempt.MaLuotLamBai);
                Assert.Equal("DangLam", activeAttempt.TrangThai);

                // 4. Autosave first answer
                bool save1 = BaiTapDeThiBUS.Instance.LuuCauTraLoiTamThoi(maLuot, maCauHoi, maLuaChon1, null, 15);
                Assert.True(save1, "First autosave must succeed");

                // Verify retrieval
                var savedList1 = BaiTapDeThiBUS.Instance.LayChiTietTraLoiCuaLuot(maLuot);
                Assert.Single(savedList1);
                Assert.Equal(maCauHoi, savedList1[0].MaCauHoi);
                Assert.Equal(maLuaChon1, savedList1[0].MaLuaChonDaChon);

                // 5. Test UPSERT: Change answer on same question (MUST NOT throw UQ_ChiTietTraLoi violation)
                bool save2 = BaiTapDeThiBUS.Instance.LuuCauTraLoiTamThoi(maLuot, maCauHoi, maLuaChon2, "Updated answer", 30);
                Assert.True(save2, "Second autosave UPSERT must succeed");

                var savedList2 = BaiTapDeThiBUS.Instance.LayChiTietTraLoiCuaLuot(maLuot);
                Assert.Single(savedList2); // Still exactly 1 row
                Assert.Equal(maLuaChon2, savedList2[0].MaLuaChonDaChon);
                Assert.Equal("Updated answer", savedList2[0].CauTraLoiVanBan);

                // 6. Final Submit with pre-existing autosaved answers (Verifies UPSERT in LuuKetQuaThi)
                var questions = new List<CauHoiKhaoSatDTO>
                {
                    new()
                    {
                        MaCauHoi = maCauHoi,
                        LoaiCauHoi = "TracNghiem",
                        Diem = 10.0,
                        DanhSachLuaChon = new List<LuaChonDapAnDTO>
                        {
                            new() { MaLuaChon = maLuaChon2, LaDapAnDung = true }
                        }
                    }
                };

                var userAnswers = new Dictionary<int, int> { { maCauHoi, maLuaChon2 } };

                var submitResult = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                    maDeThi: -1, // In-memory grading for test isolation
                    hinhThucLam: "ThiThu",
                    thoiDiemBatDau: activeAttempt.ThoiDiemBatDau,
                    thoiGianGiay: 35,
                    questions: questions,
                    userAnswers: userAnswers,
                    maNguoiDung: maNguoiDung,
                    maLuotLamBaiHienTai: maLuot
                );

                Assert.Equal(maLuot, submitResult.MaLuotLamBai);
                Assert.Equal(1, submitResult.SoCauDung);
                Assert.Equal(10.0m, submitResult.DiemSo);

                // Verify LuotLamBai status updated to DaNop
                var completedAttempt = BaiTapDeThiDAL.Instance.LayChiTietKetQuaLamBai(maLuot);
                Assert.NotNull(completedAttempt);
                Assert.Equal("DaNop", completedAttempt.TrangThai);
            }
            finally
            {
                // Clean up test records
                SqlDataProvider.Instance.ExecuteNonQuery("DELETE FROM LichSuHocTap WHERE MaLuotLamBai = @MaLuot", new[] { new SqlParameter("@MaLuot", maLuot) });
                SqlDataProvider.Instance.ExecuteNonQuery("DELETE FROM ChiTietTraLoi WHERE MaLuotLamBai = @MaLuot", new[] { new SqlParameter("@MaLuot", maLuot) });
                SqlDataProvider.Instance.ExecuteNonQuery("DELETE FROM LuotLamBai WHERE MaLuotLamBai = @MaLuot", new[] { new SqlParameter("@MaLuot", maLuot) });
            }
        }
    }
}
