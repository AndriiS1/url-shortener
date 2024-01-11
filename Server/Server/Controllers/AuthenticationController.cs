using Domain;
using Domain.Dto;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ServerPesentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IHashService _hashService;
        private readonly IValidationService _validationService;
        private readonly IConfiguration _config;

        public AuthenticationController(IUnitOfWork unitOfWork, IJwtService jwtService, IHashService hashService, IValidationService validationService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _hashService = hashService;
            _validationService = validationService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult LoginController(LoginDto loginData)
        {
            IActionResult response = Unauthorized("User not found.");
            var user = _unitOfWork.Users.SingleOrDefault(u => u.Password == _hashService.getHash(loginData.Password) && u.Email == loginData.Email);
            if (user != null)
            {
                var accessToken = _jwtService.GenerateJSONWebToken(user);
                var refreshTokenDataDto = _jwtService.GenerateRefreshTokenData();
                _unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, refreshTokenDataDto);
                _unitOfWork.Complete();
                response = Ok(new { accessToken = accessToken, refreshToken = refreshTokenDataDto.RefreshToken});
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user)
        {
            if (_validationService.UserIsValid(user))
            {
                var tryFindExistingUser = _unitOfWork.Users.FirstOrDefault(u => u.Email== user.Email);
                if (tryFindExistingUser != null) 
                {
                    return BadRequest("User with this email already exists.");
                }
                user.Password = _hashService.getHash(user.Password ?? "");
                _unitOfWork.Users.Add(user);
                _unitOfWork.Complete();
                var accessToken = _jwtService.GenerateJSONWebToken(user);
                var refreshTokenDataDto = _jwtService.GenerateRefreshTokenData();
                _unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, refreshTokenDataDto);
                _unitOfWork.Complete();
                return Ok(new { accessToken = accessToken, refreshToken = refreshTokenDataDto.RefreshToken });
            }
            else
            {
                return BadRequest("User data is not valid.");
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDto tokenData)
        {
            if (tokenData is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenData.AccessToken;
            string? refreshToken = tokenData.RefreshToken;

            var claims = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            if (claims == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var userId = claims.Single(claim => claim.Type == JwtRegisteredClaimNames.NameId.ToString())?.Value;

            var user = _unitOfWork.Users.Single(u => u.Id == long.Parse(userId));

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = _jwtService.GenerateJSONWebToken(user);
            var newRefreshTokenDataDto = _jwtService.GenerateRefreshTokenData();
            _unitOfWork.Users.UpdateUserRefreshTokenData(user.Id, newRefreshTokenDataDto);
            _unitOfWork.Complete();

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshTokenDataDto.RefreshToken
            });
        }
    }
}
