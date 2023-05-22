using Eshop.Mobile.Models;

namespace Eshop.Mobile.Views.DataTemplateSelectors;

public class ProductsDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate MultipleImages { get; set; }
    public DataTemplate SingleImage { get; set; }



    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return ((Product)item).Images.Count() == 1 ? SingleImage : MultipleImages;
    }
}