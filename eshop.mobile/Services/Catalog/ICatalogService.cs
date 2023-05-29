using Eshop.Mobile.Models;
using Eshop.Mobile.Models.ApiResponse;
using Refit;

namespace Eshop.Mobile.Services.Catalog;

public interface ICatalogService
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<IEnumerable<SubCategory>> GetSubCategoriesAsync();
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(long categoryId);
    Task<IEnumerable<Product>> GetProductsAsync(long categoryId, long subCategoryId = -1);
    Task<IEnumerable<Product>> GetProductsOfSubCategoryAsync(long subCategoryId);
    Task<Product> GetProductByIdAsync(long productId);
    Task<Category> GetCategoryByIdAsync(long categoryId);
    Task<SubCategory> GetSubCategoryByIdAsync(long subCategoryId);
    Task<IEnumerable<Category>> FindCategoriesByNameAsync(string name);
    Task<IEnumerable<SubCategory>> FindSubCategoriesByNameAsync(string name);
    Task<IEnumerable<Product>> FindProductsByNameAsync(string name);
    Task<IEnumerable<Product>> FindProductsByNameAsync(string name, long categoryId = -1, long subCategoryId = -1);
}

public interface ICatalogApi
{
    [Get("/categories?populate=deep,3")]
    Task<BaseListResponse<CategoryDto>> GetCategoriesAsync();

    [Get("/sub-categories?populate=deep,3")]
    Task<BaseListResponse<SubCategoryDto>> GetSubCategoriesAsync();

    [Get("/products?populate=deep,3")]
    Task<BaseListResponse<ProductDto>> GetProductsAsync();

    [Get("/sub-categories?populate=deep,3&filters[categories][id][$in]={categoryId}")]
    Task<BaseListResponse<SubCategoryDto>> GetSubCategoriesAsync(long categoryId);

    [Get("/products?populate=deep,3&filters[categories][id][$in][0]={categoryId}")]
    Task<BaseListResponse<ProductDto>> GetProductsOfCategoryAsync(long categoryId);

    [Get(
        "/products?populate=deep,3&filters[categories][id][$in][0]={categoryId}&filters[sub_categories][id][$in][1]={subCategoryId}")]
    Task<BaseListResponse<ProductDto>> GetProductsAsync(long categoryId, long subCategoryId);

    [Get("/products?populate=deep,3&filters[sub_categories][id][$in][1]={subCategoryId}")]
    Task<BaseListResponse<ProductDto>> GetProductsOfSubCategoryAsync(long subCategoryId);

    [Get("/products/{productId}?populate=deep,3")]
    Task<BaseSingleResponse<ProductDto>> GetProductByIdAsync(long productId);

    [Get("/categories/{categoryId}?populate=deep,3")]
    Task<BaseSingleResponse<CategoryDto>> GetCategoryByIdAsync(long categoryId);

    [Get("/sub-categories/{subCategoryId}?populate=deep,3")]
    Task<BaseSingleResponse<SubCategoryDto>> GetSubCategoryByIdAsync(long subCategoryId);

    [Get("/categories?populate=deep,3&filters[title][$containsi][0]={name}")]
    Task<BaseListResponse<CategoryDto>> FindCategoriesByNameAsync(string name);

    [Get("/sub-categories?populate=deep,3&filters[title][$containsi][0]={name}")]
    Task<BaseListResponse<SubCategoryDto>> FindSubCategoriesByNameAsync(string name);

    [Get("/products?populate=deep,3&filters[name][$containsi][0]={name}")]
    Task<BaseListResponse<ProductDto>> FindProductsByNameAsync(string name);

    [Get("/products?populate=deep,3&filters[categories][id]={categoryId}&filters[name][$containsi]={name}")]
    Task<BaseListResponse<ProductDto>> FindCategoryProductsByNameAsync(long categoryId, string name);

    [Get("/products?populate=deep,3&filters[sub_categories][id]={subCategoryId}&filters[name][$containsi]={name}")]
    Task<BaseListResponse<ProductDto>> FindSubCategoryProductsByNameAsync(long subCategoryId, string name);
}