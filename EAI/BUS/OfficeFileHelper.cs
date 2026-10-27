using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using DTO;

namespace BUS
{
    /// <summary>
    /// Tiện ích xử lý đọc và xuất các định dạng file Office (.xlsx, .docx, .json)
    /// Chuẩn OpenXML thuần túy, an toàn, không phụ thuộc thư viện bên ngoài
    /// </summary>
    public static class OfficeFileHelper
    {
        private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        private static readonly XNamespace S = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private static readonly XNamespace R = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        #region 1. ĐỌC TỆP WORD (.docx)
        public static List<string> DocCacDoanVanDocx(string filePath)
        {
            var paragraphs = new List<string>();
            if (!File.Exists(filePath)) return paragraphs;

            using var archive = ZipFile.OpenRead(filePath);
            var entry = archive.GetEntry("word/document.xml");
            if (entry == null) return paragraphs;

            using var stream = entry.Open();
            var doc = XDocument.Load(stream);

            foreach (var p in doc.Descendants(W + "p"))
            {
                var sb = new StringBuilder();
                foreach (var t in p.Descendants(W + "t"))
                {
                    sb.Append(t.Value);
                }
                string text = sb.ToString().Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    paragraphs.Add(text);
                }
            }

            return paragraphs;
        }
        #endregion

        #region 2. ĐỌC TỆP EXCEL (.xlsx)
        public static List<List<string>> DocBangTinhExcel(string filePath)
        {
            var rows = new List<List<string>>();
            if (!File.Exists(filePath)) return rows;

            using var archive = ZipFile.OpenRead(filePath);

            // Nạp SharedStrings (chuỗi dùng chung)
            var sharedStrings = new List<string>();
            var sstEntry = archive.GetEntry("xl/sharedStrings.xml");
            if (sstEntry != null)
            {
                using var sstStream = sstEntry.Open();
                var sstDoc = XDocument.Load(sstStream);
                foreach (var si in sstDoc.Descendants(S + "si"))
                {
                    var sb = new StringBuilder();
                    foreach (var t in si.Descendants(S + "t"))
                    {
                        sb.Append(t.Value);
                    }
                    sharedStrings.Add(sb.ToString());
                }
            }

            // Đọc worksheet đầu tiên
            var sheetEntry = archive.GetEntry("xl/worksheets/sheet1.xml") ?? archive.Entries.FirstOrDefault(e => e.FullName.StartsWith("xl/worksheets/sheet") && e.FullName.EndsWith(".xml"));
            if (sheetEntry == null) return rows;

            using var sheetStream = sheetEntry.Open();
            var sheetDoc = XDocument.Load(sheetStream);

            foreach (var rowElem in sheetDoc.Descendants(S + "row"))
            {
                var rowData = new List<string>();
                int currentCol = 0;

                foreach (var c in rowElem.Elements(S + "c"))
                {
                    string cellRef = c.Attribute("r")?.Value ?? "";
                    int targetCol = CellRefToColIndex(cellRef);
                    while (currentCol < targetCol)
                    {
                        rowData.Add(string.Empty);
                        currentCol++;
                    }

                    string type = c.Attribute("t")?.Value ?? "";
                    string rawVal = c.Element(S + "v")?.Value ?? "";

                    string finalVal = "";
                    if (type == "s" && int.TryParse(rawVal, out int sstIdx) && sstIdx >= 0 && sstIdx < sharedStrings.Count)
                    {
                        finalVal = sharedStrings[sstIdx];
                    }
                    else if (type == "inlineStr")
                    {
                        finalVal = c.Element(S + "is")?.Element(S + "t")?.Value ?? "";
                    }
                    else
                    {
                        finalVal = rawVal;
                    }

                    rowData.Add(finalVal.Trim());
                    currentCol++;
                }

                if (rowData.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    rows.Add(rowData);
                }
            }

            return rows;
        }

        private static int CellRefToColIndex(string cellRef)
        {
            if (string.IsNullOrEmpty(cellRef)) return 0;
            int col = 0;
            foreach (char ch in cellRef)
            {
                if (char.IsLetter(ch))
                {
                    col = col * 26 + (char.ToUpper(ch) - 'A' + 1);
                }
                else break;
            }
            return Math.Max(0, col - 1);
        }
        #endregion

        #region 3. XUẤT TỆP EXCEL (.xlsx) CHUẨN OPENXML (ĐA SHEET)
        public class ExcelSheetData
        {
            public string SheetName { get; set; } = "Sheet1";
            public List<string> Headers { get; set; } = new();
            public List<List<string>> Rows { get; set; } = new();
        }

