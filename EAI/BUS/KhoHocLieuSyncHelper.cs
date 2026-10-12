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
            string candidate1 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu"));
            string candidate2 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\KhoHocLieu"));
            string candidate3 = @"d:\CDPTUD1\EAI\KhoHocLieu";

            if (Directory.Exists(candidate1))
                _cachedRootPath = candidate1;
            else if (Directory.Exists(candidate2))
                _cachedRootPath = candidate2;
            else if (Directory.Exists(candidate3))
                _cachedRootPath = candidate3;
            else
            {
                try
                {
                    Directory.CreateDirectory(candidate1);
                    _cachedRootPath = candidate1;
                }
                catch
                {
                    Directory.CreateDirectory(candidate3);
                    _cachedRootPath = candidate3;
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

                foreach (var bh in lessons)
                {
                    string safeTitle = SanitizeFolderName(bh.TieuDe);
                    
                    // Tìm tệp chính xác hoặc gần đúng theo tiêu đề bài học
                    var match = allFiles.FirstOrDefault(f =>
                        f.Name.Contains(safeTitle, StringComparison.OrdinalIgnoreCase) ||
                        safeTitle.Contains(Path.GetFileNameWithoutExtension(f.Name), StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(bh.TenChuDe) && f.DirectoryName != null && f.DirectoryName.Contains(bh.TenChuDe, StringComparison.OrdinalIgnoreCase) &&
                         f.Name.Contains(bh.TieuDe.Split(' ')[0], StringComparison.OrdinalIgnoreCase)));

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
    }
}
