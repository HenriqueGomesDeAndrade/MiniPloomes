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

        /// <summary>
        /// Consulta o usuário de acordo com o Token passado.
        /// </summary>
        /// <param name="token">Token do usuário</param>
        /// <returns>Usuário</returns>
        /// <response code="200">Usuário encontrado</response>
        /// <response code="404">Não foi encontrado nenhum usuário com esse token</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpGet]
        public IActionResult GetUserByToken([FromHeader] string token)
        {
            try
            {
                var exposableUser = _repository.GetExposableUserByToken(token);

                if (exposableUser == null)
                {
                    return NotFound();
                }
                return Ok(exposableUser);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database - {e.Message}");
            }
        }

        /// <summary>
        /// Cadastro de um usuário.
        /// </summary>
        /// <remarks>
        /// {
        ///     "name" : "Henrique"  ,
        ///     "email": "hgomes.andrade@gmail.com",
        ///     "password": "SenhaExemplo"
        /// }
        /// </remarks>
        /// <param name="model">Dados de um usuário</param>
        /// <returns>Usuário criado</returns>
        /// <response code="201">Cadastro realizado com sucesso</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPost]
        public IActionResult CreateUser(AddUserInputModel model)
        {
            try
            {
                User user = new User(model.Name, model.Email, model.Password);
                _repository.AddUser(user);
                var exposableUser = _repository.GetExposableUserByToken(user.Token);
                return CreatedAtAction("GetUserByToken", new { id = exposableUser.Id }, exposableUser);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database - {e.Message}");
            }
            
        }

        /// <summary>
        /// Realiza o Login do usuário, devolvendo um token novo.
        /// </summary>
        /// <remarks>
        /// {
        ///     "email" : "emailexemplo"  ,
        ///     "password": "SenhaExemplo"
        /// }
        /// </remarks>
        /// <param name="model">Dados do Usuáio</param>
        /// <returns>Usuário Logado</returns>
        /// <response code="200">Usuário logado</response>
        /// <response code="404">Não foi encontrado nenhum usuáio com essas informações</response>
        /// <response code="404">Email ou senha estão vazios ou nulos</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPost("Login")]
        public IActionResult Login(UserLoginInputModel model)
        {
            try
            {
                if (!String.IsNullOrEmpty(model.Email))
                {
                    if (!String.IsNullOrEmpty(model.Password))
                    {
                        var exposableUser = _repository.ValidateUser(model.Email, model.Password);

                        if (exposableUser != null)
                        {
                            exposableUser.Token = _repository.UpdateTokenById(exposableUser.Id);
                            return Ok(exposableUser);
                        }
                        return NotFound();
                    }
                    else
                    {
                        throw new ArgumentNullException("Password can't be null");
                    }
                }
                else
                {
                    throw new ArgumentNullException("Email can't be null");
                }
            }
            catch(ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data - {e.Message}");
            }
        }

        /// <summary>
        /// Desloga um usuário, removendo o seu Token
        /// </summary>
        /// <param name="token">token do usuário que será deslogado</param>
        /// <response code="204">Usuário deslogado com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPost("Logout")]
        public IActionResult Logout([FromHeader] string token)
        {
            try
            {
                var exposableUser = _repository.GetExposableUserByToken(token);

                if (exposableUser == null)
                {
                    return NotFound();
                }

                _repository.RemoveToken(token);
                return NoContent();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data - {e.Message}");
            }
            
        }

        /// <summary>
        /// Atualiza um usuário
        /// </summary>
        /// <remarks>
        /// {
        ///     "name" : "Henrique"  ,
        ///     "email": "hgomes.andrade@gmail.com",
        ///     "password": "SenhaExemplo"
        /// }    
        /// </remarks>
        /// <param name="token">token do usuário que será atualizado</param>
        /// <param name="model">Dados do usuário</param>
        /// <returns>Usuário atualizado</returns>
        /// <response code="200">Usuário atualizado com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPut]
        public IActionResult UpdateUser([FromHeader] string token, AddUserInputModel model)
        {
            try
            {
                var user = _repository.GetExposableUserByToken(token);

                if (user == null)
                {
                    return NotFound();
                }

                var newUser = new User(user.Id, model.Name, model.Email, model.Password, user.Token, user.CreateDate);

                _repository.UpdateUser(newUser);
                var exposableUser = _repository.GetExposableUserByToken(token);
                return Ok(exposableUser);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data - {e.Message}");
            }
            
        }

        /// <summary>
        /// Apaga um usuário
        /// </summary>
        /// <param name="token">Token do usuário que será apagado</param>
        /// <returns>Usuário atualizado</returns>
        /// <response code="200">Usuário apagado com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpDelete]
        public IActionResult DeleteUser([FromHeader] string token)
        {
            try
            {
                var user = _repository.GetExposableUserByToken(token);

                if (user == null)
                {
                    return NotFound();
                }

                _repository.RemoveUser(token);
                return Ok("The user was deleted");
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting data - {e.Message}");
            }
            
        }
    }
}
