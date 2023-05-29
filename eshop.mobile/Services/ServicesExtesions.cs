using Eshop.Mobile.Services.Auth;
using Eshop.Mobile.Services.Cart;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.DataBase;
using Eshop.Mobile.Services.Dialog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.Services.Order;
using Eshop.Mobile.Services.RequestProvider;
using Eshop.Mobile.Services.Settings;
using Eshop.Mobile.Services.Wish;
using Refit;

namespace Eshop.Mobile.Services;

public static class ServicesExtesions
{
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IRequestProvider, RequestProvider.RequestProvider>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<ICatalogService, CatalogService>();
        builder.Services.AddSingleton<ICatalogApi>(_ => RestService.For<ICatalogApi>(GlobalSettings.ApiUrl));
        //builder.Services.AddSingleton<IOrderApi>(_ => RestService.For<IOrderApi>(GlobalSettings.ApiUrl, new RefitSettings()
        //{
        //    AuthorizationHeaderValueGetter = () => Task.FromResult()
        //}));
        builder.Services.AddSingleton<IDatabaseService, EShopDataBaseService>();
        builder.Services.AddSingleton<IWishService, WishService>();
        builder.Services.AddSingleton<ICartService, CartService>();
        builder.Services.AddSingleton<IOrderService, OrderService>();


        return builder;
    }
}