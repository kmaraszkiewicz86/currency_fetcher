using System;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyFetcherApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult OnAction(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (NotFoundException err)
            {
                return NotFound(new CurrencyErrorModel(err.Message));
            }
            catch (BadRequestException err)
            {
                return BadRequest(new CurrencyErrorModel(err.Message));
            }
            catch (Exception err)
            {
                return BadRequest(new CurrencyErrorModel(err.Message));
            }
        }

        protected async Task<IActionResult> OnActionAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (NotFoundException err)
            {
                return NotFound(new CurrencyErrorModel(err.Message));
            }
            catch (BadRequestException err)
            {
                return BadRequest(new CurrencyErrorModel(err.Message));
            }
            catch (Exception err)
            {
                return BadRequest(new CurrencyErrorModel(err.Message));
            }
        }
    }
}
