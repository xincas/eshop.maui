using System.Reflection;

namespace Eshop.Mobile.ViewModels;

public static class ViewModelsExtensions
{
    private static readonly Assembly ProjectAssembly = typeof(MauiProgram).GetTypeInfo().Assembly;

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        //builder.Services.AddTransient<LoginVM>();
        //builder.Services.AddSingleton<CatalogVM>();
        //builder.Services.AddSingleton<MainVM>();

        var vms = ProjectAssembly.ExportedTypes.Where(
            type => type.Namespace.StartsWith("Eshop.Mobile.ViewModels")
                    && !type.GetTypeInfo().IsAbstract
                    && !type.GetTypeInfo().IsInterface
                    && type.Name.Contains("VM"));

        foreach (var vmType in vms)
        {
            if (vmType.Name.EndsWith("VM"))
                builder.Services.AddSingleton(vmType);
            else
                builder.Services.AddTransient(vmType);
        }

        return builder;
    }
}