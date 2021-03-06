using System.Collections.Generic;

namespace BPWA.Web.Services.Models
{
    public class UserAddModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? CityId { get; set; }
        public DropdownItem CityIdDropdownItem { get; set; }
        public List<DropdownItem<string>> RoleIdsDropdownItems { get; set; }
        public List<string> RoleIds => RoleIdsDropdownItems.GetIds();
    }
}
