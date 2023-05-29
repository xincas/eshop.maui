using System.Globalization;

namespace Eshop.Mobile.Converters;

public class WishStateToImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value is not bool val) throw new ArgumentException("value");

        return val
            ? ImageSource.FromResource("HeartSIcon", typeof(App))
            : ImageSource.FromResource("HeartRIcon", typeof(App));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}