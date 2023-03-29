using AutoMapper;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
