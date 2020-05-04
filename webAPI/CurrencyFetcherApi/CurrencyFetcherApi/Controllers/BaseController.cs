using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace CurrencyFetcherApi.Controllers
{
    public abstract class BaseController<TController> : ControllerBase
        where TController: ControllerBase
    {
        protected ILogger<TController> _logger;

        protected BaseController(ILogger<TController> logger)
        {
            _logger = logger;
        }

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
                _logger.LogInformation($"Occurs {nameof(NotFoundException)} with errorMessage => {err.Message}");
                return NotFound(new CurrencyErrorModel(err.Message));
            }
            catch (BadRequestException err)
            {
                _logger.LogInformation($"Occurs {nameof(BadRequestException)} with errorMessage => {err.Message}");
                return BadRequest(new CurrencyErrorModel(err.Message));
            }
            catch (Exception err)
            {
                _logger.LogInformation($"Occurs {nameof(Exception)} with errorMessage => {err.Message}");
                return BadRequest(new CurrencyErrorModel("An unknown error occurs. Please contact to system administrator."));
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
