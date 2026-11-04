using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BUS;
using BUS.AI;
using BUS.AI.Configuration;
using BUS.AI.Models;
using BUS.AI.Providers;
using GUI.ViewModels.Common;
using GUI.Views.Common;

namespace GUI.ViewModels.GiangVien
{
    public class GiamSatHeThongViewModel : ViewModelBase
    {
        private readonly DashboardBUS _dashboardBUS = new();
        private bool _isRefreshing;
        private string _statusMessage = string.Empty;
        private string _lastRefreshTime = string.Empty;

        // Thống kê tổng quan
        private string _systemHealthText = "Hệ thống hoạt động ổn định";
        private string _systemHealthBadgeColor = "#15803D";
        private string _systemHealthBadgeBg = "#DCFCE7";
        private string _totalRequestsText = "0";
        private string _averageLatencyText = "0 ms";
        private string _databaseStatusText = "Đang kiểm tra...";
        private string _databaseLatencyText = "Chờ kết nối";
        private string _errorRateText = "0.0%";

        // Thống kê phân hệ AI
        private string _aiTutorUsageText = "0 lượt";
        private string _aiWritingUsageText = "0 bài";
        private string _lastAiUsageTimeText = "Chưa ghi nhận";

        // Trạng thái từng Provider
        private AIHealthStatus _geminiStatus = new() { ProviderName = "Google Gemini", ProviderType = AIProviderType.Gemini, Health = "Unavailable" };
        private AIHealthStatus _groqStatus = new() { ProviderName = "Groq Cloud", ProviderType = AIProviderType.Groq, Health = "Unavailable" };
        private AIHealthStatus _xkioStatus = new() { ProviderName = "xKiro AI Gateway", ProviderType = AIProviderType.Xkio, Health = "Unavailable" };

        // Masked keys
        private string _geminiMaskedKey = "Chưa cấu hình";
        private string _groqMaskedKey = "Chưa cấu hình";
        private string _xkioMaskedKey = "Chưa cấu hình";

        // Cấu hình chuyển đổi mô hình (Model Switching)
        private string _selectedProvider = "Google Gemini";
        private string _selectedModel = "gemini-2.0-flash";
        private bool _isFallbackEnabled = true;

        public ObservableCollection<string> AvailableProviders { get; } = new()
        {
            "Google Gemini",
            "Groq Cloud",
            "xKiro AI Gateway"
        };

        public ObservableCollection<string> AvailableModels { get; } = new();
        public ObservableCollection<AIModelInfo> AvailableModelInfos { get; } = new();
        public ObservableCollection<AIRequestLog> RequestLogs { get; } = new();

        private bool _isLoadingModels;
        public bool IsLoadingModels
        {
            get => _isLoadingModels;
            set => SetProperty(ref _isLoadingModels, value);
        }

        private string _catalogStatusMessage = string.Empty;
        public string CatalogStatusMessage
        {
            get => _catalogStatusMessage;
            set => SetProperty(ref _catalogStatusMessage, value);
        }

        #region Properties
        public string ConfigSourceName => "apiminh.json (Hệ thống tập trung)";
        public string ConfigSourcePath => AIConfigurationService.Instance.ResolvedConfigFilePath ?? "D:\\CDPTUD1\\EAI\\EAI\\apiminh.json";

        public string GeminiMaskedKey
        {
            get => _geminiMaskedKey;
            set => SetProperty(ref _geminiMaskedKey, value);
        }

        public string GroqMaskedKey
        {
            get => _groqMaskedKey;
            set => SetProperty(ref _groqMaskedKey, value);
        }

        public string XkioMaskedKey
        {
            get => _xkioMaskedKey;
            set => SetProperty(ref _xkioMaskedKey, value);
        }

        public string AiTutorUsageText
        {
            get => _aiTutorUsageText;
            set => SetProperty(ref _aiTutorUsageText, value);
        }

        public string AiWritingUsageText
        {
            get => _aiWritingUsageText;
            set => SetProperty(ref _aiWritingUsageText, value);
        }

