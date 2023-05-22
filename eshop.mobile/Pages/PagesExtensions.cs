using System.Reflection;

namespace Eshop.Mobile.Pages;

public static class PagesExtensions
{
    private static readonly Assembly ProjectAssembly = typeof(MauiProgram).GetTypeInfo().Assembly;

    public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
    {
        /*builder.Services.AddTransient<LoginPage>();
        builder.Services.AddSingleton<CatalogPage>();
        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<WishlistPage>();
        builder.Services.AddSingleton<ProfilePage>();*/

        var pages = ProjectAssembly.ExportedTypes.Where(
            type => type.Namespace.StartsWith("Eshop.Mobile.Pages")
                    && !type.GetTypeInfo().IsAbstract
                    && !type.GetTypeInfo().IsInterface);

        foreach (var pageType in pages)
        {
            if (pageType.Name.EndsWith("Page"))
                builder.Services.AddSingleton(pageType);
            else
                builder.Services.AddTransient(pageType);
        }

        return builder;
    }
}