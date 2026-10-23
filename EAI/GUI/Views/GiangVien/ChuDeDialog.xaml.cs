using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class ChuDeDialog : Window
    {
        public ChuDeDTO ChuDeData { get; private set; }
        private readonly bool _isEditMode;

        public ChuDeDialog(List<ChuDeDTO> danhSachChuDe, ChuDeDTO? existing = null, int? defaultParentId = null)
        {
            InitializeComponent();
            _isEditMode = existing != null;

            // Danh sách chủ đề cha
            var parentList = new List<ChuDeDTO>
            {
                new ChuDeDTO { MaChuDe = 0, TenChuDe = "— Không có (Chủ đề gốc) —" }
            };

            // Khi sửa, loại trừ chính chủ đề này và toàn bộ các chủ đề con/cháu (tránh tạo vòng lặp)
            var excludedIds = new HashSet<int>();
            if (_isEditMode && existing != null)
            {
                excludedIds.Add(existing.MaChuDe);
                ThuThapCacChuDeConChau(existing.MaChuDe, danhSachChuDe, excludedIds);
            }

            foreach (var item in danhSachChuDe)
            {
                if (excludedIds.Contains(item.MaChuDe))
                    continue;

                parentList.Add(item);
            }

            cboChuDeCha.ItemsSource = parentList;

            if (_isEditMode && existing != null)
            {
                ChuDeData = existing;
                txtTitle.Text = "Chỉnh sửa Chủ đề";
                txtTenChuDe.Text = existing.TenChuDe;
                txtMoTa.Text = existing.MoTa ?? "";

                if (existing.MaChuDeCha.HasValue)
                {
                    cboChuDeCha.SelectedValue = existing.MaChuDeCha.Value;
                }
                else
                {
                    cboChuDeCha.SelectedValue = 0;
                }
            }
            else
            {
                ChuDeData = new ChuDeDTO();
                txtTitle.Text = "Thêm Chủ đề Mới";

                if (defaultParentId.HasValue && defaultParentId.Value > 0)
                {
                    cboChuDeCha.SelectedValue = defaultParentId.Value;
                }
                else
                {
                    cboChuDeCha.SelectedValue = 0;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string ten = txtTenChuDe.Text.Trim();
            if (string.IsNullOrWhiteSpace(ten))
            {
                Views.Common.ThongBaoDialog.CanhBao(this, "Vui lòng nhập tên chủ đề học liệu.", "Thiếu thông tin");
                txtTenChuDe.Focus();
                return;
            }

            ChuDeData.TenChuDe = ten;
            ChuDeData.MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim();

            if (cboChuDeCha.SelectedValue is int parentId && parentId > 0)
            {
                ChuDeData.MaChuDeCha = parentId;
            }
            else
            {
                ChuDeData.MaChuDeCha = null;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private static void ThuThapCacChuDeConChau(int parentId, List<ChuDeDTO> allTopics, HashSet<int> result)
        {
            var children = allTopics.Where(c => c.MaChuDeCha.HasValue && c.MaChuDeCha.Value == parentId).ToList();
            foreach (var child in children)
            {
                if (result.Add(child.MaChuDe))
                {
                    ThuThapCacChuDeConChau(child.MaChuDe, allTopics, result);
                }
            }
        }
    }
}
