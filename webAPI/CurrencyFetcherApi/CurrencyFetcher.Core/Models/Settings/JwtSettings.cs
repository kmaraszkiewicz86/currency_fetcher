using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyFetcher.Core.Models.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; }

        public string Issuer { get; set; }
    }
}
