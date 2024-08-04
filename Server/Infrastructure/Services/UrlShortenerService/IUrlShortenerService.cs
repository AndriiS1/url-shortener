namespace Infrastructure.Services.UrlShortenerService;

public interface IUrlShortenerService
{
    string GenerateShortUrl(string url);
}
