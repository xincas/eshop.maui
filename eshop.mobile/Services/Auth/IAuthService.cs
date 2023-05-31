using Eshop.Mobile.Models;
using Eshop.Mobile.Models.ApiResponse;
using Refit;

namespace Eshop.Mobile.Services.Auth;

public interface IAuthService
{
    bool ValidateAccessToken(string accessToken);
    Task<bool> RefreshAccessTokenAsync();

    Task<User> GetMeAsync();
    Task<string> GetAccessTokenAsync();
    Task<bool> LoginAsync(UserCredentials user);
    Task<AuthResponse> RegisterNewAcc(RegisterModel user);
    Task NavigateToLoginPageAsync(bool showPopup = false);
    void Logout();
}

public interface IProfileApi
{
    [Get("/users/me")]
    [Headers("Authorization: Bearer")]
    Task<User> GetProfileAsync();
}

public struct AuthResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}