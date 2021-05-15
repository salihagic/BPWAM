using BPWA.Common.Enumerations;

namespace BPWA.Core.Entities
{
    public class CompanyActivityStatusLog : 
        BaseSoftDeletableAuditableEntity,
        IBaseEntity
    {
        public int CompanyId { get; set; }
        public ActivityStatus ActivityStatus { get; set; }
        public string Reason { get; set; }
    }
}
