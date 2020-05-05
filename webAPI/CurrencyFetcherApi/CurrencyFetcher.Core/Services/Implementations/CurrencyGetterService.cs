using System.Net.Http;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Services.Interfaces;

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
        /// Fetches currency data from foreign web service
        /// </summary>
        /// <param name="model"><see cref="CurrencyModel"/></param>
        /// <returns>returns xml body from web service</returns>
        public async Task<string> FetchDataAsync(CurrencyModel model)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var startDate = model.StartDate;
                    var endDate = model.EndDate ?? model.StartDate; 

                    HttpResponseMessage response = await client.GetAsync(
                        $"{WebServiceUrl}/service/data/EXR/D.{model.CurrencyBeingMeasured}.{model.CurrencyMatched}.SP00.A?startPeriod={startDate:yyyy-MM-dd}&endPeriod={endDate:yyyy-MM-dd}&details=serieskeysonly");
                    
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();

                }
                catch (HttpRequestException e)
                {
                    throw new BadRequestException($"Message: {e.Message}");
                }
            }
        }
    }
}
