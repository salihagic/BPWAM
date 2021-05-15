using BPWA.Common.Enumerations;

namespace BPWA.DAL.Models
{
    public class CompanyActivityStatusLogDTO :
        BaseSoftDeletableAuditableDTO,
        IBaseDTO
    {
        public ActivityStatus ActivityStatus { get; set; }
        public string Reason { get; set; }
    }
}
