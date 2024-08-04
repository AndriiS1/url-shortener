using System.Security.Claims;
using Domain.Dtos;
using Domain.Models;
namespace Infrastructure.Services.JwtService;

public interface IJwtService
{
    string GenerateJsonWebToken(User user);
    RefreshTokenDataDto GenerateRefreshTokenData();
    IEnumerable<Claim>? GetPrincipalFromExpiredToken(string? token);
}
