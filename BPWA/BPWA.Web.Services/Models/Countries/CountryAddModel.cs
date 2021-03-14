using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BPWA.Web.Services.Models
{
    public class CountryAddModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<int> CurrencyIds { get; set; }
        public List<int> LanguageIds { get; set; }

        public List<SelectListItem> CurrencyIdsSelectList { get; set; }        
        public List<SelectListItem> LanguageIdsSelectList { get; set; }
    }
}
