using System;

namespace CurrencyFetcher.Core.Exceptions
{
    /// <summary>
    /// Helps to returns BadRequestResponse if any of that group error occurs
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="message">The error message</param>
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
