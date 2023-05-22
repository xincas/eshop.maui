using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;
using UraniumUI.Pages;

namespace Eshop.Mobile.Pages;

public partial class LoginPageT : ContentPageBase
{
    public LoginPageT(LoginVMt vm)
    {
        InitializeComponent();

        BindingContext = vm;

        //Unloaded += async (sender, args) => await ((LoginVMt)BindingContext).InitializeAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //BindingContext = Handler.MauiContext.Services.GetService(typeof(LoginVMt));

        await ((LoginVMt)BindingContext).InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        //BindingContext = Handler.MauiContext.Services.GetService(typeof(LoginVMt));
    }
    /*protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as LoginVM;

        if (vm is null) return;

        await vm.InitializeAsync();
    }*/
}