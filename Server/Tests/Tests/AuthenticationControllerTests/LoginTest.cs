using System.Linq.Expressions;
using Domain;
using Domain.Dtos;
using Domain.Models;
using Infrastructure.Services.HashService;
using Infrastructure.Services.JwtService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UseCase.Commands.Login;
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
        }

        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<IHashService> _hashServiceMock;

        [Test]
        public async Task LoginCommandHandler_ValidLoginData_ReturnsOkResultWithAccessTokenAndRefreshToken()
        {
            // Arrange
            const string userPassword = "1ecxxcJsdEREw";
            const string accessToken = "accessToken";
            const string refreshToken = "refreshToken";

            var loginCommand = new LoginCommand("test@example.com", userPassword);
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

            _unitOfWorkMock.Setup(u => u.Users.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
            _jwtServiceMock.Setup(j => j.GenerateJsonWebToken(user)).Returns(accessToken);
            _jwtServiceMock.Setup(j => j.GenerateRefreshTokenData()).Returns(refreshTokenDataDto);
            _hashServiceMock.Setup(h => h.GetHash(userPassword)).Returns("hashedPassword");

            var handler = new LoginCommandHandler(_unitOfWorkMock.Object, _jwtServiceMock.Object, _hashServiceMock.Object);

            // Act
            var result = await handler.Handle(loginCommand, CancellationToken.None) as OkObjectResult;

            // Assert
            That(result, Is.Not.Null);
            That(result?.StatusCode, Is.EqualTo(200));
            That(result?.Value, Is.Not.Null);

            var resultValue = result?.Value?.GetType().GetProperty("accessToken")?.GetValue(result.Value);
            That(resultValue, Is.EqualTo(accessToken));

            var resultRefreshToken = result?.Value?.GetType().GetProperty("refreshToken")?.GetValue(result.Value);
            That(resultRefreshToken, Is.EqualTo(refreshTokenDataDto.RefreshToken));
        }


        [Test]
        public async Task LoginCommandHandler_InvalidLoginData_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginCommand = new LoginCommand("test@example.com", "password");

            _unitOfWorkMock.Setup(u => u.Users.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User)null);

            var handler = new LoginCommandHandler(_unitOfWorkMock.Object, _jwtServiceMock.Object, _hashServiceMock.Object);

            // Act
            var result = await handler.Handle(loginCommand, CancellationToken.None) as UnauthorizedObjectResult;

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
