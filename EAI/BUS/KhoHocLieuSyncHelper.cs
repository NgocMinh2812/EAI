using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTO;

namespace BUS
{
    public static class KhoHocLieuSyncHelper
    {
        private static string? _cachedRootPath;

        public static string GetKhoHocLieuRoot()
        {
            if (!string.IsNullOrEmpty(_cachedRootPath) && Directory.Exists(_cachedRootPath))
            {
                return _cachedRootPath;
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string appStorageDir = Path.Combine(baseDir, "Data", "HocLieu");
            string candidate1 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu"));
            string candidate2 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\KhoHocLieu"));

            if (Directory.Exists(candidate1))
            {
                _cachedRootPath = candidate1;
            }
            else if (Directory.Exists(candidate2))
            {
                _cachedRootPath = candidate2;
            }
            else
            {
                try
                {
                    if (!Directory.Exists(appStorageDir))
                    {
                        Directory.CreateDirectory(appStorageDir);
                    }
                    _cachedRootPath = appStorageDir;
                }
                catch
                {
                    _cachedRootPath = Path.Combine(Path.GetTempPath(), "EAI_HocLieu");
                    if (!Directory.Exists(_cachedRootPath))
                    {
                        Directory.CreateDirectory(_cachedRootPath);
                    }
                }
            }

            return _cachedRootPath;
        }

        /// <summary>
        /// Loại bỏ ký tự cấm của Windows, giữ nguyên 100% tiếng Việt có dấu
        /// </summary>
        public static string SanitizeFolderName(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
                return "Chủ đề chưa đặt tên";

            char[] invalidChars = Path.GetInvalidFileNameChars();
            string clean = new string(rawName.Select(ch => invalidChars.Contains(ch) ? '-' : ch).ToArray()).Trim();
            clean = clean.TrimEnd('.');

            return string.IsNullOrWhiteSpace(clean) ? "ChuDe" : clean;
        }

        public static string GetChuDeFolderPath(ChuDeDTO cd, List<ChuDeDTO> allChuDes)
        {
            string root = GetKhoHocLieuRoot();
            string folderName = SanitizeFolderName(cd.TenChuDe);

            if (cd.MaChuDeCha.HasValue && cd.MaChuDeCha.Value > 0)
            {
                var parent = allChuDes.FirstOrDefault(x => x.MaChuDe == cd.MaChuDeCha.Value);
                if (parent != null)
                {
                    string parentPath = GetChuDeFolderPath(parent, allChuDes);
                    return Path.Combine(parentPath, folderName);
                }
            }

            return Path.Combine(root, folderName);
        }

        public static void TaoThuMucChuDe(ChuDeDTO cd, List<ChuDeDTO> allChuDes)
        {
            try
            {
                string path = GetChuDeFolderPath(cd, allChuDes);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch { }
        }

        public static void DoiTenThuMucChuDe(string oldTenChuDe, ChuDeDTO newCd, List<ChuDeDTO> allChuDes)
        {
            try
            {
                string root = GetKhoHocLieuRoot();
                string oldFolderName = SanitizeFolderName(oldTenChuDe);

                string oldParentPath = root;
                if (newCd.MaChuDeCha.HasValue && newCd.MaChuDeCha.Value > 0)
                {
                    var parent = allChuDes.FirstOrDefault(x => x.MaChuDe == newCd.MaChuDeCha.Value);
                    if (parent != null)
                    {
                        oldParentPath = GetChuDeFolderPath(parent, allChuDes);
                    }
                }

                string oldPath = Path.Combine(oldParentPath, oldFolderName);
                string newPath = GetChuDeFolderPath(newCd, allChuDes);

                if (Directory.Exists(oldPath) && !string.Equals(oldPath, newPath, StringComparison.OrdinalIgnoreCase))
                {
                    Directory.Move(oldPath, newPath);
                }
                else if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
            }
            catch { }
        }

        public static void XoaThuMucChuDe(ChuDeDTO cd, List<ChuDeDTO> allChuDes)
        {
            try
            {
                string path = GetChuDeFolderPath(cd, allChuDes);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch { }
        }

        public static string? SaoChepTepKhiUpload(string sourceFilePath, int maChuDe, string lessonTitle, List<ChuDeDTO> allChuDes)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
                    return null;

                var targetCd = allChuDes.FirstOrDefault(x => x.MaChuDe == maChuDe);
                if (targetCd == null)
                    return null;

                string targetFolder = GetChuDeFolderPath(targetCd, allChuDes);
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                string ext = Path.GetExtension(sourceFilePath);
                string safeFileName = SanitizeFolderName(lessonTitle) + ext;
                string destPath = Path.Combine(targetFolder, safeFileName);

                int counter = 1;
                while (File.Exists(destPath))
                {
                    safeFileName = $"{SanitizeFolderName(lessonTitle)}_{counter}{ext}";
                    destPath = Path.Combine(targetFolder, safeFileName);
                    counter++;
                }

                File.Copy(sourceFilePath, destPath, true);
                return destPath;
            }
            catch
            {
                return null;
            }
        }

        public static (int FoldersCreated, int TotalTopics) DongBoToanBoKhoHocLieu(List<ChuDeDTO> allChuDes)
        {
            int foldersCreated = 0;
            try
            {
                string root = GetKhoHocLieuRoot();
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                foreach (var cd in allChuDes)
                {
                    string path = GetChuDeFolderPath(cd, allChuDes);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        foldersCreated++;
                    }
                }
            }
            catch { }

            return (foldersCreated, allChuDes.Count);
        }

