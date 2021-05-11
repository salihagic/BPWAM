using BPWA.Common.Enumerations;

namespace BPWA.DAL.Models
{
    public class AccountTypeSearchModel : BaseSearchModel
    {
        public SystemAccountType? SystemAccountType { get; set; }
    }
}
