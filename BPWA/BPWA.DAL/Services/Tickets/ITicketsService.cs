using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ITicketsService : IBaseService<Ticket, TicketSearchModel, TicketDTO>
    {
    }
}
