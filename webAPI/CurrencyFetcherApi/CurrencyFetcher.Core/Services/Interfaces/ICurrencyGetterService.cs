using System.Threading.Tasks;
using CurrencyFetcher.Core.Models;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    /// <summary>
    /// Fetches currency data from foreign web service
    /// Web service site => https://sdw-wsrest.ecb.europa.eu
    /// </summary>
    public interface ICurrencyGetterService
    {
        /// <summary>
        /// Fetches currency data from foreign web service
        /// </summary>
        /// <param name="model"><see cref="CurrencyModel"/></param>
        /// <returns>returns xml body from web service</returns>
        public Task<string> FetchDataAsync(CurrencyModel model);
    }
}
