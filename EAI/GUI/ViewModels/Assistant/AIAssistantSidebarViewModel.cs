using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BUS;
using BUS.AI.Configuration;
using BUS.Assistant;
using BUS.Assistant.Tools;
using DTO.Assistant;
using GUI.ViewModels.Common;

namespace GUI.ViewModels.Assistant
{
    public class AssistantChatMessageItem : ViewModelBase
    {
        private string _content = string.Empty;
        private bool _isActionDone = false;
        private bool _isActionConfirmed = false;
        private bool _isActionRejected = false;
        private string _actionStatusMessage = string.Empty;

        public long MessageId { get; set; }
        public int ConversationId { get; set; }
        public string SenderRole { get; set; } = "user"; // "user" hoặc "assistant"
        
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedAtHienThi => CreatedAt.ToString("HH:mm");

        public bool IsUser => string.Equals(SenderRole, "user", StringComparison.OrdinalIgnoreCase);
        public bool IsAssistant => !IsUser;

        public string? CardType { get; set; }
        public string? CardPayloadJson { get; set; }
        public bool HasCard => !string.IsNullOrWhiteSpace(CardPayloadJson);

        public bool RequiresConfirmation { get; set; } = false;
        public string? ConfirmationToken { get; set; }

        public bool CanConfirm => RequiresConfirmation && !IsActionDone && !string.IsNullOrWhiteSpace(ConfirmationToken);

        public bool IsActionDone
        {
            get => _isActionDone;
            set
            {
                if (SetProperty(ref _isActionDone, value))
                {
                    OnPropertyChanged(nameof(CanConfirm));
                }
            }
        }

        public bool IsActionConfirmed
        {
            get => _isActionConfirmed;
            set => SetProperty(ref _isActionConfirmed, value);
        }

        public bool IsActionRejected
        {
            get => _isActionRejected;
            set => SetProperty(ref _isActionRejected, value);
        }

        public string ActionStatusMessage
        {
            get => _actionStatusMessage;
            set => SetProperty(ref _actionStatusMessage, value);
        }

        public string? ExecutedToolName { get; set; }
        public string? ModelUsed { get; set; }
        public long LatencyMs { get; set; }

