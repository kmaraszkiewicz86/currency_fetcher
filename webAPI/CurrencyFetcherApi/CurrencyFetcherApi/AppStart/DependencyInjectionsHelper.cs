using CurrencyFetcher.Core.Services.Implementations;
using CurrencyFetcher.Core.Services.Interfaces;
using CurrencyFetcherApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyFetcherApi.AppStart
{
    public static class DependencyInjectionsHelper
    {
        public static IServiceCollection AddDependencyInjectionsCollection(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrencyGetterService, CurrencyGetterService>();

            return services;
        }
    }
}
