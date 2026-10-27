using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using BUS;
using DTO;

namespace GUI.Views.GiangVien
{
    public partial class SuaCauHoiAIDialog : Window
    {
        private readonly CauHoiDoAISinhItemDTO _item;

        public SuaCauHoiAIDialog(CauHoiDoAISinhItemDTO item)
        {
            InitializeComponent();
            _item = item;
            NapDuLieu();
        }

        private void NapDuLieu()
        {
            txtSubHeader.Text = $"Mã yêu cầu AI: #{_item.MaYeuCau} • Mô hình: {_item.MoHinhAI ?? "Google Gemini"}";
            lblKyNang.Text = $"Kỹ năng: {_item.KyNang}";
            lblTrinhDo.Text = $"Trình độ: {_item.MaCapDo ?? "B1"}";
            lblLoaiCauHoi.Text = $"Dạng: {CauHoiKhaoSatDTO.ChuanHoaLoaiCauHoi(_item.LoaiCauHoi)}";

            txtNoiDungCauHoi.Text = _item.NoiDungCauHoi;
            txtDoanVanDoc.Text = _item.DoanVanDoc ?? "";
            txtGiaiThich.Text = _item.GiaiThichDapAn ?? "";

            if (string.IsNullOrWhiteSpace(_item.DoanVanDoc))
            {
                pnlDoanVanDoc.Visibility = Visibility.Collapsed;
            }

            // Nạp các lựa chọn đáp án
            var luaChonList = _item.DanhSachLuaChon;
            if (luaChonList != null && luaChonList.Count > 0)
            {
                var optA = luaChonList.FirstOrDefault(l => l.KyHieuLuaChon == "A") ?? luaChonList.ElementAtOrDefault(0);
                var optB = luaChonList.FirstOrDefault(l => l.KyHieuLuaChon == "B") ?? luaChonList.ElementAtOrDefault(1);
                var optC = luaChonList.FirstOrDefault(l => l.KyHieuLuaChon == "C") ?? luaChonList.ElementAtOrDefault(2);
                var optD = luaChonList.FirstOrDefault(l => l.KyHieuLuaChon == "D") ?? luaChonList.ElementAtOrDefault(3);

                if (optA != null)
                {
                    txtOptionA.Text = optA.NoiDungLuaChon;
                    rbOptionA.IsChecked = optA.LaDapAnDung;
                }
                if (optB != null)
                {
                    txtOptionB.Text = optB.NoiDungLuaChon;
                    rbOptionB.IsChecked = optB.LaDapAnDung;
                }
                if (optC != null)
                {
                    txtOptionC.Text = optC.NoiDungLuaChon;
                    rbOptionC.IsChecked = optC.LaDapAnDung;
                }
                if (optD != null)
                {
                    txtOptionD.Text = optD.NoiDungLuaChon;
                    rbOptionD.IsChecked = optD.LaDapAnDung;
                }
            }
            else
            {
                rbOptionA.IsChecked = true;
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
            string noiDung = txtNoiDungCauHoi.Text.Trim();
            if (string.IsNullOrEmpty(noiDung))
            {
                Common.ThongBaoDialog.CanhBao("Nội dung câu hỏi không được để trống.", "Thiếu thông tin");
                return;
            }

            string optAText = txtOptionA.Text.Trim();
            string optBText = txtOptionB.Text.Trim();
            string optCText = txtOptionC.Text.Trim();
            string optDText = txtOptionD.Text.Trim();

            if (string.IsNullOrEmpty(optAText) || string.IsNullOrEmpty(optBText))
            {
                Common.ThongBaoDialog.CanhBao("Vui lòng nhập tối thiểu 2 lựa chọn đáp án (A và B).", "Thiếu thông tin");
                return;
            }

            bool hasCorrect = (rbOptionA.IsChecked == true) || (rbOptionB.IsChecked == true) ||
                              (rbOptionC.IsChecked == true) || (rbOptionD.IsChecked == true);
            if (!hasCorrect)
            {
                Common.ThongBaoDialog.CanhBao("Vui lòng chọn 1 đáp án đúng bằng nút chọn tròn bên trái.", "Chưa chọn đáp án đúng");
                return;
            }

            var updatedOptions = new List<LuaChonDapAnDTO>
            {
                new LuaChonDapAnDTO { KyHieuLuaChon = "A", NoiDungLuaChon = optAText, LaDapAnDung = rbOptionA.IsChecked == true },
                new LuaChonDapAnDTO { KyHieuLuaChon = "B", NoiDungLuaChon = optBText, LaDapAnDung = rbOptionB.IsChecked == true }
            };

            if (!string.IsNullOrEmpty(optCText))
            {
                updatedOptions.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "C", NoiDungLuaChon = optCText, LaDapAnDung = rbOptionC.IsChecked == true });
            }
            if (!string.IsNullOrEmpty(optDText))
            {
                updatedOptions.Add(new LuaChonDapAnDTO { KyHieuLuaChon = "D", NoiDungLuaChon = optDText, LaDapAnDung = rbOptionD.IsChecked == true });
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonLuaChon = JsonSerializer.Serialize(updatedOptions, jsonOptions);

            string? doanVan = string.IsNullOrWhiteSpace(txtDoanVanDoc.Text) ? null : txtDoanVanDoc.Text.Trim();
            string? giaiThich = string.IsNullOrWhiteSpace(txtGiaiThich.Text) ? null : txtGiaiThich.Text.Trim();

            bool thanhCong = AISinhDeBUS.Instance.CapNhatNoiDungCauHoiAISinh(
                _item.MaCauHoiAI,
                noiDung,
                doanVan,
                giaiThich,
                jsonLuaChon,
                out string errorMessage
            );

            if (thanhCong)
            {
                _item.NoiDungCauHoi = noiDung;
                _item.DoanVanDoc = doanVan;
                _item.GiaiThichDapAn = giaiThich;
                _item.DanhSachLuaChon = updatedOptions;

                Common.ThongBaoDialog.ThanhCong("Đã lưu cập nhật nội dung câu hỏi thành công!", "Thành công");
                DialogResult = true;
                Close();
            }
            else
            {
                Common.ThongBaoDialog.Loi($"Không thể lưu cập nhật câu hỏi: {errorMessage}", "Lỗi lưu dữ liệu");
            }
        }
    }
}
