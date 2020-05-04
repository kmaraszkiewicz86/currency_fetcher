using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Entities
{
    public class CurrencyValue
    {
        public int Id { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime DailyDataOfCurrency { get; set; }

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }
    }
}
