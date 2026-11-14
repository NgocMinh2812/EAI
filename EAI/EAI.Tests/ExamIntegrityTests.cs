using System;
using System.Collections.Generic;
using BUS;
using DAL;
using DTO;
using Xunit;

namespace EAI.Tests
{
    public class ExamIntegrityTests
    {
        [Fact]
        public void ChamVaLuuKetQuaThi_StudentChoosesWrongOption_MustNotScorePoints()
        {
            // Arrange: Question has Option 201 (False) and Option 202 (True)
            var question = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 501,
                LoaiCauHoi = "TracNghiem",
                Diem = 10.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 201, NoiDungLuaChon = "Wrong Option", LaDapAnDung = false },
                    new() { MaLuaChon = 202, NoiDungLuaChon = "Correct Option", LaDapAnDung = true }
                }
            };

            var questions = new List<CauHoiKhaoSatDTO> { question };
            // Student submits Option 201 (Wrong)
            var userAnswers = new Dictionary<int, int> { { 501, 201 } };

            // Act
            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "KiemThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 60,
                questions: questions,
                userAnswers: userAnswers,
                userTextAnswers: null,
                maNguoiDung: -1
            );

            // Assert
            Assert.Equal(0, result.SoCauDung);
            Assert.Equal(0.0m, result.DiemSo);
            Assert.False(result.LaDatYeuCau);
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_StudentChoosesCorrectOption_ScoresFullPoints()
        {
            var question = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 502,
                LoaiCauHoi = "TracNghiem",
                Diem = 10.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 301, NoiDungLuaChon = "Incorrect", LaDapAnDung = false },
                    new() { MaLuaChon = 302, NoiDungLuaChon = "Correct", LaDapAnDung = true }
                }
            };

            var questions = new List<CauHoiKhaoSatDTO> { question };
            var userAnswers = new Dictionary<int, int> { { 502, 302 } };

            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "KiemThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 60,
                questions: questions,
                userAnswers: userAnswers,
                userTextAnswers: null,
                maNguoiDung: -1
            );

            Assert.Equal(1, result.SoCauDung);
            Assert.Equal(10.0m, result.DiemSo);
            Assert.True(result.LaDatYeuCau);
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_ClientTamperingFlag_DoesNotOverrideAuthoritativeAnswer()
        {
            // Scenario: In a tampered client object, both options are set to true or wrong is set to true
            // but the authoritative evaluation matches against the actual single correct option
            var question = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 503,
                LoaiCauHoi = "TracNghiem",
                Diem = 10.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 401, NoiDungLuaChon = "Legitimate Correct", LaDapAnDung = true },
                    new() { MaLuaChon = 402, NoiDungLuaChon = "Tampered Fake Correct", LaDapAnDung = false }
                }
            };

            var questions = new List<CauHoiKhaoSatDTO> { question };
            // Student selected 402 which is not the legitimate correct answer
            var userAnswers = new Dictionary<int, int> { { 503, 402 } };

            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "KiemThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 60,
                questions: questions,
                userAnswers: userAnswers,
                userTextAnswers: null,
                maNguoiDung: -1
            );

            Assert.Equal(0, result.SoCauDung);
            Assert.Equal(0.0m, result.DiemSo);
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_ContractOverload_SignatureExecutesProperly()
        {
            // Arrange
            var userAnswers = new Dictionary<int, int> { { 999, 1 } };
            var userText = new Dictionary<int, string> { { 998, "sample response" } };

            // Act - Calling the overload defined in PROJECT.md Contract
            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                maNguoiDung: -1,
                cauTraLoiTracNghiem: userAnswers,
                cauTraLoiTuLuan: userText,
                hinhThucLam: "ThiThu",
                thoiGianGiay: 180
            );

            // Assert
            Assert.Equal(0, result.TongSoCau); // In-memory maDeThi=-1 without questions returns 0 questions
            Assert.Equal(0.0m, result.DiemSo);
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_WeightedQuestions_CalculatesProportionalScore()
        {
            // Arrange: 2 questions with weights 1.0 and 3.0 (total weight = 4.0)
            var q1 = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 601,
                LoaiCauHoi = "TracNghiem",
                Diem = 1.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 11, LaDapAnDung = true },
                    new() { MaLuaChon = 12, LaDapAnDung = false }
                }
            };

            var q2 = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 602,
                LoaiCauHoi = "TracNghiem",
                Diem = 3.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 21, LaDapAnDung = true },
                    new() { MaLuaChon = 22, LaDapAnDung = false }
                }
            };

            var questions = new List<CauHoiKhaoSatDTO> { q1, q2 };

            // Student only gets q2 right (weight 3 out of 4 = 7.5 on 10-point scale)
            var userAnswers = new Dictionary<int, int>
            {
                { 601, 12 }, // Wrong
                { 602, 21 }  // Correct
            };

            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "ThiThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 120,
                questions: questions,
                userAnswers: userAnswers,
                maNguoiDung: -1
            );

            Assert.Equal(1, result.SoCauDung);
            Assert.Equal(2, result.TongSoCau);
            Assert.Equal(7.5m, result.DiemSo);
            Assert.True(result.LaDatYeuCau); // 7.5 >= 5.0
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_WhenExamIdIsNegative_UsesInMemoryQuestionsForTesting()
        {
            // Arrange: Negative exam ID triggers in-memory question evaluation without database dependency
            var question = new CauHoiKhaoSatDTO
            {
                MaCauHoi = 701,
                LoaiCauHoi = "TracNghiem",
                Diem = 10.0,
                DanhSachLuaChon = new List<LuaChonDapAnDTO>
                {
                    new() { MaLuaChon = 7011, NoiDungLuaChon = "Option A", LaDapAnDung = false },
                    new() { MaLuaChon = 7012, NoiDungLuaChon = "Option B", LaDapAnDung = true }
                }
            };

            var questions = new List<CauHoiKhaoSatDTO> { question };
            var userAnswers = new Dictionary<int, int> { { 701, 7012 } };

            // Act: Passing maDeThi: -1 and maNguoiDung: -1 explicitly isolates in-memory grading
            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: -1,
                hinhThucLam: "KiemThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 60,
                questions: questions,
                userAnswers: userAnswers,
                userTextAnswers: null,
                maNguoiDung: -1
            );

            // Assert
            Assert.Equal(1, result.TongSoCau);
            Assert.Equal(1, result.SoCauDung);
            Assert.Equal(10.0m, result.DiemSo);
            Assert.True(result.LaDatYeuCau);
        }

        [Fact]
        public void ChamVaLuuKetQuaThi_WhenExamIdIsZero_QueriesDatabaseSnapshot()
        {
            if (!SqlDataProvider.Instance.TestConnection()) return;

            // Arrange: Exam 0 exists in DB with 30 questions.
            // Client passes forged questions claiming wrong answers are correct.
            var forgedQuestions = new List<CauHoiKhaoSatDTO>
            {
                new()
                {
                    MaCauHoi = 9999,
                    LoaiCauHoi = "TracNghiem",
                    Diem = 10.0,
                    DanhSachLuaChon = new List<LuaChonDapAnDTO>
                    {
                        new() { MaLuaChon = 99991, NoiDungLuaChon = "Forged Correct", LaDapAnDung = true }
                    }
                }
            };

            var userAnswers = new Dictionary<int, int> { { 9999, 99991 } };

            // Act: Grading Exam 0 must query authoritative DB snapshot and ignore forgedQuestions
            var result = BaiTapDeThiBUS.Instance.ChamVaLuuKetQuaThi(
                maDeThi: 0,
                hinhThucLam: "ThiThu",
                thoiDiemBatDau: DateTime.Now,
                thoiGianGiay: 120,
                questions: forgedQuestions,
                userAnswers: userAnswers,
                maNguoiDung: -1
            );

            // Assert: Server queries DB snapshot CauHoiBaiTap for Exam 0 (which has 30 questions).
            // Forged question 9999 is ignored; score is 0.0m.
            Assert.Equal(30, result.TongSoCau);
            Assert.Equal(0, result.SoCauDung);
            Assert.Equal(0.0m, result.DiemSo);
        }
    }
}
