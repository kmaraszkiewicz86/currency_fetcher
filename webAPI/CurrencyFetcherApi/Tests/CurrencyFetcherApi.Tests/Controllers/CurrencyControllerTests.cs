using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Interfaces;
using CurrencyFetcherApi.Controllers;
using CurrencyFetcherApi.Services;
using CurrencyFetcherApi.Tests.Wrappers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CurrencyFetcherApi.Tests.Controllers
{
    public class CurrencyControllerTests
    {
        private const string ErrorMessage = "Test error message";

        private IEnumerable<CurrencyResultResponse> _expectedResult =>
            new List<CurrencyResultResponse>
            {
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,1),
                    CurrencyValue = 4.236m
                },
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,2),
                    CurrencyValue = 4.236m
                },
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,3),
                    CurrencyValue = 4.236m
                },
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,4),
                    CurrencyValue = 4.236m
                }
            };

        private CurrencyCollectionRequest _model =>
            new CurrencyCollectionRequest
            {
                CurrencyCodes = new Dictionary<string, string>
                {
                    {"USD", "PLN"}
                },
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };

        private CurrencyErrorResponse _errorModel => 
            new CurrencyErrorResponse(ErrorMessage);

        private Mock<ICurrencyService> _currencyServiceMock;
        private Mock<LoggerWrapper<CurrencyController>> _loggerMock;
        private Mock<ITokenService> _userServiceMock;
        private CurrencyController _currencyController;

        [SetUp]
        public void Setup()
        {
            _currencyServiceMock = new Mock<ICurrencyService>();
            _loggerMock = new Mock<LoggerWrapper<CurrencyController>>();
            _userServiceMock = new Mock<ITokenService>();

            _currencyController =
                new CurrencyController(_currencyServiceMock.Object, _loggerMock.Object, _userServiceMock.Object);
        }

        [Test]
        public void Get_SendValidData_ResultEmptyList()
        {
            _currencyServiceMock.Setup(c => c.GetCurrencyResultsAsync(It.IsAny<CurrencyCollectionRequest>()))
                .Returns(Task.FromResult(new List<CurrencyResultResponse>() as IEnumerable<CurrencyResultResponse>));

            var result = _currencyController.Get(_model).GetAwaiter().GetResult();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            _currencyServiceMock.Verify(c => c.GetCurrencyResultsAsync(It.IsAny<CurrencyCollectionRequest>()), Times.Once);

            okResult.Value.As<IEnumerable<CurrencyResultResponse>>().Should().BeEmpty();

        }

        [Test]
        public void Get_SendValidData_ResultNotEmptyList()
        {
            _currencyServiceMock.Setup(c => c.GetCurrencyResultsAsync(It.IsAny<CurrencyCollectionRequest>()))
                .Returns(Task.FromResult(_expectedResult));

            var result = _currencyController.Get(_model).GetAwaiter().GetResult();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            _currencyServiceMock.Verify(c => c.GetCurrencyResultsAsync(It.IsAny<CurrencyCollectionRequest>()), Times.Once);

            okResult.Value.As<IEnumerable<CurrencyResultResponse>>().ToList().Should().NotBeNullOrEmpty();
            okResult.Value.As<IEnumerable<CurrencyResultResponse>>().ToList().Should().BeEquivalentTo(_expectedResult);

        }

        [Test]
        public void Get_SendValidData_ReturnsBadResponse()
        {
            CheckErrorMessageResponse<BadRequestObjectResult>(() => throw new BadRequestException(ErrorMessage));
        }

        [Test]
        public void Get_SendValidData_ReturnsNotFoundResponse()
        {
            CheckErrorMessageResponse<NotFoundObjectResult>(() => throw new NotFoundException(ErrorMessage));
        }

        [Test]
        public void Get_SendValidData_ThrowsUnknownError()
        {
            CheckErrorMessageResponse<BadRequestObjectResult>(() => throw new Exception(ErrorMessage),
                new CurrencyErrorResponse("An unknown error occurs. Please contact to system administrator."));
        }

        private void CheckErrorMessageResponse<TObjectResult>(Func<Task<IEnumerable<CurrencyResultResponse>>> errorAction, CurrencyErrorResponse errorModel = null)
            where TObjectResult: ObjectResult
        {
            _currencyServiceMock.Setup(c => c.GetCurrencyResultsAsync(It.IsAny<CurrencyCollectionRequest>()))
                .Returns(errorAction);

            var result = _currencyController.Get(_model).GetAwaiter().GetResult();

            result.Should().BeOfType<TObjectResult>();
            var errorResponse = result as TObjectResult;

            _currencyServiceMock.Verify(c => c.GetCurrencyResultsAsync(It.IsAny<CurrencyCollectionRequest>()), Times.Once);

            errorResponse.Value.As<CurrencyErrorResponse>().Should().NotBeNull();
            errorResponse.Value.As<CurrencyErrorResponse>().Should().BeEquivalentTo(errorModel ?? _errorModel);
        }

        
    }
}