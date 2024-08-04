using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructure.Services;

public class JwtService(IConfiguration config) : IJwtService
{
    public string GenerateJsonWebToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        _ = int.TryParse(config["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.SecondName),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenDataDto GenerateRefreshTokenData()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        _ = int.TryParse(config["JWT:RefreshTokenValidityInDays"], out var tokenValidityInMinutes);
        var refreshTokenDataDto = new RefreshTokenDataDto
        {
            RefreshToken = Convert.ToBase64String(randomNumber),
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(tokenValidityInMinutes)
        };
        return refreshTokenDataDto;
    }

    public IEnumerable<Claim>? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        return jsonToken?.Claims;
    }
}
