using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using DTO;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class KhaoSatDauVaoView : Page
    {
        private List<BaiKhaoSatDTO> _dsBaiKhaoSat = new();

        public KhaoSatDauVaoView()
        {
            InitializeComponent();
            Loaded += KhaoSatDauVaoView_Loaded;
        }

        private void KhaoSatDauVaoView_Loaded(object sender, RoutedEventArgs e)
        {
            TaiDanhSachBoDeKhaoSat();
            TaiMaTranQuyDoi();
            TaiDanhSachSinhVienKhaoSat();
        }

        public void TaiDanhSachBoDeKhaoSat()
        {
            try
            {
                _dsBaiKhaoSat = KhaoSatBUS.Instance.LayDanhSachTatCaBaiKhaoSat();
                icSurveySets.ItemsSource = null;
                icSurveySets.ItemsSource = _dsBaiKhaoSat;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaiDanhSachBoDeKhaoSat] {ex.Message}");
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

                // Tính toán KPI
                txtKpiTotalAttempts.Text = $"{list.Count} lượt nộp";
                if (list.Count > 0)
                {
                    decimal avgScore = list.Average(s => s.DiemDatDuoc);
                    txtKpiAvgScore.Text = $"{avgScore.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)} / 10";

                    int passCount = list.Count(s => s.DiemDatDuoc >= 5.0m);
                    double passRate = (double)passCount * 100.0 / list.Count;
                    txtKpiPassRate.Text = $"{passRate:0}%";
                }
                else
                {
                    txtKpiAvgScore.Text = "0.0 / 10";
                    txtKpiPassRate.Text = "0%";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaiDanhSachSinhVienKhaoSat] {ex.Message}");
            }
        }

        private void BtnCapNhatBoDe_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CapNhatKhaoSatDialog();
            if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                TaiDanhSachBoDeKhaoSat();
                TaiDanhSachSinhVienKhaoSat();
            }
        }

        private void BtnChinhSuaBoDeCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is BaiKhaoSatDTO survey)
            {
                var dlg = new CapNhatKhaoSatDialog(survey.MaBaiKhaoSat);
                if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
                {
                    TaiDanhSachBoDeKhaoSat();
                    TaiDanhSachSinhVienKhaoSat();
                }
            }
        }

        private void BtnXemTruocBoDe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is BaiKhaoSatDTO survey)
            {
                var questions = KhaoSatBUS.Instance.LayDanhSachCauHoiTheoBaiKhaoSat(survey.MaBaiKhaoSat);
                var fakeDeThi = new BaiTapDeThiDTO
                {
                    TenBaiTapDeThi = survey.TenBaiKhaoSat,
                    LoaiHinh = "KhaoSat",
                    ThoiGianLamBaiPhut = survey.ThoiGianPhut,
                    TongSoCau = survey.TongSoCauHoi,
                    MoTa = survey.MoTa
                };

                var dlg = new XemTruocDeThiDialog(fakeDeThi, questions);
                ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));
            }
        }

        private void BtnRefreshSurveys_Click(object sender, RoutedEventArgs e)
        {
            TaiDanhSachBoDeKhaoSat();
        }

        private void BtnSuaMaTran_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SuaMaTranQuyDoiDialog();
            if (ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this)) == true)
            {
                TaiMaTranQuyDoi();
            }
        }

        private void TxtSearchStudent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchStudentPlaceholder != null)
            {
                txtSearchStudentPlaceholder.Visibility = string.IsNullOrEmpty(txtSearchStudent.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
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
                var dlg = new ChiTietKhaoSatSinhVienDialog(student);
                ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));
            }
        }
    }
}
