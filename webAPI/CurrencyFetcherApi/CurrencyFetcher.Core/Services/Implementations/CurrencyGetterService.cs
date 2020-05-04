using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Interfaces;

namespace CurrencyFetcher.Core.Services.Implementations
{
    public class CurrencyGetterService: ICurrencyGetterService
    {
        public async Task<IEnumerable<CurrencyResult>> GetAllAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var responseBodyInString = string.Empty;

                try
                {
                    HttpResponseMessage response = await client.GetAsync(
                        "https://sdw-wsrest.ecb.europa.eu/service/data/EXR/D.PLN.EUR.SP00.A?startPeriod=2009-05-01&endPeriod=2009-05-31&details=serieskeysonly");
                    
                    response.EnsureSuccessStatusCode();

                    responseBodyInString = await response.Content.ReadAsStringAsync();

                }
                catch (HttpRequestException e)
                {
                    throw new BadRequestException($"Message :{e.Message}");
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseBodyInString);

                var items = doc.GetElementsByTagName("message:GenericData")[0]
                    .OwnerDocument.GetElementsByTagName("message:DataSet")[0]
                    .OwnerDocument.GetElementsByTagName("generic:Obs");

                var currencyResults = new List<CurrencyResult>();

                foreach (XmlElement item in items)
                {
                    currencyResults.Add(new CurrencyResult
                    {
                        CurrencyBeingMeasured = "PLN",
                        CurrencyMatched = "EUR",
                        CurrencyValue = item.OwnerDocument.GetElementsByTagName("generic:ObsValue")[0].Attributes[0]
                            .Value,
                        DailyDataOfCurrency = item.OwnerDocument.GetElementsByTagName("generic:ObsDimension")[0]
                            .Attributes[0].Value
                    });
                }

                return currencyResults;
            }
        }
    }
}
