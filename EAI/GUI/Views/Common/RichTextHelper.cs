using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GUI.Views.Common
{
    /// <summary>
    /// Helper hỗ trợ chuyển đổi đa chiều giữa nội dung văn bản (HTML/Markdown/PlainText)
    /// và FlowDocument của WPF RichTextBox, hỗ trợ đầy đủ định dạng như Microsoft Word:
    /// Heading (H1, H2, H3), Căn lề riêng từng đoạn (Trái, Giữa, Phải), Bold, Italic, Underline, Lists.
    /// </summary>
    public static class RichTextHelper
    {
        private static readonly SolidColorBrush TextDarkBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F172A"));

        /// <summary>
        /// Kiểm tra RichTextBox có đang trống không (dùng để hiện/ẩn Placeholder)
        /// </summary>
        public static bool IsDocumentEmpty(RichTextBox rtb)
        {
            if (rtb?.Document == null) return true;
            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return string.IsNullOrWhiteSpace(tr.Text);
        }

        /// <summary>
        /// Chuyển đổi từ chuỗi lưu trong CSDL (HTML, Markdown hoặc Plain Text) thành FlowDocument
        /// </summary>
        public static FlowDocument StringToFlowDocument(string? content)
        {
            var doc = new FlowDocument
            {
                PagePadding = new Thickness(0),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13.5,
                Foreground = TextDarkBrush
            };

            if (string.IsNullOrWhiteSpace(content))
            {
                var emptyP = new Paragraph { Margin = new Thickness(0, 0, 0, 6), LineHeight = 22 };
                doc.Blocks.Add(emptyP);
                return doc;
            }

            string raw = content.Trim();

            // 1. Nếu nội dung chứa thẻ HTML chuẩn (như trong CSDL seed: <h3>, <p>, <ul>...)
            if (Regex.IsMatch(raw, @"<\/?(h[1-6]|p|ul|ol|li|strong|b|em|i|u|br)[\s\/>]", RegexOptions.IgnoreCase))
            {
                ParseHtmlToFlowDocument(raw, doc);
                if (doc.Blocks.Count == 0)
                {
                    doc.Blocks.Add(new Paragraph(new Run(raw)) { Margin = new Thickness(0, 0, 0, 6), LineHeight = 22 });
                }
                return doc;
            }

            // 2. Nếu là văn bản PlainText hoặc Markdown (từ AI sinh mẫu hoặc người dùng dán vào)
            ParsePlainTextToFlowDocument(raw, doc);
            return doc;
        }

        /// <summary>
        /// Phân tích văn bản PlainText / Markdown thành các khối FlowDocument
        /// </summary>
        private static void ParsePlainTextToFlowDocument(string text, FlowDocument doc)
        {
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List? currentList = null;

            foreach (var rawLine in lines)
            {
                string line = rawLine.TrimEnd();

                if (string.IsNullOrWhiteSpace(line))
                {
                    currentList = null;
                    doc.Blocks.Add(new Paragraph { Margin = new Thickness(0, 0, 0, 6), LineHeight = 22 });
                    continue;
                }

                // Tiêu đề H1 (■ hoặc #)
                if (line.StartsWith("■■■ ") || line.StartsWith("### "))
                {
                    currentList = null;
                    string headingText = line.Substring(line.IndexOf(' ') + 1);
                    var p = new Paragraph { FontSize = 15, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 8, 0, 4) };
                    AddFormattedInlines(p, headingText);
                    doc.Blocks.Add(p);
                }
                else if (line.StartsWith("■■ ") || line.StartsWith("## "))
                {
                    currentList = null;
                    string headingText = line.Substring(line.IndexOf(' ') + 1);
                    var p = new Paragraph { FontSize = 17, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 10, 0, 4) };
                    AddFormattedInlines(p, headingText);
                    doc.Blocks.Add(p);
                }
                else if (line.StartsWith("■ ") || line.StartsWith("# "))
                {
                    currentList = null;
                    string headingText = line.Substring(line.IndexOf(' ') + 1);
                    var p = new Paragraph { FontSize = 20, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 12, 0, 6) };
                    AddFormattedInlines(p, headingText);
                    doc.Blocks.Add(p);
                }
                // Bullet List (• hoặc - hoặc *)
                else if (Regex.IsMatch(line, @"^(\•|\-|\*)\s+"))
                {
                    int spaceIdx = line.IndexOf(' ');
                    string itemText = line.Substring(spaceIdx + 1);
                    if (currentList == null || currentList.MarkerStyle != TextMarkerStyle.Disc)
                    {
                        currentList = new List { MarkerStyle = TextMarkerStyle.Disc, Margin = new Thickness(16, 2, 0, 6) };
                        doc.Blocks.Add(currentList);
                    }
                    var pItem = new Paragraph { Margin = new Thickness(0, 0, 0, 3), LineHeight = 22 };
                    AddFormattedInlines(pItem, itemText);
                    currentList.ListItems.Add(new ListItem(pItem));
                }
                // Numbered List (1. 2. 3.)
                else if (Regex.IsMatch(line, @"^\d+[\.\)]\s+"))
                {
                    int spaceIdx = line.IndexOf(' ');
                    string itemText = line.Substring(spaceIdx + 1);
                    if (currentList == null || currentList.MarkerStyle != TextMarkerStyle.Decimal)
                    {
                        currentList = new List { MarkerStyle = TextMarkerStyle.Decimal, Margin = new Thickness(16, 2, 0, 6) };
                        doc.Blocks.Add(currentList);
                    }
                    var pItem = new Paragraph { Margin = new Thickness(0, 0, 0, 3), LineHeight = 22 };
                    AddFormattedInlines(pItem, itemText);
                    currentList.ListItems.Add(new ListItem(pItem));
                }
                else
                {
                    currentList = null;
                    var p = new Paragraph { FontSize = 13.5, Margin = new Thickness(0, 0, 0, 6), LineHeight = 22 };
                    AddFormattedInlines(p, line);
                    doc.Blocks.Add(p);
                }
            }
        }

        /// <summary>
        /// Phân tích mã HTML thành FlowDocument
        /// </summary>
        private static void ParseHtmlToFlowDocument(string html, FlowDocument doc)
        {
            // Trích xuất các block: h1, h2, h3, p, ul, ol
            var blockMatches = Regex.Matches(html, @"<(h1|h2|h3|h4|p|ul|ol)([^>]*)>(.*?)<\/\1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (blockMatches.Count == 0)
            {
                // Fallback nếu không có cấu trúc block rõ ràng
                ParsePlainTextToFlowDocument(Regex.Replace(html, @"<br\s*\/?>", "\n", RegexOptions.IgnoreCase), doc);
                return;
            }

            foreach (Match m in blockMatches)
            {
                string tag = m.Groups[1].Value.ToLowerInvariant();
                string attrs = m.Groups[2].Value;
                string inner = m.Groups[3].Value;

                TextAlignment alignment = TextAlignment.Left;
                if (attrs.Contains("text-align:center", StringComparison.OrdinalIgnoreCase) || attrs.Contains("align=\"center\"", StringComparison.OrdinalIgnoreCase))
                {
                    alignment = TextAlignment.Center;
                }
                else if (attrs.Contains("text-align:right", StringComparison.OrdinalIgnoreCase) || attrs.Contains("align=\"right\"", StringComparison.OrdinalIgnoreCase))
                {
                    alignment = TextAlignment.Right;
                }

                if (tag == "h1")
                {
                    var p = new Paragraph { FontSize = 20, FontWeight = FontWeights.Bold, TextAlignment = alignment, Margin = new Thickness(0, 12, 0, 6) };
                    AddFormattedInlines(p, inner);
                    doc.Blocks.Add(p);
                }
                else if (tag == "h2")
                {
                    var p = new Paragraph { FontSize = 17, FontWeight = FontWeights.Bold, TextAlignment = alignment, Margin = new Thickness(0, 10, 0, 4) };
                    AddFormattedInlines(p, inner);
                    doc.Blocks.Add(p);
                }
                else if (tag == "h3" || tag == "h4")
                {
                    var p = new Paragraph { FontSize = 15, FontWeight = FontWeights.Bold, TextAlignment = alignment, Margin = new Thickness(0, 8, 0, 4) };
                    AddFormattedInlines(p, inner);
                    doc.Blocks.Add(p);
                }
                else if (tag == "p")
                {
                    var p = new Paragraph { FontSize = 13.5, TextAlignment = alignment, Margin = new Thickness(0, 0, 0, 6), LineHeight = 22 };
                    AddFormattedInlines(p, inner);
                    doc.Blocks.Add(p);
                }
                else if (tag == "ul" || tag == "ol")
                {
                    var list = new List
                    {
                        MarkerStyle = tag == "ol" ? TextMarkerStyle.Decimal : TextMarkerStyle.Disc,
                        Margin = new Thickness(16, 2, 0, 6)
                    };
                    var liMatches = Regex.Matches(inner, @"<li>(.*?)<\/li>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    foreach (Match liMatch in liMatches)
                    {
                        var pItem = new Paragraph { Margin = new Thickness(0, 0, 0, 3), LineHeight = 22 };
                        AddFormattedInlines(pItem, liMatch.Groups[1].Value);
                        list.ListItems.Add(new ListItem(pItem));
                    }
                    if (list.ListItems.Count > 0)
                    {
                        doc.Blocks.Add(list);
                    }
                }
            }
        }

        /// <summary>
        /// Phân tích các định dạng nội tuyến: In đậm (<strong>, <b>, **), In nghiêng (<em>, <i>, *), Gạch chân (<u>)
        /// </summary>
        private static void AddFormattedInlines(Paragraph paragraph, string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            // Xử lý đơn giản hóa các thẻ inline
            string normalized = text
                .Replace("<strong>", "<b>").Replace("</strong>", "</b>")
                .Replace("<em>", "<i>").Replace("</em>", "</i>")
                .Replace("<br/>", "\n").Replace("<br>", "\n").Replace("<br />", "\n");

            // Chuyển markdown **text** và *text* thành thẻ để xử lý đồng nhất
            normalized = Regex.Replace(normalized, @"\*\*(.+?)\*\*", "<b>$1</b>");
            normalized = Regex.Replace(normalized, @"\*([^\*]+?)\*", "<i>$1</i>");

            // Duyệt từng đoạn token bằng Regex
            var matches = Regex.Matches(normalized, @"(<[^>]+>|[^<]+)");
            bool isBold = false;
            bool isItalic = false;
            bool isUnderline = false;

            foreach (Match m in matches)
            {
                string val = m.Value;
                if (val.Equals("<b>", StringComparison.OrdinalIgnoreCase)) { isBold = true; continue; }
                if (val.Equals("</b>", StringComparison.OrdinalIgnoreCase)) { isBold = false; continue; }
                if (val.Equals("<i>", StringComparison.OrdinalIgnoreCase)) { isItalic = true; continue; }
                if (val.Equals("</i>", StringComparison.OrdinalIgnoreCase)) { isItalic = false; continue; }
                if (val.Equals("<u>", StringComparison.OrdinalIgnoreCase)) { isUnderline = true; continue; }
                if (val.Equals("</u>", StringComparison.OrdinalIgnoreCase)) { isUnderline = false; continue; }

                if (!val.StartsWith("<"))
                {
                    var lines = val.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i > 0) paragraph.Inlines.Add(new LineBreak());
                        if (lines[i].Length > 0)
                        {
                            var run = new Run(lines[i]);
                            if (isBold) run.FontWeight = FontWeights.Bold;
                            if (isItalic) run.FontStyle = FontStyles.Italic;
                            if (isUnderline) run.TextDecorations = TextDecorations.Underline;
                            paragraph.Inlines.Add(run);
                        }
                    }
                }
            }

            if (paragraph.Inlines.Count == 0)
            {
                paragraph.Inlines.Add(new Run(""));
            }
        }

        /// <summary>
        /// Chuyển đổi FlowDocument của RichTextBox thành chuỗi định dạng HTML chuẩn lưu vào CSDL
        /// </summary>
        public static string FlowDocumentToString(FlowDocument? doc)
        {
            if (doc == null || doc.Blocks.Count == 0) return string.Empty;

            var sb = new StringBuilder();

            foreach (var block in doc.Blocks)
            {
                if (block is Paragraph p)
                {
                    string inner = SerializeInlines(p.Inlines);
                    if (string.IsNullOrWhiteSpace(inner)) continue;

                    string alignAttr = "";
                    if (p.TextAlignment == TextAlignment.Center) alignAttr = " style=\"text-align:center\"";
                    else if (p.TextAlignment == TextAlignment.Right) alignAttr = " style=\"text-align:right\"";

                    if (p.FontSize >= 19)
                    {
                        sb.AppendLine($"<h1{alignAttr}>{inner}</h1>");
                    }
                    else if (p.FontSize >= 16)
                    {
                        sb.AppendLine($"<h2{alignAttr}>{inner}</h2>");
                    }
                    else if (p.FontSize >= 14 && p.FontWeight == FontWeights.Bold)
                    {
                        sb.AppendLine($"<h3{alignAttr}>{inner}</h3>");
                    }
                    else
                    {
                        sb.AppendLine($"<p{alignAttr}>{inner}</p>");
                    }
                }
                else if (block is List list)
                {
                    string listTag = list.MarkerStyle == TextMarkerStyle.Decimal ? "ol" : "ul";
                    sb.AppendLine($"<{listTag}>");
                    foreach (var item in list.ListItems)
                    {
                        var liText = new StringBuilder();
                        foreach (var subBlock in item.Blocks)
                        {
                            if (subBlock is Paragraph subP)
                            {
                                liText.Append(SerializeInlines(subP.Inlines));
                            }
                        }
                        sb.AppendLine($"  <li>{liText}</li>");
                    }
                    sb.AppendLine($"</{listTag}>");
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static string SerializeInlines(InlineCollection inlines)
        {
            var sb = new StringBuilder();
            foreach (var inline in inlines)
            {
                if (inline is Run r)
                {
                    string text = r.Text;
                    if (string.IsNullOrEmpty(text)) continue;

                    if (r.FontWeight == FontWeights.Bold) text = $"<strong>{text}</strong>";
                    if (r.FontStyle == FontStyles.Italic) text = $"<em>{text}</em>";
                    if (r.TextDecorations != null && r.TextDecorations.Count > 0) text = $"<u>{text}</u>";

                    sb.Append(text);
                }
                else if (inline is LineBreak)
                {
                    sb.Append("<br/>");
                }
                else if (inline is Span span)
                {
                    string sub = SerializeInlines(span.Inlines);
                    if (span.FontWeight == FontWeights.Bold) sub = $"<strong>{sub}</strong>";
                    if (span.FontStyle == FontStyles.Italic) sub = $"<em>{sub}</em>";
                    if (span.TextDecorations != null && span.TextDecorations.Count > 0) sub = $"<u>{sub}</u>";
                    sb.Append(sub);
                }
            }
            return sb.ToString();
        }
    }
}