        public string LastAiUsageTimeText
        {
            get => _lastAiUsageTimeText;
            set => SetProperty(ref _lastAiUsageTimeText, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public string LastRefreshTime
        {
            get => _lastRefreshTime;
            set => SetProperty(ref _lastRefreshTime, value);
        }

        public string SystemHealthText
        {
            get => _systemHealthText;
            set => SetProperty(ref _systemHealthText, value);
        }

        public string SystemHealthBadgeColor
        {
            get => _systemHealthBadgeColor;
            set => SetProperty(ref _systemHealthBadgeColor, value);
        }

        public string SystemHealthBadgeBg
        {
            get => _systemHealthBadgeBg;
            set => SetProperty(ref _systemHealthBadgeBg, value);
        }

        public string TotalRequestsText
        {
            get => _totalRequestsText;
            set => SetProperty(ref _totalRequestsText, value);
        }

        public string AverageLatencyText
        {
            get => _averageLatencyText;
            set => SetProperty(ref _averageLatencyText, value);
        }

        public string DatabaseStatusText
        {
            get => _databaseStatusText;
            set => SetProperty(ref _databaseStatusText, value);
        }

        public string DatabaseLatencyText
        {
            get => _databaseLatencyText;
            set => SetProperty(ref _databaseLatencyText, value);
        }

        public string ErrorRateText
        {
            get => _errorRateText;
            set => SetProperty(ref _errorRateText, value);
        }

        public AIHealthStatus GeminiStatus
        {
            get => _geminiStatus;
            set => SetProperty(ref _geminiStatus, value);
        }

        public AIHealthStatus GroqStatus
        {
            get => _groqStatus;
            set => SetProperty(ref _groqStatus, value);
        }

        public AIHealthStatus XkioStatus
        {
            get => _xkioStatus;
            set => SetProperty(ref _xkioStatus, value);
        }

        public string SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                if (SetProperty(ref _selectedProvider, value))
                {
                    IsSaveNotificationVisible = false;
                    OnProviderChanged();
                }
            }
        }

        private int _selectedModelIndex = 0;
        public int SelectedModelIndex
        {
            get => _selectedModelIndex;
            set
            {
                if (SetProperty(ref _selectedModelIndex, value))
                {
                    if (value >= 0 && value < AvailableModelInfos.Count)
                    {
                        var model = AvailableModelInfos[value];
                        _selectedModel = model.Id;
                        OnPropertyChanged(nameof(SelectedModel));
                    }
                }
            }
        }

