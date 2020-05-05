using CurrencyFetcher.Core.Services.Implementations;
using CurrencyFetcher.Core.Services.Interfaces;
using CurrencyFetcherApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyFetcherApi.AppStart
{
    /// <summary>
    /// Configures a dependency injection classes
    /// </summary>
    public static class DependencyInjectionsHelper
    {
        /// <summary>
        /// Configures a dependency injection classes
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddDependencyInjectionsCollection(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrencyGetterService, CurrencyGetterService>();
            services.AddScoped<IXmlReader, XmlReader>();
            services.AddScoped<IHolidayChecker, HolidayChecker>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IDateService, DateService>();
            services.AddScoped<ICacheDatabase, CacheDatabase>();

            return services;
        }
    }
}
