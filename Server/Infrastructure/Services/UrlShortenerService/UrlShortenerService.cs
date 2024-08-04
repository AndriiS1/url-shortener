namespace Infrastructure.Services.UrlShortenerService;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly string _alphabet;
    private readonly int _base;

    public UrlShortenerService()
    {
        _alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        _base = _alphabet.Length;
    }

    public string GenerateShortUrl(string url)
    {
        var num = url.Aggregate(0, (current, c) => current * _base + _alphabet.IndexOf(url[c]));
        return Math.Abs(num).ToString();
    }
}
