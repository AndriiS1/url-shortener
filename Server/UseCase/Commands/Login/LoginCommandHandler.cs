using Domain;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.Login;

public class LoginCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IHashService hashService) : IRequestHandler<LoginCommand, IActionResult>
{
    public async Task<IActionResult> Handle(LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.SingleOrDefault(u => u.Password == hashService.GetHash(command.Password) && u.Email == command.Email);

        if (user == null)
            return new UnauthorizedObjectResult("User not found.");

        var accessToken = jwtService.GenerateJsonWebToken(user);
        var refreshTokenDataDto = jwtService.GenerateRefreshTokenData();
        await unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, refreshTokenDataDto);
        await unitOfWork.Complete();

        return new OkObjectResult(new
        {
            accessToken,
            refreshToken = refreshTokenDataDto.RefreshToken
        });
    }
}
