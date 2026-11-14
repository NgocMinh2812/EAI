using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class ChiTietBaiLamSinhVienDialog : Window
    {
        private readonly int _maLuotLamBai;

        public ChiTietBaiLamSinhVienDialog(int maLuotLamBai)
        {
            InitializeComponent();
            _maLuotLamBai = maLuotLamBai;
            Loaded += ChiTietBaiLamSinhVienDialog_Loaded;
        }

        private void ChiTietBaiLamSinhVienDialog_Loaded(object sender, RoutedEventArgs e)
        {
            NapDuLieu();
        }

        private void NapDuLieu()
        {
            try
            {
                var dto = BaiTapDeThiBUS.Instance.LayChiTietKetQuaLamBai(_maLuotLamBai);
                if (dto == null)
                {
                    Common.ThongBaoDialog.CanhBao("Không tìm thấy dữ liệu lượt làm bài này.", "Thông báo");
                    Close();
                    return;
                }

                lblStudentTitle.Text = $"Chi tiết bài làm: {dto.HoTenNguoiDung} (@{dto.TenDangNhap})";
                lblStudentSubTitle.Text = $"Mã lượt #{dto.MaLuotLamBai} • {dto.TenBaiTapDeThi} • {dto.HinhThucHienThi} • Nộp: {dto.NgayNopFormatted}";

                txtScore.Text = dto.DiemHienThi;
                txtResultStatus.Text = dto.TrangThaiKetQuaText;
                borderResultBadge.Background = new BrushConverter().ConvertFromString(dto.TrangThaiKetQuaBg) as Brush ?? Brushes.LightGreen;
                txtResultStatus.Foreground = new BrushConverter().ConvertFromString(dto.TrangThaiKetQuaFg) as Brush ?? Brushes.DarkGreen;

                if (dto.TrangThai == "DangLam")
                {
                    txtDuration.Text = "Đang đếm giờ...";
                    txtCorrectCount.Text = $"-- / {dto.TongSoCau} câu";
                }
                else if (dto.TrangThai == "DaHuy")
                {
                    txtDuration.Text = "Đã hủy";
                    txtCorrectCount.Text = $"-- / {dto.TongSoCau} câu";
                }
                else
                {
                    txtDuration.Text = dto.ThoiGianHienThi;
                    txtCorrectCount.Text = $"{dto.SoCauDung} / {dto.TongSoCau} câu ({dto.TyLeChinhXac})";
                }

                pnlAnswerDetailsList.Children.Clear();
                if (dto.DanhSachChiTiet == null || dto.DanhSachChiTiet.Count == 0)
                {
                    pnlAnswerDetailsList.Children.Add(new TextBlock
                    {
                        Text = "Chưa ghi nhận chi tiết câu trả lời cho lượt thi này (hoặc bài đang làm dở).",
                        FontSize = 13.5,
                        FontWeight = FontWeights.Medium,
                        Foreground = (Brush)FindResource("TextMuted"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 40, 0, 0)
                    });
                    return;
                }

                int index = 1;
                foreach (var item in dto.DanhSachChiTiet)
                {
                    var card = TaoTheChiTietCauTraLoi(item, index++);
                    pnlAnswerDetailsList.Children.Add(card);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ChiTietBaiLamSinhVienDialog] {ex.Message}");
            }
        }

        private Border TaoTheChiTietCauTraLoi(ChiTietTraLoiDTO item, int stt)
        {
            bool isCorrect = item.KetQuaDungSai == true;
            var card = new Border
            {
                Background = isCorrect ? new SolidColorBrush(Color.FromRgb(240, 253, 244)) : new SolidColorBrush(Color.FromRgb(254, 242, 242)),
                BorderBrush = isCorrect ? new SolidColorBrush(Color.FromRgb(187, 247, 208)) : new SolidColorBrush(Color.FromRgb(254, 202, 202)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(14),
                Margin = new Thickness(0, 0, 0, 12)
            };

            var stack = new StackPanel();

            // 1. Header: STT, Kỹ năng, Điểm số, Badge Đúng / Sai
            var headGrid = new Grid { Margin = new Thickness(0, 0, 0, 8) };
            var headLeft = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };

            headLeft.Children.Add(new TextBlock
            {
                Text = $"Câu {stt}:",
                FontWeight = FontWeights.Bold,
                FontSize = 13,
                Foreground = (Brush)FindResource("TextDark"),
                Margin = new Thickness(0, 0, 8, 0)
            });

            string skillName = item.KyNang switch
            {
                "Doc" => "Đọc hiểu",
                "Nghe" => "Nghe hiểu",
                "TuVung" => "Từ vựng",
                "NguPhap" => "Ngữ pháp",
                "Viet" => "Viết",
                _ => item.KyNang
            };

            var skillBadge = new Border
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Margin = new Thickness(0, 0, 8, 0)
            };
            skillBadge.Child = new TextBlock { Text = skillName, FontSize = 11, FontWeight = FontWeights.SemiBold, Foreground = (Brush)FindResource("TextDark") };
            headLeft.Children.Add(skillBadge);

            if (item.DiemSo > 0)
            {
                headLeft.Children.Add(new TextBlock
                {
                    Text = $"+{item.DiemSo:0.0} pts",
                    FontSize = 11.5,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(15, 76, 58)),
                    VerticalAlignment = VerticalAlignment.Center
                });
            }

            headGrid.Children.Add(headLeft);

            // Badge Đúng / Sai
            var resultBadge = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = isCorrect ? new SolidColorBrush(Color.FromRgb(220, 252, 231)) : new SolidColorBrush(Color.FromRgb(254, 226, 226)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 2, 8, 2)
            };
            resultBadge.Child = new TextBlock
            {
                Text = isCorrect ? "✓ Chính xác" : "✗ Chưa đúng",
                FontSize = 11.5,
                FontWeight = FontWeights.Bold,
                Foreground = isCorrect ? new SolidColorBrush(Color.FromRgb(21, 128, 61)) : new SolidColorBrush(Color.FromRgb(220, 38, 38))
            };
            headGrid.Children.Add(resultBadge);
            stack.Children.Add(headGrid);

            // Đoạn văn tham chiếu (nếu có)
            if (!string.IsNullOrWhiteSpace(item.DoanVanDoc))
            {
                var docBorder = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = (Brush)FindResource("BorderLight"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 0, 0, 8)
                };
                docBorder.Child = new TextBlock
                {
                    Text = $"📖 Đoạn văn tham chiếu:\n{item.DoanVanDoc}",
                    FontSize = 12,
                    Foreground = (Brush)FindResource("TextDark"),
                    TextWrapping = TextWrapping.Wrap
                };
                stack.Children.Add(docBorder);
            }

            // Nội dung câu hỏi
            var txtQuestion = new TextBlock
            {
                Text = item.NoiDungCauHoi,
                FontSize = 13.5,
                FontWeight = FontWeights.SemiBold,
                Foreground = (Brush)FindResource("TextDark"),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            };
            stack.Children.Add(txtQuestion);

            // Câu trả lời của sinh viên
            string answerText;
            if (!string.IsNullOrWhiteSpace(item.NoiDungDaChon))
            {
                answerText = !string.IsNullOrWhiteSpace(item.KyHieuDaChon) ? $"{item.KyHieuDaChon}. {item.NoiDungDaChon}" : item.NoiDungDaChon;
            }
            else if (!string.IsNullOrWhiteSpace(item.CauTraLoiVanBan))
            {
                answerText = item.CauTraLoiVanBan;
            }
            else if (isCorrect && !string.IsNullOrWhiteSpace(item.NoiDungDapAnDung))
            {
                answerText = !string.IsNullOrWhiteSpace(item.KyHieuDapAnDung) ? $"{item.KyHieuDapAnDung}. {item.NoiDungDapAnDung}" : item.NoiDungDapAnDung;
            }
            else
            {
                answerText = "Chưa trả lời";
            }

            var studentAnsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 4) };
            studentAnsPanel.Children.Add(new TextBlock
            {
                Text = "Sinh viên đã chọn / trả lời: ",
                FontSize = 12.5,
                FontWeight = FontWeights.SemiBold,
                Foreground = (Brush)FindResource("TextDark")
            });
            studentAnsPanel.Children.Add(new TextBlock
            {
                Text = answerText,
                FontSize = 12.5,
                FontWeight = FontWeights.Bold,
                Foreground = isCorrect ? new SolidColorBrush(Color.FromRgb(21, 128, 61)) : new SolidColorBrush(Color.FromRgb(220, 38, 38))
            });
            stack.Children.Add(studentAnsPanel);

            // Nếu câu trả lời sai: hiển thị đáp án đúng chuẩn
            if (!isCorrect && !string.IsNullOrWhiteSpace(item.NoiDungDapAnDung))
            {
                var correctAnsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 4) };
                correctAnsPanel.Children.Add(new TextBlock
                {
                    Text = "Đáp án đúng chuẩn: ",
                    FontSize = 12.5,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = (Brush)FindResource("TextDark")
                });
                correctAnsPanel.Children.Add(new TextBlock
                {
                    Text = $"{item.KyHieuDapAnDung}. {item.NoiDungDapAnDung}",
                    FontSize = 12.5,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61))
                });
                stack.Children.Add(correctAnsPanel);
            }

            // Giải thích chi tiết nếu có
            if (!string.IsNullOrWhiteSpace(item.GiaiThichDapAn))
            {
                var txtExplain = new TextBlock
                {
                    Text = $"💡 Giải thích chi tiết: {item.GiaiThichDapAn}",
                    FontSize = 12.5,
                    Foreground = (Brush)FindResource("TextMuted"),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 4, 0, 0)
                };
                stack.Children.Add(txtExplain);
            }

            card.Child = stack;
            return card;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
