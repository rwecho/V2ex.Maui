using System.Globalization;

namespace V2ex.Maui.Converters;

public class SimpleTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var text = value?.ToString();

        if (!string.IsNullOrEmpty(text))
        {
            text.Replace("<br/>", "\r\n").Replace("<br>", "\r\n");
        }

        return text;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}