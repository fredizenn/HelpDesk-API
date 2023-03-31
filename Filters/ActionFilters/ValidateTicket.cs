using HD_Backend.Data;
using HD_Backend.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HD_Backend.Filters.ActionFilters
{
    public class ValidateTicket : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateTicket(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT")!;

            var id = (long)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id") || x.Equals("ticketId")).SingleOrDefault()];

            var ticket = await _repository.Ticket.GetTicket(id, trackChanges);
            if (ticket is null)
            {
                _logger.LogInfo($"Ticket with id: {id} doesn't exist in the database.");
                var response = new ObjectResult(new ResponseModel
                {
                    StatusCode = 404,
                    Message = $"Ticket with id: {id} doesn't exist in the database."
                });
                context.Result = response;
            }
            else
            {
                context.HttpContext.Items.Add("ticket", ticket);
                await next();
            }
        }
    }
}
