using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class ProductDetailsPageT : ContentPageBase
{
    public ProductDetailsPageT(ProductDetailVMt vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}