using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Services.Interfaces;
using CurrencyFetcherApi.Services;
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
        /// <see cref="IUserService"/>
        /// </summary>
        private IUserService _userService;

        /// <summary>
        /// <see cref="ICurrencyService"/>
        /// </summary>
        private readonly ICurrencyService _currencyService;

        /// <summary>
        /// Create instance of class
        /// </summary>
        /// <param name="currencyService"><see cref="ICurrencyService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="userService"><see cref="IUserService"/></param>
        public CurrencyController(ICurrencyService currencyService, ILogger<CurrencyController> logger, IUserService userService)
            : base(logger)
        {
            _currencyService = currencyService;
            _userService = userService;
        }

        /// <summary>
        /// Get information about currency typed by user
        /// </summary>
        /// <param name="collectionModel"><see cref="CurrencyCollectionModel"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] CurrencyCollectionModel collectionModel)
        {
            return await OnActionAsync(async () =>
            {
                _userService.ValidateCurrentToken(collectionModel.ApiKey);

                _logger.LogInformation($"Executing CurrencyController.Get with values {collectionModel}");
                return Ok(await _currencyService.GetCurrencyResults(collectionModel));
            });
        }
    }
}