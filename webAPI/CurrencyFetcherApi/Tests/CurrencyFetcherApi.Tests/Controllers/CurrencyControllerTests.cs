using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Interfaces;
using CurrencyFetcherApi.Controllers;
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

        private IEnumerable<CurrencyResult> _expectedResult =>
            new List<CurrencyResult>
            {
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,1),
                    CurrencyValue = 4.236m
                },
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,2),
                    CurrencyValue = 4.236m
                },
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,3),
                    CurrencyValue = 4.236m
                },
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2010,1,4),
                    CurrencyValue = 4.236m
                }
            };

        private CurrencyCollectionModel _model =>
            new CurrencyCollectionModel
            {
                CurrencyCodes = new Dictionary<string, string>
                {
                    {"USD", "PLN"}
                },
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };

        private CurrencyErrorModel _errorModel => 
            new CurrencyErrorModel(ErrorMessage);

        private Mock<ICurrencyService> _currencyServiceMock;
        private Mock<LoggerWrapper<CurrencyController>> _loggerMock;
        private CurrencyController _currencyController;
        
        [SetUp]
        public void Setup()
        {
            _currencyServiceMock = new Mock<ICurrencyService>();
            _loggerMock = new Mock<LoggerWrapper<CurrencyController>>();

            _currencyController = new CurrencyController(_currencyServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public void CurrencyControllerTests_SendValidData_ResultEmptyList()
        {
            _currencyServiceMock.Setup(c => c.GetCurrencyResults(It.IsAny<CurrencyCollectionModel>()))
                .Returns(Task.FromResult(new List<CurrencyResult>() as IEnumerable<CurrencyResult>));

            var result = _currencyController.Get(_model).GetAwaiter().GetResult();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            _currencyServiceMock.Verify(c => c.GetCurrencyResults(It.IsAny<CurrencyCollectionModel>()), Times.Once);

            okResult.Value.As<IEnumerable<CurrencyResult>>().Should().BeEmpty();

        }

        [Test]
        public void CurrencyControllerTests_SendValidData_ResultNotEmptyList()
        {
            _currencyServiceMock.Setup(c => c.GetCurrencyResults(It.IsAny<CurrencyCollectionModel>()))
                .Returns(Task.FromResult(_expectedResult));

            var result = _currencyController.Get(_model).GetAwaiter().GetResult();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            _currencyServiceMock.Verify(c => c.GetCurrencyResults(It.IsAny<CurrencyCollectionModel>()), Times.Once);

            okResult.Value.As<IEnumerable<CurrencyResult>>().ToList().Should().NotBeNullOrEmpty();
            okResult.Value.As<IEnumerable<CurrencyResult>>().ToList().Should().BeEquivalentTo(_expectedResult);

        }

        [Test]
        public void CurrencyControllerTests_SendValidData_ReturnsBadResponse()
        {
            CheckErrorMessageResponse<BadRequestObjectResult>(() => throw new BadRequestException(ErrorMessage));
        }

        [Test]
        public void CurrencyControllerTests_SendValidData_ReturnsNotFoundResponse()
        {
            CheckErrorMessageResponse<NotFoundObjectResult>(() => throw new NotFoundException(ErrorMessage));
        }

        [Test]
        public void CurrencyControllerTests_SendValidData_ThrowsUnknownError()
        {
            CheckErrorMessageResponse<BadRequestObjectResult>(() => throw new Exception(ErrorMessage),
                new CurrencyErrorModel("An unknown error occurs. Please contact to system administrator."));
        }

        private void CheckErrorMessageResponse<TObjectResult>(Func<Task<IEnumerable<CurrencyResult>>> errorAction, CurrencyErrorModel errorModel = null)
            where TObjectResult: ObjectResult
        {
            _currencyServiceMock.Setup(c => c.GetCurrencyResults(It.IsAny<CurrencyCollectionModel>()))
                .Returns(errorAction);

            var result = _currencyController.Get(_model).GetAwaiter().GetResult();

            result.Should().BeOfType<TObjectResult>();
            var errorResponse = result as TObjectResult;

            _currencyServiceMock.Verify(c => c.GetCurrencyResults(It.IsAny<CurrencyCollectionModel>()), Times.Once);

            errorResponse.Value.As<CurrencyErrorModel>().Should().NotBeNull();
            errorResponse.Value.As<CurrencyErrorModel>().Should().BeEquivalentTo(errorModel ?? _errorModel);
        }

        
    }
}