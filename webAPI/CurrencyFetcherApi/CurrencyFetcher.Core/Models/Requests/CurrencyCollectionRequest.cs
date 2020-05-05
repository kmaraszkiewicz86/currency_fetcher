using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CurrencyFetcher.Core.Models.Requests
{
    /// <summary>
    /// Data about whats range of currency information will be fetched
    /// </summary>
    public class CurrencyCollectionRequest
    {
        /// <summary>
        /// the currency being measured (e.g.: US dollar - code USD) as key value
        /// and
        /// the currency against which a currency is being measured (e.g.: Euro - code EUR) as value
        ///
        /// <example>
        ///     {
        ///         "PLN": "EUR",
        ///         "USD": "EUR",
        ///         "NOK": "EUR"
        ///     }
        /// </example>
        /// </summary>
        [Required]
        public Dictionary<string, string> CurrencyCodes { get; set; }

        /// <summary>
        /// Start date from where currency information will be fetched
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1/1/1980", "3/4/2999")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date to where currency information will be fetched
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The JWT token
        /// </summary>
        [Required]
        public string ApiKey { get; set; }

        /// <summary>
        /// Generate string from class properties
        /// </summary>
        /// <returns>The string from class properties</returns>
        public override string ToString()
        {
            return $"CurrencyCodes => {string.Join(',', CurrencyCodes.Select(c => $"{c.Key}: {c.Value}"))}" +
                   $"StartDate => {StartDate:yyyy-MM-dd}" +
                   $"EndDate => {(EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "empty")}";
        }
    }
}
