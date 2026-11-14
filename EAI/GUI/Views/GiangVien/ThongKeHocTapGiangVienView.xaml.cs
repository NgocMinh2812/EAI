using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class ThongKeHocTapGiangVienView : Page
    {
        private ThongKeHocTapFilterDTO _currentFilter = new();
        private BaoCaoThongKeHocTapDTO? _currentReport;
        private List<LuotLamBaiDTO> _allSubmissions = new();
        private int _currentSubPage = 1;
        private const int SubPageSize = 10;
        private int _totalSubPages = 1;

        public ThongKeHocTapGiangVienView()
        {
            InitializeComponent();
            Loaded += ThongKeHocTapGiangVienView_Loaded;
        }

        private void ThongKeHocTapGiangVienView_Loaded(object sender, RoutedEventArgs e)
        {
            NapDanhSachDeThi();
            // Mặc định chọn 7 ngày qua đồng bộ cả giao diện nút bấm và số liệu
            BtnFilter7Ngay_Click(btnFilter7Ngay, null!);
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            NapDanhSachDeThi();
            TaiBaoCaoThongKe(resetPage: true);
        }

        private void NapDanhSachDeThi()
        {
            try
            {
                var list = BaoCaoThongKeBUS.Instance.LayDanhSachDeThiFilter();
                cboDeThi.Items.Clear();

                cboDeThi.Items.Add(new ComboBoxItem { Content = "Tất cả bài tập & Đề thi", Tag = 0 });
                foreach (var item in list)
                {
                    cboDeThi.Items.Add(new ComboBoxItem { Content = item.TenBaiTapDeThi, Tag = item.MaBaiTapDeThi });
                }
                cboDeThi.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NapDanhSachDeThi] {ex.Message}");
            }
        }

        public void TaiBaoCaoThongKe(bool resetPage = false)
        {
            try
            {
                var report = BaoCaoThongKeBUS.Instance.LayBaoCaoThongKe(_currentFilter);
                _currentReport = report;

                // 1. Cập nhật 6 thẻ KPI
                txtKpiTongLuot.Text = report.TongSoLuotLam.ToString();
                txtKpiDaNop.Text = report.SoLuotDaNop.ToString();
                txtKpiDiemTB.Text = report.DiemTrungBinh.ToString("0.0");
                txtKpiThoiGianTB.Text = (report.ThoiGianTrungBinhGiay / 60.0).ToString("0.0");
                txtKpiTyLeHoanThanh.Text = report.TyLeHoanThanh.ToString("0.0") + "%";
                txtKpiHocVien.Text = report.SoHocVienThamGia.ToString();

                // 2. Vẽ biểu đồ 1: Phân bố điểm số (Histogram)
                VeBieuDoPhanBoDiem(report.DanhSachPhanBoDiem);

                // 3. Vẽ biểu đồ 2: Hiệu suất theo kỹ năng
                VeBieuDoKyNang(report.DanhSachKyNang);

                // 4. Vẽ biểu đồ 3: Xu hướng theo ngày
                VeBieuDoXuHuongNgay(report.DanhSachXuHuongNgay);

                // 5. Vẽ biểu đồ 4: Hoàn thành theo bài thi (Tính toán từ danh sách lượt làm bài)
                VeBieuDoKyThi(report.DanhSachLuotLamBai);

                // 6. Đổ danh sách lượt làm bài có phân trang
                _allSubmissions = report.DanhSachLuotLamBai ?? new List<LuotLamBaiDTO>();
                CapNhatPhanTrangSubmissions(resetPage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaiBaoCaoThongKe] {ex.Message}");
            }
        }

        private void CapNhatPhanTrangSubmissions(bool resetPage = false)
        {
            if (resetPage) _currentSubPage = 1;

            int totalCount = _allSubmissions.Count;
            _totalSubPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / SubPageSize));

            if (_currentSubPage > _totalSubPages) _currentSubPage = _totalSubPages;
            if (_currentSubPage < 1) _currentSubPage = 1;

            var pagedItems = _allSubmissions.Skip((_currentSubPage - 1) * SubPageSize).Take(SubPageSize).ToList();
            dgDanhSachBaiNop.ItemsSource = pagedItems;
            txtSubmissionsCount.Text = $"{totalCount} lượt làm bài";

            bool coDuLieu = totalCount > 0;
            dgDanhSachBaiNop.Visibility = coDuLieu ? Visibility.Visible : Visibility.Collapsed;
            if (pnlEmptySubmissions != null)
            {
                pnlEmptySubmissions.Visibility = coDuLieu ? Visibility.Collapsed : Visibility.Visible;
            }
            if (pnlSubPagination != null)
            {
                pnlSubPagination.Visibility = coDuLieu ? Visibility.Visible : Visibility.Collapsed;
            }

            int fromIndex = totalCount == 0 ? 0 : (_currentSubPage - 1) * SubPageSize + 1;
            int toIndex = Math.Min(totalCount, _currentSubPage * SubPageSize);

            if (txtSubCounter != null)
                txtSubCounter.Text = $"Hiển thị {fromIndex} - {toIndex} / {totalCount} lượt làm bài";

            if (txtSubPageInfo != null)
                txtSubPageInfo.Text = $"Trang {_currentSubPage} / {_totalSubPages}";

            if (btnPrevSubPage != null)
                btnPrevSubPage.IsEnabled = _currentSubPage > 1;

            if (btnNextSubPage != null)
                btnNextSubPage.IsEnabled = _currentSubPage < _totalSubPages;
        }

        private void BtnPrevSubPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSubPage > 1)
            {
                _currentSubPage--;
                CapNhatPhanTrangSubmissions();
            }
        }

        private void BtnNextSubPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSubPage < _totalSubPages)
            {
                _currentSubPage++;
                CapNhatPhanTrangSubmissions();
            }
        }

        #region Render Visual Charts
        private void VeBieuDoPhanBoDiem(List<PhanBoDiemItem> items)
        {
            pnlChartPhanBoDiem.Children.Clear();
            if (items == null || items.Count == 0)
            {
                pnlChartPhanBoDiem.Children.Add(new TextBlock { Text = "Chưa có dữ liệu bài nộp trong khoảng thời gian này.", FontSize = 12, Foreground = (Brush)FindResource("TextGray"), Margin = new Thickness(0, 10, 0, 0) });
                return;
            }

            foreach (var item in items)
            {
                var rowStack = new StackPanel { Margin = new Thickness(0, 0, 0, 10) };

                var infoGrid = new Grid { Margin = new Thickness(0, 0, 0, 4) };
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                var txtName = new TextBlock { Text = $"{item.KhoangDiem} ({item.NhanXet})", FontSize = 12, FontWeight = FontWeights.SemiBold, Foreground = (Brush)FindResource("TextDark") };
                var txtCount = new TextBlock { Text = $"{item.SoLuot} lượt ({item.TyLePhanTram:0.0}%)", FontSize = 11.5, Foreground = (Brush)FindResource("TextGray") };
                Grid.SetColumn(txtCount, 1);

                infoGrid.Children.Add(txtName);
                infoGrid.Children.Add(txtCount);
                rowStack.Children.Add(infoGrid);

                // Progress Bar Meter
                var trackBorder = new Border { Height = 8, Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)), CornerRadius = new CornerRadius(4) };
                var barFill = new Border
                {
                    Height = 8,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = Math.Max(4, item.TyLePhanTram * 4.5),
                    CornerRadius = new CornerRadius(4),
                    Background = LayMauTheoKhoangDiem(item.KhoangDiem)
                };
                trackBorder.Child = barFill;
                rowStack.Children.Add(trackBorder);

                pnlChartPhanBoDiem.Children.Add(rowStack);
            }
        }

        private Brush LayMauTheoKhoangDiem(string nhan)
        {
            if (nhan.Contains("8.0") || nhan.Contains("Giỏi")) return new SolidColorBrush(Color.FromRgb(16, 185, 129)); // Green
            if (nhan.Contains("6.5") || nhan.Contains("Khá")) return new SolidColorBrush(Color.FromRgb(59, 130, 246)); // Blue
            if (nhan.Contains("5.0") || nhan.Contains("Trung bình")) return new SolidColorBrush(Color.FromRgb(245, 158, 11)); // Amber
            return new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Red
        }

        private void VeBieuDoKyNang(List<KyNangThongKeItem> items)
        {
            pnlChartKyNang.Children.Clear();
            if (items == null || items.Count == 0)
            {
                pnlChartKyNang.Children.Add(new TextBlock { Text = "Chưa ghi nhận câu hỏi theo kỹ năng.", FontSize = 12, Foreground = (Brush)FindResource("TextGray"), Margin = new Thickness(0, 10, 0, 0) });
                return;
            }

            foreach (var kn in items)
            {
                var rowStack = new StackPanel { Margin = new Thickness(0, 0, 0, 10) };

                var infoGrid = new Grid { Margin = new Thickness(0, 0, 0, 4) };
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                var txtName = new TextBlock { Text = kn.TenKyNang, FontSize = 12, FontWeight = FontWeights.SemiBold, Foreground = (Brush)FindResource("TextDark") };
                var txtScore = new TextBlock { Text = $"{kn.SoCauDung}/{kn.TongSoCau} đúng • {kn.TyLeDung:0.0}%", FontSize = 11.5, Foreground = (Brush)FindResource("TextGray") };
                Grid.SetColumn(txtScore, 1);

                infoGrid.Children.Add(txtName);
                infoGrid.Children.Add(txtScore);
                rowStack.Children.Add(infoGrid);

                var trackBorder = new Border { Height = 8, Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)), CornerRadius = new CornerRadius(4) };
                var barFill = new Border
                {
                    Height = 8,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = Math.Max(4, kn.TyLeDung * 4.5),
                    CornerRadius = new CornerRadius(4),
                    Background = new SolidColorBrush(Color.FromRgb(15, 76, 58))
                };
                trackBorder.Child = barFill;
                rowStack.Children.Add(trackBorder);

                pnlChartKyNang.Children.Add(rowStack);
            }
        }

        private void VeBieuDoXuHuongNgay(List<XuHuongNgayItem> items)
        {
            pnlChartXuHuongNgay.Children.Clear();
            if (items == null || items.Count == 0)
            {
                pnlChartXuHuongNgay.Children.Add(new TextBlock { Text = "Chưa có lượt nộp theo mốc thời gian này.", FontSize = 12, Foreground = (Brush)FindResource("TextGray"), Margin = new Thickness(0, 10, 0, 0) });
                return;
            }

            int maxCount = items.Max(i => i.SoLuot);
            if (maxCount == 0) maxCount = 1;

            foreach (var day in items.Take(7))
            {
                var rowStack = new StackPanel { Margin = new Thickness(0, 0, 0, 8) };

                var infoGrid = new Grid { Margin = new Thickness(0, 0, 0, 3) };
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                var txtDate = new TextBlock { Text = day.Ngay, FontSize = 12, Foreground = (Brush)FindResource("TextDark") };
                Grid.SetColumn(txtDate, 0);

                var track = new Border { Height = 8, Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)), CornerRadius = new CornerRadius(4), VerticalAlignment = VerticalAlignment.Center };
                double w = (double)day.SoLuot / maxCount * 280;
                var fill = new Border { Height = 8, HorizontalAlignment = HorizontalAlignment.Left, Width = Math.Max(4, w), CornerRadius = new CornerRadius(4), Background = new SolidColorBrush(Color.FromRgb(37, 99, 235)) };
                track.Child = fill;
                Grid.SetColumn(track, 1);

                var txtCount = new TextBlock { Text = $" {day.SoLuot} lượt (ĐTB: {day.DiemTrungBinh:0.0})", FontSize = 11.5, FontWeight = FontWeights.Bold, Foreground = (Brush)FindResource("TextDark"), Margin = new Thickness(8, 0, 0, 0) };
                Grid.SetColumn(txtCount, 2);

                infoGrid.Children.Add(txtDate);
                infoGrid.Children.Add(track);
                infoGrid.Children.Add(txtCount);

                rowStack.Children.Add(infoGrid);
                pnlChartXuHuongNgay.Children.Add(rowStack);
            }
        }

        private void VeBieuDoKyThi(List<LuotLamBaiDTO> submissions)
        {
            pnlChartKyThi.Children.Clear();
            if (submissions == null || submissions.Count == 0)
            {
                pnlChartKyThi.Children.Add(new TextBlock { Text = "Chưa có số liệu theo từng bài thi.", FontSize = 12, Foreground = (Brush)FindResource("TextGray"), Margin = new Thickness(0, 10, 0, 0) });
                return;
            }

            var groupDeThi = submissions
                .GroupBy(lb => string.IsNullOrWhiteSpace(lb.TenBaiTapDeThi) ? "Bài tập thực hành" : lb.TenBaiTapDeThi)
                .Select(g => new
                {
                    TenBai = g.Key,
                    TongLuot = g.Count(),
                    DaNop = g.Count(x => x.TrangThai == "DaNop"),
                    TyLe = g.Count() > 0 ? (g.Count(x => x.TrangThai == "DaNop") * 100.0 / g.Count()) : 0.0
                })
                .OrderByDescending(x => x.TongLuot)
                .Take(5)
                .ToList();

            foreach (var kt in groupDeThi)
            {
                var rowStack = new StackPanel { Margin = new Thickness(0, 0, 0, 10) };

                var infoGrid = new Grid { Margin = new Thickness(0, 0, 0, 4) };
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                string title = kt.TenBai.Length > 32 ? kt.TenBai.Substring(0, 30) + "..." : kt.TenBai;
                var txtName = new TextBlock { Text = title, FontSize = 12, FontWeight = FontWeights.SemiBold, Foreground = (Brush)FindResource("TextDark") };
                var txtStat = new TextBlock { Text = $"{kt.DaNop}/{kt.TongLuot} lượt nộp ({kt.TyLe:0.0}%)", FontSize = 11.5, Foreground = (Brush)FindResource("TextGray") };
                Grid.SetColumn(txtStat, 1);

                infoGrid.Children.Add(txtName);
                infoGrid.Children.Add(txtStat);
                rowStack.Children.Add(infoGrid);

                var track = new Border { Height = 8, Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)), CornerRadius = new CornerRadius(4) };
                var fill = new Border { Height = 8, HorizontalAlignment = HorizontalAlignment.Left, Width = Math.Max(4, kt.TyLe * 4.5), CornerRadius = new CornerRadius(4), Background = new SolidColorBrush(Color.FromRgb(16, 185, 129)) };
                track.Child = fill;
                rowStack.Children.Add(track);

                pnlChartKyThi.Children.Add(rowStack);
            }
        }
        #endregion

        #region Time Filter Buttons
        private void ResetTimeButtons()
        {
            btnFilterHomNay.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            btnFilterHomNay.Foreground = (Brush)FindResource("TextDark");

            btnFilter7Ngay.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            btnFilter7Ngay.Foreground = (Brush)FindResource("TextDark");

            btnFilter30Ngay.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            btnFilter30Ngay.Foreground = (Brush)FindResource("TextDark");

            btnFilterTatCa.Background = new SolidColorBrush(Color.FromRgb(241, 245, 249));
            btnFilterTatCa.Foreground = (Brush)FindResource("TextDark");
        }

        private void BtnFilterHomNay_Click(object sender, RoutedEventArgs e)
        {
            ResetTimeButtons();
            btnFilterHomNay.Background = (Brush)FindResource("AdminDarkGreen");
            btnFilterHomNay.Foreground = Brushes.White;

            _currentFilter.TuNgay = DateTime.Today;
            _currentFilter.DenNgay = DateTime.Today;
            CapNhatBoLocTuGiaoDien();
        }

        private void BtnFilter7Ngay_Click(object sender, RoutedEventArgs e)
        {
            ResetTimeButtons();
            btnFilter7Ngay.Background = (Brush)FindResource("AdminDarkGreen");
            btnFilter7Ngay.Foreground = Brushes.White;

            _currentFilter.TuNgay = DateTime.Today.AddDays(-7);
            _currentFilter.DenNgay = DateTime.Today;
            CapNhatBoLocTuGiaoDien();
        }

        private void BtnFilter30Ngay_Click(object sender, RoutedEventArgs e)
        {
            ResetTimeButtons();
            btnFilter30Ngay.Background = (Brush)FindResource("AdminDarkGreen");
            btnFilter30Ngay.Foreground = Brushes.White;

            _currentFilter.TuNgay = DateTime.Today.AddDays(-30);
            _currentFilter.DenNgay = DateTime.Today;
            CapNhatBoLocTuGiaoDien();
        }

        private void BtnFilterTatCa_Click(object sender, RoutedEventArgs e)
        {
            ResetTimeButtons();
            btnFilterTatCa.Background = (Brush)FindResource("AdminDarkGreen");
            btnFilterTatCa.Foreground = Brushes.White;

            _currentFilter.TuNgay = null;
            _currentFilter.DenNgay = null;
            CapNhatBoLocTuGiaoDien();
        }

        private void BtnApDungLoc_Click(object sender, RoutedEventArgs e)
        {
            CapNhatBoLocTuGiaoDien();
        }

        private void CapNhatBoLocTuGiaoDien()
        {
            // Lấy ID đề thi
            if (cboDeThi?.SelectedItem is ComboBoxItem itemDe && itemDe.Tag is int maDe)
            {
                _currentFilter.MaBaiTapDeThi = maDe > 0 ? maDe : null;
            }

            // Lấy hình thức
            if (cboHinhThuc?.SelectedItem is ComboBoxItem itemHt && itemHt.Tag is string ht)
            {
                _currentFilter.HinhThucLam = ht;
            }

            // Lấy từ khóa
            _currentFilter.TuKhoa = txtSearch?.Text?.Trim() ?? string.Empty;

            TaiBaoCaoThongKe(resetPage: true);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CapNhatBoLocTuGiaoDien();
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchPlaceholder != null)
            {
                txtSearchPlaceholder.Visibility = string.IsNullOrEmpty(txtSearch.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void BtnXemChiTietBaiLam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int maLuot && maLuot > 0)
            {
                MoDialogChiTietBaiLam(maLuot);
            }
        }

        private void DgDanhSachBaiNop_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgDanhSachBaiNop.SelectedItem is LuotLamBaiDTO item && item.MaLuotLamBai > 0)
            {
                MoDialogChiTietBaiLam(item.MaLuotLamBai);
            }
        }

        private void MoDialogChiTietBaiLam(int maLuotLamBai)
        {
            try
            {
                var dialog = new ChiTietBaiLamSinhVienDialog(maLuotLamBai);
                Common.ModalHelper.ShowDialogWithDim(dialog, Window.GetWindow(this));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MoDialogChiTietBaiLam] {ex.Message}");
            }
        }
        #endregion

        #region Excel Export
        private void BtnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            if (_currentReport == null)
            {
                Common.ThongBaoDialog.CanhBao("Chưa có số liệu báo cáo để xuất Excel.", "Thông báo");
                return;
            }

            if (_currentReport.TongSoLuotLam == 0)
            {
                bool tiepTuc = Common.ThongBaoDialog.XacNhan(
                    "Hiện tại danh sách không có dữ liệu lượt nộp bài nào theo bộ lọc đang chọn.\n\nBạn có chắc chắn muốn tiếp tục xuất tệp báo cáo tổng quan KPI không?",
                    "Xác nhận xuất Excel",
                    "Tiếp tục xuất",
                    "Hủy bỏ");
                if (!tiepTuc) return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Title = "Xuất Báo cáo Thống kê Học tập (.xlsx)",
                Filter = "Excel Spreadsheet (*.xlsx)|*.xlsx",
                FileName = $"BaoCao_ThongKe_HocTap_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    bool thanhCong = BaoCaoThongKeBUS.Instance.XuatBaoCaoExcel(_currentReport, saveFileDialog.FileName, out string errorMessage);
                    if (thanhCong)
                    {
                        bool openFile = Common.ThongBaoDialog.XacNhan(
                            $"🎉 Đã xuất thành công Báo cáo thống kê học tập (gồm 2 Sheet: Tổng quan KPI và Danh sách bài nộp) tại:\n{saveFileDialog.FileName}\n\nBạn có muốn mở tệp Excel ngay bây giờ không?", 
                            "Xuất Excel thành công",
                            "Mở tệp",
                            "Đóng");

                        if (openFile)
                        {
                            try
                            {
                                Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                            }
                            catch (Exception exOpen)
                            {
                                System.Diagnostics.Debug.WriteLine($"[Process.Start] {exOpen.Message}");
                            }
                        }
                    }
                    else
                    {
                        string friendlyMsg = errorMessage.Contains("being used by another process") || errorMessage.Contains("process cannot access")
                            ? "Tệp Excel đang được mở bởi ứng dụng khác (ví dụ: Microsoft Excel). Vui lòng đóng tệp trước khi xuất lại."
                            : errorMessage;
                        Common.ThongBaoDialog.Loi($"Không thể xuất tệp Excel: {friendlyMsg}", "Lỗi xuất báo cáo");
                    }
                }
                catch (IOException ioEx)
                {
                    Common.ThongBaoDialog.Loi($"Tệp Excel đang được mở hoặc bị khóa bởi ứng dụng khác:\n{ioEx.Message}", "Lỗi tập tin");
                }
                catch (Exception ex)
                {
                    Common.ThongBaoDialog.Loi($"Đã xảy ra lỗi trong quá trình xuất Excel: {ex.Message}", "Lỗi hệ thống");
                }
            }
        }
        #endregion
    }
}
