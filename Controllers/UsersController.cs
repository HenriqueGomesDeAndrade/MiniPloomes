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
            var exposableUser = _repository.GetExposableUserByToken(token);

            if (exposableUser == null)
            {
                return NotFound();
            }
            return Ok(exposableUser);
        }

        [HttpPost]
        public IActionResult CreateUser(AddUserInputModel model)
        {
            User user = new User(model.Name, model.Email, model.Password);
            _repository.AddUser(user);
            var exposableUser = _repository.GetExposableUserByToken(user.Token);
            return CreatedAtAction("GetUserByToken", new { id = exposableUser.Id }, exposableUser);
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginInputModel model)
        {
            if (!String.IsNullOrEmpty(model.Email))
            {
                if (!String.IsNullOrEmpty(model.Password))
                {
                    var exposableUser = _repository.ValidateUser(model.Email, model.Password);
                    
                    if (exposableUser != null)
                    {
                        exposableUser.Token =  _repository.UpdateTokenById(exposableUser.Id);
                        return Ok(exposableUser);
                    }
                    return NotFound();
                }
            }
            return BadRequest();
        }

        [HttpPost("Logout")]
        public IActionResult Logout([FromHeader] string token)
        {
            var exposableUser = _repository.GetExposableUserByToken(token);

            if (exposableUser == null)
            {
                return NotFound();
            }

            _repository.RemoveToken(token);
            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateUser([FromHeader] string token, AddUserInputModel model)
        {
            var user = _repository.GetExposableUserByToken(token);

            if (user == null)
            {
                return NotFound();
            }

            _repository.UpdateUser(model.Name, model.Email, model.Password, token);
            var exposableUser = _repository.GetExposableUserByToken(token);
            return Ok(exposableUser);
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromHeader] string token)
        {
            var user = _repository.GetExposableUserByToken(token);

            if (user == null)
            {
                return NotFound();
            }

            _repository.RemoveUser(token);
            return Ok("The user was deleted");
        }
    }
}
