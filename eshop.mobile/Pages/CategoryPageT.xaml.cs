using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class CategoryPageT : ContentPageBase
{
    private CategoryVMt ViewModel => (CategoryVMt)BindingContext;

    public CategoryPageT(CategoryVMt vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (ViewModel.IsInitialized)
        {
        }
    }
}