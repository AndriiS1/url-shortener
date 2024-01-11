using Domain;
using Domain.Dto;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(_unitOfWork.Urls.GetAllTableUrls().ToList());
        }

        [HttpPost]
        public IActionResult GetTableUrlsController(ShortenUrlDto shortenUrlDto)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userId = long.Parse(claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                Url urlInstanse = new Url { Date = DateTime.Now, OriginalUrl = shortenUrlDto.OriginalUrl, UserId = userId, ShortUrl = _urlShortenerService.GenerageShortUrl(shortenUrlDto.OriginalUrl) };
                _unitOfWork.Urls.Add(urlInstanse);
                _unitOfWork.Complete();
                return Ok();
            }
            return Unauthorized();
        }
    }
}
