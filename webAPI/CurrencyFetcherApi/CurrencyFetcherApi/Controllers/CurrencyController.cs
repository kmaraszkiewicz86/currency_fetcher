using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyFetcherApi.Controllers
{
    /// <summary>
    /// Controller has method to fetch currency values
    /// </summary>
    [ApiController]
    [Route("api/Currency")]
    public class CurrencyController : BaseController<CurrencyController>
    {
        /// <summary>
        /// <see cref="ICurrencyService"/>
        /// </summary>
        private readonly ICurrencyService _currencyService;

        /// <summary>
        /// Create instance of class
        /// </summary>
        /// <param name="currencyService"><see cref="ICurrencyService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        public CurrencyController(ICurrencyService currencyService, ILogger<CurrencyController> logger)
            : base(logger)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// Get information about currency typed by user
        /// </summary>
        /// <param name="collectionModel"><see cref="CurrencyCollectionModel"/></param>
        /// <returns></returns>
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