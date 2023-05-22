namespace Eshop.Mobile.Models;

public record SubCategory(
    long Id,
    string Title,
    string ImageUrl,
    IEnumerable<long> Products,
    IEnumerable<long> Categories);