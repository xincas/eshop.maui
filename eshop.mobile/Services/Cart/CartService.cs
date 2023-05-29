using Eshop.Mobile.Models;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.DataBase;

namespace Eshop.Mobile.Services.Cart;

public class CartService : ICartService
{
    private readonly IDatabaseService _databaseService;

    public CartService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    /// <summary>
    /// Method add product to cart db
    /// </summary>
    /// <param name="product"></param>
    /// <returns>
    /// -1 if adding didn't applied
    /// 1 if operation was success
    /// </returns>
    public async Task<int> AddProductToCartAsync(Product product)
    {
        var id = await _databaseService.AddProductToCartAsync(product);

        if (id == 0) return -1;

        product.Quantity = 1;
        product.InCart = true;
        return id;
    }

    public Task ClearCartAsync()
    {
        return _databaseService.ClearCartAsync();
    }

    public Task<int> DeleteProductToCartAsync(Product product)
    {
        return _databaseService.DeleteProductFromCartAsync(product);
    }

    /// <summary>
    /// Method change product quantity prop
    /// </summary>
    /// <param name="product"></param>
    /// <param name="deltaQuantity"></param>
    /// <returns>
    ///-1 if product not in cart
    /// 1 if product change quantity prop
    /// 0 if product deleted from cart
    /// </returns>
    public async Task<int> UpdateQuantityOfCartItemAsync(Product product, int deltaQuantity)
    {
        var item = await _databaseService.GetCartItemAsync(product.Id);

        if (item == null)
        {
            product.Quantity = 0;
            product.InCart = false;
            return -1;
        }

        if (item.Quantity + deltaQuantity > 0)
        {
            product.Quantity = item.Quantity + deltaQuantity;
            await _databaseService.UpdateCartItemQuantityAsync(product, product.Quantity);
            return 1;
        }
        else
        {
            product.Quantity = 0;
            product.InCart = false;
            await _databaseService.DeleteProductFromCartAsync(product);
            return 0;
        }
    }

    public Task<IEnumerable<CartItem>> GetAllAsync()
    {
        return _databaseService.GetCartAsync();
    }

    public Task<CartItem> GetByIdAsync(long id)
    {
        return _databaseService.GetCartItemAsync(id);
    }

    /// <summary>
    /// Method change product quantity prop
    /// </summary>
    /// <param name="product"></param>
    /// <returns>
    /// false if cart item don't present
    /// true if operation was success
    /// </returns>
    public async Task<bool> FetchProduct(Product product)
    {
        if (product is null) return false;

        var cartItem = await _databaseService.GetCartItemAsync(product.Id);

        if (cartItem == null) return false;

        product.Quantity = cartItem.Quantity;
        product.InCart = true;
        return true;
    }
}