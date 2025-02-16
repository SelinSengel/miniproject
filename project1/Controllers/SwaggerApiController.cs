using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project1.Controllers;
using project1.Repositories;
using project1.Services;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwaggerApiController : ControllerBase
    {
        private readonly UserRepository repo;

        public SwaggerApiController (UserRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("LoginInfo")]
        public async Task<IActionResult> GetAll()
        {
            var users = await repo.GetAll();
            return Ok(users);
        }

        [HttpPost("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await repo.GetUserById(id);
            if (user == null)
            {
                return BadRequest("UserId boş olamaz!");
            }
            return Ok(user);
        }

    }
}
