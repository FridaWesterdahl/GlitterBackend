using System.Data;
using GlitterBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GlitterBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly EFContext _EFContext;

        public PostController(IConfiguration configuration, IWebHostEnvironment env, EFContext context)
        {
            _configuration = configuration;
            _env = env;
            _EFContext = context;
        }


        [HttpGet("getPosts")]
        public IQueryable<Object> Get()
        {
            var q =
                (from p in _EFContext.Posts
                 join u in _EFContext.Users
                 on p.UserId equals u.Id
                 select new
                 {
                     PostId = p.Id,
                     Content = p.Content,
                     Published = p.Published,
                     User = u.Username

                 });
            
            return (q);
        }
           
        //[HttpGet("getPosts")]
        //public async Task<ActionResult<List<Post>>> Get()
        //{
        //    return Ok(await _EFContext.Posts.ToListAsync());

        //}

        [HttpGet("getPostById/{id}")]
        public async Task<ActionResult<List<Post>>> Get(int id)
        {
            var post = await _EFContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post not found");
            }
            return Ok(post);
        }


        [HttpPost("createPost")]
        public async Task<ActionResult<List<Post>>> Post(Post post)
        {
            _EFContext.Posts.Add(post);
            await _EFContext.SaveChangesAsync();
            return Ok(await _EFContext.Posts.ToListAsync());
        }

        [HttpPut("editPost/{id}")]
        public async Task<ActionResult<List<User>>> Put(Post request)
        {
            var post = await _EFContext.Posts.FindAsync(request.Id);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            post.Content = request.Content;

            await _EFContext.SaveChangesAsync();
            return Ok(await _EFContext.Posts.ToListAsync());
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<List<Post>>> Delete(int id)
        {
            var post = await _EFContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            _EFContext.Posts.Remove(post);
            await _EFContext.SaveChangesAsync();
            return Ok(await _EFContext.Posts.ToListAsync());
        }
    }
}