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
using BUS.AI.Services;
using System.Windows.Threading;
using GUI.ViewModels.Common;
using GUI.Views.Common;

namespace GUI.ViewModels.GiangVien
{
    public class GiamSatHeThongViewModel : ViewModelBase, IDisposable
    {
        private readonly DashboardBUS _dashboardBUS = new();
        private readonly IOllamaProcessService _ollamaProcessService = OllamaProcessService.Instance;
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
        private string _aiQuestionGenUsageText = "0 câu";
        private string _lastAiUsageTimeText = "Chưa ghi nhận";

        // Trạng thái từng Provider
        private AIHealthStatus _geminiStatus = new() { ProviderName = "Google Gemini", ProviderType = AIProviderType.Gemini, Health = "Unavailable" };
        private AIHealthStatus _groqStatus = new() { ProviderName = "Groq Cloud", ProviderType = AIProviderType.Groq, Health = "Unavailable" };
        private AIHealthStatus _xkioStatus = new() { ProviderName = "xKiro AI Gateway", ProviderType = AIProviderType.Xkio, Health = "Unavailable" };
        private AIHealthStatus _ollamaStatus = new() { ProviderName = "Ollama (Offline/Local LLM)", ProviderType = AIProviderType.Ollama, Health = "Unavailable" };

        // Masked keys & URLs
        private string _geminiMaskedKey = "Chưa cấu hình";
        private string _groqMaskedKey = "Chưa cấu hình";
        private string _xkioMaskedKey = "Chưa cấu hình";
        private string _ollamaMaskedKey = "Không cần API Key (Localhost)";
        private string _ollamaBaseUrl = "http://localhost:11434";

        // Trạng thái điều khiển & giám sát máy chủ Ollama
        private bool _isOllamaRunning;
        private bool _isOllamaBusy;
        private string _ollamaRamUsageText = "0 MB (Đã dừng)";
        private string _ollamaOperationalStatus = "Đã dừng";
        private string _ollamaStatusBadgeBg = "#FEE2E2";
        private string _ollamaStatusBadgeFg = "#DC2626";
        private readonly DispatcherTimer? _ollamaMonitorTimer;

        // Cấu hình chuyển đổi mô hình (Model Switching)
        private string _selectedProvider = "Google Gemini";
        private string _selectedModel = "gemini-2.0-flash";
        private bool _isFallbackEnabled = true;
        private bool _isTestingConnection;

        public ObservableCollection<string> AvailableProviders { get; } = new()
        {
            "Google Gemini",
            "Groq Cloud",
            "xKiro AI Gateway",
            "Ollama (Offline Local LLM)"
        };

        public ObservableCollection<string> AvailableModels { get; } = new();
        public ObservableCollection<AIModelInfo> AvailableModelInfos { get; } = new();
        public ObservableCollection<AIRequestLog> RequestLogs { get; } = new();

        public bool HasRequestLogs => RequestLogs != null && RequestLogs.Count > 0;
        public bool IsRequestLogsEmpty => !HasRequestLogs;

        public bool IsTestingConnection
        {
            get => _isTestingConnection;
            set
            {
                if (SetProperty(ref _isTestingConnection, value))
                {
                    OnPropertyChanged(nameof(CanTestConnection));
                }
            }
        }

        public bool CanTestConnection => !IsTestingConnection && !IsRefreshing && !IsOllamaBusy;

        public bool IsOllamaRunning
        {
            get => _isOllamaRunning;
            set
            {
                if (SetProperty(ref _isOllamaRunning, value))
                {
                    OnPropertyChanged(nameof(CanControlOllama));
                }
            }
        }

        public bool IsOllamaBusy
        {
            get => _isOllamaBusy;
            set
            {
                if (SetProperty(ref _isOllamaBusy, value))
                {
                    OnPropertyChanged(nameof(CanControlOllama));
                    OnPropertyChanged(nameof(CanTestConnection));
                }
            }
        }

        public bool CanControlOllama => !IsOllamaBusy && !IsRefreshing;

        public string OllamaRamUsageText
        {
            get => _ollamaRamUsageText;
            set => SetProperty(ref _ollamaRamUsageText, value);
        }

        public string OllamaOperationalStatus
        {
            get => _ollamaOperationalStatus;
            set => SetProperty(ref _ollamaOperationalStatus, value);
        }

        public string OllamaStatusBadgeBg
        {
            get => _ollamaStatusBadgeBg;
            set => SetProperty(ref _ollamaStatusBadgeBg, value);
        }

