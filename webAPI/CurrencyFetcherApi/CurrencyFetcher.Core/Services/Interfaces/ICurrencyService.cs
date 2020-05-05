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
        /// <param name="collectionModel"><see cref="CurrencyCollectionRequest"/></param>
        /// <returns>returns collection of <see cref="CurrencyResultResponse"/> items</returns>
        Task<IEnumerable<CurrencyResultResponse>> GetCurrencyResultsAsync(CurrencyCollectionRequest collectionModel);
    }
}
