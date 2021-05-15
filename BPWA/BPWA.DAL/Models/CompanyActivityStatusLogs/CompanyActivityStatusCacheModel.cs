using BPWA.Common.Enumerations;

namespace BPWA.DAL.Models
{
    public class CompanyActivityStatusCacheModel
    {
        public int CompanyId { get; set; }
        public ActivityStatus ActivityStatus { get; set; }
        public string Reason { get; set; }

        public string CacheKey => CompanyId.ToString();
    }
}
