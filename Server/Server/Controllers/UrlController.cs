using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Commands.CreateUrlCommand;
using UseCase.Commands.DeleteUrlCommand;
using UseCase.Queries.GetTableUrlsQuery;
using UseCase.Queries.GetUrlQuery;
namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTableUrlsController()
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        long? userId = userIdFromToken is null ? null : long.Parse(userIdFromToken);

        return await mediator.Send(new GetTableUrlQuery(userId));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUrlController([FromBody] string originalUrl)
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdFromToken == null)
            return Unauthorized();

        var userId = long.Parse(userIdFromToken);

        return await mediator.Send(new CreateUrlCommand(originalUrl, userId, HttpContext.Request.Scheme, HttpContext.Request.Host.Host));
    }

    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetUrlInfo(long id)
    {
        if (User.Identity is not ClaimsIdentity)
            return Unauthorized();

        return await mediator.Send(new GetUrlQuery(id));
    }

    [HttpDelete("{id:long}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(long id)
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdFromToken == null)
            return Unauthorized();

        var userId = long.Parse(userIdFromToken);

        return await mediator.Send(new DeleteUrlCommand(id, userId));
    }
}
