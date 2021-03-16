using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface ITicketsService : IBaseCRUDService<Ticket, TicketSearchModel, TicketDTO>
    {
    }
}
