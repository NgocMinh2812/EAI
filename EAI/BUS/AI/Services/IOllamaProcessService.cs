using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BUS.AI.Services
{
    /// <summary>
    /// Thông tin chi tiết về trạng thái tiến trình và mức tiêu thụ tài nguyên của máy chủ Ollama
    /// </summary>
    public class OllamaStatusInfo
    {
        public bool IsRunning { get; set; }
        public long RamBytes { get; set; }
        public string RamUsageFormatted { get; set; } = "0 MB (Đã dừng)";
        public int LoadedModelCount { get; set; }
        public List<string> LoadedModels { get; set; } = new();
        public string? ExecutablePath { get; set; }
        public bool IsInstalled => !string.IsNullOrEmpty(ExecutablePath) && System.IO.File.Exists(ExecutablePath);
        public int ProcessCount { get; set; }
        public string EndpointUrl { get; set; } = "http://localhost:11434";
    }

    /// <summary>
    /// Hợp đồng điều khiển vòng đời máy chủ Ollama và đo lường tài nguyên RAM
    /// </summary>
    public interface IOllamaProcessService
    {
        /// <summary>
        /// Kiểm tra máy tính đã cài đặt Ollama hay chưa
        /// </summary>
        bool IsInstalled();

        /// <summary>
        /// Dò tìm đường dẫn tệp thực thi ollama.exe trên hệ thống
        /// </summary>
        string? FindExecutablePath();

        /// <summary>
        /// Kiểm tra xem có bất kỳ tiến trình Ollama nào đang chạy không
        /// </summary>
        bool IsProcessRunning();

        /// <summary>
        /// Đo lường tổng bộ nhớ RAM (WorkingSet64) của tất cả tiến trình Ollama đang chạy
        /// </summary>
        long GetTotalWorkingSetBytes();

        /// <summary>
        /// Định dạng số byte RAM thành chuỗi MB / GB trực quan
        /// </summary>
        string FormatBytes(long bytes);

        /// <summary>
        /// Lấy thông tin trạng thái chi tiết (tiến trình, cổng HTTP, RAM chiếm dụng, mô hình đang nạp)
        /// </summary>
        Task<OllamaStatusInfo> GetDetailedStatusAsync(CancellationToken ct = default);

        /// <summary>
        /// Khởi chạy máy chủ Ollama chạy nền (ollama serve)
        /// </summary>
        Task<(bool Success, string Message)> StartServerAsync(CancellationToken ct = default);

        /// <summary>
        /// Dừng triệt để toàn bộ cây tiến trình Ollama để giải phóng 100% RAM
        /// </summary>
        Task<(bool Success, string Message, long FreedBytes)> StopServerAsync(CancellationToken ct = default);

        /// <summary>
        /// Hạ tải (unload) các mô hình AI đang chiếm RAM về 0 MB mà không cần tắt máy chủ
        /// </summary>
        Task<(bool Success, string Message)> ReleaseMemoryAsync(CancellationToken ct = default);
    }
}
