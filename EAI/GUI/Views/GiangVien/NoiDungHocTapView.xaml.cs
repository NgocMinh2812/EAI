using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Win32;
using GUI.ViewModels.GiangVien;
using GUI.Views.Common;

namespace GUI.Views.GiangVien
{
    public partial class NoiDungHocTapView : Page
    {
        private bool _isInternalChange = false;

        public NoiDungHocTapView()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
            DataContextChanged += Page_DataContextChanged;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NoiDungHocTapViewModel vm)
            {
                vm.PropertyChanged -= ViewModel_PropertyChanged;
                vm.PropertyChanged += ViewModel_PropertyChanged;

                // 1. Luôn đồng bộ danh mục chủ đề & trình độ để có ngay chủ đề mới thêm từ Danh mục & Cấu trúc
                vm.CapNhatDanhSachChuDe();
                vm.CapNhatDanhSachTrinhDo();

                // 2. Không gọi LocDanhSach() nếu người dùng đang tạo bài mới hoặc đang sinh AI
                if (!vm.IsCreatingNewLesson && !vm.IsAiGeneratingSample && !vm.IsAiGeneratingQuestions)
                {
                    if (vm.DanhSachBaiHoc.Count == 0)
                    {
                        vm.LocDanhSach();
                    }
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
        }

        // --- DANH SÁCH SỐ & GẠCH ĐẦU DÒNG CHUẨN WORD ---

        private void BtnNumberedList_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleNumbering.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        private void BtnBulletList_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleBullets.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        // --- CĂN LỀ CHỈ ÁP DỤNG RIÊNG CHO DÒNG/ĐOẠN CON TRỎ ĐỨNG HOẶC TEXT BÔI ĐEN ---

        private void BtnAlignLeft_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.AlignLeft.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        private void BtnAlignCenter_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.AlignCenter.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        private void BtnAlignRight_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.AlignRight.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        // --- U, I, B TRỰC QUAN NHƯ WORD (KHÔNG HIỆN THẺ **, *, <u> THÔ) ---

        private void BtnUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleUnderline.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        private void BtnItalic_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleItalic.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
        }

        private void BtnBold_Click(object sender, RoutedEventArgs e)
        {
            if (rtbEditorContent == null) return;
            EditingCommands.ToggleBold.Execute(null, rtbEditorContent);
            rtbEditorContent.Focus();
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

                vm.IsAiGeneratingQuestions = true;
                vm.AiQuestionsStatusMessage = "AI đang phân tích bài học và tạo câu hỏi...";

                try
                {
                    var questions = await vm.LayHoacTaoCauHoiAIAsync();
                    if (questions != null && questions.Count > 0)
                    {
                        var duyetVm = new DuyetBoCauHoiAIViewModel();
                        duyetVm.NapDuLieu(
                            tenNguon: vm.SelectedBaiHoc.TieuDe ?? "Bài học tiếng Anh",
                            nguonTao: "AI_BaiHoc",
                            cauHois: questions,
                            maBaiHoc: vm.SelectedBaiHoc.MaBaiHoc,
                            maChuDe: vm.SelectedBaiHoc.MaChuDe,
                            maTrinhDo: vm.SelectedBaiHoc.MaTrinhDoId,
                            capDo: vm.SelectedBaiHoc.MaTrinhDo ?? "B1",
                            kyNang: vm.SelectedBaiHoc.LoaiBaiHoc ?? "Doc",
                            audioPath: vm.SelectedBaiHoc.AudioPath ?? vm.SelectedBaiHoc.DuongDanAudio
                        );

                        var dlg = new DuyetBoCauHoiAIDialog(duyetVm);
                        ModalHelper.ShowDialogWithDim(dlg, Window.GetWindow(this));

                        if (duyetVm.DaLuuThanhCong)
                        {
                            ThongBaoDialog.ThanhCong(Window.GetWindow(this), $"Bộ câu hỏi cho bài học \"{vm.SelectedBaiHoc.TieuDe}\" đã được lưu thành công vào Ngân hàng câu hỏi!", "Thành công");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ThongBaoDialog.Loi(Window.GetWindow(this), $"Lỗi khi tạo câu hỏi bằng AI:\n{ex.Message}", "Lỗi tạo câu hỏi");
                }
                finally
                {
                    vm.IsAiGeneratingQuestions = false;
                }
            }
        }
    }
}
