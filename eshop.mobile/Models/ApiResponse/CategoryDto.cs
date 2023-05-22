using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record CategoryDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("attributes")] CategoryAttributes Attributes
);

public record CategoryAttributes(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("image")] DataSingleObject<ImageDto> Image,
    [property: JsonPropertyName("sub_categories")] DataList<SubCategoryDto> SubCategories,
    [property: JsonPropertyName("products")] DataList<ProductDto> Products,
    [property: JsonPropertyName("createdAt")] DateTime? CreatedAt,
    [property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt,
    [property: JsonPropertyName("publishedAt")] DateTime? PublishedAt
);

public static class CategoryDtoExtensions
{
    public static Category ToCategory(this CategoryDto categoryDto)
    {
        return new Category(
            categoryDto.Id ?? -1,
            categoryDto.Attributes?.Title ?? string.Empty,
            categoryDto.Attributes?.Image?.Data?.ToUrl() ?? string.Empty,
            categoryDto.Attributes?.Products?.Data?.Select(it => it.Id ?? -1).ToList() ?? new List<long>(),
            categoryDto.Attributes?.SubCategories.Data?.Select(it => it.ToSubCategory()).ToList() ?? new List<SubCategory>() 
            //categoryDto.Attributes?.SubCategories?.Data?.Select(it => it.Id ?? -1).ToList() ?? new List<long>()
        );
    }
}