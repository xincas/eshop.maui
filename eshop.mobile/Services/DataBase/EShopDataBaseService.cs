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

    async Task Init()
    {
        if (_connection is not null)
            return;

        _connection = new SQLiteAsyncConnection(GlobalSettings.DatabasePath, GlobalSettings.Flags);
        await _connection.CreateTableAsync<ProductDb>();
    }

    public async Task<IEnumerable<ProductDb>> GetWishesAsync()
    {
        await Init();
        return await _connection.Table<ProductDb>().ToListAsync();
    }

    public async Task<IEnumerable<ProductDb>> FindByAsync(Expression<Func<ProductDb, bool>> predicate)
    {
        await Init();
        var result = await _connection.Table<ProductDb>().Where(predicate).ToArrayAsync();

        return result;
    }

    public async Task<int> AddProductToWishesAsync(Product item)
    {
        await Init();

        var dbItem = new ProductDb()
        {
            IdOfItem = item.Id,
            Name = item.Name,
            Price = item.Price
        };

        return await _connection.InsertAsync(dbItem);
    }

    public async Task<int> DeleteProductFromWishesAsync(Product item)
    {
        await Init();

        var dbItem = await _connection.Table<ProductDb>().Where(i => i.IdOfItem == item.Id).FirstOrDefaultAsync();

        if (dbItem == null) return -1;

        return await _connection.DeleteAsync<ProductDb>(dbItem);
    }
}