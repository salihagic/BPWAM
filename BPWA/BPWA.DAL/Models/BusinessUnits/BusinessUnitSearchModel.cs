using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Models
{
    public class BusinessUnitSearchModel : BaseSearchModel
    {
        public string Name { get; set; }
        public int? CompanyId { get; set; }

        public override bool IsDirty => new List<bool>
        {
            string.IsNullOrEmpty(Name),
            CompanyId.HasValue
        }.Any(x => x);
    }
}
