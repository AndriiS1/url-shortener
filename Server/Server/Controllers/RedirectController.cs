using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers;

[ApiController]
[Route("/")]
public class RedirectController(IUnitOfWork unitOfWork) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{code}")]
    public async Task<IResult> GetTableUrlsController(string code)
    {
        var foundUrl = await unitOfWork.Urls.SingleOrDefault(u => u.Code == code);

        return foundUrl is null ? Results.NotFound() : Results.Redirect(foundUrl.OriginalUrl);
    }
}
