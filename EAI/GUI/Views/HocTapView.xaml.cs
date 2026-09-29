using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BUS;
using DTO;

namespace GUI.Views
{
    public partial class HocTapView : Page
    {
        private const string SearchPlaceholder = "Tìm bài học, chủ đề, từ vựng...";
        private List<BaiHocDTO> _danhSachBaiHoc = new List<BaiHocDTO>();
        private string _selectedSkill = "All";
        private BaiHocDTO? _currentBaiHoc;
        private bool _isInitialized = false;

        public HocTapView(int? maBaiHocMoNgay = null)
        {
            InitializeComponent();
            LoadData();
            _isInitialized = true;
            ApplyFilter();

            if (maBaiHocMoNgay.HasValue)
            {
                HienThiChiTietBaiHoc(maBaiHocMoNgay.Value);
            }
        }

        private Brush GetBrush(string key, Brush fallback)
        {
            try
            {
                var res = TryFindResource(key);
                if (res is Brush b) return b;
            }
            catch { }
            return fallback;
        }

        private void LoadData()
        {
            try
            {
                _danhSachBaiHoc = BaiHocBUS.LayDanhSachBaiHoc();

                if (cboTrinhDo != null && cboTrinhDo.SelectedIndex < 0)
                {
                    cboTrinhDo.SelectedIndex = 0;
                }

                if (cboTrangThai != null && cboTrangThai.SelectedIndex < 0)
                {
                    cboTrangThai.SelectedIndex = 0;
                }

                if (txtTimKiem != null)
                {
                    txtTimKiem.Text = SearchPlaceholder;
                    txtTimKiem.Foreground = GetBrush("TextGray", Brushes.Gray);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp dữ liệu bài học: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ApplyFilter()
        {
            if (!_isInitialized || icDanhSachBaiHoc == null || txtTongSoBai == null || txtSoBaiHoanThanh == null)
                return;

            try
            {
                var query = _danhSachBaiHoc.AsEnumerable();

                // Lọc theo Skill
                if (!string.IsNullOrEmpty(_selectedSkill) && _selectedSkill != "All")
                {
                    query = query.Where(b => b.LoaiBaiHoc.Equals(_selectedSkill, StringComparison.OrdinalIgnoreCase));
                }

                // Lọc theo Trình độ
                if (cboTrinhDo != null && cboTrinhDo.SelectedItem is ComboBoxItem itemTrinhDo)
                {
                    string trinhDo = itemTrinhDo.Content?.ToString() ?? "Tất cả";
                    if (trinhDo != "Tất cả")
                    {
                        query = query.Where(b => b.MaTrinhDo.Equals(trinhDo, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // Lọc theo Trạng thái
                if (cboTrangThai != null && cboTrangThai.SelectedItem is ComboBoxItem itemTrangThai)
                {
                    string trangThai = itemTrangThai.Content?.ToString() ?? "Tất cả";
                    if (trangThai != "Tất cả")
                    {
                        query = query.Where(b => b.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // Lọc theo từ khóa tìm kiếm
                if (txtTimKiem != null)
                {
                    string keyword = txtTimKiem.Text.Trim();
                    if (!string.IsNullOrEmpty(keyword) && keyword != SearchPlaceholder)
                    {
                        query = query.Where(b => b.TieuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                 b.TenChuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                 b.TomTat.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                    }
                }

                var filteredList = query.ToList();
                icDanhSachBaiHoc.ItemsSource = filteredList;

                // Cập nhật số liệu thống kê
                int tong = _danhSachBaiHoc.Count;
                int hoanThanh = _danhSachBaiHoc.Count(b => b.TrangThai == "Hoàn thành");
                txtTongSoBai.Text = tong.ToString();
                txtSoBaiHoanThanh.Text = $"{hoanThanh} ({(tong > 0 ? hoanThanh * 100 / tong : 0)}%)";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ApplyFilter Error: " + ex.Message);
            }
        }

        private void FilterSkill_Click(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized) return;

            if (sender is Button btn && btn.Tag is string tag)
            {
                _selectedSkill = tag;

                var textGray = GetBrush("TextGray", Brushes.Gray);
                var borderLight = GetBrush("BorderLight", Brushes.LightGray);
                var tealLight = GetBrush("BrandTealLight", new SolidColorBrush(Color.FromRgb(230, 247, 243)));
                var teal = GetBrush("BrandTeal", new SolidColorBrush(Color.FromRgb(15, 118, 110)));

                if (btn.Parent is Panel panel)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is Button b)
                        {
                            b.Background = Brushes.White;
                            b.Foreground = textGray;
                            b.FontWeight = FontWeights.Medium;
                            b.BorderBrush = borderLight;
                        }
                    }
                }

                btn.Background = tealLight;
                btn.Foreground = teal;
                btn.FontWeight = FontWeights.SemiBold;
                btn.BorderBrush = teal;

                ApplyFilter();
            }
        }

        private void CboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApplyFilter();
        }

        private void TxtTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isInitialized) return;
            ApplyFilter();
        }

        private void TxtTimKiem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiem.Text == SearchPlaceholder)
            {
                txtTimKiem.Text = "";
                txtTimKiem.Foreground = GetBrush("TextDark", Brushes.Black);
            }
        }

        private void TxtTimKiem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = SearchPlaceholder;
                txtTimKiem.Foreground = GetBrush("TextGray", Brushes.Gray);
            }
        }

