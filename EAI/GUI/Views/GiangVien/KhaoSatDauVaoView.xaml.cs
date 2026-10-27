using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class KhaoSatDauVaoView : Page
    {
        public KhaoSatDauVaoView()
        {
            InitializeComponent();
            Loaded += KhaoSatDauVaoView_Loaded;
        }

        private void KhaoSatDauVaoView_Loaded(object sender, RoutedEventArgs e)
        {
            TaiThongTinBaiKhaoSat();
            TaiMaTranQuyDoi();
            TaiDanhSachSinhVienKhaoSat();
        }

        public void TaiThongTinBaiKhaoSat()
        {
            try
            {
                var info = KhaoSatBUS.Instance.LayBaiKhaoSatDangHoatDong();
                txtTenBaiKhaoSatHienThi.Text = info.TenBai;
                txtMoTaBaiKhaoSat.Text = !string.IsNullOrWhiteSpace(info.MoTa) ? info.MoTa : "Chưa có mô tả chi tiết cho bài khảo sát này.";
                txtTongSoCauHienThi.Text = $"{info.TongSoCau} câu";

                if (info.DangHoatDong)
                {
                    txtTrangThaiBai.Text = "Đang kích hoạt";
                    txtTrangThaiBai.Foreground = new SolidColorBrush(Color.FromRgb(6, 95, 70));
                    badgeTrangThaiBai.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                }
                else
                {
                    txtTrangThaiBai.Text = "Tạm dừng";
                    txtTrangThaiBai.Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139));
                    badgeTrangThaiBai.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaiThongTinBaiKhaoSat] {ex.Message}");
            }
        }

        public void TaiMaTranQuyDoi()
        {
            try
            {
                var matrix = KhaoSatBUS.Instance.LayMaTranQuyDoi();
                pnlMatrixContainer.Children.Clear();

                foreach (var row in matrix)
                {
                    var grid = new Grid { Margin = new Thickness(0, 6, 0, 6) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    // Cột 1: Điểm %
                    var txtScoreRange = new TextBlock
                    {
                        Text = $"{row.DiemPhanTramMin}% - {row.DiemPhanTramMax}%",
                        FontSize = 13,
                        FontWeight = FontWeights.SemiBold,
                        Foreground = (Brush)FindResource("TextDark"),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(txtScoreRange, 0);
                    grid.Children.Add(txtScoreRange);

                    // Cột 2: Khung CEFR
                    Color badgeColor;
                    try
                    {
                        badgeColor = (Color)ColorConverter.ConvertFromString(row.MauSac ?? "#0F4C3A");
                    }
                    catch
                    {
                        badgeColor = Color.FromRgb(15, 76, 58);
                    }

                    var txtCefr = new TextBlock
                    {
                        Text = row.TenHienThi,
                        FontWeight = FontWeights.Bold,
                        FontSize = 13,
                        Foreground = new SolidColorBrush(badgeColor),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(txtCefr, 1);
                    grid.Children.Add(txtCefr);

                    // Cột 3: IELTS
                    var txtIelts = new TextBlock
                    {
                        Text = row.QuyDoiIelts,
                        FontSize = 12.5,
                        Foreground = (Brush)FindResource("TextDark"),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(txtIelts, 2);
                    grid.Children.Add(txtIelts);

                    // Cột 4: TOEIC
                    var txtToeic = new TextBlock
                    {
                        Text = row.QuyDoiToeic,
                        FontSize = 12.5,
                        Foreground = (Brush)FindResource("TextDark"),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Grid.SetColumn(txtToeic, 3);
                    grid.Children.Add(txtToeic);

                    // Cột 5: Lộ trình đề xuất
                    var txtLoTrinh = new TextBlock
                    {
                        Text = row.LoTrinhDeXuat,
                        FontSize = 12.5,
                        Foreground = (Brush)FindResource("TextGray"),
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap
                    };
                    Grid.SetColumn(txtLoTrinh, 4);
                    grid.Children.Add(txtLoTrinh);

                    pnlMatrixContainer.Children.Add(grid);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaiMaTranQuyDoi] {ex.Message}");
            }
        }

        public void TaiDanhSachSinhVienKhaoSat()
        {
            try
            {
                string tuKhoa = txtSearchStudent?.Text?.Trim() ?? "";
                string trinhDo = (cboFilterLevel?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "TatCa";

                var list = KhaoSatBUS.Instance.LayDanhSachSinhVienKhaoSat(tuKhoa, trinhDo);
                dgSinhVienKhaoSat.ItemsSource = list;
                txtTotalStudentsCount.Text = $"{list.Count} lượt nộp";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaiDanhSachSinhVienKhaoSat] {ex.Message}");
            }
        }

        private void BtnCapNhatBoDe_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CapNhatKhaoSatDialog { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                TaiThongTinBaiKhaoSat();
            }
        }

        private void BtnSuaMaTran_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SuaMaTranQuyDoiDialog { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                TaiMaTranQuyDoi();
            }
        }

        private void TxtSearchStudent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TaiDanhSachSinhVienKhaoSat();
        }

        private void CboFilterLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                TaiDanhSachSinhVienKhaoSat();
            }
        }

        private void BtnRefreshStudents_Click(object sender, RoutedEventArgs e)
        {
            TaiDanhSachSinhVienKhaoSat();
        }

        private void BtnXemChiTietKhaoSat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is SinhVienKhaoSatResultDTO student)
            {
                var dlg = new ChiTietKhaoSatSinhVienDialog(student) { Owner = Window.GetWindow(this) };
                dlg.ShowDialog();
            }
        }
    }
}
