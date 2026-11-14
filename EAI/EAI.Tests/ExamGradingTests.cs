using System;
using System.Collections.Generic;
using BUS;
using DTO;
using Xunit;

namespace EAI.Tests
{
    public class ExamGradingTests
    {
        [Fact]
        public void ChamVaLuuKetQuaThi_TuLuanQuestion_MustBeGradedCorrectly()
        {
            // Arrange: 1 Essay question
            var qEssay = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 101,
                LoaiCauHoi = "TuLuan",
                Diem = 10.0
            };

            var questions = new List<CauHoiKhaoSatDTO> { qEssay };

            // Student enters a 15-word academic response
            string essayText = "Environmental protection requires proactive government policy and community awareness to reduce carbon emissions sustainably.";
            var userTextAnswers = new Dictionary<int, string> { { 101, essayText } };

            // Act
            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "KiemThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 120,
                questions: questions,
                userAnswers: new Dictionary<int, int>(),
                userTextAnswers: userTextAnswers,
                maNguoiDung: -1 // Negative ID avoids DB persistence during unit test
            );

            // Assert: Essay should no longer be dead code, should be graded as correct
            Assert.Equal(1, result.SoCauDung);
            Assert.Equal(10.0m, result.DiemSo);
            Assert.True(result.LaDatYeuCau);
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_DienKhuyetQuestion_MatchesAcceptableAnswers()
        {
            var qFill = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 102,
                LoaiCauHoi = "DienKhuyet",
                Diem = 5.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 1, NoiDungLuaChon = "sustainable|eco-friendly", LaDapAnDung = true }
                }
            };

            var questions = new List<CauHoiKhaoSatDTO> { qFill };
            var userTextAnswers = new Dictionary<int, string> { { 102, "Sustainable." } };

            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "KiemThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 30,
                questions: questions,
                userAnswers: new Dictionary<int, int>(),
                userTextAnswers: userTextAnswers,
                maNguoiDung: -1
            );

            Assert.Equal(1, result.SoCauDung);
            Assert.Equal(10.0m, result.DiemSo); // Scaled to 10.0 max
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_MultipleChoice_CorrectOptionSelected()
        {
            var qMC = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 103,
                LoaiCauHoi = "TracNghiem",
                Diem = 5.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 201, NoiDungLuaChon = "Option A", LaDapAnDung = false },
                    new() { MaLuaChon = 202, NoiDungLuaChon = "Option B", LaDapAnDung = true }
                }
            };

            var questions = new List<CauHoiKhaoSatDTO> { qMC };
            var userAnswers = new Dictionary<int, int> { { 103, 202 } };

            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "KiemThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 30,
                questions: questions,
                userAnswers: userAnswers,
                userTextAnswers: new Dictionary<int, string>(),
                maNguoiDung: -1
            );

            Assert.Equal(1, result.SoCauDung);
            Assert.Equal(10.0m, result.DiemSo);
        }
    }
}
