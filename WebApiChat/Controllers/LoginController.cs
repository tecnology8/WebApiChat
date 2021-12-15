using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiChat.Models;

namespace WebApiChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly AuthenticationContext _context;
        public readonly UserManager<ApplicationUser> _userManager;
        public LoginController(AuthenticationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var users = _context.UserModels.AsQueryable();
            return Ok(users);
        }

        [HttpPost("Signup")]
        public IActionResult SignUp([FromBody] UserModel model)
        {
            if (model == null)
                return BadRequest();
            else
            {
                _context.UserModels.Add(model);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Sign up Successfully"
                });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserModel model)
        {
            if (model == null)
                return BadRequest();
            else
            {
                var user = _context.UserModels.Where(x => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Logged in Successfully",
                        UserData = model.FullName
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User Not Found"
                    });
                }
            }
        }
    }
}