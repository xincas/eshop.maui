using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models;

public record UserCredentials(
    [property: JsonPropertyName("identifier")] string Identifier,
    [property: JsonPropertyName("password")] string Password
);