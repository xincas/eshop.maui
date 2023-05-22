using CommunityToolkit.Mvvm.ComponentModel;

namespace Eshop.Mobile.Models;

[ObservableObject]
public partial record Address(
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
    public Address() : this(string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        -1)
    {
    }
};