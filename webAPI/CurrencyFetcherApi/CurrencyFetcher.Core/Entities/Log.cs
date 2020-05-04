using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Entities
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Application { get; set; }

        [Required]
        public DateTime Logged { get; set; }

        [Required]
        [MaxLength(50)]
        public string Level { get; set; }

        [Required]
        public string Message { get; set; }

        [MaxLength(250)]
        public string Logger { get; set; }

        public string Callsite { get; set; }

        public string Exception { get; set; }
    }
}
