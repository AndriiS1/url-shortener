using System.Linq.Expressions;
using Domain;
using Domain.Dtos;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServerPesentation.Controllers;
using static NUnit.Framework.Assert;
namespace Tests.Tests.AuthenticationControllerTests;

public sealed class LoginTest
{
    [TestFixture]
    public class AuthenticationControllerTests
    {

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtServiceMock = new Mock<IJwtService>();
            _hashServiceMock = new Mock<IHashService>();
            _validationServiceMock = new Mock<IValidationService>();

            _authenticationController = new AuthenticationController(
                _unitOfWorkMock.Object,
                _jwtServiceMock.Object,
                _hashServiceMock.Object,
                _validationServiceMock.Object
            );
        }
        private AuthenticationController _authenticationController;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<IHashService> _hashServiceMock;
        private Mock<IValidationService> _validationServiceMock;

        [Test]
        public async Task LoginController_ValidLoginData_ReturnsOkResultWithAccessTokenAndRefreshToken()
        {
            const string userPassword = "1ecxxcJsdEREw";
            var accessToken = "accessToken";
            var refreshToken = "refreshToken";
            // Arrange
            var loginData = new LoginDto
            {
                Email = "test@example.com",
                Password = userPassword
            };

            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Password = "hashedPassword",
                FirstName = "Name",
                SecondName = "Second name",
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now
            };


            var refreshTokenDataDto = new RefreshTokenDataDto
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(2)
            };

            _unitOfWorkMock.Setup(u => u.Users.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(user)!);
            _jwtServiceMock.Setup(j => j.GenerateJSONWebToken(user)).Returns(accessToken);
            _jwtServiceMock.Setup(j => j.GenerateRefreshTokenData()).Returns(refreshTokenDataDto);
            _hashServiceMock.Setup(h => h.GetHash(userPassword)).Returns("hashedPassword");

            // Act
            var result = (await _authenticationController.LoginController(loginData)) as OkObjectResult;
            That(result, Is.Not.Null);
            That(result?.StatusCode, Is.EqualTo(200));
            Action<object?> isNotNull = IsNotNull;
            isNotNull(result?.Value);
            That(result?.Value?.GetType().GetProperty("accessToken")?.GetValue(result.Value), Is.EqualTo(accessToken));
            AreEqual(refreshTokenDataDto.RefreshToken, result?.Value?.GetType().GetProperty("refreshToken")?.GetValue(result.Value));
        }

        [Test]
        public async Task LoginController_InvalidLoginData_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginData = new LoginDto
            {
                Email = "test@example.com",
                Password = "password"
            };

            _unitOfWorkMock.Setup(u => u.Users.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult((User)null));

            // Act
            var result = (await _authenticationController.LoginController(loginData)) as UnauthorizedObjectResult;

            // Assert
            Multiple(() =>
            {
                That(result, Is.Not.Null);
                That(result?.StatusCode, Is.EqualTo(401));
                That(result?.Value, Is.EqualTo("User not found."));
            });
        }
    }
}
