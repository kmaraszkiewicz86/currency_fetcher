using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Services.Interfaces;
using CurrencyFetcherApi.Services;
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
        /// <see cref="ITokenService"/>
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// <see cref="ICurrencyService"/>
        /// </summary>
        private readonly ICurrencyService _currencyService;

        /// <summary>
        /// Create instance of class
        /// </summary>
        /// <param name="currencyService"><see cref="ICurrencyService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="tokenService"><see cref="ITokenService"/></param>
        public CurrencyController(ICurrencyService currencyService, ILogger<CurrencyController> logger, ITokenService tokenService)
            : base(logger)
        {
            _currencyService = currencyService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Get information about currency typed by user
        /// </summary>
        /// <param name="collectionModel"><see cref="CurrencyCollectionRequest"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] CurrencyCollectionRequest collectionModel)
        {
            return await OnActionAsync(async () =>
            {
                _tokenService.ValidateCurrentToken(collectionModel.ApiKey);

                _logger.LogInformation($"Executing CurrencyController.Get with values {collectionModel}");
                return Ok(await _currencyService.GetCurrencyResultsAsync(collectionModel));
            });
        }
    }
}