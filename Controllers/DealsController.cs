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

        [HttpGet]
        public IActionResult GetDeals([FromHeader] string token)
        {
            var user = _repository.GetExposableUserByToken(token);
            if (user == null)
            {
                return Unauthorized();
            }

            var deals = _repository.GetDeals(user.Id);
            if (deals == null)
            {
                return NotFound();
            }

            return Ok(deals);
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
            int dealId = _repository.AddDeal(deal);
            var returnableDeal = new Deal(dealId, deal.Title, deal.Amount, deal.ContactId, deal.CreatorId, deal.CreateDate);

            return CreatedAtAction("GetDealById", new { id = dealId }, returnableDeal);
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

            return Ok(dealUpdated);
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
