using System.Diagnostics;
using Eshop.Mobile.Models.ApiRequest;
using Eshop.Mobile.Models.ApiResponse;
using Eshop.Mobile.Services.Auth;
using Refit;

namespace Eshop.Mobile.Services.Order;

public class OrderService : IOrderService
{
    private readonly IAuthService _auth;
    private readonly IOrderApi _orderApi;
    private readonly IDaDataApi _daDataApi;

    public OrderService(IAuthService auth)
    {
        _auth = auth;
        _orderApi = RestService.For<IOrderApi>(GlobalSettings.ApiUrl, new RefitSettings()
        {
            AuthorizationHeaderValueGetter = () => _auth.GetAccessTokenAsync()
        });

        _daDataApi = RestService.For<IDaDataApi>("https://suggestions.dadata.ru/suggestions/api/4_1",
            new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(GlobalSettings.DaDataApiKey)
            });
    }

    public async Task<IEnumerable<Models.Order>> GetOrdersAsync()
    {
        try
        {
            var response = await _orderApi.GetOrdersAsync();

            if (response == null) return Enumerable.Empty<Models.Order>();

            var result = response.Data.Select(it => it.ToOrder());

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message, "OrderService");
            return Enumerable.Empty<Models.Order>();
        }
    }

    public async Task<DaDataResponse> GetSuggestionsAsync(string query)
    {
        try
        {
            var requestData = new DaDataRequest() { query = query, count = 5 };
            var response = await _daDataApi.GetSuggestionsAsync(requestData);

            return response;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message, "OrderService");
            return null;
        }
    }

    public async Task<bool> CreateOrder(Models.Order order)
    {
        try
        {
            var requestData = new DataSingleObject<CreateOrderDto>(order.ToCreateOrderDto());
            var response = await _orderApi.CreateOrderAsync(requestData);

            if (response == null) return false;

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message, "OrderService");
            return false;
        }
    }
}