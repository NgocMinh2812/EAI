using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class CapNhatKhaoSatDialog : Window
    {
        private int _maBaiKhaoSat = 1;
        private ObservableCollection<CauHoiKhaoSatDTO> _currentQuestions = new();
        private List<CauHoiKhaoSatDTO> _availableBankQuestions = new();

        public CapNhatKhaoSatDialog()
        {
            InitializeComponent();
            Loaded += CapNhatKhaoSatDialog_Loaded;
        }

        private void CapNhatKhaoSatDialog_Loaded(object sender, RoutedEventArgs e)
        {
            NapThongTinBaiKhaoSat();
            TaiDanhSachCauHoiNganHang();
        }

        private void NapThongTinBaiKhaoSat()
        {
            var info = KhaoSatBUS.Instance.LayBaiKhaoSatDangHoatDong();
            _maBaiKhaoSat = info.MaBaiKhaoSat;
            txtTenBaiKhaoSat.Text = info.TenBai;
            txtMoTa.Text = info.MoTa;
            chkDangHoatDong.IsChecked = info.DangHoatDong;

            var questions = KhaoSatBUS.Instance.LayDanhSachCauHoiTheoBaiKhaoSat(_maBaiKhaoSat);
            _currentQuestions = new ObservableCollection<CauHoiKhaoSatDTO>(questions);
            CapNhatSoThuTu();

            dgQuestionsInSurvey.ItemsSource = _currentQuestions;
            CapNhatCountDisplay();
        }

        private void TaiDanhSachCauHoiNganHang()
        {
            string tuKhoa = txtSearchBank.Text.Trim();
            string trinhDo = (cboBankLevel.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "TatCa";

            var list = KhaoSatBUS.Instance.LayDanhSachCauHoiCoTheThem(_maBaiKhaoSat, tuKhoa, trinhDo);
            // Loại trừ cả những câu đang được thêm trong danh sách hiện tại của dialog
            var existingIds = _currentQuestions.Select(q => q.MaCauHoi).ToHashSet();
            _availableBankQuestions = list.Where(q => !existingIds.Contains(q.MaCauHoi)).ToList();
            dgAvailableBank.ItemsSource = _availableBankQuestions;
        }

        private void CapNhatSoThuTu()
        {
            int stt = 1;
            foreach (var q in _currentQuestions)
            {
                q.ThuTuCauHoi = stt++;
            }
        }

        private void CapNhatCountDisplay()
        {
            txtCountSelected.Text = $"{_currentQuestions.Count} câu";
        }

        private void BtnLen_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dgQuestionsInSurvey.SelectedIndex;
            if (selectedIndex > 0)
            {
                var item = _currentQuestions[selectedIndex];
                _currentQuestions.RemoveAt(selectedIndex);
                _currentQuestions.Insert(selectedIndex - 1, item);
                CapNhatSoThuTu();
                dgQuestionsInSurvey.SelectedIndex = selectedIndex - 1;
            }
        }

        private void BtnXuong_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dgQuestionsInSurvey.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _currentQuestions.Count - 1)
            {
                var item = _currentQuestions[selectedIndex];
                _currentQuestions.RemoveAt(selectedIndex);
                _currentQuestions.Insert(selectedIndex + 1, item);
                CapNhatSoThuTu();
                dgQuestionsInSurvey.SelectedIndex = selectedIndex + 1;
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgQuestionsInSurvey.SelectedItem is CauHoiKhaoSatDTO selected)
            {
                _currentQuestions.Remove(selected);
                CapNhatSoThuTu();
                CapNhatCountDisplay();
                TaiDanhSachCauHoiNganHang();
            }
            else
            {
                Common.ThongBaoDialog.CanhBao("Vui lòng chọn 1 câu hỏi trong bộ đề để xóa.", "Thông báo");
            }
        }

        private void BtnThemCauHoi_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CauHoiKhaoSatDTO q)
            {
                _currentQuestions.Add(new CauHoiKhaoSatDTO
                {
                    MaCauHoi = q.MaCauHoi,
                    ThuTuCauHoi = _currentQuestions.Count + 1,
                    KyNang = q.KyNang,
                    MaTrinhDo = q.MaTrinhDo,
                    TenTrinhDo = q.TenTrinhDo,
                    NoiDungCauHoi = q.NoiDungCauHoi,
                    DoanVanDoc = q.DoanVanDoc,
                    GiaiThichDapAn = q.GiaiThichDapAn,
                    LoaiCauHoi = q.LoaiCauHoi,
                    DoKho = q.DoKho,
                    DanhSachLuaChon = q.DanhSachLuaChon
                });

                CapNhatSoThuTu();
                CapNhatCountDisplay();
                TaiDanhSachCauHoiNganHang();
            }
        }

        private void TxtSearchBank_TextChanged(object sender, TextChangedEventArgs e)
        {
            TaiDanhSachCauHoiNganHang();
        }

        private void CboBankLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                TaiDanhSachCauHoiNganHang();
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
    }
}
