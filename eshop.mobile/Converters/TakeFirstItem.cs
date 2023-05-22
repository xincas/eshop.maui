using System.Globalization;

namespace Eshop.Mobile.Converters;

public class TakeFirstItem : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);

        var s = (IEnumerable<object>)value;

        if (!s.Any()) return null;

        return s.First();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}