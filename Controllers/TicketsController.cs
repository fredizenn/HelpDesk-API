using AutoMapper;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;
using HD_Backend.Data.Interfaces;
using HD_Backend.Filters.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : BaseApiController
    {
        public TicketsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : base(repository, logger, mapper)
        {
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto ticket)
        {
            var ticketCode = await _repository.Ticket.GetTicketByCode(ticket.Code, trackChanges: false);
            if (ticketCode != null)
            {
                return BadRequest($"Ticket with code: {ticket.Code} already exists");
            }
            else
            {
                var ticketData = _mapper.Map<Ticket>(ticket);
                await _repository.Ticket.CreateTicket(ticketData);
                await _repository.SaveAsync();
                var ticketReturn = _mapper.Map<TicketDto>(ticketData);
                return Created($"api/Tickets/{ticketReturn.Id}", _mapper.Map<TicketDto>(ticketData));
            }
        }


        [HttpGet]
        [ResponseCache(CacheProfileName = "30SecondsCaching")]

        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var ticketDto = _mapper.Map<IEnumerable<TicketDto>>(tickets);
                return Ok(ticketDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTickets)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]

        public async Task<IActionResult> UpdateTicket(long ticketId, [FromBody] UpdateTicketDto ticket)
        {
            var ticketData = HttpContext.Items["ticket"] as Ticket;
            _mapper.Map(ticket, ticketData);
            await _repository.SaveAsync();
            return Ok(ticketData);
        }


        [HttpDelete("{ticketId}")]
        [ServiceFilter(typeof(ValidateTicket))]
        public async Task<IActionResult> DeleteTicket(long ticketId)
        {
            var ticket = await _repository.Ticket.GetTicket(ticketId, trackChanges: false);
            if(ticket != null)
            {
                await _repository.Ticket.DeleteTicket(ticket);
                await _repository.SaveAsync();
                return Ok(true);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
