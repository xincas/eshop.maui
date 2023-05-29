using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Models.ApiResponse;
using Eshop.Mobile.Services.Auth;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.Services.Order;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class ProfileVM : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly IOrderService _orderService;

    ObservableCollection<Order> _orders;

    public IReadOnlyList<Order> Orders => _orders;
    [ObservableProperty] private User _user;

    public ProfileVM(INavigationService navigationService, IAuthService authService, IOrderService order) : base(
        navigationService)
    {
        _authService = authService;
        _orderService = order;

        _orders = new ObservableCollection<Order>();
    }

    [RelayCommand]
    async void LogOut() => await _authService.NavigateToLoginPageAsync();

    public async override Task InitializeAsync()
    {
        var user = await _authService.GetMeAsync();
        var orders = await _orderService.GetOrdersAsync();

        _orders.Clear();
        foreach (var order in orders) _orders.Add(order);

        User = user;
    }
}