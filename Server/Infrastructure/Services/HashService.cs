using System.Security.Cryptography;
using System.Text;
using Domain.Services;
namespace Infrastructure.Services;

public class HashService : IHashService
{
    public string GetHash(string text)
    {
        using var sha256 = SHA256.Create();

        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}
