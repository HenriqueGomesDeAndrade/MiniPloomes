using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Entities;
using MiniPloomes.Models.Deals;
using MiniPloomes.Persistence;
using MiniPloomes.Persistence.Repository;

namespace MiniPloomes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DealsController : ControllerBase
    {
        private readonly IMiniPloomesRepository _repository;
        public DealsController(IMiniPloomesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult GetDealById([FromHeader] string token, int id)
        {
            var user = _repository.GetExposableUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var deal = _repository.GetDealByIdAndUser(id, user.Id);
            if (deal == null)
            {
                return NotFound();
            }

            return Ok(deal);
        }

        [HttpPost]
        public IActionResult CreateDeal([FromHeader] string token, AddDealInputModel model)
        {
            var user = _repository.GetExposableUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = _repository.GetContactByIdAndUser(model.ContactId, user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            var deal = new Deal(model.Title, model.Amount, model.ContactId, user.Id);
            _repository.AddDeal(deal);

            return CreatedAtAction("GetDealById", new { id = deal.Id }, deal);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDeal([FromHeader] string token, AddDealInputModel model, int id)
        {
            var user = _repository.GetExposableUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var contact = _repository.GetContactByIdAndUser(model.ContactId, user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            var deal = _repository.GetDealByIdAndUser(id, user.Id);
            if (deal == null)
            {
                return NotFound();
            }

            var dealUpdated = new Deal(id, model.Title, model.Amount, model.ContactId, deal.CreatorId, deal.CreateDate);
            _repository.UpdateDeal(dealUpdated);

            return Ok(deal);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDeal([FromHeader] string token, int id)
        {
            var user = _repository.GetExposableUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var deal = _repository.GetDealByIdAndUser(id, user.Id);
            if (deal == null)
            {
                return NotFound();
            }

            _repository.RemoveDeal(id);
            return Ok("The deal was deleted.");
        }
    }
}
