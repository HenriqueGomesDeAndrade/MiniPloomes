using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Entities;
using MiniPloomes.Models.Deals;
using MiniPloomes.Persistence;

namespace MiniPloomes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DealsController : ControllerBase
    {
        private MiniPloomesContext _context;
        public DealsController(MiniPloomesContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetDealById([FromHeader] string token, int id)
        {
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var deal = _context.Deals.FirstOrDefault(c => c.Id == id && c.CreatorId == user.Id);
            if (deal == null)
            {
                return NotFound();
            }

            return Ok(deal);
        }

        [HttpPost]
        public IActionResult CreateDeal([FromHeader] string token, AddDealInputModel model)
        {
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = _context.Contacts.FirstOrDefault(c => c.Id == model.ContactId && c.CreatorId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            var deal = new Deal(model.Title, model.Amount, model.ContactId, user.Id);
            _context.Deals.Add(deal);

            return CreatedAtAction("GetDealById", new { id = deal.Id }, deal);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDeal([FromHeader] string token, AddDealInputModel model, int id)
        {
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = _context.Contacts.FirstOrDefault(c => c.Id == model.ContactId && c.CreatorId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            var deal = _context.Deals.FirstOrDefault(c => c.Id == id && c.CreatorId == user.Id);
            if (deal == null)
            {
                return NotFound();
            }

            deal.UpdateDeal(model.Title, model.Amount, model.ContactId);

            return Ok(deal);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDeal([FromHeader] string token, int id)
        {
            var user = _context.FindUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var deal = _context.Deals.FirstOrDefault(c => c.Id == id && c.CreatorId == user.Id);
            if (deal == null)
            {
                return NotFound();
            }

            _context.Deals.Remove(deal);
            return Ok("The deal was deleted.");
        }
    }
}
