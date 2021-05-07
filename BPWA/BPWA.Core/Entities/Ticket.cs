using BPWA.Common.Attributes;
using BPWA.Common.Enumerations;

namespace BPWA.Core.Entities
{
    public class Ticket : BaseEntity
    {
        [Translatable]
        public string Title { get; set; }
        [Translatable]
        public string Description { get; set; }
        public TicketTypes TicketType { get; set; }
        public TicketStatuses TicketStatus { get; set; }
    }
}
