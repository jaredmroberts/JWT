using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Models.Configuration;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenController : Controller
    {
        private readonly AppSettings _settings;

        public TokenController(IOptions<AppSettings> settings) => _settings = settings.Value;


        [AllowAnonymous]
        [HttpPost]
        public IActionResult GenerateToken([FromBody] TokenRequest request)
        {
            if (request.Username == "Jon" && request.Password == "letmein")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Security.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _settings.Security.Issuer,
                    audience: _settings.Security.Issuer,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok( _settings.Security.SecurityKey);
        }
    }
}
