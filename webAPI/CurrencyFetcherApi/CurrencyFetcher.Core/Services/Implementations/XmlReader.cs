using System;
using System.Collections.Generic;
using System.Globalization;
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
                    CurrencyBeingMeasured = model.CurrencyBeingMeasured,
                    CurrencyMatched = model.CurrencyMatched,
                    CurrencyValue = decimal.Parse(currencyValueXmlElement.Attributes[0].Value, CultureInfo.InvariantCulture),
                    DailyDataOfCurrency = DateTime.Parse(currencyDateXmlElement.Attributes[0].Value)
                });
            }

            return currencyResults;
        }
    }
}
