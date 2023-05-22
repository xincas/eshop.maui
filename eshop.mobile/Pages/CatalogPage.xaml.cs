using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class CatalogPage : ContentPageBase
{
    public CatalogPage(CatalogVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
}