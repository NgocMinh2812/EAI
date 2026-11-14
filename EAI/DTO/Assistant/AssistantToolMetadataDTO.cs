using System;
using System.Collections.Generic;

namespace DTO.Assistant
{
    /// <summary>
    /// Định nghĩa tham số đầu vào của một công cụ Trợ lý AI.
    /// </summary>
    public class AssistantToolParameterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "string"; // "string" | "int" | "bool" | "object" | "array"
        public string Description { get; set; } = string.Empty;
        public bool IsRequired { get; set; } = true;
        public List<string>? AllowedValues { get; set; }
        public object? DefaultValue { get; set; }

        public AssistantToolParameterDTO() { }

        public AssistantToolParameterDTO(string name, string type, string description, bool isRequired = true, List<string>? allowedValues = null, object? defaultValue = null)
        {
            Name = name;
            Type = type;
            Description = description;
            IsRequired = isRequired;
            AllowedValues = allowedValues;
            DefaultValue = defaultValue;
        }
    }

    /// <summary>
    /// DTO chứa thông tin mô tả (metadata) của một công cụ Trợ lý AI.
    /// Dùng để tạo System Prompt, sinh JSON Schema và hiển thị trên giao diện trợ lý.
    /// </summary>
    public class AssistantToolMetadataDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General"; // "Search" | "Statistics" | "ContentGen" | "Learning" | "Navigation" | "System"
        public string RequiredRole { get; set; } = "Any"; // "Any" | "QuanTriVien" | "GiangVien" | "SinhVien"
        public bool IsWriteOperation { get; set; } = false;
        public bool RequiresConfirmation { get; set; } = false;
        public List<AssistantToolParameterDTO> Parameters { get; set; } = new();

        public AssistantToolMetadataDTO() { }

        public AssistantToolMetadataDTO(
            string name, 
            string description, 
            string category, 
            string requiredRole, 
            bool isWriteOperation = false, 
            bool requiresConfirmation = false, 
            List<AssistantToolParameterDTO>? parameters = null)
        {
            Name = name;
            Description = description;
            Category = category;
            RequiredRole = requiredRole;
            IsWriteOperation = isWriteOperation;
            RequiresConfirmation = requiresConfirmation;
            if (parameters != null)
            {
                Parameters = parameters;
            }
        }
    }
}
