using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcherApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyFetcherApi.Controllers
{
    /// <summary>
    /// Controller has actions to authenticate user
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseController<TokenController>
    {
        /// <summary>
        /// <see cref="ITokenService"/>
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="tokenService"><see cref="ITokenService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        public TokenController(ITokenService tokenService, ILogger<TokenController> logger) 
            : base(logger)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authenticates user and get token string
        /// </summary>
        /// <param name="model"><see cref="TokenAuthRequest"/></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] TokenAuthRequest model)
        {
            return await OnActionAsync(async () =>
            {
                _logger.LogInformation($"Executing TokenController.Login with values {model}");

                var user = await _tokenService.AuthenticateAsync(model.Username, model.Password);

                if (user == null)
                    throw new BadRequestException("Username or Password is invalid");

                return Ok(user);
            });
        }
    }
}