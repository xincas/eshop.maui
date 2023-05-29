using System.Globalization;
using Eshop.Mobile.Pages;

namespace Eshop.Mobile.Converters;

public class ProdOrderToText : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //ArgumentNullException.ThrowIfNull(value);
        if (value == null) return string.Empty;
        if (value is not string str) return string.Empty;

        return str switch
        {
            ProductOrderingState.Default => "По умолчанию",
            ProductOrderingState.LowerPriceFirst => "Сначала дешёвые",
            ProductOrderingState.HighestPriceFirst => "Сначала дорогие",
            ProductOrderingState.HighestRatingFirst => "Высокий рейтинг",
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}