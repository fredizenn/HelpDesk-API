using AutoMapper;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Mappings
{
    public class FacultyMappingProfile : Profile
    {
        public FacultyMappingProfile()
        {
            CreateMap<Faculty, FacultyDto>();

            CreateMap<CreateFacultyDto, Faculty>();

            CreateMap<UpdateFacultyDto, Faculty>().ReverseMap();
        }
    }
}
