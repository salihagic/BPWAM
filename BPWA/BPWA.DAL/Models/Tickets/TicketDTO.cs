using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;

namespace BPWA.DAL.Models
{
    public class TicketDTO : BaseDTO, IBaseDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketTypes TicketType { get; set; }
        public string TicketTypeString => TranslationsHelper.Translate(TicketType.ToString());
        public TicketStatuses TicketStatus { get; set; }
        public string TicketStatusString => TranslationsHelper.Translate(TicketStatus.ToString());
    }
}
