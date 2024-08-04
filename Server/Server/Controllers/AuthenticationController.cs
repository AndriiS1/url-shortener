using System.IdentityModel.Tokens.Jwt;
using Domain;
using Domain.Dtos;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ServerPesentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(IUnitOfWork unitOfWork, IJwtService jwtService, IHashService hashService, IValidationService validationService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginController(LoginDto loginData)
    {
        var user = await unitOfWork.Users.SingleOrDefault(u => u.Password == hashService.GetHash(loginData.Password) && u.Email == loginData.Email);

        if (user == null)
            return Unauthorized("User not found.");

        var accessToken = jwtService.GenerateJSONWebToken(user);
        var refreshTokenDataDto = jwtService.GenerateRefreshTokenData();
        await unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, refreshTokenDataDto);
        await unitOfWork.Complete();

        return Ok(new
        {
            accessToken,
            refreshToken = refreshTokenDataDto.RefreshToken
        });
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(User user)
    {
        if (!validationService.UserIsValid(user))
            return BadRequest("User data is not valid.");

        var tryFindExistingUser = await unitOfWork.Users.SingleOrDefault(u => u.Email == user.Email);

        if (tryFindExistingUser != null)
            return BadRequest("User with this email already exists.");

        user.Password = hashService.GetHash(user.Password);
        user.Role = UserRole.Basic;
        await unitOfWork.Users.Add(user);

        var accessToken = jwtService.GenerateJSONWebToken(user);
        var refreshTokenDataDto = jwtService.GenerateRefreshTokenData();
        await unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, refreshTokenDataDto);
        await unitOfWork.Complete();
        return Ok(new
        {
            accessToken,
            refreshToken = refreshTokenDataDto.RefreshToken
        });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDto tokenData)
    {
        var accessToken = tokenData.AccessToken;
        var refreshToken = tokenData.RefreshToken;

        var claims = jwtService.GetPrincipalFromExpiredToken(accessToken);

        if (claims == null)
            return BadRequest("Invalid access or refresh token");

        var userId = claims.Single(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value;

        var user = await unitOfWork.Users.SingleOrDefault(u => u.Id == long.Parse(userId));

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access or refresh token");
        }

        var newAccessToken = jwtService.GenerateJSONWebToken(user);
        var newRefreshTokenDataDto = jwtService.GenerateRefreshTokenData();
        await unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, newRefreshTokenDataDto);
        await unitOfWork.Complete();

        return Ok(new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshTokenDataDto.RefreshToken
        });
    }
}
