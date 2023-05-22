using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record AddressDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("attributes")]
    AddressAttributes Attributes
);

public record AddressAttributes(
    [property: JsonPropertyName("street")] string Street,
    [property: JsonPropertyName("building")] string Building,
    [property: JsonPropertyName("apartment")] string Apartment,
    [property: JsonPropertyName("entrance")] string Entrance,
    [property: JsonPropertyName("floor")] string Floor,
    [property: JsonPropertyName("intercom")] string Intercom,
    [property: JsonPropertyName("full_address")] string FullAddress,
    [property: JsonPropertyName("user")] DataSingleObject<UserDto> User,
    [property: JsonPropertyName("createdAt")] DateTime? CreatedAt,
    [property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt,
    [property: JsonPropertyName("publishedAt")] DateTime? PublishedAt
);

public static class AddressDtoExtensions
{
    public static Address ToAddress(this AddressDto addressDto)
    {
        return new Address(
            addressDto.Attributes?.Street ?? string.Empty,
            addressDto.Attributes?.Building ?? string.Empty,
            addressDto.Attributes?.Apartment ?? string.Empty,
            addressDto.Attributes?.Entrance ?? string.Empty,
            addressDto.Attributes?.Floor ?? string.Empty,
            addressDto.Attributes?.Intercom ?? string.Empty,
            addressDto.Attributes?.FullAddress ?? string.Empty,
            addressDto.Attributes?.User.Data?.Id ?? -1
        );
    }
}