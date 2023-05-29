using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Cart;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.Services.Wish;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class CategoryVMt : ViewModelBase
{
    private readonly ICatalogService _catalogService;
    private readonly ICartService _cartService;
    private readonly IWishService _wishService;

    private ObservableCollection<Product> _products;
    public IReadOnlyList<Product> Products => _products;

    [ObservableProperty] private ICategory _category;
    private Type _categoryType;

    [ObservableProperty] private string _prodOrderingState;
    [ObservableProperty] private bool _isOrderingOpen = false;
    [ObservableProperty] private bool _isRefreshing = false;

    public CategoryVMt(ICatalogService catalogService, ICartService cartService, IWishService wishService,
        INavigationService navigationService) : base(navigationService)
    {
        _catalogService = catalogService;
        _cartService = cartService;
        _wishService = wishService;

        _products = new ObservableCollection<Product>();
        ProdOrderingState = ProductOrderingState.Default;
        _category = new Category(-1, "", "", Enumerable.Empty<long>(), Enumerable.Empty<SubCategory>());
        _categoryType = typeof(Category);
    }

    [RelayCommand]
    private async void InitPage()
    {
        if (_categoryType == typeof(Category))
        {
            var products = await _catalogService.GetProductsAsync(Category.Id);

            foreach (var product in products)
            {
                await _cartService.FetchProduct(product);
                await _wishService.IsWishAsync(product);
                _products.Add(product);
                Debug.WriteLine(
                    $"Product id:{product.Id}, inWish:{product.InWish}, inCart:{product.InCart}  is loaded!",
                    "CategoryViewModel");
            }
        }
        else if (_categoryType == typeof(SubCategory))
        {
            var products = await _catalogService.GetProductsOfSubCategoryAsync(Category.Id);

            foreach (var product in products)
            {
                await _cartService.FetchProduct(product);
                await _wishService.IsWishAsync(product);
                _products.Add(product);
                Debug.WriteLine(
                    $"Product id:{product.Id}, inWish:{product.InWish}, inCart:{product.InCart}  is loaded!",
                    "CategoryViewModel");
            }
        }
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (IsInitialized) return;
        if (query == null || !query.Any()) return;

        if (query.ContainsKey("category"))
        {
            Category = (Category)query["category"];
            _categoryType = typeof(Category);
        }
        else
        {
            Category = (SubCategory)query["subcategory"];
            _categoryType = typeof(SubCategory);
        }

        InitPage();

        IsInitialized = true;
    }

    [RelayCommand]
    public void OrderChange(string option)
    {
        CloseOrdering();
        if (ProdOrderingState == option) return;

        ProdOrderingState = option;
        IsRefreshing = true;
    }

    [RelayCommand]
    private void CloseOrdering() => IsOrderingOpen = false;

    [RelayCommand]
    private void OpenOrdering() => IsOrderingOpen = true;

    [RelayCommand]
    private async void ToggleWishStatus(Product product)
    {
        if (product.InWish)
            await _wishService.DeleteProductFromWishesAsync(product);
        else
            await _wishService.AddProductToWishesAsync(product);

        //product.InWish = !product.InWish;
    }


    [RelayCommand]
    private async void AddToCart(Product product)
    {
        await _cartService.AddProductToCartAsync(product);
        product.InCart = true;
    }

    [RelayCommand]
    private void DecreaseQuantity(Product product) => QuantityChange(product, -1);

    [RelayCommand]
    private void IncreaseQuantity(Product product) => QuantityChange(product, 1);

    private async void QuantityChange(Product product, int delta)
    {
        var result = await _cartService.UpdateQuantityOfCartItemAsync(product, delta);

        if (result == 0) product.InCart = false;
    }

    [RelayCommand]
    private async void Refresh()
    {
        _products.Clear();

        IEnumerable<Product> products;

        if (_categoryType == typeof(Category))
            products = await _catalogService.GetProductsAsync(Category.Id);
        else
            products = await _catalogService.GetProductsOfSubCategoryAsync(Category.Id);


        if (ProdOrderingState == ProductOrderingState.LowerPriceFirst)
            products = await Task.Run(() => products.OrderBy(it => it.Price).ToList());
        else if (ProdOrderingState == ProductOrderingState.HighestPriceFirst)
            products = await Task.Run(() => products.OrderByDescending(it => it.Price).ToList());


        foreach (var product in products)
        {
            await _cartService.FetchProduct(product);
            await _wishService.IsWishAsync(product);
            _products.Add(product);
            Debug.WriteLine(
                $"Product id:{product.Id}, inWish:{product.InWish}, inCart:{product.InCart}  is loaded!",
                "CategoryViewModel");
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    private async void NavigateToProductDetailPage(Product product)
    {
        await NavigationService.NavigateToAsync(nameof(ProductDetailsPageT),
            new Dictionary<string, object>()
            {
                { "product", product }
            });
    }
}