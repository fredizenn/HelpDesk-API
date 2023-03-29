using AutoMapper;
using HD_Backend.Data.Entities;
using HD_Backend.Data.Dtos;

namespace HD_Backend.Data.Mappings;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, DepartmentDto>();

        CreateMap<CreateDepartmentDto, Department>();

        CreateMap<UpdateDepartmentDto, Department>().ReverseMap();
    }

}





