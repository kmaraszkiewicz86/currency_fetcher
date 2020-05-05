using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Entities;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Implementations;
using CurrencyFetcher.Core.Services.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CurrencyFetcher.Core.Tests.Services.Implementations
{
    public class CurrencyServiceTest
    {
        private readonly Mock<ICurrencyGetterService> _currencyGetterServiceMock;
        private readonly Mock<IXmlReader> _xmlReaderMock;
        private readonly Mock<IDateChecker> _dateCheckerMock;
        private readonly Mock<ICacheDatabase> _cacheDatabaseMock;

        private readonly CurrencyService _currencyService;

        public CurrencyServiceTest()
        {
            _currencyGetterServiceMock = new Mock<ICurrencyGetterService>();
            _xmlReaderMock = new Mock<IXmlReader>();
            _dateCheckerMock = new Mock<IDateChecker>();
            _cacheDatabaseMock = new Mock<ICacheDatabase>();

            _currencyService = new CurrencyService(_currencyGetterServiceMock.Object,
                _xmlReaderMock.Object,
                _dateCheckerMock.Object,
                _cacheDatabaseMock.Object);
        }

        [Test]
        public void GetCurrencyResults_InvalidDate_ThrowsBadRequest()
        {
            _dateCheckerMock
                .Setup(d => d.ValidateDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new BadRequestException("Test error message"));

            Func<IEnumerable<CurrencyResult>> action = () => _currencyService.GetCurrencyResults(new CurrencyCollectionModel
            {
                CurrencyCodes = new Dictionary<string, string>
                {
                    { "PLN", "USD" }
                },
                StartDate = new DateTime(2009, 1,1),
                EndDate = new DateTime(2009, 1, 10)
            }).GetAwaiter().GetResult();

            action.Should().Throw<BadRequestException>();
        }

        [Test]
        public void GetCurrencyResults_CachedData_ReturnsNoEmptyList()
        {
            var expectedValue = new List<CurrencyValue>
            {
                new CurrencyValue
                {
                    Id = 1,
                    Value = 4.1m,
                    DailyDataOfCurrency = new DateTime(2009, 1, 1),
                    Currency = new Currency
                    {
                        Id = 1,
                        CurrencyBeingMeasured = "PLN",
                        CurrencyMatched = "USD"
                    }
                }
            };

            _dateCheckerMock
                .Setup(d => d.ValidateDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            _dateCheckerMock.Setup(d => d.SetCorrectDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(() => (new DateTime(2009, 1, 1), new DateTime(2009, 1, 1)));

            _cacheDatabaseMock.Setup(c => c.GetAsync(It.IsAny<CurrencyModel>()))
                .Returns(expectedValue);

            List<CurrencyResult> currencyResults = _currencyService.GetCurrencyResults(new CurrencyCollectionModel

            {
                CurrencyCodes = new Dictionary<string, string>
                {
                    {"PLN", "USD"}
                },
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2009, 1, 10)
            }).GetAwaiter().GetResult().ToList();

            _currencyGetterServiceMock.Verify(c => c.FetchDataAsync(It.IsAny<CurrencyModel>()), Times.Never);
            _xmlReaderMock.Verify(c => c.GetCurrencyResults(It.IsAny<CurrencyModel>(), It.IsAny<string>()), Times.Never);
            _cacheDatabaseMock.Verify(c => c.SaveAsync(It.IsAny<CurrencyResult>()), Times.Never);

            currencyResults.Should().NotBeEmpty();
            currencyResults.ForEach(c =>
                {
                    c.CurrencyBeingMeasured.Should().Be(expectedValue[0].Currency.CurrencyBeingMeasured);
                    c.CurrencyMatched.Should().Be(expectedValue[0].Currency.CurrencyMatched);
                    c.DailyDataOfCurrency.Should().Be(expectedValue[0].DailyDataOfCurrency);
                    c.CurrencyValue.Should().Be(expectedValue[0].Value);
                });
        }

        [Test]
        public void GetCurrencyResults_NotCachedData_ReturnsNoEmptyList()
        {
            var expectedValue = new List<CurrencyResult>
            {
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2009, 1,1),
                    CurrencyValue = 4.1m
                },
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "USD",
                    DailyDataOfCurrency = new DateTime(2009, 1,2),
                    CurrencyValue = 4.1m
                }
            };

            _dateCheckerMock
                .Setup(d => d.ValidateDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            _dateCheckerMock.Setup(d => d.SetCorrectDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(() => (new DateTime(2009, 1, 1), new DateTime(2009, 1, 1)));

            _cacheDatabaseMock.Setup(c => c.GetAsync(It.IsAny<CurrencyModel>()))
                .Returns(new List<CurrencyValue>());

            _currencyGetterServiceMock.Setup(c => c.FetchDataAsync(It.IsAny<CurrencyModel>()))
                .Returns(Task.FromResult("wcf tests xml"));

            _xmlReaderMock.Setup(x => x.GetCurrencyResults(It.IsAny<CurrencyModel>(), It.IsAny<string>()))
                .Returns(expectedValue);

            _cacheDatabaseMock.Setup(c => c.SaveAsync(It.IsAny<CurrencyResult>()));

            List<CurrencyResult> currencyResults = _currencyService.GetCurrencyResults(new CurrencyCollectionModel

            {
                CurrencyCodes = new Dictionary<string, string>
                {
                    {"PLN", "USD"}
                },
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2009, 1, 10)
            }).GetAwaiter().GetResult().ToList();

            _currencyGetterServiceMock.Verify(c => c.FetchDataAsync(It.IsAny<CurrencyModel>()), Times.Once);
            _xmlReaderMock.Verify(c => c.GetCurrencyResults(It.IsAny<CurrencyModel>(), It.IsAny<string>()), Times.Once);
            _cacheDatabaseMock.Verify(c => c.SaveAsync(It.IsAny<CurrencyResult>()), Times.Exactly(2));

            currencyResults.Should().NotBeEmpty();
            currencyResults.Should().BeEquivalentTo(currencyResults);
        }

        [Test]
        public void GetCurrencyResults_EmptyResultFromApi_ReturnsEmptyList()
        {
            _dateCheckerMock
                .Setup(d => d.ValidateDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            _dateCheckerMock.Setup(d => d.SetCorrectDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(() => (new DateTime(2009, 1, 1), new DateTime(2009, 1, 1)));

            _cacheDatabaseMock.Setup(c => c.GetAsync(It.IsAny<CurrencyModel>()))
                .Returns(new List<CurrencyValue>());

            _xmlReaderMock.Setup(x => x.GetCurrencyResults(It.IsAny<CurrencyModel>(), It.IsAny<string>()))
                .Returns(new List<CurrencyResult>());


            List<CurrencyResult> currencyResults = _currencyService.GetCurrencyResults(new CurrencyCollectionModel

            {
                CurrencyCodes = new Dictionary<string, string>
                {
                    {"PLN", "USD"}
                },
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2009, 1, 10)
            }).GetAwaiter().GetResult().ToList();

            currencyResults.Should().BeEmpty();
        }

        [TearDown]
        public void TearDown()
        {
            _currencyGetterServiceMock.Invocations.Clear();
            _xmlReaderMock.Invocations.Clear();
            _cacheDatabaseMock.Invocations.Clear();
        }
    }
}
