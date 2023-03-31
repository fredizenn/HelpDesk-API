using AutoMapper;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Mappings
{
    public class TicketMappingProfile : Profile
    {
       public  TicketMappingProfile()
        {
            CreateMap<Ticket, TicketDto>()
                .ForMember(t => t.DepartmentCode, opt => opt.MapFrom(t => t.Department.Code))
                .ForMember(t => t.DepartmentName, opt => opt.MapFrom(t => t.Department.Name))
                .ForMember(t => t.FacultyName, opt => opt.MapFrom(t => t.Faculty.Name))
                .ForMember(t => t.FacultyCode, opt => opt.MapFrom(t => t.Faculty.Code));

            CreateMap<CreateTicketDto, Ticket>();

            CreateMap<UpdateTicketDto, Ticket>().ReverseMap();
        }
    }
}
