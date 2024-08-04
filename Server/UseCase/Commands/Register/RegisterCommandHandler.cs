using Domain;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.Register;

public class RegisterCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IHashService hashService, IValidationService validationService) : IRequestHandler<RegisterCommand, IActionResult>
{
    public async Task<IActionResult> Handle(RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var refreshTokenDataDto = jwtService.GenerateRefreshTokenData();
        var user = new User
        {
            FirstName = command.FirstName,
            SecondName = command.SecondName,
            Email = command.Email,
            Password = hashService.GetHash(command.Password),
            Role = UserRole.Basic,
            RefreshToken = refreshTokenDataDto.RefreshToken,
            RefreshTokenExpiryTime = refreshTokenDataDto.RefreshTokenExpiryTime
        };

        if (!validationService.UserIsValid(user))
            return new BadRequestObjectResult("User data is not valid.");

        var tryFindExistingUser = await unitOfWork.Users.SingleOrDefault(u => u.Email == user.Email);

        if (tryFindExistingUser != null)
            return new BadRequestObjectResult("User with this email already exists.");

        await unitOfWork.Users.Add(user);
        await unitOfWork.Complete();

        var accessToken = jwtService.GenerateJsonWebToken(user);
        return new OkObjectResult(new
        {
            accessToken,
            refreshToken = refreshTokenDataDto.RefreshToken
        });
    }
}
