using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CurrencyFetcherApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected async Task<IActionResult> OnActionAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ValidateModelState());
                }

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

        private List<string> ValidateModelState()
        {
            var errorMessages = new List<string>();

            foreach (ModelErrorCollection errors in ModelState.Values.Select(v => v.Errors))
            {
                foreach (ModelError error in errors)
                {
                    errorMessages.Add(error.ErrorMessage);
                }
            }

            return errorMessages;
        }
    }
}
