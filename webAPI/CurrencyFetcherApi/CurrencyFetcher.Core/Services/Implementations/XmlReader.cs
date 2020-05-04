using System.Collections.Generic;
using System.Xml;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Interfaces;

namespace CurrencyFetcher.Core.Services.Implementations
{
    public class XmlReader: IXmlReader
    {
        public IEnumerable<CurrencyResult> GetCurrencyResults(CurrencyModel model, string xmlBody)
        {
            var currencyResults = new List<CurrencyResult>();

            if (string.IsNullOrWhiteSpace(xmlBody))
            {
                return currencyResults;
            }

            var doc = new XmlDocument();
            doc.LoadXml(xmlBody);

            XmlNodeList currencyValueXmlElements = doc.GetElementsByTagName("generic:ObsValue");
            XmlNodeList currencyDateXmlElements = doc.GetElementsByTagName("generic:ObsDimension");

            for (var index = 0; index < currencyValueXmlElements.Count; index++)
            {
                var currencyValueXmlElement = currencyValueXmlElements[index];
                var currencyDateXmlElement = currencyDateXmlElements[index];

                currencyResults.Add(new CurrencyResult
                {
                    CurrencyBeingMeasured = model.Currency,
                    CurrencyMatched = model.CurrencyToMatch,
                    CurrencyValue = currencyValueXmlElement.Attributes[0].Value,
                    DailyDataOfCurrency = currencyDateXmlElement.Attributes[0].Value
                });
            }

            return currencyResults;
        }
    }
}
