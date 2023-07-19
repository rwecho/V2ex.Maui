using System.Globalization;

namespace V2ex.Maui.Converters;

public class SingleObjectToArray : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Array.Empty<object>();
        }
        return new object[] { value };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}