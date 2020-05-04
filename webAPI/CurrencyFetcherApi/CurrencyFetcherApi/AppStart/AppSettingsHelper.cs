using CurrencyFetcher.Core.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyFetcherApi.AppStart
{
    public static class AppSettingsHelper
    {
        public static AppSettings ConfigureAppSettingsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingsSection);

            return appSettingsSection.Get<AppSettings>();
        }

        public static JwtSettings ConfigureJwtSettingsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            var jwtSettingsSection = appSettingsSection.GetSection("Jwt");

            services.Configure<JwtSettings>(appSettingsSection.GetSection("Jwt"));

            return jwtSettingsSection.Get<JwtSettings>();
        }
    }
}
