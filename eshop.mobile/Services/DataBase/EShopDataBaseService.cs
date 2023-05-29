using System.Diagnostics;
using System.Linq.Expressions;
using Eshop.Mobile.Models;
using SQLite;

namespace Eshop.Mobile.Services.DataBase;

public class EShopDataBaseService : IDatabaseService
{
    SQLiteAsyncConnection _connection;

    public EShopDataBaseService()
    {
    }

    async ValueTask Init()
    {
        if (_connection is not null)
            return;

        _connection = new SQLiteAsyncConnection(GlobalSettings.DatabasePath, GlobalSettings.Flags);
        await _connection.CreateTableAsync<ProductDb>();
        await _connection.CreateTableAsync<CartItem>();
    }

    public async Task<IEnumerable<ProductDb>> GetWishesAsync()
    {
        await Init();
        return await _connection.Table<ProductDb>().ToListAsync();
    }

    public async Task<IEnumerable<ProductDb>> FindWishesByAsync(Expression<Func<ProductDb, bool>> predicate)
    {
        await Init();
        var result = await _connection.Table<ProductDb>().Where(predicate).ToListAsync();

        return result;
    }

    public async Task<bool> WishExistsAsync(Product product)
    {
        await Init();
        var result = await _connection.Table<ProductDb>().Where(it => it.Id == product.Id).ToListAsync();

        return result.Any();
    }

    public async Task<int> AddProductToWishesAsync(Product item)
    {
        await Init();
        try
        {
            var dbItem = new ProductDb()
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            };

            return await _connection.InsertAsync(dbItem);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message, "LocalDataBase");
            return -1;
        }
    }

    public async Task<int> DeleteProductFromWishesAsync(Product item)
    {
        await Init();

        var dbItem = await _connection.Table<ProductDb>().Where(i => i.Id == item.Id).FirstOrDefaultAsync();

        if (dbItem == null) return -1;

        return await _connection.DeleteAsync<ProductDb>(dbItem.Id);
    }

    public async Task<IEnumerable<CartItem>> GetCartAsync()
    {
        await Init();
        return await _connection.Table<CartItem>().ToListAsync();
    }

    public async Task ClearCartAsync()
    {
        await Init();

        await _connection.Table<CartItem>().DeleteAsync(it => it.Quantity > 0);
    }

    public async Task<CartItem> GetCartItemAsync(long id)
    {
        try
        {
            return await _connection.GetAsync<CartItem>(id);
        }
        catch (Exception _)
        {
            return null;
        }
    }

    public async Task<bool> CartItemExistsAsync(Product product)
    {
        await Init();
        var result = await _connection.Table<CartItem>().Where(it => it.Id == product.Id).ToListAsync();

        return result.Any();
    }

    /// <summary>
    /// Method add cart item
    /// </summary>
    /// <param name="product"></param>
    /// <returns>
    /// 0 if adding didn't applied
    /// 1 if operation was success
    /// </returns>
    public async Task<int> AddProductToCartAsync(Product product)
    {
        await Init();
        try
        {
            var dbItem = new CartItem()
            {
                Id = product.Id,
                Price = product.Price,
                Quantity = 1
            };

            return await _connection.InsertAsync(dbItem);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message, "LocalDataBase");
            return -1;
        }
    }

    /// <summary>
    /// Method update cart item quantity
    /// </summary>
    /// <param name="product"></param>
    /// <param name="quantity"></param>
    /// <returns>
    ///-1 if product not in cart
    /// 0 if changes didn't applied
    /// 1 if operation was success
    /// </returns>
    public async Task<int> UpdateCartItemQuantityAsync(Product product, int quantity)
    {
        await Init();

        var dbItem = await _connection.Table<CartItem>().Where(i => i.Id == product.Id).FirstOrDefaultAsync();

        if (dbItem == null) return -1;

        dbItem.Quantity = quantity;

        return await _connection.UpdateAsync(dbItem);
    }

    /// <summary>
    /// Method delete cart item
    /// </summary>
    /// <param name="product"></param>
    /// <returns>
    ///-1 if product not in cart
    /// 0 if deletion didn't applied
    /// 1 if deletion was success
    /// </returns>
    public async Task<int> DeleteProductFromCartAsync(Product product)
    {
        await Init();

        var dbItem = await _connection.Table<CartItem>().Where(i => i.Id == product.Id).FirstOrDefaultAsync();

        if (dbItem == null) return -1;

        return await _connection.DeleteAsync<CartItem>(dbItem.Id);
    }
}