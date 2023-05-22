using Eshop.Mobile.Models;
using Eshop.Mobile.Models.ApiResponse;
using Eshop.Mobile.Services.RequestProvider;
using Eshop.Mobile.Services.Settings;

namespace Eshop.Mobile.Services.Catalog;

public class CatalogService : ICatalogService
{
    //IRequestProvider _requestProvider;
    ISettingsService _settingsService;
    ICatalogApi _api;

    private readonly string _baseUrl = GlobalSettings.ApiUrl;
    private readonly string _serverUrl = GlobalSettings.ServerUrl;

    public CatalogService( /*IRequestProvider requestProvider,*/ ISettingsService settingsService, ICatalogApi api)
    {
        _settingsService = settingsService;
        //_requestProvider = requestProvider;
        _api = api;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        //var uri = new Uri($"{_baseUrl}/categories?populate=deep,3");
        //var data = await _requestProvider.GetAsync<BaseListResponse<CategoryDto>>(uri.ToString());

        var data = await _api.GetCategoriesAsync();

        if (data is null) return Enumerable.Empty<Category>();

        var result = data.ToDomain(it => it.ToCategory());

        return result;
    }

    public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync()
    {
        //var uri = new Uri($"{_baseUrl}/sub-categories?populate=deep,3");
        //var data = await _requestProvider.GetAsync<BaseListResponse<SubCategoryDto>>(uri.ToString());
        var data = await _api.GetSubCategoriesAsync();

        if (data is null) return Enumerable.Empty<SubCategory>();

        var result = data.ToDomain(it => it.ToSubCategory());

        return result;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        //var uri = new Uri($"{_baseUrl}/products?populate=deep,3");
        //var data = await _requestProvider.GetAsync<BaseListResponse<ProductDto>>(uri.ToString());
        var data = await _api.GetProductsAsync();

        if (data is null) return Enumerable.Empty<Product>();

        var result = data.ToDomain(it => it.ToProduct());

        return result;
    }

    public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(long categoryId)
    {
        //var uri = new Uri($"{_baseUrl}/sub-categories?populate=deep,3&filters[categories][id][$in]={categoryId}");
        //var data = await _requestProvider.GetAsync<BaseListResponse<SubCategoryDto>>(uri.ToString());

        var data = await _api.GetSubCategoriesAsync(categoryId);

        if (data is null) return Enumerable.Empty<SubCategory>();

        var result = data.ToDomain(it => it.ToSubCategory());

        return result;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(long categoryId, long subCategoryId = -1)
    {
        /*var uri = subCategoryId == -1
            ? new Uri(
                $"{_baseUrl}/products?populate=deep,3&filters[categories][id][$in][0]={categoryId}")
            : new Uri(
                $"{_baseUrl}/products?populate=deep,3&filters[categories][id][$in][0]={categoryId}&filters[sub_categories][id][$in][1]={subCategoryId}");

        var data = await _requestProvider.GetAsync<BaseListResponse<ProductDto>>(uri.ToString());*/

        var data = subCategoryId == -1
            ? await _api.GetProductsAsync(categoryId)
            : await _api.GetProductsAsync(categoryId, subCategoryId);

        if (data is null) return Enumerable.Empty<Product>();

        var result = data.ToDomain(it => it.ToProduct());

        return result;
    }

    public async Task<Product> GetProductByIdAsync(long productId)
    {
        //var uri = new Uri($"{_baseUrl}/products?populate=deep,3&filters[id][$in][0]={productId}");
        //var data = await _requestProvider.GetAsync<BaseSingleResponse<ProductDto>>(uri.ToString());

        var data = await _api.GetProductByIdAsync(productId);

        if (data is null) return null;

        var result = data.ToDomain(it => it.ToProduct());

        return result;
    }

    public async Task<Category> GetCategoryByIdAsync(long categoryId)
    {
        //var uri = new Uri($"{_baseUrl}/categories?populate=deep,3&filters[id][$in][0]={categoryId}");
        //var data = await _requestProvider.GetAsync<BaseSingleResponse<CategoryDto>>(uri.ToString());

        var data = await _api.GetCategoryByIdAsync(categoryId);

        if (data is null) return null;

        var result = data.ToDomain(it => it.ToCategory());

        return result;
    }

    public async Task<SubCategory> GetSubCategoryByIdAsync(long subCategoryId)
    {
        //var uri = new Uri($"{_baseUrl}/sub-categories?populate=deep,3&filters[id][$in][0]={subCategoryId}");
        //var data = await _requestProvider.GetAsync<BaseSingleResponse<SubCategoryDto>>(uri.ToString());

        var data = await _api.GetSubCategoryByIdAsync(subCategoryId);

        if (data is null) return null;

        var result = data.ToDomain(it => it.ToSubCategory());

        return result;
    }

    public async Task<IEnumerable<Category>> FindCategoriesByNameAsync(string name)
    {
        //var uri = new Uri($"{_baseUrl}/categories?populate=deep,3&filters[title][$containsi][0]={name}");
        //var data = await _requestProvider.GetAsync<BaseListResponse<CategoryDto>>(uri.ToString());

        var data = await _api.FindCategoriesByNameAsync(name);

        if (data is null) return Enumerable.Empty<Category>();

        var result = data.ToDomain(it => it.ToCategory());

        return result;
    }

    public async Task<IEnumerable<SubCategory>> FindSubCategoriesByNameAsync(string name)
    {
        //var uri = new Uri($"{_baseUrl}/sub-categories?populate=deep,3&filters[title][$containsi][0]={name}");
        //var data = await _requestProvider.GetAsync<BaseListResponse<SubCategoryDto>>(uri.ToString());

        var data = await _api.FindSubCategoriesByNameAsync(name);

        if (data is null) return Enumerable.Empty<SubCategory>();

        var result = data.ToDomain(it => it.ToSubCategory());

        return result;
    }

    public async Task<IEnumerable<Product>> FindProductsByNameAsync(string name)
    {
        //var uri = new Uri($"{_baseUrl}/products?populate=deep,3&filters[name][$containsi][0]={name}");
        //var data = await _requestProvider.GetAsync<BaseListResponse<ProductDto>>(uri.ToString());

        var data = await _api.FindProductsByNameAsync(name);

        if (data is null) return Enumerable.Empty<Product>();

        var result = data.ToDomain(it => it.ToProduct());

        return result;
    }
}