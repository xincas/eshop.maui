using static Microsoft.Maui.Controls.Application;

namespace Eshop.Mobile.Services.Dialog;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string message, string title, string buttonLabel)
    {
        if (Current is null || Current.MainPage is null) return Task.CompletedTask;

        return Current.MainPage.DisplayAlert(title, message, buttonLabel);
    }
}