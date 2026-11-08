using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using BUS;
using DTO;
using Microsoft.Win32;

namespace GUI.Views.GiangVien
{
    public partial class UploadTaiLieuDialog : Window
    {
        public BaiHocDTO BaiHocData { get; private set; } = new BaiHocDTO();
        private string? _selectedFilePath;
        private string? _selectedAudioFilePath;

        public UploadTaiLieuDialog(List<ChuDeDTO> danhSachChuDe, int? defaultChuDeId = null)
        {
            InitializeComponent();

            // Nạp danh sách chủ đề
            var validChuDes = danhSachChuDe.Where(c => c.MaChuDe > 0).ToList();
            cboChuDe.ItemsSource = validChuDes;

            if (defaultChuDeId.HasValue && defaultChuDeId.Value > 0 && validChuDes.Any(c => c.MaChuDe == defaultChuDeId.Value))
            {
                cboChuDe.SelectedValue = defaultChuDeId.Value;
            }
            else if (validChuDes.Count > 0)
            {
                cboChuDe.SelectedIndex = 0;
            }

            // Nạp danh sách trình độ động từ CSDL
            NapDanhSachTrinhDo();
        }

        private void CboLoaiBaiHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pnlAudioSection == null) return;
            string loai = (cboLoaiBaiHoc.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Doc";
            bool isNghe = string.Equals(loai, "Nghe", StringComparison.OrdinalIgnoreCase) || BaiHocDTO.KiemTraKyNangNghe(loai);
            pnlAudioSection.Visibility = (isNghe || !string.IsNullOrEmpty(_selectedAudioFilePath)) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnBrowseAudio_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new OpenFileDialog
            {
                Title = "Chọn tệp âm thanh bài nghe (Audio Listening)",
                Filter = "Tệp âm thanh (*.mp3;*.wav;*.m4a;*.ogg;*.aac)|*.mp3;*.wav;*.m4a;*.ogg;*.aac",
                Multiselect = false
            };

            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string candTest1 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\TaiLieu_TestUpload"));
                string candKho1 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu"));
                if (Directory.Exists(candTest1)) openDlg.InitialDirectory = candTest1;
                else if (Directory.Exists(candKho1)) openDlg.InitialDirectory = candKho1;
            }
            catch { }

