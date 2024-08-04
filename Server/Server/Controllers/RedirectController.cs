using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ServerPesentation.Controllers;

[ApiController]
[Route("/")]
public class RedirectController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public RedirectController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    [AllowAnonymous]
    [HttpGet("{code:long}")]
    public IResult GetTableUrlsController(string code)
    {
        var foundUrl = _unitOfWork.Urls.SingleOrDefault(u => u.Code == code);

        if (foundUrl != null)
        {
            return Results.Redirect(foundUrl.OriginalUrl);
        }
        return Results.NotFound();
    }
}
