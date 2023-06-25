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
            // Constructor for the TicketsController class.
            // It initializes the controller with the required dependencies.

            // The 'repository' parameter is an instance of the IRepositoryManager interface,
            // which provides access to various repositories for data operations.

            // The 'logger' parameter is an instance of the ILoggerManager interface,
            // used for logging any relevant information or errors.

            // The 'mapper' parameter is an instance of the IMapper interface,
            // which is used for mapping objects between different types.

            // The 'dbContext' parameter is an instance of the HelpDeskDbContext,
            // representing the database context used for data access.

            // The 'base' keyword is used to call the constructor of the base class.
            // In this case, it calls the constructor of the base class (likely a controller base class)
            // and passes the dependencies to it for initialization.
        }



        //HTTP request to create a ticket
        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto ticket)
        {

            // Check if a ticket with the given code already exists
            var ticketCode = await _repository.Ticket.GetTicketByCode(ticket.Code, trackChanges: false);
            if (ticketCode != null)
            {
                // Return a BadRequest response indicating that the ticket already exists
                return BadRequest($"Ticket with code: {ticket.Code} already exists");
            }
            else
            {
                // Map the CreateTicketDto object to a Ticket object
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);
                var ticketCount = tickets.Count();
                string tCode = GenerateTicketCode(ticketCount);

                var ticketData = _mapper.Map<Ticket>(ticket);

                ticketData.Code = tCode;

                // Create the ticket in the repository
                await _repository.Ticket.CreateTicket(ticketData);

                // Save changes to the repository
                await _repository.SaveAsync();

                // Map the created Ticket object to a TicketDto object
                var ticketReturn = _mapper.Map<TicketDto>(ticketData);

                // Return a Created response with the URL of the created ticket and the TicketDto object
                return Created($"api/Tickets/{ticketReturn.Id}", _mapper.Map<TicketDto>(ticketData));
            }
        }

        private string GenerateTicketCode (int ticketCount)
        {
            var prefix = "T";
            var tCode = $"{prefix}{ticketCount.ToString().PadLeft(2, '0')}";

            return tCode;
            
        }

        //HTTP request to fetch all tickets
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Map the Ticket objects to TicketDto objects
                var ticketDto = _mapper.Map<IEnumerable<TicketDto>>(tickets);

                // Return an Ok response with the collection of TicketDto objects
                return Ok(ticketDto);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetTickets)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }


        //HTTP request to get one ticket by id
        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetOneTicket(long ticketId)
        {
            try
            {
                var ticket = await _repository.Ticket.GetTicket(ticketId, trackChanges: false);
                if (ticket == null)
                {
                    return NotFound();
                }
                else
                {
                    var ticketDto = _mapper.Map<TicketDto>(ticket);
                    return Ok(ticketDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetOneTicket)} action {ex}");

                return StatusCode(500, "Internal Server Error");

            }

        }


        //get total number of open tickets
        [HttpGet("totalOpenTickets")]
        public async Task<IActionResult> GetTotalOpenTickets()
        {
            try
            {
                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Filter the tickets to count the ones that are open
                var totalNumber = tickets.Where(t => t.IsOpen).Count();

                // Return an Ok response with the count of open tickets
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetTotalOpenTickets)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }


        //get total number of resolved and closed tickets
        [HttpGet("totalResolvedTickets")]
        public async Task<IActionResult> GetTotalResolvedTickets()
        {
            try
            {
                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Filter the tickets to count the ones that are resolved
                var totalNumber = tickets.Where(t => t.IsResolved).Count();

                // Return an Ok response with the count of resolved tickets
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetTotalResolvedTickets)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }

        //total number of cancelled tickets
        [HttpGet("totalCancelledTickets")]
        public async Task<IActionResult> GetTotalCancelledTickets()
        {
            try
            {
                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Filter the tickets to count the ones that are cancelled
                var totalNumber = tickets.Where(t => t.IsCancelled).Count();

                // Return an Ok response with the count of cancelled tickets
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetTotalCancelledTickets)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }


        //get total number of tickets on hold
        [HttpGet("totalTicketsOnHold")]
        public async Task<IActionResult> GetTotalPausedTicket()
        {
            try
            {
                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Filter the tickets to count the ones that are paused
                var totalNumber = tickets.Where(t => t.OnHold).Count();

                // Return an Ok response with the count of paused tickets
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetTotalPausedTicket)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }


        //get total number of high priority tickets
        [HttpGet("totalPriorityTickets")]
        public async Task<IActionResult> GetTotalPriorityTickets()
        {
            try
            {
                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Count the number of tickets that are open and have a priority of "HIGH"
                var totalNumber = tickets.Where(t => t.IsOpen && t.Priority == "HIGH").Count();

                // Return an Ok response with the total number of priority tickets
                return Ok(totalNumber);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetTotalPriorityTickets)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }



        //update a ticket
        [HttpPut("{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]
        public async Task<IActionResult> UpdateTicket(long ticketId, [FromBody] UpdateTicketDto ticket)
        {
            // Retrieve the ticket data from the HttpContext.Items dictionary using the "ticket" key
            var ticketData = HttpContext.Items["ticket"] as Ticket;

            // Map the properties from the provided UpdateTicketDto object to the ticketData object
            _mapper.Map(ticket, ticketData);

            // Save the changes to the repository
            await _repository.SaveAsync();

            // Return an Ok response with the updated ticket data
            return Ok(ticketData);
        }



        //put ticket on hold
        [HttpPut("holdTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]

        public async Task<IActionResult> HoldTicket(long ticketId, [FromBody] HoldTicketDto ticket)
        {
            try
            {
                // Retrieve the ticket data from the repository based on the ticketId
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);

                // Check if the ticket exists and has the correct Id
                if (ticketData == null || ticketData.Id != ticketId)
                    return NotFound("Ticket not found in database");

                // Check if the ticket is currently open
                if (ticketData.IsOpen)
                {
                    // Update the ticket properties to put it on hold
                    ticketData.IsOpen = false;
                    ticketData.Status = "On Hold";
                    ticketData.OnHold = ticket.OnHold;

                    // Save the changes to the repository
                    await _repository.SaveAsync();

                    // Return an Ok response with the updated HoldTicketDto object
                    return Ok(ticket);
                }
                else
                {
                    // Return a BadRequest response indicating that the ticket cannot be put on hold
                    return BadRequest("Could not hold ticket. This is not an open ticket.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(HoldTicket)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }


        //reopen ticket
        [HttpPut("openTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]
        public async Task<IActionResult> OpenTicket(long ticketId, [FromBody] OpenTicketDto ticket)
        {
            try
            {
                // Retrieve the ticket data from the repository based on the ticketId
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);

                // Check if the ticket exists and has the correct Id
                if (ticketData == null || ticketData.Id != ticketId)
                    return NotFound("Ticket not found in database");

                // Check if the ticket is not cancelled and not resolved
                if (!ticketData.IsCancelled && !ticketData.IsResolved)
                {
                    // Update the ticket properties to mark it as open
                    ticketData.IsOpen = ticket.IsOpen;
                    ticketData.OnHold = false;
                    ticketData.Status = "Open";

                    // Save the changes to the repository
                    await _repository.SaveAsync();

                    // Return an Ok response with the updated OpenTicketDto object
                    return Ok(ticket);
                }
                else
                {
                    // Return a BadRequest response indicating that the ticket could not be reopened
                    return BadRequest("Could not be reopened.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(OpenTicket)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }

        //cancel ticket
        [HttpPut("cancelTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]

        public async Task<IActionResult> CancelTicket(long ticketId, [FromBody] CancelTicketDto ticket)
        {
            try
            {
                // Retrieve the ticket data from the repository based on the ticketId
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);

                // Check if the ticket exists and has the correct Id
                if (ticketData == null || ticketData.Id != ticketId)
                    return NotFound("Ticket not found in database");

                // Check if the ticket is open or on hold and not resolved
                if ((ticketData.IsOpen || ticketData.OnHold) && !ticketData.IsResolved)
                {
                    // Update the ticket properties to mark it as cancelled
                    ticketData.IsOpen = false;
                    ticketData.OnHold = false;
                    ticketData.IsCancelled = ticket.IsCancelled;
                    ticketData.Status = "Cancelled";

                    // Save the changes to the repository
                    await _repository.SaveAsync();

                    // Return an Ok response with the updated CancelTicketDto object
                    return Ok(ticket);
                }
                else
                {
                    // Return a BadRequest response indicating that the ticket could not be cancelled
                    return BadRequest("Could not be cancelled.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(CancelTicket)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }

        //resolve Ticket
        [HttpPut("resolveTicket{ticketId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTicket))]

        public async Task<IActionResult> ResolveTicket(long ticketId, [FromBody] ResolveTicketDto ticket)
        {
            try
            {
                // Retrieve the ticket data from the repository based on the ticketId
                var ticketData = await _repository.Ticket.GetTicket(ticketId, trackChanges: true);

                // Check if the ticket exists and has the correct Id
                if (ticketData == null || ticketData.Id != ticketId)
                    return BadRequest("Ticket not found in database");

                // Check if the ticket is not open
                if (!ticketData.IsOpen)
                {
                    // Return a BadRequest response indicating that the ticket could not be resolved
                    return BadRequest("Could not be resolved. This is not an open ticket");
                }
                else
                {
                    // Update the ticket properties to mark it as resolved
                    ticketData.IsOpen = false;
                    ticketData.IsResolved = ticket.IsResolved;
                    ticketData.Status = "Resolved";

                    // Save the changes to the repository
                    await _repository.SaveAsync();

                    // Return an Ok response with the updated ResolveTicketDto object
                    return Ok(ticket);
                }
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(ResolveTicket)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }


        //delete ticket
        [HttpDelete("{ticketId}")]
        [ServiceFilter(typeof(ValidateTicket))]
        public async Task<IActionResult> DeleteTicket(long ticketId)
        {
            // Retrieve the ticket from the repository based on the ticketId
            var ticket = await _repository.Ticket.GetTicket(ticketId, trackChanges: false);

            // Check if the ticket exists
            if (ticket != null)
            {
                // Delete the ticket from the repository
                await _repository.Ticket.DeleteTicket(ticket);

                // Save the changes to the repository
                await _repository.SaveAsync();

                // Return an Ok response indicating the successful deletion
                return Ok(true);
            }
            else
            {
                // Return a NotFound response if the ticket was not found
                return NotFound();
            }
        }

        //get tickets for current day
        [HttpGet("todayTickets")]
        public async Task<IActionResult> GetTodayTickets()
        {
            try
            {
                // Get the current date and time
                DateTime currentDate = DateTime.Now;

                // Get the start of the current day (midnight)
                DateTime startOfDay = currentDate.Date;

                // Get the end of the current day (one tick before midnight of the next day)
                DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);

                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Filter the tickets to count the ones created within the current day
                var res = tickets.Where(t => t.CreatedDate >= startOfDay && t.CreatedDate <= endOfDay).Count();

                // Return an Ok response with the count of tickets created today
                return Ok(res);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetTodayTickets)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }


        //get tickets for current month
        [HttpGet("thisMonthTickets")]
        public async Task<IActionResult> GetMonthTickets()
        {
            try
            {
                // Get the current date and time
                DateTime currentDate = DateTime.Now;

                // Get the start of the current month
                DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

                // Get the end of the current month (one day before the start of the next month)
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                // Retrieve all tickets from the repository
                var tickets = await _repository.Ticket.GetAllTickets(trackChanges: false);

                // Filter the tickets to count the ones created within the current month
                var res = tickets.Where(t => t.CreatedDate >= startOfMonth && t.CreatedDate <= endOfMonth).Count();

                // Return an Ok response with the count of tickets created in the current month
                return Ok(res);
            }
            catch (Exception ex)
            {
                // Log the exception with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetMonthTickets)} action {ex}");

                // Return a StatusCode 500 (Internal Server Error) response with a generic error message
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
