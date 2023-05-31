using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Eshop.Mobile.Models;
using Eshop.Mobile.Models.ApiResponse;
using Eshop.Mobile.Pages;
using Eshop.Mobile.Services.Dialog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.Services.RequestProvider;
using Eshop.Mobile.Services.Settings;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Refit;
using Sentry.Protocol;

namespace Eshop.Mobile.Services.Auth;

public class AuthService : IAuthService
{
    private const string _baseUrl = GlobalSettings.ApiUrl;
    private readonly string _loginUrl = $"{_baseUrl}/auth/local";
    private readonly string _refreshUrl = $"{_baseUrl}/token/refresh";
    private readonly string _registerUrl = $"{_baseUrl}/auth/local/register";

    private readonly IJsonSerializer _serializer = new JsonNetSerializer();
    private readonly IDateTimeProvider _provider = new UtcDateTimeProvider();
    private readonly IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
    private readonly IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

    private readonly IDialogService _dialog;
    private readonly INavigationService _navigation;
    private readonly ISettingsService _settings;
    private readonly IProfileApi _profileApi;

    private readonly HttpClient _httpClient;

    private HttpClientHandler httpClientHandler = new HttpClientHandler();

    public AuthService(INavigationService navigation, ISettingsService settings, IDialogService dialog)
    {
        _navigation = navigation;
        _settings = settings;
        _dialog = dialog;
        httpClientHandler.CookieContainer = new();
        _httpClient = new HttpClient(httpClientHandler);
        _profileApi = RestService.For<IProfileApi>(GlobalSettings.ApiUrl,
            new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => GetAccessTokenAsync()
            });
    }

    public bool ValidateAccessToken(string accessToken)
    {
        try
        {
            IJwtValidator validator = new JwtValidator(_serializer, _provider);
            IJwtDecoder decoder = new JwtDecoder(_serializer, validator, _urlEncoder, _algorithm);
            var token = decoder.DecodeToObject<JwtToken>(accessToken);
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.Exp);
            return DateTimeOffset.UtcNow < dateTimeOffset;
        }
        catch (TokenExpiredException)
        {
            return false;
        }
        catch (SignatureVerificationException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RefreshAccessTokenAsync()
    {
        try
        {
            httpClientHandler.CookieContainer = new();
            var refreshToken = new RefreshToken(await _settings.GetRefreshTokenAsync());

            var jsonContent = JsonContent.Create(refreshToken, options: RequestProvider.RequestProvider.JsonOptions);

            httpClientHandler.CookieContainer.Add(new Uri(_baseUrl),
                new Cookie("refreshToken", $"{refreshToken.Refresh}"));

            var response = await _httpClient.PostAsync(new Uri(_refreshUrl), jsonContent);

            if (response.StatusCode != HttpStatusCode.OK) return false;

            var data = await response.Content.ReadFromJsonAsync<RefreshTokenDto>();

            _settings.SaveAccessToken(data!.Jwt);
            _settings.SaveRefreshToken(data!.RefreshToken);

            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return false;
        }
    }

    public Task<User> GetMeAsync()
    {
        try
        {
            return _profileApi.GetProfileAsync();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message, "AuthService");
            return null;
        }
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var token = _settings.GetAccessToken();

        if (!ValidateAccessToken(token))
        {
            await RefreshAccessTokenAsync();
            return _settings.GetAccessToken();
        }

        return token;
    }

    public async Task<bool> LoginAsync(UserCredentials user)
    {
        try
        {
            var jsonContent = JsonContent.Create(user, options: RequestProvider.RequestProvider.JsonOptions);
            var response = await _httpClient.PostAsync(new Uri(_loginUrl), jsonContent);

            if (response.StatusCode != HttpStatusCode.OK) return false;

            string refreshToken = response.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(it => it.StartsWith("refreshToken"))!;
            refreshToken = refreshToken.Split("=")[1].Split(";")[0];

            var data = await response.Content.ReadFromJsonAsync<LoginDto>();

            _settings.ClientId = data!.User.Id;
            _settings.SaveAccessToken(data!.Jwt);
            _settings.SaveRefreshToken(refreshToken);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            Logout();
            return false;
        }

        return true;
    }

    public async Task<AuthResponse> RegisterNewAcc(RegisterModel user)
    {
        try
        {
            var jsonContent = JsonContent.Create(user, options: RequestProvider.RequestProvider.JsonOptions);
            var response = await _httpClient.PostAsync(new Uri(_registerUrl), jsonContent);

            if (response.StatusCode != HttpStatusCode.OK)
                return new AuthResponse() { IsSuccess = false, Message = response.Content.ToString() };

            return new AuthResponse() { IsSuccess = true, Message = response.Content.ToString() };
        }
        catch (Exception e)
        {
            Debug.WriteLine(e, "AuthService");
            return new AuthResponse() { IsSuccess = false, Message = e.ToString() };
        }
    }

    public async Task NavigateToLoginPageAsync(bool showPopup)
    {
        _settings.LogOut();

        if (showPopup) await _dialog.ShowAlertAsync("Пожалуйста авторизируйтесь снова.", "Авторизация", "Ок!");

        await _navigation.NavigateToAsync($"//{nameof(LoginPageT)}");
    }

    public void Logout() => _settings.LogOut();

    private record JwtToken(
        long Exp,
        long Iat,
        long Id
    );

    private record RefreshToken(
        [property: JsonPropertyName("refreshToken")]
        string Refresh
    );

    private record RefreshTokenDto(
        [property: JsonPropertyName("jwt")] string Jwt,
        [property: JsonPropertyName("refreshToken")]
        string RefreshToken
    );
}