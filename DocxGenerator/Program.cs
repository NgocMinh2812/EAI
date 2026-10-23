using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DocxGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== BẮT ĐẦU TẠO BỘ TÀI LIỆU TEST CASE 5 CỘT CHO MINH VÀ ÊM ===");

            string baseDir = @"d:\TDC\HK5\CDPTUD\EAI";
            string html1Path = Path.Combine(baseDir, "TestCases_Minh_HocVien.html");
            string docx1Path = Path.Combine(baseDir, "TestCases_Sprint1_Sprint2_Minh_HocVien.docx");

            string html2Path = Path.Combine(baseDir, "TestCases_Em_GiangVien_Admin.html");
            string docx2Path = Path.Combine(baseDir, "TestCases_Sprint1_Sprint2_Em_GiangVien_Admin.docx");

            // 1. Build File 1 (Minh - Học viên)
            Console.WriteLine("1. Đang xây dựng nội dung HTML cho FILE 1 (Minh - Học viên)...");
            string html1 = DocumentBuilder.BuildFile1Html();
            File.WriteAllText(html1Path, html1, new UTF8Encoding(false));
            Console.WriteLine($"   -> Đã lưu: {html1Path} ({html1.Length:N0} ký tự)");

            // 2. Build File 2 (Êm - Giảng viên / Admin)
            Console.WriteLine("2. Đang xây dựng nội dung HTML cho FILE 2 (Êm - Giảng viên/Admin)...");
            string html2 = DocumentBuilder.BuildFile2Html();
            File.WriteAllText(html2Path, html2, new UTF8Encoding(false));
            Console.WriteLine($"   -> Đã lưu: {html2Path} ({html2.Length:N0} ký tự)");

            // 3. Convert to DOCX via Word COM
            Console.WriteLine("3. Đang chuyển đổi sang định dạng Word .docx qua Word COM Engine...");
            var sw = Stopwatch.StartNew();

            Type? wordType = Type.GetTypeFromProgID("Word.Application");
            if (wordType == null)
            {
                Console.WriteLine("LỖI: Không tìm thấy Microsoft Word trên hệ thống!");
                return;
            }

            dynamic word = Activator.CreateInstance(wordType)!;
            word.Visible = false;
            word.DisplayAlerts = 0;

            try
            {
                // Convert File 1
                Console.WriteLine("   -> Đang chuyển đổi FILE 1: TestCases_Sprint1_Sprint2_Minh_HocVien.docx");
                dynamic doc1 = word.Documents.Open(html1Path);
                doc1.PageSetup.Orientation = 1; // wdOrientLandscape
                doc1.PageSetup.TopMargin = 34;
                doc1.PageSetup.BottomMargin = 34;
                doc1.PageSetup.LeftMargin = 34;
                doc1.PageSetup.RightMargin = 34;

                // Configure tables: repeat header row and avoid row breaking
                foreach (dynamic tbl in doc1.Tables)
                {
                    try
                    {
                        tbl.Rows[1].HeadingFormat = -1; // wdToggle / True
                        tbl.Rows.AllowBreakAcrossPages = 0; // wdFalse
                    }
                    catch { }
                }

                int pages1 = 0;
                try { pages1 = (int)doc1.ComputeStatistics(2); } catch { } // 2 = wdStatisticPages

                doc1.SaveAs2(docx1Path, 16); // 16 = wdFormatXMLDocument (.docx)
                doc1.Close(false);
                Console.WriteLine($"      Thành công! Kích thước: {new FileInfo(docx1Path).Length:N0} bytes | Số trang: ~{pages1} trang");

                // Convert File 2
                Console.WriteLine("   -> Đang chuyển đổi FILE 2: TestCases_Sprint1_Sprint2_Em_GiangVien_Admin.docx");
                dynamic doc2 = word.Documents.Open(html2Path);
                doc2.PageSetup.Orientation = 1; // wdOrientLandscape
                doc2.PageSetup.TopMargin = 34;
                doc2.PageSetup.BottomMargin = 34;
                doc2.PageSetup.LeftMargin = 34;
                doc2.PageSetup.RightMargin = 34;

                // Configure tables: repeat header row and avoid row breaking
                foreach (dynamic tbl in doc2.Tables)
                {
                    try
                    {
                        tbl.Rows[1].HeadingFormat = -1; // wdToggle / True
                        tbl.Rows.AllowBreakAcrossPages = 0; // wdFalse
                    }
                    catch { }
                }

                int pages2 = 0;
                try { pages2 = (int)doc2.ComputeStatistics(2); } catch { } // 2 = wdStatisticPages

                doc2.SaveAs2(docx2Path, 16); // 16 = wdFormatXMLDocument (.docx)
                doc2.Close(false);
                Console.WriteLine($"      Thành công! Kích thước: {new FileInfo(docx2Path).Length:N0} bytes | Số trang: ~{pages2} trang");

                // Copy to parent folder as well
                string parentDir = @"d:\TDC\HK5\CDPTUD";
                string copy1 = Path.Combine(parentDir, "TestCases_Sprint1_Sprint2_Minh_HocVien.docx");
                string copy2 = Path.Combine(parentDir, "TestCases_Sprint1_Sprint2_Em_GiangVien_Admin.docx");
                File.Copy(docx1Path, copy1, true);
                File.Copy(docx2Path, copy2, true);
                Console.WriteLine($"   -> Đã sao chép 1 bản sang: {parentDir}");
            }
            finally
            {
                word.Quit();
                Marshal.ReleaseComObject(word);
            }

            sw.Stop();
            Console.WriteLine($"=== HOÀN TẤT TOÀN BỘ QUÁ TRÌNH TRONG {sw.ElapsedMilliseconds:N0} MS ===");
        }
    }
}
