using System;

namespace CurrencyFetcher.Core.Exceptions
{
    /// <summary>
    /// Helps to returns NotFoundResponse if any of that group error occurs
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="message">The error message</param>
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
