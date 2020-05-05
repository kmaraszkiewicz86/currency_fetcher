using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Entities;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    /// <summary>
    /// Caches currency information in database
    /// </summary>
    public interface ICacheDatabase
    {
        /// <summary>
        /// Saves <see cref="CurrencyModel"/> into database
        /// </summary>
        /// <param name="result"><see cref="CurrencyModel"/></param>
        /// <returns></returns>
        Task SaveAsync(CurrencyResultResponse result);

        /// <summary>
        /// Get caches data from database
        /// </summary>
        /// <param name="model"><see cref="CurrencyModel"/></param>
        /// <returns><see cref="IEnumerable{CurrencyValue}"/></returns>
        IEnumerable<CurrencyValue> GetAsync(CurrencyModel model);
    }
}
