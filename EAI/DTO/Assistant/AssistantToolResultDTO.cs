using System;
using System.Text.Json;

namespace DTO.Assistant
{
    /// <summary>
    /// Kết quả thực thi tiêu chuẩn của một công cụ (IAssistantTool) trong Trợ lý AI EAI.
    /// Hỗ trợ cả trả về dữ liệu thuần, thẻ giao diện (card), và yêu cầu xác nhận thao tác ghi an toàn.
    /// </summary>
    public class AssistantToolResultDTO
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string? CardType { get; set; } // "preview_card" | "stats_card" | "users_card" | "lessons_card" | "exams_card" | "navigation_card" | "system_health_card" | "error_card"
        public string? CardPayloadJson { get; set; }
        public bool RequiresConfirmation { get; set; } = false;
        public string? ConfirmationToken { get; set; }
        public string? ErrorCode { get; set; }

        public AssistantToolResultDTO() { }

        public static AssistantToolResultDTO Ok(string message, object? data = null, string? cardType = null, string? cardPayloadJson = null)
        {
            return new AssistantToolResultDTO
            {
                Success = true,
                Message = message,
                Data = data,
                CardType = cardType,
                CardPayloadJson = cardPayloadJson
            };
        }

        public static AssistantToolResultDTO OkWithJsonPayload<T>(string message, T payload, string cardType)
        {
            return new AssistantToolResultDTO
            {
                Success = true,
                Message = message,
                Data = payload,
                CardType = cardType,
                CardPayloadJson = JsonSerializer.Serialize(payload)
            };
        }

        public static AssistantToolResultDTO RequireConfirmation(string message, string confirmationToken, string? cardType = "preview_card", string? cardPayloadJson = null, object? previewData = null)
        {
            return new AssistantToolResultDTO
            {
                Success = true,
                Message = message,
                RequiresConfirmation = true,
                ConfirmationToken = confirmationToken,
                CardType = cardType,
                CardPayloadJson = cardPayloadJson,
                Data = previewData
            };
        }

        public static AssistantToolResultDTO Fail(string message, string? errorCode = "TOOL_EXECUTION_ERROR", string? cardType = "error_card")
        {
            return new AssistantToolResultDTO
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                CardType = cardType
            };
        }
    }
}
