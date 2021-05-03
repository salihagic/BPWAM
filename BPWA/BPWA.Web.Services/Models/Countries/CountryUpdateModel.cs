using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class CountryUpdateModel : BaseUpdateModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<DropdownItem> CurrencyIdsDropdownItems { get; set; }
        public List<int> CurrencyIds => CurrencyIdsDropdownItems.GetIds();
        public List<DropdownItem> LanguageIdsDropdownItems { get; set; }
        public List<int> LanguageIds => LanguageIdsDropdownItems.GetIds();
    }
}
