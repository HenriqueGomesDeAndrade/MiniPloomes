using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Entities;
using MiniPloomes.Models;
using MiniPloomes.Persistence;

namespace MiniPloomes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MiniPloomesContext _context;
        public UsersController(MiniPloomesContext context)
        {
            _context = context; 
        }


        [HttpGet]
        public IActionResult GetUserById([FromHeader] string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.Token == token);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser(AddUserInputModel model)
        {
            User user = new User(model.Name, model.Email, model.Password);
            _context.Users.Add(user);
            return CreatedAtAction("GetUserById", new { id = user.Id }, user);
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginInputModel model)
        {
            if (!String.IsNullOrEmpty(model.Email))
            {
                if (!String.IsNullOrEmpty(model.Password))
                {
                    var user = _context.ValidateUser(model.Email, model.Password);
                    
                    if (user != null)
                    {
                        user = user.Login();
                        return Ok(user);
                    }
                    return NotFound();
                }
            }
            return BadRequest();
        }

        [HttpPost("Logout")]
        public IActionResult Logout([FromHeader] string token)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == token);

            if (user == null)
            {
                return BadRequest();
            }

            user = user.Logout();
            return Ok(user);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromHeader] string token, AddUserInputModel model)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == token);

            if (user == null)
            {
                return BadRequest();
            }

            user.UpdateUser(model.Name, model.Email, model.Password);   
            return Ok(user);
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromHeader] string token)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == token);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            return Ok("O usuário foi excluído");
        }
    }
}
