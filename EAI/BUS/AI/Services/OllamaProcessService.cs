using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BUS.AI.Configuration;

namespace BUS.AI.Services
{
    /// <summary>
    /// Dịch vụ quản lý vòng đời tiến trình Ollama (khởi chạy, dừng cây tiến trình, theo dõi RAM thực tế và hạ tải mô hình)
    /// </summary>
    public class OllamaProcessService : IOllamaProcessService
    {
        private static IOllamaProcessService? _instance;
        private static readonly object _lock = new();

        public static IOllamaProcessService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new OllamaProcessService();
                    }
                }
                return _instance;
            }
            set
            {
                lock (_lock)
                {
                    _instance = value;
                }
            }
        }

        private readonly HttpClient _httpClient;
        private readonly string[] _targetProcessNames = { "ollama", "ollama app", "llama-server", "ollama_llama_server" };

        public OllamaProcessService(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
        }

        private string GetBaseEndpointUrl()
        {
            try
            {
                var (baseUrl, _) = AIConfigHelper.GetOllamaConfig();
                if (!string.IsNullOrWhiteSpace(baseUrl))
                {
                    return baseUrl.TrimEnd('/');
                }
            }
            catch
            {
                // Fallback nếu chưa khởi tạo cấu hình
            }
            return "http://localhost:11434";
        }

        /// <inheritdoc />
        public bool IsInstalled()
        {
            return !string.IsNullOrEmpty(FindExecutablePath());
        }

        /// <inheritdoc />
        public string? FindExecutablePath()
        {
            try
            {
                // 1. Kiểm tra biến môi trường PATH
                var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
                var paths = pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var p in paths)
                {
                    try
                    {
                        string candidate = Path.Combine(p, "ollama.exe");
                        if (File.Exists(candidate))
                        {
                            return Path.GetFullPath(candidate);
                        }
                    }
                    catch
                    {
                        // Bỏ qua đường dẫn không hợp lệ trong PATH
                    }
                }

                // 2. Kiểm tra thư mục LocalAppData (%LOCALAPPDATA%\Programs\Ollama\ollama.exe)
                string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (!string.IsNullOrEmpty(localApp))
                {
                    string candidate = Path.Combine(localApp, "Programs", "Ollama", "ollama.exe");
                    if (File.Exists(candidate))
                    {
                        return Path.GetFullPath(candidate);
                    }
                }

                // 3. Kiểm tra ProgramFiles (%ProgramFiles%\Ollama\ollama.exe)
                string progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                if (!string.IsNullOrEmpty(progFiles))
                {
                    string candidate = Path.Combine(progFiles, "Ollama", "ollama.exe");
                    if (File.Exists(candidate))
                    {
                        return Path.GetFullPath(candidate);
                    }
                }

                // 4. Kiểm tra ProgramFiles(x86)
                string progFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                if (!string.IsNullOrEmpty(progFilesX86))
                {
                    string candidate = Path.Combine(progFilesX86, "Ollama", "ollama.exe");
                    if (File.Exists(candidate))
                    {
                        return Path.GetFullPath(candidate);
                    }
                }
            }
            catch
            {
                // Trả về null nếu có lỗi truy cập hệ thống file
            }

            return null;
        }

        /// <inheritdoc />
        public bool IsProcessRunning()
        {
            var processes = GetRunningOllamaProcesses();
            bool hasProcess = processes.Count > 0;
            DisposeProcesses(processes);
            return hasProcess;
        }

        /// <inheritdoc />
        public long GetTotalWorkingSetBytes()
        {
            long totalBytes = 0;
            var processes = GetRunningOllamaProcesses();
            foreach (var proc in processes)
            {
                try
                {
                    if (!proc.HasExited)
                    {
                        proc.Refresh();
                        totalBytes += proc.WorkingSet64;
                    }
                }
                catch
                {
                    // Bỏ qua nếu tiến trình vừa thoát hoặc không đủ quyền đọc
                }
            }
            DisposeProcesses(processes);
            return totalBytes;
        }

        /// <inheritdoc />
        public string FormatBytes(long bytes)
        {
            if (bytes <= 0)
            {
                return "0 MB";
            }

            double mb = bytes / (1024.0 * 1024.0);
            if (mb >= 1024.0)
            {
                double gb = mb / 1024.0;
                return $"{gb:F2} GB";
            }

            return $"{mb:F1} MB";
        }

        /// <inheritdoc />
        public async Task<OllamaStatusInfo> GetDetailedStatusAsync(CancellationToken ct = default)
        {
            var endpoint = GetBaseEndpointUrl();
            var status = new OllamaStatusInfo
            {
                EndpointUrl = endpoint,
                ExecutablePath = FindExecutablePath()
            };

            var processes = GetRunningOllamaProcesses();
            status.ProcessCount = processes.Count;

            long totalWorkingSet = 0;
            foreach (var p in processes)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        p.Refresh();
                        totalWorkingSet += p.WorkingSet64;
                    }
                }
                catch
                {
                    // Bỏ qua nếu tiến trình thoát giữa chừng
                }
            }
            DisposeProcesses(processes);

            status.RamBytes = totalWorkingSet;
            bool isProcessPresent = status.ProcessCount > 0;

            // Kiểm tra kết nối HTTP đến Ollama API
            bool isHttpResponding = false;
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                cts.CancelAfter(TimeSpan.FromSeconds(2));

                var res = await _httpClient.GetAsync($"{endpoint}/api/ps", cts.Token).ConfigureAwait(false);
                if (res.IsSuccessStatusCode)
                {
                    isHttpResponding = true;
                    string json = await res.Content.ReadAsStringAsync(cts.Token).ConfigureAwait(false);
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("models", out var modelsArr) && modelsArr.ValueKind == JsonValueKind.Array)
                    {
                        status.LoadedModelCount = modelsArr.GetArrayLength();
                        foreach (var m in modelsArr.EnumerateArray())
                        {
                            if (m.TryGetProperty("name", out var n) && n.ValueKind == JsonValueKind.String)
                            {
                                string name = n.GetString() ?? string.Empty;
                                if (!string.IsNullOrEmpty(name))
                                {
                                    status.LoadedModels.Add(name);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // HTTP có thể chưa mở nếu tiến trình mới khởi động hoặc đang tắt
            }

            status.IsRunning = isProcessPresent || isHttpResponding;

            if (status.IsRunning)
            {
                string baseRamStr = FormatBytes(status.RamBytes);
                if (status.LoadedModelCount > 0)
                {
                    string modelSummary = string.Join(", ", status.LoadedModels);
                    status.RamUsageFormatted = $"{baseRamStr} ({modelSummary})";
                }
                else
                {
                    status.RamUsageFormatted = baseRamStr;
                }
            }
            else
            {
                status.RamUsageFormatted = "0 MB (Đã dừng)";
            }

            return status;
        }

        /// <inheritdoc />
        public async Task<(bool Success, string Message)> StartServerAsync(CancellationToken ct = default)
        {
            var endpoint = GetBaseEndpointUrl();

            // Kiểm tra nếu đang chạy
            if (IsProcessRunning())
            {
                // Thử ping HTTP để xác nhận cổng đã mở
                try
                {
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    cts.CancelAfter(TimeSpan.FromSeconds(3));
                    var ping = await _httpClient.GetAsync($"{endpoint}/api/tags", cts.Token).ConfigureAwait(false);
                    if (ping.IsSuccessStatusCode)
                    {
                        return (true, $"Máy chủ Ollama đã đang hoạt động trên cổng {endpoint}.");
                    }
                }
                catch { }

                return (true, $"Tiến trình máy chủ Ollama đã đang hoạt động trên hệ thống (cổng {endpoint}).");
            }

            string? exePath = FindExecutablePath();
            if (string.IsNullOrEmpty(exePath) || !File.Exists(exePath))
            {
                return (false, "Không tìm thấy tệp thực thi ollama.exe trên máy tính.\nVui lòng cài đặt Ollama từ trang chính thức: https://ollama.com");
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = "serve",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path.GetDirectoryName(exePath) ?? string.Empty
                };

                // Tránh redirect streams để không bao giờ bị nghẽn buffer 4KB của Windows
                psi.RedirectStandardOutput = false;
                psi.RedirectStandardError = false;

                var startedProc = Process.Start(psi);
                if (startedProc == null)
                {
                    return (false, "Không thể khởi động tiến trình ollama.exe (Process.Start trả về null).");
                }

                // Poll HTTP endpoint trong tối đa 10 giây (mỗi 300ms) để xác nhận máy chủ sẵn sàng
                var stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < 10000 && !ct.IsCancellationRequested)
                {
                    await Task.Delay(300, ct).ConfigureAwait(false);
                    try
                    {
                        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                        cts.CancelAfter(TimeSpan.FromSeconds(1));
                        var check = await _httpClient.GetAsync($"{endpoint}/api/tags", cts.Token).ConfigureAwait(false);
                        if (check.IsSuccessStatusCode)
                        {
                            return (true, $"Máy chủ Ollama đã khởi động thành công và sẵn sàng nhận kết nối tại {endpoint}!");
                        }
                    }
                    catch
                    {
                        // Máy chủ đang khởi tạo socket
                    }
                }

                // Nếu sau 10s HTTP chưa phản hồi nhưng tiến trình vẫn chạy
                if (IsProcessRunning())
                {
                    return (true, "Tiến trình Ollama đã được kích hoạt chạy ngầm. Cổng dịch vụ đang tiếp tục được khởi tạo.");
                }

                return (false, "Tiến trình Ollama đã thoát đột ngột sau khi khởi chạy. Vui lòng kiểm tra cổng 11434 có bị ứng dụng khác chiếm dụng không.");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi khởi chạy tiến trình Ollama: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<(bool Success, string Message, long FreedBytes)> StopServerAsync(CancellationToken ct = default)
        {
            var processes = GetRunningOllamaProcesses();
            if (processes.Count == 0)
            {
                return (true, "Máy chủ Ollama hiện tại không chạy.", 0);
            }

            long ramBeforeStop = 0;
            foreach (var p in processes)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        p.Refresh();
                        ramBeforeStop += p.WorkingSet64;
                    }
                }
                catch { }
            }

            int killCount = 0;
            List<string> errorDetails = new();

            foreach (var p in processes)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        // Kill toàn bộ cây tiến trình (bao gồm các child process như llama-server)
                        p.Kill(entireProcessTree: true);
                        p.WaitForExit(1500);
                        killCount++;
                    }
                }
                catch (Exception ex)
                {
                    errorDetails.Add($"{p.ProcessName} (PID {p.Id}): {ex.Message}");
                }
                finally
                {
                    p.Dispose();
                }
            }

            await Task.Delay(400, ct).ConfigureAwait(false);

            // Kiểm tra dự phòng: nếu vẫn còn tiến trình sót lại, chạy taskkill cưỡng chế
            var lingering = GetRunningOllamaProcesses();
            if (lingering.Count > 0)
            {
                DisposeProcesses(lingering);
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "taskkill",
                        Arguments = "/F /T /IM ollama.exe /IM \"ollama app.exe\" /IM llama-server.exe /IM ollama_llama_server.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    using var killProc = Process.Start(psi);
                    killProc?.WaitForExit(2000);
                }
                catch { }

                await Task.Delay(300, ct).ConfigureAwait(false);
            }

            long ramAfterStop = GetTotalWorkingSetBytes();
            long freedBytes = Math.Max(0, ramBeforeStop - ramAfterStop);
            string freedFormatted = FormatBytes(ramBeforeStop > 0 ? ramBeforeStop : freedBytes);

            if (GetRunningOllamaProcesses().Count == 0)
            {
                return (true, $"Đã dừng toàn bộ tiến trình Ollama và giải phóng {freedFormatted} RAM cho hệ điều hành.", freedBytes);
            }

            if (errorDetails.Count > 0)
            {
                return (false, $"Không thể dừng triệt để một số tiến trình do phân quyền: {string.Join("; ", errorDetails)}", freedBytes);
            }

            return (true, $"Đã gửi lệnh dừng tiến trình Ollama. Đã giải phóng {freedFormatted} RAM.", freedBytes);
        }

        /// <inheritdoc />
        public async Task<(bool Success, string Message)> ReleaseMemoryAsync(CancellationToken ct = default)
        {
            var endpoint = GetBaseEndpointUrl();
            if (!IsProcessRunning())
            {
                return (false, "Máy chủ Ollama hiện chưa được khởi động.");
            }

            try
            {
                using var overallCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                overallCts.CancelAfter(TimeSpan.FromSeconds(15));

                using var psCts = CancellationTokenSource.CreateLinkedTokenSource(overallCts.Token);
                psCts.CancelAfter(TimeSpan.FromSeconds(5));

                var psRes = await _httpClient.GetAsync($"{endpoint}/api/ps", psCts.Token).ConfigureAwait(false);
                if (!psRes.IsSuccessStatusCode)
                {
                    return (false, "Không thể kết nối đến API Ollama để truy vấn danh sách mô hình đang nạp.");
                }

                string body = await psRes.Content.ReadAsStringAsync(psCts.Token).ConfigureAwait(false);
                using var doc = JsonDocument.Parse(body);
                int unloadedCount = 0;
                List<string> unloadedModelNames = new();

                if (doc.RootElement.TryGetProperty("models", out var arr) && arr.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in arr.EnumerateArray())
                    {
                        if (item.TryGetProperty("name", out var nProp))
                        {
                            string modelName = nProp.GetString() ?? string.Empty;
                            if (!string.IsNullOrEmpty(modelName))
                            {
                                // Gửi request keep_alive: 0 tới /api/generate để Ollama dỡ bỏ model khỏi VRAM/RAM
                                var payload = JsonSerializer.Serialize(new { model = modelName, keep_alive = 0 });
                                using var content = new StringContent(payload, Encoding.UTF8, "application/json");

                                using var unloadCts = CancellationTokenSource.CreateLinkedTokenSource(overallCts.Token);
                                unloadCts.CancelAfter(TimeSpan.FromSeconds(5));

                                var unloadRes = await _httpClient.PostAsync($"{endpoint}/api/generate", content, unloadCts.Token).ConfigureAwait(false);
                                if (unloadRes.IsSuccessStatusCode)
                                {
                                    unloadedCount++;
                                    unloadedModelNames.Add(modelName);
                                }
                            }
                        }
                    }
                }

                if (unloadedCount > 0)
                {
                    string names = string.Join(", ", unloadedModelNames);
                    return (true, $"Đã giải phóng thành công {unloadedCount} mô hình ({names}) khỏi bộ nhớ RAM.\nBộ nhớ đã được trả về trạng thái tối thiểu!");
                }

                return (true, "Hiện tại không có mô hình nào đang giữ trong bộ nhớ RAM.");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi giải phóng bộ nhớ mô hình Ollama: {ex.Message}");
            }
        }

        private List<Process> GetRunningOllamaProcesses()
        {
            var result = new List<Process>();
            var seenIds = new HashSet<int>();

            foreach (var name in _targetProcessNames)
            {
                try
                {
                    var procs = Process.GetProcessesByName(name);
                    foreach (var p in procs)
                    {
                        if (seenIds.Add(p.Id))
                        {
                            result.Add(p);
                        }
                        else
                        {
                            p.Dispose();
                        }
                    }
                }
                catch
                {
                    // Tiếp tục với các tiến trình khác
                }
            }

            return result;
        }

        private static void DisposeProcesses(IEnumerable<Process> processes)
        {
            foreach (var p in processes)
            {
                try
                {
                    p.Dispose();
                }
                catch { }
            }
        }
    }
}
