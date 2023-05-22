using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.Services.Settings;

namespace Eshop.Mobile;

public partial class App : Application
{
    readonly private INavigationService _navigationService;
    readonly private ISettingsService _settingsService;

    public App(INavigationService navigation, ISettingsService settings)
    {
        _navigationService = navigation;
        _settingsService = settings;

        InitializeComponent();

        MainPage = new AppShell(navigation);
    }
}