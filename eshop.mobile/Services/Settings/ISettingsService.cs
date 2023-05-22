namespace Eshop.Mobile.Services.Settings;

public interface ISettingsService
{
    /*string AuthAccessToken { get; set; }
    string AuthRefreshToken { get; set; }*/
    string ClientId { get; set; }
    string Latitude { get; set; }
    string Longitude { get; set; }
    bool AllowGpsLocation { get; set; }

    Task<string> GetAccessTokenAsync();
    string GetAccessToken();
    void SaveAccessToken(string token);
    Task<string> GetRefreshTokenAsync();
    string GetRefreshToken();
    void SaveRefreshToken(string token);
    void LogOut();
}