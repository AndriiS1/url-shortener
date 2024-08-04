using System.Security.Claims;
using Domain;
using Domain.Dtos;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController(IUnitOfWork unitOfWork, IUrlShortenerService urlShortenerService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTableUrlsController()
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdFromToken == null)
            return Ok(unitOfWork.Urls.GetAllTableUrls().ToList());

        var userId = long.Parse(userIdFromToken);
        var foundUser = await unitOfWork.Users.SingleOrDefault(e => e.Id == userId);

        return foundUser?.Role == UserRole.Admin ? Ok(unitOfWork.Urls.GetAllAdminTableUrls().ToList())
            : Ok(unitOfWork.Urls.GetAllTableUrlsWithDeleteCheck(userId).ToList());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUrlController(ShortenUrlDto shortenUrlDto)
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdFromToken == null)
            return Unauthorized();

        var userId = long.Parse(userIdFromToken);
        var tryOriginalUrl = await unitOfWork.Urls.SingleOrDefault(u => string.Compare(u.OriginalUrl, shortenUrlDto.OriginalUrl) == 0);

        if (tryOriginalUrl != null)
            return BadRequest("This url is already shorten");

        var generatedShortUrl = "";
        var code = "";
        while (true)
        {
            var saltedPrefix = "";
            var serverAddress = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            code = urlShortenerService.GenerateShortUrl(shortenUrlDto.OriginalUrl.ToLower() + saltedPrefix);
            generatedShortUrl = serverAddress + "/" + code;
            var tryFindShortUrl = await unitOfWork.Urls.SingleOrDefault(u => u.ShortUrl == generatedShortUrl);

            if (tryFindShortUrl == null)
                break;

            var random = new Random();
            saltedPrefix += random.Next(10).ToString();
        }

        var urlInstance = new Url
        {
            Date = DateTime.Now,
            OriginalUrl = shortenUrlDto.OriginalUrl,
            UserId = userId,
            Code = code,
            ShortUrl = generatedShortUrl
        };

        await unitOfWork.Urls.Add(urlInstance);
        await unitOfWork.Complete();
        return Ok();
    }

    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<IActionResult> GetUrlInfo(long id)
    {
        if (User.Identity is not ClaimsIdentity)
            return Unauthorized();

        var foundUrl = await unitOfWork.Urls.GetUrlWithLoadedUserData(id);

        if (foundUrl == null)
            return BadRequest("Url with this id is not found.");

        return Ok(new UrlInfoDto
        {
            Id = foundUrl.Id,
            OriginalUrl = foundUrl.OriginalUrl,
            ShortUrl = foundUrl.ShortUrl,
            Date = foundUrl.Date,
            UserName = $"{foundUrl.User.FirstName} {foundUrl.User.SecondName}"
        });
    }

    [HttpDelete("{id:long}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(long id)
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdFromToken == null)
            return Unauthorized();

        var userId = long.Parse(userIdFromToken);
        var foundUrl = await unitOfWork.Urls.SingleOrDefault(e => e.Id == id);

        if (foundUrl == null)
            return BadRequest("Url with this id is not found.");

        var foundUser = await unitOfWork.Users.SingleOrDefault(e => e.Id == userId);

        if (foundUrl.UserId != userId && foundUser?.Role != UserRole.Admin) return BadRequest("No permission.");

        unitOfWork.Urls.Remove(foundUrl);
        await unitOfWork.Complete();
        return NoContent();
    }
}
