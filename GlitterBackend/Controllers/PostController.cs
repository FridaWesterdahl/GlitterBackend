using System.Collections;
using System.Data;
using GlitterBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
        public async Task<IEnumerable> Get()
        {
            var data = await _EFContext.Posts
                .Select(p => new { p.Id, p.Content, p.Published, p.UserId })
                .ToListAsync();

            return data;
        }
        [HttpPost("createPost")]
        public JsonResult Post(Post post)
        {
            var date = DateTime.Now;
            string q = @"insert into dbo.Posts values
               ('" + post.Content + "', '" + date + "', '" + post.UserId + "')";
            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("Connectionstring");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(dataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(q, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult("Added successfully!");
        }

        [HttpDelete("delete/{id}")]
        public JsonResult Delete(int id)
        {
            string q = @"delete from dbo.Posts
                where Id = " + id + "";
            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("Connectionstring");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(dataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(q, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult("Deleted successfully!");
        }
    }
}