        public string SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (SetProperty(ref _selectedModel, value))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        for (int i = 0; i < AvailableModelInfos.Count; i++)
                        {
                            if (AvailableModelInfos[i].Id == value)
                            {
                                if (_selectedModelIndex != i)
                                {
                                    _selectedModelIndex = i;
                                    OnPropertyChanged(nameof(SelectedModelIndex));
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        public bool IsFallbackEnabled
        {
            get => _isFallbackEnabled;
            set => SetProperty(ref _isFallbackEnabled, value);
        }

        private bool _isSaveNotificationVisible;
        public bool IsSaveNotificationVisible
        {
            get => _isSaveNotificationVisible;
            set => SetProperty(ref _isSaveNotificationVisible, value);
        }

        private string _saveNotificationMessage = string.Empty;
        public string SaveNotificationMessage
        {
            get => _saveNotificationMessage;
            set => SetProperty(ref _saveNotificationMessage, value);
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand { get; }
        public ICommand TestGeminiCommand { get; }
        public ICommand TestGroqCommand { get; }
        public ICommand TestXkioCommand { get; }
        public ICommand TestCurrentModelCommand { get; }
        public ICommand RestoreDefaultModelCommand { get; }
        public ICommand SaveModelConfigCommand { get; }
        public ICommand DismissSaveNotificationCommand { get; }
        public ICommand ReloadCatalogCommand { get; }
        #endregion

        public GiamSatHeThongViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await ThucHienKiemTraToanBoAsync());
            TestGeminiCommand = new RelayCommand(async () => await KiemTraProviderAsync(AIProviderType.Gemini));
            TestGroqCommand = new RelayCommand(async () => await KiemTraProviderAsync(AIProviderType.Groq));
            TestXkioCommand = new RelayCommand(async () => await KiemTraProviderAsync(AIProviderType.Xkio));
            TestCurrentModelCommand = new RelayCommand(async () => await KiemTraMoHinhHienTaiAsync());
            RestoreDefaultModelCommand = new RelayCommand(KhoiPhucMoHinhMacDinh);
            SaveModelConfigCommand = new RelayCommand(LuuCauHinhMoHinh);
            DismissSaveNotificationCommand = new RelayCommand(() => IsSaveNotificationVisible = false);
            ReloadCatalogCommand = new RelayCommand(async () => await TaiDanhSachMoHinhAsync(forceRefresh: true));

            // Đồng bộ trạng thái nhà cung cấp hiện tại từ AIConfigurationService
            AIConfigurationService.Instance.LoadActiveModelConfig();
            switch (AIConfigurationService.Instance.ActiveProvider)
            {
                case AIProviderType.Groq:
                    _selectedProvider = "Groq Cloud";
                    break;
                case AIProviderType.Xkio:
                    _selectedProvider = "xKiro AI Gateway";
                    break;
                default:
                    _selectedProvider = "Google Gemini";
                    break;
            }
            _selectedModel = AIConfigurationService.Instance.ActiveModel;
            _isFallbackEnabled = AIConfigurationService.Instance.EnableFallback;

            UpdateMaskedKeys();

            // Khởi tạo danh sách mô hình
            OnProviderChanged(keepCurrentModelIfAvailable: true);

            // Nạp dữ liệu giám sát ban đầu
            _ = ThucHienKiemTraToanBoAsync();
        }

        private void UpdateMaskedKeys()
        {
            GeminiMaskedKey = AIConfigurationService.Instance.GetMaskedApiKey(AIProviderType.Gemini);
            GroqMaskedKey = AIConfigurationService.Instance.GetMaskedApiKey(AIProviderType.Groq);
            XkioMaskedKey = AIConfigurationService.Instance.GetMaskedApiKey(AIProviderType.Xkio);
        }

        private IAIProvider GetCurrentSelectedProvider()
        {
            if (SelectedProvider.Contains("Groq"))
                return AIServiceManager.Instance.GetProvider(AIProviderType.Groq);
            if (SelectedProvider.IndexOf("Xkio", StringComparison.OrdinalIgnoreCase) >= 0 || SelectedProvider.IndexOf("xKiro", StringComparison.OrdinalIgnoreCase) >= 0)
                return AIServiceManager.Instance.GetProvider(AIProviderType.Xkio);
            return AIServiceManager.Instance.GetProvider(AIProviderType.Gemini);
        }

        private AIProviderType GetSelectedProviderType()
        {
            if (SelectedProvider.Contains("Groq")) return AIProviderType.Groq;
            if (SelectedProvider.IndexOf("Xkio", StringComparison.OrdinalIgnoreCase) >= 0 || SelectedProvider.IndexOf("xKiro", StringComparison.OrdinalIgnoreCase) >= 0)
                return AIProviderType.Xkio;
            return AIProviderType.Gemini;
        }

        private void OnProviderChanged(bool keepCurrentModelIfAvailable = false)
        {
            var provider = GetCurrentSelectedProvider();
            var targetType = GetSelectedProviderType();

            AvailableModelInfos.Clear();
            AvailableModels.Clear();

            var models = AIConfigurationService.Instance.GetSupportedModels(targetType);
            string defModel = AIConfigurationService.Instance.GetDefaultModel(targetType);

            foreach (var m in models)
            {
                AvailableModelInfos.Add(m);
                AvailableModels.Add(m.Id);
            }

            // Chọn model
            int targetIndex = 0;
            if (keepCurrentModelIfAvailable && !string.IsNullOrWhiteSpace(_selectedModel))
            {
                for (int i = 0; i < AvailableModelInfos.Count; i++)
                {
                    if (AvailableModelInfos[i].Id == _selectedModel)
                    {
                        targetIndex = i;
                        break;
                    }
                }
            }

            if (AvailableModelInfos.Count > 0)
            {
                SelectedModelIndex = targetIndex;
                SelectedModel = AvailableModelInfos[targetIndex].Id;
            }

            _ = TaiDanhSachMoHinhAsync(forceRefresh: false, keepCurrentModelIfAvailable);
        }

        public async Task TaiDanhSachMoHinhAsync(bool forceRefresh = false, bool keepCurrentModelIfAvailable = false)
        {
            var provider = GetCurrentSelectedProvider();
            IsLoadingModels = true;
            try
            {
                var models = await provider.GetModelsAsync(forceRefresh);
                if (models != null && models.Count > 0)
                {
                    AvailableModelInfos.Clear();
                    AvailableModels.Clear();

                    foreach (var m in models)
                    {
                        AvailableModelInfos.Add(m);
                        AvailableModels.Add(m.Id);
                    }

                    CatalogStatusMessage = provider.CatalogStatusMessage;

                    int targetIndex = 0;
                    string currentModel = SelectedModel;
                    if (keepCurrentModelIfAvailable && !string.IsNullOrWhiteSpace(currentModel))
                    {
                        for (int i = 0; i < AvailableModelInfos.Count; i++)
                        {
                            if (AvailableModelInfos[i].Id == currentModel)
                            {
                                targetIndex = i;
                                break;
                            }
                        }
                    }

                    if (AvailableModelInfos.Count > 0)
                    {
                        SelectedModelIndex = targetIndex;
                        SelectedModel = AvailableModelInfos[targetIndex].Id;
                    }
                }
            }
            catch (Exception ex)
            {
                CatalogStatusMessage = "Lỗi khi tải danh mục: " + ex.Message;
            }
            finally
            {
                IsLoadingModels = false;
            }
        }

        private void KhoiPhucMoHinhMacDinh()
        {
            var targetType = GetSelectedProviderType();
            string defModel = AIConfigurationService.Instance.GetDefaultModel(targetType);

            SelectedModel = defModel;
            for (int i = 0; i < AvailableModelInfos.Count; i++)
            {
                if (AvailableModelInfos[i].Id == defModel)
                {
                    SelectedModelIndex = i;
                    break;
                }
            }

            ThongBaoDialog.ThongTin(
                $"Đã khôi phục mô hình mặc định: {defModel} cho nhà cung cấp {SelectedProvider}.\nNhấn [Lưu cấu hình mô hình] để áp dụng chính thức cho hệ thống.",
                "Khôi phục mặc định");
        }

        private async Task KiemTraMoHinhHienTaiAsync()
        {
            var targetType = GetSelectedProviderType();
            string modelToTest = SelectedModel;

            StatusMessage = $"Đang gửi request kiểm tra thực tế tới {SelectedProvider} ({modelToTest})...";

            try
            {
                var resp = await AIConfigurationService.Instance.TestConnectionAsync(targetType, modelToTest);

                if (resp.Success)
                {
                    ThongBaoDialog.ThanhCong(
                        $"Kết nối thành công đến AI Service!\n\n" +
                        $"• Nhà cung cấp: {SelectedProvider}\n" +
                        $"• Mô hình phản hồi: {resp.ModelUsed}\n" +
                        $"• Thời gian phản hồi: {resp.LatencyMs} ms\n" +
                        $"• Nguồn API Key: apiminh.json\n\n" +
                        $"Dịch vụ hoạt động ổn định và sẵn sàng phục vụ học viên.",
                        "Kiểm tra kết nối thành công");
                }
                else
                {
                    ThongBaoDialog.Loi(
                        $"Không thể kết nối đến AI Service ({SelectedProvider}):\n\n" +
                        $"• Lỗi: {resp.ErrorMessage}\n" +
                        $"• Mã trạng thái: {resp.StatusCode}\n" +
                        $"• Nguồn cấu hình: apiminh.json",
                        "Kiểm tra kết nối thất bại");
                }

                _ = ThucHienKiemTraToanBoAsync();
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi($"Lỗi khi kiểm tra kết nối: {ex.Message}", "Lỗi");
            }
        }

        private void LuuCauHinhMoHinh()
        {
            // 1. Kiểm tra quyền Admin ở Backend
            if (!AIConfigurationService.Instance.CheckAdminPermission())
            {
                ThongBaoDialog.CanhBao("Bạn không có quyền thực hiện thao tác này. Chỉ tài khoản Quản trị viên (Admin/Giảng viên) mới được phép thay đổi cấu hình LLM hệ thống.", "Từ chối truy cập");
                return;
            }

            var targetType = GetSelectedProviderType();

            // 2. Xác nhận trước khi áp dụng
            if (!ThongBaoDialog.XacNhan(
                $"Bạn có chắc chắn muốn lưu và áp dụng cấu hình mô hình AI mới cho toàn bộ hệ thống không?\n\n" +
                $"• Nhà cung cấp: {SelectedProvider}\n" +
                $"• Mô hình chọn: {SelectedModel}\n" +
                $"• Nguồn cấu hình: apiminh.json\n" +
                $"• Tự động dự phòng (Fallback): {(IsFallbackEnabled ? "Bật" : "Tắt")}\n\n" +
                $"Cấu hình này sẽ tự động áp dụng ngay lập tức cho cả 2 phân hệ:\n" +
                $"1. Gia sư Trợ lý AI (TroLyAIView)\n" +
                $"2. Chấm & Sửa lỗi Bài viết AI (BaiVietAIView)",
                "Xác nhận thay đổi cấu hình LLM"))
            {
                return;
            }

            try
            {
                AIConfigurationService.Instance.SaveActiveConfig(targetType, SelectedModel, IsFallbackEnabled);

                SaveNotificationMessage = $"Đã lưu và kích hoạt thành công: {SelectedProvider} — Mô hình: {SelectedModel}";
                IsSaveNotificationVisible = true;

                ThongBaoDialog.ThanhCong(
                    $"Đã lưu và áp dụng cấu hình mô hình AI mới thành công!\n\n" +
                    $"• Nhà cung cấp hoạt động: {SelectedProvider}\n" +
                    $"• Mô hình LLM: {SelectedModel}\n" +
                    $"• Nguồn API Key: apiminh.json\n\n" +
                    $"Cả Gia sư Trợ lý AI và AI Writing đã được đồng bộ sang mô hình mới.",
                    "Lưu cấu hình thành công");

                _ = ThucHienKiemTraToanBoAsync();
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi($"Không thể lưu cấu hình mô hình AI:\n{ex.Message}", "Lỗi lưu cấu hình");
            }
        }

        public async Task ThucHienKiemTraToanBoAsync()
        {
            if (IsRefreshing) return;
            IsRefreshing = true;
            StatusMessage = "Đang kiểm tra trạng thái kết nối các nhà cung cấp AI và Database...";

            try
            {
                // 1. Kiểm tra Database
                var dbCheck = _dashboardBUS.KiemTraKetNoiDatabase();
                DatabaseStatusText = dbCheck.IsConnected ? "Kết nối tốt" : "Mất kết nối";
                DatabaseLatencyText = dbCheck.Message;

                // 2. Tải lại cấu hình từ apiminh.json
                AIConfigurationService.Instance.ReloadConfiguration();
                UpdateMaskedKeys();

                // 3. Kiểm tra các AI Providers
                var healthList = await AIServiceManager.Instance.CheckAllProvidersHealthAsync();
                foreach (var h in healthList)
                {
                    if (h.ProviderType == AIProviderType.Gemini) GeminiStatus = h;
                    else if (h.ProviderType == AIProviderType.Groq) GroqStatus = h;
                    else if (h.ProviderType == AIProviderType.Xkio) XkioStatus = h;
                }

                // 4. Cập nhật số liệu Metrics
                var metrics = AIServiceManager.Instance.Metrics;
                TotalRequestsText = metrics.TotalRequests.ToString("N0");
                AverageLatencyText = $"{metrics.AverageLatencyMs:0} ms";
                ErrorRateText = $"{metrics.ErrorRatePercent:0.0}%";

                // 5. Thống kê lượt sử dụng từ Database
                int tutorCount = HoiThoaiAIBUS.Instance.DemTongLuotDung();
                int writingCount = BaiVietAIBUS.Instance.DemTongLuotCham();
                AiTutorUsageText = $"{tutorCount:N0} tin nhắn";
                AiWritingUsageText = $"{writingCount:N0} bài luận";

                // Đánh giá sức khỏe tổng thể
                int healthyCount = healthList.Count(x => x.Health == "Healthy");
                if (dbCheck.IsConnected && healthyCount >= 1)
                {
                    SystemHealthText = $"Hệ thống sẵn sàng ({healthyCount}/3 AI Providers khả dụng)";
                    SystemHealthBadgeColor = "#15803D";
                    SystemHealthBadgeBg = "#DCFCE7";
                }
                else if (!dbCheck.IsConnected)
                {
                    SystemHealthText = "Lỗi kết nối cơ sở dữ liệu";
                    SystemHealthBadgeColor = "#DC2626";
                    SystemHealthBadgeBg = "#FEE2E2";
                }
                else
                {
                    SystemHealthText = "Cần kiểm tra lại cấu hình API Keys";
                    SystemHealthBadgeColor = "#D97706";
                    SystemHealthBadgeBg = "#FEF3C7";
                }

                // 6. Nạp dòng sự kiện gần nhất (Stream logs)
                RequestLogs.Clear();
                var logs = metrics.GetRecentLogs();
                foreach (var log in logs)
                {
                    RequestLogs.Add(log);
                }

                LastRefreshTime = $"Cập nhật lúc: {DateTime.Now:HH:mm:ss}";
                StatusMessage = "Kiểm tra hoàn tất.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Lỗi khi kiểm tra: " + ex.Message;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task KiemTraProviderAsync(AIProviderType type)
        {
            AIConfigurationService.Instance.ReloadConfiguration();
            UpdateMaskedKeys();
            var provider = AIServiceManager.Instance.GetProvider(type);
            var status = await provider.CheckHealthAsync();
            if (type == AIProviderType.Gemini) GeminiStatus = status;
            else if (type == AIProviderType.Groq) GroqStatus = status;
            else if (type == AIProviderType.Xkio) XkioStatus = status;
        }
    }
}
