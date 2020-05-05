using System;
using System.Collections.Generic;
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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Currency Fetcher Documentation", 
                    Version = "v1",
                    Description = "To start working with service. You have to generate token jwt token. <br />" +
                                  "To generate token move to section Token (/api/Token). <br />" +
                                  "Secondly You have to click button Authorization and provide token jwt using format: <br />" +
                                  "Bearer {token} in Value input, after that You can sending requests to Currency (api/Currency) endpoint"
                });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });

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
