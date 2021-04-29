using System.Collections.Generic;
using System.Linq;

namespace BPWA.Web.Services.Models
{
    public class CountryAddModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<DropdownItem> CurrencyIdsDropdownItems { get; set; }
        public List<int> CurrencyIds => CurrencyIdsDropdownItems.GetIds();
        public List<DropdownItem> LanguageIdsDropdownItems { get; set; }
        public List<int> LanguageIds => LanguageIdsDropdownItems.GetIds();
    }
}
