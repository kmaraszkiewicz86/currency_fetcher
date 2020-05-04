using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    public interface ICurrencyGetterService
    {
        public Task<IEnumerable<CurrencyResult>> GetAllAsync();
    }
}
