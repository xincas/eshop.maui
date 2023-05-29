using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class RegistrationPageT : ContentPage
{
    public RegistrationPageT(RegistrationVMt vm)
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
}