using BPWA.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Models
{
    public class CountrySearchModel : BaseSearchModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<int> CurrencyIds { get; set; }
        public List<int> LanguageIds { get; set; }

        public override bool IsDirty => new List<bool>
        {
            string.IsNullOrEmpty(Code),
            string.IsNullOrEmpty(Name),
            CurrencyIds.IsNotEmpty(),
            LanguageIds.IsNotEmpty(),
        }.Any();
    }
}
