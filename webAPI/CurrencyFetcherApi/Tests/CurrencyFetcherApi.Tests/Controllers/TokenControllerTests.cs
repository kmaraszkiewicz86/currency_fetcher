using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcherApi.Controllers;
using CurrencyFetcherApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CurrencyFetcherApi.Tests.Controllers
{
    public class TokenControllerTests
    {
        private TokenResponse ExpectedResult =>
            new TokenResponse
            {
                Token = "test_token"
            };

        private TokenAuthRequest AuthModel =>
            new TokenAuthRequest
            {
                Password = "test_password",
                Username = "username"
            };

        private Mock<ITokenService> _userServiceMock;
        private Mock<ILogger<TokenController>> _loggerMock;
        private TokenController _tokenController;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<ITokenService>();
            _loggerMock = new Mock<ILogger<TokenController>>();

            _tokenController = new TokenController(_userServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Login_SendValidCredentials_ReturnsToken()
        {
            _userServiceMock.Setup(u => u.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(ExpectedResult));

            var result = _tokenController.Login(AuthModel).GetAwaiter().GetResult();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            _userServiceMock.Verify(u => u.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            okResult.Value.As<TokenResponse>().Should().NotBeNull();
            okResult.Value.As<TokenResponse>().Should().BeEquivalentTo(ExpectedResult);
        }

        [Test]
        public void Login_SendInvalidCredentials_ReturnsBadRequest()
        {
            _userServiceMock.Setup(u => u.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<TokenResponse>(null));

            var result = _tokenController.Login(AuthModel).GetAwaiter().GetResult();

            result.Should().BeOfType<BadRequestObjectResult>();
            var okResult = result as BadRequestObjectResult;

            _userServiceMock.Verify(u => u.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            okResult.Value.As<CurrencyErrorResponse>().Should().NotBeNull();
            okResult.Value.As<CurrencyErrorResponse>().Should().BeEquivalentTo(new CurrencyErrorResponse("Username or Password is invalid"));
        }
    }
}
