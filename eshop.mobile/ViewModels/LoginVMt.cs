using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Auth;
using Eshop.Mobile.Services.Dialog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class LoginVMt : ViewModelBase
{
    readonly private IAuthService _auth;
    readonly private IDialogService _dialog;

    [ObservableProperty] private string _email;
    [ObservableProperty] private string _password;

    public LoginVMt(IDialogService dialog, IAuthService auth, INavigationService navigation) : base(navigation)
    {
        _dialog = dialog;
        _auth = auth;
    }

    [RelayCommand]
    async void Login()
    {
        await IsBusyFor(async () =>
        {
            var isSuccess = await _auth.LoginAsync(new UserCredentials(Email, Password));

            if (!isSuccess)
            {
                await _dialog.ShowAlertAsync("Неверная пара логина и пароля!", "Ошибка", "Ок!");
                return;
            }

            await NavigationService.NavigateToAsync(@$"//{nameof(MainPage)}");
        });
    }

    [RelayCommand]
    async void ToRegistrationPage() => await NavigationService.NavigateToAsync(nameof(RegistrationPageT));

    public override Task InitializeAsync()
    {
        Email = null;
        Password = null;

        return base.InitializeAsync();
    }
}