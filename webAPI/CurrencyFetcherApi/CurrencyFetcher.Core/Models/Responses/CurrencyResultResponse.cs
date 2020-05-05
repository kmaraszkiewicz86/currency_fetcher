using System;

namespace CurrencyFetcher.Core.Models.Responses
{
    /// <summary>
    /// The currency result fetched from web service
    /// </summary>
    public class CurrencyResultResponse
    {
        /// <summary>
        /// the currency being measured (e.g.: US dollar - code USD)
        /// </summary>
        public string CurrencyBeingMeasured { get; set; }

        /// <summary>
        /// the currency against which a currency is being measured (e.g.: Euro - code EUR),
        /// </summary>
        public string CurrencyMatched { get; set; }

        /// <summary>
        /// The currency value
        /// </summary>
        public decimal CurrencyValue { get; set; }

        /// <summary>
        /// The date of currency value
        /// </summary>
        public DateTime DailyDataOfCurrency { get; set; }

    }
}
