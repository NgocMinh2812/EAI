using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using BUS;
using DTO;
using LiveCharts;
using LiveCharts.Wpf;

namespace GUI.Views
{
    public partial class TienDoHocView : Page
    {
        private bool _isInitialized = false;
        private string _currentFilter = "TatCa";
        private TongKetTienDoDTO? _currentTongKet;
        private List<DiemThiTheoThoiGianDTO> _currentLichSu = new();
        private List<GioHocTheoNgayDTO> _currentGioHoc = new();

        public TienDoHocView()
        {
            InitializeComponent();
            _isInitialized = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Filter_Checked(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized) return;

            if (rbFilterTuan == null || rbFilterThang == null || rbFilterTatCa == null)
                return;

            if (rbFilterTuan.IsChecked == true)
                _currentFilter = "TuanNay";
            else if (rbFilterThang.IsChecked == true)
                _currentFilter = "ThangNay";
            else
                _currentFilter = "TatCa";

            LoadData();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (!_isInitialized) return;

            try
            {
                int userId = SessionHelper.CurrentMaNguoiDung;
                if (userId <= 0)
                {
                    _currentTongKet = new TongKetTienDoDTO();
                    _currentLichSu = new List<DiemThiTheoThoiGianDTO>();
                    _currentGioHoc = new List<GioHocTheoNgayDTO>();
                    HienThiKpi();
                    CapNhatBieuDoDiemSo();
                    CapNhatBieuDoTyLe();
                    CapNhatBieuDoThoiGianHoc();
                    HienThiBangLichSu();
                    return;
                }

                // 1. Lấy dữ liệu từ BUS
                _currentTongKet = TienDoHocBUS.Instance.LayTongKetTienDo(userId, _currentFilter);
                _currentLichSu = TienDoHocBUS.Instance.LayLichSuDiemThi(userId, _currentFilter);
                _currentGioHoc = TienDoHocBUS.Instance.LayThongKeGioHoc(userId, _currentFilter);

                // 2. Cập nhật 4 Card KPI
                HienThiKpi();

                // 3. Cập nhật Biểu đồ đường (LineChart): Điểm số qua từng lần thi
                CapNhatBieuDoDiemSo();

                // 4. Cập nhật Biểu đồ tròn (PieChart): Tỷ lệ Đạt & Chưa đạt
                CapNhatBieuDoTyLe();

                // 5. Cập nhật Biểu đồ cột (ColumnChart): Thời gian học theo ngày
                CapNhatBieuDoThoiGianHoc();

                // 6. Cập nhật Bảng lịch sử các lần thi
                HienThiBangLichSu();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocView.LoadData] Lỗi nạp dữ liệu: {ex.Message}");
            }
        }

