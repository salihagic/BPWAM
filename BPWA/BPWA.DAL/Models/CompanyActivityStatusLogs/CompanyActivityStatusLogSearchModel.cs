using BPWA.Common.Enumerations;

namespace BPWA.DAL.Models
{
    public class CompanyActivityStatusLogSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Name { get; set; }
        public AccountType? AccountType { get; set; }
    }
}
