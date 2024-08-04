using System.Security.Claims;
using Domain;
using Domain.Dtos;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ServerPesentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUrlShortenerService _urlShortenerService;

    public UrlController(IUnitOfWork unitOfWork, IUrlShortenerService urlShortenerService)
    {
        _unitOfWork = unitOfWork;
        _urlShortenerService = urlShortenerService;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetTableUrlsController()
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdFromToken != null)
        {
            var userId = long.Parse(userIdFromToken);
            var foundUser = _unitOfWork.Users.FirstOrDefault(e => e.Id == userId);
            if (foundUser?.Role == UserRole.Admin)
            {
                return Ok(_unitOfWork.Urls.GetAllAdminTableUrls().ToList());
            }
            return Ok(_unitOfWork.Urls.GetAllTableUrlsWithDeleteCheck(userId).ToList());
        }
        return Ok(_unitOfWork.Urls.GetAllTableUrls().ToList());
    }

    [HttpPost]
    public IActionResult CreateUrlController(ShortenUrlDto shortenUrlDto)
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdFromToken != null)
        {
            var userId = long.Parse(userIdFromToken);
            var tryOriginalUrl = _unitOfWork.Urls.SingleOrDefault(u => string.Compare(u.OriginalUrl, shortenUrlDto.OriginalUrl) == 0);
            if (tryOriginalUrl != null)
            {
                return BadRequest("This url is already shorten");
            }

            var generatedShortUrl = "";
            var code = "";
            while (true)
            {
                var saltedPrefix = "";
                var serverAddress = string.Format("{0}://{1}",
                    HttpContext.Request.Scheme, HttpContext.Request.Host);
                code = _urlShortenerService.GenerageShortUrl(shortenUrlDto?.OriginalUrl?.ToLower() + saltedPrefix);
                generatedShortUrl = serverAddress + "/" + code;
                var tryFindShortUrl = _unitOfWork.Urls.SingleOrDefault(u => u.ShortUrl == generatedShortUrl);
                if (tryFindShortUrl == null)
                {
                    break;
                }
                var random = new Random();
                saltedPrefix += random.Next(10).ToString();
            }

            var urlInstanse = new Url
            {
                Date = DateTime.Now,
                OriginalUrl = shortenUrlDto?.OriginalUrl,
                UserId = userId,
                Code = code,
                ShortUrl = generatedShortUrl
            };
            _unitOfWork.Urls.Add(urlInstanse);
            _unitOfWork.Complete();
            return Ok();
        }
        return Unauthorized();
    }

    [HttpGet("{id:long}")]
    public IActionResult GetUrlInfo(long id)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        if (claimsIdentity != null)
        {
            var foundUrl = _unitOfWork.Urls.GetUrlWithLoadedUserData(id);
            if (foundUrl != null)
            {
                return Ok(new UrlInfoDto
                {
                    Id = foundUrl.Id,
                    OriginalUrl = foundUrl.OriginalUrl,
                    ShortUrl = foundUrl.ShortUrl,
                    Date = foundUrl.Date,
                    UserName = $"{foundUrl.User?.FirstName} {foundUrl.User?.SecondName}"
                });
            }
            return BadRequest("Url with this id is not found.");
        }
        return Unauthorized();
    }

    [HttpDelete("{id:long}")]
    public IActionResult DeleteUrl(long id)
    {
        var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdFromToken != null)
        {
            var userId = long.Parse(userIdFromToken);
            var foundUrl = _unitOfWork.Urls.SingleOrDefault(e => e.Id == id);
            if (foundUrl != null)
            {
                var foundUser = _unitOfWork.Users.FirstOrDefault(e => e.Id == userId);
                if (foundUrl.UserId == userId || foundUser?.Role == UserRole.Admin)
                {
                    _unitOfWork.Urls.Remove(foundUrl);
                    _unitOfWork.Complete();
                    return NoContent();
                }
                return BadRequest("No permission.");
            }
            return BadRequest("Url with this id is not found.");
        }
        return Unauthorized();
    }
}
