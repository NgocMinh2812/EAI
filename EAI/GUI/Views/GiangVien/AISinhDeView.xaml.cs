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
            CapNhatThongTinMoHinhAI();
            NapDanhSachBaiHocCoSan();
            TaiDanhSachChoDuyet();
        }

        private void CapNhatThongTinMoHinhAI()
        {
            try
            {
                BUS.AI.AIServiceManager.Instance.SyncActiveConfigFromStorage();
                string provider = BUS.AI.AIServiceManager.Instance.ActiveProviderType.ToString();
                string model = BUS.AI.AIServiceManager.Instance.ActiveModel ?? "Mặc định";
                txtActiveAiModelInfo.Text = $"{provider} ({model})";
            }
            catch
            {
                txtActiveAiModelInfo.Text = "Google Gemini (gemini-1.5-flash)";
            }
        }

        private void NapDanhSachBaiHocCoSan()
        {
            try
            {
                var lessons = BaiHocBUS.LayDanhSachBaiHoc();
                cboExistingLesson.Items.Clear();
                cboExistingLesson.Items.Add(new ComboBoxItem
                {
                    Content = "-- Nhập chủ đề / yêu cầu tự do --",
                    Tag = null,
                    IsSelected = true
                });

                if (lessons != null)
                {
                    foreach (var bh in lessons)
                    {
                        string display = $"[{bh.MaTrinhDo ?? "B1"}] {bh.TieuDe} ({bh.TenLoaiHienThi ?? bh.LoaiBaiHoc})";
                        cboExistingLesson.Items.Add(new ComboBoxItem
                        {
                            Content = display,
                            Tag = bh
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NapDanhSachBaiHocCoSan] {ex.Message}");
            }
        }

        private void CboExistingLesson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboExistingLesson.SelectedItem is ComboBoxItem item && item.Tag is BaiHocDTO bh)
            {
                string content = !string.IsNullOrWhiteSpace(bh.TomTat) 
                    ? bh.TomTat 
                    : (!string.IsNullOrWhiteSpace(bh.NoiDung) ? bh.NoiDung : bh.TieuDe);
                if (content.Length > 300) content = content.Substring(0, 300) + "...";

                txtPrompt.Text = $"Bài học: {bh.TieuDe}\nNội dung trọng tâm: {content}";

                // Tự động map Trình độ
                if (!string.IsNullOrEmpty(bh.MaTrinhDo))
                {
                    for (int i = 0; i < cboLevel.Items.Count; i++)
                    {
                        if ((cboLevel.Items[i] as ComboBoxItem)?.Content?.ToString()?.StartsWith(bh.MaTrinhDo, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            cboLevel.SelectedIndex = i;
                            break;
                        }
                    }
                }

                // Tự động map Kỹ năng
                string skillTag = bh.LoaiBaiHoc switch
                {
                    "Nghe" => "Nghe",
                    "Doc" => "Doc",
                    "Viet" => "Viet",
                    "Noi" => "Noi",
                    "NguPhap" => "NguPhap",
                    "TuVung" => "TuVung",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(skillTag))
                {
                    for (int i = 0; i < cboSkill.Items.Count; i++)
                    {
                        if (string.Equals((cboSkill.Items[i] as ComboBoxItem)?.Tag?.ToString(), skillTag, StringComparison.OrdinalIgnoreCase))
                        {
                            cboSkill.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnClearPrompt_Click(object sender, RoutedEventArgs e)
        {
            txtPrompt.Text = "";
            txtPrompt.Focus();
        }

        private void QuickPrompt_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tagStr)
            {
                var parts = tagStr.Split('|');
                if (parts.Length >= 1) txtPrompt.Text = parts[0];

                if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1]))
                {
                    for (int i = 0; i < cboSkill.Items.Count; i++)
                    {
                        if (string.Equals((cboSkill.Items[i] as ComboBoxItem)?.Tag?.ToString(), parts[1], StringComparison.OrdinalIgnoreCase))
                        {
                            cboSkill.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (parts.Length >= 3 && !string.IsNullOrEmpty(parts[2]))
                {
                    for (int i = 0; i < cboLevel.Items.Count; i++)
                    {
                        if ((cboLevel.Items[i] as ComboBoxItem)?.Content?.ToString()?.StartsWith(parts[2], StringComparison.OrdinalIgnoreCase) == true)
                        {
                            cboLevel.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (parts.Length >= 4 && !string.IsNullOrEmpty(parts[3]))
                {
                    for (int i = 0; i < cboQuestionType.Items.Count; i++)
                    {
                        if (string.Equals((cboQuestionType.Items[i] as ComboBoxItem)?.Tag?.ToString(), parts[3], StringComparison.OrdinalIgnoreCase))
                        {
                            cboQuestionType.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnApproveAll_Click(object sender, RoutedEventArgs e)
        {
            var list = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            if (list == null || list.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Không có câu hỏi nào đang chờ duyệt trong hàng đợi.", "Thông báo");
                return;
            }

            string chuDeGoiY = list.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.ChuDeYeuCau))?.ChuDeYeuCau ?? txtPrompt.Text.Trim();
            var dlg = new XacNhanPheDuyetVaTaoBoDialog(list.Count, chuDeGoiY);
            if (Common.ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) != true) return;

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            var (success, fail, dsMaMoi, err) = AISinhDeBUS.Instance.PheDuyetTatCa(list);
            TaiDanhSachChoDuyet();

            if (success > 0 && dsMaMoi.Count > 0)
            {
                if (dlg.HanhDong == HanhDongPheDuyet.TaoBoMoi)
                {
                    bool okBo = BoCauHoiBUS.Instance.TaoBoCauHoiTuDanhSachMaCauHoi(
                        tenBo: dlg.TenBoMoi,
                        moTa: dlg.MoTaBoMoi,
                        tenNguon: chuDeGoiY,
                        maCauHois: dsMaMoi,
                        nguoiTaoId: userId,
                        out int maBoMoi,
                        out string errBo
                    );

                    if (okBo)
                    {
                        Common.ThongBaoDialog.ThanhCong(
                            $"✓ Đã phê duyệt {success} câu hỏi vào Ngân hàng câu hỏi!\n" +
                            $"✓ Đã tạo thành công Bộ câu hỏi mới: '{dlg.TenBoMoi}' (Mã #{maBoMoi}) gồm {dsMaMoi.Count} câu hỏi.",
                            "Phê duyệt & Tạo bộ thành công");
                    }
                    else
                    {
                        Common.ThongBaoDialog.CanhBao(
                            $"Đã phê duyệt {success} câu hỏi vào Ngân hàng câu hỏi thành công, nhưng không thể tạo bộ câu hỏi: {errBo}",
                            "Thông báo");
                    }
                }
                else if (dlg.HanhDong == HanhDongPheDuyet.ThemVaoBoCoSan)
                {
                    bool okBo = BoCauHoiBUS.Instance.ThemDanhSachCauHoiVaoBo(
                        maBo: dlg.MaBoCoSan,
                        maCauHois: dsMaMoi,
                        out int soLuongThem,
                        out string errBo
                    );

                    if (okBo)
                    {
                        Common.ThongBaoDialog.ThanhCong(
                            $"✓ Đã phê duyệt {success} câu hỏi vào Ngân hàng câu hỏi!\n" +
                            $"✓ Đã thêm thành công {soLuongThem} câu hỏi vào Bộ câu hỏi '{dlg.TenBoCoSan}'.",
                            "Phê duyệt & Thêm vào bộ thành công");
                    }
                    else
                    {
                        Common.ThongBaoDialog.CanhBao(
                            $"Đã phê duyệt {success} câu hỏi vào Ngân hàng câu hỏi thành công, nhưng không thể thêm vào bộ câu hỏi: {errBo}",
                            "Thông báo");
                    }
                }
                else
                {
                    Common.ThongBaoDialog.ThanhCong(
                        $"✓ Đã phê duyệt {success} câu hỏi vào Ngân hàng câu hỏi chung thành công!",
                        "Phê duyệt thành công");
                }
            }
            else if (fail > 0)
            {
                Common.ThongBaoDialog.Loi($"Phê duyệt thất bại: {err}", "Lỗi phê duyệt");
            }
        }

        private void BtnRejectAll_Click(object sender, RoutedEventArgs e)
        {
            var list = AISinhDeBUS.Instance.LayDanhSachChoDuyet();
            if (list == null || list.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Không có câu hỏi nào đang chờ duyệt trong hàng đợi.", "Thông báo");
                return;
            }

            bool xacNhan = Common.ThongBaoDialog.XacNhanXoa(
                Window.GetWindow(this),
                $"Bạn có chắc chắn muốn TỪ CHỐI TOÀN BỘ {list.Count} câu hỏi đang chờ trong hàng đợi không?\nThao tác này sẽ xóa các câu hỏi khỏi danh sách chờ duyệt.",
                "Xác nhận từ chối tất cả");

            if (!xacNhan) return;

            var (success, err) = AISinhDeBUS.Instance.TuChoiTatCa();
            TaiDanhSachChoDuyet();
            Common.ThongBaoDialog.ThongTin($"Đã từ chối {success} câu hỏi khỏi hàng đợi kiểm duyệt.", "Đã từ chối tất cả");
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
                    if (btnApproveAll != null) { btnApproveAll.IsEnabled = false; btnApproveAll.Opacity = 0.5; }
                    if (btnRejectAll != null) { btnRejectAll.IsEnabled = false; btnRejectAll.Opacity = 0.5; }
                    return;
                }

                pnlEmptyQueue.Visibility = Visibility.Collapsed;
                txtPendingCount.Text = $"{list.Count} chờ duyệt";
                txtPendingCount.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));
                if (btnApproveAll != null) { btnApproveAll.IsEnabled = true; btnApproveAll.Opacity = 1.0; }
                if (btnRejectAll != null) { btnRejectAll.IsEnabled = true; btnRejectAll.Opacity = 1.0; }

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
            string moHinhDisplay = !string.IsNullOrWhiteSpace(q.MoHinhAI) ? q.MoHinhAI : "AI Generator";
            string srcText = $"{q.ThoiGianTao:dd/MM HH:mm} • {moHinhDisplay}";
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

            // Đoạn văn tham chiếu (chỉ hiển thị khi có nội dung thực sự và không phải tiêu đề lặp lại)
            if (!string.IsNullOrWhiteSpace(q.DoanVanDoc) && 
                q.DoanVanDoc.Trim().Length > 45 && 
                !string.Equals(q.DoanVanDoc.Trim(), q.ChuDeYeuCau?.Trim(), StringComparison.OrdinalIgnoreCase))
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

                    // Làm sạch nội dung lựa chọn để tránh lặp ký hiệu đúng/sai
                    string cleanOpt = opt.NoiDungLuaChon ?? "";
                    cleanOpt = System.Text.RegularExpressions.Regex.Replace(cleanOpt, @"\s*[\(\[]?\s*(?:Đúng|Đáp án đúng|Correct)\s*[\✓\✔]?\s*[\)\]]?\s*$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                    cleanOpt = System.Text.RegularExpressions.Regex.Replace(cleanOpt, @"[\✓\✔]\s*$", "").Trim();

                    string optDisplayText;
                    if (opt.KyHieuLuaChon == "ANS")
                    {
                        optDisplayText = $"Đáp án mẫu: {cleanOpt.Replace("|", "  /  ")}";
                    }
                    else
                    {
                        optDisplayText = cleanOpt + (opt.LaDapAnDung ? "  ✓ (Đáp án đúng)" : "");
                    }

                    var txtOpt = new TextBlock
                    {
                        Text = optDisplayText,
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

            // Nút Sửa (Bo góc 8px)
            var btnSua = new Button
            {
                Style = (Style)FindResource("CardBtnSuaStyle")
            };
            var stackSua = new StackPanel { Orientation = Orientation.Horizontal };
            stackSua.Children.Add(new LucideIcon { Kind = LucideIconKind.Pencil, Size = 12, Foreground = (Brush)FindResource("TextDark"), Margin = new Thickness(0, 0, 5, 0), VerticalAlignment = VerticalAlignment.Center });
            stackSua.Children.Add(new TextBlock { Text = "Sửa", VerticalAlignment = VerticalAlignment.Center });
            btnSua.Content = stackSua;
            btnSua.Click += (s, e) =>
            {
                var dlg = new SuaCauHoiAIDialog(q);
                if (Common.ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
                {
                    TaiDanhSachChoDuyet();
                }
            };
            actionStack.Children.Add(btnSua);

            // Nút Từ chối (Bo góc 8px)
            var btnTuChoi = new Button
            {
                Style = (Style)FindResource("CardBtnTuChoiStyle")
            };
            var stackTuChoi = new StackPanel { Orientation = Orientation.Horizontal };
            stackTuChoi.Children.Add(new LucideIcon { Kind = LucideIconKind.X, Size = 12, Foreground = new SolidColorBrush(Color.FromRgb(220, 38, 38)), Margin = new Thickness(0, 0, 5, 0), VerticalAlignment = VerticalAlignment.Center });
            stackTuChoi.Children.Add(new TextBlock { Text = "Từ chối", VerticalAlignment = VerticalAlignment.Center });
            btnTuChoi.Content = stackTuChoi;
            btnTuChoi.Click += (s, e) =>
            {
                bool xacNhan = Common.ThongBaoDialog.XacNhanXoa(
                    Window.GetWindow(this),
                    "Bạn có chắc chắn muốn từ chối câu hỏi này khỏi hàng đợi kiểm duyệt không?",
                    "Xác nhận từ chối");
                if (!xacNhan) return;

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

            // Nút Phê duyệt (Bo góc 8px)
            var btnDuyet = new Button
            {
                Style = (Style)FindResource("CardBtnDuyetStyle")
            };
            var stackDuyet = new StackPanel { Orientation = Orientation.Horizontal };
            stackDuyet.Children.Add(new LucideIcon { Kind = LucideIconKind.Check, Size = 12, Foreground = Brushes.White, Margin = new Thickness(0, 0, 5, 0), VerticalAlignment = VerticalAlignment.Center });
            stackDuyet.Children.Add(new TextBlock { Text = "Phê duyệt", VerticalAlignment = VerticalAlignment.Center });
            btnDuyet.Content = stackDuyet;
            btnDuyet.Click += (s, e) =>
            {
                string chuDeGoiY = !string.IsNullOrWhiteSpace(q.ChuDeYeuCau) ? q.ChuDeYeuCau : (txtPrompt?.Text.Trim() ?? "Chủ đề tự do");
                var dlg = new XacNhanPheDuyetVaTaoBoDialog(1, chuDeGoiY);
                if (Common.ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) != true) return;

                bool thanhCong = AISinhDeBUS.Instance.PheDuyetCauHoiAI(q.MaCauHoiAI, out int maCauHoiMoi, out string err);
                if (thanhCong)
                {
                    int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
                    TaiDanhSachChoDuyet();

                    if (dlg.HanhDong == HanhDongPheDuyet.TaoBoMoi && maCauHoiMoi > 0)
                    {
                        bool okBo = BoCauHoiBUS.Instance.TaoBoCauHoiTuDanhSachMaCauHoi(
                            tenBo: dlg.TenBoMoi,
                            moTa: dlg.MoTaBoMoi,
                            tenNguon: chuDeGoiY,
                            maCauHois: new List<int> { maCauHoiMoi },
                            nguoiTaoId: userId,
                            out int maBoMoi,
                            out string errBo
                        );

                        if (okBo)
                        {
                            Common.ThongBaoDialog.ThanhCong(
                                $"✓ Đã phê duyệt câu hỏi #{maCauHoiMoi} vào Ngân hàng câu hỏi!\n" +
                                $"✓ Đã tạo thành công Bộ câu hỏi mới: '{dlg.TenBoMoi}' (Mã #{maBoMoi}).",
                                "Phê duyệt & Tạo bộ thành công");
                        }
                        else
                        {
                            Common.ThongBaoDialog.CanhBao(
                                $"Đã phê duyệt câu hỏi #{maCauHoiMoi} vào Ngân hàng câu hỏi thành công, nhưng không thể tạo bộ câu hỏi: {errBo}",
                                "Thông báo");
                        }
                    }
                    else if (dlg.HanhDong == HanhDongPheDuyet.ThemVaoBoCoSan && maCauHoiMoi > 0)
                    {
                        bool okBo = BoCauHoiBUS.Instance.ThemCauHoiVaoBo(dlg.MaBoCoSan, maCauHoiMoi, out string errBo);
                        if (okBo)
                        {
                            Common.ThongBaoDialog.ThanhCong(
                                $"✓ Đã phê duyệt câu hỏi #{maCauHoiMoi} vào Ngân hàng câu hỏi!\n" +
                                $"✓ Đã thêm vào Bộ câu hỏi '{dlg.TenBoCoSan}'.",
                                "Phê duyệt & Thêm vào bộ thành công");
                        }
                        else
                        {
                            Common.ThongBaoDialog.CanhBao(
                                $"Đã phê duyệt câu hỏi #{maCauHoiMoi} vào Ngân hàng câu hỏi thành công, nhưng không thể thêm vào bộ: {errBo}",
                                "Thông báo");
                        }
                    }
                    else
                    {
                        Common.ThongBaoDialog.ThanhCong(
                            $"✓ Đã phê duyệt câu hỏi #{maCauHoiMoi} và lưu vào Ngân hàng câu hỏi thành công!",
                            "Phê duyệt thành công");
                    }
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

        private static string DoanKyNangTuPrompt(string prompt) => AISinhDeBUS.DoanKyNangTuPrompt(prompt);

        private void SetFormEnabled(bool isEnabled)
        {
            if (cboSkill != null) cboSkill.IsEnabled = isEnabled;
            if (cboQuestionType != null) cboQuestionType.IsEnabled = isEnabled;
            if (cboLevel != null) cboLevel.IsEnabled = isEnabled;
            if (cboCount != null) cboCount.IsEnabled = isEnabled;
            if (cboExistingLesson != null) cboExistingLesson.IsEnabled = isEnabled;
            if (txtPrompt != null) txtPrompt.IsEnabled = isEnabled;
            if (tabNhapTep != null) tabNhapTep.IsEnabled = isEnabled;
        }

        private async void BtnSinhCauHoi_Click(object sender, RoutedEventArgs e)
        {
            string prompt = txtPrompt.Text.Trim();
            if (string.IsNullOrEmpty(prompt))
            {
                Common.ThongBaoDialog.CanhBao("Vui lòng nhập chủ đề hoặc yêu cầu tạo câu hỏi bằng AI.", "Thông báo");
                txtPrompt.Focus();
                return;
            }

            if (prompt.Length < 4)
            {
                Common.ThongBaoDialog.CanhBao("Chủ đề hoặc yêu cầu quá ngắn (tối thiểu 4 ký tự). Vui lòng nhập rõ chủ đề kiến thức bạn muốn AI tạo câu hỏi.", "Thông báo");
                txtPrompt.Focus();
                return;
            }

            string level = (cboLevel.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "B1";
            if (level.Contains("—")) level = level.Split('—')[0].Trim();

            string countStr = (cboCount.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "5";
            int.TryParse(countStr, out int count);
            if (count <= 0) count = 5;

            string selectedSkillTag = (cboSkill.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "";
            string loaiBaiHoc = !string.IsNullOrWhiteSpace(selectedSkillTag) ? selectedSkillTag : DoanKyNangTuPrompt(prompt);

            string selectedQTypeTag = (cboQuestionType.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "";
            string? yeuCauLoaiCauHoi = !string.IsNullOrWhiteSpace(selectedQTypeTag) ? selectedQTypeTag : null;

            // Đồng bộ mô hình AI hoạt động từ Giám sát hệ thống
            BUS.AI.AIServiceManager.Instance.SyncActiveConfigFromStorage();
            string activeProvider = BUS.AI.AIServiceManager.Instance.ActiveProviderType.ToString();
            string activeModel = BUS.AI.AIServiceManager.Instance.ActiveModel ?? "Mặc định";
            string modelDisplay = $"{activeProvider} ({activeModel})";

            _generationCts = new CancellationTokenSource();
            var ct = _generationCts.Token;

            SetFormEnabled(false);
            btnSinhCauHoi.Visibility = Visibility.Collapsed;
            pnlGeneratingProgress.Visibility = Visibility.Visible;
            btnCancelGeneration.IsEnabled = true;
            txtGeneratingStatus.Text = $"Đang phân tích, gọi AI ({modelDisplay}) và lưu vào CSDL...";

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;

            try
            {
                var (questions, error) = await AISinhDeBUS.Instance.SinhCauHoiVaLuuHangDoiAsync(
                    prompt: prompt,
                    level: level,
                    count: count,
                    skill: loaiBaiHoc,
                    questionType: yeuCauLoaiCauHoi,
                    modelSelection: "", // Tự động lấy cấu hình Active từ Giám sát hệ thống
                    userId: userId,
                    ct: ct
                );

                if (questions != null && questions.Count > 0)
                {
                    TaiDanhSachChoDuyet();
                    Common.ThongBaoDialog.ThanhCong(
                        $"AI ({modelDisplay}) đã tạo thành công {questions.Count} câu hỏi theo chủ đề '{prompt}'!\nCác câu hỏi đã được lưu an toàn vào hàng đợi kiểm duyệt.",
                        "Tạo câu hỏi thành công");
                }
                else
                {
                    Common.ThongBaoDialog.CanhBao(error ?? "Không nhận được phản hồi từ AI. Vui lòng kiểm tra lại cấu hình API key tại Giám sát hệ thống.", "Thông báo AI");
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
                SetFormEnabled(true);
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
                Title = "Chọn tệp đề thi / giáo trình / tài liệu học tập (.docx, .txt, .pdf, .json)",
                Filter = "Tất cả định dạng hỗ trợ (*.docx;*.txt;*.pdf;*.json)|*.docx;*.txt;*.pdf;*.json|Word Document (*.docx)|*.docx|Văn bản thuần Text (*.txt)|*.txt|PDF Document (*.pdf)|*.pdf|JSON Data (*.json)|*.json"
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

            var fi = new FileInfo(filePath);
            // Giới hạn dung lượng tối đa 10MB theo yêu cầu của hệ thống
            if (fi.Length > AISinhDeBUS.MAX_FILE_SIZE_BYTES)
            {
                Common.ThongBaoDialog.CanhBao("Dung lượng tệp vượt quá giới hạn cho phép (tối đa 10MB) để đảm bảo mô hình AI có thể đọc và phân tích toàn diện.", "Giới hạn dung lượng");
                return;
            }

            string ext = fi.Extension.ToLowerInvariant();
            if (ext != ".docx" && ext != ".json" && ext != ".txt" && ext != ".pdf")
            {
                Common.ThongBaoDialog.CanhBao("Hệ thống chỉ hỗ trợ các tệp có định dạng .docx, .txt, .pdf hoặc .json (tối đa 10MB).", "Định dạng không hỗ trợ");
                return;
            }

            _currentLoadedFileName = fi.Name;
            pnlParsingProgress.Visibility = Visibility.Visible;
            pnlParseResult.Visibility = Visibility.Collapsed;
            txtParsingStatus.Text = $"Đang đọc nội dung và dùng AI phân tích, bóc tách câu hỏi từ tệp '{_currentLoadedFileName}'...";

            try
            {
                var result = await AISinhDeBUS.Instance.BocTachTepTinAsync(filePath);
                _currentParsedResult = result;

                pnlParsingProgress.Visibility = Visibility.Collapsed;
                pnlParseResult.Visibility = Visibility.Visible;

                txtParseSummary.Text = $"Đã nhận diện {result.DanhSachCauHoi.Count} câu hỏi từ tệp '{_currentLoadedFileName}' (Định dạng {result.DinhDang.ToUpper()})";

                if (result.DanhSachLoi != null && result.DanhSachLoi.Count > 0)
                {
                    pnlParseErrors.Visibility = Visibility.Visible;
                    txtParseErrorDetails.Text = string.Join("\n", result.DanhSachLoi.Take(5)) +
                        (result.DanhSachLoi.Count > 5 ? $"\n... và còn {result.DanhSachLoi.Count - 5} thông báo khác." : "");
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
                        string rawOpt = q.DanhSachLuaChon[q.DapAnDungIndex.Value];
                        string cleanOpt = CauHoiDTO.StripOptionPrefix(rawOpt);
                        cleanOpt = System.Text.RegularExpressions.Regex.Replace(cleanOpt, @"\s*[\(\[]?\s*(?:Đúng|Đáp án đúng|Correct)\s*[\✓\✔]?\s*[\)\]]?\s*$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                        cleanOpt = System.Text.RegularExpressions.Regex.Replace(cleanOpt, @"[\✓\✔]\s*$", "").Trim();
                        dapAnStr = $"{prefix}) {cleanOpt}";
                    }
                    else if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
                    {
                        dapAnStr = q.DapAnVanBan;
                    }

                    string normKyNang = BoCauHoiBUS.ChuankHoaKyNang(q.KyNang);
                    string normLoai = CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(q.LoaiCauHoi);

                    return new
                    {
                        ThuTuHienThi = idx + 1,
                        KyNang = normKyNang,
                        KyNangHienThi = CauHoiKhaoSatDTO.LayTenKyNang(normKyNang),
                        TrinhDo = string.IsNullOrWhiteSpace(q.TrinhDo) ? "B1" : q.TrinhDo.Trim().ToUpperInvariant(),
                        LoaiCauHoi = normLoai,
                        LoaiCauHoiHienThi = CauHoiKhaoSatDTO.LayTenLoaiCauHoi(normLoai),
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

        private void XoaKetQuaBocTach()
        {
            _currentParsedResult = null;
            _currentLoadedFileName = null;
            dgPreviewParsedQuestions.ItemsSource = null;
            pnlParseResult.Visibility = Visibility.Collapsed;
            pnlParseErrors.Visibility = Visibility.Collapsed;
            txtParseErrorDetails.Text = string.Empty;
        }

        private void BtnHuyBocTach_Click(object sender, RoutedEventArgs e)
        {
            XoaKetQuaBocTach();
            Common.ThongBaoDialog.ThongTin("Đã xóa và dọn sạch bảng kết quả bóc tách hiện tại.", "Dọn sạch bảng");
        }

        private void BtnLuuVaoHangDoi_Click(object sender, RoutedEventArgs e)
        {
            if (_currentParsedResult == null || _currentParsedResult.DanhSachCauHoi.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Chưa có câu hỏi nào được nhận diện để lưu.", "Thông báo");
                return;
            }

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            bool thanhCong = AISinhDeBUS.Instance.LuuCauHoiVaoHangDoiTuTep(
                _currentParsedResult.DanhSachCauHoi,
                _currentLoadedFileName,
                userId,
                out int soLuongLuu,
                out string errorMessage
            );

            if (thanhCong)
            {
                // Tự động xóa sạch bảng bóc tách sau khi đã chuyển thành công vào hàng đợi
                XoaKetQuaBocTach();
                TaiDanhSachChoDuyet();
                TabAISinhDe_Click(this, new RoutedEventArgs());
                Common.ThongBaoDialog.ThanhCong(
                    $"Đã đưa thành công {soLuongLuu} câu hỏi vào Hàng đợi kiểm duyệt!\nBảng dữ liệu đã được dọn sạch và hệ thống đã chuyển sang màn hình Kiểm duyệt.",
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

            int userId = SessionHelper.CurrentUser?.MaNguoiDung ?? 0;
            bool thanhCong = AISinhDeBUS.Instance.LuuCauHoiVaoNganHangTuTep(
                _currentParsedResult.DanhSachCauHoi,
                userId,
                out int soLuongLuu,
                out string errorMessage
            );

            if (thanhCong)
            {
                // Tự động xóa sạch bảng bóc tách sau khi đã lưu trực tiếp vào ngân hàng
                XoaKetQuaBocTach();
                Common.ThongBaoDialog.ThanhCong(
                    $"🎉 Đã lưu trực tiếp thành công {soLuongLuu} câu hỏi vào Ngân hàng câu hỏi chính thức của hệ thống (Nguồn: NhapFile)!\nBảng dữ liệu đã được dọn sạch.",
                    "Lưu Ngân hàng câu hỏi thành công");
            }
            else
            {
                Common.ThongBaoDialog.Loi($"Không thể lưu vào ngân hàng câu hỏi: {errorMessage}", "Lỗi lưu dữ liệu");
            }
        }

        private void BtnDownloadTemplateTxt_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Tải file Text mẫu (.txt)",
                Filter = "Văn bản thuần Text (*.txt)|*.txt",
                FileName = "Mau_CauHoi_EnglishAI.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    AISinhDeBUS.Instance.TaoFileMauTxt(saveFileDialog.FileName);
                    Common.ThongBaoDialog.ThanhCong($"Đã lưu file mẫu Text thành công tại:\n{saveFileDialog.FileName}", "Tải mẫu thành công");
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