        public static void TaoTepExcel(string filePath, List<ExcelSheetData> sheets)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);

            // 1. [Content_Types].xml
            var sbContentTypes = new StringBuilder();
            sbContentTypes.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sbContentTypes.AppendLine("<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
            sbContentTypes.AppendLine("  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
            sbContentTypes.AppendLine("  <Default Extension=\"xml\" ContentType=\"application/xml\"/>");
            sbContentTypes.AppendLine("  <Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>");
            sbContentTypes.AppendLine("  <Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>");

            for (int i = 1; i <= sheets.Count; i++)
            {
                sbContentTypes.AppendLine($"  <Override PartName=\"/xl/worksheets/sheet{i}.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>");
            }
            sbContentTypes.AppendLine("</Types>");
            AddArchiveEntry(archive, "[Content_Types].xml", sbContentTypes.ToString());

            // 2. _rels/.rels
            string rels = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""xl/workbook.xml""/>
</Relationships>";
            AddArchiveEntry(archive, "_rels/.rels", rels);

            // 3. xl/_rels/workbook.xml.rels
            var sbWbRels = new StringBuilder();
            sbWbRels.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sbWbRels.AppendLine("<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
            sbWbRels.AppendLine("  <Relationship Id=\"rIdStyles\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>");
            for (int i = 1; i <= sheets.Count; i++)
            {
                sbWbRels.AppendLine($"  <Relationship Id=\"rId{i}\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet{i}.xml\"/>");
            }
            sbWbRels.AppendLine("</Relationships>");
            AddArchiveEntry(archive, "xl/_rels/workbook.xml.rels", sbWbRels.ToString());

            // 4. xl/workbook.xml
            var sbWb = new StringBuilder();
            sbWb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sbWb.AppendLine("<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");
            sbWb.AppendLine("  <sheets>");
            for (int i = 1; i <= sheets.Count; i++)
            {
                string safeName = EscapeXml(sheets[i - 1].SheetName);
                sbWb.AppendLine($"    <sheet name=\"{safeName}\" sheetId=\"{i}\" r:id=\"rId{i}\"/>");
            }
            sbWb.AppendLine("  </sheets>");
            sbWb.AppendLine("</workbook>");
            AddArchiveEntry(archive, "xl/workbook.xml", sbWb.ToString());

            // 5. xl/styles.xml
            string styles = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<styleSheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">
  <fonts count=""2"">
    <font><sz val=""11""/><name val=""Segoe UI""/></font>
    <font><b/><sz val=""11""/><color rgb=""FFFFFFFF""/><name val=""Segoe UI""/></font>
  </fonts>
  <fills count=""3"">
    <fill><patternFill patternType=""none""/></fill>
    <fill><patternFill patternType=""gray125""/></fill>
    <fill><patternFill patternType=""solid""><fgColor rgb=""FF0F4C3A""/><bgColor indexed=""64""/></patternFill></fill>
  </fills>
  <borders count=""1"">
    <border><left/><right/><top/><bottom/><diagonal/></border>
  </borders>
  <cellStyleXfs count=""1"">
    <xf numFmtId=""0"" fontId=""0"" fillId=""0"" borderId=""0""/>
  </cellStyleXfs>
  <cellXfs count=""2"">
    <xf numFmtId=""0"" fontId=""0"" fillId=""0"" borderId=""0"" xfId=""0""/>
    <xf numFmtId=""0"" fontId=""1"" fillId=""2"" borderId=""0"" xfId=""0"" applyFont=""1"" applyFill=""1""/>
  </cellXfs>
</styleSheet>";
            AddArchiveEntry(archive, "xl/styles.xml", styles);

            // 6. Từng sheet xml
            for (int sIdx = 1; sIdx <= sheets.Count; sIdx++)
            {
                var sheet = sheets[sIdx - 1];
                var sbSheet = new StringBuilder();
                sbSheet.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                sbSheet.AppendLine("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
                sbSheet.AppendLine("  <sheetData>");

                int rowIndex = 1;

                // Headers
                if (sheet.Headers != null && sheet.Headers.Count > 0)
                {
                    sbSheet.AppendLine($"    <row r=\"{rowIndex}\">");
                    for (int c = 0; c < sheet.Headers.Count; c++)
                    {
                        string cellRef = GetColLetter(c) + rowIndex;
                        string val = EscapeXml(sheet.Headers[c]);
                        sbSheet.AppendLine($"      <c r=\"{cellRef}\" t=\"inlineStr\" s=\"1\"><is><t>{val}</t></is></c>");
                    }
                    sbSheet.AppendLine("    </row>");
                    rowIndex++;
                }

                // Data Rows
                foreach (var row in sheet.Rows)
                {
                    sbSheet.AppendLine($"    <row r=\"{rowIndex}\">");
                    for (int c = 0; c < row.Count; c++)
                    {
                        string cellRef = GetColLetter(c) + rowIndex;
                        string val = EscapeXml(row[c]);
                        sbSheet.AppendLine($"      <c r=\"{cellRef}\" t=\"inlineStr\"><is><t>{val}</t></is></c>");
                    }
                    sbSheet.AppendLine("    </row>");
                    rowIndex++;
                }

                sbSheet.AppendLine("  </sheetData>");
                sbSheet.AppendLine("</worksheet>");

                AddArchiveEntry(archive, $"xl/worksheets/sheet{sIdx}.xml", sbSheet.ToString());
            }
        }

        private static string GetColLetter(int colIndex)
        {
            string col = "";
            colIndex++;
            while (colIndex > 0)
            {
                int mod = (colIndex - 1) % 26;
                col = (char)('A' + mod) + col;
                colIndex = (colIndex - mod) / 26;
            }
            return col;
        }

        private static void AddArchiveEntry(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
            using var entryStream = entry.Open();
            using var writer = new StreamWriter(entryStream, Encoding.UTF8);
            writer.Write(content);
        }

        private static string EscapeXml(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }
        #endregion

        #region 4. XUẤT TỆP MẪU WORD (.docx)
        public static void TaoTepMauWord(string filePath, string title, List<CauHoiDTO> questions)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);

            // [Content_Types].xml
            string contentTypes = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Types xmlns=""http://schemas.openxmlformats.org/package/2006/content-types"">
  <Default Extension=""rels"" ContentType=""application/vnd.openxmlformats-package.relationships+xml""/>
  <Default Extension=""xml"" ContentType=""application/xml""/>
  <Override PartName=""/word/document.xml"" ContentType=""application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml""/>
</Types>";
            AddArchiveEntry(archive, "[Content_Types].xml", contentTypes);

            // _rels/.rels
            string rels = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""word/document.xml""/>
</Relationships>";
            AddArchiveEntry(archive, "_rels/.rels", rels);

            // word/document.xml
            var sbDoc = new StringBuilder();
            sbDoc.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sbDoc.AppendLine("<w:document xmlns:w=\"http://schemas.openxmlformats.org/wordprocessingml/2006/main\">");
            sbDoc.AppendLine("  <w:body>");

            // Header title
            sbDoc.AppendLine($"    <w:p><w:pPr><w:jc w:val=\"center\"/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val=\"36\"/><w:color w:val=\"0F4C3A\"/></w:rPr><w:t>{EscapeXml(title)}</w:t></w:r></w:p>");
            sbDoc.AppendLine("    <w:p><w:pPr><w:spacing w:after=\"200\"/></w:pPr></w:p>");

            int index = 1;
            foreach (var q in questions)
            {
                sbDoc.AppendLine($"    <w:p><w:r><w:rPr><w:b/><w:color w:val=\"0F172A\"/></w:rPr><w:t>Question {index}: {EscapeXml(q.NoiDungCauHoi)}</w:t></w:r></w:p>");
                if (q.DanhSachLuaChon != null)
                {
                    for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                    {
                        char pfx = (char)('A' + i);
                        string opt = CauHoiDTO.StripOptionPrefix(q.DanhSachLuaChon[i]);
                        sbDoc.AppendLine($"    <w:p><w:r><w:t>{pfx}. {EscapeXml(opt)}</w:t></w:r></w:p>");
                    }
                }

                char correctLetter = (q.DapAnDungIndex.HasValue && q.DapAnDungIndex.Value >= 0) ? (char)('A' + q.DapAnDungIndex.Value) : 'A';
                sbDoc.AppendLine($"    <w:p><w:r><w:rPr><w:b/><w:color w:val=\"10B981\"/></w:rPr><w:t>Answer: {correctLetter}</w:t></w:r></w:p>");
                if (!string.IsNullOrWhiteSpace(q.GiaiThichChiTiet))
                {
                    sbDoc.AppendLine($"    <w:p><w:r><w:rPr><w:color w:val=\"64748B\"/></w:rPr><w:t>Explanation: {EscapeXml(q.GiaiThichChiTiet)}</w:t></w:r></w:p>");
                }
                sbDoc.AppendLine("    <w:p><w:pPr><w:spacing w:after=\"160\"/></w:pPr></w:p>");
                index++;
            }

            sbDoc.AppendLine("  </w:body>");
            sbDoc.AppendLine("</w:document>");

            AddArchiveEntry(archive, "word/document.xml", sbDoc.ToString());
        }
        #endregion
    }
}
