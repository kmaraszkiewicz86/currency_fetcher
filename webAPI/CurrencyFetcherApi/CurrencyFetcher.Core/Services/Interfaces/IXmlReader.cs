using System;
using System.Collections.Generic;
using System.Text;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    public interface IXmlReader
    {
        IEnumerable<CurrencyResult> GetCurrencyResults(CurrencyModel model, string xmlBody);
    }
}
