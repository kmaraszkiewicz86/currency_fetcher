using CurrencyFetcherApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyFetcherApi.AppStart
{
    public static class DependencyInjectionsHelper
    {
        public static IServiceCollection AddDependencyInjectionsCollection(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
