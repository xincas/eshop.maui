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
using Eshop.Mobile.Shims;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class ProductDetailVMt : ViewModelBase
{
    private readonly ICatalogService _catalogService;
    private readonly IWishService _wishService;
    private readonly ICartService _cartService;

    [ObservableProperty] private Product _product;

    [ObservableProperty] private string _wishImage;
    [ObservableProperty] private bool _singleImage;

    public ProductDetailVMt(INavigationService navigationService, ICatalogService catalogService,
        IWishService wishService, ICartService cartService) : base(
        navigationService)
    {
        _catalogService = catalogService;
        _wishService = wishService;
        _cartService = cartService;
        _wishImage = ProductDetailImages.HeartR;
        _product = ProductShim.Product;
    }

    public override async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || !query.Any()) return;

        Product = (Product)query["product"];
        SingleImage = Product.Images.Count() == 1;

        await _cartService.FetchProduct(Product);
        await _wishService.IsWishAsync(Product);
        WishImage = Product.InWish ? ProductDetailImages.HeartS : ProductDetailImages.HeartR;

        IsInitialized = true;
    }

    [RelayCommand]
    async void ToggleWishStatus()
    {
        if (Product.InWish)
            await _wishService.DeleteProductFromWishesAsync(Product);
        else
            await _wishService.AddProductToWishesAsync(Product);

        WishImage = Product.InWish ? ProductDetailImages.HeartS : ProductDetailImages.HeartR;
    }

    [RelayCommand]
    private async void AddToCart()
    {
        Debug.WriteLine("AddToCartCommand has been executed!");
        await _cartService.AddProductToCartAsync(Product);
    }

    [RelayCommand]
    private async void ToCart() => await NavigationService.NavigateToAsync($"///{nameof(CartPage)}");

    [RelayCommand]
    private async void QuantityChange(string delta)
    {
        if (!int.TryParse(delta, out var num)) return;
        Debug.WriteLine("QuantityChange has been executed!");

        await _cartService.UpdateQuantityOfCartItemAsync(Product, num);
    }
}

public static class ProductDetailStates
{
    public const string Loading = nameof(Loading);
    public const string Success = nameof(Success);
    public const string Error = nameof(Error);
    public const string AlreadyInCart = nameof(AlreadyInCart);
    public const string NotInCart = nameof(NotInCart);
}

public static class ProductDetailImages
{
    public const string HeartS = nameof(HeartS);
    public const string HeartR = nameof(HeartR);
}