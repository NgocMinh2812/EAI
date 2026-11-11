using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BUS;
using DTO;
using GUI.Services;

namespace GUI.Views.GiangVien
{
    public partial class TaoFlashcardDialog : Window
    {
        private readonly DanhMucBUS _danhMucBUS = new DanhMucBUS();
        private readonly TuVungBUS _tuVungBUS = TuVungBUS.Instance;

        public bool HasNewFlashcard { get; private set; } = false;

        private List<FlashcardDTO> _allFlashcards = new List<FlashcardDTO>();
        private List<FlashcardDTO> _filteredFlashcards = new List<FlashcardDTO>();
        private int _currentIndex = 0;
        private bool _isFlipped = false;

        public TaoFlashcardDialog(bool openStudyTab = false)
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                KhoiTaoDuLieu();
                if (cboLocTrinhDo != null)
                {
                    cboLocTrinhDo.SelectionChanged += CboLocTrinhDo_SelectionChanged;
                }
                if (openStudyTab)
                {
                    ChuyenSangTabOnTap();
                }
            };
        }

        public TaoFlashcardDialog(string? prefillWord, string? prefillPhienAm = null, string? prefillNghia = null, string? prefillViDu = null, bool openStudyTab = false)
            : this(openStudyTab)
        {
            this.Loaded += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(prefillWord)) txtTu.Text = prefillWord;
                if (!string.IsNullOrWhiteSpace(prefillPhienAm)) txtPhienAm.Text = prefillPhienAm;
                if (!string.IsNullOrWhiteSpace(prefillNghia)) txtNghia.Text = prefillNghia;
                if (!string.IsNullOrWhiteSpace(prefillViDu)) txtCauViDu.Text = prefillViDu;
            };
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = HasNewFlashcard;
            this.Close();
        }

        private void KhoiTaoDuLieu()
        {
            try
            {
                // Nạp danh sách chủ đề
                var chuDes = _danhMucBUS.LayDanhSachChuDe();
                cboChuDe.ItemsSource = chuDes;
                cboChuDe.DisplayMemberPath = "TenChuDe";
                cboChuDe.SelectedValuePath = "MaChuDe";
                if (chuDes.Count > 0)
                {
                    cboChuDe.SelectedIndex = 0;
                }

                // Nạp danh sách Flashcard
                TaiDanhSachFlashcard();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaoFlashcardDialog] Lỗi khởi tạo dữ liệu: {ex.Message}");
            }
        }

        private void TaiDanhSachFlashcard()
        {
            try
            {
                _allFlashcards = _tuVungBUS.LayDanhSachFlashcard() ?? new List<FlashcardDTO>();
                ApDungBoLocTrinhDo();
            }
            catch { }
        }

        private void ApDungBoLocTrinhDo()
        {
            if (!this.IsLoaded) return;

            string? trinhDo = null;
            if (cboLocTrinhDo?.SelectedItem is ComboBoxItem item)
            {
                string text = item.Content.ToString() ?? "";
                if (text != "Tất cả cấp độ") trinhDo = text.Trim();
            }

            if (string.IsNullOrEmpty(trinhDo))
            {
                _filteredFlashcards = new List<FlashcardDTO>(_allFlashcards);
            }
            else
            {
                _filteredFlashcards = _allFlashcards
                    .Where(x => string.Equals(x.MaCapDo?.Trim(), trinhDo, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            _currentIndex = 0;
            _isFlipped = false;
            HienThiCardHienTai();
        }

        private void HienThiCardHienTai()
        {
            if (!this.IsLoaded || txtTienDoOnTap == null || txtDaThuocCount == null || txtMatTruocTu == null)
                return;

            if (_filteredFlashcards.Count == 0)
            {
                txtTienDoOnTap.Text = "0 / 0";
                txtDaThuocCount.Text = "Đã thuộc: 0/0";
                txtCardBadge.Text = "CHƯA CÓ THẺ";
                txtMatTruocTu.Text = "Chưa có từ vựng";
                txtMatTruocIPA.Text = "";
                txtMatSauTu.Text = "";
                txtMatSauLoaiTu.Text = "";
                txtMatSauNghia.Text = "Vui lòng chuyển qua tab '+ Tạo Flashcard mới' để thêm từ vựng.";
                txtMatSauViDu.Text = "";
                txtMatSauThongKe.Text = "";
                pnlMatTruoc.Visibility = Visibility.Visible;
                pnlMatSau.Visibility = Visibility.Collapsed;
                return;
            }

            if (_currentIndex < 0) _currentIndex = _filteredFlashcards.Count - 1;
            if (_currentIndex >= _filteredFlashcards.Count) _currentIndex = 0;

            var card = _filteredFlashcards[_currentIndex];
            txtTienDoOnTap.Text = $"Thẻ {_currentIndex + 1} / {_filteredFlashcards.Count}";

            int daThuocCount = _filteredFlashcards.Count(x => x.DaThuoc);
            txtDaThuocCount.Text = $"Đã thuộc: {daThuocCount}/{_filteredFlashcards.Count}";

            // Mặt trước
            txtCardBadge.Text = $"{card.TenChuDe.ToUpper()} • {card.MaCapDo}";
            txtMatTruocTu.Text = card.Tu;
            txtMatTruocIPA.Text = !string.IsNullOrWhiteSpace(card.PhienAmIPA) ? card.PhienAmIPA : "(Chưa có phiên âm IPA)";

            // Mặt sau
            txtMatSauTu.Text = card.Tu;
            txtMatSauLoaiTu.Text = !string.IsNullOrWhiteSpace(card.LoaiTu) ? card.LoaiTu : "Từ vựng";
            txtMatSauNghia.Text = card.Nghia;
            txtMatSauViDu.Text = !string.IsNullOrWhiteSpace(card.CauViDu) ? $"\"{card.CauViDu}\"" : "\"No example sentence provided.\"";
            txtMatSauThongKe.Text = $"Đã ôn tập: {card.SoLanOnTap} lần • Ghi nhớ đúng: {card.SoLanDung} lần";

            // Trạng thái lật thẻ
            pnlMatTruoc.Visibility = _isFlipped ? Visibility.Collapsed : Visibility.Visible;
            pnlMatSau.Visibility = _isFlipped ? Visibility.Visible : Visibility.Collapsed;
        }

        #region Tab Navigation
        private void BtnTabTaoMoi_Click(object sender, RoutedEventArgs e)
        {
            pnlTaoMoi.Visibility = Visibility.Visible;
            pnlOnTap.Visibility = Visibility.Collapsed;

            btnTabTaoMoi.Background = new SolidColorBrush(Color.FromRgb(15, 118, 110)); // #0F766E
            btnTabTaoMoi.Foreground = Brushes.White;

            btnTabOnTap.Background = Brushes.Transparent;
            btnTabOnTap.Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139)); // #64748B
        }

        private void BtnTabOnTap_Click(object sender, RoutedEventArgs e)
        {
            ChuyenSangTabOnTap();
        }

        private void ChuyenSangTabOnTap()
        {
            pnlTaoMoi.Visibility = Visibility.Collapsed;
            pnlOnTap.Visibility = Visibility.Visible;

            btnTabOnTap.Background = new SolidColorBrush(Color.FromRgb(15, 118, 110));
            btnTabOnTap.Foreground = Brushes.White;

            btnTabTaoMoi.Background = Brushes.Transparent;
            btnTabTaoMoi.Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139));

            TaiDanhSachFlashcard();
        }
        #endregion

        #region Thao tác Tạo mới Flashcard
        private void BtnIpaChar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Content != null)
            {
                string ch = btn.Content.ToString() ?? "";
                txtPhienAm.Text += ch;
                txtPhienAm.CaretIndex = txtPhienAm.Text.Length;
                txtPhienAm.Focus();
            }
        }

        private void BtnDocThuTu_Click(object sender, RoutedEventArgs e)
        {
            string tu = txtTu.Text.Trim();
            if (string.IsNullOrWhiteSpace(tu))
            {
                HienThiThongBao("Vui lòng nhập từ vựng tiếng Anh trước khi nghe phát âm.", isError: true);
                return;
            }
            PhatAmTiengAnh(tu);
        }

        private void BtnLuuFlashcard_Click(object sender, RoutedEventArgs e)
        {
            string tu = txtTu.Text.Trim();
            string nghia = txtNghia.Text.Trim();

            if (string.IsNullOrWhiteSpace(tu))
            {
                HienThiThongBao("Vui lòng nhập từ vựng tiếng Anh (*).", isError: true);
                txtTu.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(nghia))
            {
                HienThiThongBao("Vui lòng nhập giải nghĩa tiếng Việt (*).", isError: true);
                txtNghia.Focus();
                return;
            }

            string loaiTu = (cboLoaiTu.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Danh từ";
            // Rút gọn loại từ nếu có ngoặc, e.g. "Danh từ (noun)" -> "danh từ"
            if (loaiTu.Contains("(")) loaiTu = loaiTu.Substring(0, loaiTu.IndexOf('(')).Trim().ToLower();

            string trinhDoText = (cboTrinhDo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "B2";
            string maCapDo = trinhDoText.Length >= 2 ? trinhDoText.Substring(0, 2).Trim() : "B2";

            int? maTrinhDo = maCapDo switch
            {
                "A1" => 0,
                "A2" => 1,
                "B1" => 2,
                "B2" => 3,
                "C1" => 4,
                "C2" => 5,
                _ => 3
            };

            int? maChuDe = cboChuDe.SelectedValue != null ? Convert.ToInt32(cboChuDe.SelectedValue) : (int?)null;

            var tvDto = new TuVungDTO
            {
                Tu = tu,
                LoaiTu = loaiTu,
                PhienAmIPA = txtPhienAm.Text.Trim(),
                Nghia = nghia,
                CauViDu = txtCauViDu.Text.Trim(),
                MaChuDe = maChuDe,
                MaTrinhDo = maTrinhDo,
                MaCapDo = maCapDo
            };

            var (success, msg, maTuVung) = _tuVungBUS.ThemTuVungVaFlashcard(tvDto);
            if (success)
            {
                HasNewFlashcard = true;
                HienThiThongBao($"✓ Đã lưu thành công từ vựng '{tu}' vào Flashcard!", isError: false);

                // Reset form
                txtTu.Text = "";
                txtPhienAm.Text = "";
                txtNghia.Text = "";
                txtCauViDu.Text = "";

                // Tải lại danh sách để tab ôn tập có ngay từ mới
                TaiDanhSachFlashcard();

                // Tự động phát âm từ vừa lưu
                PhatAmTiengAnh(tu);
            }
            else
            {
                HienThiThongBao(msg, isError: true);
            }
        }

        private void HienThiThongBao(string msg, bool isError)
        {
            txtStatus.Text = msg;
            txtStatus.Foreground = isError 
                ? new SolidColorBrush(Color.FromRgb(220, 38, 38)) 
                : new SolidColorBrush(Color.FromRgb(21, 128, 61));
            txtStatus.Visibility = Visibility.Visible;
        }
        #endregion

        #region Thao tác Thẻ Ôn tập Flashcard
        private void CboLocTrinhDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded) return;
            ApDungBoLocTrinhDo();
        }

        private void BdCard_Click(object sender, MouseButtonEventArgs e)
        {
            _isFlipped = !_isFlipped;
            pnlMatTruoc.Visibility = _isFlipped ? Visibility.Collapsed : Visibility.Visible;
            pnlMatSau.Visibility = _isFlipped ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnCardFlip_Click(object sender, RoutedEventArgs e)
        {
            _isFlipped = !_isFlipped;
            pnlMatTruoc.Visibility = _isFlipped ? Visibility.Collapsed : Visibility.Visible;
            pnlMatSau.Visibility = _isFlipped ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnPhatAmMatTruoc_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (_filteredFlashcards.Count > 0 && _currentIndex < _filteredFlashcards.Count)
            {
                PhatAmTiengAnh(_filteredFlashcards[_currentIndex].Tu);
            }
        }

        private void BtnCardPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            _currentIndex--;
            _isFlipped = false;
            HienThiCardHienTai();
        }

        private void BtnCardNext_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            _currentIndex++;
            _isFlipped = false;
            HienThiCardHienTai();
        }

        private void BtnChuaNho_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            var card = _filteredFlashcards[_currentIndex];
            _tuVungBUS.CapNhatKetQuaFlashcard(card.MaTuVung, daNho: false);
            card.SoLanOnTap++;
            card.SoLanDung = 0;

            // Chuyển sang thẻ tiếp theo
            _currentIndex++;
            _isFlipped = false;
            HienThiCardHienTai();
        }

        private void BtnDaThuoc_Click(object sender, RoutedEventArgs e)
        {
            if (_filteredFlashcards.Count == 0) return;
            var card = _filteredFlashcards[_currentIndex];
            _tuVungBUS.CapNhatKetQuaFlashcard(card.MaTuVung, daNho: true);
            card.SoLanOnTap++;
            card.SoLanDung++;

            // Chuyển sang thẻ tiếp theo
            _currentIndex++;
            _isFlipped = false;
            HienThiCardHienTai();
        }
        #endregion

        #region AI Pronunciation Audio
        private void PhatAmTiengAnh(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return;
            _ = AIPronunciationService.Instance.PlayPronunciationAsync(word);
        }
        #endregion
    }
}

