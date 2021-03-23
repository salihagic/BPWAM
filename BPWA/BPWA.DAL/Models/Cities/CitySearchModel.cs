using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Models
{
    public class CitySearchModel : BaseSearchModel
    {
        public string Name { get; set; }
        public int? CountryId { get; set; }

        public override bool IsDirty => new List<bool>
        {
            string.IsNullOrEmpty(Name),
            CountryId.HasValue,
        }.Any(x => x);
    }
}
