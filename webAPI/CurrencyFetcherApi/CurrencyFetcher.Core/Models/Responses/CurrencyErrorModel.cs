using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyFetcher.Core.Models.Responses
{
    public class CurrencyErrorModel
    {
        public List<string> ErrorMessages { get; set; }

        public CurrencyErrorModel(string errorMessage)
        {
            ErrorMessages = new List<string>
            {
                errorMessage
            };
        }

        public CurrencyErrorModel(List<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
