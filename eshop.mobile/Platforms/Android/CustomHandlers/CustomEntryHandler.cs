using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.AppCompat.Widget;
using Color = Android.Graphics.Color;

namespace Eshop.Mobile.Platforms.Android.CustomHandlers;

public class CustomEntryHandler : Microsoft.Maui.Handlers.EntryHandler
{
    protected override void ConnectHandler(AppCompatEditText platformView)
    {
        platformView.Background = new ColorDrawable(new Color(255, 255, 255, 0));
        base.ConnectHandler(platformView);
    }
}