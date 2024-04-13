using System;
using System.Globalization;
using System.IO;
using Microsoft.Maui.Controls;

namespace ReminderApplication.Converters
{
    public class FileNameFromPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filePath)
            {
                return Path.GetFileName(filePath);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
