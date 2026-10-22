using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GUI.ViewModels.Common
{
    public class StringNotEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class StringNotEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class PublishStatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b) return "✓ Đã xuất bản";
            return "✎ Bản nháp";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class PublishStatusBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b) return new SolidColorBrush(Color.FromRgb(220, 252, 231)); // #DCFCE7
            return new SolidColorBrush(Color.FromRgb(241, 245, 249)); // #F1F5F9
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class PublishStatusForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b) return new SolidColorBrush(Color.FromRgb(21, 128, 61)); // #15803D
            return new SolidColorBrush(Color.FromRgb(100, 116, 139)); // #64748B
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class HealthToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (value as string ?? "").Trim();
            return status switch
            {
                "Healthy" => new SolidColorBrush(Color.FromRgb(21, 128, 61)),      // #15803D
                "Degraded" => new SolidColorBrush(Color.FromRgb(217, 119, 6)),     // #D97706
                "Unavailable" => new SolidColorBrush(Color.FromRgb(220, 38, 38)),   // #DC2626
                "Unverified" => new SolidColorBrush(Color.FromRgb(71, 85, 105)),    // #475569
                _ => new SolidColorBrush(Color.FromRgb(100, 116, 139))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class HealthToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (value as string ?? "").Trim();
            return status switch
            {
                "Healthy" => new SolidColorBrush(Color.FromRgb(220, 252, 231)),    // #DCFCE7
                "Degraded" => new SolidColorBrush(Color.FromRgb(254, 243, 199)),   // #FEF3C7
                "Unavailable" => new SolidColorBrush(Color.FromRgb(254, 226, 226)), // #FEE2E2
                "Unverified" => new SolidColorBrush(Color.FromRgb(241, 245, 249)),  // #F1F5F9
                _ => new SolidColorBrush(Color.FromRgb(241, 245, 249))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class HealthToVietnameseTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = (value as string ?? "").Trim();
            return status switch
            {
                "Healthy" => "Kết nối tốt",
                "Degraded" => "Hiệu năng giảm",
                "Unavailable" => "Không khả dụng",
                "Unverified" => "Chưa xác minh",
                _ => string.IsNullOrWhiteSpace(status) ? "Chưa kiểm tra" : status
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class BooleanToConfiguredTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? "✓ Đã cấu hình API key" : "✗ Chưa cấu hình API key";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class BooleanToConfiguredBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b)
                ? new SolidColorBrush(Color.FromRgb(220, 252, 231))
                : new SolidColorBrush(Color.FromRgb(241, 245, 249));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class BooleanToConfiguredForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b)
                ? new SolidColorBrush(Color.FromRgb(21, 128, 61))
                : new SolidColorBrush(Color.FromRgb(100, 116, 139));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return b ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class BooleanToSuccessErrorForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b) return new SolidColorBrush(Color.FromRgb(21, 128, 61)); // #15803D (Green)
            return new SolidColorBrush(Color.FromRgb(220, 38, 38)); // #DC2626 (Red)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class IntGreaterThanZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i && i > 0) return Visibility.Visible;
            if (value is long l && l > 0) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Visible;
            if (value is string s && string.IsNullOrWhiteSpace(s)) return Visibility.Visible;
            if (value is System.Collections.ICollection c && c.Count == 0) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class NotNullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            if (value is string s && string.IsNullOrWhiteSpace(s)) return Visibility.Collapsed;
            if (value is System.Collections.ICollection c && c.Count == 0) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
