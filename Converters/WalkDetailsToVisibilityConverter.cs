using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ReminderApplication.Converters
{
    public class WalkDetailsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
