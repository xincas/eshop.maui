using Eshop.Mobile.ViewModels.Base;
using UraniumUI.Pages;

namespace Eshop.Mobile.Pages.Base;

public abstract class ContentPageBase : UraniumContentPage
{
    public ContentPageBase()
    {
        NavigationPage.SetBackButtonTitle(this, string.Empty);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not IViewModelBase ivmb)
        {
            return;
        }

        if (ivmb.IsInitialized)
        {
            return;
        }

        await ivmb.InitializeAsyncCommand.ExecuteAsync(null);
    }
}