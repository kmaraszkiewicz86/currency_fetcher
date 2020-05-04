using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyFetcherApi.Controllers
{
    [ApiController]
    [Route("api/Currency")]
    public class CurrencyController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "USD", "PLN", "EUR"
        };

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(Summaries);
        }
    }
}
