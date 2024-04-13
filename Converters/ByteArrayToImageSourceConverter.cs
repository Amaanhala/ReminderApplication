using System.Globalization;

namespace ReminderApplication.Converters;

public class ByteArrayToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        byte[] bytes = value as byte[];
        if (bytes == null)
            return null;

        var stream = new MemoryStream(bytes);
        return ImageSource.FromStream(() => stream);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
