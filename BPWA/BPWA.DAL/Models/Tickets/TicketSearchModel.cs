using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Models
{
    public class TicketSearchModel : BaseSearchModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<TicketTypes> TicketTypes { get; set; }
        public List<TicketStatuses> TicketStatuses { get; set; }

        public override bool IsDirty => new List<bool>
        {
            string.IsNullOrEmpty(Title),
            string.IsNullOrEmpty(Description),
            TicketTypes.IsNotEmpty(),
            TicketStatuses.IsNotEmpty(),
        }.Any();
    }
}