        public string OllamaStatusBadgeFg
        {
            get => _ollamaStatusBadgeFg;
            set => SetProperty(ref _ollamaStatusBadgeFg, value);
        }

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
        public string ConfigSourceName
        {
            get
            {
                if (!string.IsNullOrEmpty(AIConfigurationService.Instance.ResolvedConfigFilePath) && System.IO.File.Exists(AIConfigurationService.Instance.ResolvedConfigFilePath))
                {
                    return "apiminh.json (Hệ thống tập trung)";
                }
                return ".NET User Secrets (Cấu hình an toàn)";
            }
        }

        public string ConfigSourcePath
        {
            get
            {
                if (!string.IsNullOrEmpty(AIConfigurationService.Instance.ResolvedConfigFilePath) && System.IO.File.Exists(AIConfigurationService.Instance.ResolvedConfigFilePath))
                {
                    return AIConfigurationService.Instance.ResolvedConfigFilePath;
                }
                return "%APPDATA%\\Microsoft\\UserSecrets (Môi trường bảo mật)";
            }
        }

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

        public string OllamaMaskedKey
        {
            get => _ollamaMaskedKey;
            set => SetProperty(ref _ollamaMaskedKey, value);
        }

        public string OllamaBaseUrl
        {
            get => _ollamaBaseUrl;
            set => SetProperty(ref _ollamaBaseUrl, value);
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

        public string AiQuestionGenUsageText
        {
            get => _aiQuestionGenUsageText;
            set => SetProperty(ref _aiQuestionGenUsageText, value);
        }

        public string LastAiUsageTimeText
        {
            get => _lastAiUsageTimeText;
            set => SetProperty(ref _lastAiUsageTimeText, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (SetProperty(ref _isRefreshing, value))
                {
                    OnPropertyChanged(nameof(CanTestConnection));
                    OnPropertyChanged(nameof(CanControlOllama));
                }
            }
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

        public AIHealthStatus OllamaStatus
        {
            get => _ollamaStatus;
            set => SetProperty(ref _ollamaStatus, value);
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
        public ICommand TestOllamaCommand { get; }
        public ICommand StartOllamaCommand { get; }
        public ICommand StopOllamaCommand { get; }
        public ICommand ReleaseOllamaRamCommand { get; }
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
            TestOllamaCommand = new RelayCommand(async () => await KiemTraProviderAsync(AIProviderType.Ollama));
            StartOllamaCommand = new RelayCommand(async () => await BatMayChuOllamaAsync());
            StopOllamaCommand = new RelayCommand(async () => await TatMayChuOllamaAsync());
            ReleaseOllamaRamCommand = new RelayCommand(async () => await GiaiPhongRamOllamaAsync());
            TestCurrentModelCommand = new RelayCommand(async () => await KiemTraMoHinhHienTaiAsync());
            RestoreDefaultModelCommand = new RelayCommand(KhoiPhucMoHinhMacDinh);
            SaveModelConfigCommand = new RelayCommand(LuuCauHinhMoHinh);
            DismissSaveNotificationCommand = new RelayCommand(() => IsSaveNotificationVisible = false);
            ReloadCatalogCommand = new RelayCommand(async () => await TaiDanhSachMoHinhAsync(forceRefresh: true));

            // Timer giám sát tiến trình Ollama cục bộ định kỳ (4s), an toàn và không gây nghẽn UI
            try
            {
                _ollamaMonitorTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(4)
                };
                _ollamaMonitorTimer.Tick += async (s, e) =>
                {
                    if (!_isOllamaBusy && !_isRefreshing)
                    {
                        await CapNhatTrangThaiOllamaAsync(silent: true);
                    }
                };
                _ollamaMonitorTimer.Start();
            }
            catch
            {
                // DispatcherTimer có thể không khả dụng trong môi trường Unit Test không có UI Dispatcher
            }

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
                case AIProviderType.Ollama:
                    _selectedProvider = "Ollama (Offline Local LLM)";
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
            OllamaMaskedKey = AIConfigurationService.Instance.GetMaskedApiKey(AIProviderType.Ollama);
            var (ollamaUrl, _) = AIConfigHelper.GetOllamaConfig();
            OllamaBaseUrl = ollamaUrl;
        }

        private IAIProvider GetCurrentSelectedProvider()
        {
            if (SelectedProvider.Contains("Groq"))
                return AIServiceManager.Instance.GetProvider(AIProviderType.Groq);
            if (SelectedProvider.IndexOf("Xkio", StringComparison.OrdinalIgnoreCase) >= 0 || SelectedProvider.IndexOf("xKiro", StringComparison.OrdinalIgnoreCase) >= 0)
                return AIServiceManager.Instance.GetProvider(AIProviderType.Xkio);
            if (SelectedProvider.IndexOf("Ollama", StringComparison.OrdinalIgnoreCase) >= 0 || SelectedProvider.IndexOf("Local", StringComparison.OrdinalIgnoreCase) >= 0)
                return AIServiceManager.Instance.GetProvider(AIProviderType.Ollama);
            return AIServiceManager.Instance.GetProvider(AIProviderType.Gemini);
        }

        private AIProviderType GetSelectedProviderType()
        {
            if (SelectedProvider.Contains("Groq")) return AIProviderType.Groq;
            if (SelectedProvider.IndexOf("Xkio", StringComparison.OrdinalIgnoreCase) >= 0 || SelectedProvider.IndexOf("xKiro", StringComparison.OrdinalIgnoreCase) >= 0)
                return AIProviderType.Xkio;
            if (SelectedProvider.IndexOf("Ollama", StringComparison.OrdinalIgnoreCase) >= 0 || SelectedProvider.IndexOf("Local", StringComparison.OrdinalIgnoreCase) >= 0)
                return AIProviderType.Ollama;
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

                    if (forceRefresh)
                    {
                        ThongBaoDialog.ThanhCong($"Đã đồng bộ thành công danh mục {models.Count} mô hình trực tiếp từ {SelectedProvider}!", "Đồng bộ danh mục");
                    }
                }
            }
            catch (Exception ex)
            {
                CatalogStatusMessage = "Lỗi khi tải danh mục: " + ex.Message;
                if (forceRefresh)
                {
                    ThongBaoDialog.Loi($"Không thể đồng bộ danh mục từ {SelectedProvider}:\n{ex.Message}", "Lỗi đồng bộ");
                }
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
            if (IsTestingConnection) return;

            var targetType = GetSelectedProviderType();
            string modelToTest = SelectedModel;

            if (string.IsNullOrWhiteSpace(modelToTest))
            {
                ThongBaoDialog.CanhBao("Vui lòng chọn một mô hình AI hợp lệ trước khi kiểm tra kết nối.", "Chưa chọn mô hình");
                return;
            }

            IsTestingConnection = true;
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
                        $"• Nguồn cấu hình: {ConfigSourceName}\n\n" +
                        $"Dịch vụ hoạt động ổn định và sẵn sàng phục vụ toàn hệ thống.",
                        "Kiểm tra kết nối thành công");
                }
                else
                {
                    ThongBaoDialog.Loi(
                        $"Không thể kết nối đến AI Service ({SelectedProvider}):\n\n" +
                        $"• Lỗi: {resp.ErrorMessage}\n" +
                        $"• Mã trạng thái: {resp.StatusCode}\n" +
                        $"• Nguồn cấu hình: {ConfigSourceName}",
                        "Kiểm tra kết nối thất bại");
                }

