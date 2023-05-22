using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Security.Authentication;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Eshop.Mobile.Services.Auth;

namespace Eshop.Mobile.Services.RequestProvider;

public class RequestProvider : IRequestProvider
{
    private readonly Lazy<HttpClient> _httpClient =
        new(() =>
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = TimeSpan.FromSeconds(15);
            return httpClient;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

    private readonly IAuthService _auth;

    public RequestProvider(IAuthService auth)
    {
        _auth = auth;
    }

    public static JsonSerializerOptions JsonOptions => new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        WriteIndented = true
    };

    public async Task<TResult?> GetAsync<TResult>(string uri, string token = "", string header = "")
        where TResult : class
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (await ValidateToken(token))
                return null;
        }

        var httpClient = GetOrCreateHttpClient(token);
        var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync<TResult>(options: JsonOptions);

        return result;
    }

    public async Task<TResult?> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        where TResult : class
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (await ValidateToken(token))
                return null;
        }

        var httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            AddHeaderParameter(httpClient, header);
        }

        var content = new StringContent(JsonSerializer.Serialize(data));
        var response = await httpClient.PutAsync(uri, content).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);
        var result = await response.Content.ReadFromJsonAsync<TResult>();

        return result;
    }

    public async Task<TResult?> PostAsync<TResult>(string uri, string data, string token = "", string header = "",
        CookieContainer? cookies = null) where TResult : class
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (await ValidateToken(token))
                return null;
        }

        var httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            AddHeaderParameter(httpClient, header);
        }

        var content = new StringContent(data);
        var response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

        await HandleResponse(response);

        if (cookies is not null)
        {
            var url = new Uri(uri);
            foreach (var cookieHeader in response.Headers.GetValues("Set-Cookie"))
                cookies.SetCookies(url, cookieHeader);
        }

        var result = await response.Content.ReadFromJsonAsync<TResult>();

        return result;
    }

    public async Task<TResult?> PostAsync<TData, TResult>(string uri, TData data, string token = "", string header = "",
        CookieContainer? cookies = null) where TResult : class
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (await ValidateToken(token))
                return null;
        }

        var httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            AddHeaderParameter(httpClient, header);
        }

        var jsonContent = JsonContent.Create(data, options: JsonOptions);
        var response = await httpClient.PostAsync(uri, jsonContent).ConfigureAwait(false);
        await HandleResponse(response);

        if (cookies is not null)
        {
            var url = new Uri(uri);
            foreach (var cookieHeader in response.Headers.GetValues("Set-Cookie"))
                cookies.SetCookies(url, cookieHeader);
        }

        var result = await response.Content.ReadFromJsonAsync<TResult>();

        return result;
    }


    public async Task<TResult?> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        where TResult : class
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (await ValidateToken(token))
                return null;
        }

        var httpClient = GetOrCreateHttpClient(token);

        if (!string.IsNullOrEmpty(header))
        {
            AddHeaderParameter(httpClient, header);
        }

        var content = new StringContent(JsonSerializer.Serialize(data));
        var response = await httpClient.PutAsync(uri, content).ConfigureAwait(false);

        await HandleResponse(response).ConfigureAwait(false);
        var result = await response.Content.ReadFromJsonAsync<TResult>();

        return result;
    }

    public async Task DeleteAsync<TResult>(string uri, string token = "", string header = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (await ValidateToken(token))
                return;
        }

        var httpClient = GetOrCreateHttpClient(token);
        await httpClient.DeleteAsync(uri).ConfigureAwait(false);
    }

    private HttpClient GetOrCreateHttpClient(string token = "")
    {
        var httpClient = _httpClient.Value;

        httpClient.DefaultRequestHeaders.Authorization =
            !string.IsNullOrEmpty(token) ? new AuthenticationHeaderValue("Bearer", token) : null;

        return httpClient;
    }

    private static void AddHeaderParameter(HttpClient httpClient, string parameter)
    {
        if (httpClient is null) return;
        if (string.IsNullOrEmpty(parameter)) return;

        httpClient.DefaultRequestHeaders.Add(parameter, new Guid().ToString());
    }

    private async ValueTask<bool> ValidateToken(string token)
    {
        if (_auth.ValidateAccessToken(token)) return true;

        if (await _auth.RefreshAccessTokenAsync()) return true;

        return false;
    }

    private async Task HandleResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized)
            {
                //throw new AuthenticationException(content);
                _auth.NavigateToLoginPageAsync();
            }

            throw new HttpRequestException($"Status code:{response.StatusCode}\nContent:{content}");
        }
    }
}