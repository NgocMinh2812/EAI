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
        }

        private void BtnBrowseFile_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new OpenFileDialog
            {
                Title = "Chọn tệp đề thi hoặc tài liệu học liệu",
                Filter = "Tệp học liệu (*.pdf;*.docx;*.txt)|*.pdf;*.docx;*.txt|Word Document (*.docx)|*.docx|Adobe PDF (*.pdf)|*.pdf|Văn bản thuần (*.txt)|*.txt|Tất cả tệp (*.*)|*.*",
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
                _selectedFilePath = openDlg.FileName;
                ProcessSelectedFile(_selectedFilePath);
            }
        }

        private void ProcessSelectedFile(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                string ext = fileInfo.Extension.ToLower();
                string fileName = fileInfo.Name;
                double sizeKb = Math.Round(fileInfo.Length / 1024.0, 1);
                string sizeText = sizeKb > 1024 ? $"{Math.Round(sizeKb / 1024.0, 2)} MB" : $"{sizeKb} KB";

                txtFileNameDisplay.Text = fileName;
                txtFileSizeDisplay.Text = $"Định dạng: {ext.ToUpper()} • Dung lượng: {sizeText} • Sẵn sàng trích xuất";

                // Đổi icon tương ứng
                if (ext == ".pdf")
                {
                    icoFileTypeDisplay.Kind = LucideIconKind.FileText;
                    icoFileTypeDisplay.Foreground = (Brush)new BrushConverter().ConvertFrom("#DC2626")!;
                    pnlFileIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#FEE2E2")!;
                    cboLoaiBaiHoc.SelectedIndex = 0; // Đọc hiểu
                }
                else if (ext == ".docx")
                {
                    icoFileTypeDisplay.Kind = LucideIconKind.FileText;
                    icoFileTypeDisplay.Foreground = (Brush)new BrushConverter().ConvertFrom("#2563EB")!;
                    pnlFileIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#DBEAFE")!;
                }
                else
                {
                    icoFileTypeDisplay.Kind = LucideIconKind.FileText;
                    icoFileTypeDisplay.Foreground = (Brush)new BrushConverter().ConvertFrom("#0284C7")!;
                    pnlFileIconBadge.Background = (Brush)new BrushConverter().ConvertFrom("#E0F2FE")!;
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
                    txtTieuDe.Text = title;
                }
                else
                {
                    txtTieuDe.Text = Path.GetFileNameWithoutExtension(fileName)
                        .Replace('_', ' ')
                        .Replace('-', ' ');
                }

                // 2. Nội dung: Đưa toàn bộ nội dung văn bản thực tế bóc tách được vào TextBox
                txtNoiDung.Text = !string.IsNullOrWhiteSpace(content)
                    ? content
                    : "Không thể trích xuất văn bản từ tệp này (có thể tệp rỗng hoặc là hình ảnh scan).";

                // 3. Tóm tắt: Tạo tóm tắt từ chính nội dung thực tế trong tệp thay vì câu mẫu
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    txtTomTat.Text = summary;
                }
                else if (!string.IsNullOrWhiteSpace(content))
                {
                    txtTomTat.Text = content.Length > 180 ? content.Substring(0, 177) + "..." : content;
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
                var matches = Regex.Matches(rawContent, @"\((.*?)\)\s*Tj", RegexOptions.Singleline);
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
                    var tjMatches = Regex.Matches(rawContent, @"\[(.*?)\]\s*TJ", RegexOptions.Singleline);
                    foreach (Match m in tjMatches)
                    {
                        var innerMatches = Regex.Matches(m.Groups[1].Value, @"\((.*?)\)");
                        var lineParts = new List<string>();
                        foreach (Match im in innerMatches)
                        {
                            lineParts.Add(im.Groups[1].Value.Replace(@"\(", "(").Replace(@"\)", ")"));
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
                string raw = File.ReadAllText(filePath, Encoding.UTF8).Trim();
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

            string loai = (cboLoaiBaiHoc.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Doc";
            string capDo = (cboTrinhDo.SelectedItem as ComboBoxItem)?.Content?.ToString()?.Substring(0, 2) ?? "B1";
            int thoiGian = int.TryParse(txtThoiGian.Text.Trim(), out int tg) ? tg : 25;

            BaiHocData.TieuDe = tieuDe;
            BaiHocData.MaChuDe = maChuDe;
            BaiHocData.MaTrinhDo = capDo;
            BaiHocData.LoaiBaiHoc = loai;
            BaiHocData.ThoiGianUocTinhPhut = thoiGian;
            BaiHocData.TomTat = txtTomTat.Text.Trim();
            BaiHocData.NoiDung = txtNoiDung.Text.Trim();
            BaiHocData.DaXuatBan = true;
            BaiHocData.NguoiTao = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;

            // Thực thi lưu vào CSDL qua BUS
            var (success, msg, newId) = BUS.BaiHocBUS.ThemBaiHoc(BaiHocData);
            if (success)
            {
                BaiHocData.MaBaiHoc = newId;

                // Tự động sao chép tệp nguồn vào thư mục KhoHocLieu theo chủ đề đã chọn
                if (!string.IsNullOrEmpty(_selectedFilePath) && File.Exists(_selectedFilePath))
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
