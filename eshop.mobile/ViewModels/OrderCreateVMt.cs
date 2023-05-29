using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Models.ApiRequest;
using Eshop.Mobile.Services.Cart;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.Dialog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.Services.Order;
using Eshop.Mobile.Services.Settings;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class OrderCreateVMt : ViewModelBase
{
    private readonly ICartService _cart;
    private readonly ICatalogService _catalog;
    private readonly ISettingsService _settings;
    private readonly IOrderService _orderService;
    private readonly IDialogService _dialog;

    private ObservableCollection<Product> _products;
    private ObservableCollection<string> _addresses;

    public IReadOnlyList<Product> Products => _products;
    public IReadOnlyList<string> SearchResults => _addresses;
    [ObservableProperty] private decimal _total;
    [ObservableProperty] private string _address;
    private bool _serching = false;
    private bool _addressSelecting = false;

    public OrderCreateVMt(INavigationService navigationService, ISettingsService settings, ICartService cart,
        ICatalogService catalog, IOrderService orderService, IDialogService dialog) : base(navigationService)
    {
        _cart = cart;
        _catalog = catalog;
        _settings = settings;
        _orderService = orderService;
        _dialog = dialog;

        _products = new ObservableCollection<Product>();
        _addresses = new ObservableCollection<string>();
    }

    [RelayCommand]
    private async void AddressSuggest(string query)
    {
        if (string.IsNullOrEmpty(query) || _addressSelecting) _addresses.Clear();
        if (_serching || query.Length < 5 || _addressSelecting) return;
        _serching = true;

        _addresses.Clear();
        var response = await _orderService.GetSuggestionsAsync(query);

        foreach (var suggestion in response.suggestions)
        {
            Debug.WriteLine(suggestion.value);
            _addresses.Add(suggestion.value);
        }

        _serching = false;
    }

    [RelayCommand]
    private void AddressSelect(string address)
    {
        _addressSelecting = true;
        Address = address;
        _addressSelecting = false;
    }

    [RelayCommand]
    private async void CreateOrder()
    {
        await IsBusyFor(async () =>
        {
            var orderItems = _products.Select(it => new Item(it.Quantity, it, it.TotalPrice));

            //TODO Address checking and adding to account
            var address = new Address(Address);

            var order = new Order(-1, _settings.ClientId, OrderStatus.Processing, orderItems.Sum(it => it.Total),
                address,
                orderItems);

            if (!await _orderService.CreateOrder(order))
            {
                await _dialog.ShowAlertAsync("Что-то пошло не так, заказ не оформлен!", "Ошибка", "Закрыть");
                return;
            }

            await _cart.ClearCartAsync();
            await _dialog.ShowAlertAsync("Ваш заказ успешно оформлен!!", "Заказ оформлен", "Закрыть");
            await NavigationService.NavigateToAsync("..");
        });
    }

    public async override Task InitializeAsync()
    {
        var items = await _cart.GetAllAsync();

        var tasks = items.Select(it => _catalog.GetProductByIdAsync(it.Id));

        var prods = await Task.WhenAll(tasks);


        decimal sum = 0;
        foreach (var (cartItem, product) in Enumerable.Zip(items, prods))
        {
            product.Quantity = cartItem.Quantity;
            product.InCart = true;
            sum += product.TotalPrice;
            _products.Add(product);
        }

        Total = sum;
    }
}