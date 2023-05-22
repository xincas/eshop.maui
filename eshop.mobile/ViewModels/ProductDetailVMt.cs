using CommunityToolkit.Mvvm.ComponentModel;
using Eshop.Mobile.Models;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class ProductDetailVMt : ViewModelBase
{
    private readonly ICatalogService _catalogService;
    //TODO add cart service

    [ObservableProperty] private Product _product;

    public ProductDetailVMt(INavigationService navigationService, ICatalogService catalogService) : base(
        navigationService)
    {
        _catalogService = catalogService;
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || !query.Any()) return;

        Product = (Product)query["product"];
    }
}