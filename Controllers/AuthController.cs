using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API_App.DAL;
using API_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API_App.Controllers
{
    public class AuthController : ControllerBase
    {
        
        private IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        private string GenerateJsonWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(120),
                claims: GetTokenClaims(userInfo),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static IEnumerable<Claim> GetTokenClaims(UserModel user)
        {
            return new List<Claim>
            {
                new Claim("UserName", user.Username),
            };
        }
        
        private async Task<UserModel> AuthenticateUser(string username, string password)
        {
            var db = new UserContext();
            var test = db.Users
                .FirstOrDefault(p => p.Username == username && p.Password == password);
            return test;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromQuery] string username,[FromQuery] string password)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(username,password);
            if (username != null)
            {
                var tokenString = GenerateJsonWebToken(user);
                response = Ok(new {Token = tokenString, Message = "Success"});
            }
            return response;
        }
        
        [AllowAnonymous]
        [HttpPost(nameof(Register))]
        public void Register([FromQuery] string username,[FromQuery] string password,[FromQuery] string mailaddress)
        {
            var db = new UserContext();
            db.Database.EnsureCreated();
            db.Users.Add(new UserModel()
            {
                Username = username,
                Password = password,
                Mailaddress = mailaddress,
                CreationDate = DateTime.Now
            });
            db.SaveChanges();
        }
        
        [HttpGet(nameof(GetJwtToken))]
        public async Task<JwtSecurityToken> GetJwtToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            return token;
        }
    }
}