using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Navigation;

namespace Eshop.Mobile;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigation)
    {
        _navigationService = navigation;

        InitializeComponent();
        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(LoginPageT), typeof(LoginPageT));
        Routing.RegisterRoute(nameof(RegistrationPageT), typeof(RegistrationPageT));
        Routing.RegisterRoute(nameof(ProductDetailsPageT), typeof(ProductDetailsPageT));
        /*Routing.RegisterRoute(nameof(CatalogPage), typeof(CatalogPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        Routing.RegisterRoute(nameof(WishlistPage), typeof(WishlistPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));*/
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is not null)
        {
            await _navigationService.InitializeAsync();
            //_navigationService.Initialize();
        }
    }
}