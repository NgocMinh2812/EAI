using System;

namespace BUS.AI.Models
{
    /// <summary>
    /// Thông tin mô hình AI chuẩn hóa, phân tách Model ID thực tế dùng trong API
    /// với tên hiển thị trực quan (Display Name) trên giao diện.
    /// </summary>
    public class AIModelInfo
    {
        /// <summary>
        /// Mã định danh mô hình dùng khi gửi request tới API (ví dụ: "llama-3.3-70b-versatile", "qwen/qwen3.5-flash:free")
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Tên mô hình hiển thị thân thiện trên UI (ví dụ: "Llama 3.3 70B Versatile", "Qwen 3.5 Flash (Free)")
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Tên nhà cung cấp (ví dụ: "Google Gemini", "Groq Cloud", "xKiro AI Gateway")
        /// </summary>
        public string ProviderName { get; set; } = string.Empty;

        /// <summary>
        /// Loại nhà cung cấp theo enum
        /// </summary>
        public AIProviderType ProviderType { get; set; } = AIProviderType.Gemini;

        /// <summary>
        /// Đơn vị sở hữu / hãng phát triển (ví dụ: "meta", "google", "qwen", "openai", "nvidia", "cohere")
        /// </summary>
        public string OwnedBy { get; set; } = string.Empty;

        /// <summary>
        /// Phân hạng truy cập: "free" (miễn phí), "paid" (trả phí), "premium", hoặc "standard"
        /// </summary>
        public string AccessTier { get; set; } = "free";

        /// <summary>
        /// Loại tác vụ hỗ trợ: "chat", "image", "tts", "embedding", v.v.
        /// </summary>
        public string Modality { get; set; } = "chat";

        /// <summary>
        /// Độ dài ngữ cảnh tối đa (tokens) nếu nhà cung cấp công bố
        /// </summary>
        public int? ContextLength { get; set; }

        /// <summary>
        /// Mô hình lấy trực tiếp từ API catalog động (true) hay từ danh mục dự phòng (false)
        /// </summary>
        public bool IsDynamic { get; set; } = false;

        /// <summary>
        /// Đánh dấu đây là mô hình mặc định của nhà cung cấp
        /// </summary>
        public bool DefaultModel { get; set; } = false;

        /// <summary>
        /// Mô hình còn khả dụng hay đã bị ngừng hỗ trợ / deprecated
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Ghi chú cảnh báo nếu mô hình bị ngừng hỗ trợ
        /// </summary>
        public string DeprecationNote { get; set; } = string.Empty;

        /// <summary>
        /// Chuỗi hiển thị đầy đủ gồm Tên + Model ID + Nhãn phân hạng truy cập
        /// </summary>
        public string FullDisplay
        {
            get
            {
                string tierTag = "";
                if (!string.IsNullOrWhiteSpace(AccessTier) && !AccessTier.Equals("standard", StringComparison.OrdinalIgnoreCase))
                {
                    tierTag = AccessTier.Equals("free", StringComparison.OrdinalIgnoreCase) ? " [Miễn phí]" : $" [{AccessTier.ToUpper()}]";
                }

                if (string.Equals(DisplayName, Id, StringComparison.OrdinalIgnoreCase))
                {
                    return $"{Id}{tierTag}";
                }

                return $"{DisplayName} ({Id}){tierTag}";
            }
        }

        public override string ToString() => FullDisplay;
    }
}
