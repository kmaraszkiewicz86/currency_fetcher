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
        /// <see cref="IUserService"/>
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="userService"><see cref="IUserService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        public TokenController(IUserService userService, ILogger<TokenController> logger) 
            : base(logger)
        {
            _userService = userService;
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

                var user = await _userService.AuthenticateAsync(model.Username, model.Password);

                if (user == null)
                    throw new BadRequestException("Username or Password is invalid");

                return Ok(user);
            });
        }
    }
}