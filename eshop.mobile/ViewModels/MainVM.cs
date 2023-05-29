using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.ViewModels.Base;
using Sentry;

namespace Eshop.Mobile.ViewModels;

public partial class MainVM : ViewModelBase
{
    private ICatalogService _catalogService;

    private ObservableCollection<Product> _products;

    public IReadOnlyList<Product> Products => _products;

    [ObservableProperty] private bool _isRefreshing;

    public MainVM(ICatalogService catalog, INavigationService navigationService) : base(navigationService)
    {
        _catalogService = catalog;

        _products = new ObservableCollection<Product>();
    }


    [RelayCommand]
    public async void RefreshProducts()
    {
        /*await IsBusyFor(() =>
        {
            _products.Clear();

            foreach (var product in Shims.ProductShim.Products)
            {
                _products.Add(product);
            }

            return Task.CompletedTask;
        });*/

        await IsBusyFor(InitializeAsync);

        IsRefreshing = false;
    }

    [RelayCommand]
    public async void NavigateToProductDetailPage(Product product)
    {
        await NavigationService.NavigateToAsync(nameof(ProductDetailsPageT),
            new Dictionary<string, object>()
            {
                { "product", product }
            });
    }

    public override async Task InitializeAsync()
    {
        var products = await _catalogService.GetProductsAsync();

        _products.Clear();

        foreach (var product in products)
        {
            Debug.WriteLine($"Product id:{product.Id} is loaded", "MainViewModel");
            _products.Add(product);
        }
    }
}