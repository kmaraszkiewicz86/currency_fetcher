using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Models.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyFetcherApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly AppSettings _appSettings;
        private readonly JwtSettings _jwtSettings;

        private List<Claim> _claims;

        public UserService(IOptions<AppSettings> appSettings,
            IOptions<JwtSettings> jwtSettings,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenModel> Authenticate(string login, string password)
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
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_appSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)
            );

            return new TokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task CreateUser(UserModel model)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new BadRequestException("Unable to create user");
        }

        private void GenerateDefaultClaims(IdentityUser user)
        {
            _claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

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
