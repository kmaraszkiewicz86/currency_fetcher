using CurrencyFetcher.Core.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyFetcherApi.AppStart
{
    /// <summary>
    /// Configures data from appsettings into models
    /// </summary>
    public static class OptionsSettingsHelper
    {
        /// <summary>
        /// Configures data from appsettings into <seealso cref="AppSettings"/> model
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><seealso cref="IConfiguration"/></param>
        /// <returns><seealso cref="AppSettings"/></returns>
        public static AppSettings ConfigureAppSettingsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingsSection);

            return appSettingsSection.Get<AppSettings>();
        }

        /// <summary>
        /// Configures data from appsettings into <seealso cref="JwtSettings"/> model
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><seealso cref="IConfiguration"/></param>
        /// <returns><seealso cref="JwtSettings"/></returns>
        public static JwtSettings ConfigureJwtSettingsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            var jwtSettingsSection = appSettingsSection.GetSection("Jwt");

            services.Configure<JwtSettings>(appSettingsSection.GetSection("Jwt"));

            return jwtSettingsSection.Get<JwtSettings>();
        }
    }
}