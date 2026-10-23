using System;
using System.Collections.Generic;
using System.Text;

namespace DocxGenerator
{
    public class TestCase
    {
        public string Id { get; set; } = "";
        public string Sprint { get; set; } = "";
        public string Module { get; set; } = "";
        public string TaskId { get; set; } = "";
        public string Scenario { get; set; } = "";
        public string Type { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Preconditions { get; set; } = "";
        public string TestData { get; set; } = "";
        public string Steps { get; set; } = "";
        public string Expected { get; set; } = "";
        public string Actual { get; set; } = "";
        public string Status { get; set; } = "Not Run";
        public string Evidence { get; set; } = "";
        public string DefectId { get; set; } = "";
        public string ScreenshotPath { get; set; } = "";
        public string ScreenshotCaption { get; set; } = "";
    }

    public class TraceItem
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string AcceptanceCriteria { get; set; } = "";
        public string TestCaseIds { get; set; } = "";
        public string Note { get; set; } = "";
    }

    public static class HtmlHelper
    {
        public static string GetCss()
        {
            return @"
    @page { 
        size: A4 landscape; 
        margin: 15mm 15mm 15mm 15mm; 
        mso-header-margin: 10mm;
        mso-footer-margin: 10mm;
    }
    body { 
        font-family: 'Times New Roman', Times, serif; 
        font-size: 13pt; 
        color: #000000; 
        line-height: 1.35; 
        margin: 0; 
        padding: 5px; 
        background-color: #FFFFFF; 
    }
    .cover-box { 
        border: 1.5px solid #000000; 
        background: #FFFFFF; 
        padding: 16px 22px; 
        margin-bottom: 20px; 
    }
    .cover-title { 
        font-size: 16pt; 
        font-weight: bold; 
        color: #000000; 
        margin-bottom: 6px; 
        text-transform: uppercase; 
        text-align: center;
    }
    .cover-sub { 
        font-size: 15pt; 
        font-weight: bold; 
        color: #000000; 
        margin-bottom: 12px; 
        text-align: center;
    }
    .meta-table { 
        width: 100%; 
        border-collapse: collapse; 
        font-size: 13pt; 
        color: #000000;
    }
    .meta-table td { 
        padding: 4px 6px; 
        border: none; 
        font-size: 13pt;
        color: #000000;
    }
    .meta-table td.lbl { 
        font-weight: bold; 
        color: #000000; 
        width: 220px; 
    }
    h1, .sprint-header { 
        font-size: 16pt; 
        font-weight: bold; 
        color: #000000; 
        border-bottom: 1.5px solid #000000; 
        padding-bottom: 4px; 
        margin-top: 22px; 
        margin-bottom: 10px; 
        text-transform: uppercase; 
    }
    h2, .module-title { 
        font-size: 15pt; 
        font-weight: bold; 
        color: #000000; 
        margin-top: 16px; 
        margin-bottom: 6px; 
    }
    .module-header {
        margin-top: 14px;
        margin-bottom: 6px;
        border-bottom: 1px dashed #444444;
        padding-bottom: 3px;
    }
    .module-desc {
        font-size: 13pt;
        color: #000000;
        font-style: italic;
    }
    p, li { 
        font-size: 13pt; 
        color: #000000;
        text-align: justify; 
        margin: 5px 0; 
    }
    ul { 
        margin: 4px 0 10px 24px; 
    }
    table.tc-table { 
        width: 100%; 
        border-collapse: collapse; 
        margin: 6px 0 16px 0; 
        font-size: 13pt; 
        color: #000000;
        border: 1px solid #000000; 
    }
    table.tc-table thead { 
        display: table-header-group; 
    }
    table.tc-table th { 
        background-color: #F0F0F0; 
        color: #000000; 
        padding: 6px 8px; 
        border: 1px solid #000000; 
        text-align: left; 
        font-weight: bold; 
        font-size: 13pt; 
    }
    table.tc-table td { 
        padding: 6px 8px; 
        border: 1px solid #000000; 
        vertical-align: top; 
        font-size: 13pt; 
        color: #000000;
        line-height: 1.35; 
    }
    table.tc-table tr { 
        page-break-inside: avoid; 
    }
    .tc-id { 
        font-weight: bold; 
        color: #000000; 
        text-align: center;
        white-space: nowrap; 
    }
    .tc-name { 
        font-weight: bold; 
        color: #000000; 
    }
    .tc-steps { 
        color: #000000; 
    }
    .tc-expected { 
        color: #000000; 
    }
    .tc-result {
        text-align: center;
        vertical-align: middle !important;
        color: #000000;
        white-space: nowrap;
    }
    .chk-box {
        font-family: 'Times New Roman', Times, serif;
        font-size: 12.5pt;
        color: #000000;
    }
    table.tbl-summary, table.tbl-matrix { 
        width: 100%; 
        border-collapse: collapse; 
        margin: 10px 0 16px 0; 
        font-size: 13pt; 
        color: #000000;
        border: 1px solid #000000; 
    }
    table.tbl-summary th, table.tbl-matrix th { 
        background-color: #F0F0F0; 
        color: #000000; 
        padding: 6px 8px; 
        border: 1px solid #000000; 
        text-align: left; 
        font-weight: bold; 
        font-size: 13pt; 
    }
    table.tbl-summary td, table.tbl-matrix td { 
        padding: 6px 8px; 
        border: 1px solid #000000; 
        vertical-align: top; 
        font-size: 13pt;
        color: #000000;
    }
    .page-break { 
        page-break-before: always; 
    }
";
        }

