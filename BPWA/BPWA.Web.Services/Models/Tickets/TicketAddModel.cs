using BPWA.Common.Enumerations;

namespace BPWA.Web.Services.Models
{
    public class TicketAddModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketTypes? TicketType { get; set; }
        public TicketStatuses? TicketStatus { get; set; }
    }
}
