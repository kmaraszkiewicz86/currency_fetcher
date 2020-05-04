using System;

namespace CurrencyFetcher.Core.Models
{
    public class CurrencyModel
    {
        public string CurrencyBeingMeasured { get; set; }

        public string CurrencyMatched { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
