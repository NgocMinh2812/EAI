using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DTO;

namespace GUI.Views.GiangVien
{
    public class ChuDeDisplayItem
    {
        public int MaChuDe { get; set; }
        public string TenHienThi { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
    }

    public partial class DiChuyenThuMucDialog : Window
    {
        public int SelectedTargetMaChuDe { get; private set; }
        public string SelectedTargetTenChuDe { get; private set; } = string.Empty;

        private readonly BaiHocDTO _lesson;
        private readonly List<ChuDeDisplayItem> _displayItems = new();

        public DiChuyenThuMucDialog(BaiHocDTO lesson, List<ChuDeDTO> allChuDe)
        {
            InitializeComponent();
            _lesson = lesson;

            txtLessonTitle.Text = lesson.TieuDe;
            txtCurrentFolder.Text = $"📁 {lesson.TenChuDe}";

            // Xây dựng danh sách thư mục có thụt đầu dòng phân cấp
            BuildHierarchicalDisplay(allChuDe);
            cboTargetFolder.ItemsSource = _displayItems;

            // Mặc định chọn thư mục đầu tiên khác thư mục hiện tại
            var firstOther = _displayItems.FirstOrDefault(x => !x.IsCurrent);
            if (firstOther != null)
            {
                cboTargetFolder.SelectedValue = firstOther.MaChuDe;
            }
            else if (_displayItems.Count > 0)
            {
                cboTargetFolder.SelectedIndex = 0;
            }
        }

        private void BuildHierarchicalDisplay(List<ChuDeDTO> allChuDe)
        {
            var validChuDe = allChuDe.Where(c => c.MaChuDe > 0).ToList();
            var roots = validChuDe.Where(c => !c.MaChuDeCha.HasValue || c.MaChuDeCha <= 0).ToList();
            var visited = new HashSet<int>();

            foreach (var r in roots)
            {
                AddNodeAndChildren(r, validChuDe, visited, 0);
            }

            // Phòng trường hợp có node mồ côi
            foreach (var orphan in validChuDe.Where(c => !visited.Contains(c.MaChuDe)))
            {
                AddNodeAndChildren(orphan, validChuDe, visited, 0);
            }
        }

        private void AddNodeAndChildren(ChuDeDTO node, List<ChuDeDTO> all, HashSet<int> visited, int depth)
        {
            if (visited.Contains(node.MaChuDe)) return;
            visited.Add(node.MaChuDe);

            string indent = depth == 0 ? "📁 " : new string(' ', depth * 4) + "└─ 📁 ";
            bool isCurrent = node.MaChuDe == _lesson.MaChuDe;
            string text = $"{indent}{node.TenChuDe}" + (isCurrent ? " (Thư mục hiện tại)" : "");

            _displayItems.Add(new ChuDeDisplayItem
            {
                MaChuDe = node.MaChuDe,
                TenHienThi = text,
                IsCurrent = isCurrent
            });

            var children = all.Where(c => c.MaChuDeCha == node.MaChuDe).ToList();
            foreach (var ch in children)
            {
                AddNodeAndChildren(ch, all, visited, depth + 1);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (cboTargetFolder.SelectedValue is int targetId && targetId > 0)
            {
                if (targetId == _lesson.MaChuDe)
                {
                    Views.Common.ThongBaoDialog.CanhBao(this, "Bài học đã nằm trong thư mục này rồi! Vui lòng chọn một thư mục khác.", "Chưa thay đổi thư mục");
                    return;
                }

                SelectedTargetMaChuDe = targetId;
                var found = _displayItems.FirstOrDefault(x => x.MaChuDe == targetId);
                SelectedTargetTenChuDe = found != null ? found.TenHienThi.Trim() : "Thư mục đích";

                DialogResult = true;
                Close();
            }
            else
            {
                Views.Common.ThongBaoDialog.CanhBao(this, "Vui lòng chọn thư mục đích cần chuyển đến.", "Chưa chọn thư mục");
            }
        }
    }
}
