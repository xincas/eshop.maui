using System.Globalization;
using Eshop.Mobile.Models;

namespace Eshop.Mobile.Converters;

public class OrderStatusToText : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return "";

        if (value is OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Processing => "Обрабатывается",
                OrderStatus.Cancelled => "Отменен",
                OrderStatus.Shipping => "Доставляется",
                OrderStatus.Delivered => "Доставлен",
                _ => ""
            };
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}