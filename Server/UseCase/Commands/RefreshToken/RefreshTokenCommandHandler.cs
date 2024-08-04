using Domain;
using Infrastructure.Services.JwtService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
namespace UseCase.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService) : IRequestHandler<RefreshTokenCommand, IActionResult>
{
    public async Task<IActionResult> Handle(RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var claims = jwtService.GetPrincipalFromExpiredToken(command.AccessToken);

        if (claims == null)
            return new BadRequestObjectResult("Invalid access or refresh token");

        var userId = claims.Single(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value;

        var user = await unitOfWork.Users.SingleOrDefault(u => u.Id == long.Parse(userId));

        if (user == null || user.RefreshToken != command.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return new BadRequestObjectResult("Invalid access or refresh token");
        }

        var newAccessToken = jwtService.GenerateJsonWebToken(user);
        var newRefreshTokenDataDto = jwtService.GenerateRefreshTokenData();
        await unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, newRefreshTokenDataDto);
        await unitOfWork.Complete();

        return new OkObjectResult(new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshTokenDataDto.RefreshToken
        });
    }
}
