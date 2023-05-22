using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record ReviewDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("attributes")] ReviewAttributes Attributes
);

public record ReviewAttributes(
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("score")] int? Score,
    [property: JsonPropertyName("images")] DataList<ImageDto> Images,
    [property: JsonPropertyName("product")] DataSingleObject<ProductDto> Product,
    [property: JsonPropertyName("user")] DataSingleObject<UserDto> User
);

public static class ReviewDtoExtensions
{
    public static Review ToReview(this ReviewDto reviewDto)
    {
        return new Review(
            reviewDto.Id ?? -1,
            reviewDto.Attributes?.User?.Data?.Id ?? -1,
            reviewDto.Attributes?.Product?.Data?.Id ?? -1,
            reviewDto.Attributes?.Content ?? string.Empty,
            reviewDto.Attributes?.Score ?? 0,
            reviewDto.Attributes?.Images?.Data?.Select(it => it.ToUrl()) ?? new List<string>()
        );
    }
}



/*public record UserDto(
    [property: JsonPropertyName("username")] string? Username,
    [property: JsonPropertyName("email")] string? Email
);*/