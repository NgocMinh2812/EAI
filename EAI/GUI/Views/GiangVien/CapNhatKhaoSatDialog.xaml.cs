using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BUS;
using DTO;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class CapNhatKhaoSatDialog : Window
    {
        private int _maBaiKhaoSat = 1;
        private readonly ObservableCollection<CauHoiKhaoSatDTO> _currentQuestions = new();
        private List<CauHoiKhaoSatDTO> _allBankQuestions = new();
        private List<BaiKhaoSatDTO> _allSurveys = new();
        private bool _isInternalSwitching = false;

        public CapNhatKhaoSatDialog() : this(1)
        {
        }

        public CapNhatKhaoSatDialog(int maBaiKhaoSat)
        {
            _maBaiKhaoSat = maBaiKhaoSat;
            InitializeComponent();
            Loaded += CapNhatKhaoSatDialog_Loaded;
        }

        private void CapNhatKhaoSatDialog_Loaded(object sender, RoutedEventArgs e)
        {
            NapDanhSachBoDe();
            ChuyenDoiBoDe(_maBaiKhaoSat);
        }

        private void NapDanhSachBoDe()
        {
            try
            {
                _isInternalSwitching = true;
                _allSurveys = KhaoSatBUS.Instance.LayDanhSachTatCaBaiKhaoSat();
                cboChonBoDe.Items.Clear();

                int selectedIdx = 0;
                for (int i = 0; i < _allSurveys.Count; i++)
                {
                    var s = _allSurveys[i];
                    var item = new ComboBoxItem
                    {
                        Content = $"[{s.MaCapDo}] {s.TenBaiKhaoSat}",
                        Tag = s.MaBaiKhaoSat
                    };
                    cboChonBoDe.Items.Add(item);

                    if (s.MaBaiKhaoSat == _maBaiKhaoSat)
                    {
                        selectedIdx = i;
                    }
                }

                if (cboChonBoDe.Items.Count > 0)
                {
                    cboChonBoDe.SelectedIndex = selectedIdx;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NapDanhSachBoDe] {ex.Message}");
            }
            finally
            {
                _isInternalSwitching = false;
            }
        }

        private void ChuyenDoiBoDe(int maBaiKhaoSat)
        {
            _isInternalSwitching = true;
            try
            {
                _maBaiKhaoSat = maBaiKhaoSat;
                var survey = _allSurveys.FirstOrDefault(s => s.MaBaiKhaoSat == maBaiKhaoSat) 
                             ?? KhaoSatBUS.Instance.LayBaiKhaoSatTheoMa(maBaiKhaoSat);

                if (survey != null)
                {
                    txtTenBaiKhaoSat.Text = survey.TenBaiKhaoSat;
                    txtMoTa.Text = survey.MoTa;
                    chkDangHoatDong.IsChecked = survey.DangHoatDong;
                    txtThoiGianPhut.Text = survey.ThoiGianPhut > 0 ? survey.ThoiGianPhut.ToString() : "30";

                    // Chọn cấp độ tương ứng trong cboCapDoSurvey
                    for (int i = 0; i < cboCapDoSurvey.Items.Count; i++)
                    {
                        if (cboCapDoSurvey.Items[i] is ComboBoxItem item &&
                            string.Equals(item.Tag?.ToString(), survey.MaCapDo, StringComparison.OrdinalIgnoreCase))
                        {
                            cboCapDoSurvey.SelectedIndex = i;
                            break;
                        }
                    }

                    // Đồng bộ combobox cấp độ bên tìm kiếm ngân hàng
                    for (int i = 0; i < cboBankLevel.Items.Count; i++)
                    {
                        if (cboBankLevel.Items[i] is ComboBoxItem item &&
                            string.Equals(item.Tag?.ToString(), survey.MaCapDo, StringComparison.OrdinalIgnoreCase))
                        {
                            cboBankLevel.SelectedIndex = i;
                            break;
                        }
                    }
                }

                // Nạp danh sách câu hỏi đang nằm trong bộ đề
                var questions = KhaoSatBUS.Instance.LayDanhSachCauHoiTheoBaiKhaoSat(_maBaiKhaoSat);
                _currentQuestions.Clear();
                int stt = 1;
                foreach (var q in questions)
                {
                    q.IsInCurrentSurvey = true;
                    q.ThuTuTrongBo = stt++;
                    _currentQuestions.Add(q);
                }

                // Nạp tất cả câu hỏi khả dụng từ ngân hàng
                _allBankQuestions = KhaoSatBUS.Instance.LayDanhSachCauHoiCoTheThem(_maBaiKhaoSat, "", "TatCa");
                foreach (var q in _allBankQuestions)
                {
                    q.IsInCurrentSurvey = false;
                    q.ThuTuTrongBo = 0;
                }

                CapNhatThongKeVaPhanBo();
                ApDungBoLocHienThi();
            }
            finally
            {
                _isInternalSwitching = false;
            }
        }

        private void CboChonBoDe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInternalSwitching || !IsLoaded) return;

            if (cboChonBoDe.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                int newId = Convert.ToInt32(item.Tag);
                ChuyenDoiBoDe(newId);
            }
        }

        private void CboCapDoSurvey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Có thể tự động lọc gợi ý khi đổi cấp độ
        }

        private void CapNhatThongKeVaPhanBo()
        {
            int count = _currentQuestions.Count;
            txtCountSelected.Text = $"{count} / 20 câu";
            txtSelectedFilterCount.Text = $"{count} câu";

            // Phân bổ kỹ năng
            int nGrammar = 0, nVocab = 0, nReading = 0, nListeningOther = 0;
            foreach (var q in _currentQuestions)
            {
                string skill = CauHoiKhaoSatDTO.ChuanHoaKyNang(q.KyNang);
                if (skill == "NguPhap") nGrammar++;
                else if (skill == "TuVung") nVocab++;
                else if (skill == "Doc" || skill == "DocHieu") nReading++;
                else nListeningOther++;
            }

            txtSkillNguPhap.Text = $"{nGrammar} câu";
            txtSkillTuVung.Text = $"{nVocab} câu";
            txtSkillDoc.Text = $"{nReading} câu";
            txtSkillNghe.Text = $"{nListeningOther} câu";

            if (count == 20)
            {
                badgeCountSelected.Background = new SolidColorBrush(Color.FromRgb(220, 252, 231)); // #DCFCE7
                txtCountSelected.Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61));    // #15803D
            }
            else
            {
                badgeCountSelected.Background = new SolidColorBrush(Color.FromRgb(254, 243, 199)); // #FEF3C7
                txtCountSelected.Foreground = new SolidColorBrush(Color.FromRgb(180, 83, 9));     // #B45309
            }
        }

        private void ApDungBoLocHienThi()
        {
            if (!IsLoaded) return;

            string tuKhoa = txtSearchBank?.Text.Trim() ?? "";
            txtSearchPlaceholder.Visibility = string.IsNullOrEmpty(tuKhoa) ? Visibility.Visible : Visibility.Collapsed;

            string levelFilter = (cboBankLevel.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "TatCa";
            string skillFilter = (cboBankSkill.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "TatCa";
            string diffFilter = (cboBankDifficulty.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "TatCa";
            bool onlySelected = chkOnlySelected.IsChecked == true;

            var inSurveyIds = _currentQuestions.Select(q => q.MaCauHoi).ToHashSet();

            // Tổng hợp danh sách nguồn
            List<CauHoiKhaoSatDTO> sourceList;
            if (onlySelected)
            {
                sourceList = _currentQuestions.ToList();
            }
            else
            {
                sourceList = new List<CauHoiKhaoSatDTO>();
                // Thêm các câu trong bài trước (để giảng viên dễ xem và đổi thứ tự)
                sourceList.AddRange(_currentQuestions);
                // Sau đó thêm các câu ngoài bài từ ngân hàng
                foreach (var q in _allBankQuestions)
                {
                    if (!inSurveyIds.Contains(q.MaCauHoi))
                    {
                        sourceList.Add(q);
                    }
                }
            }

            // Lọc theo từ khóa
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                string kwLower = tuKhoa.ToLowerInvariant();
                sourceList = sourceList.Where(q =>
                    (q.NoiDungCauHoi != null && q.NoiDungCauHoi.ToLowerInvariant().Contains(kwLower)) ||
                    (q.DoanVanDoc != null && q.DoanVanDoc.ToLowerInvariant().Contains(kwLower)) ||
                    (q.GiaiThichDapAn != null && q.GiaiThichDapAn.ToLowerInvariant().Contains(kwLower))
                ).ToList();
            }

            // Lọc theo cấp độ
            if (!string.Equals(levelFilter, "TatCa", StringComparison.OrdinalIgnoreCase))
            {
                sourceList = sourceList.Where(q => string.Equals(q.MaTrinhDo, levelFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Lọc theo kỹ năng
            if (!string.Equals(skillFilter, "TatCa", StringComparison.OrdinalIgnoreCase))
            {
                sourceList = sourceList.Where(q => string.Equals(CauHoiKhaoSatDTO.ChuanHoaKyNang(q.KyNang), skillFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Lọc theo độ khó
            if (!string.Equals(diffFilter, "TatCa", StringComparison.OrdinalIgnoreCase))
            {
                sourceList = sourceList.Where(q => string.Equals(CauHoiKhaoSatDTO.ChuanHoaDoKho(q.DoKho), diffFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            lstQuestions.ItemsSource = sourceList;
            txtBankCount.Text = $"{sourceList.Count} câu hiển thị";
            pnlEmptyState.Visibility = sourceList.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TxtSearchBank_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApDungBoLocHienThi();
        }

        private void CboBankLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApDungBoLocHienThi();
        }

        private void CboBankSkill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApDungBoLocHienThi();
        }

        private void CboBankDifficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApDungBoLocHienThi();
        }

        private void ChkOnlySelected_Changed(object sender, RoutedEventArgs e)
        {
            ApDungBoLocHienThi();
        }

        private void BtnToggleDapAn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is CauHoiKhaoSatDTO q)
            {
                q.IsAnswersExpanded = !q.IsAnswersExpanded;
            }
        }

        private void BtnThemCauHoi_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is CauHoiKhaoSatDTO q)
            {
                if (_currentQuestions.Any(x => x.MaCauHoi == q.MaCauHoi)) return;

                q.IsInCurrentSurvey = true;
                _currentQuestions.Add(q);

                CapNhatSoThuTu();
                CapNhatThongKeVaPhanBo();
                ApDungBoLocHienThi();
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is CauHoiKhaoSatDTO q)
            {
                var target = _currentQuestions.FirstOrDefault(x => x.MaCauHoi == q.MaCauHoi);
                if (target != null)
                {
                    target.IsInCurrentSurvey = false;
                    target.ThuTuTrongBo = 0;
                    _currentQuestions.Remove(target);

                    CapNhatSoThuTu();
                    CapNhatThongKeVaPhanBo();
                    ApDungBoLocHienThi();
                }
            }
        }

        private void BtnLen_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is CauHoiKhaoSatDTO q)
            {
                int idx = _currentQuestions.IndexOf(q);
                if (idx > 0)
                {
                    _currentQuestions.Move(idx, idx - 1);
                    CapNhatSoThuTu();
                    ApDungBoLocHienThi();
                }
            }
        }

        private void BtnXuong_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement elem && elem.DataContext is CauHoiKhaoSatDTO q)
            {
                int idx = _currentQuestions.IndexOf(q);
                if (idx >= 0 && idx < _currentQuestions.Count - 1)
                {
                    _currentQuestions.Move(idx, idx + 1);
                    CapNhatSoThuTu();
                    ApDungBoLocHienThi();
                }
            }
        }

        private void BtnBoChonTatCa_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestions.Count == 0) return;

            foreach (var q in _currentQuestions)
            {
                q.IsInCurrentSurvey = false;
                q.ThuTuTrongBo = 0;
            }
            _currentQuestions.Clear();

            CapNhatThongKeVaPhanBo();
            ApDungBoLocHienThi();
        }

        private void CapNhatSoThuTu()
        {
            int stt = 1;
            foreach (var q in _currentQuestions)
            {
                q.ThuTuTrongBo = stt;
                q.ThuTuCauHoi = stt;
                stt++;
            }
        }

        private void BtnTuDongChon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string capDo = (cboCapDoSurvey.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "B1";

                // Lấy danh sách câu hỏi đề xuất từ ngân hàng câu hỏi
                var autoQuestions = KhaoSatBUS.Instance.LayDanhSachCauHoiTheoTrinhDo(capDo);

                if (autoQuestions != null && autoQuestions.Count >= 10)
                {
                    // Đánh dấu bỏ chọn các câu cũ
                    foreach (var oldQ in _currentQuestions)
                    {
                        oldQ.IsInCurrentSurvey = false;
                        oldQ.ThuTuTrongBo = 0;
                    }

                    _currentQuestions.Clear();
                    int stt = 1;
                    foreach (var q in autoQuestions.Take(20))
                    {
                        q.IsInCurrentSurvey = true;
                        q.ThuTuTrongBo = stt;
                        q.ThuTuCauHoi = stt;
                        stt++;
                        _currentQuestions.Add(q);
                    }

                    CapNhatThongKeVaPhanBo();
                    ApDungBoLocHienThi();
                    Common.ThongBaoDialog.ThanhCong($"Đã tự động chọn {_currentQuestions.Count} câu hỏi đa kỹ năng khớp chuẩn cấp độ {capDo}!", "Tự động chọn hoàn tất");
                }
                else
                {
                    Common.ThongBaoDialog.CanhBao("Không đủ số lượng câu hỏi trong ngân hàng để tự động chọn.", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.Loi($"Lỗi tự động chọn: {ex.Message}", "Lỗi");
            }
        }

        private void BtnXemTruoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(txtThoiGianPhut.Text.Trim(), out int thoiGian);
                if (thoiGian <= 0) thoiGian = 30;

                var deThi = new BaiTapDeThiDTO
                {
                    TenBaiTapDeThi = string.IsNullOrWhiteSpace(txtTenBaiKhaoSat.Text) ? "Khảo sát Năng lực Đầu vào" : txtTenBaiKhaoSat.Text.Trim(),
                    LoaiHinh = "KhaoSat",
                    ThoiGianLamBaiPhut = thoiGian,
                    TongSoCau = _currentQuestions.Count,
                    MoTa = txtMoTa.Text.Trim()
                };

                var previewDlg = new XemTruocDeThiDialog(deThi, _currentQuestions.ToList());
                ModalHelper.ShowDialogWithDim(previewDlg, this);
            }
            catch (Exception ex)
            {
                Common.ThongBaoDialog.Loi($"Lỗi khi mở xem trước: {ex.Message}", "Lỗi");
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            string tenBai = txtTenBaiKhaoSat.Text.Trim();
            if (string.IsNullOrEmpty(tenBai))
            {
                Common.ThongBaoDialog.CanhBao("Tên bài khảo sát không được để trống.", "Thiếu thông tin");
                return;
            }

            if (_currentQuestions.Count == 0)
            {
                Common.ThongBaoDialog.CanhBao("Bài khảo sát phải có ít nhất 1 câu hỏi.", "Thiếu câu hỏi");
                return;
            }

            string moTa = txtMoTa.Text.Trim();
            bool dangHoatDong = chkDangHoatDong.IsChecked == true;

            // 1. Cập nhật thông tin chung
            bool okInfo = KhaoSatBUS.Instance.CapNhatThongTinBaiKhaoSat(_maBaiKhaoSat, tenBai, moTa, dangHoatDong);
            if (!okInfo)
            {
                Common.ThongBaoDialog.Loi("Không thể cập nhật thông tin bài khảo sát.", "Lỗi");
                return;
            }

            // 2. Cập nhật danh sách câu hỏi và thứ tự
            var questionIds = _currentQuestions.Select(q => q.MaCauHoi).ToList();
            bool okQuestions = KhaoSatBUS.Instance.CapNhatCauHoiBaiKhaoSat(_maBaiKhaoSat, questionIds, out string errorMessage);

            if (okQuestions)
            {
                Common.ThongBaoDialog.ThanhCong(
                    $"🎉 Đã lưu bộ đề khảo sát thành công gồm {questionIds.Count} câu hỏi theo đúng thứ tự thiết lập!",
                    "Cập nhật thành công");
                DialogResult = true;
                Close();
            }
            else
            {
                Common.ThongBaoDialog.Loi($"Lỗi khi cập nhật danh sách câu hỏi: {errorMessage}", "Lỗi");
            }
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
