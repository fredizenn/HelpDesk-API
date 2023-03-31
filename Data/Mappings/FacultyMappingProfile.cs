using AutoMapper;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Mappings
{
    public class FacultyMappingProfile : Profile
    {
        public FacultyMappingProfile()
        {
            CreateMap<Faculty, FacultyDto>()
                .ForMember(t => t.DepartmentName, opt => opt.MapFrom(t => t.Department.Name));

            CreateMap<CreateFacultyDto, Faculty>();

            CreateMap<UpdateFacultyDto, Faculty>().ReverseMap(); 
        }
    } 
}
 