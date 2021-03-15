using BPWA.Common.Enumerations;

namespace BPWA.Core.Entities
{
    public class Ticket : BaseEntity, IBaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketTypes TicketType { get; set; }
        public TicketStatuses TicketStatus { get; set; }
    }
}
