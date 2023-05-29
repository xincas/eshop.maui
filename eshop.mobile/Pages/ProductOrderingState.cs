namespace Eshop.Mobile.Pages;

public static class ProductOrderingState
{
    public const string Default = nameof(Default);
    public const string LowerPriceFirst = nameof(LowerPriceFirst);
    public const string HighestPriceFirst = nameof(HighestPriceFirst);
    public const string HighestRatingFirst = nameof(HighestRatingFirst);

    public static List<string> States = new List<string>()
    {
        Default, LowerPriceFirst, HighestPriceFirst /*, HighestRatingFirst*/
    };
}