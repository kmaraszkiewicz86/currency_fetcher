using System;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Services.Implementations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CurrencyFetcher.Core.Tests.Services.Implementations
{
    public class CurrencyGetterServiceTest
    {
        private CurrencyGetterService _currencyGetterService;

        private Mock<ILogger<CurrencyGetterService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CurrencyGetterService>>();

            _currencyGetterService = new CurrencyGetterService(_loggerMock.Object);
        }

        [Test]
        public void FetchData_WithValidData_ReturnNoEmptyResult()
        {
             string result = _currencyGetterService.FetchDataAsync(new CurrencyModel
                {
                    StartDate = new DateTime(2009, 1, 1),
                    EndDate = new DateTime(2009, 1, 5),
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "EUR"
                }).GetAwaiter().GetResult();

             result.Should().NotBeEmpty();
        }

        [Test]
        public void FetchData_WithInValidCurrencyData_ReturnNoEmptyResult()
        {
            var xmlBody = _currencyGetterService.FetchDataAsync(new CurrencyModel
            {
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2009, 1, 5),
                CurrencyBeingMeasured = "PLN",
                CurrencyMatched = "EUR123"
            }).GetAwaiter().GetResult();

            xmlBody.Should().BeEmpty();
        }

        [Test]
        public void FetchData_WithInValidDate_ReturnEmptyResult()
        {

            string result = _currencyGetterService.FetchDataAsync(new CurrencyModel
            {
                StartDate = new DateTime(1980, 1, 1),
                EndDate = new DateTime(1980, 1, 5),
                CurrencyBeingMeasured = "PLN",
                CurrencyMatched = "EUR"
            }).GetAwaiter().GetResult(); ;

            result.Should().BeEmpty();
        }
    }
}
