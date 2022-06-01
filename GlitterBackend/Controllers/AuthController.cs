using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlitterBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace GlitterBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EFContext _EFContext;

        public AuthController(IConfiguration configuration, EFContext context)
        {
            _configuration = configuration;
            _EFContext = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] AuthUser authUser)
        {
            var user = Authenticate(authUser);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found with that username and password.");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(AuthUser authUser)
        {
            var user = _EFContext.Users.SingleOrDefault(x => x.Username == authUser.Username);
            if (user == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(authUser.Password, user.Password))
            {
                return user;
            }
            else
            {
                return null;
            }

        }

    }
}
