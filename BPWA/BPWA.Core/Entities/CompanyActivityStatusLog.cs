using BPWA.Common.Enumerations;
using System;

namespace BPWA.Core.Entities
{
    public class CompanyActivityStatusLog : 
        BaseSoftDeletableAuditableEntity,
        IBaseEntity
    {
        public int CompanyId { get; set; }
        public ActivityStatus ActivityStatus { get; set; }
        public string Reason { get; set; }
        public DateTime? ActivityStartUtc { get; set; }
        public DateTime? ActivityEndUtc { get; set; }
    }
}
