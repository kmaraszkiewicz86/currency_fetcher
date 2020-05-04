using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Models.Requests
{
    public class CurrencyCollectionModel
    {
        [Required]
        public Dictionary<string, string> CurrencyCodes { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1980", "3/4/2999")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
