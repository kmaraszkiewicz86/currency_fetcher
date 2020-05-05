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
    /// <summary>
    /// Global constructor has shared method for process errors
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    public abstract class BaseController<TController> : ControllerBase
        where TController: ControllerBase
    {
        /// <summary>
        /// <see cref="ILogger{TConroller}"/>
        /// </summary>
        protected ILogger<TController> _logger;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="logger"><see cref="ILogger{TConroller}"/></param>
        protected BaseController(ILogger<TController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method that to handle errors from services
        /// </summary>
        /// <param name="action"><see cref="Func{Task{IActionResult}}"/></param>
        /// <returns><see cref="Task{IActionResult}"/></returns>
        protected async Task<IActionResult> OnActionAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new CurrencyErrorResponse(ValidateModelState()));
                }

                return await action();
            }
            catch (NotFoundException err)
            {
                _logger.LogError($"Occurs {nameof(NotFoundException)} with errorMessage => {err.Message}");
                return NotFound(new CurrencyErrorResponse(err.Message));
            }
            catch (UnauthorizedException)
            {
                _logger.LogError($"User provide invalid token");
                return Unauthorized();
            }
            catch (BadRequestException err)
            {
                _logger.LogError($"Occurs {nameof(BadRequestException)} with errorMessage => {err.Message}");
                return BadRequest(new CurrencyErrorResponse(err.Message));
            }
            catch (Exception err)
            {
                _logger.LogError($"Occurs {nameof(Exception)} with errorMessage => {err.Message}");
                return BadRequest(new CurrencyErrorResponse("An unknown error occurs. Please contact to system administrator."));
            }
        }

        /// <summary>
        /// Get error from <see cref="ModelStateDictionary"/>
        /// </summary>
        /// <returns>return list of errors in <seealso cref="List{string}"/></returns>
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