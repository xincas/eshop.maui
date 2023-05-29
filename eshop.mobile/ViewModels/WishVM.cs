using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.Services.Wish;
using Eshop.Mobile.ViewModels.Base;
using Microsoft.Maui.Platform;

namespace Eshop.Mobile.ViewModels;

public partial class WishVM : ViewModelBase
{
    private readonly IWishService _wishService;

    private ObservableCollection<Product> _wishes;
    public IReadOnlyList<Product> Wishes => _wishes;

    [ObservableProperty] private bool _isFiltred;
    [ObservableProperty] private bool _isAvailbleFilter;
    [ObservableProperty] private double _maxPriceFilter;
    [ObservableProperty] private double _maxPossiblePrice;
    [ObservableProperty] private bool _isRefreshing;

    public WishVM(INavigationService navigationService, IWishService wishService) : base(navigationService)
    {
        _wishService = wishService;

        _wishes = new ObservableCollection<Product>();
    }

    [RelayCommand]
    public Task ApplyFilter()
    {
        return IsBusyFor(async () =>
        {
            var wishList = await _wishService.GetWishesAsync();

            _wishes.Clear();

            foreach (var product in wishList
                         .Where(it => IsAvailbleFilter != true || it.Available == true)
                         .Where(it => it.Price <= (decimal)MaxPriceFilter))
            {
                _wishes.Add(product);
            }

            IsFiltred = true;
        });
    }

    [RelayCommand]
    public async void ResetFilter()
    {
        await IsBusyFor(async () =>
        {
            IsAvailbleFilter = false;
            MaxPriceFilter = MaxPossiblePrice;
            await InitializeAsync();
            IsFiltred = false;
        });
    }

    [RelayCommand]
    public async void RefreshWishes()
    {
        await IsBusyFor(() =>
        {
            if (IsFiltred)
                ApplyFilter();
            else
                ResetFilter();

            return Task.CompletedTask;
        });
        IsRefreshing = false;
    }

    [RelayCommand]
    private async void DeleteWish(Product product)
    {
        await _wishService.DeleteProductFromWishesAsync(product);
        _wishes.Remove(product);
    }

    [RelayCommand]
    private async void NavigateToProductDetailPage(Product product)
    {
        await NavigationService.NavigateToAsync(nameof(ProductDetailsPageT), new Dictionary<string, object>()
        {
            { "product", product }
        });
    }

    public override async Task InitializeAsync()
    {
        var wishes = await _wishService.GetWishesAsync();

        _wishes.Clear();
        decimal max = 0;

        foreach (var wish in wishes)
        {
            if (wish.Price > max) max = wish.Price;
            Debug.WriteLine(wish);
            _wishes.Add(wish);
        }

        MaxPossiblePrice = (double)max;
    }
}