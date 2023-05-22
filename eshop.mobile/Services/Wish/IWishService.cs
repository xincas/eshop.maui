using Eshop.Mobile.Models;

namespace Eshop.Mobile.Services.Wish;

public interface IWishService
{
    Task<IEnumerable<Product>> GetWishesAsync();
    Task AddProductToWishesAsync(Product product);
    Task DeleteProductToWishesAsync(Product product);
    Task<IEnumerable<Product>> GetWishesAsync(Func<ProductDb, bool> predicate);
}