using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyFetcher.Core.Models.Responses
{
    public class CurrencyResult
    {
        public string CurrencyBeingMeasured { get; set; }

        public string CurrencyMatched { get; set; }

        public string CurrencyValue { get; set; }

        public string DailyDataOfCurrency { get; set; }

    }
}
