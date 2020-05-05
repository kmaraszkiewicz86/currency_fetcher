using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Interfaces;

namespace CurrencyFetcher.Core.Services.Implementations
{
    /// <summary>
    /// Handles xmlBody into <see cref="IEnumerable{CurrencyResult}"/> list
    /// </summary>
    public class XmlReader: IXmlReader
    {
        /// <summary>
        /// Get currency information from web service result string
        /// </summary>
        /// <param name="model"><seealso cref="CurrencyModel"/></param>
        /// <param name="xmlBody">XmlBody fetched from web api</param>
        /// <returns></returns>
        public IEnumerable<CurrencyResultResponse> GetCurrencyResults(CurrencyModel model, string xmlBody)
        {
            var currencyResults = new List<CurrencyResultResponse>();

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
                XmlNode currencyValueXmlElement = currencyValueXmlElements[index];
                XmlNode currencyDateXmlElement = currencyDateXmlElements[index];

                //if generic:ObsValue == NaN skip that value
                if (decimal.TryParse(currencyValueXmlElement.Attributes[0].Value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var currencyValue) &&
                    DateTime.TryParse(currencyDateXmlElement.Attributes[0].Value, out var dailyDataOfCurrency))
                {
                    currencyResults.Add(new CurrencyResultResponse
                    {
                        CurrencyBeingMeasured = model.CurrencyBeingMeasured.ToUpper(),
                        CurrencyMatched = model.CurrencyMatched.ToUpper(),
                        CurrencyValue = currencyValue,
                        DailyDataOfCurrency = dailyDataOfCurrency
                    });
                }
            }

            return currencyResults;
        }
    }
}
