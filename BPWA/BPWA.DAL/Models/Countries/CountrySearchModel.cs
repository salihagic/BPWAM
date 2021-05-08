using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class CountrySearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<int> CurrencyIds { get; set; }
        public List<int> LanguageIds { get; set; }
    }
}
