using CommunityToolkit.Maui;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services;
using Eshop.Mobile.ViewModels;
using InputKit.Handlers;
using Microsoft.Extensions.Logging;
using UraniumUI;

namespace Eshop.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font-Awesome-6-Brands-Regular.otf", "FABrands");
                //fonts.AddFont("Font-Awesome-6-Regular.otf", "FARegular");
                //fonts.AddFont("Font-Awesome-6-Solid.otf", "FASolid");
                fonts.AddFont("Racama.otf", "Racama");
                fonts.AddFontAwesomeIconFonts();
                fonts.AddMaterialIconFonts();
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddInputKitHandlers();
            })
            .RegisterPages()
            .RegisterViewModels()
            .RegisterServices();

#if DEBUG
        builder.Services.AddLogging(configure =>
        {
            configure.AddDebug();
                //.AddFilter("com.companyname.eshop.mobile", LogLevel.Trace)
                //.AddFilter("Microsoft", LogLevel.Warning);
        });
#endif

        return builder.Build();
	}
}
