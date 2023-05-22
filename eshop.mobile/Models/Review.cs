namespace Eshop.Mobile.Models;

public record Review(
    long Id,
    long UserId,
    long ProductId,
    string Content,
    int Score, //min = 0; max = 10
    IEnumerable<string> ImageUrls);