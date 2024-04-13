using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ReminderApplication.Converters
{
    public class WalkTimeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (TimeSpan)value != TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
