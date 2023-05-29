using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class CartPage : ContentPageBase
{
    private CartVM ViewModel => (CartVM)BindingContext;

    public CartPage(CartVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (ViewModel.IsInitialized)
            ViewModel.IsRefreshing = true;
    }
}