using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record SubCategoryDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("attributes")] SubCategoryAttributes Attributes
);

public record SubCategoryAttributes(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("image")] DataSingleObject<ImageDto> Image,
    [property: JsonPropertyName("categories")] DataList<CategoryDto> Categories,
    [property: JsonPropertyName("products")] DataList<ProductDto> Products,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("updatedAt")] DateTime UpdatedAt,
    [property: JsonPropertyName("publishedAt")] DateTime PublishedAt
);

public static class SubCategoryDtoExtensions
{
    public static SubCategory ToSubCategory(this SubCategoryDto subCategoryDto)
    {
        return new SubCategory(
            subCategoryDto.Id ?? -1,
            subCategoryDto.Attributes?.Title ?? string.Empty,
            subCategoryDto.Attributes?.Image?.Data?.ToUrl() ?? string.Empty,
            subCategoryDto.Attributes?.Products.Data?.Select(it => it.Id ?? -1).ToList() ?? new List<long>(),
            //subCategoryDto.Attributes?.Categories.Data?.Select(it => it.ToCategory()).ToList() ?? new List<Category>()
            subCategoryDto.Attributes?.Categories.Data?.Select(it => it.Id ?? -1).ToList() ?? new List<long>()
        );
    }
}