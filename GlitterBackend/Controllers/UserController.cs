using GlitterBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GlitterBackend.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly EFContext _EFContext;
        public UserController(IConfiguration configuration, IWebHostEnvironment env, EFContext context)
        {
            _configuration = configuration;
            _env = env;
            _EFContext = context;
        }

        [AllowAnonymous]
        [HttpGet("getCurrentUser")]
        public IQueryable<Object> Get(string username)
        {
            var q =
                (from u in _EFContext.Users
                 where u.Username == username
                 select u);

            return (q);
        }

        [AllowAnonymous]
        [HttpGet("getUsers")]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(await _EFContext.Users.ToListAsync());
        }

        [HttpGet("getUserById/{id}")]
        public async Task<ActionResult<List<User>>> Get(int id)
        {
            var user = await _EFContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("createUser")]
        public async Task<ActionResult<List<User>>> Post(User user)
        {
            if (await _EFContext.Users.AnyAsync(x => x.Username == user.Username))
            {
                ModelState.AddModelError("username", "Username is already taken.");
                return ValidationProblem();
            }

            var newUser = new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };
            
            _EFContext.Users.Add(newUser);
            await _EFContext.SaveChangesAsync();
            return Ok(newUser);
        }

        [HttpPut("editUser/{id}")]
        public async Task<ActionResult<List<User>>> Put(User request)
        {
            var user = await _EFContext.Users.FindAsync(request.Id);
            if (user == null)
            {
                return NotFound("User not found");
            }
 
            user.Username = request.Username;
            user.Email = request.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                
            await _EFContext.SaveChangesAsync();
            return Ok(await _EFContext.Users.ToListAsync()); 
            
        }

        [HttpPut("editUsername/{id}")]
        public async Task<ActionResult<List<User>>> PutUsername(User request)
        {
             var user = await _EFContext.Users.FindAsync(request.Id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (await _EFContext.Users.AnyAsync(user => user.Username == request.Username))
            {
                ModelState.AddModelError("username", "Username is already taken.");
                return ValidationProblem();
            }
            else
            {
                user.Username = request.Username;

                await _EFContext.SaveChangesAsync();
                return Ok(await _EFContext.Users.ToListAsync());
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            var user = await _EFContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            _EFContext.Users.Remove(user);
            await _EFContext.SaveChangesAsync();
            return Ok(await _EFContext.Users.ToListAsync());
        }

    }   
}
