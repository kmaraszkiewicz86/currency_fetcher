using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcherApi.Services
{
    /// <summary>
    /// Contains user helper methods
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Authenticate and generate token string
        /// </summary>
        /// <param name="login">The username</param>
        /// <param name="password">The password</param>
        /// <returns>Token string</returns>
        Task<TokenModel> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="model"><see cref="UserModel"/></param>
        /// <returns><see cref="Task"/></returns>
        Task CreateUserAsync(UserModel model);

        /// <summary>
        /// Validate api key token
        /// </summary>
        /// <param name="token">Ap key token string</param>
        public void ValidateCurrentToken(string token);
    }
}
