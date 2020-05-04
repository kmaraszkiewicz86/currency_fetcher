using System.Net.Http;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Services.Interfaces;

namespace CurrencyFetcher.Core.Services.Implementations
{
    public class CurrencyGetterService: ICurrencyGetterService
    {
        private const string ApiHostUrl = "https://sdw-wsrest.ecb.europa.eu";

        public async Task<string> FetchData(CurrencyModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                var responseBodyInString = string.Empty;

                try
                {
                    var startDate = model.StartDate;
                    var endDate = model.EndDate.HasValue ? model.EndDate : model.StartDate; 

                    HttpResponseMessage response = await client.GetAsync(
                        $"{ApiHostUrl}/service/data/EXR/D.{model.CurrencyBeingMeasured}.{model.CurrencyMatched}.SP00.A?startPeriod={startDate:yyyy-MM-dd}&endPeriod={endDate:yyyy-MM-dd}&details=serieskeysonly");
                    
                    response.EnsureSuccessStatusCode();

                    responseBodyInString = await response.Content.ReadAsStringAsync();

                }
                catch (HttpRequestException e)
                {
                    throw new BadRequestException($"Message :{e.Message}");
                }

                return responseBodyInString;
            }
        }
    }
}
