using System.Collections.Generic;

namespace CurrencyFetcher.Core.Models.Responses
{
    /// <summary>
    /// Data contains error messages occurs in system
    /// </summary>
    public class CurrencyErrorModel
    {
        /// <summary>
        /// List of error message to show to user
        /// </summary>
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public CurrencyErrorModel(string errorMessage)
        {
            ErrorMessages = new List<string>
            {
                errorMessage
            };
        }

        /// <summary>
        /// Created for json deserialization
        /// </summary>
        public CurrencyErrorModel()
        {
            
        }

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="errorMessages">The list of error messages</param>
        public CurrencyErrorModel(List<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
