using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Models
{
    public class CompanySearchModel : BaseSearchModel
    {
        public string Name { get; set; }

        public override bool IsDirty => new List<bool>
        {
            string.IsNullOrEmpty(Name),
        }.Any(x => x);
    }
}
