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

        [HttpGet]
        public IActionResult GetContacts([FromHeader] string token)
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
            int contactId = _repository.AddContact(contact);
            var returnableContact = new Contact(contactId, contact.Name, contact.CreatorId, contact.CreateDate);

            return CreatedAtAction("GetContactById", new { id = contactId }, returnableContact);
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

            var contactModel = new Contact(id, model.Name, contact.CreatorId, contact.CreateDate);
            _repository.UpdateContact(contactModel);

            return Ok(contactModel);
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

            _repository.RemoveContact(id);
            return Ok("The contact was deleted.");
        }
    }
}
