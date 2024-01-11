using Domain;
using Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerPesentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UrlController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get-table-urls")]
        public IActionResult GetTableUrlsController()
        {
            return Ok(_unitOfWork.Urls.GetAllTableUrls().ToList());
        }
    }
}
