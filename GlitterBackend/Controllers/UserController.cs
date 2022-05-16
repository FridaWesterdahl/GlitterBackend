using GlitterBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace GlitterBackend.Controllers
{
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

        [HttpPost("createUser")]
        public async Task<ActionResult<List<User>>> Post(User user)
        {
            var newUser = new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                PhotoFileName = user.PhotoFileName
            };
            
            _EFContext.Users.Add(newUser);
            await _EFContext.SaveChangesAsync();
            return Ok(await _EFContext.Users.ToListAsync());
        }

        [HttpPut("editUser")]
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
            user.PhotoFileName = request.PhotoFileName;

            await _EFContext.SaveChangesAsync();
            return Ok(await _EFContext.Users.ToListAsync());
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

        [Route("SavePic")]
        [HttpPost]
        public JsonResult SavePic()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedPic = httpRequest.Files[0];
                string filename = postedPic.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedPic.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("nopic.png");
            }
        }
    }   
}
