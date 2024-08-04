using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Commands.Login;
using UseCase.Commands.Register;
namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginController(LoginCommand command)
    {
        return await mediator.Send(command);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterCommand registerCommand)
    {
        return await mediator.Send(registerCommand);
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(RegisterCommand command)
    {
        return await mediator.Send(command);
    }
}
