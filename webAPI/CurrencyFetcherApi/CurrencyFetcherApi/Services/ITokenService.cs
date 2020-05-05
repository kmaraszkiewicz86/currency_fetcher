using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcherApi.Services
{
    /// <summary>
    /// Contains user helper methods
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Authenticate and generate token string
        /// </summary>
        /// <param name="login">The username</param>
        /// <param name="password">The password</param>
        /// <returns>Token string</returns>
        Task<TokenResponse> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Validate api key token
        /// </summary>
        /// <param name="token">Ap key token string</param>
        public void ValidateCurrentToken(string token);
    }
}
