using System.Globalization;

namespace Eshop.Mobile.Services.Settings;

public class SettingsService : ISettingsService
{
    #region Setting Constants

    private const string AccessToken = "access_token";
    private const string RefreshToken = "refresh_token";
    private const string IdClient = "id_client";

    private const string IdLatitude = "latitude";
    private const string IdLongitude = "longitude";
    private const string IdAllowGpsLocation = "allow_gps_location";

    private readonly string AccessTokenDefault = string.Empty;
    private readonly string RefreshTokenDefault = string.Empty;
    private readonly string IdClientDefault = string.Empty;
    private const double FakeLatitudeDefault = 59.986751d;
    private const double FakeLongitudeDefault = 30.300527d;
    private const bool AllowGpsLocationDefault = false;

    #endregion

    //public string AuthAccessToken
    //{
    //    get => Preferences.Get(AccessToken, AccessTokenDefault); 
    //    set => Preferences.Set(AccessToken, value);
    //}

    //public string AuthRefreshToken
    //{
    //    get => Preferences.Get(RefreshToken, RefreshTokenDefault); 
    //    set => Preferences.Set(RefreshToken, value);
    //}
    public string ClientId
    {
        get => Preferences.Get(IdClient, IdClientDefault);
        set => Preferences.Set(IdClient, value);
    }

    public string Latitude
    {
        get => Preferences.Get(IdLatitude, FakeLatitudeDefault.ToString(CultureInfo.InvariantCulture));
        set => Preferences.Set(IdLatitude, value);
    }

    public string Longitude
    {
        get => Preferences.Get(IdLongitude, FakeLongitudeDefault.ToString(CultureInfo.InvariantCulture));
        set => Preferences.Set(IdLongitude, value);
    }

    public bool AllowGpsLocation
    {
        get => Preferences.Get(IdAllowGpsLocation, AllowGpsLocationDefault);
        set => Preferences.Set(IdAllowGpsLocation, value);
    }

    public async Task<string> GetAccessTokenAsync()
    {
        return await SecureStorage.GetAsync(AccessToken);
    }

    public string GetAccessToken()
    {
        return SecureStorage.GetAsync(AccessToken).Result;
    }

    public async void SaveAccessToken(string token)
    {
        await SecureStorage.SetAsync(AccessToken, token);
    }

    public async Task<string> GetRefreshTokenAsync()
    {
        return await SecureStorage.GetAsync(RefreshToken);
    }

    public string GetRefreshToken()
    {
        return SecureStorage.GetAsync(RefreshToken).Result;
    }

    public async void SaveRefreshToken(string token)
    {
        await SecureStorage.SetAsync(RefreshToken, token);
    }

    public void LogOut()
    {
        SaveAccessToken(string.Empty);
        SaveRefreshToken(string.Empty);
    }
}