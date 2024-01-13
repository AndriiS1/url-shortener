using Domain;
using Domain.Dto;
using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace ServerPesentation.Controllers
{
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

            if (userIdFromToken != null){
                long userId = long.Parse(userIdFromToken);
                User? foundUser = _unitOfWork.Users.FirstOrDefault(e => e.Id == userId);
                if (foundUser?.Role == UserRole.Admin)
                {
                    return Ok(_unitOfWork.Urls.GetAllAdminTableUrls().ToList());
                }
                else
                {
                    return Ok(_unitOfWork.Urls.GetAllTableUrlsWithDeleteCheck(userId).ToList());
                }
            }
            return Ok(_unitOfWork.Urls.GetAllTableUrls().ToList());
        }

        [HttpPost]
        public IActionResult CreateUrlController(ShortenUrlDto shortenUrlDto)
        {
            var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdFromToken != null)
            {
                long userId = long.Parse(userIdFromToken);
                var tryOriginalUrl = _unitOfWork.Urls.SingleOrDefault(u => String.Compare(u.OriginalUrl, shortenUrlDto.OriginalUrl) == 0);
                if (tryOriginalUrl != null) 
                {
                    return BadRequest("This url is already shorten");
                }

                string generatedShortUrl = "";
                string code = "";
                while (true)
                {
                    string saltedPrefix = "";
                    string? serverAddress = string.Format("{0}://{1}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);
                    code = _urlShortenerService.GenerageShortUrl(shortenUrlDto?.OriginalUrl?.ToLower() + saltedPrefix);
                    generatedShortUrl = serverAddress + "/" + code;
                    var tryFindShortUrl = _unitOfWork.Urls.SingleOrDefault(u => u.ShortUrl == generatedShortUrl);
                    if(tryFindShortUrl == null){
                        break;
                    }
                    else
                    {
                        Random random = new Random();
                        saltedPrefix += random.Next(10).ToString();
                    }
                }

                Url urlInstanse = new Url { 
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
                Url? foundUrl = _unitOfWork.Urls.GetUrlWithLoadedUserData(id);
                if (foundUrl != null) 
                {
                    return Ok(new UrlInfoDto { Id = foundUrl.Id, 
                        OriginalUrl = foundUrl.OriginalUrl, 
                        ShortUrl = foundUrl.ShortUrl, 
                        Date = foundUrl.Date,
                        UserName = $"{foundUrl.User?.FirstName} {foundUrl.User?.SecondName}" });
                }
                else
                {
                    return BadRequest("Url with this id is not found.");
                }
            }
            return Unauthorized();
        }

        [HttpDelete("{id:long}")]
        public IActionResult DeleteUrl(long id)
        {
            var userIdFromToken = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdFromToken != null)
            {
                long userId = long.Parse(userIdFromToken);
                Url? foundUrl = _unitOfWork.Urls.SingleOrDefault(e=> e.Id == id);
                if (foundUrl != null)
                {
                    User? foundUser = _unitOfWork.Users.FirstOrDefault(e => e.Id == userId);
                    if(foundUrl.UserId == userId || foundUser?.Role == UserRole.Admin)
                    {
                        _unitOfWork.Urls.Remove(foundUrl);
                        _unitOfWork.Complete();
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest("No permission.");
                    }
                }
                else
                {
                    return BadRequest("Url with this id is not found.");
                }
            }
            return Unauthorized();
        }
    }
}
