namespace Eshop.Mobile.Models;

public record Order(
    long UserId,
    OrderStatus Status,
    decimal Total,
    Address Address,
    IEnumerable<Item> Items
    );

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