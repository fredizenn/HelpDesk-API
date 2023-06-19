using AutoMapper;
using HD_Backend.Data;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Interfaces;
using HD_Backend.Filters.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD_Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        public AuthController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, HelpDeskDbContext dbContext) : base(repository, logger, mapper, dbContext)
        {
            // Constructor for the AuthController class

            // Initialize the base class with the provided repository, logger, mapper, and dbContext instances
            // This allows the AuthController to inherit the common functionality and dependencies from the base controller class
        }


        //register agent
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            // Action method for user registration

            // Call the RegisterUserAsync method from the UserAuthentication repository to register the user
            var userResult = await _repository.UserAuthentication.RegisterUserAsync(userRegistration);

            if (!userResult.Succeeded)
            {
                // If user registration is not successful, return a BadRequestObjectResult with the error result
                return new BadRequestObjectResult(userResult);
            }
            else
            {
                // If user registration is successful, return a StatusCode 201 (Created) response
                return StatusCode(201);
            }
        }


        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            // Action method for user authentication

            // Call the ValidateUserAsync method from the UserAuthentication repository to validate the user's credentials
            if (!await _repository.UserAuthentication.ValidateUserAsync(user))
            {
                // If the user credentials are invalid, return an Unauthorized response with an error message
                return Unauthorized("Invalid username or password");
            }
            else
            {
                // If the user credentials are valid, create a token using the CreateTokenAsync method from the UserAuthentication repository
                var token = await _repository.UserAuthentication.CreateTokenAsync();

                // Return an Ok response with a message indicating successful login and the generated token
                return Ok(new { Message = "Logged in successfully", Token = token });
            }
        }



    }
}
