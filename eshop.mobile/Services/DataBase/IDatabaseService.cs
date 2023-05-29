using Eshop.Mobile.Models;
using System.Linq.Expressions;

namespace Eshop.Mobile.Services.DataBase;

public interface IDatabaseService
{
    #region Wishes

    public Task<IEnumerable<ProductDb>> GetWishesAsync();
    public Task<IEnumerable<ProductDb>> FindWishesByAsync(Expression<Func<ProductDb, bool>> predicate);
    public Task<bool> WishExistsAsync(Product product);
    public Task<int> AddProductToWishesAsync(Product product);
    public Task<int> DeleteProductFromWishesAsync(Product product);

    #endregion

    #region CartItems

    public Task<IEnumerable<CartItem>> GetCartAsync();
    public Task ClearCartAsync();
    public Task<CartItem> GetCartItemAsync(long id);
    public Task<bool> CartItemExistsAsync(Product product);
    public Task<int> AddProductToCartAsync(Product product);
    public Task<int> UpdateCartItemQuantityAsync(Product product, int quantity);
    public Task<int> DeleteProductFromCartAsync(Product product);

    #endregion
}