namespace CurrencyFetcher.Core.Models.Responses
{
    /// <summary>
    /// Token data that was generated from jwt service
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// The token bearer auth string
        /// </summary>
        public string Token { get; set; }
    }
}
