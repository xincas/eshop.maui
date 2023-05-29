using Eshop.Mobile.Models.ApiRequest;

namespace Eshop.Mobile.Models;

public record Order(
    long Id,
    long UserId,
    OrderStatus Status,
    decimal Total,
    Address Address,
    IEnumerable<Item> Items
)
{
    public CreateOrderDto ToCreateOrderDto()
    {
        var items = Items.Select(it => new CreateItemOrderDto(it.Product.Id, it.Count, it.Count * it.Product.Price));
        return new CreateOrderDto(UserId, "processing", items.Sum(it => it.Total), Address.Id ?? null, items);
    }
};

public record Item(
    int Count,
    Product Product,
    decimal Total
);

public enum OrderStatus
{
    Processing,
    Shipping,
    Delivered,
    Cancelled
}