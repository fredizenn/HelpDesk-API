using HD_Backend.Data.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HD_Backend.Data.Interfaces
{
    public interface IUserAuthenticationRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userForRegistration);

        Task<bool> ValidateUserAsync(UserLoginDto loginDto);

        Task<string> CreateTokenAsync();
    }
}
