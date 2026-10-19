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

        public static List<string> GetAllHocLieuRoots()
        {
            var list = new List<string>();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string[] candidates = new[]
            {
                GetKhoHocLieuRoot(),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\..\KhoHocLieu")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\KhoHocLieu")),
                @"d:\TDC\HK5\CDPTUD\EAI\KhoHocLieu",
                @"d:\CDPTUD1\EAI\KhoHocLieu",
                Path.Combine(baseDir, "Data", "KhoHocLieu"),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\..\TaiLieu_TestUpload")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\TaiLieu_TestUpload")),
                @"d:\TDC\HK5\CDPTUD\EAI\TaiLieu_TestUpload",
                @"d:\CDPTUD1\EAI\TaiLieu_TestUpload"
            };

            foreach (var dir in candidates)
            {
                try
                {
                    if (Directory.Exists(dir) && !list.Any(x => x.Equals(dir, StringComparison.OrdinalIgnoreCase)))
                    {
                        list.Add(dir);
                    }
                }
                catch { }
            }

            if (list.Count == 0)
            {
                string fallback = Path.Combine(baseDir, "Data", "KhoHocLieu");
                try { Directory.CreateDirectory(fallback); } catch { }
                list.Add(fallback);
            }

            return list;
        }

        public static string GetKhoHocLieuRoot()
        {
            if (!string.IsNullOrEmpty(_cachedRootPath) && Directory.Exists(_cachedRootPath))
            {
                return _cachedRootPath;
            }

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] preferredRoots = new[]
            {
                @"d:\TDC\HK5\CDPTUD\EAI\KhoHocLieu",
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\..\KhoHocLieu")),
                Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu")),
                Path.Combine(baseDir, "Data", "KhoHocLieu")
            };

            foreach (var r in preferredRoots)
            {
                try
                {
                    if (Directory.Exists(r))
                    {
                        _cachedRootPath = r;
                        return _cachedRootPath;
                    }
                }
                catch { }
            }

            // Nếu chưa có, tự động tạo KhoHocLieu chuẩn tại d:\TDC\HK5\CDPTUD\EAI\KhoHocLieu
            string defaultKho = @"d:\TDC\HK5\CDPTUD\EAI\KhoHocLieu";
            try
            {
                if (!Directory.Exists(defaultKho))
                {
                    Directory.CreateDirectory(defaultKho);
                }
                _cachedRootPath = defaultKho;
                return _cachedRootPath;
            }
            catch
            {
                string fallback = Path.Combine(baseDir, "Data", "KhoHocLieu");
                try { Directory.CreateDirectory(fallback); } catch { }
                _cachedRootPath = fallback;
                return _cachedRootPath;
            }
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
                var roots = GetAllHocLieuRoots();
                List<FileInfo> allFiles = new List<FileInfo>();
                foreach (var r in roots)
                {
                    if (Directory.Exists(r))
                    {
                        try
                        {
                            allFiles.AddRange(Directory.GetFiles(r, "*.*", SearchOption.AllDirectories).Select(f => new FileInfo(f)));
                        }
                        catch { }
                    }
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

        /// <summary>
        /// Chuyển đổi mã HTML thành văn bản thuần có cấu trúc đẹp (tiêu đề ■, gạch đầu dòng •, danh sách số 1. 2., ngắt đoạn tự nhiên)
        /// </summary>
        public static string ChuyenDoiHtmlSangVanBan(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "Nội dung bài học chưa được cập nhật chi tiết.";

            if (!raw.Contains("<") || !raw.Contains(">"))
                return raw.Trim();

            string text = raw;

            // Xử lý tiêu đề h1 - h6
            text = System.Text.RegularExpressions.Regex.Replace(text, @"<h[1-6][^>]*>(.*?)</h[1-6]>", "\n■ $1\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

            // Xử lý danh sách có thứ tự ol li -> 1. 2. 3.
            var olMatches = System.Text.RegularExpressions.Regex.Matches(text, @"<ol[^>]*>(.*?)</ol>", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            foreach (System.Text.RegularExpressions.Match olm in olMatches)
            {
                string olContent = olm.Groups[1].Value;
                var liMatches = System.Text.RegularExpressions.Regex.Matches(olContent, @"<li[^>]*>(.*?)</li>", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                var sbLi = new System.Text.StringBuilder("\n");
                int idx = 1;
                foreach (System.Text.RegularExpressions.Match lim in liMatches)
                {
                    string liText = System.Text.RegularExpressions.Regex.Replace(lim.Groups[1].Value, @"<[^>]+>", "").Trim();
                    sbLi.AppendLine($"{idx}. {liText}");
                    idx++;
                }
                text = text.Replace(olm.Value, sbLi.ToString());
            }

            // Xử lý danh sách không có thứ tự ul li -> • bullet
            text = System.Text.RegularExpressions.Regex.Replace(text, @"<li[^>]*>(.*?)</li>", "\n• $1", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

            // Xử lý ngắt đoạn và xuống dòng
            text = System.Text.RegularExpressions.Regex.Replace(text, @"<br\s*/?>", "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"</p>", "\n\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"<p[^>]*>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"</div[^>]*>", "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"<div[^>]*>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"</?ul[^>]*>", "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"</?ol[^>]*>", "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Loại bỏ toàn bộ các thẻ HTML còn lại (strong, em, b, i, span, v.v.)
            text = System.Text.RegularExpressions.Regex.Replace(text, @"<[^>]+>", "", System.Text.RegularExpressions.RegexOptions.Singleline);

            // Giải mã HTML entities
            text = System.Net.WebUtility.HtmlDecode(text);

            // Chuẩn hóa khoảng trắng và dòng trống liên tiếp
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[ \t]+", " ");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"(\r?\n){3,}", "\n\n");

            return text.Trim();
        }

        private static string CleanFallbackText(string? raw)
        {
            return ChuyenDoiHtmlSangVanBan(raw);
        }

        public static string? TimTepVatLyChoBaiHoc(BaiHocDTO bh)
        {
            if (bh == null) return null;
            if (!string.IsNullOrEmpty(bh.DuongDanTepVatLy) && File.Exists(bh.DuongDanTepVatLy))
                return bh.DuongDanTepVatLy;

            var roots = GetAllHocLieuRoots();
            List<FileInfo> allFiles = new List<FileInfo>();
            foreach (var r in roots)
            {
                if (Directory.Exists(r))
                {
                    try
                    {
                        allFiles.AddRange(Directory.GetFiles(r, "*.*", SearchOption.AllDirectories).Select(f => new FileInfo(f)));
                    }
                    catch { }
                }
            }

            string safeTitle = SanitizeFolderName(bh.TieuDe);
            string normTitle = new string((bh.TieuDe ?? "").Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();

            // 1. Khớp chính xác tên tệp vật lý nếu có
            if (!string.IsNullOrEmpty(bh.TenTepVatLy))
            {
                var byName = allFiles.FirstOrDefault(f => f.Name.Equals(bh.TenTepVatLy, StringComparison.OrdinalIgnoreCase));
                if (byName != null) return byName.FullName;
            }

            // 2. Khớp theo tiêu đề bài học
            var match = allFiles.FirstOrDefault(f =>
                f.Name.Equals($"{safeTitle}.docx", StringComparison.OrdinalIgnoreCase) ||
                f.Name.Equals($"{safeTitle}.pdf", StringComparison.OrdinalIgnoreCase) ||
                f.Name.Contains(safeTitle, StringComparison.OrdinalIgnoreCase) ||
                safeTitle.Contains(Path.GetFileNameWithoutExtension(f.Name), StringComparison.OrdinalIgnoreCase) ||
                (normTitle.Length > 4 && Path.GetFileNameWithoutExtension(f.Name).ToLowerInvariant().Contains(normTitle)));

            if (match != null) return match.FullName;

            // 3. Khớp theo chủ đề và từ khóa
            if (!string.IsNullOrEmpty(bh.TenChuDe))
            {
                var topicFiles = allFiles.Where(f => f.DirectoryName != null && f.DirectoryName.Contains(bh.TenChuDe, StringComparison.OrdinalIgnoreCase)).ToList();
                if (topicFiles.Count > 0)
                {
                    var titleWords = (bh.TieuDe ?? "").Split(new[] { ' ', '-', '_', ':', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Where(w => w.Length > 2).ToList();
                    var best = topicFiles
                        .Select(f => new { File = f, Score = titleWords.Count(w => f.Name.Contains(w, StringComparison.OrdinalIgnoreCase)) })
                        .Where(x => x.Score > 0)
                        .OrderByDescending(x => x.Score)
                        .Select(x => x.File)
                        .FirstOrDefault();

                    if (best != null) return best.FullName;
                }
            }

            return null;
        }

        public static void TaoTepDocxTuBaiHoc(string filePath, BaiHocDTO bh)
        {
            if (File.Exists(filePath))
            {
                try { File.Delete(filePath); } catch { }
            }

            using var zip = System.IO.Compression.ZipFile.Open(filePath, System.IO.Compression.ZipArchiveMode.Create);

            // [Content_Types].xml
            var entryTypes = zip.CreateEntry("[Content_Types].xml");
            using (var sw = new StreamWriter(entryTypes.Open(), System.Text.Encoding.UTF8))
            {
                sw.Write(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Types xmlns=""http://schemas.openxmlformats.org/package/2006/content-types"">
  <Default Extension=""rels"" ContentType=""application/vnd.openxmlformats-package.relationships+xml""/>
  <Default Extension=""xml"" ContentType=""application/xml""/>
  <Override PartName=""/word/document.xml"" ContentType=""application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml""/>
</Types>");
            }

            // _rels/.rels
            var entryRels = zip.CreateEntry("_rels/.rels");
            using (var sw = new StreamWriter(entryRels.Open(), System.Text.Encoding.UTF8))
            {
                sw.Write(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""word/document.xml""/>
</Relationships>");
            }

            // word/document.xml
            var entryDoc = zip.CreateEntry("word/document.xml");
            using (var sw = new StreamWriter(entryDoc.Open(), System.Text.Encoding.UTF8))
            {
                var sb = new System.Text.StringBuilder();
                sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<w:document xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"">
  <w:body>");

                string safeTitle = System.Security.SecurityElement.Escape(bh.TieuDe ?? "HỌC LIỆU");
                sb.Append($@"<w:p><w:pPr><w:jc w:val=""center""/><w:spacing w:after=""240""/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val=""36""/><w:color w:val=""0F4C3A""/></w:rPr><w:t>{safeTitle}</w:t></w:r></w:p>");

                string meta = $"Chủ đề: {bh.TenChuDe} | Trình độ: {bh.MaTrinhDo} | Thời lượng: {bh.ThoiGianUocTinhPhut} phút";
                sb.Append($@"<w:p><w:pPr><w:jc w:val=""center""/><w:spacing w:after=""360""/></w:pPr><w:r><w:rPr><w:i/><w:sz w:val=""20""/><w:color w:val=""64748B""/></w:rPr><w:t>{System.Security.SecurityElement.Escape(meta)}</w:t></w:r></w:p>");

                if (!string.IsNullOrWhiteSpace(bh.TomTat))
                {
                    sb.Append($@"<w:p><w:pPr><w:spacing w:after=""160""/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val=""24""/><w:color w:val=""1E293B""/></w:rPr><w:t>TÓM TẮT BÀI HỌC</w:t></w:r></w:p>");
                    sb.Append($@"<w:p><w:pPr><w:spacing w:after=""300""/></w:pPr><w:r><w:rPr><w:sz w:val=""22""/><w:color w:val=""334155""/></w:rPr><w:t>{System.Security.SecurityElement.Escape(bh.TomTat)}</w:t></w:r></w:p>");
                }

                sb.Append($@"<w:p><w:pPr><w:spacing w:after=""160""/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val=""24""/><w:color w:val=""1E293B""/></w:rPr><w:t>NỘI DUNG GIÁO TRÌNH</w:t></w:r></w:p>");
                var lines = (bh.NoiDung ?? "").Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    string esc = System.Security.SecurityElement.Escape(line);
                    sb.Append($@"<w:p><w:pPr><w:spacing w:after=""120""/></w:pPr><w:r><w:rPr><w:sz w:val=""22""/></w:rPr><w:t xml:space=""preserve"">{esc}</w:t></w:r></w:p>");
                }

                if (bh.DanhSachCauHoi != null && bh.DanhSachCauHoi.Count > 0)
                {
                    sb.Append($@"<w:p><w:pPr><w:spacing w:before=""360"" w:after=""160""/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val=""24""/><w:color w:val=""4F46E5""/></w:rPr><w:t>BỘ CÂU HỎI ÔN TẬP CỦNG CỐ</w:t></w:r></w:p>");
                    int qIdx = 1;
                    foreach (var q in bh.DanhSachCauHoi)
                    {
                        sb.Append($@"<w:p><w:pPr><w:spacing w:before=""160"" w:after=""80""/></w:pPr><w:r><w:rPr><w:b/><w:sz w:val=""22""/></w:rPr><w:t xml:space=""preserve"">Câu {qIdx++}: {System.Security.SecurityElement.Escape(q.NoiDungCauHoi)}</w:t></w:r></w:p>");
                        for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                        {
                            string mark = (i == q.DapAnDungIndex) ? " (Đúng ✓)" : "";
                            sb.Append($@"<w:p><w:pPr><w:ind w:left=""360""/><w:spacing w:after=""60""/></w:pPr><w:r><w:rPr><w:sz w:val=""21""/></w:rPr><w:t xml:space=""preserve"">{System.Security.SecurityElement.Escape(q.DanhSachLuaChon[i] + mark)}</w:t></w:r></w:p>");
                        }
                        if (!string.IsNullOrWhiteSpace(q.GiaiThichChiTiet))
                        {
                            sb.Append($@"<w:p><w:pPr><w:ind w:left=""360""/><w:spacing w:after=""160""/></w:pPr><w:r><w:rPr><w:i/><w:sz w:val=""20""/><w:color w:val=""2563EB""/></w:rPr><w:t xml:space=""preserve"">➔ {System.Security.SecurityElement.Escape(q.GiaiThichChiTiet)}</w:t></w:r></w:p>");
                        }
                    }
                }

                sb.Append(@"<w:sectPr><w:pgSz w:w=""11906"" w:h=""16838""/><w:pgMar w:top=""1440"" w:right=""1440"" w:bottom=""1440"" w:left=""1440""/></w:sectPr>
  </w:body>
</w:document>");

                sw.Write(sb.ToString());
            }
        }

        public static void TaoTepHtmlInAn(string filePath, BaiHocDTO bh)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(@"<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <title>" + System.Net.WebUtility.HtmlEncode(bh.TieuDe ?? "Học liệu") + @"</title>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #1e293b; max-width: 800px; margin: 40px auto; padding: 0 20px; }
        h1 { color: #0f4c3a; text-align: center; margin-bottom: 8px; font-size: 26px; }
        .meta { text-align: center; color: #64748b; font-size: 14px; margin-bottom: 24px; }
        .summary-box { background: #f8fafc; border-left: 4px solid #10b981; padding: 14px 18px; border-radius: 6px; margin-bottom: 24px; font-style: italic; }
        .section-title { font-size: 18px; font-weight: bold; color: #0f172a; margin-top: 24px; margin-bottom: 12px; border-bottom: 1px solid #e2e8f0; padding-bottom: 6px; }
        .content { white-space: pre-wrap; font-size: 15px; }
        .question-card { background: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; padding: 14px; margin-bottom: 12px; }
        .choice { padding: 6px 10px; margin: 4px 0; background: white; border: 1px solid #cbd5e1; border-radius: 6px; font-size: 14px; }
        .choice.correct { border-color: #10b981; background: #ecfdf5; font-weight: bold; color: #047857; }
        .explanation { color: #1d4ed8; font-size: 13.5px; margin-top: 8px; font-style: italic; }
        @media print { body { margin: 20px; max-width: 100%; } }
    </style>
</head>
<body>");

            sb.Append("<h1>" + System.Net.WebUtility.HtmlEncode(bh.TieuDe ?? "Học liệu") + "</h1>");
            sb.Append($"<div class=\"meta\">Chủ đề: {System.Net.WebUtility.HtmlEncode(bh.TenChuDe)} | Trình độ: {bh.MaTrinhDo} | Thời lượng: {bh.ThoiGianUocTinhPhut} phút | Định dạng: {bh.DinhDangTep}</div>");

            if (!string.IsNullOrWhiteSpace(bh.TomTat))
            {
                sb.Append("<div class=\"summary-box\">" + System.Net.WebUtility.HtmlEncode(bh.TomTat) + "</div>");
            }

            sb.Append("<div class=\"section-title\">NỘI DUNG GIÁO TRÌNH LÝ THUYẾT</div>");
            sb.Append("<div class=\"content\">" + System.Net.WebUtility.HtmlEncode(bh.NoiDung) + "</div>");

            if (bh.DanhSachCauHoi != null && bh.DanhSachCauHoi.Count > 0)
            {
                sb.Append("<div class=\"section-title\" style=\"margin-top: 32px;\">BỘ CÂU HỎI TRẮC NGHIỆM ÔN TẬP</div>");
                int qIdx = 1;
                foreach (var q in bh.DanhSachCauHoi)
                {
                    sb.Append("<div class=\"question-card\">");
                    sb.Append($"<strong>Câu {qIdx++}:</strong> {System.Net.WebUtility.HtmlEncode(q.NoiDungCauHoi)}<div style=\"margin-top: 8px;\">");
                    for (int i = 0; i < q.DanhSachLuaChon.Count; i++)
                    {
                        bool isCorrect = (i == q.DapAnDungIndex);
                        string cssClass = isCorrect ? "choice correct" : "choice";
                        string mark = isCorrect ? " ✓" : "";
                        sb.Append($"<div class=\"{cssClass}\">{System.Net.WebUtility.HtmlEncode(q.DanhSachLuaChon[i])}{mark}</div>");
                    }
                    sb.Append("</div>");
                    if (!string.IsNullOrWhiteSpace(q.GiaiThichChiTiet))
                    {
                        sb.Append($"<div class=\"explanation\">➔ {System.Net.WebUtility.HtmlEncode(q.GiaiThichChiTiet)}</div>");
                    }
                    sb.Append("</div>");
                }
            }

            sb.Append("</body></html>");
            File.WriteAllText(filePath, sb.ToString(), System.Text.Encoding.UTF8);
        }
    }
}
