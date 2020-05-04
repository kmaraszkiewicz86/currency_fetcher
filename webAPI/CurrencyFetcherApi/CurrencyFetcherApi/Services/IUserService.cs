using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;

namespace CurrencyFetcherApi.Services
{
    public interface IUserService
    {
        Task<TokenModel> Authenticate(string username, string password);

        Task CreateUser(UserModel model);
    }
}
