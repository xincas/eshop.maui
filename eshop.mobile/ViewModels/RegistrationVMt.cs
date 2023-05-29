using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Auth;
using Eshop.Mobile.Services.Dialog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class RegistrationVMt : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;

    [ObservableProperty] private string _name;
    [ObservableProperty] private string _email;
    [ObservableProperty] private string _phone;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _oneMorePassword;

    [ObservableProperty] private bool _isReady = true;

    public RegistrationVMt(IDialogService dialogService, IAuthService authService, INavigationService navigationService)
        : base(navigationService)
    {
        _authService = authService;
        _dialogService = dialogService;
    }

    private async Task<bool> Verify()
    {
        if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone) ||
            string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(OneMorePassword))
        {
            await _dialogService.ShowAlertAsync("Неверные данные для регистрации", "Ошибка регистрации", ":(");
            return false;
        }

        if (OneMorePassword != Password)
        {
            await _dialogService.ShowAlertAsync("Пароли не совпадают", "Ошибка регистрации", ":(");
            return false;
        }

        return true;
    }

    [RelayCommand]
    private async void Register()
    {
        IsReady = false;
        await IsBusyFor(async () =>
        {
            if (!await Verify()) return;

            var user = new RegisterModel()
            {
                email = Email,
                name = Name,
                phone = Phone,
                password = Password,
                username = Phone
            };

            var response = await _authService.RegisterNewAcc(user);

            if (!response.IsSuccess)
            {
                await _dialogService.ShowAlertAsync("Пользователь с таким телефоном уже зарегистрирован",
                    "Ошибка регистрации", ":(");
                Debug.WriteLine(response.Message);
                return;
            }

            await NavigationService.NavigateToAsync($"///{nameof(LoginPageT)}");
        });
        IsReady = true;
    }
}