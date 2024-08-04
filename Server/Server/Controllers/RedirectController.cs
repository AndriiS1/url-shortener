using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Commands.Redirect;
namespace Presentation.Controllers;

[ApiController]
[Route("/")]
public class RedirectController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{code}")]
    public async Task<IActionResult> GetTableUrlsController(string code)
    {
        return await mediator.Send(new RedirectCommand(code));
    }
}
