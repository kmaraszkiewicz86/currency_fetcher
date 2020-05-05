using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyFetcher.Core.Entities
{
    /// <summary>
    /// Reference to Logs table into database
    /// </summary>
    public class Log
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The application section eg. HomeController
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Application { get; set; }

        /// <summary>
        /// Time that log was created
        /// </summary>
        [Required]
        public DateTime Logged { get; set; }

        /// <summary>
        /// Log level
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Level { get; set; }

        /// <summary>
        /// The messege of log
        /// </summary>
        [Required]
        public string Message { get; set; }

        [MaxLength(250)]
        public string Logger { get; set; }

        public string Callsite { get; set; }

        public string Exception { get; set; }
    }
}
