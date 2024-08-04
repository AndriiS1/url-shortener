using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<IActionResult>;
