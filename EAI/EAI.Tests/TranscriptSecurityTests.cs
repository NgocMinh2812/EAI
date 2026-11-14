using System;
using System.Collections.Generic;
using BUS;
using DAL;
using DTO;
using Microsoft.Data.SqlClient;
using Xunit;

namespace EAI.Tests
{
    public class TranscriptSecurityTests : IDisposable
    {
        private const int TestLessonId = 88888;
        private const string TestAudioPath = "audio/test_secret_listening.mp3";
        private const string TestAudioHash = "SECRET_HASH_88888_ABC";
        private const string SecretTranscript = "This is a strictly confidential listening exam transcript that must never leak.";
        private const string SecretScript = "Confidential audio script.";
        private const string SecretSegmentsJson = "[{\"id\":1,\"start\":0.0,\"end\":4.5,\"text\":\"This is a strictly confidential\"}]";

        public TranscriptSecurityTests()
        {
            // Seed a test transcript with ChoPhepSinhVienXem = false
            CleanupTestData();

            const string seedLessonSql = @"
                IF NOT EXISTS (SELECT 1 FROM BaiHoc WHERE MaBaiHoc = @MaBaiHoc)
                BEGIN
                    DECLARE @MaCD INT = (SELECT TOP 1 MaChuDe FROM ChuDe);
                    DECLARE @MaTD INT = (SELECT TOP 1 MaTrinhDo FROM TrinhDo);
                    IF @MaCD IS NULL
                    BEGIN
                        INSERT INTO ChuDe (TenChuDe, MoTa) VALUES (N'General English', N'Mô tả chủ đề');
                        SET @MaCD = SCOPE_IDENTITY();
                    END;
                    IF @MaTD IS NULL
                    BEGIN
                        INSERT INTO TrinhDo (MaCapDo, TenTrinhDo) VALUES (N'B1', N'Trung cấp');
                        SET @MaTD = SCOPE_IDENTITY();
                    END;

                    SET IDENTITY_INSERT BaiHoc ON;
                    INSERT INTO BaiHoc (MaBaiHoc, MaChuDe, MaTrinhDo, TieuDe, LoaiBaiHoc, TomTat, DaXuatBan)
                    VALUES (@MaBaiHoc, @MaCD, @MaTD, N'Test Listening Lesson', N'Nghe', N'Tóm tắt bài nghe kiểm tra', 1);
                    SET IDENTITY_INSERT BaiHoc OFF;
                END;";

            DatabaseHelper.ExecuteNonQuery(seedLessonSql, new[] { new SqlParameter("@MaBaiHoc", TestLessonId) });

            const string insertSql = @"
                INSERT INTO BaiHocAudioTranscript (
                    MaBaiHoc, AudioPath, LoaiAudio, TieuDeAudio, ScriptGoc, TranscriptText,
                    TranscriptSegmentsJson, Provider, MoHinhSTT, MoHinhTTS, VoiceName,
                    AudioHash, TrangThaiDuyet, ChoPhepSinhVienXem, ThoiLuongGiay, DungLuongBytes,
                    NguoiTao, NgayTao, NgayCapNhat
                )
                VALUES (
                    @MaBaiHoc, @AudioPath, N'ExamListening', N'Secret Exam Listening', @ScriptGoc, @TranscriptText,
                    @SegmentsJson, N'Groq', N'whisper', N'none', N'diana',
                    @AudioHash, N'DaDuyet', 0, 120.0, 1024,
                    1, SYSDATETIME(), SYSDATETIME()
                );";

            DatabaseHelper.ExecuteNonQuery(insertSql, new[]
            {
                new SqlParameter("@MaBaiHoc", TestLessonId),
                new SqlParameter("@AudioPath", TestAudioPath),
                new SqlParameter("@ScriptGoc", SecretScript),
                new SqlParameter("@TranscriptText", SecretTranscript),
                new SqlParameter("@SegmentsJson", SecretSegmentsJson),
                new SqlParameter("@AudioHash", TestAudioHash)
            });
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
                DatabaseHelper.ExecuteNonQuery("DELETE FROM BaiHocAudioTranscript WHERE MaBaiHoc = @MaBaiHoc OR AudioHash = @Hash;", new[]
                {
                    new SqlParameter("@MaBaiHoc", TestLessonId),
                    new SqlParameter("@Hash", TestAudioHash)
                });
                DatabaseHelper.ExecuteNonQuery("DELETE FROM BaiHoc WHERE MaBaiHoc = @MaBaiHoc;", new[]
                {
                    new SqlParameter("@MaBaiHoc", TestLessonId)
                });
            }
            catch { }
        }

