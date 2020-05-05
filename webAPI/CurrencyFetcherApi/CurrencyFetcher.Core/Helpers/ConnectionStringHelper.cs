using Microsoft.Extensions.Configuration;

namespace CurrencyFetcher.Core.Helpers
{
    /// <summary>
    /// The <see cref="IConfiguration"/> helper methods
    /// </summary>
    public static class ConnectionStringHelper
    {
        /// <summary>
        /// Generates default version of <see cref="IConfiguration"/>
        /// </summary>
        /// <returns><see cref="IConfiguration"/></returns>
        public static IConfiguration GetDefaultConfigurationBuild()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        /// <summary>
        /// Get default connection string for database from appsettings
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>The default connection string of database</returns>
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration["ConnectionString"];
        }
    }
}
