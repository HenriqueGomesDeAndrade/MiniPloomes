using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Entities;
using MiniPloomes.Models.Contacts;
using MiniPloomes.Persistence;
using MiniPloomes.Persistence.Repository;

namespace MiniPloomes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IMiniPloomesRepository _repository;
        public ContactsController(IMiniPloomesRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Consulta todos os clientes de um usuário.
        /// </summary>
        /// <param name="token">Token do criador dos clientes</param>
        /// <returns>Lista de clientes</returns>
        /// <response code="200">Clientes encontrados</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">´Cliente não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpGet]
        public IActionResult GetContacts([FromHeader] string token)
        {
            try
            {
                var user = _repository.GetExposableUserByToken(token);
                if (user == null)
                {
                    return Unauthorized();
                }

                var contacts = _repository.GetContacts(user.Id);
                if (contacts == null)
                {
                    return NotFound();
                }

                return Ok(contacts);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database - {e.Message}");
            }
            
        }

        /// <summary>
        /// Consulta um cliente de um usuário exibindo mais informações.
        /// </summary>
        /// <param name="token">Token do criador do cliente</param>
        /// <param name="id">Id do cliente</param>
        /// <returns>Cliente</returns>
        /// <response code="200">Cliente encontrado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">´Cliente não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpGet("{id}")]
        public IActionResult GetContactById([FromHeader] string token, int id)
        {
            try
            {
                var user = _repository.GetExposableUserByToken(token);
                if (user == null)
                {
                    return Unauthorized();
                }

                var contact = _repository.GetContactByIdAndUser(id, user.Id);
                if (contact == null)
                {
                    return NotFound();
                }

                return Ok(contact);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database - {e.Message}");
            }
            
        }

        /// <summary>
        /// Cadastra um cliente.
        /// </summary>
        /// <remarks>
        /// {
        ///     "name" : "ClienteExemplo"
        /// }
        /// </remarks>
        /// <param name="token">Token de quem será o criador do cliente</param>
        /// <param name="model">Dados do cliente</param>
        /// <returns>Cliente</returns>
        /// <response code="201">Cliente criado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPost]
        public IActionResult CreateContact([FromHeader] string token, AddContactInputModel model)
        {
            try
            {
                var user = _repository.GetExposableUserByToken(token);
                if (user == null)
                {
                    return Unauthorized();
                }

                var contact = new Contact(model.Name, user.Id);
                int contactId = _repository.AddContact(contact);
                var returnableContact = new Contact(contactId, contact.Name, contact.CreatorId, contact.CreateDate);

                return CreatedAtAction("GetContactById", new { id = contactId }, returnableContact);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error inserting data - {e.Message}");
            }
            
        }

        /// <summary>
        /// Atualiza um cliente.
        /// </summary>
        /// <remarks>
        /// {
        ///     "name" : "ClienteExemplo"
        /// }
        /// </remarks>
        /// <param name="token">Token do criador do cliente</param>
        /// <param name="model">Dados do cliente</param>
        /// <param name="id">Id do cliente</param>
        /// <returns>Cliente atualizado</returns>
        /// <response code="200">Cliente atualizado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">´Cliente não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPut("{id}")]
        public IActionResult UpdateContact([FromHeader] string token, AddContactInputModel model, int id)
        {
            try
            {
                var user = _repository.GetExposableUserByToken(token);
                if (user == null)
                {
                    return Unauthorized();
                }

                var contact = _repository.GetContactByIdAndUser(id, user.Id);
                if (contact == null)
                {
                    return NotFound();
                }

                var contactModel = new Contact(id, model.Name, contact.CreatorId, contact.CreateDate);
                _repository.UpdateContact(contactModel);

                return Ok(contactModel);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data from the database - {e.Message}");
            }
            
        }

        /// <summary>
        /// Apaga um cliente.
        /// </summary>
        /// <param name="token">Token do criador do cliente</param>
        /// <param name="id">Id do cliente</param>
        /// <returns>Cliente atualizado</returns>
        /// <response code="200">Cliente apagado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">´Cliente não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteContact([FromHeader] string token, int id)
        {
            try
            {
                var user = _repository.GetExposableUserByToken(token);
                if (user == null)
                {
                    return Unauthorized();
                }

                var contact = _repository.GetContactByIdAndUser(id, user.Id);
                if (contact == null)
                {
                    return NotFound();
                }

                _repository.RemoveContact(id);
                return Ok("The contact was deleted.");
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                     $"Error deleting data - {e.Message}");
            }
            
        }
    }
}
