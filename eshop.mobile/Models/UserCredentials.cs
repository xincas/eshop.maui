using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models;

public record UserCredentials(
    [property: JsonPropertyName("identifier")]
    string Identifier,
    [property: JsonPropertyName("password")]
    string Password
);

public class RegisterModel
{
    public string username { get; set; }
    public string phone { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public string name { get; set; }
}