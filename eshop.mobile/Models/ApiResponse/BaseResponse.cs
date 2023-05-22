using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record BaseListResponse<TData>(
    [property: JsonPropertyName("data")] IEnumerable<TData> Data,
    [property: JsonPropertyName("meta")] Meta Meta
);

public record BaseSingleResponse<TData>(
    [property: JsonPropertyName("data")] TData Data,
    [property: JsonPropertyName("meta")] Meta Meta
);

public record DataSingleObject<TData>(
    [property: JsonPropertyName("data")] TData? Data
);

public record DataList<TData>(
    [property: JsonPropertyName("data")] IEnumerable<TData> Data
);

public record Meta(
    [property: JsonPropertyName("pagination")] Pagination Pagination
);

public record Pagination(
    [property: JsonPropertyName("page")] int Page,
    [property: JsonPropertyName("pageSize")] int PageSize,
    [property: JsonPropertyName("pageCount")] int PageCount,
    [property: JsonPropertyName("total")] int Total
);

public static class ResponseExtensions
{
    public static IEnumerable<T>
        ToDomain<TData, T>(this BaseListResponse<TData> response,
            Func<TData, T> action) => //response.Data.AsParallel().AsOrdered().Select(action);
        response.Data.Select(action);
    public static T ToDomain<TData, T>(this BaseSingleResponse<TData> response, Func<TData, T> action) => action(response.Data);
}