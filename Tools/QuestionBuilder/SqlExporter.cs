using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QuestionBuilder
{
    public static class SqlExporter
    {
        public static void ExportBatch(string filePath, string batchName, List<QuestionItem> questions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- ====================================================================================");
            sb.AppendLine($"-- SEED QUESTIONS BATCH: {batchName}");
            sb.AppendLine($"-- Số lượng câu hỏi: {questions.Count}");
            sb.AppendLine("-- Đảm bảo: Idempotent (Chạy lại nhiều lần không trùng lặp)");
            sb.AppendLine("-- ====================================================================================");
            sb.AppendLine();
            sb.AppendLine("USE TiengAnhAI;");
            sb.AppendLine("GO");
            sb.AppendLine("SET ANSI_NULLS ON;");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON;");
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine($"PRINT N'Đang nạp {batchName} ({questions.Count} câu hỏi)...';");
            sb.AppendLine();

            foreach (var q in questions)
            {
                string safeSeed = EscapeSql(q.SeedId);
                string safeQuestion = EscapeSql(q.QuestionText);
                string safePassage = q.ReadingPassage != null ? $"N'{EscapeSql(q.ReadingPassage)}'" : "NULL";
                string safeAudio = q.AudioPath != null ? $"N'{EscapeSql(q.AudioPath)}'" : "NULL";
                string safeExplanation = EscapeSql(q.Explanation);
                string safeCitation = EscapeSql(q.Citation);
                string safeSourceFile = EscapeSql(q.SourceFile);
                string lessonIdStr = q.LessonId.HasValue ? q.LessonId.Value.ToString() : "NULL";

                sb.AppendLine($"-- Câu hỏi {q.SeedId} ({q.Skill} - Trình độ {q.LevelId} - Dạng {q.QuestionType} - {q.SourceType})");
                sb.AppendLine($"IF NOT EXISTS (SELECT 1 FROM CauHoi WHERE MaDinhDanhSeed = '{safeSeed}')");
                sb.AppendLine("BEGIN");
                sb.AppendLine("    INSERT INTO CauHoi (MaChuDe, MaTrinhDo, LoaiCauHoi, KyNang, DoKho, NoiDungCauHoi,");
                sb.AppendLine("                        DoanVanDoc, DuongDanAudio, GiaiThichDapAn, LoiGiaiChiTiet, DaKiemDuyet,");
                sb.AppendLine("                        NguoiTao, NguonTao, NgayTao, DoanVanDanChung, LoaiNguon, TepTinNguon, MaBaiHoc, MaDinhDanhSeed)");
                sb.AppendLine($"    VALUES ({q.TopicId}, {q.LevelId}, N'{EscapeSql(q.QuestionType)}', N'{EscapeSql(q.Skill)}', N'{EscapeSql(q.Difficulty)}', N'{safeQuestion}',");
                sb.AppendLine($"            {safePassage}, {safeAudio}, N'{safeExplanation}', N'{safeExplanation}', 1,");
                sb.AppendLine($"            1, N'HocLieuChuan', SYSDATETIME(), N'{safeCitation}', N'{EscapeSql(q.SourceType)}', N'{safeSourceFile}', {lessonIdStr}, '{safeSeed}');");
                sb.AppendLine();
                sb.AppendLine("    DECLARE @MaCH INT = SCOPE_IDENTITY();");

                if (q.Options.Count > 0)
                {
                    sb.AppendLine("    INSERT INTO LuaChonDapAn (MaCauHoi, KyHieuLuaChon, NoiDungLuaChon, LaDapAnDung) VALUES");
                    for (int i = 0; i < q.Options.Count; i++)
                    {
                        var opt = q.Options[i];
                        string comma = (i == q.Options.Count - 1) ? ";" : ",";
                        int correctBit = opt.isCorrect ? 1 : 0;
                        sb.AppendLine($"        (@MaCH, N'{EscapeSql(opt.key)}', N'{EscapeSql(opt.text)}', {correctBit}){comma}");
                    }
                }

                sb.AppendLine("END");
                sb.AppendLine("GO");
                sb.AppendLine();
            }

            sb.AppendLine($"PRINT N'Đã hoàn thành nạp {batchName} thành công!';");
            sb.AppendLine("GO");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string EscapeSql(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Replace("'", "''");
        }
    }
}

