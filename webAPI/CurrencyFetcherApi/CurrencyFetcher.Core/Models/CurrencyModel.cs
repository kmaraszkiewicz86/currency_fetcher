using System;

namespace CurrencyFetcher.Core.Models
{
    /// <summary>
    /// Currency data for one instance of <see cref="CurrencyCollectionModel"/> model
    /// </summary>
    public class CurrencyModel
    {
        /// <summary>
        /// the currency being measured (e.g.: US dollar - code USD),
        /// </summary>
        public string CurrencyBeingMeasured { get; set; }

        /// <summary>
        /// the currency against which a currency is being measured (e.g.: Euro - code EUR),
        /// </summary>
        public string CurrencyMatched { get; set; }

        /// <summary>
        /// Start date for start currency date that fetch from web services
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date for end currency date that fetch from web services
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
