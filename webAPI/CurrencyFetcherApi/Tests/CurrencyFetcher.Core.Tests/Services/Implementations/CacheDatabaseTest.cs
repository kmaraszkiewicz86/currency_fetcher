using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyFetcher.Core.Core;
using CurrencyFetcher.Core.Entities;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Implementations;
using CurrencyFetcher.Core.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;

namespace CurrencyFetcher.Core.Tests.Services.Implementations
{
    public class CacheDatabaseTest
    {
        private CurrencyResult CurrencyResult =>
            new CurrencyResult
            {
                CurrencyBeingMeasured = "PLN",
                CurrencyMatched = "USD",
                DailyDataOfCurrency = new DateTime(2010, 1, 1),
                CurrencyValue = 4.1m
            };

        private Mock<IHolidayChecker> _holidayCheckerMock;
        private CurrencyDbContext _dbContext;
        private CacheDatabase _cacheDatabase;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CurrencyDbContext>()
                .UseInMemoryDatabase(databaseName: "CurrencyListDatabase")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _dbContext = new CurrencyDbContext(options);

            _holidayCheckerMock = new Mock<IHolidayChecker>();

            _cacheDatabase = new CacheDatabase(_dbContext, _holidayCheckerMock.Object);
        }

        [Test]
        public void SaveAsync_SaveCurrencyValues()
        {
            _cacheDatabase.SaveAsync(CurrencyResult).GetAwaiter().GetResult();

            _cacheDatabase.SaveAsync(CurrencyResult).GetAwaiter().GetResult();

            _dbContext.Currencies.ToListAsync().GetAwaiter().GetResult().Count.Should().Be(1);

            _dbContext.Currencies.ToListAsync().GetAwaiter().GetResult().ForEach(c =>
                {
                    c.CurrencyBeingMeasured.Should().Be(CurrencyResult.CurrencyBeingMeasured);
                    c.CurrencyMatched.Should().Be(CurrencyResult.CurrencyMatched);
                    c.CurrencyValues.Count.Should().Be(1);
                    c.CurrencyValues.ToList().ForEach(cv =>
                    {
                        cv.Value.Should().Be(CurrencyResult.CurrencyValue);
                        cv.DailyDataOfCurrency.Should()
                            .Be(CurrencyResult.DailyDataOfCurrency);
                    });
                });
        }

        [Test]
        public void GetAsync_GetDataFromEmptyDatabase_ReturnsEmptyList()
        {
            _holidayCheckerMock.Setup(h => h.ReturnDateBeforeDayOff(It.IsAny<DateTime>()))
                .Returns(new DateTime(2010, 1, 1));

            List<CurrencyValue> currencyValues = _cacheDatabase.GetAsync(new CurrencyModel
            {
                CurrencyBeingMeasured = "PLN",
                CurrencyMatched = "USD",
                StartDate = new DateTime(2010, 1, 1),
                EndDate = new DateTime(2010, 1, 3)
            }).ToList();

            currencyValues.Should().BeEmpty();
        }

        [Test]
        public void GetAsync_GetDataFromNotEmptyDatabase_ReturnsNotEmptyList()
        {
            _cacheDatabase.SaveAsync(CurrencyResult).GetAwaiter().GetResult();

            _holidayCheckerMock.Setup(h => h.ReturnDateBeforeDayOff(It.IsAny<DateTime>()))
                .Returns(new DateTime(2010, 1, 1));

            List<CurrencyValue> currencyValues = _cacheDatabase.GetAsync(new CurrencyModel
            {
                CurrencyBeingMeasured = "PLN",
                CurrencyMatched = "USD",
                StartDate = new DateTime(2010, 1, 1),
                EndDate = new DateTime(2010, 1, 3)
            }).ToList();

            currencyValues.Should().NotBeEmpty();
            currencyValues.ForEach(cv =>
            {
                cv.Value = CurrencyResult.CurrencyValue;
                cv.DailyDataOfCurrency = CurrencyResult.DailyDataOfCurrency;
                cv.Currency.CurrencyBeingMeasured = CurrencyResult.CurrencyBeingMeasured;
                cv.Currency.CurrencyMatched = CurrencyResult.CurrencyMatched;
            });
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }
    }
}
