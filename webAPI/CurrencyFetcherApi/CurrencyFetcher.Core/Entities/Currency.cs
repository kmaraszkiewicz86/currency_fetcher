using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Entities
{
    /// <summary>
    /// Reference to Currencies table into database
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The currency being measured (e.g.: US dollar - code USD),
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string CurrencyBeingMeasured { get; set; }

        /// <summary>
        /// the currency against which a currency is being measured (e.g.: Euro - code EUR),
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string CurrencyMatched { get; set; }

        /// <summary>
        /// List of referenced rows from <see cref="CurrencyValue"/> database table
        /// </summary>
        public ICollection<CurrencyValue> CurrencyValues { get; set; }
    }
}
