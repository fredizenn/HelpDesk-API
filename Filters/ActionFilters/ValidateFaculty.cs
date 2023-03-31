using HD_Backend.Data;
using HD_Backend.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HD_Backend.Filters.ActionFilters
{
    public class ValidateFaculty : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateFaculty(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT")!;

            var id = (long)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id") || x.Equals("facultyId")).SingleOrDefault()];

            var faculty = await _repository.Faculty.GetFacultyById(id, trackChanges);
            if (faculty is null)
            {
                _logger.LogInfo($"Faculty with id: {id} doesn't exist in the database.");
                var response = new ObjectResult(new ResponseModel
                {
                    StatusCode = 404,
                    Message = $"Faculty with id: {id} doesn't exist in the database."
                });
                context.Result = response;
            }
            else
            {
                context.HttpContext.Items.Add("faculty", faculty);
                await next();
            }
        }
    }
}
