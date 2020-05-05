using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    /// <summary>
    /// Currency service (Facade) to split all services in one place
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Get currency information
        /// </summary>
        /// <param name="collectionModel"><see cref="CurrencyCollectionModel"/></param>
        /// <returns>returns collection of <see cref="CurrencyResult"/> items</returns>
        Task<IEnumerable<CurrencyResult>> GetCurrencyResults(CurrencyCollectionModel collectionModel);
    }
}
