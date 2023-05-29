using CommunityToolkit.Maui.Layouts;
using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class ProductDetailsPageT : ContentPageBase
{
    ProductDetailVMt ViewModel => BindingContext as ProductDetailVMt;

    public ProductDetailsPageT(ProductDetailVMt vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}