using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcherApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyFetcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseController<TokenController>
    {
        private readonly IUserService _userService;

        public TokenController(IUserService userService, ILogger<TokenController> logger) 
            : base(logger)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            return await OnActionAsync(async () =>
            {
                _logger.LogInformation($"Executing TokenController.Login with values {model}");

                var user = await _userService.Authenticate(model.Username, model.Password);

                if (user == null)
                    throw new BadRequestException("Username or Password is invalid");

                return Ok(user);
            });
        }
    }
}