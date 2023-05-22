using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Services.Auth;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class ProfileVM : ViewModelBase
{
    private readonly IAuthService _authService;

    public ProfileVM(INavigationService navigationService, IAuthService authService) : base(navigationService)
    {
        _authService = authService;
    }

    [RelayCommand]
    async void LogOut() => await _authService.NavigateToLoginPageAsync();
}