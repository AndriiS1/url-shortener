using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<IActionResult>;