        private void HienThiKpi()
        {
            if (_currentTongKet == null || txtTongGioHoc == null || txtTongBaiThi == null || txtDiemTB == null || txtPhanTramMucTieu == null) return;

            // Card 1: Tổng thời gian học
            txtTongGioHoc.Text = _currentTongKet.TongGioHocText;
            txtSubGioHoc.Text = _currentFilter switch
            {
                "TuanNay" => "Tích lũy trong 7 ngày qua",
                "ThangNay" => "Tích lũy trong 30 ngày qua",
                _ => "Tích lũy trên toàn hệ thống"
            };

            // Card 2: Tổng bài thi hoàn thành
            txtTongBaiThi.Text = $"{_currentTongKet.TongSoBaiThi} bài";
            txtBadgeTyLeDat.Text = _currentTongKet.TongSoBaiThi > 0 ? $"{_currentTongKet.TyLeDat:0.0}% Đạt" : "Chưa có";
            txtSubBaiThi.Text = _currentTongKet.TongSoBaiThi > 0 
                ? $"Đạt {_currentTongKet.SoBaiDat}/{_currentTongKet.TongSoBaiThi} bài" 
                : "Chưa hoàn thành bài nào";

            // Card 3: Điểm trung bình các lần thi
            txtDiemTB.Text = _currentTongKet.DiemTrungBinhText;
            if (_currentTongKet.TongSoBaiThi > 0)
            {
                txtXepLoaiBadge.Text = _currentTongKet.DiemTrungBinh >= 8.0 ? "Xuất sắc"
                                    : _currentTongKet.DiemTrungBinh >= 6.5 ? "Khá - Tốt"
                                    : _currentTongKet.DiemTrungBinh >= 5.0 ? "Đạt chuẩn"
                                    : "Cần cải thiện";
                txtSubDiemTB.Text = $"Tương đương CEFR {LayTrinhDoTheoDiem(_currentTongKet.DiemTrungBinh)}";
            }
            else
            {
                txtXepLoaiBadge.Text = "Chưa có";
                txtSubDiemTB.Text = "Điểm trung bình các bài thi";
            }

            // Card 4: Tiến độ hoàn thành mục tiêu học tập
            txtPhanTramMucTieu.Text = _currentTongKet.PhanTramMucTieuText;
            txtMucTieuTitle.Text = _currentTongKet.MucTieuText;
            pbTienDoMucTieu.Value = _currentTongKet.PhanTramMucTieu;
            txtMoTaMucTieu.Text = _currentTongKet.MoTaTienDoMucTieu;
        }

        private void CapNhatBieuDoDiemSo()
        {
            if (chartDiemSo == null || pnlEmptyLineChart == null) return;

            if (_currentLichSu.Count == 0)
            {
                chartDiemSo.Visibility = Visibility.Hidden;
                pnlEmptyLineChart.Visibility = Visibility.Visible;
                return;
            }

            chartDiemSo.Visibility = Visibility.Visible;
            pnlEmptyLineChart.Visibility = Visibility.Collapsed;

            var values = new ChartValues<double>(_currentLichSu.Select(x => (double)x.DiemSo));

            var lineSeries = new LineSeries
            {
                Title = "Điểm số",
                Values = values,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F766E")),
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#150F766E")),
                PointGeometrySize = 10,
                PointForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F766E")),
                LineSmoothness = 0.35,
                StrokeThickness = 2.8,
                LabelPoint = point => $"{point.Y:0.0} điểm"
            };

            chartDiemSo.Series = new SeriesCollection { lineSeries };
            axisXDiemSo.Labels = _currentLichSu.Select(x => $"{x.LanThiLabel} ({x.NgayNopNgan})").ToList();
        }

        private void CapNhatBieuDoTyLe()
        {
            if (chartTyLe == null || pnlEmptyPieChart == null || txtSoBaiDat == null || txtSoBaiChuaDat == null) return;

            if (_currentTongKet == null || _currentTongKet.TongSoBaiThi == 0)
            {
                chartTyLe.Visibility = Visibility.Hidden;
                pnlEmptyPieChart.Visibility = Visibility.Visible;
                txtSoBaiDat.Text = "0 bài (0%)";
                txtSoBaiChuaDat.Text = "0 bài (0%)";
                return;
            }

            chartTyLe.Visibility = Visibility.Visible;
            pnlEmptyPieChart.Visibility = Visibility.Collapsed;

            var seriesList = new SeriesCollection();

            if (_currentTongKet.SoBaiDat > 0)
            {
                seriesList.Add(new PieSeries
                {
                    Title = "Đạt (Pass)",
                    Values = new ChartValues<int> { _currentTongKet.SoBaiDat },
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981")),
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y} bài ({chartPoint.Participation:P0})"
                });
            }

