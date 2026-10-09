using System;
using System.Collections.ObjectModel;

namespace DTO
{
    public class TreeNodeDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Icon { get; set; } = "📁";
        public int ItemCount { get; set; } = 0;
        public string BadgeText => ItemCount > 0 ? $"({ItemCount})" : "";
        public bool IsExpanded { get; set; } = true;
        public bool IsSelected { get; set; } = false;
        public object? Tag { get; set; }

        public ObservableCollection<TreeNodeDTO> Children { get; set; } = new ObservableCollection<TreeNodeDTO>();
    }
}
