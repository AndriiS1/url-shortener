using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.Redirect;

public record RedirectCommand(string code) : IRequest<IActionResult>;