            if (openDlg.ShowDialog() == true)
            {
                UpdateAudioDisplay(openDlg.FileName);
            }
        }

        private void BtnRemoveAudio_Click(object sender, RoutedEventArgs e)
        {
            UpdateAudioDisplay(null);
        }

        private void UpdateAudioDisplay(string? filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                string ext = fileInfo.Extension.ToLowerInvariant();
                var validAudioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".mp3", ".wav", ".m4a", ".ogg", ".aac", ".flac" };
                if (!validAudioExtensions.Contains(ext))
                {
                    Common.ThongBaoDialog.CanhBao(this,
                        $"Tệp '{fileInfo.Name}' không phải là định dạng âm thanh được hỗ trợ.\n\nVui lòng chỉ chọn tệp âm thanh: .mp3, .wav, .m4a, .ogg, .aac",
                        "Định dạng âm thanh không hợp lệ");
                    return;
                }

                _selectedAudioFilePath = filePath;
                double sizeKb = Math.Round(fileInfo.Length / 1024.0, 1);
                string sizeText = sizeKb > 1024 ? $"{Math.Round(sizeKb / 1024.0, 2)} MB" : $"{sizeKb} KB";

                txtAudioNameDisplay.Text = fileInfo.Name;
                txtAudioSizeDisplay.Text = $"Định dạng: {ext.ToUpper()} • Dung lượng: {sizeText} • Đã gắn audio";
                btnRemoveAudio.Visibility = Visibility.Visible;
                pnlAudioSection.Visibility = Visibility.Visible;
            }
            else
            {
                _selectedAudioFilePath = null;
                txtAudioNameDisplay.Text = "Chưa chọn tệp audio (.mp3, .wav, .m4a)";
                txtAudioSizeDisplay.Text = "Nhấn nút bên phải hoặc kéo thả tệp âm thanh vào đây";
                btnRemoveAudio.Visibility = Visibility.Collapsed;

                string loai = (cboLoaiBaiHoc.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Doc";
                bool isNghe = string.Equals(loai, "Nghe", StringComparison.OrdinalIgnoreCase) || BaiHocDTO.KiemTraKyNangNghe(loai);
                pnlAudioSection.Visibility = isNghe ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void NapDanhSachTrinhDo()
        {
            try
            {
                var danhMucBUS = new BUS.DanhMucBUS();
                var danhSachTrinhDo = danhMucBUS.LayDanhSachTrinhDo();
                if (danhSachTrinhDo != null && danhSachTrinhDo.Count > 0)
                {
                    cboTrinhDo.ItemsSource = danhSachTrinhDo;
                    var defaultItem = danhSachTrinhDo.FirstOrDefault(t => t.MaCapDo == "B1") ?? danhSachTrinhDo[0];
                    cboTrinhDo.SelectedItem = defaultItem;
                    cboTrinhDo.SelectedValue = defaultItem.MaCapDo;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UploadTaiLieuDialog] Lỗi nạp trình độ: {ex.Message}");
            }
        }

        private void BtnBrowseFile_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new OpenFileDialog
            {
                Title = "Chọn tệp tài liệu học liệu (.docx, .pdf, .txt)",
                Filter = "Tệp tài liệu văn bản (*.docx;*.pdf;*.txt)|*.docx;*.pdf;*.txt|Word Document (*.docx)|*.docx|Adobe PDF (*.pdf)|*.pdf|Văn bản thuần (*.txt)|*.txt",
                Multiselect = false
            };

            // Ưu tiên mở ngay vào thư mục TaiLieu_TestUpload hoặc KhoHocLieu của dự án
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string candTest1 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\TaiLieu_TestUpload"));
                string candTest2 = @"d:\CDPTUD1\EAI\TaiLieu_TestUpload";
                string candKho1 = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\..\KhoHocLieu"));
                string candKho2 = @"d:\CDPTUD1\EAI\KhoHocLieu";

                if (Directory.Exists(candTest1))
                    openDlg.InitialDirectory = candTest1;
                else if (Directory.Exists(candTest2))
                    openDlg.InitialDirectory = candTest2;
                else if (Directory.Exists(candKho1))
                    openDlg.InitialDirectory = candKho1;
                else if (Directory.Exists(candKho2))
                    openDlg.InitialDirectory = candKho2;
            }
            catch { }

            if (openDlg.ShowDialog() == true)
            {
                ProcessSelectedFile(openDlg.FileName);
            }
        }

        private void ProcessSelectedFile(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                string ext = fileInfo.Extension.ToLowerInvariant();
                string fileName = fileInfo.Name;

                // GIỚI HẠN ĐỊNH DẠNG TÀI LIỆU: Chỉ cho phép .docx, .pdf, .txt
                var validDocExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".docx", ".pdf", ".txt" };
                if (!validDocExtensions.Contains(ext))
                {
                    var audioExts = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".mp3", ".wav", ".m4a", ".ogg", ".aac", ".flac" };
                    if (audioExts.Contains(ext))
                    {
                        Common.ThongBaoDialog.CanhBao(this,
                            $"Tệp bạn vừa chọn '{fileName}' là tệp âm thanh ({ext.ToUpper()}), không phải tài liệu văn bản.\n\n" +
                            "• Mục này dùng để tải lên tệp tài liệu văn bản (.docx, .pdf, .txt) nhằm trích xuất nội dung học tập và sinh câu hỏi.\n\n" +
                            "💡 Hướng dẫn: Nếu đây là bài Luyện nghe (Listening), bạn vui lòng chọn tệp văn bản bài học trước, sau đó chọn loại bài học 'Luyện nghe' và đính kèm tệp âm thanh này tại mục 'Tệp âm thanh bài nghe (Audio)' ở bên dưới.",
                            "Định dạng tệp không hợp lệ");
                    }
                    else
                    {
                        Common.ThongBaoDialog.CanhBao(this,
                            $"Định dạng tệp '{ext}' không được hỗ trợ.\n\nHệ thống chỉ hỗ trợ tải lên các tệp tài liệu văn bản: .docx, .pdf, .txt.",
                            "Định dạng tệp không hợp lệ");
                    }
                    return;
                }

                _selectedFilePath = filePath;
                double sizeKb = Math.Round(fileInfo.Length / 1024.0, 1);
                string sizeText = sizeKb > 1024 ? $"{Math.Round(sizeKb / 1024.0, 2)} MB" : $"{sizeKb} KB";

                txtFileNameDisplay.Text = fileName;
                txtFileSizeDisplay.Text = $"Định dạng: {ext.ToUpper()} • Dung lượng: {sizeText} • Sẵn sàng trích xuất";

                bool isListeningDoc = fileName.ToLower().Contains("listening") || fileName.ToLower().Contains("nghe");

                // Đổi icon tương ứng
                if (ext == ".pdf")
                {
                    icoFileTypeDisplay.Kind = LucideIconKind.FileText;
                    icoFileTypeDisplay.Foreground = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    pnlFileIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEE2E2")!;
                    if (isListeningDoc) cboLoaiBaiHoc.SelectedIndex = 3;
                    else cboLoaiBaiHoc.SelectedIndex = 0; // Đọc hiểu
                }
                else if (ext == ".docx")
                {
                    icoFileTypeDisplay.Kind = LucideIconKind.FileText;
                    icoFileTypeDisplay.Foreground = (Brush)new BrushConverter().ConvertFrom("#2563EB")!;
                    pnlFileIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#DBEAFE")!;
                    if (isListeningDoc) cboLoaiBaiHoc.SelectedIndex = 3;
                }
                else // .txt
                {
                    icoFileTypeDisplay.Kind = LucideIconKind.FileText;
                    icoFileTypeDisplay.Foreground = (Brush)new BrushConverter().ConvertFrom("#0284C7")!;
                    pnlFileIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#E0F2FE")!;
                    if (isListeningDoc) cboLoaiBaiHoc.SelectedIndex = 3;
                }

                // Nếu là bài nghe mà chưa có audio, tự động tìm tệp audio cùng tên trong cùng thư mục
                if (cboLoaiBaiHoc.SelectedIndex == 3 && string.IsNullOrEmpty(_selectedAudioFilePath))
                {
                    string baseName = Path.GetFileNameWithoutExtension(filePath);
                    string? dir = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    {
                        string[] audioExts = { ".mp3", ".wav", ".m4a", ".aac", ".ogg" };
                        foreach (var aExt in audioExts)
                        {
                            string candidate = Path.Combine(dir, baseName + aExt);
                            if (File.Exists(candidate))
                            {
                                UpdateAudioDisplay(candidate);
                                break;
                            }
                        }
                    }
                }

                // Bóc tách nội dung thực tế từ tệp
                var (content, title, summary) = ext switch
                {
                    ".docx" => ExtractFromDocx(filePath),
                    ".pdf" => ExtractFromPdf(filePath),
                    _ => ExtractFromTxt(filePath)
                };

                // 1. Tiêu đề: Ưu tiên dòng tiêu đề thực tế từ tệp nếu có, nếu không lấy tên tệp
                if (!string.IsNullOrWhiteSpace(title) && title.Length >= 5 && title.Length <= 120)
                {
                    txtTieuDe.Text = SanitizeText(title);
                }
                else
                {
                    txtTieuDe.Text = SanitizeText(Path.GetFileNameWithoutExtension(fileName)
                        .Replace('_', ' ')
                        .Replace('-', ' '));
                }

                // 2. Nội dung: Đưa toàn bộ nội dung văn bản thực tế bóc tách được vào TextBox
                txtNoiDung.Text = !string.IsNullOrWhiteSpace(content)
                    ? SanitizeText(content)
                    : "Không thể trích xuất văn bản từ tệp này (có thể tệp rỗng hoặc là hình ảnh scan).";

                // 3. Tóm tắt: Tạo tóm tắt từ chính nội dung thực tế trong tệp thay vì câu mẫu
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    txtTomTat.Text = SanitizeText(summary);
                }
                else if (!string.IsNullOrWhiteSpace(content))
                {
                    string clean = SanitizeText(content);
                    txtTomTat.Text = clean.Length > 180 ? clean.Substring(0, 177) + "..." : clean;
                }
                else
                {
                    txtTomTat.Text = $"Tài liệu {fileName}.";
                }
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.CanhBao(this, $"Không thể đọc tệp tin:\n{ex.Message}", "Lỗi đọc tệp");
            }
        }

        private (string Content, string Title, string Summary) ExtractFromDocx(string filePath)
        {
            try
            {
                using var zip = ZipFile.OpenRead(filePath);
                var docEntry = zip.GetEntry("word/document.xml");
                if (docEntry != null)
                {
                    using var stream = docEntry.Open();
                    var xdoc = XDocument.Load(stream);
                    XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
                    var paragraphs = xdoc.Descendants(w + "p")
                        .Select(p => string.Concat(p.Descendants(w + "t").Select(t => t.Value)).Trim())
                        .Where(text => !string.IsNullOrWhiteSpace(text))
                        .ToList();

                    if (paragraphs.Count > 0)
                    {
                        string content = string.Join("\n\n", paragraphs);
                        string title = paragraphs[0];
                        if (title.Length > 100) title = title.Substring(0, 97) + "...";

                        // Tóm tắt từ các đoạn nội dung thực tế trong bài
                        string summary = "";
                        var bodyParas = paragraphs.Skip(1).Where(p => p.Length > 15).ToList();
                        if (bodyParas.Count > 0)
                        {
                            summary = string.Join(" ", bodyParas.Take(2));
                            if (summary.Length > 220) summary = summary.Substring(0, 217) + "...";
                        }
                        else
                        {
                            summary = paragraphs[0];
                        }

                        return (content, title, summary);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ExtractFromDocx] {ex.Message}");
            }

            return ("", "", "");
        }

        private (string Content, string Title, string Summary) ExtractFromPdf(string filePath)
        {
            try
            {
                var rawBytes = File.ReadAllBytes(filePath);
                var rawContent = Encoding.UTF8.GetString(rawBytes);

                var lines = new List<string>();
                var matches = Regex.Matches(rawContent, @"\(((?:\\.|[^\\)])*)\)\s*Tj", RegexOptions.Singleline);
                foreach (Match m in matches)
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
                    var tjMatches = Regex.Matches(rawContent, @"\[((?:\\.|[^\]])*)\]\s*TJ", RegexOptions.Singleline);
                    foreach (Match m in tjMatches)
                    {
                        var innerMatches = Regex.Matches(m.Groups[1].Value, @"\(((?:\\.|[^\\)])*)\)");
                        var lineParts = new List<string>();
                        foreach (Match im in innerMatches)
                        {
                            lineParts.Add(im.Groups[1].Value.Replace(@"\(", "(").Replace(@"\)", ")").Replace(@"\\", @"\"));
                        }
                        string line = string.Concat(lineParts).Trim();
                        if (!string.IsNullOrWhiteSpace(line) && line.Length > 1) lines.Add(line);
                    }
                }

                if (lines.Count > 0)
                {
                    string content = string.Join("\n\n", lines);
                    string title = lines[0];
                    if (title.Length > 100) title = title.Substring(0, 97) + "...";

                    string summary = "";
                    var bodyLines = lines.Skip(1).Where(l => l.Length > 15).ToList();
                    if (bodyLines.Count > 0)
                    {
                        summary = string.Join(" ", bodyLines.Take(2));
                        if (summary.Length > 220) summary = summary.Substring(0, 217) + "...";
                    }
                    else
                    {
                        summary = lines[0];
                    }

                    return (content, title, summary);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ExtractFromPdf] {ex.Message}");
            }

            return ("", "", "");
        }

        private (string Content, string Title, string Summary) ExtractFromTxt(string filePath)
        {
            try
            {
                string raw;
                try
                {
                    raw = File.ReadAllText(filePath, Encoding.UTF8).Trim();
                }
                catch
                {
                    raw = File.ReadAllText(filePath, Encoding.Default).Trim();
                }
                raw = SanitizeText(raw);
                var lines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(l => l.Trim())
                               .Where(l => !string.IsNullOrWhiteSpace(l))
                               .ToList();

                if (lines.Count > 0)
                {
                    string content = string.Join("\n\n", lines);
                    string title = lines[0];
                    if (title.Length > 100) title = title.Substring(0, 97) + "...";

                    string summary = "";
                    var body = lines.Skip(1).Where(l => l.Length > 15).ToList();
                    if (body.Count > 0)
                    {
                        summary = string.Join(" ", body.Take(2));
                        if (summary.Length > 220) summary = summary.Substring(0, 217) + "...";
                    }
                    else
                    {
                        summary = lines[0];
                    }

                    return (content, title, summary);
                }
            }
            catch { }

            return ("", "", "");
        }

        private static string SanitizeText(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return new string(input.Where(c => c == '\r' || c == '\n' || c == '\t' || !char.IsControl(c)).ToArray());
        }

        private void PnlFile_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void PnlFile_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[]? files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0 && File.Exists(files[0]))
                {
                    ProcessSelectedFile(files[0]);
                }
            }
        }

        private void PnlAudio_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[]? files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0 && File.Exists(files[0]))
                {
                    UpdateAudioDisplay(files[0]);
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string tieuDe = txtTieuDe.Text.Trim();
            if (string.IsNullOrWhiteSpace(tieuDe))
            {
                Common.ThongBaoDialog.CanhBao(this, "Vui lòng nhập tiêu đề cho bài học / tài liệu.", "Thiếu thông tin");
                txtTieuDe.Focus();
                return;
            }

            if (cboChuDe.SelectedValue is not int maChuDe || maChuDe <= 0)
            {
                Common.ThongBaoDialog.CanhBao(this, "Vui lòng chọn danh mục chủ đề để phân loại học liệu.", "Chưa chọn chủ đề");
                return;
            }

            // Kiểm tra tệp tài liệu đính kèm (nếu có) phải hợp lệ .docx, .pdf, .txt
            if (!string.IsNullOrEmpty(_selectedFilePath))
            {
                string ext = Path.GetExtension(_selectedFilePath).ToLowerInvariant();
                var validDocExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".docx", ".pdf", ".txt" };
                if (!validDocExtensions.Contains(ext))
                {
                    Common.ThongBaoDialog.CanhBao(this, "Tệp tài liệu văn bản không hợp lệ (.docx, .pdf, .txt). Vui lòng chọn lại tệp tài liệu.", "Lỗi định dạng tệp");
                    return;
                }
            }

            string loai = (cboLoaiBaiHoc.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Doc";
            bool isNghe = string.Equals(loai, "Nghe", StringComparison.OrdinalIgnoreCase) || BaiHocDTO.KiemTraKyNangNghe(loai);

            // Ràng buộc bài học nghe bắt buộc phải có Audio
            if (isNghe && (string.IsNullOrEmpty(_selectedAudioFilePath) || !File.Exists(_selectedAudioFilePath)))
            {
                var confirm = Common.ThongBaoDialog.XacNhan(this, 
                    "Bạn đang tạo bài học thuộc kỹ năng Luyện nghe (Listening) nhưng chưa đính kèm tệp âm thanh (Audio).\n\n" +
                    "Để học viên có thể luyện nghe và hệ thống phát audio cho câu hỏi, bạn có muốn chọn tệp Audio (.mp3, .wav, .m4a) ngay bây giờ không?", 
                    "Yêu cầu tệp âm thanh");

                if (confirm)
                {
                    BtnBrowseAudio_Click(sender, e);
                    if (string.IsNullOrEmpty(_selectedAudioFilePath) || !File.Exists(_selectedAudioFilePath))
                    {
                        return;
                    }
                }
                else
                {
                    Common.ThongBaoDialog.CanhBao(this, "Bài học Luyện nghe bắt buộc phải có tệp Audio để học viên luyện nghe và phát câu hỏi. Vui lòng chọn tệp Audio trước khi lưu.", "Thiếu tệp âm thanh");
                    return;
                }
            }

            string capDo = cboTrinhDo.SelectedValue?.ToString()
                ?? (cboTrinhDo.SelectedItem as TrinhDoDTO)?.MaCapDo
                ?? (cboTrinhDo.SelectedItem as ComboBoxItem)?.Content?.ToString()?.Substring(0, 2)
                ?? "B1";
            int maTrinhDoId = (cboTrinhDo.SelectedItem as TrinhDoDTO)?.MaTrinhDo ?? 0;
            int thoiGian = int.TryParse(txtThoiGian.Text.Trim(), out int tg) ? tg : 25;

            // Sao chép tệp audio vào thư mục KhoHocLieu theo chủ đề để đồng bộ hệ thống
            string? audioPathToSave = _selectedAudioFilePath;
            if (!string.IsNullOrEmpty(_selectedAudioFilePath) && File.Exists(_selectedAudioFilePath))
            {
                var danhMucBUS = new BUS.DanhMucBUS();
                var allChuDe = danhMucBUS.LayDanhSachChuDe();
                string? destAudio = BUS.KhoHocLieuSyncHelper.SaoChepTepKhiUpload(_selectedAudioFilePath, maChuDe, tieuDe + "_Audio", allChuDe);
                audioPathToSave = destAudio ?? _selectedAudioFilePath;
            }

            BaiHocData.TieuDe = SanitizeText(tieuDe);
            BaiHocData.MaChuDe = maChuDe;
            BaiHocData.MaTrinhDo = capDo;
            BaiHocData.MaTrinhDoId = maTrinhDoId > 0 ? maTrinhDoId : 3;
            BaiHocData.LoaiBaiHoc = loai;
            BaiHocData.ThoiGianUocTinhPhut = thoiGian;
            BaiHocData.TomTat = SanitizeText(txtTomTat.Text.Trim());
            BaiHocData.NoiDung = SanitizeText(txtNoiDung.Text.Trim());
            BaiHocData.DuongDanAudio = audioPathToSave;
            BaiHocData.AudioPath = audioPathToSave;
            BaiHocData.DaXuatBan = true;
            BaiHocData.NguoiTao = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;

            // Thực thi lưu vào CSDL qua BUS
            var (success, msg, newId) = BUS.BaiHocBUS.ThemBaiHoc(BaiHocData);
            if (success)
            {
                BaiHocData.MaBaiHoc = newId;

                // Tự động sao chép tệp tài liệu văn bản nguồn vào thư mục KhoHocLieu theo chủ đề đã chọn
                if (!string.IsNullOrEmpty(_selectedFilePath) && File.Exists(_selectedFilePath) && !_selectedFilePath.Equals(_selectedAudioFilePath, StringComparison.OrdinalIgnoreCase))
                {
                    var danhMucBUS = new BUS.DanhMucBUS();
                    var allChuDe = danhMucBUS.LayDanhSachChuDe();
                    BUS.KhoHocLieuSyncHelper.SaoChepTepKhiUpload(_selectedFilePath, BaiHocData.MaChuDe, BaiHocData.TieuDe, allChuDe);
                }

                DialogResult = true;
                Close();
            }
            else
            {
                Common.ThongBaoDialog.CanhBao(this, msg, "Lỗi lưu dữ liệu");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
