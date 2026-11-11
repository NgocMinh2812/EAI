using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public enum HanhDongPheDuyet
    {
        TaoBoMoi,
        ThemVaoBoCoSan,
        ChiLuu
    }

    public partial class XacNhanPheDuyetVaTaoBoDialog : Window
    {
        public HanhDongPheDuyet HanhDong { get; private set; } = HanhDongPheDuyet.TaoBoMoi;
        public string TenBoMoi { get; private set; } = string.Empty;
        public string? MoTaBoMoi { get; private set; }
        public int MaBoCoSan { get; private set; } = 0;
        public string TenBoCoSan { get; private set; } = string.Empty;

        private readonly List<BoCauHoiDTO> _dsBoCoSan = new();

        public XacNhanPheDuyetVaTaoBoDialog(int soLuongCauHoi, string? chuDe = null)
        {
            InitializeComponent();

            txtSoLuongPheDuyet.Text = $"{soLuongCauHoi} câu hỏi";
            string safeTopic = !string.IsNullOrWhiteSpace(chuDe) ? chuDe.Trim() : "Luyện tập tiếng Anh";
            txtChuDeYeuCau.Text = $"Chủ đề / Yêu cầu: {safeTopic}";

            // Gợi ý tên bộ mới
            string suggestName = safeTopic.Length > 60 ? safeTopic.Substring(0, 57) + "..." : safeTopic;
            txtTenBoMoi.Text = $"Bộ câu hỏi: {suggestName}";
            txtMoTaBoMoi.Text = $"Bộ câu hỏi luyện tập tạo bằng AI theo chủ đề '{safeTopic}'";

            NapDanhSachBoCauHoiCoSan();
        }

        private void NapDanhSachBoCauHoiCoSan()
        {
            try
            {
                var list = BoCauHoiBUS.Instance.LayDanhSachBoCauHoi();
                cboBoCauHoiCoSan.Items.Clear();
                _dsBoCoSan.Clear();

                if (list != null && list.Count > 0)
                {
                    _dsBoCoSan.AddRange(list);
                    foreach (var bo in list)
                    {
                        cboBoCauHoiCoSan.Items.Add(new ComboBoxItem
                        {
                            Content = $"[#{bo.MaBoCauHoi}] {bo.TenBoCauHoi} ({bo.SoLuongCauHoi} câu hiện tại)",
                            Tag = bo
                        });
                    }
                    cboBoCauHoiCoSan.SelectedIndex = 0;
                }
                else
                {
                    cboBoCauHoiCoSan.Items.Add(new ComboBoxItem
                    {
                        Content = "-- Chưa có bộ câu hỏi nào trong hệ thống --",
                        Tag = null,
                        IsEnabled = false
                    });
                    cboBoCauHoiCoSan.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NapDanhSachBoCauHoiCoSan] {ex.Message}");
            }
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (pnlTaoBoMoi == null || pnlThemVaoBoCoSan == null) return;

            if (rbTaoBoMoi.IsChecked == true)
            {
                pnlTaoBoMoi.Visibility = Visibility.Visible;
                pnlThemVaoBoCoSan.Visibility = Visibility.Collapsed;
            }
            else if (rbThemVaoBoCoSan.IsChecked == true)
            {
                pnlTaoBoMoi.Visibility = Visibility.Collapsed;
                pnlThemVaoBoCoSan.Visibility = Visibility.Visible;
            }
            else
            {
                pnlTaoBoMoi.Visibility = Visibility.Collapsed;
                pnlThemVaoBoCoSan.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (rbTaoBoMoi.IsChecked == true)
            {
                string ten = txtTenBoMoi.Text.Trim();
                if (string.IsNullOrWhiteSpace(ten))
                {
                    Common.ThongBaoDialog.CanhBao("Vui lòng nhập tên cho Bộ câu hỏi mới.", "Thiếu thông tin");
                    txtTenBoMoi.Focus();
                    return;
                }

                HanhDong = HanhDongPheDuyet.TaoBoMoi;
                TenBoMoi = ten;
                MoTaBoMoi = txtMoTaBoMoi.Text.Trim();
            }
            else if (rbThemVaoBoCoSan.IsChecked == true)
            {
                if (cboBoCauHoiCoSan.SelectedItem is ComboBoxItem item && item.Tag is BoCauHoiDTO bo)
                {
                    HanhDong = HanhDongPheDuyet.ThemVaoBoCoSan;
                    MaBoCoSan = bo.MaBoCauHoi;
                    TenBoCoSan = bo.TenBoCauHoi;
                }
                else
                {
                    Common.ThongBaoDialog.CanhBao("Hệ thống chưa có bộ câu hỏi nào để thêm vào. Vui lòng chọn tùy chọn 'Tạo Bộ câu hỏi mới' hoặc 'Chỉ lưu vào Ngân hàng'.", "Thông báo");
                    return;
                }
            }
            else
            {
                HanhDong = HanhDongPheDuyet.ChiLuu;
            }

            DialogResult = true;
            Close();
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}

