using CurrencyFetcher.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CurrencyFetcher.Core.Core
{
    /// <summary>
    /// Class needed for create and manages migrations in class outside from asp.net core
    /// </summary>
    public class CurrencyDbContextFactory : IDesignTimeDbContextFactory<CurrencyDbContext>
    {
        /// <summary>
        /// <see cref="IConfiguration"/>
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        public CurrencyDbContextFactory()
        {
            _configuration = ConnectionStringHelper.GetDefaultConfigurationBuild();
        }

        /// <summary>
        /// Create db context
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CurrencyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CurrencyDbContext>();
            builder.UseSqlServer(_configuration.GetDefaultConnectionString());
            return new CurrencyDbContext(builder.Options);
        }
    }
}
