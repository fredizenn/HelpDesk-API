using AutoMapper;
using HD_Backend.Data;
using HD_Backend.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        public AccountsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, HelpDeskDbContext dbContext) : base(repository, logger, mapper, dbContext)
        {

        }

        //get all agents (agents are users)
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                // Action method to get all users

                // Retrieve all users from the database
                var users = _dbContext.Users
                            .Where(x => x.Id != null)
                            .Include(d => d.Faculty) // Include the related Faculty entity
                            .Include(d => d.Department) // Include the related Department entity
                            .ToList();
                // Return an Ok response with the list of users
                return Ok(users);
            }
            catch (Exception ex)
            {
                // If an exception occurs, log the error and return a StatusCode 500 response with an error message
                _logger.LogError($"Something went wrong in the {nameof(GetUsers)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
