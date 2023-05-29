using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Settings;

namespace Eshop.Mobile.Services.Navigation;

public class NavigationService : INavigationService
{
    private readonly ISettingsService _settingsService;


    public NavigationService(ISettingsService settings)
    {
        _settingsService = settings;
    }

    public Task InitializeAsync()
    {
        var token = _settingsService.GetAccessToken();

        return NavigateToAsync(string.IsNullOrEmpty(token)
            ? @$"//{nameof(LoginPageT)}"
            : @$"//{nameof(MainPage)}");
    }

    public Task NavigateToAsync(string route, IDictionary<string, object>? routeParams = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return routeParams is not null
            ? Shell.Current.GoToAsync(shellNavigation, false, routeParams)
            : Shell.Current.GoToAsync(shellNavigation, false);
    }


    public Task PopAsync() => Shell.Current.GoToAsync("..");
}