namespace Eshop.Mobile.Models;

public partial record Address(
    long? Id,
    string Street,
    string Building,
    string Apartment,
    string Entrance,
    string Floor,
    string Intercom,
    string FullAddress,
    long UserId
)
{
    public Address(string fulladdress) : this(null, "", "", "", "", "", "", fulladdress, -1)
    {
    }

    public static Address Empty => new Address(-1, string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        -1);
};