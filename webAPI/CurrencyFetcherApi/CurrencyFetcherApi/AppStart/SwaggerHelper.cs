using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CurrencyFetcherApi.AppStart
{
    /// <summary>
    /// Helps to configure swagger
    /// </summary>
    public static class SwaggerHelper
    {
        /// <summary>
        /// Register the Swagger generator, defining 1 or more Swagger documents
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency Fetcher Documentation", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        /// <summary>
        /// Enable middleware to serve generated Swagger as a JSON endpoint.
        /// and
        /// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        /// specifying the Swagger JSON endpoint.
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerFunctionality(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
