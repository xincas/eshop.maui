using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Eshop.Mobile.Messeges;
using Eshop.Mobile.Models;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Auth;
using Eshop.Mobile.Services.Cart;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class CartVM : ViewModelBase, IRecipient<ChangeQuantityCartItemMessage>,
    IRecipient<AddProductToCartMessage>, IRecipient<DeleteCartItemMessage>
{
    private readonly ICartService _cartService;
    private readonly IAuthService _authService;
    private readonly ICatalogService _catalogService;

    ObservableCollection<Product> _products;

    [ObservableProperty] private decimal _totalCartPrice;

    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private bool _isEmpty;

    public IReadOnlyList<Product> Products => _products;

    public CartVM(INavigationService navigationService, ICartService cartService, IAuthService authService,
        ICatalogService catalogService) : base(navigationService)
    {
        _cartService = cartService;
        _authService = authService;
        _catalogService = catalogService;

        _products = new ObservableCollection<Product>();
        WeakReferenceMessenger.Default.Register<AddProductToCartMessage>(this);
        WeakReferenceMessenger.Default.Register<ChangeQuantityCartItemMessage>(this);
        WeakReferenceMessenger.Default.Register<DeleteCartItemMessage>(this);
    }

    [RelayCommand]
    private void DecreaseQuantity(Product product) => QuantityChange(product, -1);

    [RelayCommand]
    private void IncreaseQuantity(Product product) => QuantityChange(product, 1);

    private async void QuantityChange(Product product, int delta)
    {
        var result = await _cartService.UpdateQuantityOfCartItemAsync(product, delta);

        if (result == 0) _products.Remove(product);

        TotalCartPrice = _products.Sum(it => it.TotalPrice);
    }

    [RelayCommand]
    private async void DeleteCartItem(Product product)
    {
        var result = await _cartService.DeleteProductToCartAsync(product);

        if (result == 1)
        {
            _products.Remove(product);
            if (_products.Count == 0)
            {
                IsEmpty = true;
                return;
            }

            TotalCartPrice = _products.Sum(it => it.TotalPrice);
        }
    }

    [RelayCommand]
    public async void RefreshCart()
    {
        await IsBusyFor(InitializeAsync);
        IsRefreshing = false;
    }

    [RelayCommand]
    public async void NavigateToProductDetail(Product product)
    {
        await NavigationService.NavigateToAsync(nameof(ProductDetailsPageT), new Dictionary<string, object>()
        {
            { "product", product }
        });
    }

    [RelayCommand]
    public async void NavigateToOrderCreate()
    {
        await NavigationService.NavigateToAsync(nameof(OrderCreatePageT));
    }

    public async override Task InitializeAsync()
    {
        _products.Clear();

        var cartItems = await _cartService.GetAllAsync();

        if (!cartItems.Any())
        {
            IsEmpty = true;
            return;
        }

        IsEmpty = false;
        var tasks = new List<Task<Product>>();
        foreach (var cartItem in cartItems) tasks.Add(_catalogService.GetProductByIdAsync(cartItem.Id));

        decimal sum = 0;
        var products = await Task.WhenAll(tasks);
        foreach (var (cartItem, product) in Enumerable.Zip(cartItems, products))
        {
            product.Quantity = cartItem.Quantity;
            product.InCart = true;
            sum += product.TotalPrice;
            _products.Add(product);
        }

        TotalCartPrice = sum;
    }

    public async void Receive(ChangeQuantityCartItemMessage message)
    {
        var item = await _cartService.GetByIdAsync(message.Value);

        var product = _products.FirstOrDefault(it => it.Id == item.Id);

        if (product != null) await _cartService.FetchProduct(product);
    }

    public async void Receive(AddProductToCartMessage message)
    {
        var item = await _cartService.GetByIdAsync(message.Value);

        //Todo some checks
        _products.Add(await _catalogService.GetProductByIdAsync(item.Id));
    }

    public void Receive(DeleteCartItemMessage message)
    {
        //Todo some checks
        _products.Remove(_products.FirstOrDefault(it => it.Id == message.Value));
    }
}