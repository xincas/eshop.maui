using Eshop.Mobile.Models;

namespace Eshop.Mobile.Services.Auth;

public interface IAuthService
{
    bool ValidateAccessToken(string accessToken);
    Task<bool> RefreshAccessTokenAsync();
    Task<bool> LoginAsync(UserCredentials user);
    Task NavigateToLoginPageAsync();
    void Logout();
}