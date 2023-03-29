using AutoMapper;
using HD_Backend.Data.Dtos;
using HD_Backend.Data.Entities;

namespace HD_Backend.Data.Mappings
{
    public class TicketMappingProfile : Profile
    {
       public  TicketMappingProfile()
        {
            CreateMap<Ticket, TicketDto>();

            CreateMap<CreateTicketDto, Ticket>();

            CreateMap<UpdateTicketDto, Ticket>().ReverseMap();
        }
    }
}
