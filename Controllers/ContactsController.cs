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

        [HttpGet("{id}")]
        public IActionResult GetContactById([FromHeader] string token, int id)
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

        [HttpPost]
        public IActionResult CreateContact([FromHeader] string token, AddContactInputModel model)
        {
            var user = _repository.GetExposableUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = new Contact(model.Name, user.Id);
            _repository.AddContact(contact);

            return CreatedAtAction("GetContactById", new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContact([FromHeader] string token, AddContactInputModel model, int id)
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

            contact.UpdateContact(model.Name);

            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContact([FromHeader] string token, int id)
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

            _repository.RemoveContact(contact);
            return Ok("The contact was deleted.");
        }
    }
}
