using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcherApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyFetcherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseController
    {
        private readonly IUserService _userService;

        public TokenController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            return await OnActionAsync(async () =>
            {
                var user = await _userService.Authenticate(model.Username, model.Password);

                if (user == null)
                    throw new BadRequestException("Username or Password is invalid");

                return Ok(user);
            });
        }
    }
}