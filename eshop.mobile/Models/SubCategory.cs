namespace Eshop.Mobile.Models;

public record SubCategory(
    long Id,
    string Title,
    string ImageUrl,
    IEnumerable<long> Products,
    IEnumerable<long> Categories) : ICategory;

public interface ICategory
{
    long Id { get; }
    string Title { get; }
    string ImageUrl { get; }
    IEnumerable<long> Products { get; }
}