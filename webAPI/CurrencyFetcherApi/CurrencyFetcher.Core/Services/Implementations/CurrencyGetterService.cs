using System.Net.Http;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyFetcher.Core.Services.Implementations
{
    /// <summary>
    /// Fetches currency data from foreign web service
    /// Web service site => https://sdw-wsrest.ecb.europa.eu
    /// </summary>
    public class CurrencyGetterService: ICurrencyGetterService
    {
        /// <summary>
        /// Web service url
        /// </summary>
        private const string WebServiceUrl = "https://sdw-wsrest.ecb.europa.eu";

        /// <summary>
        /// <seealso cref="ILogger{CurrencyGetterService}"/>
        /// </summary>
        private ILogger<CurrencyGetterService> _logger;

        public CurrencyGetterService(ILogger<CurrencyGetterService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Fetches currency data from foreign web service
        /// </summary>
        /// <param name="model"><see cref="CurrencyModel"/></param>
        /// <returns>returns xml body from web service</returns>
        public async Task<string> FetchDataAsync(CurrencyModel model)
        {
            using (var client = new HttpClient())
            {
                var startDate = model.StartDate;
                var endDate = model.EndDate ?? model.StartDate;

                var url =
                    $"{WebServiceUrl}/service/data/EXR/D.{model.CurrencyBeingMeasured?.ToUpper() ?? ""}.{model.CurrencyMatched?.ToUpper() ?? ""}.SP00.A?startPeriod={startDate:yyyy-MM-dd}&endPeriod={endDate:yyyy-MM-dd}&details=serieskeysonly";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();

                }
                catch (HttpRequestException e)
                {
                    _logger.LogError($"Error occurs trying to open {url} with message: {e.Message}");
                    return string.Empty;
                }
            }
        }
    }
}
