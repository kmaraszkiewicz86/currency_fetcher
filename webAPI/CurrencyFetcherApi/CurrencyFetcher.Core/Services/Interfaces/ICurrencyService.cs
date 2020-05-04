using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyResult>> GetCurrencyResults(CurrencyCollectionModel collectionModel);
    }
}