            if (_currentTongKet.SoBaiChuaDat > 0)
            {
                seriesList.Add(new PieSeries
                {
                    Title = "Chưa đạt",
                    Values = new ChartValues<int> { _currentTongKet.SoBaiChuaDat },
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444")),
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y} bài ({chartPoint.Participation:P0})"
                });
            }

            chartTyLe.Series = seriesList;
            txtSoBaiDat.Text = $"{_currentTongKet.SoBaiDat} bài ({_currentTongKet.TyLeDat:0.0}%)";
            txtSoBaiChuaDat.Text = $"{_currentTongKet.SoBaiChuaDat} bài ({_currentTongKet.TyLeChuaDat:0.0}%)";
        }

        private void CapNhatBieuDoThoiGianHoc()
        {
            if (chartThoiGian == null || axisXThoiGian == null || txtTongPhutGiaiDoan == null) return;

            var values = new ChartValues<int>(_currentGioHoc.Select(x => x.SoPhutHoc));

            var colSeries = new ColumnSeries
            {
                Title = "Thời gian học (phút)",
                Values = values,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F766E")),
                MaxColumnWidth = 32,
                DataLabels = true,
                LabelsPosition = BarLabelPosition.Top,
                LabelPoint = point => point.Y > 0 ? $"{point.Y}p" : ""
            };

            chartThoiGian.Series = new SeriesCollection { colSeries };
            axisXThoiGian.Labels = _currentGioHoc.Select(x => x.NhanNgay).ToList();

            int tongPhut = _currentGioHoc.Sum(x => x.SoPhutHoc);
            txtTongPhutGiaiDoan.Text = tongPhut >= 60 ? $"{tongPhut / 60}h {tongPhut % 60}m" : $"{tongPhut} phút";
        }

        private void HienThiBangLichSu()
        {
            if (icLichSuThi == null || txtTongSoLanLam == null || pnlEmptyState == null) return;
            icLichSuThi.ItemsSource = _currentLichSu;
            txtTongSoLanLam.Text = $"Tổng cộng: {_currentLichSu.Count} lượt làm";

            if (_currentLichSu.Count == 0)
            {
                pnlEmptyState.Visibility = Visibility.Visible;
                icLichSuThi.Visibility = Visibility.Collapsed;
            }
            else
            {
                pnlEmptyState.Visibility = Visibility.Collapsed;
                icLichSuThi.Visibility = Visibility.Visible;
            }
        }

        private static string LayTrinhDoTheoDiem(double diem)
        {
            if (diem >= 8.5) return "C1 - C2 Advanced";
            if (diem >= 7.0) return "B2 - C1 Upper-Int";
            if (diem >= 5.5) return "B1 Intermediate";
            if (diem >= 4.0) return "A2 Pre-Int";
            return "A1 Elementary";
        }

        private void BtnExportReport_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTongKet == null) return;

            try
            {
                string reportContent = TienDoHocBUS.Instance.TaoBaoCaoVanBan(_currentTongKet, _currentLichSu);

                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string fileName = $"EnglishAI_BaoCaoTienDo_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string fullPath = Path.Combine(downloadsPath, fileName);

                File.WriteAllText(fullPath, reportContent, System.Text.Encoding.UTF8);

                Common.ThongBaoDialog.ThanhCong(
                    $"📊 Báo cáo tiến độ học tập chi tiết đã được xuất thành công!\n\n📁 Đường dẫn file:\n{fullPath}",
                    "Xuất báo cáo thành công");
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.CanhBao($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi xuất file");
            }
        }

        private void BtnGoToThiThu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Window.GetWindow(this) is MainWindow mw)
                {
                    mw.SetActiveMenu("ThiThu");
                    mw.MainFrame.Navigate(new ThiThuView());
                }
                else
                {
                    NavigationService?.Navigate(new ThiThuView());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocView] Chuyển trang Thi thử lỗi: {ex.Message}");
            }
        }

        private void BtnGoToLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Window.GetWindow(this) is MainWindow mw)
                {
                    mw.SetActiveMenu("LuyenTap");
                    mw.MainFrame.Navigate(new LuyenTapView());
                }
                else
                {
                    NavigationService?.Navigate(new LuyenTapView());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TienDoHocView] Chuyển trang Luyện tập lỗi: {ex.Message}");
            }
        }
    }
}
