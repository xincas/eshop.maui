using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
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

    [ObservableProperty] private bool _isAvailbleFilter;
    [ObservableProperty] private double _maxPriceFilter;
    [ObservableProperty] private double _maxPossiblePrice;

    public WishVM(INavigationService navigationService, IWishService wishService) : base(navigationService)
    {
        _wishService = wishService;

        _wishes = new ObservableCollection<Product>();
    }

    [RelayCommand]
    public async void ApplyFilter()
    {
        await IsBusyFor(() =>
        {
            var wishList = _wishes.ToList();

            _wishes.Clear();

            foreach (var product in wishList
                         .Where(it => IsAvailbleFilter != true || it.Available == true)
                         .Where(it => it.Price <= (decimal)MaxPriceFilter))
            {
                _wishes.Add(product);
            }

            return Task.CompletedTask;
        });
    }

    [RelayCommand]
    public async void ResetFilter()
    {
        IsAvailbleFilter = false;
        MaxPriceFilter = MaxPossiblePrice;
        await InitializeAsync();
    }

    public override async Task InitializeAsync()
    {
        var wishes = await _wishService.GetWishesAsync();

        if (!wishes.Any())
        {
            var wish = new List<Product>()
            {
                new Product(17, "123", String.Empty, false, false, String.Empty, 0, 50, new List<AttributeList>(),
                    new List<Review>(), new List<ImageSource>()),
                new Product(5, "123456", String.Empty, false, false, String.Empty, 0, 100, new List<AttributeList>(),
                    new List<Review>(), new List<ImageSource>()),
                new Product(9, "123456789", String.Empty, false, false, String.Empty, 0, 150, new List<AttributeList>(),
                    new List<Review>(), new List<ImageSource>()),
            };

            var tasks = wish.Select(_wishService.AddProductToWishesAsync);

            await Task.WhenAll(tasks);
        }

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