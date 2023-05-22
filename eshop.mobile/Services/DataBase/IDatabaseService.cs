using Eshop.Mobile.Models;
using System.Linq.Expressions;

namespace Eshop.Mobile.Services.DataBase;

public interface IDatabaseService
{
    public Task<IEnumerable<ProductDb>> GetWishesAsync();

    public Task<IEnumerable<ProductDb>> FindByAsync(Expression<Func<ProductDb, bool>>  predicate);
    public Task<int> AddProductToWishesAsync(Product product);
    public Task<int> DeleteProductFromWishesAsync(Product product);
}