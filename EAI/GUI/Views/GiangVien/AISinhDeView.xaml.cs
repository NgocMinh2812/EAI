using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class AISinhDeView : Page
    {
        private CancellationTokenSource? _generationCts;
        private KetQuaBocTachTepTinDTO? _currentParsedResult;
        private string _currentLoadedFileName = "";

        public AISinhDeView()
        {
            InitializeComponent();
            Loaded += AISinhDeView_Loaded;
        }

        private void AISinhDeView_Loaded(object sender, RoutedEventArgs e)
        {
            TaiDanhSachChoDuyet();
        }

        #region Tab Navigation
        private void TabAISinhDe_Click(object sender, RoutedEventArgs e)
        {
            pnlAISinhDe.Visibility = Visibility.Visible;
            pnlNhapTep.Visibility = Visibility.Collapsed;

            tabAISinhDe.Background = (Brush)FindResource("AdminDarkGreen");
            tabAISinhDe.Foreground = Brushes.White;

            tabNhapTep.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            tabNhapTep.Foreground = (Brush)FindResource("TextDark");
        }

        private void TabNhapTep_Click(object sender, RoutedEventArgs e)
        {
            pnlAISinhDe.Visibility = Visibility.Collapsed;
            pnlNhapTep.Visibility = Visibility.Visible;

            tabNhapTep.Background = (Brush)FindResource("AdminDarkGreen");
            tabNhapTep.Foreground = Brushes.White;

            tabAISinhDe.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            tabAISinhDe.Foreground = (Brush)FindResource("TextDark");
        }
        #endregion

        #region TAB 1: AI SINH ĐỀ & KIỂM DUYỆT (US-GV08)

        private void BtnRefreshQueue_Click(object sender, RoutedEventArgs e)
        {
            TaiDanhSachChoDuyet();
        }

        public void TaiDanhSachChoDuyet()
        {
            try
            {
                var list = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
                pnlQuestionsList.Children.Clear();

                if (list == null || list.Count == 0)
                {
                    pnlEmptyQueue.Visibility = Visibility.Visible;
                    txtPendingCount.Text = "0 chờ duyệt";
                    txtPendingCount.Foreground = (Brush)FindResource("AdminMint");
                    return;
                }

                pnlEmptyQueue.Visibility = Visibility.Collapsed;
                txtPendingCount.Text = $"{list.Count} chờ duyệt";
                txtPendingCount.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));

                foreach (var item in list)
                {
                    var card = TaoTheCauHoiChoDuyet(item);
                    pnlQuestionsList.Children.Add(card);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaiDanhSachChoDuyet] {ex.Message}");
            }
        }

        private Border TaoTheCauHoiChoDuyet(CauHoiDoAISinhItemDTO q)
        {
            var cardBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 0, 16)
            };

            var mainStack = new StackPanel();

            // 1. Header Card: Tiêu đề yêu cầu, Kỹ năng, Cấp độ, Thời gian, Trạng thái
            var headerGrid = new Grid { Margin = new Thickness(0, 0, 0, 10) };
            var headerLeft = new StackPanel { Orientation = Orientation.Horizontal };

            string topicText = !string.IsNullOrWhiteSpace(q.ChuDeYeuCau) ? q.ChuDeYeuCau : "Yêu cầu AI sinh câu hỏi";
            if (topicText.Length > 40) topicText = topicText.Substring(0, 37) + "...";

            headerLeft.Children.Add(new TextBlock
            {
                Text = topicText,
                FontWeight = FontWeights.Bold,
                FontSize = 13.5,
                Foreground = (Brush)FindResource("TextDark"),
                Margin = new Thickness(0, 0, 8, 0),
                VerticalAlignment = VerticalAlignment.Center
            });

            // Badge loại câu hỏi & Kỹ năng
            var typeBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(243, 232, 255)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Margin = new Thickness(0, 0, 6, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            typeBadge.Child = new TextBlock
            {
                Text = CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(q.LoaiCauHoi),
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(126, 34, 206))
            };
            headerLeft.Children.Add(typeBadge);

            // Badge Cấp độ
            var levelBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(236, 253, 245)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Margin = new Thickness(0, 0, 8, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            levelBadge.Child = new TextBlock
            {
                Text = q.MaCapDo ?? "B1",
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(5, 150, 105))
            };
            headerLeft.Children.Add(levelBadge);

            // Nguồn và thời gian
            string srcText = $"{q.ThoiGianTao:dd/MM HH:mm} • {q.MoHinhAI ?? "AI Generator"}";
            headerLeft.Children.Add(new TextBlock
            {
                Text = srcText,
                FontSize = 11.5,
                Foreground = (Brush)FindResource("TextGray"),
                VerticalAlignment = VerticalAlignment.Center
            });

            headerGrid.Children.Add(headerLeft);

            // Badge Chờ duyệt
            var badgeBorder = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = new SolidColorBrush(Color.FromRgb(254, 243, 199)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2)
            };
            badgeBorder.Child = new TextBlock
            {
                Text = "Chờ duyệt",
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9))
            };
            headerGrid.Children.Add(badgeBorder);
            mainStack.Children.Add(headerGrid);

            // 2. Nội dung câu hỏi + Các lựa chọn
            var contentBorder = new Border
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12),
                Margin = new Thickness(0, 0, 0, 12)
            };

            var bodyStack = new StackPanel();

            // Đoạn văn tham chiếu (nếu có)
            if (!string.IsNullOrWhiteSpace(q.DoanVanDoc))
            {
                var docBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                    BorderBrush = (Brush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(8),
                    Margin = new Thickness(0, 0, 0, 8)
                };
                docBorder.Child = new TextBlock
                {
                    Text = $"📖 Đoạn văn tham chiếu:\n{q.DoanVanDoc}",
                    FontSize = 12,
                    Foreground = (Brush)FindResource("TextDark"),
                    TextWrapping = TextWrapping.Wrap,
                    FontStyle = FontStyles.Italic
                };
                bodyStack.Children.Add(docBorder);
            }

            // Nội dung câu hỏi
            bodyStack.Children.Add(new TextBlock
            {
                Text = q.NoiDungCauHoi,
                FontWeight = FontWeights.SemiBold,
                FontSize = 13,
                Foreground = (Brush)FindResource("TextDark"),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            });

            // Lựa chọn đáp án
            if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
            {
                foreach (var opt in q.DanhSachLuaChon)
                {
                    var optGrid = new Grid { Margin = new Thickness(0, 2, 0, 2) };
                    optGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(28) });
                    optGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var keyBorder = new Border
                    {
                        Width = 22,
                        Height = 22,
                        CornerRadius = new CornerRadius(11),
                        Background = opt.LaDapAnDung ? new SolidColorBrush(Color.FromRgb(220, 252, 231)) : new SolidColorBrush(Color.FromRgb(241, 245, 249)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    keyBorder.Child = new TextBlock
                    {
                        Text = opt.KyHieuLuaChon,
                        FontSize = 11,
                        FontWeight = FontWeights.Bold,
                        Foreground = opt.LaDapAnDung ? new SolidColorBrush(Color.FromRgb(21, 128, 61)) : (Brush)FindResource("TextDark"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    optGrid.Children.Add(keyBorder);

                    var txtOpt = new TextBlock
                    {
                        Text = opt.NoiDungLuaChon + (opt.LaDapAnDung ? "  ✓ (Đáp án đúng)" : ""),
                        FontSize = 12.5,
                        Foreground = opt.LaDapAnDung ? new SolidColorBrush(Color.FromRgb(21, 128, 61)) : (Brush)FindResource("TextDark"),
                        FontWeight = opt.LaDapAnDung ? FontWeights.Bold : FontWeights.Normal,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap
                    };
                    Grid.SetColumn(txtOpt, 1);
                    optGrid.Children.Add(txtOpt);

                    bodyStack.Children.Add(optGrid);
                }
            }

            // Giải thích chi tiết
            if (!string.IsNullOrWhiteSpace(q.GiaiThichDapAn))
            {
                bodyStack.Children.Add(new TextBlock
                {
                    Text = $"💡 Giải thích: {q.GiaiThichDapAn}",
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(3, 105, 161)),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 8, 0, 0)
                });
            }

            contentBorder.Child = bodyStack;
            mainStack.Children.Add(contentBorder);

            // 3. Nút hành động: Sửa, Từ chối, Phê duyệt
            var actionStack = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };

            // Nút Sửa
            var btnSua = new Button
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                FontWeight = FontWeights.SemiBold,
                FontSize = 12,
                Padding = new Thickness(12, 6, 12, 6),
                Margin = new Thickness(0, 0, 8, 0),
                Cursor = Cursors.Hand
            };
            var stackSua = new StackPanel { Orientation = Orientation.Horizontal };
            stackSua.Children.Add(new LucideIcon { Kind = LucideIconKind.Pencil, Size = 12, Foreground = (Brush)FindResource("TextDark"), Margin = new Thickness(0, 0, 5, 0) });
            stackSua.Children.Add(new TextBlock { Text = "Sửa" });
            btnSua.Content = stackSua;
            btnSua.Click += (s, e) =>
            {
                var dlg = new SuaCauHoiAIDialog(q) { Owner = Window.GetWindow(this) };
                if (dlg.ShowDialog() == true)
                {
                    TaiDanhSachChoDuyet();
                }
            };
            actionStack.Children.Add(btnSua);

            // Nút Từ chối
            var btnTuChoi = new Button
            {
                Background = new SolidColorBrush(Color.FromRgb(254, 242, 242)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(254, 202, 202)),
                BorderThickness = new Thickness(1),
                Foreground = new SolidColorBrush(Color.FromRgb(239, 68, 68)),
                FontWeight = FontWeights.SemiBold,
                FontSize = 12,
                Padding = new Thickness(12, 6, 12, 6),
                Margin = new Thickness(0, 0, 8, 0),
                Cursor = Cursors.Hand
            };
            var stackTuChoi = new StackPanel { Orientation = Orientation.Horizontal };
            stackTuChoi.Children.Add(new LucideIcon { Kind = LucideIconKind.X, Size = 12, Foreground = new SolidColorBrush(Color.FromRgb(239, 68, 68)), Margin = new Thickness(0, 0, 5, 0) });
            stackTuChoi.Children.Add(new TextBlock { Text = "Từ chối" });
            btnTuChoi.Content = stackTuChoi;
            btnTuChoi.Click += (s, e) =>
            {
                bool thanhCong = AISinhDeBUS.Instance.TuChoiCauHoiAI(q.MaCauHoiAI, out string err);
                if (thanhCong)
                {
                    TaiDanhSachChoDuyet();
                    Common.ThongBaoDialog.ThongTin("Đã từ chối câu hỏi khỏi hàng đợi kiểm duyệt.", "Đã từ chối");
                }
                else
                {
                    Common.ThongBaoDialog.Loi($"Lỗi khi từ chối câu hỏi: {err}", "Lỗi");
                }
            };
            actionStack.Children.Add(btnTuChoi);

            // Nút Phê duyệt
            var btnDuyet = new Button
            {
                Background = (Brush)FindResource("AdminDarkGreen"),
                Foreground = Brushes.White,
                FontWeight = FontWeights.SemiBold,
                FontSize = 12,
                Padding = new Thickness(16, 6, 16, 6),
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand
            };
            var stackDuyet = new StackPanel { Orientation = Orientation.Horizontal };
            stackDuyet.Children.Add(new LucideIcon { Kind = LucideIconKind.Check, Size = 12, Foreground = Brushes.White, Margin = new Thickness(0, 0, 5, 0) });
            stackDuyet.Children.Add(new TextBlock { Text = "Phê duyệt" });
            btnDuyet.Content = stackDuyet;
            btnDuyet.Click += (s, e) =>
            {
                bool thanhCong = AISinhDeBUS.Instance.PheDuyetCauHoiAI(q.MaCauHoiAI, out int maCauHoiMoi, out string err);
                if (thanhCong)
                {
                    TaiDanhSachChoDuyet();
                    Common.ThongBaoDialog.ThanhCong($"✓ Đã phê duyệt và lưu vào Ngân hàng câu hỏi thành công!\nMã câu hỏi chính thức: #{maCauHoiMoi}.", "Phê duyệt thành công");
                }
                else
                {
                    Common.ThongBaoDialog.Loi($"Lỗi khi phê duyệt câu hỏi: {err}", "Lỗi phê duyệt");
                }
            };
            actionStack.Children.Add(btnDuyet);

            mainStack.Children.Add(actionStack);
            cardBorder.Child = mainStack;
            return cardBorder;
        }

        private async void BtnSinhCauHoi_Click(object sender, RoutedEventArgs e)
        {
            string prompt = txtPrompt.Text.Trim();
            if (string.IsNullOrEmpty(prompt))
            {
                Common.ThongBaoDialog.CanhBao("Vui lòng nhập chủ đề hoặc yêu cầu tạo câu hỏi bằng AI.", "Thông báo");
                return;
            }

            string level = (cboLevel.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "B1";
            if (level.Contains("—")) level = level.Split('—')[0].Trim();

            string countStr = (cboCount.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "5";
            int.TryParse(countStr, out int count);
            if (count <= 0) count = 5;

            string selectedSkillTag = (cboSkill.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "";
            string loaiBaiHoc = !string.IsNullOrWhiteSpace(selectedSkillTag) ? selectedSkillTag : "Doc";

            string selectedQTypeTag = (cboQuestionType.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "";
            string? yeuCauLoaiCauHoi = !string.IsNullOrWhiteSpace(selectedQTypeTag) ? selectedQTypeTag : null;

            string modelSelection = (cboModel.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Google Gemini";

            _generationCts = new CancellationTokenSource();
            var ct = _generationCts.Token;

            btnSinhCauHoi.Visibility = Visibility.Collapsed;
            pnlGeneratingProgress.Visibility = Visibility.Visible;
            btnCancelGeneration.IsEnabled = true;
            txtGeneratingStatus.Text = $"Đang phân tích, gọi AI ({modelSelection}) và lưu vào CSDL...";

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;

            try
            {
                var (questions, error) = await AISinhDeBUS.Instance.SinhCauHoiVaLuuHangDoiAsync(
                    prompt: prompt,
                    level: level,
                    count: count,
                    skill: loaiBaiHoc,
                    questionType: yeuCauLoaiCauHoi,
                    modelSelection: modelSelection,
                    userId: userId,
                    ct: ct
                );

                if (questions != null && questions.Count > 0)
                {
                    TaiDanhSachChoDuyet();
                    Common.ThongBaoDialog.ThanhCong(
                        $"AI ({modelSelection}) đã tạo thành công {questions.Count} câu hỏi theo chủ đề '{prompt}'!\nCác câu hỏi đã được lưu an toàn vào hàng đợi kiểm duyệt.",
                        "Tạo câu hỏi thành công");
                }
                else
                {
                    Common.ThongBaoDialog.CanhBao(error ?? "Không nhận được phản hồi từ AI. Vui lòng kiểm tra API key hoặc chọn mô hình khác.", "Thông báo AI");
                }
            }
            catch (OperationCanceledException)
            {
                Common.ThongBaoDialog.ThongTin("Đã hủy quá trình tạo câu hỏi theo yêu cầu của bạn.", "Đã hủy tác vụ");
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.Loi($"Có lỗi xảy ra khi tạo câu hỏi: {ex.Message}", "Lỗi tác vụ AI");
            }
            finally
            {
                pnlGeneratingProgress.Visibility = Visibility.Collapsed;
                btnSinhCauHoi.Visibility = Visibility.Visible;
                _generationCts?.Dispose();
                _generationCts = null;
            }
        }

        private void BtnCancelGeneration_Click(object sender, RoutedEventArgs e)
        {
            if (_generationCts != null && !_generationCts.IsCancellationRequested)
            {
                txtGeneratingStatus.Text = "Đang hủy tác vụ AI...";
                btnCancelGeneration.IsEnabled = false;
                _generationCts.Cancel();
            }
        }

        #endregion

        #region TAB 2: NHẬP TỆP HÀNG LOẠT (US-GV08)

        private void BtnChonFile_Click(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Chọn tệp đề thi / câu hỏi (.xlsx, .docx, .json)",
                Filter = "Tất cả định dạng hỗ trợ (*.xlsx;*.docx;*.json)|*.xlsx;*.docx;*.json|Excel Spreadsheet (*.xlsx)|*.xlsx|Word Document (*.docx)|*.docx|JSON Data (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _ = XuLyTepTinAsync(openFileDialog.FileName);
            }
        }

        private void DropZone_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        private void DropZone_DragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void DropZone_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    _ = XuLyTepTinAsync(files[0]);
                }
            }
        }

        private async Task XuLyTepTinAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Common.ThongBaoDialog.CanhBao("Tệp tin không tồn tại trên máy tính.", "Lỗi tệp");
                return;
            }

            string ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".xlsx" && ext != ".docx" && ext != ".json")
            {
                Common.ThongBaoDialog.CanhBao("Hệ thống chỉ hỗ trợ các tệp có định dạng .xlsx, .docx hoặc .json.", "Định dạng không hỗ trợ");
                return;
            }

            _currentLoadedFileName = Path.GetFileName(filePath);
            pnlParsingProgress.Visibility = Visibility.Visible;
            pnlParseResult.Visibility = Visibility.Collapsed;
            txtParsingStatus.Text = $"Đang đọc và phân tích cấu trúc dữ liệu từ tệp '{_currentLoadedFileName}'...";

            try
            {
                var result = await AISinhDeBUS.Instance.BocTachTepTinAsync(filePath);
                _currentParsedResult = result;

                pnlParsingProgress.Visibility = Visibility.Collapsed;
                pnlParseResult.Visibility = Visibility.Visible;

                txtParseSummary.Text = $"Đã nhận diện {result.DanhSachCauHoi.Count} câu hỏi từ tệp '{_currentLoadedFileName}' (Định dạng {result.DinhDangTep.ToUpper()})";

                if (result.DanhSachLoi != null && result.DanhSachLoi.Count > 0)
                {
                    pnlParseErrors.Visibility = Visibility.Visible;
                    txtParseErrorDetails.Text = string.Join("\n", result.DanhSachLoi.Take(5)) +
                        (result.DanhSachLoi.Count > 5 ? $"\n... và còn {result.DanhSachLoi.Count - 5} cảnh báo khác." : "");
                }
                else
                {
                    pnlParseErrors.Visibility = Visibility.Collapsed;
                }

                // Đổ dữ liệu vào bảng xem trước
                var previewItems = result.DanhSachCauHoi.Select((q, idx) =>
                {
                    string dapAnStr = "—";
                    if (q.DapAnDungIndex.HasValue && q.DanhSachLuaChon != null &&
                        q.DapAnDungIndex.Value >= 0 && q.DapAnDungIndex.Value < q.DanhSachLuaChon.Count)
                    {
                        char prefix = (char)('A' + q.DapAnDungIndex.Value);
                        dapAnStr = $"{prefix}) {q.DanhSachLuaChon[q.DapAnDungIndex.Value]}";
                    }
                    else if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
                    {
                        dapAnStr = q.DapAnVanBan;
                    }

                    return new
                    {
                        ThuTuHienThi = idx + 1,
                        KyNang = q.KyNang ?? "Doc",
                        TrinhDo = q.TrinhDo ?? "B1",
                        LoaiCauHoi = CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(q.LoaiCauHoi),
                        NoiDungCauHoi = q.NoiDungCauHoi,
                        DapAnDungHienThi = dapAnStr
                    };
                }).ToList();

                dgPreviewParsedQuestions.ItemsSource = previewItems;
            }
            catch (Exception ex)
            {
                pnlParsingProgress.Visibility = Visibility.Collapsed;
                Common.ThongBaoDialog.Loi($"Lỗi khi bóc tách tệp: {ex.Message}", "Lỗi bóc tách");
            }
        }

        private void BtnLuuVaoHangDoi_Click(object sender, RoutedEventArgs e)
        {
            if (_currentParsedResult == null || _currentParsedResult.DanhSachCauHoi.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Chưa có câu hỏi nào được nhận diện để lưu.", "Thông báo");
                return;
            }

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
            bool thanhCong = AISinhDeBUS.Instance.LuuCauHoiVaoHangDoiTuTep(
                _currentParsedResult.DanhSachCauHoi,
                _currentLoadedFileName,
                userId,
                out int soLuongLuu,
                out string errorMessage
            );

            if (thanhCong)
            {
                TaiDanhSachChoDuyet();
                Common.ThongBaoDialog.ThanhCong(
                    $"Đã đưa thành công {soLuongLuu} câu hỏi vào Hàng đợi kiểm duyệt!\nBạn có thể sang tab 'Tạo câu hỏi bằng AI' để xem và phê duyệt từng câu.",
                    "Lưu hàng đợi thành công");
            }
            else
            {
                Common.ThongBaoDialog.Loi($"Không thể lưu vào hàng đợi: {errorMessage}", "Lỗi lưu dữ liệu");
            }
        }

        private void BtnLuuVaoNganHang_Click(object sender, RoutedEventArgs e)
        {
            if (_currentParsedResult == null || _currentParsedResult.DanhSachCauHoi.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Chưa có câu hỏi nào được nhận diện để lưu.", "Thông báo");
                return;
            }

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 1;
            bool thanhCong = AISinhDeBUS.Instance.LuuCauHoiVaoNganHangTuTep(
                _currentParsedResult.DanhSachCauHoi,
                userId,
                out int soLuongLuu,
                out string errorMessage
            );

            if (thanhCong)
            {
                Common.ThongBaoDialog.ThanhCong(
                    $"🎉 Đã lưu trực tiếp thành công {soLuongLuu} câu hỏi vào Ngân hàng câu hỏi chính thức của hệ thống (Nguồn: NhapFile)!",
                    "Lưu Ngân hàng câu hỏi thành công");
            }
            else
            {
                Common.ThongBaoDialog.Loi($"Không thể lưu vào ngân hàng câu hỏi: {errorMessage}", "Lỗi lưu dữ liệu");
            }
        }

        private void BtnDownloadTemplateExcel_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Tải file Excel mẫu (.xlsx)",
                Filter = "Excel Spreadsheet (*.xlsx)|*.xlsx",
                FileName = "Mau_CauHoi_EnglishAI.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    AISinhDeBUS.Instance.XuatFileMauExcel(saveFileDialog.FileName);
                    Common.ThongBaoDialog.ThanhCong($"Đã lưu file mẫu Excel thành công tại:\n{saveFileDialog.FileName}", "Tải mẫu thành công");
                }
                catch (Exception ex)
                {
                    Common.ThongBaoDialog.Loi($"Không thể lưu file mẫu: {ex.Message}", "Lỗi");
                }
            }
        }

        private void BtnDownloadTemplateDocx_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Tải file Word mẫu (.docx)",
                Filter = "Word Document (*.docx)|*.docx",
                FileName = "Mau_CauHoi_EnglishAI.docx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    AISinhDeBUS.Instance.TaoFileMauDocx(saveFileDialog.FileName);
                    Common.ThongBaoDialog.ThanhCong($"Đã lưu file mẫu Word thành công tại:\n{saveFileDialog.FileName}", "Tải mẫu thành công");
                }
                catch (Exception ex)
                {
                    Common.ThongBaoDialog.Loi($"Không thể lưu file mẫu: {ex.Message}", "Lỗi");
                }
            }
        }

        private void BtnDownloadTemplateJson_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Tải file JSON mẫu (.json)",
                Filter = "JSON Data (*.json)|*.json",
                FileName = "Mau_CauHoi_EnglishAI.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    AISinhDeBUS.Instance.TaoFileMauJson(saveFileDialog.FileName);
                    Common.ThongBaoDialog.ThanhCong($"Đã lưu file mẫu JSON thành công tại:\n{saveFileDialog.FileName}", "Tải mẫu thành công");
                }
                catch (Exception ex)
                {
                    Common.ThongBaoDialog.Loi($"Không thể lưu file mẫu: {ex.Message}", "Lỗi");
                }
            }
        }

        #endregion
    }
}
