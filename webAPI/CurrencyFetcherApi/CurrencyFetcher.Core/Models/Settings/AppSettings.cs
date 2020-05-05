namespace CurrencyFetcher.Core.Models.Settings
{
    /// <summary>
    /// Data fetched from appsettings from section JwtSettings
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The token secret hash
        /// </summary>
        public string Secret { get; set; }
    }
}