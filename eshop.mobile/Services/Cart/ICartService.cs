using Eshop.Mobile.Models;

namespace Eshop.Mobile.Services.Cart;

public interface ICartService
{
    Task<int> AddProductToCartAsync(Product product);
    Task ClearCartAsync();
    Task<int> DeleteProductToCartAsync(Product product);
    Task<int> UpdateQuantityOfCartItemAsync(Product product, int deltaQuantity);
    Task<IEnumerable<CartItem>> GetAllAsync();
    Task<CartItem> GetByIdAsync(long id);
    Task<bool> FetchProduct(Product product);
}