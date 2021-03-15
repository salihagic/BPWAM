using BPWA.Common.Enumerations;

namespace BPWA.DAL.Models
{
    public class TicketDTO : BaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketTypes TicketType { get; set; }
        public TicketStatuses TicketStatus { get; set; }
    }
}
