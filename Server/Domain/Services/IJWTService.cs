using Domain.Dto;
using Domain.Models;
using System.Security.Claims;

namespace Domain.Services
{
    public interface IJwtService
    {
        string GenerateJSONWebToken(User user);
        RefreshTokenDataDto GenerateRefreshTokenData();
        IEnumerable<Claim>? GetPrincipalFromExpiredToken(string? token);
    }
}
