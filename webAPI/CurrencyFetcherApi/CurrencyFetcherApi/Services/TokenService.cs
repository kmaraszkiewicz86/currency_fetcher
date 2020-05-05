using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Models.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyFetcherApi.Services
{
    /// <summary>
    /// Contains user helper methods
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// <see cref="UserManager{IdentityUser}"/>
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// <see cref="SignInManager{IdentityUser}"/>
        /// </summary>
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// <see cref="AppSettings"/>
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// <see cref="JwtSettings"/>
        /// </summary>
        private readonly JwtSettings _jwtSettings;

        private List<Claim> _claims;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="appSettings"><see cref="AppSettings"/></param>
        /// <param name="jwtSettings"><see cref="JwtSettings"/></param>
        /// <param name="userManager"><see cref="UserManager{IdentityUser}"/></param>
        /// <param name="signInManager"><see cref="SignInManager{IdentityUser}"/></param>
        public TokenService(IOptions<AppSettings> appSettings,
            IOptions<JwtSettings> jwtSettings,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Authenticate and generate token string
        /// </summary>
        /// <param name="login">The username</param>
        /// <param name="password">The password</param>
        /// <returns>Token string</returns>
        public async Task<TokenResponse> AuthenticateAsync(string login, string password)
        {
            var user = await _userManager.FindByNameAsync(login);

            if (user == null)
                return null;

            var result = await _signInManager.PasswordSignInAsync(
                login, password, false, false);

            if (!result.Succeeded)
                return null;

            GenerateDefaultClaims(user);
            MergeUserClaims(user);

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Issuer,
                _claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiresInHours),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_appSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)
            );

            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        /// <summary>
        /// Validate api key token
        /// </summary>
        /// <param name="token">Ap key token string</param>
        public void ValidateCurrentToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = _jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret))
                }, out SecurityToken validatedToken);
            }
            catch
            {
                throw new UnauthorizedException();
            }
        }

        /// <summary>
        /// Generate default claims for user
        /// </summary>
        /// <param name="user"></param>
        private void GenerateDefaultClaims(IdentityUser user)
        {
            _claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        /// <summary>
        /// Merge user claims whit default claims
        /// </summary>
        /// <param name="user"></param>
        private async void MergeUserClaims(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            foreach (var userClaim in userClaims)
            {
                _claims.Add(userClaim);
            }
        }
    }
}