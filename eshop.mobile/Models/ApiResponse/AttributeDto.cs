using System.Text.Json.Serialization;

namespace Eshop.Mobile.Models.ApiResponse;

public record AttributeListDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("attributes")] IEnumerable<AttributeDto> Attributes
);

public record AttributeDto(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("value")] string Value,
    [property: JsonPropertyName("special_value")] string SpecialValue,
    [property: JsonPropertyName("dimension")] string Dimension,
    [property: JsonPropertyName("type")] string Type
);

public static class AttributeDtoExtension
{
    public static Attribute ToAttribute(this AttributeDto attributeDto)
    {
        var type = attributeDto?.Type switch
        {
            "string" => AttributeType.String,
            "decimal" => AttributeType.Decimal,
            "int" => AttributeType.Int,
            _ => AttributeType.String
        };

        return new Attribute(
            attributeDto?.Id ?? -1,
            attributeDto?.Name ?? string.Empty,
            attributeDto?.Value ?? string.Empty,
            attributeDto?.Dimension ?? string.Empty,
            attributeDto?.SpecialValue ?? string.Empty,
            type
        );
    }

    public static AttributeList ToAttributeList(this AttributeListDto attributeListDto)
    {
        var attributes = attributeListDto?.Attributes?.Select(it => it.ToAttribute()) ?? new List<Attribute>();

        return new AttributeList(
            attributeListDto?.Id ?? -1,
            attributeListDto?.Title ?? string.Empty,
            attributes);
    }
}