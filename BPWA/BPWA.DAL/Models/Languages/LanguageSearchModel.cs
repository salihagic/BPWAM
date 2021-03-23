using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Models
{
    public class LanguageSearchModel : BaseSearchModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override bool IsDirty => new List<bool>
        {
            string.IsNullOrEmpty(Code),
            string.IsNullOrEmpty(Name),
        }.Any(x => x);
    }
}
