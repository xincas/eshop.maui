using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record OrderDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("attributes")]
    OrderAttributes Attributes
);

public record OrderAttributes(
    [property: JsonPropertyName("user")] DataSingleObject<UserDto> User,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("total")] decimal? Total,
    [property: JsonPropertyName("address")]
    DataSingleObject<AddressDto> Address,
    [property: JsonPropertyName("items")] IEnumerable<ItemDto> Items,
    [property: JsonPropertyName("createdAt")]
    DateTime? CreatedAt,
    [property: JsonPropertyName("updatedAt")]
    DateTime? UpdatedAt,
    [property: JsonPropertyName("publishedAt")]
    DateTime? PublishedAt
);

public record ItemDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("product")]
    DataSingleObject<ProductDto> Product,
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("total")] decimal? Total
);

public static class OrderDtoExtensions
{
    public static Order ToOrder(this OrderDto orderDto)
    {
        var status = orderDto.Attributes?.Status switch
        {
            "processing" => OrderStatus.Processing,
            "shipping" => OrderStatus.Shipping,
            "delivered" => OrderStatus.Delivered,
            "cancelled" => OrderStatus.Cancelled,
            _ => OrderStatus.Processing
        };

        var address = orderDto.Attributes?.Address?.Data?.ToAddress();

        return new Order(
            orderDto.Id ?? -1,
            orderDto.Attributes?.User.Data?.Id ?? -1,
            status,
            orderDto.Attributes?.Total ?? decimal.Zero,
            address ?? Address.Empty,
            orderDto.Attributes?.Items?.Select(it => it.ToItem()) ?? new List<Item>()
        );
    }

    public static Item ToItem(this ItemDto itemDto)
    {
        return new Item(
            itemDto.Count ?? -1,
            itemDto.Product.Data?.ToProduct() ?? Product.Empty,
            itemDto.Total ?? decimal.Zero
        );
    }
}