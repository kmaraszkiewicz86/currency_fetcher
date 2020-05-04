using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Entities;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    public interface ICacheDatabase
    {
        Task SaveAsync(CurrencyResult result);

        IEnumerable<CurrencyValue> GetAsync(CurrencyModel model);
    }
}
