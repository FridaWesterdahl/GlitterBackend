﻿using System.Collections;
using System.Data;
using GlitterBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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

        private static List<User> users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "FridaCool",
                Email = "fridacool@gmail.com",
                Password = "password",
                PhotoFileName = "nopic.png"
            }
        };


        [HttpGet("getUsers")]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(users);
        }

        [HttpGet("getUserById/{id}")]
        public async Task<ActionResult<List<User>>> Get(int id)
        {
            var user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return Ok(users);
        }

        [HttpPost("createUser")]
        public async Task<ActionResult<List<User>>> Post(User user)
        {
            users.Add(user);
            return Ok(users);
        }

        [HttpPut("editUser")]
        public async Task<ActionResult<List<User>>> Put(User request)
        {
            var user = users.Find(u => u.Id == request.Id);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            user.Username = request.Username;
            user.Email = request.Email;
            user.Password = request.Password;
            user.PhotoFileName = request.PhotoFileName;

            users.Add(user);
            return Ok(users);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            var user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            users.Remove(user);
            return Ok(users);
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