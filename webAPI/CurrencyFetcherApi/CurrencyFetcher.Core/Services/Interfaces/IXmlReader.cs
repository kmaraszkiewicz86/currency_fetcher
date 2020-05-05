using System;
using System.Collections.Generic;
using System.Text;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    /// <summary>
    /// Handles xmlBody into <see cref="IEnumerable{CurrencyResult}"/> list
    /// </summary>
    public interface IXmlReader
    {
        /// <summary>
        /// Get currency information from web service result string
        /// </summary>
        /// <param name="model"><seealso cref="CurrencyModel"/></param>
        /// <param name="xmlBody">XmlBody fetched from web api</param>
        /// <returns></returns>
        IEnumerable<CurrencyResult> GetCurrencyResults(CurrencyModel model, string xmlBody);
    }
}
