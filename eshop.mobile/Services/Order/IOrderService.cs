using Eshop.Mobile.Models.ApiRequest;
using Eshop.Mobile.Models.ApiResponse;
using Refit;

namespace Eshop.Mobile.Services.Order;

public interface IOrderService
{
    Task<IEnumerable<Models.Order>> GetOrdersAsync();
    Task<DaDataResponse> GetSuggestionsAsync(string query);
    Task<bool> CreateOrder(Models.Order order);
}

public interface IOrderApi
{
    [Get("/orders?populate=deep,3")]
    [Headers("Authorization: Bearer")]
    Task<BaseListResponse<OrderDto>> GetOrdersAsync();

    [Post("/orders")]
    [Headers("Authorization: Bearer")]
    Task<BaseSingleResponse<OrderDto>> CreateOrderAsync([Body] DataSingleObject<CreateOrderDto> order);
}

public interface IDaDataApi
{
    [Post("/rs/suggest/address")]
    [Headers("Authorization: Token")]
    Task<DaDataResponse> GetSuggestionsAsync([Body] DaDataRequest order);
}