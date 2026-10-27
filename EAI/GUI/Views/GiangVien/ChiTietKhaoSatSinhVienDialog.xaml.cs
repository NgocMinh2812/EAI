using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class ChiTietKhaoSatSinhVienDialog : Window
    {
        private readonly SinhVienKhaoSatResultDTO _student;

        public ChiTietKhaoSatSinhVienDialog(SinhVienKhaoSatResultDTO student)
        {
            InitializeComponent();
            _student = student;
            Loaded += ChiTietKhaoSatSinhVienDialog_Loaded;
        }

        private void ChiTietKhaoSatSinhVienDialog_Loaded(object sender, RoutedEventArgs e)
        {
            NapDuLieu();
        }

        private void NapDuLieu()
        {
            lblStudentTitle.Text = $"Chi tiết bài làm khảo sát: {_student.HoTen} ({_student.TenDangNhap})";
            lblStudentSubTitle.Text = $"Lượt thi #{_student.MaLuotKhaoSat} • Nộp bài: {_student.ThoiDiemNopBaiHienThi} • Email: {_student.Email}";

            txtScore.Text = $"{_student.DiemDatDuoc:0.0} / 10";
            txtCorrectCount.Text = $"{_student.SoCauDung} / {_student.TongSoCau} câu";
            txtLevelResult.Text = $"{_student.MaCapDo} ({_student.TenTrinhDo})";
            txtEquiv.Text = $"{_student.QuyDoiIelts} • {_student.QuyDoiToeic}";

            // Tải chi tiết các câu trả lời
            var details = KhaoSatBUS.Instance.LayChiTietKhaoSatHocVien(_student.MaLuotKhaoSat);
            pnlAnswerDetailsList.Children.Clear();

            if (details == null || details.Count == 0)
            {
                pnlAnswerDetailsList.Children.Add(new TextBlock
                {
                    Text = "Không tìm thấy dữ liệu chi tiết câu trả lời cho lượt khảo sát này.",
                    FontSize = 13,
                    Foreground = (Brush)FindResource("TextGray"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 30, 0, 0)
                });
                return;
            }

            int index = 1;
            foreach (var item in details)
            {
                var card = TaoTheChiTietCauTraLoi(item, index++);
                pnlAnswerDetailsList.Children.Add(card);
            }
        }

        private Border TaoTheChiTietCauTraLoi(ChiTietTraLoiKhaoSatItemDTO item, int stt)
        {
            var card = new Border
            {
                Background = item.KetQuaDungSai ? new SolidColorBrush(Color.FromRgb(240, 253, 244)) : new SolidColorBrush(Color.FromRgb(254, 242, 242)),
                BorderBrush = item.KetQuaDungSai ? new SolidColorBrush(Color.FromRgb(187, 247, 208)) : new SolidColorBrush(Color.FromRgb(254, 202, 202)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(14),
                Margin = new Thickness(0, 0, 0, 12)
            };

            var stack = new StackPanel();

            // 1. Header: STT, Kỹ năng, Cấp độ, Badge Đúng / Sai
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

            var skillBadge = new Border
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2),
                Margin = new Thickness(0, 0, 6, 0)
            };
            skillBadge.Child = new TextBlock { Text = item.KyNang, FontSize = 11, FontWeight = FontWeights.SemiBold, Foreground = (Brush)FindResource("TextDark") };
            headLeft.Children.Add(skillBadge);

            var levelBadge = new Border
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(6, 2, 6, 2)
            };
            levelBadge.Child = new TextBlock { Text = item.MaTrinhDo, FontSize = 11, FontWeight = FontWeights.SemiBold, Foreground = (Brush)FindResource("TextDark") };
            headLeft.Children.Add(levelBadge);

            headGrid.Children.Add(headLeft);

            // Badge Đúng / Sai
            var resultBadge = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = item.KetQuaDungSai ? new SolidColorBrush(Color.FromRgb(220, 252, 231)) : new SolidColorBrush(Color.FromRgb(254, 226, 226)),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(8, 2, 8, 2)
            };
            resultBadge.Child = new TextBlock
            {
                Text = item.KetQuaDungSai ? "✓ Chính xác" : "✗ Chưa đúng",
                FontSize = 11.5,
                FontWeight = FontWeights.Bold,
                Foreground = item.KetQuaDungSai ? new SolidColorBrush(Color.FromRgb(21, 128, 61)) : new SolidColorBrush(Color.FromRgb(220, 38, 38))
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
                    Padding = new Thickness(8),
                    Margin = new Thickness(0, 0, 0, 6)
                };
                docBorder.Child = new TextBlock
                {
                    Text = $"📖 Đoạn văn tham chiếu:\n{item.DoanVanDoc}",
                    FontSize = 12,
                    Foreground = (Brush)FindResource("TextDark"),
                    TextWrapping = TextWrapping.Wrap,
                    FontStyle = FontStyles.Italic
                };
                stack.Children.Add(docBorder);
            }

            // 2. Nội dung câu hỏi
            stack.Children.Add(new TextBlock
            {
                Text = item.NoiDungCauHoi,
                FontWeight = FontWeights.SemiBold,
                FontSize = 13,
                Foreground = (Brush)FindResource("TextDark"),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            });

            // 3. Câu trả lời của sinh viên & Đáp án đúng
            var ansBorder = new Border
            {
                Background = Brushes.White,
                BorderBrush = (Brush)FindResource("BorderLight"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 6)
            };

            var ansStack = new StackPanel();

            string studentAns = !string.IsNullOrWhiteSpace(item.KyHieuDaChon)
                ? $"{item.KyHieuDaChon}) {item.NoiDungDaChon}"
                : "Không chọn đáp án";

            var txtStudent = new TextBlock
            {
                Text = $"• Sinh viên đã chọn: {studentAns}",
                FontSize = 12.5,
                FontWeight = FontWeights.Bold,
                Foreground = item.KetQuaDungSai ? new SolidColorBrush(Color.FromRgb(21, 128, 61)) : new SolidColorBrush(Color.FromRgb(220, 38, 38)),
                Margin = new Thickness(0, 0, 0, 4)
            };
            ansStack.Children.Add(txtStudent);

            var dapAnDung = item.DanhSachLuaChon?.FirstOrDefault(l => l.LaDapAnDung);
            if (dapAnDung != null)
            {
                var txtCorrect = new TextBlock
                {
                    Text = $"• Đáp án đúng: {dapAnDung.KyHieuLuaChon}) {dapAnDung.NoiDungLuaChon}",
                    FontSize = 12.5,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61))
                };
                ansStack.Children.Add(txtCorrect);
            }

            ansBorder.Child = ansStack;
            stack.Children.Add(ansBorder);

            // 4. Giải thích chi tiết
            if (!string.IsNullOrWhiteSpace(item.GiaiThichDapAn))
            {
                stack.Children.Add(new TextBlock
                {
                    Text = $"💡 Giải thích: {item.GiaiThichDapAn}",
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(3, 105, 161)),
                    TextWrapping = TextWrapping.Wrap
                });
            }

            card.Child = stack;
            return card;
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
