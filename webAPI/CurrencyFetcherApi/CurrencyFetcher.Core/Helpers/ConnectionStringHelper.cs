using Microsoft.Extensions.Configuration;

namespace CurrencyFetcher.Core.Helpers
{
    public static class ConnectionStringHelper
    {
        public static IConfiguration GetDefaultConfigurationBuild()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration["ConnectionString"];
        }
    }
}
