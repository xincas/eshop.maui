using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SQLite;

namespace Eshop.Mobile.Models;

public record Product(
    long Id,
    string Name,
    string Description,
    bool Available,
    bool IsNew,
    string Slug,
    int Count,
    decimal Price,
    IEnumerable<AttributeList> AttributesList,
    IEnumerable<Review> Reviews,
    IEnumerable<ImageSource> ImagesP) : INotifyPropertyChanged
{
    public static IEnumerable<ImageSource> NoImage = new List<ImageSource>() { ImageSource.FromFile("noimage.png") };

    public IEnumerable<ImageSource> Images => !ImagesP.Any() ? NoImage : ImagesP;

    public ImageSource Image => Images.First();

    private int _quantity = 0;
    private bool _inCart = false;
    private bool _inWish = false;

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged($"{nameof(TotalPrice)}");
            }
        }
    }

    public bool InCart
    {
        get => _inCart;
        set
        {
            if (_inCart != value)
            {
                _inCart = value;
                OnPropertyChanged();
            }
        }
    }

    public bool InWish
    {
        get => _inWish;
        set
        {
            if (_inWish != value)
            {
                _inWish = value;
                OnPropertyChanged();
            }
        }
    }

    public static Product Empty => new Product(-1, "", "", false, false, "", 0, 0, Enumerable.Empty<AttributeList>(),
        Enumerable.Empty<Review>(), Enumerable.Empty<ImageSource>());

    public decimal TotalPrice => Price * Quantity;


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
};

public class ProductDb
{
    [PrimaryKey] public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class CartItem
{
    [PrimaryKey] public long Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    [Ignore] public decimal TotalPrice => Price * Quantity;
}