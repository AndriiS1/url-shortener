namespace Domain.Models;

public class Url
{
    public long Id { get; set; }
    public required DateTime Date { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortUrl { get; set; }
    public required string Code { get; set; }
    public required long UserId { get; set; }
    public User User { get; set; } = null!;
}
