using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcherApi.IntegrationTests.Core;
using CurrencyFetcherApi.IntegrationTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace CurrencyFetcherApi.IntegrationTests.Tests
{
    /// <summary>
    /// Performance tests for fetching currency data and use cached functionality
    /// </summary>
    [TestFixture]
    public class CurrencyPerformanceTests
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
        public async Task WhenGetLargeAmountOfData_ThenCheckDifferenceNoCacheAndCachedData()
        {
            var resultStringBuilder = new StringBuilder();

            var currencyCollectionModel = new CurrencyCollectionModel
            {
                CurrencyCodes = new Dictionary<string, string>
                {
                    {"PLN", "EUR"},
                },
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2019, 1, 1)
            };

            try
            {
                _currencyAutomatedTestHelper.CurrencyCollectionModel = currencyCollectionModel;

                Stopwatch stopwatch = Stopwatch.StartNew();
                var currencyResults = await _currencyAutomatedTestHelper.GetCurrencyResultsAsync();
                stopwatch.Stop();

                resultStringBuilder.AppendLine($"Elapsed time => {stopwatch.ElapsedMilliseconds} with no cached data with {currencyResults.ToList().Count()}");

                var expectedCount = currencyResults.Count();

                stopwatch = Stopwatch.StartNew();
                currencyResults = await _currencyAutomatedTestHelper.GetCurrencyResultsAsync();
                stopwatch.Stop();

                resultStringBuilder.AppendLine($"Elapsed time => {stopwatch.ElapsedMilliseconds} with cached data with {currencyResults.ToList().Count()}");

                currencyResults.Count().Should().Be(expectedCount);

                Console.WriteLine(resultStringBuilder.ToString());
            }
            finally
            {
                await _currencyAutomatedTestHelper.ResetDatabase();
            }
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