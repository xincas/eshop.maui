using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;
public record LoginDto(
    [property: JsonPropertyName("jwt")] string Jwt,
    [property: JsonPropertyName("user")] UserPlain User
);

public record UserPlain(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("provider")] string Provider,
    [property: JsonPropertyName("confirmed")] bool Confirmed,
    [property: JsonPropertyName("blocked")] bool Blocked,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("updatedAt")] DateTime UpdatedAt,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("name")] string Name
);

public record UserDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("attributes")] UserAttributes Attributes
);

public record UserAttributes(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("provider")] string Provider,
    [property: JsonPropertyName("confirmed")] bool Confirmed,
    [property: JsonPropertyName("blocked")] bool Blocked,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("updatedAt")] DateTime UpdatedAt,
    [property: JsonPropertyName("phone")] string Phone,
    [property: JsonPropertyName("name")] string Name
);