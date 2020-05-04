using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyFetcher.Core.Models.Responses
{
    public class CurrencyResult
    {
        public string CurrencyBeingMeasured { get; set; }

        public string CurrencyMatched { get; set; }

        public decimal CurrencyValue { get; set; }

        public DateTime DailyDataOfCurrency { get; set; }

    }
}
