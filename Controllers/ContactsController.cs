using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Entities;
using MiniPloomes.Models.Contacts;
using MiniPloomes.Persistence;

namespace MiniPloomes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly MiniPloomesContext _context;
        public ContactsController(MiniPloomesContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetContactById([FromHeader] string token, int id)
        {
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id && c.CreatorId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        public IActionResult CreateContact([FromHeader] string token, AddContactInputModel model)
        {
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = new Contact(model.Name, user.Id);
            _context.Contacts.Add(contact);

            return CreatedAtAction("GetContactById", new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContact([FromHeader] string token, AddContactInputModel model, int id)
        {
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id && c.CreatorId == user.Id);
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
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id && c.CreatorId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            return Ok("The contact was deleted.");
        }
    }
}
