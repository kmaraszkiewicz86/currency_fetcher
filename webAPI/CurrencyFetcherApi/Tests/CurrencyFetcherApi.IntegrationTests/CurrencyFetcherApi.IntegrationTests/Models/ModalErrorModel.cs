using System.Collections.Generic;

namespace CurrencyFetcherApi.IntegrationTests.Models
{
    /// <summary>
    /// Error Modal State response data model
    /// </summary>
    public class ModalErrorModel
    {
        /// <summary>
        /// Collection of errors
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
