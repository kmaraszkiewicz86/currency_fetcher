using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CurrencyFetcher.Core.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(4)]
        public string CurrencyBeingMeasured { get; set; }

        [Required]
        [MaxLength(4)]
        public string CurrencyMatched { get; set; }

        public ICollection<CurrencyValue> CurrencyValues { get; set; }
    }
}
