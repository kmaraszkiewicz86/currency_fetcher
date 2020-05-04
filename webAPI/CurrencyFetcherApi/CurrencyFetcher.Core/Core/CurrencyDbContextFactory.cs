using CurrencyFetcher.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CurrencyFetcher.Core.Core
{
    public class CurrencyDbContextFactory : IDesignTimeDbContextFactory<CurrencyDbContext>
    {
        public IConfiguration _configuration;

        public CurrencyDbContextFactory()
        {
            _configuration = ConnectionStringHelper.GetDefaultConfigurationBuild();
        }

        public CurrencyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CurrencyDbContext>();
            builder.UseSqlServer(_configuration.GetDefaultConnectionString());
            return new CurrencyDbContext(builder.Options);
        }
    }
}
