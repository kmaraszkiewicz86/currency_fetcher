using CurrencyFetcher.Core.Core;
using CurrencyFetcher.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyFetcherApi.AppStart
{
    /// <summary>
    /// Configures database context
    /// </summary>
    public static class DbContextHelper
    {
        /// <summary>
        /// Configures database context
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddDefaultDbContext(this IServiceCollection services)
        {
            services.AddDbContext<CurrencyDbContext>(options =>
                options.UseSqlServer(ConnectionStringHelper.GetDefaultConfigurationBuild().GetDefaultConnectionString()));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<CurrencyDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
