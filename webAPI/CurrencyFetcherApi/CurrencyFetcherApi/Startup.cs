using CurrencyFetcher.Core.Helpers;
using CurrencyFetcherApi.AppStart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CurrencyFetcherApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = ConnectionStringHelper.GetDefaultConfigurationBuild();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton(Configuration);

            var appSettings = services.ConfigureAppSettingsOptions(Configuration);
            var jwtSettings = services.ConfigureJwtSettingsOptions(Configuration);

            services.AddDependencyInjectionsCollection();

            services.AddDefaultDbContext();

            services.ConfigureJwtBearer(appSettings, jwtSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
