using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.Services.Navigation;

public interface INavigationService
{
    Task InitializeAsync();
    Task NavigateToAsync(string route, IDictionary<string, object>? routeParams = null);
    Task PopAsync();
}