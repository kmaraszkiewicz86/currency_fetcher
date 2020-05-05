namespace CurrencyFetcher.Core.Models.Settings
{
    /// <summary>
    /// Data fetched from appsettings from section JwtSettings
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// The token key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The issuer site for token
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Time until token expires.
        /// In hours
        /// </summary>
        public int ExpiresInHours { get; set; }
    }
}