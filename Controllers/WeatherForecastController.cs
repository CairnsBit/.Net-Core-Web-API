using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using API_App.DAL;
using API_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getall")]
        [Produces("application/json")]
        public async Task<List<TestModel>> GetAll()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var username = token.Claims.FirstOrDefault(c => c.Type == "UserName");
            var mailadress = token.Claims.FirstOrDefault(c => c.Type == "Mailadress");
            
            var db = new AppDbContext();
            var test = db.Tables
                .ToList();
            return test;
        }

        [HttpGet("getbyid")]
        [Produces("application/json")]
        public async Task<TestModel> GetById([FromQuery] int id)
        {    
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            
            var db = new AppDbContext();
            var test = db.Tables
                .FirstOrDefault(p => p.Id == id);
            return test;
        }
        
        [HttpGet("getmaxtemp")]
        [Produces("application/json")]
        public async Task<int> GetMaxTemp()
        {    
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            
            var db = new AppDbContext();
            int test = db.Tables
                .Max(p => p.Temperature);
            return test;
        }
        
        [HttpGet("getmintemp")]
        [Produces("application/json")]
        public async Task<int> GetMinTemp()
        {   
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            
            var db = new AppDbContext();
            int test = db.Tables
                .Min(p => p.Temperature);
            return test;
        }
        
        [HttpGet("getbysummary")]
        [Produces("application/json")]
        public async Task<TestModel> GetBySummary([FromQuery]string summary)
        {    
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            
            var db = new AppDbContext();
            var test = db.Tables
                .FirstOrDefault(p=> p.Summary == summary);
            return test;
        }
        
        [HttpPost("post")]
        public async Task Post([FromQuery] int temp, [FromQuery] string summary)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            
            var db = new AppDbContext();
            db.Database.EnsureCreated();
            db.Tables.Add(new TestModel
            {
                Temperature = temp,
                Summary = summary,
                Date = DateTime.Now
            });
            db.SaveChanges();
        }

        [HttpDelete("delete")]
        public async void DeleteById([FromQuery] int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            
            var db = new AppDbContext();
            var test = db.Tables
                .FirstOrDefault(p => p.Id == id);

            if (test != null)
            {
                db.Tables.Remove(test);
                db.SaveChanges();
            }
        }
    }
}