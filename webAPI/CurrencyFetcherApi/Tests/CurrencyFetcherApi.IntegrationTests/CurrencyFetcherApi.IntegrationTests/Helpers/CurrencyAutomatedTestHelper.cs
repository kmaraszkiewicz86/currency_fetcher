using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Core;
using CurrencyFetcher.Core.Helpers;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CurrencyFetcherApi.IntegrationTests.Helpers
{
    /// <summary>
    /// Helper flow for testing Currency controller
    /// </summary>
    public class CurrencyAutomatedTestHelper
    {
        /// <summary>
        /// Data model for searching currency information
        /// </summary>
        public CurrencyCollectionRequest CurrencyCollectionModel { get; set; }

        /// <summary>
        /// <see cref="HttpClient"/>
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Storage JWT token string
        /// </summary>
        private TokenResponse _tokenModel;

        private HolidayChecker _holidayChecker;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="client"></param>
        public CurrencyAutomatedTestHelper(HttpClient client)
        {
            _client = client;
            _holidayChecker = new HolidayChecker();
        }

        /// <summary>
        /// Generate and save in <see cref="_tokenModel"/> JWT token string
        /// </summary>
        public void GenerateTokenString()
        {
            var tokenAuthRequest = new StringContent(JsonConvert.SerializeObject(new TokenAuthRequest
            {
                Username = "currency",
                Password = "Currency123)(*"
            }), Encoding.Default, "application/json");

            var tokenModelResponse = _client.PostAsync("/api/Token", tokenAuthRequest).GetAwaiter().GetResult();

            tokenModelResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            _tokenModel = JsonConvert.DeserializeObject<TokenResponse>(tokenModelResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        /// <summary>
        /// Call to /api/Currency and get currency data
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CurrencyResultResponse>> GetCurrencyResultsAsync()
        {
            HttpResponseMessage currencyResultsResponse = await PostCurrencyRequestAsync();

            currencyResultsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            return JsonConvert.DeserializeObject<IEnumerable<CurrencyResultResponse>>(await currencyResultsResponse.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Check currency endpoint validation
        /// </summary>
        /// <returns></returns>
        public async Task<TModel> GetErrorModelsResults<TModel>(HttpStatusCode code)
        {
            HttpResponseMessage currencyResultsResponse = await PostCurrencyRequestAsync();

            currencyResultsResponse.StatusCode.Should().Be(code);

            var result = await currencyResultsResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TModel>(result);
        }

        /// <summary>
        /// Reset database
        /// </summary>
        /// <returns></returns>
        public async Task ResetDatabase()
        {
            var builder = new DbContextOptionsBuilder<CurrencyDbContext>();
            builder.UseSqlServer(ConnectionStringHelper.GetDefaultConfigurationBuild().GetDefaultConnectionString());

            using (var context = new CurrencyDbContext(builder.Options))
            {
                var startDate = _holidayChecker.ReturnDateBeforeDayOff(CurrencyCollectionModel.StartDate);

                foreach (var currencyCode in CurrencyCollectionModel.CurrencyCodes)
                {
                    if (string.IsNullOrWhiteSpace(currencyCode.Key) || string.IsNullOrWhiteSpace(currencyCode.Value))
                    {
                        continue;
                    }

                    var itemsToRemove = context.CurrencyValues.Include(c => c.Currency).Where(
                        c => c.Currency.CurrencyBeingMeasured.ToLower() == currencyCode.Key.ToLower() &&
                             c.Currency.CurrencyMatched.ToLower() == currencyCode.Value.ToLower() &&
                             (c.DailyDataOfCurrency >= startDate &&
                              c.DailyDataOfCurrency <= CurrencyCollectionModel.EndDate));

                    context.CurrencyValues.RemoveRange(itemsToRemove);

                    await context.SaveChangesAsync();
                }
            }
        }


        /// <summary>
        /// Post request to /api/Currency endpoint
        /// </summary>
        /// <returns><see cref="Task{HttpResponseMessage}"/></returns>
        private async Task<HttpResponseMessage> PostCurrencyRequestAsync()
        {
            CurrencyCollectionModel.ApiKey = _tokenModel.Token;

            var currencyCollectionModelRequest =
                new StringContent(JsonConvert.SerializeObject(CurrencyCollectionModel), Encoding.Default,
                    "application/json");

            return await _client.PostAsync("/api/Currency", currencyCollectionModelRequest);
        }
    }
}