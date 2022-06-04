using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Entities;
using MiniPloomes.Models;
using MiniPloomes.Persistence;
using MiniPloomes.Persistence.Repository;

namespace MiniPloomes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMiniPloomesRepository _repository;
        public UsersController(IMiniPloomesRepository repository)
        {
            _repository = repository; 
        }


        [HttpGet]
        public IActionResult GetUserByToken([FromHeader] string token)
        {
            var user = _repository.GetUserByToken(token);

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
            _repository.AddUser(user);
            return CreatedAtAction("GetUserByToken", new { id = user.Id }, user);
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginInputModel model)
        {
            if (!String.IsNullOrEmpty(model.Email))
            {
                if (!String.IsNullOrEmpty(model.Password))
                {
                    var user = _repository.ValidateUser(model.Email, model.Password);
                    
                    if (user != null)
                    {
                        user = user.UpdateToken();
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
            User user = _repository.GetUserByToken(token);

            if (user == null)
            {
                return BadRequest();
            }

            user = user.RemoveToken();
            return Ok(user);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromHeader] string token, AddUserInputModel model)
        {
            User user = _repository.GetUserByToken(token);

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
            User user = _repository.GetUserByToken(token);

            if (user == null)
            {
                return NotFound();
            }

            _repository.RemoveUser(user);
            return Ok("The user was deleted");
        }
    }
}
