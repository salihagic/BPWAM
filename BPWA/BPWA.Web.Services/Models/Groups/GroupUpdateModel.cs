using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class GroupUpdateModel : BaseUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> UserIds { get; set; }
        public List<SelectListItem> UserIdsSelectList { get; set; }
    }
}
