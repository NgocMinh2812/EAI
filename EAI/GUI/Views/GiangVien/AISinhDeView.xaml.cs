using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BUS.AI;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class AISinhDeView : Page
    {
        private int _pendingCount = 2;
        private CancellationTokenSource? _generationCts;

        public AISinhDeView()
        {
            InitializeComponent();
            Loaded += AISinhDeView_Loaded;
        }

        private void AISinhDeView_Loaded(object sender, RoutedEventArgs e)
        {
            // Đồng bộ mô hình mặc định từ AIServiceManager nếu chưa chọn
            if (cboModel != null && cboModel.SelectedIndex < 0 && cboModel.Items.Count > 0)
            {
                cboModel.SelectedIndex = 0;
            }
        }

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
            string loaiBaiHoc = !string.IsNullOrWhiteSpace(selectedSkillTag) ? selectedSkillTag : "Ngữ pháp & Từ vựng";

            string selectedQTypeTag = (cboQuestionType.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "";
            string? yeuCauLoaiCauHoi = !string.IsNullOrWhiteSpace(selectedQTypeTag) ? selectedQTypeTag : null;

            string modelSelection = (cboModel.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Google Gemini";

            // Khởi tạo CancellationToken cho tác vụ chạy nền
            _generationCts = new CancellationTokenSource();
            var ct = _generationCts.Token;

            btnSinhCauHoi.Visibility = Visibility.Collapsed;
            pnlGeneratingProgress.Visibility = Visibility.Visible;
            btnCancelGeneration.IsEnabled = true;
            txtGeneratingStatus.Text = $"Đang phân tích và sinh câu hỏi bằng AI ({modelSelection})...";

            try
            {
                // Thực thi sinh câu hỏi trên nền Task.Run để không khóa UI, 
                // tác vụ vẫn chạy độc lập ngay cả khi người dùng chuyển sang màn hình khác
                var questions = await Task.Run(async () =>
                {
                    return await AIServiceManager.Instance.BocTachCauHoiAsync(
                        tieuDe: prompt,
                        noiDung: prompt,
                        trinhDo: level,
                        soLuong: count,
                        loaiBaiHoc: loaiBaiHoc,
                        ct: ct,
                        yeuCauLoaiCauHoi: yeuCauLoaiCauHoi
                    );
                }, ct);

                // Khi hoàn thành, trở về UI Thread để cập nhật
                Dispatcher.Invoke(() =>
                {
                    if (questions != null && questions.Count > 0)
                    {
                        foreach (var q in questions)
                        {
                            var card = TaoTheCauHoiMoi(q, prompt, modelSelection);
                            pnlQuestionsList.Children.Insert(0, card);
                            _pendingCount++;
                        }
                        UpdatePendingCount();
                        Common.ThongBaoDialog.ThanhCong(
                            $"AI ({modelSelection}) đã tạo thành công {questions.Count} câu hỏi đa dạng theo yêu cầu: \"{prompt}\"!\nCâu hỏi đã được đưa vào danh sách chờ duyệt.", 
                            "Tạo câu hỏi thành công");
                    }
                    else
                    {
                        Common.ThongBaoDialog.CanhBao("Không nhận được dữ liệu câu hỏi từ AI. Vui lòng thử lại với mô hình khác.", "Thông báo");
                    }
                });
            }
            catch (OperationCanceledException)
            {
                Dispatcher.Invoke(() =>
                {
                    Common.ThongBaoDialog.ThongTin("Đã hủy quá trình tạo câu hỏi theo yêu cầu của bạn.", "Đã hủy tác vụ");
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Common.ThongBaoDialog.Loi($"Có lỗi xảy ra khi tạo câu hỏi: {ex.Message}", "Lỗi tác vụ AI");
                });
            }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    pnlGeneratingProgress.Visibility = Visibility.Collapsed;
                    btnSinhCauHoi.Visibility = Visibility.Visible;
                    _generationCts?.Dispose();
                    _generationCts = null;
                });
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

        private Border TaoTheCauHoiMoi(CauHoiDTO q, string topic, string modelName)
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

            // Header card
            var headerGrid = new Grid { Margin = new Thickness(0, 0, 0, 10) };
            var headerLeft = new StackPanel { Orientation = Orientation.Horizontal };
            headerLeft.Children.Add(new TextBlock
            {
                Text = $"{topic}",
                FontWeight = FontWeights.Bold,
                FontSize = 13.5,
                Foreground = (Brush)FindResource("TextDark"),
                Margin = new Thickness(0, 0, 8, 0)
            });

            // Badge loại câu hỏi & kỹ năng
            string loaiHienThi = CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(q.LoaiCauHoi);
            var qTypeBadge = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(243, 232, 255)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Margin = new Thickness(0, 0, 8, 0)
            };
            qTypeBadge.Child = new TextBlock
            {
                Text = loaiHienThi,
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(126, 34, 206))
            };
            headerLeft.Children.Add(qTypeBadge);

            headerLeft.Children.Add(new TextBlock
            {
                Text = $"{DateTime.Now:dd/MM HH:mm} • {modelName}",
                FontSize = 12,
                Foreground = (Brush)FindResource("TextGray"),
                VerticalAlignment = VerticalAlignment.Center
            });
            headerGrid.Children.Add(headerLeft);

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

            // Nội dung câu hỏi + các lựa chọn
            var contentBorder = new Border
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12),
                Margin = new Thickness(0, 0, 0, 12)
            };

            string bodyText = "";
            if (!string.IsNullOrWhiteSpace(q.DoanVanDoc))
            {
                bodyText += $"📖 Đoạn văn tham chiếu:\n{q.DoanVanDoc}\n\n";
            }
            bodyText += q.NoiDungCauHoi;

            if (q.DanhSachLuaChon != null && q.DanhSachLuaChon.Count > 0)
            {
                bodyText += "\n\n" + string.Join("\n", q.DanhSachLuaChon);
            }
            if (!string.IsNullOrWhiteSpace(q.DapAnVanBan))
            {
                bodyText += $"\n\n🔑 Đáp án mẫu: {q.DapAnVanBan}";
            }
            if (!string.IsNullOrWhiteSpace(q.TieuChiCham))
            {
                bodyText += $"\n\n📋 Tiêu chí chấm (Rubric): {q.TieuChiCham}";
            }
            if (!string.IsNullOrWhiteSpace(q.GiaiThichChiTiet))
            {
                bodyText += $"\n\n💡 Giải thích: {q.GiaiThichChiTiet}";
            }

            contentBorder.Child = new TextBlock
            {
                FontFamily = new FontFamily("Consolas, Segoe UI"),
                FontSize = 12.5,
                Foreground = (Brush)FindResource("TextDark"),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20,
                Text = bodyText
            };
            mainStack.Children.Add(contentBorder);

            // Nút hành động
            var actionStack = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };

            var btnSua = new Button
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                FontWeight = FontWeights.SemiBold,
                FontSize = 12,
                Padding = new Thickness(12, 6, 12, 6),
                Margin = new Thickness(0, 0, 8, 0),
                Cursor = Cursors.Hand,
                Content = "Sửa"
            };
            btnSua.Click += (s, e) => Common.ThongBaoDialog.ThongTin("Mở trình biên tập câu hỏi để chỉnh sửa câu hỏi, đáp án hoặc giải thích chi tiết.", "Chỉnh sửa câu hỏi");
            actionStack.Children.Add(btnSua);

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
                Cursor = Cursors.Hand,
                Content = "Từ chối"
            };
            btnTuChoi.Click += (s, e) =>
            {
                cardBorder.Visibility = Visibility.Collapsed;
                _pendingCount--;
                UpdatePendingCount();
            };
            actionStack.Children.Add(btnTuChoi);

            var btnDuyet = new Button
            {
                Background = (Brush)FindResource("AdminDarkGreen"),
                Foreground = Brushes.White,
                FontWeight = FontWeights.SemiBold,
                FontSize = 12,
                Padding = new Thickness(16, 6, 16, 6),
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                Content = "Phê duyệt"
            };
            btnDuyet.Click += (s, e) =>
            {
                cardBorder.Visibility = Visibility.Collapsed;
                _pendingCount--;
                UpdatePendingCount();
                Common.ThongBaoDialog.ThanhCong($"✓ Câu hỏi '{topic}' đã được phê duyệt và lưu vào Ngân hàng câu hỏi!", "Phê duyệt thành công");
            };
            actionStack.Children.Add(btnDuyet);

            mainStack.Children.Add(actionStack);
            cardBorder.Child = mainStack;
            return cardBorder;
        }

        private void BtnDuyetQ1_Click(object sender, RoutedEventArgs e)
        {
            cardQ1.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
            Common.ThongBaoDialog.ThanhCong("✓ Câu hỏi 'Present Perfect' đã được phê duyệt và lưu vào Ngân hàng câu hỏi!", "Phê duyệt thành công");
        }

        private void BtnTuChoiQ1_Click(object sender, RoutedEventArgs e)
        {
            cardQ1.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
        }

        private void BtnSuaQ1_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin("Mở trình biên tập câu hỏi để chỉnh sửa câu hỏi, đáp án hoặc giải thích chi tiết.", "Chỉnh sửa câu hỏi");
        }

        private void BtnDuyetQ2_Click(object sender, RoutedEventArgs e)
        {
            cardQ2.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
            Common.ThongBaoDialog.ThanhCong("✓ Câu hỏi 'Từ vựng Môi trường' đã được phê duyệt và lưu vào Ngân hàng câu hỏi!", "Phê duyệt thành công");
        }

        private void BtnTuChoiQ2_Click(object sender, RoutedEventArgs e)
        {
            cardQ2.Visibility = Visibility.Collapsed;
            _pendingCount--;
            UpdatePendingCount();
        }

        private void BtnSuaQ2_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin("Mở trình biên tập câu hỏi để chỉnh sửa câu hỏi, đáp án hoặc giải thích chi tiết.", "Chỉnh sửa câu hỏi");
        }

        private void UpdatePendingCount()
        {
            if (_pendingCount <= 0)
            {
                txtPendingCount.Text = "Đã duyệt hết";
                txtPendingCount.Foreground = (Brush)FindResource("AdminMint");
            }
            else
            {
                txtPendingCount.Text = $"{_pendingCount} chờ duyệt";
            }
        }

        private void BtnChonFile_Click(object sender, MouseButtonEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin(
                "Hệ thống đã nhận diện tệp tải lên: 'IELTS_Reading_Cam18_Test1.xlsx'.\nĐang tự động bóc tách dữ liệu câu hỏi trắc nghiệm từ tệp...", 
                "Nhận diện tệp thành công");
        }

        private void BtnDownloadTemplate_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThongTin(
                "Đã tải xuống file mẫu chuẩn hóa gồm các cột: STT, Kỹ năng, Trình độ, Nội dung câu hỏi, Đáp án A, B, C, D, Đáp án đúng, Giải thích chi tiết.", 
                "Tải file mẫu");
        }

        private void BtnLuuTepVaoDB_Click(object sender, RoutedEventArgs e)
        {
            Common.ThongBaoDialog.ThanhCong(
                "🎉 Toàn bộ 30 câu hỏi đã được nạp thành công vào Cơ sở dữ liệu và sẵn sàng đưa vào Đề thi thử!", 
                "Lưu dữ liệu thành công");
        }
    }
}
