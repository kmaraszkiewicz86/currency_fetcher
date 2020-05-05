using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Entities
{
    /// <summary>
    /// Reference to CurrencyValues table into database
    /// </summary>
    public class CurrencyValue
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Value of currency
        /// </summary>
        [Required]
        public decimal Value { get; set; }

        /// <summary>
        /// Date for <see cref="CurrencyValue"/> value
        /// </summary>
        [Required]
        public DateTime DailyDataOfCurrency { get; set; }

        /// <summary>
        /// The <see cref="Currency"/> identifier
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// The reference to <see cref="Currency"/> database table
        /// </summary>
        public Currency Currency { get; set; }
    }
}
