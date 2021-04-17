using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class CompanyUserAddModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? CityId { get; set; }
        public List<string> RoleIds { get; set; }
        public List<int> BusinessUnitIds { get; set; }

        public string SelectedCity { get; set; }
        public List<SelectListItem> RoleIdsSelectList { get; set; }
        public List<SelectListItem> BusinessUnitIdsSelectList { get; set; }
    }
}
