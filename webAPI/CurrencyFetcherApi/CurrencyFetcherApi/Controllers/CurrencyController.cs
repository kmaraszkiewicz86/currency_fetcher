using System.Threading.Tasks;
using CurrencyFetcher.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyFetcherApi.Controllers
{
    [ApiController]
    [Route("api/Currency")]
    public class CurrencyController : BaseController
    {
        private ICurrencyGetterService _currencyGetterService;

        public CurrencyController(ICurrencyGetterService currencyGetterService)
        {
            _currencyGetterService = currencyGetterService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return await OnActionAsync(async () => 
                Ok(await _currencyGetterService.GetAllAsync()));
        }
    }
}
