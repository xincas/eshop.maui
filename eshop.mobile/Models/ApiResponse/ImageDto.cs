using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record ImageDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("attributes")] ImageAttributes Attributes
);

public record ImageAttributes(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("alternativeText")] object AlternativeText,
    [property: JsonPropertyName("caption")] object Caption,
    [property: JsonPropertyName("width")] int? Width,
    [property: JsonPropertyName("height")] int? Height,
    [property: JsonPropertyName("formats")] object Formats,
    [property: JsonPropertyName("hash")] string Hash,
    [property: JsonPropertyName("ext")] string Ext,
    [property: JsonPropertyName("mime")] string Mime,
    [property: JsonPropertyName("size")] double? Size,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("previewUrl")] object PreviewUrl,
    [property: JsonPropertyName("provider")] string Provider,
    [property: JsonPropertyName("provider_metadata")] object ProviderMetadata,
    [property: JsonPropertyName("createdAt")] DateTime? CreatedAt,
    [property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt
);

public static class ImageDtoExtensions
{
    public static string ToUrl(this ImageDto imageDto) => imageDto.Attributes?.Url is { } url
        ? $"{GlobalSettings.ServerUrl}{url}"
        : string.Empty;
}