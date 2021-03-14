using System.Collections.Generic;
using System.Linq;

namespace BPWA.DAL.Models
{
    public class CurrencySearchModel : BaseSearchModel
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public override bool IsDirty => new List<bool>
        {
            string.IsNullOrEmpty(Code),
            string.IsNullOrEmpty(Symbol),
            string.IsNullOrEmpty(Name),
        }.Any();
    }
}
