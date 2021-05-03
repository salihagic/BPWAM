using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class GroupUpdateModel : BaseUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<DropdownItem<string>> UserIdsDropdownItems { get; set; }
        public List<string> UserIds => UserIdsDropdownItems.GetIds();
    }
}
