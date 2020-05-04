using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyFetcherApi.Controllers
{
    [ApiController]
    [Route("api/Currency")]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get([FromBody] CurrencyCollectionModel collectionModel)
        {
            return await OnActionAsync(async () =>
                Ok(await _currencyService.GetCurrencyResults(collectionModel)));
        }
    }
}
