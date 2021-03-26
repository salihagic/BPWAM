using BPWA.Common.Enumerations;
using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class TicketSearchModel : BaseSearchModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<TicketTypes> TicketTypes { get; set; }
        public List<TicketStatuses> TicketStatuses { get; set; }
    }
}
