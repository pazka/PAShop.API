using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Models;
using Services.Interfaces;

namespace PAShop.API.Helper
{
    public class AuthenticationHelper
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public AuthenticationHelper(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        public AuthenticationHelper()
        {
        }

        public User Authenticate(dynamic login)
        {
            return this._userService.Authenticate((string)login.Email, (string)login.Password);
        }

        public string BuildToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