        private QuestionPreviewCardPayloadDTO? _questionPreview;
        public QuestionPreviewCardPayloadDTO? QuestionPreview
        {
            get
            {
                if (_questionPreview == null && !string.IsNullOrWhiteSpace(CardPayloadJson) &&
                    (CardType == "question_preview_card" || CardPayloadJson.Contains("\"Questions\"") || CardPayloadJson.Contains("\"questions\"")))
                {
                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        _questionPreview = JsonSerializer.Deserialize<QuestionPreviewCardPayloadDTO>(CardPayloadJson, options);
                    }
                    catch
                    {
                        _questionPreview = null;
                    }
                }
                return _questionPreview;
            }
        }
    }

    public class AIAssistantSidebarViewModel : ViewModelBase
    {
        private static readonly Lazy<AIAssistantSidebarViewModel> _instance = new(() => new AIAssistantSidebarViewModel());
        public static AIAssistantSidebarViewModel Instance => _instance.Value;

        private readonly AssistantOrchestrator _orchestrator;
        private readonly AssistantConversationService _conversationService;
        private readonly AssistantContextProvider _contextProvider;

        private AIAssistantConversationDTO? _cuocTroChuyenHienTai;
        private string _tuKhoaTimKiem = string.Empty;
        private string _noiDungNhap = string.Empty;
        private bool _dangXuLy = false;
        private string _trangThaiText = "Đang sẵn sàng";
        private string _tenModelHienThi = "AI Assistant";
        private CancellationTokenSource? _cts;

        public ObservableCollection<AIAssistantConversationDTO> DanhSachCuocTroChuyen { get; } = new();
        public ObservableCollection<AIAssistantConversationDTO> DanhSachCuocTroChuyenHienThi { get; } = new();
        public ObservableCollection<AssistantChatMessageItem> DanhSachTinNhan { get; } = new();

        public AIAssistantConversationDTO? CuocTroChuyenHienTai
        {
            get => _cuocTroChuyenHienTai;
            set
            {
                if (SetProperty(ref _cuocTroChuyenHienTai, value))
                {
                    OnPropertyChanged(nameof(TieuDeCuocTroChuyenHienTai));
                    OnPropertyChanged(nameof(CoCuocTroChuyenHienTai));
                }
            }
        }

        public string TieuDeCuocTroChuyenHienTai => _cuocTroChuyenHienTai?.TieuDe ?? "Cuộc trò chuyện mới";
        public bool CoCuocTroChuyenHienTai => _cuocTroChuyenHienTai != null && _cuocTroChuyenHienTai.MaHoiThoai > 0;

        public string TuKhoaTimKiem
        {
            get => _tuKhoaTimKiem;
            set
            {
                if (SetProperty(ref _tuKhoaTimKiem, value))
                {
                    LocDanhSachCuocTroChuyen();
                }
            }
        }

        public string NoiDungNhap
        {
            get => _noiDungNhap;
            set => SetProperty(ref _noiDungNhap, value);
        }

        public bool DangXuLy
        {
            get => _dangXuLy;
            set
            {
                if (SetProperty(ref _dangXuLy, value))
                {
                    OnPropertyChanged(nameof(CoTheGui));
                    OnPropertyChanged(nameof(CoTheHuy));
                }
            }
        }

        public bool CoTheGui => !DangXuLy && !string.IsNullOrWhiteSpace(NoiDungNhap);
        public bool CoTheHuy => DangXuLy;

        public string TrangThaiText
        {
            get => _trangThaiText;
            set => SetProperty(ref _trangThaiText, value);
        }

        public string TenModelHienThi
        {
            get => _tenModelHienThi;
            set => SetProperty(ref _tenModelHienThi, value);
        }

        public bool CoTinNhan => DanhSachTinNhan.Count > 0;
        public bool KhongCoTinNhan => DanhSachTinNhan.Count == 0;

        public event Action? ScrollToBottomRequested;

        public AIAssistantSidebarViewModel()
        {
            _orchestrator = AssistantOrchestrator.Instance;
            _conversationService = AssistantConversationService.Instance;
            _contextProvider = AssistantContextProvider.Instance;

            CapNhatTenModelHienThi();
            _ = KhoiTaoBanDauAsync();
        }

        public void CapNhatTenModelHienThi()
        {
            try
            {
                var provider = AIConfigurationService.Instance.ActiveProvider;
                var model = AIConfigurationService.Instance.ActiveModel;
                TenModelHienThi = $"{provider} ({model})";
            }
            catch
            {
                TenModelHienThi = "EAI Assistant";
            }
        }

        public async Task KhoiTaoBanDauAsync()
        {
            CapNhatTenModelHienThi();
            await NapDanhSachCuocTroChuyenAsync();

            if (CuocTroChuyenHienTai == null)
            {
                if (DanhSachCuocTroChuyen.Count > 0)
                {
                    await ChonCuocTroChuyenAsync(DanhSachCuocTroChuyen[0]);
                }
                else
                {
                    await TaoCuocTroChuyenMoiAsync();
                }
            }
        }

        public async Task NapDanhSachCuocTroChuyenAsync()
        {
            try
            {
                int userId = SessionHelper.CurrentMaNguoiDung;
                if (userId <= 0) userId = 1; // Khách mặc định

                var list = await _conversationService.GetConversationsAsync(userId, null);
                
                DanhSachCuocTroChuyen.Clear();
                foreach (var item in list)
                {
                    DanhSachCuocTroChuyen.Add(item);
                }

                LocDanhSachCuocTroChuyen();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NapDanhSachCuocTroChuyenAsync] {ex.Message}");
            }
        }

        private void LocDanhSachCuocTroChuyen()
        {
            DanhSachCuocTroChuyenHienThi.Clear();
            string keyword = TuKhoaTimKiem?.Trim() ?? string.Empty;

            foreach (var conv in DanhSachCuocTroChuyen)
            {
                if (string.IsNullOrWhiteSpace(keyword) || 
                    conv.TieuDe.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    DanhSachCuocTroChuyenHienThi.Add(conv);
                }
            }
        }

        public async Task TaoCuocTroChuyenMoiAsync()
        {
            try
            {
                int userId = SessionHelper.CurrentMaNguoiDung;
                if (userId <= 0) userId = 1;

                var newConv = await _conversationService.CreateConversationAsync(userId, "Cuộc trò chuyện mới");
                if (newConv != null)
                {
                    DanhSachCuocTroChuyen.Insert(0, newConv);
                    LocDanhSachCuocTroChuyen();
                    await ChonCuocTroChuyenAsync(newConv);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TaoCuocTroChuyenMoiAsync] {ex.Message}");
            }
        }

        public async Task ChonCuocTroChuyenAsync(AIAssistantConversationDTO? conv)
        {
            if (conv == null) return;
            CuocTroChuyenHienTai = conv;
            DanhSachTinNhan.Clear();

            try
            {
                var messages = await _conversationService.GetMessagesAsync(conv.MaHoiThoai);
                foreach (var m in messages)
                {
                    var item = new AssistantChatMessageItem
                    {
                        MessageId = m.MaTinNhan,
                        ConversationId = m.MaHoiThoai,
                        SenderRole = m.VaiTro,
                        Content = m.NoiDung,
                        CreatedAt = m.ThoiGianGui,
                        CardType = m.CardPayloadJson != null ? "preview_card" : null,
                        CardPayloadJson = m.CardPayloadJson
                    };
                    DanhSachTinNhan.Add(item);
                }

                OnPropertyChanged(nameof(CoTinNhan));
                OnPropertyChanged(nameof(KhongCoTinNhan));
                ScrollToBottomRequested?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ChonCuocTroChuyenAsync] {ex.Message}");
            }
        }

        public async Task GuiTinNhanAsync(string? tinNhanTuyChon = null)
        {
            string messageToSend = !string.IsNullOrWhiteSpace(tinNhanTuyChon) 
                ? tinNhanTuyChon.Trim() 
                : NoiDungNhap?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(messageToSend) || DangXuLy) return;

            if (CuocTroChuyenHienTai == null)
            {
                await TaoCuocTroChuyenMoiAsync();
            }

            if (CuocTroChuyenHienTai == null) return;

            // Xóa ô nhập liệu
            if (string.IsNullOrWhiteSpace(tinNhanTuyChon))
            {
                NoiDungNhap = string.Empty;
            }

            // Thêm tin nhắn của User vào danh sách UI
            var userMsgItem = new AssistantChatMessageItem
            {
                ConversationId = CuocTroChuyenHienTai.MaHoiThoai,
                SenderRole = "user",
                Content = messageToSend,
                CreatedAt = DateTime.Now
            };
            DanhSachTinNhan.Add(userMsgItem);
            OnPropertyChanged(nameof(CoTinNhan));
            OnPropertyChanged(nameof(KhongCoTinNhan));
            ScrollToBottomRequested?.Invoke();

            DangXuLy = true;
            TrangThaiText = "Đang xử lý & phân tích...";
            _cts = new CancellationTokenSource();

            try
            {
                var context = _contextProvider.GetCurrentContext();
                var response = await _orchestrator.ProcessUserMessageAsync(
                    CuocTroChuyenHienTai.MaHoiThoai, 
                    messageToSend, 
                    context, 
                    _cts.Token);

                var aiMsgItem = new AssistantChatMessageItem
                {
                    ConversationId = CuocTroChuyenHienTai.MaHoiThoai,
                    SenderRole = "assistant",
                    Content = response.Message,
                    CreatedAt = DateTime.Now,
                    CardType = response.CardType,
                    CardPayloadJson = response.CardPayloadJson,
                    RequiresConfirmation = response.RequiresConfirmation,
                    ConfirmationToken = response.ConfirmationToken,
                    ExecutedToolName = response.ExecutedToolName,
                    ModelUsed = response.ModelUsed,
                    LatencyMs = response.LatencyMs
                };

                DanhSachTinNhan.Add(aiMsgItem);
                TrangThaiText = "Đang sẵn sàng";
            }
            catch (OperationCanceledException)
            {
                var cancelMsgItem = new AssistantChatMessageItem
                {
                    ConversationId = CuocTroChuyenHienTai.MaHoiThoai,
                    SenderRole = "assistant",
                    Content = "⏹ Đã dừng phản hồi theo yêu cầu của bạn.",
                    CreatedAt = DateTime.Now
                };
                DanhSachTinNhan.Add(cancelMsgItem);
                TrangThaiText = "Đã dừng phản hồi";
            }
            catch (Exception ex)
            {
                var errorMsgItem = new AssistantChatMessageItem
                {
                    ConversationId = CuocTroChuyenHienTai.MaHoiThoai,
                    SenderRole = "assistant",
                    Content = $"❌ Không thể hoàn tất yêu cầu: {ex.Message}",
                    CreatedAt = DateTime.Now
                };
                DanhSachTinNhan.Add(errorMsgItem);
                TrangThaiText = "Lỗi xử lý";
            }
            finally
            {
                DangXuLy = false;
                _cts?.Dispose();
                _cts = null;
                OnPropertyChanged(nameof(CoTinNhan));
                OnPropertyChanged(nameof(KhongCoTinNhan));
                ScrollToBottomRequested?.Invoke();

                // Cập nhật lại danh sách tiêu đề hội thoại (nếu có tự động đổi tên)
                _ = NapDanhSachCuocTroChuyenAsync();
            }
        }

        public void HuyPhanHoi()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                TrangThaiText = "Đang dừng...";
            }
        }

        public async Task XacNhanHanhDongGhiAsync(AssistantChatMessageItem msg)
        {
            if (msg == null || string.IsNullOrWhiteSpace(msg.ConfirmationToken) || msg.IsActionDone) return;

            DangXuLy = true;
            TrangThaiText = "Đang lưu dữ liệu vào hệ thống...";

            try
            {
                var result = await _orchestrator.ConfirmActionAsync(msg.ConfirmationToken);
                msg.IsActionDone = true;
                msg.IsActionConfirmed = true;
                msg.ActionStatusMessage = result.Message;

                var confirmResultItem = new AssistantChatMessageItem
                {
                    ConversationId = msg.ConversationId,
                    SenderRole = "assistant",
                    Content = $"✓ **Xác nhận hoàn tất**: {result.Message}",
                    CreatedAt = DateTime.Now,
                    ExecutedToolName = msg.ExecutedToolName
                };
                DanhSachTinNhan.Add(confirmResultItem);
                TrangThaiText = "Lưu thành công";
            }
            catch (Exception ex)
            {
                msg.ActionStatusMessage = $"Lỗi: {ex.Message}";
                TrangThaiText = "Lỗi xác nhận";
            }
            finally
            {
                DangXuLy = false;
                ScrollToBottomRequested?.Invoke();
            }
        }

        public void TuChoiHanhDongGhi(AssistantChatMessageItem msg)
        {
            if (msg == null || string.IsNullOrWhiteSpace(msg.ConfirmationToken) || msg.IsActionDone) return;

            _orchestrator.CancelAction(msg.ConfirmationToken);
            msg.IsActionDone = true;
            msg.IsActionRejected = true;
            msg.ActionStatusMessage = "Bạn đã hủy thao tác này.";

            var cancelItem = new AssistantChatMessageItem
            {
                ConversationId = msg.ConversationId,
                SenderRole = "assistant",
                Content = "✕ Thao tác lưu câu hỏi đã được hủy bỏ.",
                CreatedAt = DateTime.Now
            };
            DanhSachTinNhan.Add(cancelItem);
            ScrollToBottomRequested?.Invoke();
        }

        public async Task XoaCuocTroChuyenAsync(AIAssistantConversationDTO conv)
        {
            if (conv == null) return;

            var confirm = GUI.Views.Common.ThongBaoDialog.XacNhan(
                $"Bạn có chắc chắn muốn xóa cuộc trò chuyện '{conv.TieuDe}' không?\nToàn bộ tin nhắn sẽ bị xóa vĩnh viễn.",
                "Xác nhận xóa cuộc trò chuyện");

            if (!confirm) return;

            try
            {
                int userId = SessionHelper.CurrentMaNguoiDung;
                if (userId <= 0) userId = 1;

                await _conversationService.DeleteConversationAsync(conv.MaHoiThoai, userId);
                DanhSachCuocTroChuyen.Remove(conv);
                LocDanhSachCuocTroChuyen();

                if (CuocTroChuyenHienTai?.MaHoiThoai == conv.MaHoiThoai)
                {
                    if (DanhSachCuocTroChuyenHienThi.Count > 0)
                    {
                        await ChonCuocTroChuyenAsync(DanhSachCuocTroChuyenHienThi[0]);
                    }
                    else
                    {
                        await TaoCuocTroChuyenMoiAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                GUI.Views.Common.ThongBaoDialog.Loi($"Không thể xóa cuộc trò chuyện: {ex.Message}", "Lỗi");
            }
        }

        public async Task DoiTenCuocTroChuyenAsync(AIAssistantConversationDTO conv, string newTitle)
        {
            if (conv == null || string.IsNullOrWhiteSpace(newTitle)) return;

            try
            {
                int userId = SessionHelper.CurrentMaNguoiDung;
                if (userId <= 0) userId = 1;

                await _conversationService.RenameConversationAsync(conv.MaHoiThoai, userId, newTitle.Trim());
                conv.TieuDe = newTitle.Trim();
                LocDanhSachCuocTroChuyen();
                if (CuocTroChuyenHienTai?.MaHoiThoai == conv.MaHoiThoai)
                {
                    OnPropertyChanged(nameof(TieuDeCuocTroChuyenHienTai));
                }
            }
            catch (Exception ex)
            {
                GUI.Views.Common.ThongBaoDialog.Loi($"Không thể đổi tên cuộc trò chuyện: {ex.Message}", "Lỗi");
            }
        }
    }
}
