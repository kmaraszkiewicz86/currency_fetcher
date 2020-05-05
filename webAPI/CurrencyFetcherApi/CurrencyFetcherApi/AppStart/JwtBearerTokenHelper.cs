using System;
using System.Text;
using CurrencyFetcher.Core.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyFetcherApi.AppStart
{
    /// <summary>
    /// Helper for configure JSON web token feature
    /// </summary>
    public static class JwtBearerTokenHelper
    {
        /// <summary>
        /// Configure JSON web token feature
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="appSettings"><seealso cref="AppSettings"/></param>
        /// <param name="jwtSettings"><seealso cref="JwtSettings"/></param>
        /// <returns><seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureJwtBearer(this IServiceCollection services, AppSettings appSettings,
            JwtSettings jwtSettings)
        {
            services.AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(auth =>
                {
                    auth.RequireHttpsMetadata = false;
                    auth.SaveToken = true;
                    auth.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret)),
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Issuer,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}