                _ = ThucHienKiemTraToanBoAsync();
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi($"Lỗi khi kiểm tra kết nối: {ex.Message}", "Lỗi");
            }
            finally
            {
                IsTestingConnection = false;
                StatusMessage = "Sẵn sàng.";
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

            if (string.IsNullOrWhiteSpace(SelectedModel))
            {
                ThongBaoDialog.CanhBao("Vui lòng chọn một mô hình AI hợp lệ từ danh sách trước khi lưu.", "Chưa chọn mô hình");
                return;
            }

            var targetType = GetSelectedProviderType();

            // 2. Xác nhận trước khi áp dụng
            if (!ThongBaoDialog.XacNhan(
                $"Bạn có chắc chắn muốn lưu và áp dụng cấu hình mô hình AI mới cho toàn bộ hệ thống không?\n\n" +
                $"• Nhà cung cấp: {SelectedProvider}\n" +
                $"• Mô hình chọn: {SelectedModel}\n" +
                $"• Nguồn cấu hình: {ConfigSourceName}\n" +
                $"• Tự động dự phòng (Fallback): {(IsFallbackEnabled ? "Bật" : "Tắt")}\n\n" +
                $"Cấu hình này sẽ tự động áp dụng ngay lập tức cho toàn bộ các phân hệ AI hệ thống:\n" +
                $"1. Gia sư Trợ lý AI (TroLyAIView)\n" +
                $"2. Chấm & Sửa lỗi Bài viết AI (BaiVietAIView)\n" +
                $"3. Tạo câu hỏi bằng AI & Bóc tách đề thi (AISinhDeView)",
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
                    $"• Nguồn cấu hình: {ConfigSourceName}\n\n" +
                    $"Toàn bộ các phân hệ AI hệ thống đã được đồng bộ sang mô hình mới.",
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

                // 2. Tải lại cấu hình từ apiminh.json (hoặc UserSecrets)
                AIConfigurationService.Instance.ReloadConfiguration();
                UpdateMaskedKeys();

                // 3. Kiểm tra các AI Providers
                var healthList = await AIServiceManager.Instance.CheckAllProvidersHealthAsync();
                foreach (var h in healthList)
                {
                    if (h.ProviderType == AIProviderType.Gemini) GeminiStatus = h;
                    else if (h.ProviderType == AIProviderType.Groq) GroqStatus = h;
                    else if (h.ProviderType == AIProviderType.Xkio) XkioStatus = h;
                    else if (h.ProviderType == AIProviderType.Ollama) OllamaStatus = h;
                }

                // Cập nhật trạng thái tiến trình và RAM thực tế của Ollama
                await CapNhatTrangThaiOllamaAsync(silent: true);

                // 4. Cập nhật số liệu Metrics
                var metrics = AIServiceManager.Instance.Metrics;
                TotalRequestsText = metrics.TotalRequests.ToString("N0");
                AverageLatencyText = $"{metrics.AverageLatencyMs:0} ms";
                ErrorRateText = $"{metrics.ErrorRatePercent:0.0}%";

                // 5. Thống kê lượt sử dụng từ Database
                int tutorCount = HoiThoaiAIBUS.Instance.DemTongLuotDung();
                int writingCount = BaiVietAIBUS.Instance.DemTongLuotCham();
                int questionGenCount = AISinhDeBUS.Instance.DemTongCauHoiDoAISinh();
                AiTutorUsageText = $"{tutorCount:N0} tin nhắn";
                AiWritingUsageText = $"{writingCount:N0} bài luận";
                AiQuestionGenUsageText = $"{questionGenCount:N0} câu hỏi";

                // Đánh giá sức khỏe tổng thể
                int healthyCount = healthList.Count(x => x.Health == "Healthy");
                if (dbCheck.IsConnected && healthyCount >= 1)
                {
                    SystemHealthText = $"Hệ thống sẵn sàng ({healthyCount}/4 AI Providers khả dụng)";
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
                OnPropertyChanged(nameof(HasRequestLogs));
                OnPropertyChanged(nameof(IsRequestLogsEmpty));

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
            if (IsTestingConnection) return;
            IsTestingConnection = true;

            string providerName = type switch
            {
                AIProviderType.Groq => "Groq Cloud",
                AIProviderType.Xkio => "xKiro AI Gateway",
                AIProviderType.Ollama => "Ollama Local (Offline)",
                _ => "Google Gemini"
            };

            StatusMessage = $"Đang gửi request kiểm tra thực tế tới {providerName}...";

            try
            {
                AIConfigurationService.Instance.ReloadConfiguration();
                UpdateMaskedKeys();
                var provider = AIServiceManager.Instance.GetProvider(type);
                var status = await provider.CheckHealthAsync();
                if (type == AIProviderType.Gemini) GeminiStatus = status;
                else if (type == AIProviderType.Groq) GroqStatus = status;
                else if (type == AIProviderType.Xkio) XkioStatus = status;
                else if (type == AIProviderType.Ollama) OllamaStatus = status;

                if (status.Health == "Healthy")
                {
                    ThongBaoDialog.ThanhCong(
                        $"Kết nối thành công đến {providerName}!\n\n" +
                        $"• Trạng thái: {status.Message}\n" +
                        $"• Độ trễ phản hồi: {status.LatencyMs} ms\n" +
                        $"• Mô hình hoạt động: {status.ActiveModel}\n" +
                        $"• Nguồn cấu hình: {ConfigSourceName}",
                        "Kiểm tra kết nối thành công");
                }
                else
                {
                    ThongBaoDialog.CanhBao(
                        $"Kiểm tra kết nối {providerName}:\n\n" +
                        $"• Trạng thái: {status.Message}\n" +
                        $"• Vui lòng kiểm tra lại cấu hình API Key hoặc hạn mức dịch vụ.",
                        "Cảnh báo kết nối AI");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi($"Lỗi khi kiểm tra kết nối {providerName}: {ex.Message}", "Lỗi kiểm tra");
            }
            finally
            {
                IsTestingConnection = false;
                StatusMessage = "Sẵn sàng.";
            }
        }

        public async Task BatMayChuOllamaAsync()
        {
            if (IsOllamaBusy) return;
            IsOllamaBusy = true;
            OllamaOperationalStatus = "Đang khởi động...";
            OllamaStatusBadgeBg = "#FEF3C7";
            OllamaStatusBadgeFg = "#D97706";
            StatusMessage = "Đang khởi động máy chủ Ollama chạy nền...";

            try
            {
                var (success, message) = await _ollamaProcessService.StartServerAsync();
                if (success)
                {
                    ThongBaoDialog.ThanhCong(message, "Khởi động Ollama thành công");
                }
                else
                {
                    ThongBaoDialog.CanhBao(message, "Không thể khởi động Ollama");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi($"Lỗi khi khởi động Ollama: {ex.Message}", "Lỗi ngoại lệ");
            }
            finally
            {
                await CapNhatTrangThaiOllamaAsync(silent: false);
                IsOllamaBusy = false;
                StatusMessage = "Sẵn sàng.";
            }
        }

        public async Task TatMayChuOllamaAsync()
        {
            if (IsOllamaBusy) return;
            if (!ThongBaoDialog.XacNhan(
                "Bạn có chắc chắn muốn dừng máy chủ Ollama không?\n\nToàn bộ tiến trình Ollama sẽ bị tắt và 100% RAM chiếm dụng sẽ được giải phóng cho hệ điều hành.",
                "Xác nhận dừng máy chủ Ollama"))
            {
                return;
            }

            IsOllamaBusy = true;
            OllamaOperationalStatus = "Đang tắt...";
            OllamaStatusBadgeBg = "#FEF3C7";
            OllamaStatusBadgeFg = "#D97706";
            StatusMessage = "Đang dừng tiến trình Ollama và thu hồi RAM...";

            try
            {
                var (success, message, freedBytes) = await _ollamaProcessService.StopServerAsync();
                if (success)
                {
                    ThongBaoDialog.ThanhCong(message, "Đã dừng máy chủ Ollama");
                }
                else
                {
                    ThongBaoDialog.CanhBao(message, "Cảnh báo dừng Ollama");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi($"Lỗi khi dừng Ollama: {ex.Message}", "Lỗi dừng tiến trình");
            }
            finally
            {
                await CapNhatTrangThaiOllamaAsync(silent: false);
                IsOllamaBusy = false;
                StatusMessage = "Sẵn sàng.";
            }
        }

        public async Task GiaiPhongRamOllamaAsync()
        {
            if (IsOllamaBusy) return;
            IsOllamaBusy = true;
            StatusMessage = "Đang gửi yêu cầu giải phóng bộ nhớ mô hình tới Ollama...";

            try
            {
                var (success, message) = await _ollamaProcessService.ReleaseMemoryAsync();
                if (success)
                {
                    ThongBaoDialog.ThanhCong(message, "Giải phóng RAM thành công");
                }
                else
                {
                    ThongBaoDialog.CanhBao(message, "Không thể giải phóng bộ nhớ");
                }
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi($"Lỗi khi giải phóng RAM: {ex.Message}", "Lỗi giải phóng RAM");
            }
            finally
            {
                await CapNhatTrangThaiOllamaAsync(silent: false);
                IsOllamaBusy = false;
                StatusMessage = "Sẵn sàng.";
            }
        }

        public async Task CapNhatTrangThaiOllamaAsync(bool silent = false)
        {
            try
            {
                var status = await _ollamaProcessService.GetDetailedStatusAsync();
                IsOllamaRunning = status.IsRunning;
                OllamaRamUsageText = status.RamUsageFormatted;

                if (status.IsRunning)
                {
                    OllamaOperationalStatus = "Đang chạy";
                    OllamaStatusBadgeBg = "#DCFCE7";
                    OllamaStatusBadgeFg = "#15803D";
                }
                else
                {
                    OllamaOperationalStatus = "Đã dừng";
                    OllamaStatusBadgeBg = "#FEE2E2";
                    OllamaStatusBadgeFg = "#DC2626";
                }

                if (!silent)
                {
                    var provider = AIServiceManager.Instance.GetProvider(AIProviderType.Ollama);
                    OllamaStatus = await provider.CheckHealthAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CapNhatTrangThaiOllamaAsync] {ex.Message}");
            }
        }

        public void Dispose()
        {
            try
            {
                _ollamaMonitorTimer?.Stop();
            }
            catch
            {
            }
        }
    }
}
