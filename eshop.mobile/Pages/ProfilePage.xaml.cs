using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class ProfilePage : ContentPageBase
{
    public ProfilePage(ProfileVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        //await ((ProfileVM)BindingContext).InitializeAsync();
    }
}