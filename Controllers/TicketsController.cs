using AutoMapper;
using HD_Backend.Data;
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
        public TicketsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, HelpDeskDbContext dbContext) : base(repository, logger, mapper, dbContext)
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
      //  [ResponseCache(CacheProfileName = "30SecondsCaching")]

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

        [HttpGet("todayTickets")]

        public async Task<IActionResult> GetTodayTickets()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime startOfDay = currentDate.Date;
                DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var res = tickets.Where(t => t.CreatedDate >= startOfDay && t.CreatedDate <= endOfDay).Count();
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTodayTickets)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("thisMonthTickets")]
        public async Task<IActionResult> GetMonthTickets()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var res = tickets.Where(t => t.CreatedDate >= startOfMonth && t.CreatedDate <= endOfMonth).Count();
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetMonthTickets)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalOpenTickets")]
        public async Task<IActionResult> GetTotalOpenTickets()
        {
            try
            {
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var totalNumber = tickets.Where(t => t.IsOpen).Count();
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTotalOpenTickets)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalResolvedTickets")]
        public async Task<IActionResult> GetTotalResolvedTickets()
        {
            try
            {
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var totalNumber = tickets.Where(t => t.IsResolved).Count();
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTotalResolvedTickets)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalCancelledTickets")]
        public async Task<IActionResult> GetTotalCancelledTickets()
        {
            try
            {
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var totalNumber = tickets.Where(t => t.IsCancelled).Count();
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTotalCancelledTickets)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalTicketsOnHold")]
        public async Task<IActionResult> GetTotalPausedTicket()
        {
            try
            {
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var totalNumber = tickets.Where(t => t.OnHold).Count();
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTotalPausedTicket)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("totalPriorityTickets")]
        public async Task<IActionResult> GetTotalPriorityTickets()
        {
            try
            {
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var totalNumber = tickets.Where(t => t.IsOpen && t.Priority == "HIGH").Count();
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetTotalPriorityTickets)} action {ex}");
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



        [HttpPut("holdTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]

        public async Task<IActionResult> HoldTicket(long ticketId, [FromBody] HoldTicketDto ticket)
        {
            try
            {
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);
                if (ticketData == null || ticketData.Id != ticketId) return NotFound("Ticket not found in database");
                if (ticketData.IsOpen)
                {
                    ticketData.IsOpen = false;
                    ticketData.Status = "On Hold";
                    ticketData.OnHold = ticket.OnHold;
                    await _repository.SaveAsync();
                    return Ok(ticket);
                }
                else
                {
                    return BadRequest("Could not hold ticket. This is not an open ticket.");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(HoldTicket)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("openTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]
        public async Task<IActionResult> OpenTicket(long ticketId, [FromBody] OpenTicketDto ticket)
        {
            try
            {
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);
                if (ticketData == null || ticketData.Id != ticketId) return NotFound("Ticket not found in database");
                if (!ticketData.IsCancelled && !ticketData.IsResolved)
                {
                    ticketData.IsOpen = ticket.IsOpen;
                    ticketData.OnHold = false;
                    ticketData.Status = "Open";
                    await _repository.SaveAsync();
                    return Ok(ticket);
                }
                else
                {
                    return BadRequest("Could not be reopened.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(OpenTicket)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("cancelTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]

        public async Task<IActionResult> CancelTicket(long ticketId, [FromBody] CancelTicketDto ticket)
        {
            try
            {
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);
                if (ticketData == null || ticketData.Id != ticketId) return NotFound("Ticket not found in database");
                if (ticketData.IsOpen || ticketData.OnHold && !ticketData.IsResolved)
                {
                    ticketData.IsOpen = false;
                    ticketData.OnHold = false;
                    ticketData.IsCancelled = ticket.IsCancelled;
                    ticketData.Status = "Cancelled";
                    await _repository.SaveAsync();
                    return Ok(ticket);
                }
                else
                {
                    return BadRequest("Could not be cancelled.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CancelTicket)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("resolveTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]

        public async Task<IActionResult> ResolveTicket(long ticketId, [FromBody] ResolveTicketDto ticket)
        {
            try
            {
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);
                if (ticketData == null || ticketData.Id != ticketId) return BadRequest("Ticket not found in database");
                if(!ticketData.IsOpen)
                {
                    return BadRequest("Could not be resolved. This is not an open ticket");
                }
                else
                {
                    ticketData.IsOpen = false;
                    ticketData.IsResolved = ticket.IsResolved;
                    ticketData.Status = "Resolved";
                    await _repository.SaveAsync();
                    return Ok(ticket);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(ResolveTicket)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
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
