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
        private List<SinhVienKhaoSatResultDTO> _allStudents = new();
        private int _currentStudentPage = 1;
        private const int StudentPageSize = 10;
        private int _totalStudentPages = 1;

        public KhaoSatDauVaoView()
        {
            InitializeComponent();
            Loaded += KhaoSatDauVaoView_Loaded;
        }

        public KhaoSatDauVaoView(string? searchKeyword = null, string? filterLevel = null) : this()
        {
            Loaded += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(searchKeyword) && txtSearchStudent != null)
                {
                    txtSearchStudent.Text = searchKeyword.Trim();
                }

                if (!string.IsNullOrWhiteSpace(filterLevel) && cboFilterLevel != null)
                {
                    foreach (ComboBoxItem item in cboFilterLevel.Items)
                    {
                        if (string.Equals(item.Tag?.ToString(), filterLevel, StringComparison.OrdinalIgnoreCase))
                        {
                            cboFilterLevel.SelectedItem = item;
                            break;
                        }
                    }
                }

                TaiDanhSachSinhVienKhaoSat();
            };
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

                if (txtKpiTotalSets != null)
                {
                    txtKpiTotalSets.Text = $"{_dsBaiKhaoSat.Count} Bộ (A1 - C2 & TH)";
                }
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

        public void TaiDanhSachSinhVienKhaoSat(bool resetPage = false)
        {
            try
            {
                string tuKhoa = txtSearchStudent?.Text?.Trim() ?? "";
                string trinhDo = (cboFilterLevel?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "TatCa";

                _allStudents = KhaoSatBUS.Instance.LayDanhSachSinhVienKhaoSat(tuKhoa, trinhDo) ?? new List<SinhVienKhaoSatResultDTO>();
                CapNhatPhanTrangSinhVien(resetPage);

                // Tính toán KPI
                txtKpiTotalAttempts.Text = $"{_allStudents.Count} lượt nộp";
                if (_allStudents.Count > 0)
                {
                    decimal avgScore = _allStudents.Average(s => s.DiemDatDuoc);
                    txtKpiAvgScore.Text = $"{avgScore.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)} / 10";

                    int passCount = _allStudents.Count(s => s.DiemDatDuoc >= 5.0m);
                    double passRate = (double)passCount * 100.0 / _allStudents.Count;
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

        private void CapNhatPhanTrangSinhVien(bool resetPage = false)
        {
            if (resetPage) _currentStudentPage = 1;

            int totalCount = _allStudents.Count;
            _totalStudentPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / StudentPageSize));

            if (_currentStudentPage > _totalStudentPages) _currentStudentPage = _totalStudentPages;
            if (_currentStudentPage < 1) _currentStudentPage = 1;

            var pagedItems = _allStudents.Skip((_currentStudentPage - 1) * StudentPageSize).Take(StudentPageSize).ToList();
            dgSinhVienKhaoSat.ItemsSource = pagedItems;
            txtTotalStudentsCount.Text = $"{totalCount} lượt nộp";

            if (pnlEmptyStudents != null)
            {
                pnlEmptyStudents.Visibility = totalCount == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            if (dgSinhVienKhaoSat != null)
            {
                dgSinhVienKhaoSat.Visibility = totalCount == 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            if (pnlStudentPagination != null)
            {
                pnlStudentPagination.Visibility = totalCount > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            int fromIndex = totalCount == 0 ? 0 : (_currentStudentPage - 1) * StudentPageSize + 1;
            int toIndex = Math.Min(totalCount, _currentStudentPage * StudentPageSize);

            if (txtStudentCounter != null)
                txtStudentCounter.Text = $"Hiển thị {fromIndex} - {toIndex} / {totalCount} lượt nộp";

            if (txtStudentPageInfo != null)
                txtStudentPageInfo.Text = $"Trang {_currentStudentPage} / {_totalStudentPages}";

            if (btnPrevStudentPage != null)
                btnPrevStudentPage.IsEnabled = _currentStudentPage > 1;

            if (btnNextStudentPage != null)
                btnNextStudentPage.IsEnabled = _currentStudentPage < _totalStudentPages;
        }

        private void BtnPrevStudentPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStudentPage > 1)
            {
                _currentStudentPage--;
                CapNhatPhanTrangSinhVien();
            }
        }

        private void BtnNextStudentPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentStudentPage < _totalStudentPages)
            {
                _currentStudentPage++;
                CapNhatPhanTrangSinhVien();
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
                if (questions == null || questions.Count == 0)
                {
                    ThongBaoDialog.CanhBao($"Bộ đề \"{survey.TenBaiKhaoSat}\" hiện chưa có câu hỏi nào để xem trước. Vui lòng bấm 'Chỉnh sửa' để thêm câu hỏi.", "Thông báo");
                    return;
                }

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
            TaiDanhSachSinhVienKhaoSat(resetPage: true);
        }

        private void CboFilterLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                TaiDanhSachSinhVienKhaoSat(resetPage: true);
            }
        }

        private void BtnRefreshStudents_Click(object sender, RoutedEventArgs e)
        {
            TaiDanhSachSinhVienKhaoSat(resetPage: true);
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
