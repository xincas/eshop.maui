using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class MainPage : ContentPageBase
{
    public MainPage(MainVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}

