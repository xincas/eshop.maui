namespace Eshop.Mobile.Models;

public record Category(
    long Id,
    string Title,
    string ImageUrl,
    IEnumerable<long> Products,
    IEnumerable<SubCategory> SubCategories);