namespace Domain.Services;

public interface IUrlShortenerService
{
    string GenerateShortUrl(string url);
}
