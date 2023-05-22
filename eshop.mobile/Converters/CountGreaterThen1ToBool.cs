using System.Globalization;

namespace Eshop.Mobile.Converters;

public class CountGreaterThen1ToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);

        return ((IEnumerable<object>)value).Count() > 1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return false;
    }
}