        [Fact]
        public void DAL_LayTheoMaBaiHoc_StudentRole_MustReturnEmptyTranscriptFields()
        {
            var transcripts = AudioTranscriptDAL.Instance.LayTheoMaBaiHoc(TestLessonId, laGiangVien: false);
            Assert.NotEmpty(transcripts);
            var item = transcripts[0];

            Assert.False(item.ChoPhepSinhVienXem);
            Assert.Equal(string.Empty, item.TranscriptText);
            Assert.Equal(string.Empty, item.ScriptGoc);
            Assert.Null(item.TranscriptSegmentsJson);
            Assert.Empty(item.Segments);
            Assert.False(item.HasTranscript);
        }

        [Fact]
        public void DAL_LayAudioChinhCuaBaiHoc_StudentRole_MustReturnEmptyTranscriptFields()
        {
            var item = AudioTranscriptDAL.Instance.LayAudioChinhCuaBaiHoc(TestLessonId, laGiangVien: false);
            Assert.NotNull(item);
            Assert.False(item!.ChoPhepSinhVienXem);
            Assert.Equal(string.Empty, item.TranscriptText);
            Assert.Equal(string.Empty, item.ScriptGoc);
            Assert.Null(item.TranscriptSegmentsJson);
            Assert.Empty(item.Segments);
        }

        [Fact]
        public void DAL_LayTheoAudioHash_StudentRole_MustReturnEmptyTranscriptFields()
        {
            var item = AudioTranscriptDAL.Instance.LayTheoAudioHash(TestAudioHash, laGiangVien: false);
            Assert.NotNull(item);
            Assert.Equal(string.Empty, item!.TranscriptText);
            Assert.Equal(string.Empty, item.ScriptGoc);
            Assert.Null(item.TranscriptSegmentsJson);
        }

        [Fact]
        public void DAL_LayTheoDuongDanAudio_StudentRole_MustReturnEmptyTranscriptFields()
        {
            var item = AudioTranscriptDAL.Instance.LayTheoDuongDanAudio(TestAudioPath, laGiangVien: false);
            Assert.NotNull(item);
            Assert.Equal(string.Empty, item!.TranscriptText);
            Assert.Equal(string.Empty, item.ScriptGoc);
            Assert.Null(item.TranscriptSegmentsJson);
        }

        [Fact]
        public void DAL_LecturerRole_MustReturnFullTranscriptEvenIfHiddenFromStudents()
        {
            var item = AudioTranscriptDAL.Instance.LayAudioChinhCuaBaiHoc(TestLessonId, laGiangVien: true);
            Assert.NotNull(item);
            Assert.False(item!.ChoPhepSinhVienXem);
            Assert.Equal(SecretTranscript, item.TranscriptText);
            Assert.Equal(SecretScript, item.ScriptGoc);
            Assert.Equal(SecretSegmentsJson, item.TranscriptSegmentsJson);
            Assert.NotEmpty(item.Segments);
            Assert.True(item.HasTranscript);
        }

        [Fact]
        public void BUS_LayAudioTranscriptChoBaiHoc_StudentSession_MustBeSanitized()
        {
            // Simulate Student Login
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 200,
                TenDangNhap = "student_test",
                TenVaiTro = "HocVien",
                MaVaiTro = 1
            };

            var transcript = BaiHocBUS.LayAudioTranscriptChoBaiHoc(TestLessonId, TestAudioPath);
            Assert.NotNull(transcript);
            Assert.False(transcript!.ChoPhepSinhVienXem);
            Assert.Equal(string.Empty, transcript.TranscriptText);
            Assert.Equal(string.Empty, transcript.ScriptGoc);
            Assert.Null(transcript.TranscriptSegmentsJson);
            Assert.Empty(transcript.Segments);
        }

        [Fact]
        public void BUS_LayAudioTranscriptChoBaiHoc_LecturerSession_MustPreserveTranscript()
        {
            // Simulate Lecturer Login
            SessionHelper.CurrentUser = new NguoiDungDTO
            {
                MaNguoiDung = 10,
                TenDangNhap = "teacher_test",
                TenVaiTro = "GiangVien",
                MaVaiTro = 0
            };

            var transcript = BaiHocBUS.LayAudioTranscriptChoBaiHoc(TestLessonId, TestAudioPath);
            Assert.NotNull(transcript);
            Assert.False(transcript!.ChoPhepSinhVienXem);
            Assert.Equal(SecretTranscript, transcript.TranscriptText);
            Assert.Equal(SecretScript, transcript.ScriptGoc);
        }

        [Fact]
        public void Session_Unauthenticated_DefaultsToStudent_MustBeSanitized()
        {
            SessionHelper.Logout();
            Assert.False(SessionHelper.IsGiangVien);

            var transcript = BaiHocBUS.LayAudioTranscriptChoBaiHoc(TestLessonId, TestAudioPath);
            Assert.NotNull(transcript);
            Assert.Equal(string.Empty, transcript!.TranscriptText);
            Assert.Equal(string.Empty, transcript.ScriptGoc);
            Assert.Null(transcript.TranscriptSegmentsJson);
        }
    }
}
