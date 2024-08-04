namespace Domain.Dtos;

public class UrlInfoDto
{
    public required long Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortUrl { get; set; }
    public required string UserName { get; set; }
    public required DateTime Date { get; set; }
}
