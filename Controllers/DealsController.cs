using Microsoft.AspNetCore.Mvc;
using MiniPloomes.Entities;
using MiniPloomes.Models.Deals;
using MiniPloomes.Persistence;
using MiniPloomes.Persistence.Repository;
using System.Data.SqlClient;

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

        /// <summary>
        /// Consulta todos os negócios de um usuário.
        /// </summary>
        /// <param name="token">Token do criador dos negócios</param>
        /// <returns>Lista de negócios</returns>
        /// <response code="200">negócios encontrados</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">Negócio não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpGet]
        public IActionResult GetDeals([FromHeader] string token)
        {
            try
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database - {e.Message}");
            }

        }

        /// <summary>
        /// Consulta um negócio de um usuário exibindo mais informações.
        /// </summary>
        /// <param name="token">Token do criador do negócio</param>
        /// <param name="id">Id do negócio</param>
        /// <returns>Negócio</returns>
        /// <response code="200">Negócio encontrado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">Negócio não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpGet("{id}")]
        public IActionResult GetDealById([FromHeader] string token, int id)
        {
            try
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
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error retrieving data from the database - {e.Message}");
            }
            
        }

        /// <summary>
        /// Cadastra um negócio.
        /// </summary>
        /// <remarks>
        /// {
        ///     "title" : "negócioExemplo",
        ///     "amount" : 33.567,
        ///     "contactId" : 3
        /// }
        /// </remarks>
        /// <param name="token">Token de quem será o criador do negócio</param>
        /// <param name="model">Dados do negócio</param>
        /// <returns>Negócio</returns>
        /// <response code="201">negócio criado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPost]
        public IActionResult CreateDeal([FromHeader] string token, AddDealInputModel model)
        {
            try
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error inserting data - {e.Message}");
            }

        }

        /// <summary>
        /// Atualiza um negócio.
        /// </summary>
        /// <remarks>
        /// {
        ///     "title" : "negócioExemplo",
        ///     "amount" : 33.567,
        ///     "contactId" : 3
        /// }
        /// </remarks>
        /// <param name="token">Token do criador do negócio</param>
        /// <param name="model">Dados do negócio</param>
        /// <param name="id">Id do negócio</param>
        /// <returns>negócio atualizado</returns>
        /// <response code="200">Negócio atualizado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">Negócio/Cliente não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpPut("{id}")]
        public IActionResult UpdateDeal([FromHeader] string token, AddDealInputModel model, int id)
        {
            try
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
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data - {e.Message}");
            }
            
        }

        /// <summary>
        /// Apaga um negócio.
        /// <param name="token">Token do criador do negócio</param>
        /// <param name="id">Id do negócio</param>
        /// <returns>Negócio atualizado</returns>
        /// <response code="200">Negócio apagado</response>
        /// <response code="401">Usuário não encontrado</response>
        /// <response code="404">Negócio não encontrado</response>
        /// <response code="500">Ocorreu algum erro</response>
        [HttpDelete("{id}")]
        public IActionResult DeleteDeal([FromHeader] string token, int id)
        {
            try
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
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting data - {e.Message}");
            }
            
        }
    }
}
