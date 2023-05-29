using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiRequest;

public record CreateOrderDto(
    [property: JsonPropertyName("user")] long userid,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("total")] decimal? Total,
    [property: JsonPropertyName("address")]
    long? Address,
    [property: JsonPropertyName("items")] IEnumerable<CreateItemOrderDto> Items
);

public record CreateItemOrderDto(
    [property: JsonPropertyName("product")]
    long Product,
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("total")] decimal? Total
);