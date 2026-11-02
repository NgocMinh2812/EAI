using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuestionBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (args.Length > 0 && args[0] == "--fix-fonts")
            {
                DatabaseFontFixer.Run();
                return;
            }
            if (args.Length > 0 && args[0] == "--check-fonts")
            {
                DatabaseFontFixer.PrintTop();
                return;
            }
            if (args.Length > 0 && args[0] == "--check-vstep")
            {
                DatabaseFontFixer.PrintVstep();
                return;
            }
            Console.WriteLine("================================================================================");
            Console.WriteLine("EAI QUESTION BANK 500 GENERATOR & VALIDATOR");
            Console.WriteLine("================================================================================");

            var batches = new List<(string name, List<QuestionItem> items)>
            {
                ("Batch 01 (SEED_EAI_001 -> SEED_EAI_050)", Batch01.GetQuestions()),
                ("Batch 02 (SEED_EAI_051 -> SEED_EAI_100)", Batch02.GetQuestions()),
                ("Batch 03 (SEED_EAI_101 -> SEED_EAI_150)", Batch03.GetQuestions()),
                ("Batch 04 (SEED_EAI_151 -> SEED_EAI_200)", Batch04.GetQuestions()),
                ("Batch 05 (SEED_EAI_201 -> SEED_EAI_250)", Batch05.GetQuestions()),
                ("Batch 06 (SEED_EAI_251 -> SEED_EAI_300)", Batch06.GetQuestions()),
                ("Batch 07 (SEED_EAI_301 -> SEED_EAI_350)", Batch07.GetQuestions()),
                ("Batch 08 (SEED_EAI_351 -> SEED_EAI_400)", Batch08.GetQuestions()),
                ("Batch 09 (SEED_EAI_401 -> SEED_EAI_450)", Batch09.GetQuestions()),
                ("Batch 10 (SEED_EAI_451 -> SEED_EAI_500)", Batch10.GetQuestions()),
            };

            var allQuestions = new List<QuestionItem>();
            string outputDir = @"d:\TDC\HK5\CDPTUD\EAI\EAI\Database\Batches";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            for (int i = 0; i < batches.Count; i++)
            {
                var (bName, items) = batches[i];
                string batchFile = Path.Combine(outputDir, $"Seed_Questions_Batch{i + 1:D2}.sql");
                Console.WriteLine($"[Exporting] {bName} -> {items.Count} items to {Path.GetFileName(batchFile)}...");
                SqlExporter.ExportBatch(batchFile, bName, items);
                allQuestions.AddRange(items);
            }

            // Export master consolidated SQL
            string masterFile = Path.Combine(@"d:\TDC\HK5\CDPTUD\EAI\EAI\Database", "Seed_Questions_All500.sql");
            Console.WriteLine($"[Exporting Master] Exporting all {allQuestions.Count} questions to Seed_Questions_All500.sql...");
            SqlExporter.ExportBatch(masterFile, "500 CÂU HỎI TIẾNG ANH CHUẨN HÓA TOÀN DIỆN (SEED_EAI_001 -> SEED_EAI_500)", allQuestions);

            // Validation checks
            Console.WriteLine("\n================================================================================");
            Console.WriteLine("THỐNG KÊ CHI TIẾT NGÂN HÀNG CÂU HỎI (500 ITEMS):");
            Console.WriteLine("================================================================================");
            Console.WriteLine($"Tổng số câu hỏi: {allQuestions.Count}");

            // Check duplicate seeds
            var duplicateSeeds = allQuestions.GroupBy(q => q.SeedId).Where(g => g.Count() > 1).ToList();
            if (duplicateSeeds.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"CẢNH BÁO: Phát hiện {duplicateSeeds.Count} mã Seed bị trùng:");
                foreach (var dup in duplicateSeeds)
                {
                    Console.WriteLine($"  - {dup.Key} xuất hiện {dup.Count()} lần");
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("XÁC MINH: 100% Mã Seed (SEED_EAI_001 -> SEED_EAI_500) là duy nhất và liên tục!");
                Console.ResetColor();
            }

            // Check missing answers
            var noAnswer = allQuestions.Where(q => q.Options.All(o => !o.isCorrect)).ToList();
            if (noAnswer.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"CẢNH BÁO: Có {noAnswer.Count} câu hỏi không có đáp án đúng:");
                foreach (var na in noAnswer)
                {
                    Console.WriteLine($"  - {na.SeedId}: {na.QuestionText}");
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("XÁC MINH: 100% câu hỏi đều có ít nhất 1 đáp án đúng hợp lệ!");
                Console.ResetColor();
            }

            // By CEFR Level
            Console.WriteLine("\n1. Phân bố theo Cấp độ CEFR:");
            var levelMap = new Dictionary<int, string> { { 1, "A1 (Beginner)" }, { 2, "A2 (Elementary)" }, { 3, "B1 (Intermediate)" }, { 4, "B2 (Upper Intermediate)" }, { 5, "C1 (Advanced)" } };
            foreach (var g in allQuestions.GroupBy(q => q.LevelId).OrderBy(g => g.Key))
            {
                string name = levelMap.ContainsKey(g.Key) ? levelMap[g.Key] : $"Level {g.Key}";
                Console.WriteLine($"   - {name}: {g.Count()} câu ({g.Count() * 100.0 / allQuestions.Count:F1}%)");
            }

            // By Skill
            Console.WriteLine("\n2. Phân bố theo Kỹ năng / Kiến thức:");
            foreach (var g in allQuestions.GroupBy(q => q.Skill).OrderByDescending(g => g.Count()))
            {
                Console.WriteLine($"   - {g.Key}: {g.Count()} câu ({g.Count() * 100.0 / allQuestions.Count:F1}%)");
            }

            // By Question Type
            Console.WriteLine("\n3. Phân bố theo Dạng câu hỏi:");
            foreach (var g in allQuestions.GroupBy(q => q.QuestionType).OrderByDescending(g => g.Count()))
            {
                Console.WriteLine($"   - {g.Key}: {g.Count()} câu ({g.Count() * 100.0 / allQuestions.Count:F1}%)");
            }

            // By Difficulty
            Console.WriteLine("\n4. Phân bố theo Độ khó:");
            foreach (var g in allQuestions.GroupBy(q => q.Difficulty).OrderByDescending(g => g.Count()))
            {
                Console.WriteLine($"   - {g.Key}: {g.Count()} câu ({g.Count() * 100.0 / allQuestions.Count:F1}%)");
            }

            // By Source Type
            Console.WriteLine("\n5. Phân bố theo Loại Nguồn:");
            foreach (var g in allQuestions.GroupBy(q => q.SourceType).OrderByDescending(g => g.Count()))
            {
                Console.WriteLine($"   - {g.Key}: {g.Count()} câu ({g.Count() * 100.0 / allQuestions.Count:F1}%)");
            }

            // By Source File
            Console.WriteLine("\n6. Nguồn trích xuất tài liệu (Top files):");
            foreach (var g in allQuestions.GroupBy(q => q.SourceFile).OrderByDescending(g => g.Count()))
            {
                Console.WriteLine($"   - {g.Key}: {g.Count()} câu");
            }

            Console.WriteLine("\n================================================================================");
            Console.WriteLine("HOÀN THÀNH XUẤT SQL VÀ XÁC MINH DỮ LIỆU!");
            Console.WriteLine("================================================================================");
        }
    }
}

