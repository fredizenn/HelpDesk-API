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
        public AuthController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : base(repository, logger, mapper)
        {

        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistration)
        {
            var userResult = await _repository.UserAuthentication.RegisterUserAsync(userRegistration);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            return !await _repository.UserAuthentication.ValidateUserAsync(user)
                ? Unauthorized("Invalid username or password")
                : Ok(new { Token = await _repository.UserAuthentication.CreateTokenAsync() });
        }
    }
}
