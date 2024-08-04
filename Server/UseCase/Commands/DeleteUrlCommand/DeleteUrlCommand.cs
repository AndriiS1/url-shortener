using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.DeleteUrlCommand;

public record DeleteUrlCommand(long LinkId, long UserId) : IRequest<IActionResult>;
