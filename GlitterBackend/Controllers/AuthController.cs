using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlitterBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;

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
            var resp = Authenticate(authUser);

            if (resp != null)
            {
                var token = Generate(resp);
                return Ok(token);
            }

            return NotFound("User not found");
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
            
            var hashedPassword = user.Password;
            var verifiedPassword = BCrypt.Net.BCrypt.Verify(hashedPassword, authUser.Password);

            var resp = _EFContext.Users.FirstOrDefault(x => x.Username.ToLower() == 
            authUser.Username.ToLower() && verifiedPassword);

            if (resp != null)
            {
                return resp;
            }

            return null;
        }
    }
}
