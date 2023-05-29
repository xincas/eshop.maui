using Eshop.Mobile.Pages.Base;
using Eshop.Mobile.ViewModels;

namespace Eshop.Mobile.Pages;

public partial class OrderCreatePageT : ContentPageBase
{
    public OrderCreatePageT(OrderCreateVMt vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}