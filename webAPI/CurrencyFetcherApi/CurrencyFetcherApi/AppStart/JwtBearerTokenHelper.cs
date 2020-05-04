using System;
using System.Text;
using CurrencyFetcher.Core.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyFetcherApi.AppStart
{
    public static class JwtBearerTokenHelper
    {
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