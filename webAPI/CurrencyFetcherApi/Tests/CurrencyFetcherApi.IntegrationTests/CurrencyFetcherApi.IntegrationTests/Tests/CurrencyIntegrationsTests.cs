using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcherApi.IntegrationTests.Core;
using CurrencyFetcherApi.IntegrationTests.Helpers;
using CurrencyFetcherApi.IntegrationTests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace CurrencyFetcherApi.IntegrationTests.Tests
{
    /// <summary>
    /// Performance integrations tests
    /// </summary>
    [TestFixture]
    public class CurrencyIntegrationsTests
    {
        /// <summary>
        /// <see cref="ApiWebApplicationFactory"/>
        /// </summary>
        private ApiWebApplicationFactory _factory;

        /// <summary>
        /// <see cref="HttpClient"/>
        /// </summary>
        private HttpClient _client;

        /// <summary>
        /// <see cref="CurrencyAutomatedTestHelper"/>
        /// </summary>
        private CurrencyAutomatedTestHelper _currencyAutomatedTestHelper;

        /// <summary>
        /// Initializes required services for testing
        /// </summary>
        [OneTimeSetUp]
        public void GivenInitClient()
        {
            _factory = new ApiWebApplicationFactory();
            _client = _factory.CreateClient();
            _currencyAutomatedTestHelper = new CurrencyAutomatedTestHelper(_client);
            _currencyAutomatedTestHelper.GenerateTokenString();
        }

        /// <summary>
        /// Checks time elapsed between non cached and cache fetch data
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenGetMultipleCurrency_ThenValidDataIsReturn()
        {
            var expectedResults = new List<CurrencyResult>
            {
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "EUR",
                    CurrencyValue = 4.1535m,
                    DailyDataOfCurrency = new DateTime(2008,12,31)
                },
                new CurrencyResult
                {
                    CurrencyBeingMeasured = "NOK",
                    CurrencyMatched = "EUR",
                    CurrencyValue = 9.75m,
                    DailyDataOfCurrency = new DateTime(2008,12,31)
                }
            };

            try
            {
                _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionModel
                {
                    CurrencyCodes = new Dictionary<string, string>
                    {
                        {"PLN", "EUR"},
                        {"EUR", "USD"},
                        {"NOK", "EUR"},
                        {"", "EUR"},
                        {"CAD", ""},
                    },
                    StartDate = new DateTime(2009, 1, 1),
                    EndDate = new DateTime(2009, 1, 1)
                };

                List<CurrencyResult> currencyResults = (await _currencyAutomatedTestHelper.GetCurrencyResultsAsync()).ToList();

                currencyResults.Count.Should().Be(2);
                currencyResults.Should().BeEquivalentTo(expectedResults);
            }
            finally
            {
                await _currencyAutomatedTestHelper.ResetDatabase();
            }
        }

        /// <summary>
        /// Check no start error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendNoStartDate_ThenCurrencyErrorModelReturn()
        {
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionModel
            {
                CurrencyCodes = new Dictionary<string, string>
                    {
                        {"PLN", "EUR"},
                        {"EUR", "USD"},
                        {"NOK", "EUR"},
                        {"", "EUR"},
                        {"CAD", ""},
                    },
                EndDate = new DateTime(2009, 1, 1)
            };

            ErrorModelCollection currencyResults =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<ErrorModelCollection>(HttpStatusCode
                    .BadRequest);

            string[] errors = currencyResults.Errors["StartDate"];
            errors.Length.Should().Be(1);
            errors.First().Should()
                .Be("The field StartDate must be between 01.01.1980 00:00:00 and 03.04.2999 00:00:00.");
        }

        /// <summary>
        /// Check no currency codes error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendNoCurrencyCodes_ThenCurrencyErrorModelReturn()
        {
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionModel
            {
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2008, 1, 1)
            };

            ErrorModelCollection currencyResults =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<ErrorModelCollection>(HttpStatusCode
                    .BadRequest);

            string[] errors = currencyResults.Errors["CurrencyCodes"];
            errors.Length.Should().Be(1);
            errors.First().Should()
                .Be("The CurrencyCodes field is required.");
        }

        /// <summary>
        /// Check start date in future error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendStartDateInFuture_ThenCurrencyErrorModelReturn()
        {
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionModel
            {
                CurrencyCodes = new Dictionary<string, string>
                    {
                        {"PLN", "EUR"},
                        {"EUR", "USD"},
                        {"NOK", "EUR"},
                        {"", "EUR"},
                        {"CAD", ""},
                    },
                StartDate = new DateTime(2099, 1, 1),
                EndDate = new DateTime(2009, 1, 1)
            };

            CurrencyErrorModel currencyErrorModel =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<CurrencyErrorModel>(HttpStatusCode
                    .NotFound);

            currencyErrorModel.ErrorMessages.Count.Should().Be(1);
            currencyErrorModel.ErrorMessages[0].Should().Be("The startDate could not be after current date");
        }

        /// <summary>
        /// Check end date in future error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendEndDateInFuture_ThenCurrencyErrorModelReturn()
        {

            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionModel
            {
                CurrencyCodes = new Dictionary<string, string>
                    {
                        {"PLN", "EUR"},
                        {"EUR", "USD"},
                        {"NOK", "EUR"},
                        {"", "EUR"},
                        {"CAD", ""},
                    },
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2099, 1, 1)
            };

            CurrencyErrorModel currencyErrorModel =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<CurrencyErrorModel>(HttpStatusCode
                    .NotFound);

            currencyErrorModel.ErrorMessages.Count.Should().Be(1);
            currencyErrorModel.ErrorMessages[0].Should().Be("The endDate could not be after current date");

        }

        /// <summary>
        /// Check end date before start date error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendEndDateBeforeStartDate_ThenCurrencyErrorModelReturn()
        {
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionModel
            {
                CurrencyCodes = new Dictionary<string, string>
                    {
                        {"PLN", "EUR"},
                        {"EUR", "USD"},
                        {"NOK", "EUR"},
                        {"", "EUR"},
                        {"CAD", ""},
                    },
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2008, 1, 1)
            };

            CurrencyErrorModel currencyErrorModel =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<CurrencyErrorModel>(HttpStatusCode
                    .BadRequest);

            currencyErrorModel.ErrorMessages.Count.Should().Be(1);
            currencyErrorModel.ErrorMessages[0].Should().Be("The endDate could not be before startDate date");
        }


        /// <summary>
        /// Disposing services
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
            _currencyAutomatedTestHelper = null;
        }
    }
}