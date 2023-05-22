using System.Linq.Expressions;
using Eshop.Mobile.Models;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.DataBase;

namespace Eshop.Mobile.Services.Wish;

public class WishService : IWishService
{
    private readonly ICatalogService _catalogService;
    private readonly IDatabaseService _databaseService;

    public WishService(ICatalogService catalogService, IDatabaseService databaseService)
    {
        _catalogService = catalogService;
        _databaseService = databaseService;
    }

    public async Task<IEnumerable<Product>> GetWishesAsync()
    {
        var wishesDb = await _databaseService.GetWishesAsync();

        var productsTasks = new List<Task<Product>>();

        foreach (var productDb in wishesDb)
        {
            productsTasks.Add(_catalogService.GetProductByIdAsync(productDb.IdOfItem));
        }

        var prods = await Task.WhenAll(productsTasks);

        return prods.ToList();
    }

    public async Task AddProductToWishesAsync(Product product)
    {
        await _databaseService.AddProductToWishesAsync(product);
    }

    public async Task DeleteProductToWishesAsync(Product product)
    {
        await _databaseService.DeleteProductFromWishesAsync(product);
    }

    public async Task<IEnumerable<Product>> GetWishesAsync(Func<ProductDb, bool> predicate)
    {
        var wishes = await _databaseService.FindByAsync(x => predicate(x));

        if (wishes == null || !wishes.Any()) return Enumerable.Empty<Product>();

        var productsTask = new List<Task<Product>>();

        foreach (var productDb in wishes)
        {
            productsTask.Add(_catalogService.GetProductByIdAsync(productDb.Id));
        }

        var prods = await Task.WhenAll(productsTask);

        return prods.ToList();
    }
}