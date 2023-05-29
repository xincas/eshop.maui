using AndroidX.AppCompat.Widget;

namespace Eshop.Mobile.Platforms.Android.CustomHandlers;

public class CustomEntryHandler : Microsoft.Maui.Handlers.EntryHandler
{
    protected override void ConnectHandler(AppCompatEditText platformView)
    {
        platformView.Background = null;
        base.ConnectHandler(platformView);
    }
}