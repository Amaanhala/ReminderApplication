using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using ReminderApplication.ViewModel;

namespace ReminderApplication.Converters
{
    public class CardImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Card card)
            {
                string imageSource = card.IsFlipped ? card.ImageSource : card.FrontImageSource;
                Console.WriteLine($"Card ID: {card.Id}, IsFlipped: {card.IsFlipped}, ImageSource: {imageSource}");
                return imageSource;
            }

            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
