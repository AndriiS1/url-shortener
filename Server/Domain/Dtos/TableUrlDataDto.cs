namespace Domain.Dtos;

public class TableUrlDataDto
{
    public required long Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortUrl { get; set; }
    public bool CanDelete { get; set; } = false;
}
