namespace Eshop.Mobile.Services.Dialog;

public interface IDialogService
{
    Task ShowAlertAsync(string message, string title, string buttonLabel);
    Task<bool> ShowAlertAsync(string message, string title, string acceptButton, string cancelButton);
}