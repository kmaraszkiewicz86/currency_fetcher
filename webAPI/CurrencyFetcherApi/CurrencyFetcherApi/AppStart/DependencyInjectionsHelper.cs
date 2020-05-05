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
            services.AddScoped<IXmlReader, XmlReader>();
            services.AddScoped<IHolidayChecker, HolidayChecker>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IDateChecker, DateChecker>();
            services.AddScoped<ICacheDatabase, CacheDatabase>();

            return services;
        }
    }
}
