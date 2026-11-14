using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DTO.Assistant;

namespace BUS.Assistant
{
    /// <summary>
    /// Định nghĩa cấu trúc và kiểu dữ liệu của một tham số trong công cụ Trợ lý AI.
    /// </summary>
    public class ToolParameterDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "string"; // "string" | "int" | "bool" | "object" | "array"
        public string Description { get; set; } = string.Empty;
        public bool IsRequired { get; set; } = true;
        public IReadOnlyList<string>? AllowedValues { get; set; }
        public object? DefaultValue { get; set; }

        public ToolParameterDefinition() { }

        public ToolParameterDefinition(
            string name, 
            string type, 
            string description, 
            bool isRequired = true, 
            IReadOnlyList<string>? allowedValues = null, 
            object? defaultValue = null)
        {
            Name = name;
            Type = type;
            Description = description;
            IsRequired = isRequired;
            AllowedValues = allowedValues;
            DefaultValue = defaultValue;
        }

        public AssistantToolParameterDTO ToDTO()
        {
            return new AssistantToolParameterDTO
            {
                Name = Name,
                Type = Type,
                Description = Description,
                IsRequired = IsRequired,
                AllowedValues = AllowedValues != null ? AllowedValues.ToList() : null,
                DefaultValue = DefaultValue
            };
        }
    }

    /// <summary>
    /// Ngữ cảnh hiện tại khi người dùng gửi yêu cầu đến Trợ lý AI.
    /// Mang thông tin phiên đăng nhập, trang hiện tại, và mục tiêu đào tạo/học tập đang xem.
    /// </summary>
    public class AssistantContext
    {
        public int UserId { get; set; } = 0;
        public string Username { get; set; } = "khach";
        public string FullName { get; set; } = "Khách vãng lai";
        public string UserRole { get; set; } = "Khach"; // "QuanTriVien" | "GiangVien" | "SinhVien" | "Khach"
        public string CurrentPage { get; set; } = "General";
        public string CurrentPageTitle { get; set; } = "Màn hình chung";
        public int? TargetId { get; set; }
        public string? TargetType { get; set; } // "Lesson" | "Exam" | "Question" | "User"
        public Dictionary<string, object?> AdditionalContext { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public bool IsAdmin => string.Equals(UserRole, "QuanTriVien", StringComparison.OrdinalIgnoreCase) || 
                               string.Equals(UserRole, "Admin", StringComparison.OrdinalIgnoreCase);

        public bool IsLecturer => IsAdmin || string.Equals(UserRole, "GiangVien", StringComparison.OrdinalIgnoreCase);

        public bool IsStudent => string.Equals(UserRole, "SinhVien", StringComparison.OrdinalIgnoreCase) || 
                                 string.Equals(UserRole, "HocVien", StringComparison.OrdinalIgnoreCase);

        public bool IsGuest => string.Equals(UserRole, "Khach", StringComparison.OrdinalIgnoreCase) || UserId <= 0;
    }

    /// <summary>
    /// Kết quả thực thi một công cụ Assistant Tool.
    /// </summary>
    public class AssistantToolResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string? CardType { get; set; }
        public string? CardPayloadJson { get; set; }
        public bool RequiresConfirmation { get; set; } = false;
        public string? ConfirmationToken { get; set; }
        public string? ErrorCode { get; set; }

        public AssistantToolResult() { }

        public static AssistantToolResult Ok(string message, object? data = null, string? cardType = null, string? cardPayloadJson = null)
        {
            return new AssistantToolResult
            {
                Success = true,
                Message = message,
                Data = data,
                CardType = cardType,
                CardPayloadJson = cardPayloadJson
            };
        }

        public static AssistantToolResult OkWithJsonPayload<T>(string message, T payload, string cardType)
        {
            return new AssistantToolResult
            {
                Success = true,
                Message = message,
                Data = payload,
                CardType = cardType,
                CardPayloadJson = JsonSerializer.Serialize(payload)
            };
        }

        public static AssistantToolResult RequireConfirmation(
            string message, 
            string confirmationToken, 
            string? cardType = "preview_card", 
            string? cardPayloadJson = null, 
            object? previewData = null)
        {
            return new AssistantToolResult
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

        public static AssistantToolResult Fail(string message, string? errorCode = "TOOL_ERROR", string? cardType = "error_card")
        {
            return new AssistantToolResult
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                CardType = cardType
            };
        }

        public AssistantToolResultDTO ToDTO()
        {
            return new AssistantToolResultDTO
            {
                Success = Success,
                Message = Message,
                Data = Data,
                CardType = CardType,
                CardPayloadJson = CardPayloadJson,
                RequiresConfirmation = RequiresConfirmation,
                ConfirmationToken = ConfirmationToken,
                ErrorCode = ErrorCode
            };
        }
    }

    /// <summary>
    /// Interface cốt lõi cho mọi công cụ nghiệp vụ trong Trợ lý AI EAI.
    /// </summary>
    public interface IAssistantTool
    {
        string Name { get; }
        string Description { get; }
        string Category { get; } // "Search" | "Statistics" | "ContentGen" | "Learning" | "Navigation" | "System"
        string RequiredRole { get; } // "Any" | "SinhVien" | "GiangVien" | "QuanTriVien"
        bool IsWriteOperation { get; }
        bool RequiresConfirmation { get; }
        IReadOnlyList<ToolParameterDefinition> Parameters { get; }

        Task<AssistantToolResult> ExecuteAsync(Dictionary<string, object?> arguments, AssistantContext context, CancellationToken ct = default);

        AssistantToolMetadataDTO GetMetadata()
        {
            return new AssistantToolMetadataDTO
            {
                Name = Name,
                Description = Description,
                Category = Category,
                RequiredRole = RequiredRole,
                IsWriteOperation = IsWriteOperation,
                RequiresConfirmation = RequiresConfirmation,
                Parameters = Parameters.Select(p => p.ToDTO()).ToList()
            };
        }
    }
}
