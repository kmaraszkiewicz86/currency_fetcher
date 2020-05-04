using System;

namespace CurrencyFetcher.Core.Models
{
    public class CurrencyModel
    {
        public string Currency { get; set; }

        public string CurrencyToMatch { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
