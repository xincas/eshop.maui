using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class WishlistPage : ContentPageBase
{
    private WishVM ViewModel => (WishVM)BindingContext;

    public WishlistPage(WishVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Filter.IsPresented = false;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (ViewModel.IsInitialized)
            ViewModel.IsRefreshing = true;
    }
}