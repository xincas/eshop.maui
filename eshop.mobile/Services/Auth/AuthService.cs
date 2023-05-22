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

namespace Eshop.Mobile.Services.Auth;

public class AuthService : IAuthService
{
    private const string _baseUrl = GlobalSettings.ApiUrl;
    private readonly string _loginUrl = $"{_baseUrl}/auth/local";
    private readonly string _refreshUrl = $"{_baseUrl}/token/refresh";

    private readonly IJsonSerializer _serializer = new JsonNetSerializer();
    private readonly IDateTimeProvider _provider = new UtcDateTimeProvider();
    private readonly IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
    private readonly IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

    private readonly IDialogService _dialog;
    private readonly INavigationService _navigation;
    private readonly ISettingsService _settings;

    private readonly HttpClient _httpClient;
    //private readonly IRequestProvider _requestProvider;

    public AuthService(INavigationService navigation, ISettingsService settings, IDialogService dialog)
    {
        _navigation = navigation;
        _settings = settings;
        //_requestProvider = request;
        _dialog = dialog;

        _httpClient = new HttpClient();
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
            //var refreshToken = new RefreshToken(_settings.AuthRefreshToken);
            var refreshToken = new RefreshToken(await _settings.GetRefreshTokenAsync());

            var jsonContent = JsonContent.Create(refreshToken, options: RequestProvider.RequestProvider.JsonOptions);

            var response = await _httpClient.PostAsync(new Uri(_refreshUrl), jsonContent);

            //var response = await _requestProvider.PostAsync<RefreshToken, RefreshTokenDto>(_refreshUrl, refreshToken);

            if (response.StatusCode != HttpStatusCode.OK) return false;

            var data = await response.Content.ReadFromJsonAsync<RefreshTokenDto>();

            _settings.SaveAccessToken(data!.Jwt);
            _settings.SaveRefreshToken(data!.RefreshToken);

            /*_settings.AuthAccessToken = response.Jwt;
            _settings.AuthRefreshToken = response.RefreshToken;*/

            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> LoginAsync(UserCredentials user)
    {
        try
        {
            var jsonContent = JsonContent.Create(user, options: RequestProvider.RequestProvider.JsonOptions);
            var response = await _httpClient.PostAsync(new Uri(_loginUrl), jsonContent);

            if (response.StatusCode != HttpStatusCode.OK) return false;

            //var responseDto = await _requestProvider.PostAsync<UserCredentials, LoginDto>(_loginUrl, user, cookies: cookies);

            //if (responseDto is null) throw new ArgumentNullException(nameof(responseDto));

            string refreshToken = response.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(it => it.StartsWith("refreshToken"))!;
            refreshToken = refreshToken.Split("=")[1].Split(";")[0];

            var data = await response.Content.ReadFromJsonAsync<LoginDto>();

            _settings.SaveAccessToken(data!.Jwt);
            _settings.SaveRefreshToken(refreshToken);

            //_settings.AuthAccessToken = responseDto.Jwt;
            //_settings.AuthRefreshToken = cookies.GetCookies(new Uri(_loginUrl)).FirstOrDefault(it => it.Name == "refreshToken")?.Value!;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            Logout();
            return false;
        }

        return true;
    }

    public async Task NavigateToLoginPageAsync()
    {
        _settings.LogOut();

        await _dialog.ShowAlertAsync("Пожалуйста авторизируйтесь снова.", "Авторизация", "Ок!");

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