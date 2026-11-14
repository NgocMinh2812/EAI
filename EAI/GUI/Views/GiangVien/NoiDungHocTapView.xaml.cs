using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using GUI.ViewModels.GiangVien;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class NoiDungHocTapView : Page
    {
        private bool _isInternalChange = false;
        private Window? _parentWindow = null;

        public NoiDungHocTapView()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
            Unloaded += Page_Unloaded;
            DataContextChanged += Page_DataContextChanged;
        }

        public NoiDungHocTapView(string? searchKeyword = null, string? kyNang = null, int? targetLessonId = null, int? maChuDe = null) : this()
        {
            Loaded += (s, e) =>
            {
                if (DataContext is NoiDungHocTapViewModel vm)
                {
                    if (maChuDe.HasValue && maChuDe.Value > 0)
                    {
                        var cd = vm.DanhSachChuDe.FirstOrDefault(c => c.MaChuDe == maChuDe.Value);
                        if (cd != null)
                        {
                            vm.SelectedChuDeFilter = cd;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(searchKeyword))
                    {
                        vm.TuKhoaTimKiem = searchKeyword.Trim();
                    }
                    if (!string.IsNullOrWhiteSpace(kyNang))
                    {
                        string kn = kyNang.Trim();
                        if (kn.Equals("Ngữ pháp", StringComparison.OrdinalIgnoreCase) || kn.Equals("NguPhap", StringComparison.OrdinalIgnoreCase))
                            vm.SelectedKyNangFilter = "NguPhap";
                        else if (kn.Equals("Từ vựng", StringComparison.OrdinalIgnoreCase) || kn.Equals("TuVung", StringComparison.OrdinalIgnoreCase))
                            vm.SelectedKyNangFilter = "TuVung";
                        else if (kn.Equals("Đọc hiểu", StringComparison.OrdinalIgnoreCase) || kn.Equals("Doc", StringComparison.OrdinalIgnoreCase))
                            vm.SelectedKyNangFilter = "Doc";
                        else if (kn.Equals("Nghe hiểu", StringComparison.OrdinalIgnoreCase) || kn.Equals("Nghe", StringComparison.OrdinalIgnoreCase))
                            vm.SelectedKyNangFilter = "Nghe";
                        else if (kn.Equals("Viết luận", StringComparison.OrdinalIgnoreCase) || kn.Equals("Viet", StringComparison.OrdinalIgnoreCase))
                            vm.SelectedKyNangFilter = "Viet";
                        else if (kn.Equals("Luyện nói", StringComparison.OrdinalIgnoreCase) || kn.Equals("Noi", StringComparison.OrdinalIgnoreCase))
                            vm.SelectedKyNangFilter = "Noi";
                        else
                            vm.SelectedKyNangFilter = kn;
                    }
                    if (targetLessonId.HasValue && targetLessonId.Value > 0)
                    {
                        var target = vm.DanhSachBaiHoc.FirstOrDefault(b => b.MaBaiHoc == targetLessonId.Value);
                        if (target != null)
                        {
                            vm.SelectedBaiHocInList = target;
                        }
                    }
                }
            };
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_parentWindow != null)
            {
                _parentWindow.LocationChanged -= Window_LocationOrSizeChanged;
                _parentWindow.SizeChanged -= Window_LocationOrSizeChanged;
                _parentWindow = null;
            }

            if (DataContext is NoiDungHocTapViewModel vm)
            {
                vm.DungAudio();
            }
        }

        private void SldAudioTimeline_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NoiDungHocTapViewModel vm)
            {
                vm.BatDauTuaAudio();
            }
        }

        private void SldAudioTimeline_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NoiDungHocTapViewModel vm && sender is Slider slider)
            {
                vm.KetThucTuaAudio(slider.Value);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow != null)
            {
                _parentWindow.LocationChanged -= Window_LocationOrSizeChanged;
                _parentWindow.LocationChanged += Window_LocationOrSizeChanged;
                _parentWindow.SizeChanged -= Window_LocationOrSizeChanged;
                _parentWindow.SizeChanged += Window_LocationOrSizeChanged;
            }

            if (DataContext is NoiDungHocTapViewModel vm)
            {
                vm.PropertyChanged -= ViewModel_PropertyChanged;
                vm.PropertyChanged += ViewModel_PropertyChanged;

                // 1. Luôn đồng bộ danh mục chủ đề & trình độ để có ngay chủ đề mới thêm từ Danh mục & Cấu trúc
                vm.CapNhatDanhSachChuDe();
                vm.CapNhatDanhSachTrinhDo();

                // 2. Luôn đồng bộ danh sách bài học mới nhất từ CSDL khi chuyển sang trang này
                // (loại bỏ các bài học/tệp đã bị xóa ở Danh mục & Cấu trúc, bảo toàn bản nháp nếu đang tạo mới)
                if (!vm.IsAiGeneratingSample && !vm.IsAiGeneratingQuestions)
                {
                    vm.LocDanhSach();
                }

                // 3. Khôi phục nội dung văn bản trong RichTextBox nếu ViewModel có nội dung mới hơn
                // (ví dụ AI vừa hoàn thành sinh nội dung trong lúc người dùng đang ở tab khác)
                string currentDocText = RichTextHelper.FlowDocumentToString(rtbEditorContent?.Document);
                if (!string.Equals(currentDocText?.Trim(), vm.SelectedBaiHoc?.NoiDung?.Trim()))
                {
                    LoadDocumentFromBaiHoc(vm.SelectedBaiHoc?.NoiDung);
                }
            }
        }

        private void Page_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is NoiDungHocTapViewModel oldVm)
            {
                oldVm.PropertyChanged -= ViewModel_PropertyChanged;
            }
            if (e.NewValue is NoiDungHocTapViewModel newVm)
            {
                newVm.PropertyChanged -= ViewModel_PropertyChanged;
                newVm.PropertyChanged += ViewModel_PropertyChanged;
                newVm.CapNhatDanhSachChuDe();
                newVm.CapNhatDanhSachTrinhDo();
                LoadDocumentFromBaiHoc(newVm.SelectedBaiHoc?.NoiDung);
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoiDungHocTapViewModel.SelectedBaiHoc))
            {
                if (DataContext is NoiDungHocTapViewModel vm)
                {
                    LoadDocumentFromBaiHoc(vm.SelectedBaiHoc?.NoiDung);
                }
            }
        }

        private void LoadDocumentFromBaiHoc(string? content)
        {
            if (rtbEditorContent == null) return;
            _isInternalChange = true;
            try
            {
                rtbEditorContent.Document = RichTextHelper.StringToFlowDocument(content);
                if (pnlPlaceholder != null)
                {
                    pnlPlaceholder.Visibility = RichTextHelper.IsDocumentEmpty(rtbEditorContent) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            finally
            {
                _isInternalChange = false;
                UpdateToolbarStates();
            }
        }

        private void RtbEditorContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pnlPlaceholder != null && rtbEditorContent != null)
            {
                pnlPlaceholder.Visibility = RichTextHelper.IsDocumentEmpty(rtbEditorContent) ? Visibility.Visible : Visibility.Collapsed;
            }

            if (!_isInternalChange && DataContext is NoiDungHocTapViewModel vm && vm.SelectedBaiHoc != null && rtbEditorContent != null)
            {
                vm.SelectedBaiHoc.NoiDung = RichTextHelper.FlowDocumentToString(rtbEditorContent.Document);
            }
        }

        // ==========================================
        // TOOLBAR & MENU ACTIONS (CHUẨN MICROSOFT WORD)
        // ==========================================

        private void BtnInsertElement_Click(object sender, RoutedEventArgs e)
        {
            if (popupInsertBlock != null)
            {
                popupInsertBlock.IsOpen = !popupInsertBlock.IsOpen;
            }
        }

        private void BtnOpenEmojiPicker_Click(object sender, RoutedEventArgs e)
        {
            if (popupInsertBlock != null) popupInsertBlock.IsOpen = false;
            if (popupEmojiPalette != null) popupEmojiPalette.IsOpen = true;
        }

        private void EmojiSelected_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Content is string emoji)
            {
                InsertTextAtCursor(emoji + " ");
                if (popupEmojiPalette != null) popupEmojiPalette.IsOpen = false;
            }
        }

        // --- CÁC TIÊU ĐỀ HEADING TO IN ĐẬM NHƯ WORD THẬT ---

        private void BtnHeading1_Click(object sender, RoutedEventArgs e)
        {
            ApplyHeading(20, FontWeights.Bold, "Tiêu đề cấp 1");
        }

        private void BtnHeading2_Click(object sender, RoutedEventArgs e)
        {
            ApplyHeading(17, FontWeights.Bold, "Tiêu đề cấp 2");
        }

        private void BtnHeading3_Click(object sender, RoutedEventArgs e)
        {
            ApplyHeading(15, FontWeights.Bold, "Tiêu đề cấp 3");
        }

        private void BtnBodyText_Click(object sender, RoutedEventArgs e)
        {
            ApplyHeading(13.5, FontWeights.Normal, "Nội dung văn bản chi tiết...");
        }

        private void ApplyHeading(double fontSize, FontWeight fontWeight, string defaultText)
        {
            if (rtbEditorContent == null) return;
            CloseInsertPopup();

            TextSelection selection = rtbEditorContent.Selection;
            if (selection != null)
            {
                if (!selection.IsEmpty)
                {
                    // Vùng bôi đen: Biến thành heading to in đậm
                    selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
                    selection.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
                    Paragraph? p = selection.Start.Paragraph;
                    if (p != null)
                    {
                        p.FontSize = fontSize;
                        p.FontWeight = fontWeight;
                    }
                }
                else
                {
                    // Không bôi đen: Áp dụng vào đoạn văn bản con trỏ đang đứng
                    Paragraph? curParagraph = selection.Start.Paragraph;
                    if (curParagraph != null)
                    {
                        TextRange pRange = new TextRange(curParagraph.ContentStart, curParagraph.ContentEnd);
                        string lineText = pRange.Text.Trim();
                        if (string.IsNullOrEmpty(lineText))
                        {
                            // Dòng trống: Chèn defaultText vào và bôi đen để gõ đè ngay
                            curParagraph.FontSize = fontSize;
                            curParagraph.FontWeight = fontWeight;
                            curParagraph.Inlines.Clear();
                            curParagraph.Inlines.Add(new Run(defaultText) { FontSize = fontSize, FontWeight = fontWeight });
                            rtbEditorContent.Selection.Select(curParagraph.ContentStart, curParagraph.ContentEnd);
                        }
                        else
                        {
                            // Dòng đã có chữ: Biến cả dòng thành Heading to in đậm
                            curParagraph.FontSize = fontSize;
                            curParagraph.FontWeight = fontWeight;
                            foreach (var inline in curParagraph.Inlines)
                            {
                                inline.FontSize = fontSize;
                                inline.FontWeight = fontWeight;
                            }
                        }
                    }
                    else
                    {
                        var newP = new Paragraph(new Run(defaultText))
                        {
                            FontSize = fontSize,
                            FontWeight = fontWeight
                        };
                        rtbEditorContent.Document.Blocks.Add(newP);
                        rtbEditorContent.Selection.Select(newP.ContentStart, newP.ContentEnd);
                    }
                }
            }
            rtbEditorContent.Focus();
        }

        private void BtnParagraph_Click(object sender, RoutedEventArgs e)
        {
            CloseInsertPopup();
            EditingCommands.EnterParagraphBreak.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        private void BtnLink_Click(object sender, RoutedEventArgs e)
        {
            InsertTextAtCursor(" [Liên kết tài liệu](https://example.com) ");
            CloseInsertPopup();
        }

        private void BtnFileUpload_Click(object sender, RoutedEventArgs e)
        {
            CloseInsertPopup();
            var dlg = new OpenFileDialog
            {
                Filter = "Tài liệu học tập (*.pdf;*.docx;*.txt;*.mp3)|*.pdf;*.docx;*.txt;*.mp3|Tất cả tệp (*.*)|*.*",
                Title = "Chọn tệp đính kèm bài giảng"
            };
            if (dlg.ShowDialog() == true)
            {
                string fileName = Path.GetFileName(dlg.FileName);
                InsertTextAtCursor($"📎 [Tài liệu đính kèm: {fileName}]\n");
            }
        }

        private void BtnImageUpload_Click(object sender, RoutedEventArgs e)
        {
            CloseInsertPopup();
            var dlg = new OpenFileDialog
            {
                Filter = "Hình ảnh minh họa (*.png;*.jpg;*.jpeg;*.webp)|*.png;*.jpg;*.jpeg;*.webp",
                Title = "Chọn hình ảnh chèn vào giáo trình"
            };
            if (dlg.ShowDialog() == true)
            {
                string fileName = Path.GetFileName(dlg.FileName);
                InsertTextAtCursor($"🖼 [Hình ảnh: {fileName}]\n");
            }
        }

        private void BtnAddHint_Click(object sender, RoutedEventArgs e)
        {
            InsertTextAtCursor("💡 Hint: [Nhập mẹo ghi nhớ hoặc lưu ý quan trọng tại đây...]\n");
            CloseInsertPopup();
        }

        private void CloseInsertPopup()
        {
            if (popupInsertBlock != null) popupInsertBlock.IsOpen = false;
            if (popupEmojiPalette != null) popupEmojiPalette.IsOpen = false;
        }

        // ==========================================
        // RÀNG BUỘC CUỘN & NEO DÍNH POPUP NHƯ COMBOBOX
        // ==========================================

        private void EditorScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateEditorPopups();
        }

        private void Window_LocationOrSizeChanged(object? sender, EventArgs e)
        {
            UpdateEditorPopups();
        }

        private void UpdateEditorPopups()
        {
            UpdateSinglePopup(popupInsertBlock, btnInsertElement);
            UpdateSinglePopup(popupEmojiPalette, btnInsertElement);
        }

        private void UpdateSinglePopup(Popup? popup, FrameworkElement? target)
        {
            if (popup == null || !popup.IsOpen || target == null || editorScrollViewer == null) return;

            try
            {
                // 1. Kiểm tra vị trí target đối với khung nhìn viewport của ScrollViewer
                GeneralTransform transform = target.TransformToAncestor(editorScrollViewer);
                Point pt = transform.Transform(new Point(0, 0));
                Rect targetBounds = new Rect(pt, target.RenderSize);
                Rect viewport = new Rect(0, 0, editorScrollViewer.ActualWidth, editorScrollViewer.ActualHeight);

                // Nếu nút '+' bị cuộn ra ngoài khung nhìn -> Tự động đóng popup để không bị lơ lửng
                if (!viewport.IntersectsWith(targetBounds))
                {
                    popup.IsOpen = false;
                    return;
                }

                // 2. Ép WPF Popup cập nhật lại vị trí screen tọa độ theo target (dính chặt vào button)
                double h = popup.HorizontalOffset;
                popup.HorizontalOffset = h + 0.0001;
                popup.HorizontalOffset = h;
            }
            catch
            {
                popup.IsOpen = false;
            }
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        // --- CẬP NHẬT TRẠNG THÁI ACTIVE TRỰC QUAN CHO CÁC NÚT ĐỊNH DẠNG (U, I, B, CĂN LỀ, LIST) ---

        private void RtbEditorContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateToolbarStates();
        }

        private void UpdateToolbarStates()
        {
            if (rtbEditorContent == null) return;

            try
            {
                // 1. In đậm (Bold)
                object weight = rtbEditorContent.Selection.GetPropertyValue(TextElement.FontWeightProperty);
                if (btnBold != null)
                {
                    btnBold.IsChecked = (weight != DependencyProperty.UnsetValue) && weight is FontWeight fw &&
                                        (fw == FontWeights.Bold || fw == FontWeights.SemiBold || fw == FontWeights.ExtraBold || fw == FontWeights.Black);
                }

                // 2. In nghiêng (Italic)
                object style = rtbEditorContent.Selection.GetPropertyValue(TextElement.FontStyleProperty);
                if (btnItalic != null)
                {
                    btnItalic.IsChecked = (style != DependencyProperty.UnsetValue) && style is FontStyle fs &&
                                          (fs == FontStyles.Italic || fs == FontStyles.Oblique);
                }

                // 3. Gạch chân (Underline)
                object deco = rtbEditorContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                if (btnUnderline != null)
                {
                    bool isUnderline = false;
                    if (deco != DependencyProperty.UnsetValue && deco is TextDecorationCollection tdc)
                    {
                        foreach (var d in tdc)
                        {
                            if (d.Location == TextDecorationLocation.Underline)
                            {
                                isUnderline = true;
                                break;
                            }
                        }
                    }
                    btnUnderline.IsChecked = isUnderline;
                }

                // 4. Căn lề (Left, Center, Right)
                TextAlignment currentAlign = TextAlignment.Left;
                object align = rtbEditorContent.Selection.GetPropertyValue(Block.TextAlignmentProperty);
                if (align != DependencyProperty.UnsetValue && align is TextAlignment ta)
                {
                    currentAlign = ta;
                }
                else
                {
                    var para = rtbEditorContent.Selection?.Start?.Paragraph;
                    if (para != null) currentAlign = para.TextAlignment;
                }

                if (btnAlignLeft != null) btnAlignLeft.IsChecked = (currentAlign == TextAlignment.Left);
                if (btnAlignCenter != null) btnAlignCenter.IsChecked = (currentAlign == TextAlignment.Center);
                if (btnAlignRight != null) btnAlignRight.IsChecked = (currentAlign == TextAlignment.Right);

                // 5. Danh sách số và gạch đầu dòng (Lists)
                bool isNumbered = false;
                bool isBullet = false;
                var start = rtbEditorContent.Selection?.Start;
                var curPara = start?.Paragraph;
                if (curPara?.Parent is ListItem listItem && listItem.List != null)
                {
                    var marker = listItem.List.MarkerStyle;
                    isNumbered = (marker == TextMarkerStyle.Decimal || marker == TextMarkerStyle.LowerLatin ||
                                  marker == TextMarkerStyle.UpperLatin || marker == TextMarkerStyle.LowerRoman ||
                                  marker == TextMarkerStyle.UpperRoman);
                    isBullet = (marker == TextMarkerStyle.Disc || marker == TextMarkerStyle.Circle ||
                                marker == TextMarkerStyle.Square || marker == TextMarkerStyle.Box);
                }
                if (btnNumberedList != null) btnNumberedList.IsChecked = isNumbered;
                if (btnBulletList != null) btnBulletList.IsChecked = isBullet;
            }
            catch
            {
                // Bỏ qua lỗi đánh giá thuộc tính tạm thời khi người dùng đang nhập nhanh
            }
        }

        // --- DANH SÁCH SỐ & GẠCH ĐẦU DÒNG CHUẨN WORD ---

        private void BtnNumberedList_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleNumbering.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        private void BtnBulletList_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleBullets.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        // --- CĂN LỀ CHỈ ÁP DỤNG RIÊNG CHO DÒNG/ĐOẠN CON TRỎ ĐỨNG HOẶC TEXT BÔI ĐEN ---

        private void BtnAlignLeft_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.AlignLeft.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        private void BtnAlignCenter_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.AlignCenter.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        private void BtnAlignRight_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.AlignRight.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        // --- U, I, B TRỰC QUAN NHƯ WORD (KHÔNG HIỆN THẺ **, *, <u> THÔ) ---

        private void BtnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleUnderline.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        private void BtnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleItalic.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        private void BtnBold_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleBold.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
            UpdateToolbarStates();
        }

        private void InsertTextAtCursor(string text)
        {
            if (rtbEditorContent == null) return;
            rtbEditorContent.Selection.Text = text;
            rtbEditorContent.CaretPosition = rtbEditorContent.Selection.End;
            rtbEditorContent.Focus();
        }

        // --- XỬ LÝ PHÍM: TAB ĐỂ XUỐNG DÒNG, GÕ "/" ĐỂ GỌI AI ---

        private void RtbEditorContent_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Phím Tab: Xuống dòng mới (theo yêu cầu người dùng)
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                EditingCommands.EnterParagraphBreak.Execute(null, rtbEditorContent);
                return;
            }

            // Phím /: Tự động sinh nội dung mẫu bằng AI khi tài liệu đang trống và đã có tiêu đề bài học
            if (e.Key == Key.OemQuestion || e.Key == Key.Divide)
            {
                if (RichTextHelper.IsDocumentEmpty(rtbEditorContent))
                {
                    if (DataContext is NoiDungHocTapViewModel vm)
                    {
                        if (vm.CanTaoBangAI)
                        {
                            e.Handled = true;
                            vm.SinhNoiDungMauAICommand.Execute(null);
                        }
                        else
                        {
                            // Chưa nhập tiêu đề: Chặn tự động sinh mẫu và nhắc nhở giảng viên nhập tiêu đề
                            e.Handled = true;
                            vm.ThongBaoLoi = "Vui lòng nhập tiêu đề bài học trước khi nhấn '/' để tạo nội dung mẫu bằng AI.";
                        }
                    }
                }
            }
        }

        private async void BtnTaoCauHoiAI_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is NoiDungHocTapViewModel vm)
            {
                if (!vm.CanTaoBangAI) return;
                if (vm.SelectedBaiHoc == null) return;
                if (string.IsNullOrWhiteSpace(vm.SelectedBaiHoc.TieuDe))
                {
                    vm.ThongBaoLoi = "Vui lòng nhập tiêu đề bài học trước khi tạo câu hỏi bằng AI.";
                    return;
                }

                // Kiểm tra bài học còn tồn tại trong hệ thống hay không (tránh trường hợp bài đã bị xóa từ Danh mục & Cấu trúc)
                if (vm.SelectedBaiHoc.MaBaiHoc > 0 && !vm.DanhSachBaiHoc.Any(b => b.MaBaiHoc == vm.SelectedBaiHoc.MaBaiHoc))
                {
                    ThongBaoDialog.CanhBao(Window.GetWindow(this), "Bài học này không còn tồn tại trong cơ sở dữ liệu (có thể đã bị xóa từ mục Danh mục & Cấu trúc).", "Bài học không tồn tại");
                    vm.LocDanhSach();
                    return;
                }

                // Ràng buộc đối với bài học Luyện nghe (Listening): Bắt buộc phải có tệp Audio
                if (vm.IsKyNangNghe && !vm.HasAudioFile)
                {
                    bool xacNhan = ThongBaoDialog.XacNhan(
                        Window.GetWindow(this),
                        "Bài học này thuộc kỹ năng Luyện nghe (Listening) nhưng hiện tại chưa có tệp âm thanh (Audio).\n\n" +
                        "Để học viên có thể luyện nghe và hệ thống phát audio cho các câu hỏi trong bài học cũng như trong ngân hàng câu hỏi, bài học bắt buộc phải có tệp Audio.\n\n" +
                        "Bạn có muốn chọn tệp Audio (.mp3, .wav, .m4a) cho bài học này ngay bây giờ không?",
                        "Yêu cầu tệp âm thanh Audio");

                    if (xacNhan)
                    {
                        vm.ChonAudioCommand.Execute(null);
                        if (!vm.HasAudioFile)
                        {
                            vm.ThongBaoLoi = "Vui lòng đính kèm tệp âm thanh (Audio) cho bài học nghe hiểu trước khi tạo câu hỏi bằng AI.";
                            return;
                        }
                    }
                    else
                    {
                        vm.ThongBaoLoi = "Bài học Luyện nghe bắt buộc phải có tệp Audio để tạo câu hỏi nghe hiểu.";
                        ThongBaoDialog.CanhBao(
                            Window.GetWindow(this),
                            "Bài học Luyện nghe bắt buộc phải có tệp Audio để tạo câu hỏi nghe hiểu và phát âm thanh trong ngân hàng câu hỏi.",
                            "Chưa có tệp Audio");
                        return;
                    }
                }

                vm.IsAiGeneratingQuestions = true;
                vm.AiQuestionsStatusMessage = "AI đang phân tích bài học và tạo câu hỏi...";

                try
                {
                    await Views.Common.AICauHoiUIHelper.TaoVaDuyetBoCauHoiAsync(
                        Window.GetWindow(this),
                        vm.SelectedBaiHoc,
                        nguonTao: "AI_BaiHoc",
                        soLuong: 10,
                        forceRecreate: true,
                        onStatusUpdate: msg => vm.AiQuestionsStatusMessage = msg);
                }
                finally
                {
                    vm.IsAiGeneratingQuestions = false;
                }
            }
        }
    
        private void BtnMoQuanLyAudioTranscript_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is NoiDungHocTapViewModel vm && vm.SelectedBaiHoc != null)
            {
                var dlg = new QuanLyAudioTranscriptDialog(vm.SelectedBaiHoc)
                {
                    Owner = Window.GetWindow(this)
                };
                dlg.ShowDialog();
                if (!string.IsNullOrEmpty(vm.SelectedBaiHoc.AudioPath))
                {
                    vm.SelectedBaiHoc.DuongDanAudio = vm.SelectedBaiHoc.AudioPath;
                }
            }
        }
}
}
