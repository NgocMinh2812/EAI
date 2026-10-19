using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BUS;
using BUS.AI;
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

        // Trạng thái từng Provider
        private AIHealthStatus _geminiStatus = new() { ProviderName = "Google Gemini", ProviderType = AIProviderType.Gemini, Health = "Unavailable" };
        private AIHealthStatus _groqStatus = new() { ProviderName = "Groq Cloud", ProviderType = AIProviderType.Groq, Health = "Unavailable" };
        private AIHealthStatus _xkioStatus = new() { ProviderName = "xKiro AI Gateway", ProviderType = AIProviderType.Xkio, Health = "Unavailable" };

        // Cấu hình chuyển đổi mô hình (Model Switching)
        private string _selectedProvider = "Google Gemini";
        private string _selectedModel = "gemini-3.5-flash";
        private bool _isFallbackEnabled = true;

        public ObservableCollection<string> AvailableProviders { get; } = new()
        {
            "Google Gemini",
            "Groq Cloud",
            "xKiro AI Gateway"
        };

        public ObservableCollection<string> AvailableModels { get; } = new();

        public ObservableCollection<AIRequestLog> RequestLogs { get; } = new();

        #region Properties

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

        public string SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        public bool IsFallbackEnabled
        {
            get => _isFallbackEnabled;
            set
            {
                if (SetProperty(ref _isFallbackEnabled, value))
                {
                    AIServiceManager.Instance.EnableFallback = value;
                }
            }
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
        public ICommand SaveModelConfigCommand { get; }
        public ICommand DismissSaveNotificationCommand { get; }

        #endregion

        public GiamSatHeThongViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await ThucHienKiemTraToanBoAsync());
            TestGeminiCommand = new RelayCommand(async () => await KiemTraProviderAsync(AIProviderType.Gemini));
            TestGroqCommand = new RelayCommand(async () => await KiemTraProviderAsync(AIProviderType.Groq));
            TestXkioCommand = new RelayCommand(async () => await KiemTraProviderAsync(AIProviderType.Xkio));
            SaveModelConfigCommand = new RelayCommand(LuuCauHinhMoHinh);
            DismissSaveNotificationCommand = new RelayCommand(() => IsSaveNotificationVisible = false);

            // Đồng bộ trạng thái nhà cung cấp hiện tại từ AIServiceManager
            switch (AIServiceManager.Instance.ActiveProviderType)
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
            _isFallbackEnabled = AIServiceManager.Instance.EnableFallback;

            // Khởi tạo danh sách mô hình
            OnProviderChanged(keepCurrentModelIfAvailable: true);

            // Nạp dữ liệu giám sát ban đầu
            _ = ThucHienKiemTraToanBoAsync();
        }

        private void OnProviderChanged(bool keepCurrentModelIfAvailable = false)
        {
            AvailableModels.Clear();
            IAIProvider provider;
            if (SelectedProvider.Contains("Gemini"))
            {
                provider = AIServiceManager.Instance.GetProvider(AIProviderType.Gemini);
            }
            else if (SelectedProvider.Contains("Groq"))
            {
                provider = AIServiceManager.Instance.GetProvider(AIProviderType.Groq);
            }
            else
            {
                provider = AIServiceManager.Instance.GetProvider(AIProviderType.Xkio);
            }

            foreach (var m in provider.SupportedModels)
            {
                AvailableModels.Add(m);
            }

            string? currentModel = AIServiceManager.Instance.ActiveModel;
            if (keepCurrentModelIfAvailable && !string.IsNullOrWhiteSpace(currentModel) && AvailableModels.Contains(currentModel))
            {
                SelectedModel = currentModel;
            }
            else
            {
                SelectedModel = provider.DefaultModel;
            }
        }

        private void LuuCauHinhMoHinh()
        {
            try
            {
                AIProviderType targetType = AIProviderType.Gemini;
                if (SelectedProvider.Contains("Groq")) targetType = AIProviderType.Groq;
                else if (SelectedProvider.IndexOf("Xkio", StringComparison.OrdinalIgnoreCase) >= 0 || SelectedProvider.IndexOf("xKiro", StringComparison.OrdinalIgnoreCase) >= 0) targetType = AIProviderType.Xkio;

                AIServiceManager.Instance.ActiveProviderType = targetType;
                AIServiceManager.Instance.ActiveModel = SelectedModel;
                AIServiceManager.Instance.EnableFallback = IsFallbackEnabled;

                // Ghi bền vững vào cấu hình hệ thống
                BUS.AI.Configuration.AIConfigHelper.SaveActiveModelConfig(targetType.ToString(), SelectedModel ?? "", IsFallbackEnabled);

                SaveNotificationMessage = $"Đã lưu và kích hoạt thành công: {SelectedProvider} — Mô hình: {SelectedModel}";
                IsSaveNotificationVisible = true;

                ThongBaoDialog.ThanhCong(
                    $"Đã lưu và kích hoạt cấu hình mô hình AI thành công!\n\n" +
                    $"• Nhà cung cấp: {SelectedProvider}\n" +
                    $"• Mô hình hoạt động: {SelectedModel}\n" +
                    $"• Chế độ chuyển đổi dự phòng: {(IsFallbackEnabled ? "Đang bật" : "Đã tắt")}\n\n" +
                    $"Toàn bộ chức năng AI trong hệ thống sẽ tự động đồng bộ theo cấu hình này.",
                    "Lưu cấu hình AI thành công");
            }
            catch (Exception ex)
            {
                ThongBaoDialog.Loi(
                    $"Không thể lưu cấu hình mô hình AI:\n{ex.Message}",
                    "Lỗi lưu cấu hình");
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

                // 2. Kiểm tra các AI Providers
                var healthList = await AIServiceManager.Instance.CheckAllProvidersHealthAsync();
                foreach (var h in healthList)
                {
                    if (h.ProviderType == AIProviderType.Gemini) GeminiStatus = h;
                    else if (h.ProviderType == AIProviderType.Groq) GroqStatus = h;
                    else if (h.ProviderType == AIProviderType.Xkio) XkioStatus = h;
                }

                // 3. Cập nhật số liệu Metrics
                var metrics = AIServiceManager.Instance.Metrics;
                TotalRequestsText = metrics.TotalRequests.ToString("N0");
                AverageLatencyText = $"{metrics.AverageLatencyMs:0} ms";
                ErrorRateText = $"{metrics.ErrorRatePercent:0.0}%";

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

                // 4. Nạp dòng sự kiện gần nhất (Stream logs)
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
            var provider = AIServiceManager.Instance.GetProvider(type);
            var status = await provider.CheckHealthAsync();
            if (type == AIProviderType.Gemini) GeminiStatus = status;
            else if (type == AIProviderType.Groq) GroqStatus = status;
            else if (type == AIProviderType.Xkio) XkioStatus = status;
        }
    }
}
