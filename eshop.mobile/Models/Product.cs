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
    IEnumerable<ImageSource> ImagesP)
{
    public static IEnumerable<ImageSource> NoImage => new List<ImageSource>() { ImageSource.FromFile("noimage.png") };

    public IEnumerable<ImageSource> Images => !ImagesP.Any() ? NoImage : ImagesP;
};

public class ProductDb
{
    [PrimaryKey, AutoIncrement]
    public long Id { get; set; }
    public long IdOfItem { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}