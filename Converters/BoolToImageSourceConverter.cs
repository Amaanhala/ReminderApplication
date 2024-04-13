using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ReminderApplication.Converters
{
    public class BoolToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFlipped && parameter is string imagePath)
            {
                return isFlipped ? new Image { Source = imagePath }.Source : Microsoft.Maui.Graphics.Colors.LightGray; // Change the color to your desired placeholder color
            }

            return Microsoft.Maui.Graphics.Colors.LightGray; // Change the color to your desired placeholder color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}



