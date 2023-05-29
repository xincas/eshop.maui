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
    }

    private void RotateBackground(CancellationToken token)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                await back.RotateTo(360, 100000, Easing.Linear);
                back.Rotation = 0;
            }
        }, token);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((LoginVMt)BindingContext).InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}