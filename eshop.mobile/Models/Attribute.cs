namespace Eshop.Mobile.Models;

public record Attribute(
    long Id,
    string Name,
    string Value,
    string Dimension,
    string SpecialValue,
    AttributeType Type);

public record AttributeList(
    long Id,
    string Title,
    IEnumerable<Attribute> Attributes);

public enum AttributeType
{
    String = 0,
    Int,
    Decimal
}
