using System.Threading.Tasks;
using CurrencyFetcher.Core.Models;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    public interface ICurrencyGetterService
    {
        public Task<string> FetchDataAsync(CurrencyModel model);
    }
}
