using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class CountryUpdateModel : BaseUpdateModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<int> CurrencyIds { get; set; }
        public List<SelectListItem> CurrencyIdsSelectList { get; set; }
        public List<int> LanguageIds { get; set; }
        public List<SelectListItem> LanguageIdsSelectList { get; set; }
    }
}
