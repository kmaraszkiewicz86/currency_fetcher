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
            var expectedResults = new List<CurrencyResultResponse>
            {
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "EUR",
                    CurrencyValue = 4.1535m,
                    DailyDataOfCurrency = new DateTime(2008,12,31)
                },
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "NOK",
                    CurrencyMatched = "EUR",
                    CurrencyValue = 9.75m,
                    DailyDataOfCurrency = new DateTime(2008,12,31)
                }
            };

            try
            {
                _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionRequest
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

                List<CurrencyResultResponse> currencyResults = (await _currencyAutomatedTestHelper.GetCurrencyResultsAsync()).ToList();

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
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionRequest
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

            ModalErrorModel modalErrorModel =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<ModalErrorModel>(HttpStatusCode
                    .BadRequest);

            await ValidateModalErrorModelAsync("StartDate",
                "The field StartDate must be between 01.01.1980 00:00:00 and 03.04.2999 00:00:00.");
        }

        /// <summary>
        /// Check no currency codes error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendNoCurrencyCodes_ThenCurrencyErrorModelReturn()
        {
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionRequest
            {
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2008, 1, 1)
            };

            await ValidateModalErrorModelAsync("CurrencyCodes", "The CurrencyCodes field is required.");
        }

        /// <summary>
        /// Check start date in future error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendStartDateInFuture_ThenCurrencyErrorModelReturn()
        {
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionRequest
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

            await ValidateCurrencyErrorResponseAsync(HttpStatusCode
                .NotFound, "The startDate could not be after current date");
        }

        /// <summary>
        /// Check end date in future error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendEndDateInFuture_ThenCurrencyErrorModelReturn()
        {

            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionRequest
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

            await ValidateCurrencyErrorResponseAsync(HttpStatusCode
                .NotFound, "The endDate could not be after current date");
        }

        /// <summary>
        /// Check end date before start date error validation
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Test]
        public async Task WhenSendEndDateBeforeStartDate_ThenCurrencyErrorModelReturn()
        {
            _currencyAutomatedTestHelper.CurrencyCollectionModel = new CurrencyCollectionRequest
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

            await ValidateCurrencyErrorResponseAsync(HttpStatusCode
                .BadRequest, "The endDate could not be before startDate date");
        }

        /// <summary>
        /// Validate currency error response
        /// </summary>
        /// <param name="expectedErrorMessage">The expected error message</param>
        /// <returns></returns>
        private async Task ValidateCurrencyErrorResponseAsync(HttpStatusCode code, string expectedErrorMessage)
        {
            CurrencyErrorResponse currencyErrorResponse =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<CurrencyErrorResponse>(code);

            currencyErrorResponse.ErrorMessages.Count.Should().Be(1);
            currencyErrorResponse.ErrorMessages[0].Should().Be(expectedErrorMessage);
        }

        /// <summary>
        /// Validate modal error model
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expectedErrorMessage">The expected error message</param>
        /// <returns></returns>
        private async Task ValidateModalErrorModelAsync(string type, string expectedErrorMessage)
        {
            ModalErrorModel modalErrorModel =
                await _currencyAutomatedTestHelper.GetErrorModelsResults<ModalErrorModel>(HttpStatusCode
                    .BadRequest);

            string[] errors = modalErrorModel.Errors[type];
            errors.Length.Should().Be(1);
            errors.First().Should()
                .Be(expectedErrorMessage);
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