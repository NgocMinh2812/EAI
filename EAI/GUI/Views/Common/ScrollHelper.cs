using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.Common
{
    public static class ScrollHelper
    {
        public static readonly DependencyProperty FixMouseWheelBubbleProperty =
            DependencyProperty.RegisterAttached(
                "FixMouseWheelBubble",
                typeof(bool),
                typeof(ScrollHelper),
                new PropertyMetadata(false, OnFixMouseWheelBubbleChanged));

        public static bool GetFixMouseWheelBubble(DependencyObject obj)
        {
            return (bool)obj.GetValue(FixMouseWheelBubbleProperty);
        }

        public static void SetFixMouseWheelBubble(DependencyObject obj, bool value)
        {
            obj.SetValue(FixMouseWheelBubbleProperty, value);
        }

        private static void OnFixMouseWheelBubbleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if ((bool)e.NewValue)
                {
                    element.PreviewMouseWheel += Element_PreviewMouseWheel;
                }
                else
                {
                    element.PreviewMouseWheel -= Element_PreviewMouseWheel;
                }
            }
        }

        private static void Element_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                ScrollViewer? scv = sender as ScrollViewer ?? (sender is DependencyObject d ? FindVisualChild<ScrollViewer>(d) : null);
                if (scv != null && scv.ScrollableHeight > 0)
                {
                    if (scv.CanContentScroll)
                    {
                        // Cuộn theo từng item (1 item mỗi khấc lăn chuột), không nhảy vọt xuống đáy
                        double step = Math.Sign(e.Delta) > 0 ? -1.0 : 1.0;
                        double newOffset = Math.Clamp(scv.VerticalOffset + step, 0, scv.ScrollableHeight);
                        scv.ScrollToVerticalOffset(newOffset);
                    }
                    else
                    {
                        // Cuộn theo pixel mượt mà (32px mỗi khấc lăn chuột)
                        double step = -(Math.Sign(e.Delta) * 32.0);
                        double newOffset = Math.Clamp(scv.VerticalOffset + step, 0, scv.ScrollableHeight);
                        scv.ScrollToVerticalOffset(newOffset);
                    }
                    e.Handled = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// Đóng tất cả dropdown ComboBox đang mở trên toàn bộ các cửa sổ của ứng dụng.
        /// </summary>
        public static void CloseAllOpenComboBoxes()
        {
            try
            {
                if (Application.Current == null) return;
                foreach (Window win in Application.Current.Windows)
                {
                    if (win == null) continue;
                    CloseOpenComboBoxesInternal(win);
                }
            }
            catch { }
        }

        private static void CloseOpenComboBoxesInternal(DependencyObject parent)
        {
            if (parent == null) return;
            try
            {
                if (parent is Visual || parent is System.Windows.Media.Media3D.Visual3D)
                {
                    int count = VisualTreeHelper.GetChildrenCount(parent);
                    for (int i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(parent, i);
                        if (child is ComboBox cb)
                        {
                            if (cb.IsDropDownOpen)
                            {
                                cb.IsDropDownOpen = false;
                            }
                        }
                        else
                        {
                            CloseOpenComboBoxesInternal(child);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Xử lý cuộn mượt cho ListBox hoặc ScrollViewer cột chính:
        /// - Bỏ qua nếu chuột đang nằm trên Popup dropdown hoặc ComboBox đang mở dropdown.
        /// - Tự động đóng các dropdown đang mở khi người dùng cuộn cột ngoài.
        /// - An toàn tuyệt đối với mọi cấu trúc Visual/FrameworkContentElement (tránh crash do Paragraph/Run).
        /// </summary>
        public static void HandleSmoothPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                if (sender is DependencyObject dep)
                {
                    var myScv = dep as ScrollViewer ?? FindVisualChild<ScrollViewer>(dep);
                    if (myScv == null || myScv.ScrollableHeight <= 0)
                    {
                        return;
                    }

                    if (e.OriginalSource is DependencyObject orig)
                    {
                        // 1. Nếu chuột nằm trên Popup dropdown, nhường quyền cho dropdown tự cuộn
                        if (IsInsidePopup(orig))
                        {
                            return;
                        }

                        // 2. Nếu chuột nằm trên ComboBox ĐANG MỞ dropdown, nhường quyền cho ComboBox
                        var cb = FindVisualParent<ComboBox>(orig);
                        if (cb != null && cb.IsDropDownOpen)
                        {
                            return;
                        }

                        // 3. Nếu chuột nằm trong một ScrollViewer con khác (không phải myScv) và ScrollViewer con đó thực sự có nội dung cuộn được (như RichTextBox dài)
                        var childScv = FindVisualParent<ScrollViewer>(orig);
                        if (childScv != null && childScv != myScv && childScv.ScrollableHeight > 0)
                        {
                            return;
                        }
                    }

                    // Người dùng đang cuộn cột ngoài hoặc ListBox: đóng ngay bất kỳ dropdown nào đang mở để không bị tách rời
                    CloseAllOpenComboBoxes();

                    double step = -(e.Delta * 0.35);
                    double targetOffset = Math.Clamp(myScv.VerticalOffset + step, 0, myScv.ScrollableHeight);
                    myScv.ScrollToVerticalOffset(targetOffset);
                    e.Handled = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// Kiểm tra xem một DependencyObject có nằm trong Popup (như Dropdown của ComboBox) hay không.
        /// </summary>
        public static bool IsInsidePopup(DependencyObject? obj)
        {
            if (obj == null) return false;
            try
            {
                if (obj is Visual visual)
                {
                    var src = PresentationSource.FromVisual(visual);
                    if (src?.RootVisual != null && src.RootVisual.GetType().Name.Contains("Popup"))
                    {
                        return true;
                    }
                }

                DependencyObject? current = obj;
                while (current != null)
                {
                    var typeName = current.GetType().Name;
                    if (typeName.Contains("Popup"))
                    {
                        return true;
                    }

                    if (current is Visual || current is System.Windows.Media.Media3D.Visual3D)
                    {
                        current = VisualTreeHelper.GetParent(current);
                    }
                    else if (current is FrameworkContentElement fce)
                    {
                        current = fce.Parent;
                    }
                    else
                    {
                        current = LogicalTreeHelper.GetParent(current);
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Tìm con cháu kiểu T trong Visual Tree hoặc Logical Tree một cách an toàn.
        /// </summary>
        public static T? FindVisualChild<T>(DependencyObject? parent) where T : DependencyObject
        {
            if (parent == null) return null;
            try
            {
                if (parent is Visual || parent is System.Windows.Media.Media3D.Visual3D)
                {
                    int count = VisualTreeHelper.GetChildrenCount(parent);
                    for (int i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(parent, i);
                        if (child is T typedChild) return typedChild;
                        var found = FindVisualChild<T>(child);
                        if (found != null) return found;
                    }
                }
                else if (parent is FrameworkContentElement fce)
                {
                    foreach (var logicalChild in LogicalTreeHelper.GetChildren(fce))
                    {
                        if (logicalChild is DependencyObject dep)
                        {
                            if (dep is T typedChild) return typedChild;
                            var found = FindVisualChild<T>(dep);
                            if (found != null) return found;
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Tìm tổ tiên kiểu T duyệt an toàn qua cả Visual Tree lẫn FrameworkContentElement (Paragraph, Run, Span...).
        /// Ngăn chặn triệt để InvalidOperationException khi chuột nằm trên vùng Text/Paragraph.
        /// </summary>
        public static T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T typed) return typed;

                try
                {
                    if (child is Visual || child is System.Windows.Media.Media3D.Visual3D)
                    {
                        child = VisualTreeHelper.GetParent(child);
                    }
                    else if (child is FrameworkContentElement fce)
                    {
                        child = fce.Parent;
                    }
                    else
                    {
                        child = LogicalTreeHelper.GetParent(child);
                    }
                }
                catch
                {
                    break;
                }
            }
            return null;
        }
    }
}