        private void BtnVaoHoc_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int maBaiHoc)
            {
                HienThiChiTietBaiHoc(maBaiHoc);
            }
        }

        private void HienThiChiTietBaiHoc(int maBaiHoc)
        {
            _currentBaiHoc = BaiHocBUS.LayChiTietBaiHoc(maBaiHoc);
            if (_currentBaiHoc == null) return;

            // Update info
            txtChiTietTieuDe.Text = _currentBaiHoc.TieuDe;
            txtChiTietTomTat.Text = _currentBaiHoc.TomTat;
            txtChiTietLoai.Text = _currentBaiHoc.TenLoaiHienThi;
            txtChiTietTrinhDo.Text = $"Trình độ {_currentBaiHoc.MaTrinhDo}";
            txtChiTietChuDe.Text = $"Chủ đề: {_currentBaiHoc.TenChuDe}";
            txtChiTietTienDo.Text = $"{_currentBaiHoc.PhanTramTienDo:0}%";
            txtChiTietTrangThai.Text = _currentBaiHoc.TrangThai;

            // Format Theory
            txtChiTietNoiDung.Text = _currentBaiHoc.NoiDung;

            // Build Questions UI
            TaoGiaoDienCauHoi(_currentBaiHoc);

            // Default to Theory Tab
            SwitchToTab(isLyThuyet: true);

            // Toggle views
            pnlDanhSachBaiHoc.Visibility = Visibility.Collapsed;
            pnlChiTietBaiHoc.Visibility = Visibility.Visible;
        }

        private void TaoGiaoDienCauHoi(BaiHocDTO baiHoc)
        {
            pnlDanhSachCauHoi.Children.Clear();

            if (baiHoc.DanhSachCauHoi == null || baiHoc.DanhSachCauHoi.Count == 0)
            {
                var emptyNotice = new TextBlock
                {
                    Text = "Chưa có câu hỏi trắc nghiệm cho bài học này. Bạn có thể nhấn 'Hoàn thành bài học' để ghi nhận tiến độ.",
                    Foreground = Brushes.Gray,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 10, 0, 10)
                };
                pnlDanhSachCauHoi.Children.Add(emptyNotice);
                return;
            }

            for (int i = 0; i < baiHoc.DanhSachCauHoi.Count; i++)
            {
                var ch = baiHoc.DanhSachCauHoi[i];

                var cardBorder = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(248, 250, 252)),
                    BorderBrush = GetBrush("BorderLight", Brushes.LightGray),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(16),
                    Margin = new Thickness(0, 0, 0, 16)
                };

                var spCard = new StackPanel();

                // Question Title
                var txtQ = new TextBlock
                {
                    Text = $"Câu {i + 1}: {ch.NoiDungCauHoi}",
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 12)
                };
                spCard.Children.Add(txtQ);

                // Explanation Box (Hidden by default)
                var bdrGiaiThich = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(240, 253, 250)),
                    BorderBrush = GetBrush("BrandMintSoft", new SolidColorBrush(Color.FromRgb(204, 251, 241))),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(12, 10, 12, 10),
                    Margin = new Thickness(0, 10, 0, 0),
                    Visibility = Visibility.Collapsed
                };
                var txtGiaiThich = new TextBlock
                {
                    Text = $"💡 Giải thích: {ch.GiaiThichChiTiet}",
                    Foreground = GetBrush("BrandTealDark", new SolidColorBrush(Color.FromRgb(4, 47, 46))),
                    FontSize = 12.5,
                    TextWrapping = TextWrapping.Wrap
                };
                bdrGiaiThich.Child = txtGiaiThich;

                // Options list
                for (int optIdx = 0; optIdx < ch.DanhSachLuaChon.Count; optIdx++)
                {
                    string optionText = ch.DanhSachLuaChon[optIdx];
                    char optionLetter = (char)('A' + optIdx);
                    int currentOpt = optIdx;

                    var rb = new RadioButton
                    {
                        GroupName = $"Q_{ch.MaCauHoi}",
                        Content = $"{optionLetter}. {optionText}",
                        FontSize = 13,
                        Margin = new Thickness(4, 4, 4, 8),
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    rb.Checked += (s, ev) =>
                    {
                        ch.LuaChonCuaNguoiDung = currentOpt;
                        bool isCorrect = (currentOpt == ch.DapAnDungIndex);
                        ch.DaTraLoiDung = isCorrect;

                        bdrGiaiThich.Visibility = Visibility.Visible;
                        if (isCorrect)
                        {
                            txtGiaiThich.Text = $"✅ Chính xác! {ch.GiaiThichChiTiet}";
                            bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(236, 253, 245));
                        }
                        else
                        {
                            char correctLetter = (char)('A' + ch.DapAnDungIndex);
                            txtGiaiThich.Text = $"❌ Chưa đúng (Đáp án đúng là {correctLetter}). {ch.GiaiThichChiTiet}";
                            bdrGiaiThich.Background = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                        }
                    };

                    spCard.Children.Add(rb);
                }

                spCard.Children.Add(bdrGiaiThich);
                cardBorder.Child = spCard;
                pnlDanhSachCauHoi.Children.Add(cardBorder);
            }
        }

        private void BtnQuayLai_Click(object sender, RoutedEventArgs e)
        {
            pnlChiTietBaiHoc.Visibility = Visibility.Collapsed;
            pnlDanhSachBaiHoc.Visibility = Visibility.Visible;
            ApplyFilter();
        }

        private void TabLyThuyet_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: true);
        }

        private void TabBaiTap_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: false);
        }

        private void BtnChuyenSangBaiTap_Click(object sender, RoutedEventArgs e)
        {
            SwitchToTab(isLyThuyet: false);
        }

        private void SwitchToTab(bool isLyThuyet)
        {
            var textDark = GetBrush("TextDark", Brushes.Black);
            var textGray = GetBrush("TextGray", Brushes.Gray);

            if (isLyThuyet)
            {
                pnlTabLyThuyet.Visibility = Visibility.Visible;
                pnlTabBaiTap.Visibility = Visibility.Collapsed;

                btnTabLyThuyet.Background = Brushes.White;
                btnTabLyThuyet.Foreground = textDark;
                btnTabLyThuyet.FontWeight = FontWeights.SemiBold;

                btnTabBaiTap.Background = Brushes.Transparent;
                btnTabBaiTap.Foreground = textGray;
                btnTabBaiTap.FontWeight = FontWeights.Medium;
            }
            else
            {
                pnlTabLyThuyet.Visibility = Visibility.Collapsed;
                pnlTabBaiTap.Visibility = Visibility.Visible;

                btnTabBaiTap.Background = Brushes.White;
                btnTabBaiTap.Foreground = textDark;
                btnTabBaiTap.FontWeight = FontWeights.SemiBold;

                btnTabLyThuyet.Background = Brushes.Transparent;
                btnTabLyThuyet.Foreground = textGray;
                btnTabLyThuyet.FontWeight = FontWeights.Medium;
            }
        }

        private void BtnPlayAudio_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đang phát audio giọng đọc người bản xứ mẫu chuẩn phát âm IPA...", "EnglishAI Audio Player", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAskAI_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBaiHoc == null) return;
            MessageBox.Show($"Trợ lý AI sẵn sàng giải thích chuyên sâu cho bài học '{_currentBaiHoc.TieuDe}'. Bạn có thể đặt câu hỏi về ngữ pháp, từ vựng hoặc ví dụ mở rộng bất kỳ lúc nào!", "EnglishAI Tutor Assistant", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnHoanThanhBaiHoc_Click(object sender, RoutedEventArgs e)
        {
            if (_currentBaiHoc == null) return;

            BaiHocBUS.CapNhatTienDo(_currentBaiHoc.MaBaiHoc, 100, "Hoàn thành", "10/10");

            MessageBox.Show($"🎉 Chúc mừng bạn đã hoàn thành bài học '{_currentBaiHoc.TieuDe}'!\nTiến độ và điểm số đã được tự động cập nhật vào hồ sơ học tập.", 
                            "Hoàn thành bài học", MessageBoxButton.OK, MessageBoxImage.Information);

            // Quay lại danh sách bài học và làm mới
            BtnQuayLai_Click(sender, e);
        }

        private void NavTrangChu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new TrangChuView());
        }

        private void NavLuyenTap_Click(object sender, RoutedEventArgs e)
        {
            _selectedSkill = "NguPhap";
            ApplyFilter();
            pnlChiTietBaiHoc.Visibility = Visibility.Collapsed;
            pnlDanhSachBaiHoc.Visibility = Visibility.Visible;
        }

        private void NavAIChat_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new TroLyAIView());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var dangNhapWindow = new DangNhapWindow();
            dangNhapWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
