using Domain.Services;
namespace Infrastructure.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly string ALPHABET;
    private readonly int BASE;

    public UrlShortenerService()
    {
        ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        BASE = ALPHABET.Count();
    }

    public string GenerageShortUrl(string url)
    {
        var num = 0;
        for (var i = 0; i < url.Count(); i++)
            num = num * BASE + ALPHABET.IndexOf(url[i]);
        return Math.Abs(num).ToString();
    }
}
