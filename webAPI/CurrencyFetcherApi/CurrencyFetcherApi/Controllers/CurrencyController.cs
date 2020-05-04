using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyFetcherApi.Controllers
{
    [ApiController]
    [Route("api/Currency")]
    public class CurrencyController : BaseController<CurrencyController>
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService, ILogger<CurrencyController> logger)
            : base(logger)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get([FromBody] CurrencyCollectionModel collectionModel)
        {
            return await OnActionAsync(async () =>
            {
                _logger.LogInformation($"Executing CurrencyController.Get with values {collectionModel}");
                return Ok(await _currencyService.GetCurrencyResults(collectionModel));
            });

        }
    }
}
