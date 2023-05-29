using System.Diagnostics;
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
        Routing.RegisterRoute(nameof(OrderCreatePageT), typeof(OrderCreatePageT));
        Routing.RegisterRoute(nameof(CategoryPageT), typeof(CategoryPageT));
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

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        var newTarget = new ShellNavigationState(args.Target.Location.OriginalString.Replace("/", string.Empty));
        var newArgs = new ShellNavigatingEventArgs(args.Current, newTarget, args.Source, args.CanCancel);
        base.OnNavigating(newArgs);
        /*if (args.Target.Location.OriginalString == $"//{nameof(MainPage)}" ||
            args.Target.Location.OriginalString == $"//{nameof(CatalogPage)}" ||
            args.Target.Location.OriginalString == $"//{nameof(CartPage)}" ||
            args.Target.Location.OriginalString == $"//{nameof(WishlistPage)}" ||
            args.Target.Location.OriginalString == $"//{nameof(ProfilePage)}")
            Debug.WriteLine(Current.Navigation.NavigationStack);
        Debug.WriteLine(Current.Navigation.NavigationStack);*/
    }
}