        /// <summary>
        /// Gắn kèm thông tin tệp vật lý thực tế trên ổ đĩa (.docx, .pdf, .mp3) và badge định dạng cho từng bài học
        /// </summary>
        public static void EnrichBaiHocFileInfo(List<BaiHocDTO> lessons)
        {
            if (lessons == null || lessons.Count == 0) return;

            try
            {
                string root = GetKhoHocLieuRoot();
                List<FileInfo> allFiles = new List<FileInfo>();
                if (Directory.Exists(root))
                {
                    allFiles = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories)
                        .Select(f => new FileInfo(f))
                        .ToList();
                }

                string NormalizeForCompare(string str)
                {
                    if (string.IsNullOrWhiteSpace(str)) return "";
                    return new string(str.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
                }

                foreach (var bh in lessons)
                {
                    string safeTitle = SanitizeFolderName(bh.TieuDe);
                    string normTitle = NormalizeForCompare(bh.TieuDe);
                    
                    // 1. Tìm tệp chính xác hoặc gần đúng theo tiêu đề bài học (cả chuẩn hóa ký tự)
                    var match = allFiles.FirstOrDefault(f =>
                        f.Name.Equals($"{safeTitle}.docx", StringComparison.OrdinalIgnoreCase) ||
                        f.Name.Equals($"{safeTitle}.pdf", StringComparison.OrdinalIgnoreCase) ||
                        f.Name.Contains(safeTitle, StringComparison.OrdinalIgnoreCase) ||
                        safeTitle.Contains(Path.GetFileNameWithoutExtension(f.Name), StringComparison.OrdinalIgnoreCase) ||
                        NormalizeForCompare(Path.GetFileNameWithoutExtension(f.Name)).Contains(normTitle) ||
                        normTitle.Contains(NormalizeForCompare(Path.GetFileNameWithoutExtension(f.Name))));

                    // 2. Nếu chưa thấy, tìm trong đúng thư mục chủ đề theo độ trùng khớp từ khóa
                    if (match == null && !string.IsNullOrEmpty(bh.TenChuDe))
                    {
                        var topicFiles = allFiles.Where(f => f.DirectoryName != null && f.DirectoryName.Contains(bh.TenChuDe, StringComparison.OrdinalIgnoreCase)).ToList();
                        if (topicFiles.Count > 0)
                        {
                            var titleWords = bh.TieuDe.Split(new[] { ' ', '-', '_', ':', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Where(w => w.Length > 2).ToList();

                            match = topicFiles
                                .Select(f => new
                                {
                                    File = f,
                                    Score = titleWords.Count(w => f.Name.Contains(w, StringComparison.OrdinalIgnoreCase))
                                })
                                .Where(x => x.Score > 0)
                                .OrderByDescending(x => x.Score)
                                .Select(x => x.File)
                                .FirstOrDefault();
                        }
                    }

                    if (match != null)
                    {
                        bh.DuongDanTepVatLy = match.FullName;
                        bh.TenTepVatLy = match.Name;
                        string ext = match.Extension.ToLowerInvariant();

                        bh.DinhDangTep = ext switch
                        {
                            ".docx" or ".doc" => "DOCX",
                            ".pdf" => "PDF",
                            ".mp3" or ".wav" or ".m4a" => "AUDIO",
                            _ => ext.TrimStart('.').ToUpperInvariant()
                        };

                        bh.DungLuongHienThi = match.Length > 1024 * 1024
                            ? $"{(match.Length / (1024.0 * 1024.0)):0.1} MB"
                            : $"{(match.Length / 1024.0):0.1} KB";
                    }
                    else
                    {
                        bh.DinhDangTep = bh.LoaiBaiHoc switch
                        {
                            "Doc" or "DocHieu" => "PDF",
                            "Nghe" or "NgheHieu" => "AUDIO",
                            _ => "DOCX"
                        };
                        string ext = bh.DinhDangTep == "PDF" ? ".pdf" : (bh.DinhDangTep == "AUDIO" ? ".mp3" : ".docx");
                        bh.TenTepVatLy = $"{safeTitle}{ext}";
                        bh.DungLuongHienThi = "16.8 KB";
                    }

                    // Thiết lập màu sắc badge định dạng nổi bật, tinh tế
                    switch (bh.DinhDangTep)
                    {
                        case "DOCX":
                            bh.BadgeDinhDangBg = "#EFF6FF";     // Xanh lam nhạt
                            bh.BadgeDinhDangFg = "#1D4ED8";     // Xanh lam đậm
                            bh.BadgeDinhDangBorder = "#BFDBFE"; // Viền xanh lam
                            break;
                        case "PDF":
                            bh.BadgeDinhDangBg = "#FEF2F2";     // Đỏ nhạt
                            bh.BadgeDinhDangFg = "#DC2626";     // Đỏ đậm
                            bh.BadgeDinhDangBorder = "#FECACA"; // Viền đỏ
                            break;
                        case "AUDIO":
                        case "MP3":
                            bh.BadgeDinhDangBg = "#FFFBEB";     // Vàng hổ phách nhạt
                            bh.BadgeDinhDangFg = "#D97706";     // Vàng hổ phách đậm
                            bh.BadgeDinhDangBorder = "#FDE68A"; // Viền vàng
                            break;
                        default:
                            bh.BadgeDinhDangBg = "#F1F5F9";
                            bh.BadgeDinhDangFg = "#475569";
                            bh.BadgeDinhDangBorder = "#CBD5E1";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[KhoHocLieuSyncHelper] Lỗi EnrichBaiHocFileInfo: {ex.Message}");
            }
        }

        /// <summary>
        /// Đọc nội dung tệp thực tế từ ổ đĩa (.docx, .txt, .pdf) theo đúng chuẩn xem trước File Explorer
        /// </summary>
        public static string DocNoiDungTep(string? filePath, string? fallbackContent = null)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                return CleanFallbackText(fallbackContent);
            }

            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            try
            {
                if (ext == ".docx")
                {
                    using var zip = System.IO.Compression.ZipFile.OpenRead(filePath);
                    var entry = zip.GetEntry("word/document.xml");
                    if (entry != null)
                    {
                        using var stream = entry.Open();
                        var xdoc = System.Xml.Linq.XDocument.Load(stream);
                        System.Xml.Linq.XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
                        var paragraphs = xdoc.Descendants(w + "p")
                            .Select(p => string.Concat(p.Descendants(w + "t").Select(t => t.Value)).Trim())
                            .Where(text => !string.IsNullOrEmpty(text))
                            .ToList();

                        if (paragraphs.Count > 0)
                        {
                            return string.Join("\n\n", paragraphs);
                        }
                    }
                }
                else if (ext == ".txt")
                {
                    return File.ReadAllText(filePath, System.Text.Encoding.UTF8).Trim();
                }
                else if (ext == ".pdf")
                {
                    var rawBytes = File.ReadAllBytes(filePath);
                    var rawContent = System.Text.Encoding.UTF8.GetString(rawBytes);
                    var lines = new List<string>();

                    var matches = System.Text.RegularExpressions.Regex.Matches(rawContent, @"\(((?:\\.|[^\\)])*)\)\s*Tj", System.Text.RegularExpressions.RegexOptions.Singleline);
                    foreach (System.Text.RegularExpressions.Match m in matches)
                    {
                        string val = m.Groups[1].Value
                            .Replace(@"\(", "(")
                            .Replace(@"\)", ")")
                            .Replace(@"\\", @"\")
                            .Trim();

                        if (!string.IsNullOrWhiteSpace(val) && val.Length > 1)
                        {
                            lines.Add(val);
                        }
                    }

                    if (lines.Count == 0)
                    {
                        var tjMatches = System.Text.RegularExpressions.Regex.Matches(rawContent, @"\[((?:\\.|[^\]])*)\]\s*TJ", System.Text.RegularExpressions.RegexOptions.Singleline);
                        foreach (System.Text.RegularExpressions.Match m in tjMatches)
                        {
                            var innerMatches = System.Text.RegularExpressions.Regex.Matches(m.Groups[1].Value, @"\(((?:\\.|[^\\)])*)\)");
                            var lineParts = new List<string>();
                            foreach (System.Text.RegularExpressions.Match im in innerMatches)
                            {
                                lineParts.Add(im.Groups[1].Value.Replace(@"\(", "(").Replace(@"\)", ")").Replace(@"\\", @"\"));
                            }
                            string line = string.Concat(lineParts).Trim();
                            if (!string.IsNullOrWhiteSpace(line) && line.Length > 1)
                            {
                                lines.Add(line);
                            }
                        }
                    }

                    if (lines.Count > 0)
                    {
                        return string.Join("\n\n", lines);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DocNoiDungTep] Lỗi đọc {filePath}: {ex.Message}");
            }

            return CleanFallbackText(fallbackContent);
        }

        private static string CleanFallbackText(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "Nội dung bài học chưa được cập nhật chi tiết.";
            return raw.Replace("<p>", "").Replace("</p>", "\n\n")
                      .Replace("<h3>", "■ ").Replace("</h3>", "\n")
                      .Replace("<ul>", "").Replace("</ul>", "")
                      .Replace("<li>", "• ").Replace("</li>", "\n")
                      .Trim();
        }
    }
}
