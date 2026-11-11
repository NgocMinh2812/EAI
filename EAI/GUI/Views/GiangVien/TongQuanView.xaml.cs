using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using BUS;
using DTO;
using GUI.Services;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class TongQuanView : Page
    {
        private readonly DashboardBUS _dashboardBUS = new DashboardBUS();
        private readonly TuVungBUS _tuVungBUS = TuVungBUS.Instance;
        private DashboardKpiDTO? _lastKpi;
        private DispatcherTimer? _liveFeedTimer;
        private DateTime _lastAudioClick = DateTime.MinValue;
        private int _filterIndex = 0; // 0: Hôm nay, 1: 7 ngày qua, 2: Tháng này

        public TongQuanView()
        {
            InitializeComponent();
            this.Loaded += TongQuanView_Loaded;
            this.Unloaded += TongQuanView_Unloaded;
        }

        private async void TongQuanView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRealKpisAsync(_filterIndex);
            LoadTuVungHomNay();

            // Khởi tạo timer làm mới số liệu tự động (30 giây)
            if (_liveFeedTimer == null)
            {
                _liveFeedTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(30)
                };
                _liveFeedTimer.Tick += async (s, ev) =>
                {
                    if (this.IsLoaded)
                    {
                        await LoadRealKpisAsync(_filterIndex);
                    }
                };
            }
            _liveFeedTimer.Start();
        }

        private void TongQuanView_Unloaded(object sender, RoutedEventArgs e)
        {
            _liveFeedTimer?.Stop();
        }

        private void LoadTuVungHomNay()
        {
            try
            {
                var tv = _tuVungBUS.LayTuVungHomNay();
                if (tv != null)
                {
                    txtTuVungWord.Text = tv.Tu;
                    txtTuVungPhienAm.Text = !string.IsNullOrWhiteSpace(tv.PhienAmIPA) ? tv.PhienAmIPA : "";
                    txtTuVungLoaiTu.Text = !string.IsNullOrWhiteSpace(tv.LoaiTu) ? tv.LoaiTu : "từ vựng";
                    txtTuVungNghia.Text = tv.Nghia;
                    txtTuVungCauViDu.Text = !string.IsNullOrWhiteSpace(tv.CauViDu) ? $"\"{tv.CauViDu}\"" : "\"Practice makes perfect.\"";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TongQuanView] Lỗi tải từ vựng hôm nay: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task LoadRealKpisAsync(int filterPeriod = 0)
        {
            try
            {
                var kpi = await System.Threading.Tasks.Task.Run(() => _dashboardBUS.LaySoLieuTongQuan(filterPeriod));
                _lastKpi = kpi;

                txtKpiHocVien.Text = kpi.TongHocVien.ToString("N0");
                txtKpiBaiHoc.Text = kpi.TongBaiHoc.ToString("N0");
                txtKpiCauHoi.Text = kpi.TongCauHoi.ToString("N0");
                txtKpiLuotThi.Text = kpi.TongLuotThi.ToString("N0");

                var greenBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#16A34A")!;
                var redBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#DC2626")!;

                void SetTrendBlock(TextBlock tb, double pct, int countNew)
                {
                    tb.Text = kpi.FormatTrend(pct, countNew);
                    tb.ToolTip = tb.Text.Replace("\n", " ");
                    tb.Foreground = pct < 0 ? redBrush : greenBrush;
                }

                SetTrendBlock(txtTrendHocVien, kpi.TangTruongHocVien, kpi.HocVienMoi);
                SetTrendBlock(txtTrendBaiHoc, kpi.TangTruongBaiHoc, kpi.BaiHocMoi);
                SetTrendBlock(txtTrendCauHoi, kpi.TangTruongCauHoi, kpi.CauHoiMoi);
                SetTrendBlock(txtTrendLuotThi, kpi.TangTruongLuotThi, kpi.LuotThiMoi);

                txtKpiDeThi.Text = kpi.TongDeThi.ToString("N0");
                txtKpiLuyenTap.Text = kpi.TongLuyenTap.ToString("N0");
                txtKpiChoDuyet.Text = kpi.ChoDuyet.ToString("N0");

                // Biểu đồ xu hướng tuần qua
                if (kpi.XuHuongHocTap != null && kpi.XuHuongHocTap.Count > 0)
                {
                    icBieuDoXuHuong.ItemsSource = kpi.XuHuongHocTap;
                    int tongTuan = 0;
                    foreach (var p in kpi.XuHuongHocTap) tongTuan += p.SoLuotLamBai;
                    txtTongLuotTuanQua.Text = $"Tổng: {tongTuan:N0} lượt";
                }

                // Danh sách nộp bài trực tiếp từ database (Live feed)
                if (kpi.DanhSachHoatDong != null)
                {
                    icVuaNopBai.ItemsSource = kpi.DanhSachHoatDong;
                }

                // Thống kê nhóm học theo trình độ thực tế từ CSDL
                txtNhomA1Count.Text = kpi.NhomA1Count.ToString();
                txtNhomB1Count.Text = kpi.NhomB1Count.ToString();
                txtNhomB2Count.Text = kpi.NhomB2Count.ToString();
                txtNhomC1Count.Text = kpi.NhomC1Count.ToString();

                // Bài học 1 từ CSDL
                if (kpi.BaiHoc1 != null)
                {
                    txtLoaiBaiHoc1.Text = kpi.BaiHoc1.LoaiHinh;
                    txtTrinhDoBaiHoc1.Text = kpi.BaiHoc1.TrinhDo;
                    txtMaCodeBaiHoc1.Text = kpi.BaiHoc1.MaCode;
                    txtTieuDeBaiHoc1.Text = kpi.BaiHoc1.TieuDe;
                    if (!string.IsNullOrWhiteSpace(kpi.BaiHoc1.TomTat))
                        txtTomTatBaiHoc1.Text = kpi.BaiHoc1.TomTat;
                    txtTrangThaiBaiHoc1.Text = kpi.BaiHoc1.TrangThai;
                }

                // Đề thi nổi bật từ CSDL
                if (kpi.DeThi1 != null)
                {
                    txtLoaiDeThi.Text = kpi.DeThi1.LoaiHinh;
                    txtTrinhDoDeThi.Text = kpi.DeThi1.TrinhDo;
                    txtMaCodeDeThi.Text = kpi.DeThi1.MaCode;
                    txtTieuDeDeThi.Text = kpi.DeThi1.TieuDe;
                    if (!string.IsNullOrWhiteSpace(kpi.DeThi1.TomTat))
                        txtMoTaDeThi.Text = kpi.DeThi1.TomTat;
                    txtTrangThaiDeThi.Text = kpi.DeThi1.TrangThai;
                }

                // Bài học 2 từ CSDL
                if (kpi.BaiHoc2 != null)
                {
                    txtLoaiBaiHoc2.Text = kpi.BaiHoc2.LoaiHinh;
                    txtTrinhDoBaiHoc2.Text = kpi.BaiHoc2.TrinhDo;
                    txtMaCodeBaiHoc2.Text = kpi.BaiHoc2.MaCode;
                    txtTieuDeBaiHoc2.Text = kpi.BaiHoc2.TieuDe;
                    if (!string.IsNullOrWhiteSpace(kpi.BaiHoc2.TomTat))
                        txtTomTatBaiHoc2.Text = kpi.BaiHoc2.TomTat;
                    txtTrangThaiBaiHoc2.Text = kpi.BaiHoc2.TrangThai;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TongQuanView] Lỗi tải số liệu KPI: {ex.Message}");
            }
        }

        private void BtnPhatAmTuVungHomNay_Click(object sender, RoutedEventArgs e)
        {
            if ((DateTime.Now - _lastAudioClick).TotalSeconds < 1.2) return;
            _lastAudioClick = DateTime.Now;

            string word = txtTuVungWord.Text.Trim();
            if (!string.IsNullOrWhiteSpace(word))
            {
                _ = AIPronunciationService.Instance.PlayPronunciationAsync(word);
            }
        }

        private void BtnTaoFlashcard_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new TaoFlashcardDialog(
                prefillWord: txtTuVungWord.Text.Trim(),
                prefillPhienAm: txtTuVungPhienAm.Text.Trim(),
                prefillNghia: txtTuVungNghia.Text.Trim(),
                prefillViDu: txtTuVungCauViDu.Text.Trim('"', ' '),
                openStudyTab: false);

            ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));

            if (dlg.HasNewFlashcard)
            {
                LoadTuVungHomNay();
            }
        }

        private void BtnXemTatCaTuVung_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dlg = new TaoFlashcardDialog(openStudyTab: true);
            ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));

            LoadTuVungHomNay();
        }

        private void BtnTaiNguyenGrammar_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new NoiDungHocTapView(kyNang: "Ngữ pháp"));
        }

        private void BtnTaiNguyenIelts_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new DeThiLuyenTapView(searchKeyword: "IELTS"));
        }

        private void BtnTaiNguyenToeic_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new DeThiLuyenTapView(searchKeyword: "TOEIC"));
        }

        private void BtnTaiNguyenIpa_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new NoiDungHocTapView(kyNang: "Phát âm"));
        }

        private void TxtSearchNoiDung_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchNoiDung.Text == "Tìm kiếm bài học, đề thi...")
            {
                txtSearchNoiDung.Text = "";
                txtSearchNoiDung.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(15, 23, 42));
            }
        }

        private void TxtSearchNoiDung_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchNoiDung.Text))
            {
                txtSearchNoiDung.Text = "Tìm kiếm bài học, đề thi...";
                txtSearchNoiDung.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 116, 139));
            }
        }

        private void TxtSearchNoiDung_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string kw = txtSearchNoiDung.Text.Trim();
                if (!string.IsNullOrEmpty(kw) && kw != "Tìm kiếm bài học, đề thi...")
                {
                    this.NavigationService?.Navigate(new NoiDungHocTapView(searchKeyword: kw));
                }
            }
        }

        private void BtnXemTatCaNoiDung_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new NoiDungHocTapView());
        }

        private void BtnChinhSuaBaiHoc1_Click(object sender, RoutedEventArgs e)
        {
            int maBaiHoc = _lastKpi?.BaiHoc1?.MaId ?? 0;
            this.NavigationService?.Navigate(new NoiDungHocTapView(targetLessonId: maBaiHoc));
        }

        private void BtnChinhSuaDeThi_Click(object sender, RoutedEventArgs e)
        {
            int maDeThi = _lastKpi?.DeThi1?.MaId ?? 0;
            this.NavigationService?.Navigate(new DeThiLuyenTapView(targetExamId: maDeThi));
        }

        private void BtnPlayDeThi_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int maDeThi = _lastKpi?.DeThi1?.MaId ?? 0;
            this.NavigationService?.Navigate(new DeThiLuyenTapView(targetExamId: maDeThi));
        }

        private void BtnChinhSuaBaiHoc2_Click(object sender, RoutedEventArgs e)
        {
            int maBaiHoc = _lastKpi?.BaiHoc2?.MaId ?? 0;
            this.NavigationService?.Navigate(new NoiDungHocTapView(targetLessonId: maBaiHoc));
        }

        private async void BtnLocThoiGian_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _filterIndex = (_filterIndex + 1) % 3;
            var txt = sender as TextBlock;
            if (txt != null)
            {
                txt.Text = _filterIndex switch
                {
                    1 => "7 ngày qua ▼",
                    2 => "Tháng này ▼",
                    _ => "Hôm nay ▼"
                };
            }
            await LoadRealKpisAsync(_filterIndex);
        }

        private void ItemVuaNopBai_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is HoatDongGanDayDTO hd && !string.IsNullOrWhiteSpace(hd.HoTen))
            {
                bool isSurvey = hd.LoaiHoatDong == "KhaoSat"
                             || (!string.IsNullOrWhiteSpace(hd.TieuDe) && hd.TieuDe.IndexOf("Khảo sát", StringComparison.OrdinalIgnoreCase) >= 0)
                             || (!string.IsNullOrWhiteSpace(hd.NoiDung) && hd.NoiDung.IndexOf("Khảo sát", StringComparison.OrdinalIgnoreCase) >= 0);

                if (isSurvey)
                {
                    this.NavigationService?.Navigate(new KhaoSatDauVaoView(searchKeyword: hd.HoTen));
                }
                else
                {
                    this.NavigationService?.Navigate(new DeThiLuyenTapView(searchKeyword: hd.HoTen));
                }
            }
            else
            {
                this.NavigationService?.Navigate(new DeThiLuyenTapView());
            }
        }

        private void BtnNhomHoc_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is string trinhDo && !string.IsNullOrWhiteSpace(trinhDo))
            {
                // Điều hướng sang Khảo sát đầu vào đã chọn sẵn cấp độ nhóm học tương ứng (A1, B1, B2, C1)
                this.NavigationService?.Navigate(new KhaoSatDauVaoView(filterLevel: trinhDo));
            }
            else
            {
                this.NavigationService?.Navigate(new KhaoSatDauVaoView());
            }
        }

        private void TxtKpiChoDuyet_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.NavigationService?.Navigate(new AISinhDeView());
        }

        private void BtnPhanTich_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new ThongKeHocTapGiangVienView());
        }

        private void BtnQuickTaoCauHoi_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new NganHangCauHoiView());
        }

        private void BtnQuickTaoDeThi_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeThiLuyenTapView());
        }

        private void BtnQuickAISinh_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new AISinhDeView());
        }

        private void BtnQuickGiamSat_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new GiamSatHeThongView());
        }
    }
}
