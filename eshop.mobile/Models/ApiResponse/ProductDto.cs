using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record ProductDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("attributes")]
    ProductAttributes Attributes
);

public record ProductAttributes(
    [property: JsonPropertyName("available")]
    bool? Available,
    [property: JsonPropertyName("is_new")] bool? IsNew,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("price")] decimal? Price,
    [property: JsonPropertyName("images")] DataList<ImageDto> Images,
    [property: JsonPropertyName("categories")]
    DataList<CategoryDto> Categories,
    [property: JsonPropertyName("sub_categories")]
    DataList<SubCategoryDto> SubCategories,
    [property: JsonPropertyName("reviews")]
    DataList<ReviewDto> Reviews,
    [property: JsonPropertyName("attribute_lists")]
    IEnumerable<AttributeListDto> AttributesList,
    [property: JsonPropertyName("createdAt")]
    DateTime? CreatedAt,
    [property: JsonPropertyName("updatedAt")]
    DateTime? UpdatedAt,
    [property: JsonPropertyName("publishedAt")]
    DateTime? PublishedAt
);

public static class ProductDtoExtensions
{
    public static Product ToProduct(this ProductDto productDto)
    {
        var reviews = productDto.Attributes?.Reviews?.Data?.Select(it => it.ToReview()) ?? new List<Review>();

        var images = productDto.Attributes?.Images?.Data?.Select(it => new UriImageSource()
                         { Uri = new Uri(it.ToUrl()), CacheValidity = TimeSpan.FromDays(10) } as ImageSource) ??
                     new List<ImageSource>();

        var attributes = productDto.Attributes?.AttributesList?.Select(it => it.ToAttributeList()) ??
                         new List<AttributeList>();

        return new Product(
            productDto.Id ?? -1,
            productDto.Attributes?.Name.Replace("\n", string.Empty) ?? string.Empty,
            //productDto.Attributes?.Description.Replace("\n", string.Empty) ?? string.Empty,
            productDto.Attributes?.Description ?? string.Empty,
            productDto.Attributes?.Available ?? false,
            productDto.Attributes?.IsNew ?? false,
            productDto.Attributes?.Slug ?? string.Empty,
            productDto.Attributes?.Count ?? 0,
            productDto.Attributes?.Price ?? decimal.Zero,
            attributes,
            reviews,
            images
        );
    }
}