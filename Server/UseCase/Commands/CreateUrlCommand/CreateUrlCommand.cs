using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace UseCase.Commands.CreateUrlCommand;

public record CreateUrlCommand(string OriginalUrl, long UserId, string Scheme, string Host) : IRequest<IActionResult>;
