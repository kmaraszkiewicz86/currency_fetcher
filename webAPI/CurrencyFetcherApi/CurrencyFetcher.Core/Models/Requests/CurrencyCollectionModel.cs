using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public override string ToString()
        {
            return $"CurrencyCodes => {string.Join(',', CurrencyCodes.Select(c => $"{c.Key}: {c.Value}"))}" +
                   $"StartDate => {StartDate:yyyy-MM-dd}" +
                   $"EndDate => {(EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "empty")}";
        }
    }
}
