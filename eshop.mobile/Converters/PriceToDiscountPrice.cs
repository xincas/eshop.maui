using System.Globalization;

namespace Eshop.Mobile.Converters;

public class PriceToDiscountPrice : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(parameter);

        return (decimal)value * (1 + decimal.Parse((string)parameter, CultureInfo.InvariantCulture));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}