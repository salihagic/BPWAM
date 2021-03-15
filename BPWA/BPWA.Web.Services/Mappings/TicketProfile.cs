using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<TicketAddModel, Ticket>();
            CreateMap<Ticket, TicketUpdateModel>()
                .ReverseMap();
        }
    }
}