        public static string RenderTableHeader()
        {
            return @"<thead>
        <tr>
            <th style=""width:10%; text-align:center;"">Mã TC</th>
            <th style=""width:18%;"">Tên TC</th>
            <th style=""width:36%;"">Thực nghiệm</th>
            <th style=""width:28%;"">Kết quả mong đợi</th>
            <th style=""width:8%; text-align:center;"">Pass/Fail</th>
        </tr>
    </thead>";
        }

        public static string RenderTableRow(TestCase tc)
        {
            var sb = new StringBuilder();
            sb.Append("    <tr>\n");
            
            // 1. Mã TC (VD: TC-01)
            sb.Append($"        <td class=\"tc-id\"><b>{tc.Id}</b></td>\n");
            
            // 2. Tên TC
            sb.Append($"        <td class=\"tc-name\"><b>{tc.Scenario}</b></td>\n");
            
            // 3. Thực nghiệm
            sb.Append("        <td class=\"tc-steps\">\n");
            if (!string.IsNullOrWhiteSpace(tc.Preconditions))
            {
                sb.Append($"            <b>Chuẩn bị:</b> {tc.Preconditions.Trim()}<br>\n");
            }
            if (!string.IsNullOrWhiteSpace(tc.TestData))
            {
                sb.Append($"            <b>Dữ liệu:</b> {tc.TestData.Trim()}<br>\n");
            }
            string stepsHtml = tc.Steps.Replace("\n", "<br>\n            ");
            sb.Append($"            <b>Các bước thực hiện:</b><br>\n            {stepsHtml}\n");
            sb.Append("        </td>\n");
            
            // 4. Kết quả mong đợi
            string expHtml = tc.Expected.Replace("\n", "<br>\n            ");
            sb.Append($"        <td class=\"tc-expected\">\n            {expHtml}\n        </td>\n");
            
            // 5. Pass/Fail
            sb.Append("        <td class=\"tc-result\">\n");
            sb.Append("            <div style=\"line-height:1.8;\">\n");
            sb.Append("                <span class=\"chk-box\">[ &nbsp; ] Pass</span><br>\n");
            sb.Append("                <span class=\"chk-box\">[ &nbsp; ] Fail</span>\n");
            sb.Append("            </div>\n");
            sb.Append("        </td>\n");
            
            sb.Append("    </tr>");
            return sb.ToString();
        }
    }
}
